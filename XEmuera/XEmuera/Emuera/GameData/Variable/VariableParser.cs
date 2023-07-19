﻿using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameData.Expression;
using trerror = EvilMask.Emuera.Lang.Error;

namespace MinorShift.Emuera.GameData.Variable
{
	internal static class VariableParser
	{
		public static void Initialize()
		{
			ZeroTerm = new SingleTerm(0);
			IOperandTerm[] zeroArgs = new IOperandTerm[] { ZeroTerm };
			TARGET = new VariableTerm(GlobalStatic.VariableData.GetSystemVariableToken("TARGET"), zeroArgs);
		}

		public static SingleTerm ZeroTerm { get; private set; }
		public static VariableTerm TARGET { get; private set; }

		public static bool IsVariable(string ids)
		{
			if (string.IsNullOrEmpty(ids))
				return false;
			string[] idlist = ids.Split(':');
			//idlist = synonym.ApplySynonym(idlist);
			VariableToken id = GlobalStatic.IdentifierDictionary.GetVariableToken(idlist[0], null, false);
			return id != null;
		}

		///// <summary>
		///// まだ最初の識別子を読んでいない状態から決め打ちで変数を解読する
		///// </summary>
		///// <param name="st"></param>
		///// <returns></returns>
		//public static VariableTerm ReduceVariable(WordCollection wc)
		//{
		//    IdentifierWord id = wc.Current as IdentifierWord;
		//    if (id == null)
		//        return null;
		//    wc.ShiftNext();
		//    VariableToken vid = ExpressionParser.ReduceVariableIdentifier(wc, id.Code);
		//    if (vid == null)
		//        throw new CodeEE("\"" + id.Code + "\"は解釈できない識別子です");
		//    return ReduceVariable(vid, wc);
		//}

		/// <summary>
		/// 識別子を読み終えた状態からの解析
		/// </summary>
		/// <param name="st"></param>
		/// <returns></returns>
		public static VariableTerm ReduceVariable(VariableToken id, WordCollection wc)
		{
			IOperandTerm operand;
			IOperandTerm op1 = null;
			IOperandTerm op2 = null;
			IOperandTerm op3 = null;
			int i = 0;
			while (true)
			{
				if (wc.Current.Type != ':')
					break;
				if (i >= 3)
					throw new CodeEE(string.Format(trerror.TooMany2DCharaVarArg.Text, id.Code.ToString()));
				wc.ShiftNext();
				#region EE_ERD
				// operand = ExpressionParser.ReduceVariableArgument(wc, id.Code);
				operand = ExpressionParser.ReduceVariableArgument(wc, id.Code, id);
				#endregion
				if (i == 0)
					op1 = operand;
				else if (i == 1)
					op2 = operand;
				else if (i == 2)
					op3 = operand;
				i++;
			}
			return ReduceVariable(id, op1, op2, op3);

		}



		public static VariableTerm ReduceVariable(VariableToken id, IOperandTerm p1, IOperandTerm p2, IOperandTerm p3)
		{
			IOperandTerm[] terms;
			IOperandTerm op1 = p1;
			IOperandTerm op2 = p2;
			IOperandTerm op3 = p3;
			//引数の推測
			if (id.IsCharacterData)
			{
				if (id.IsArray2D)
				{
					if ((op1 == null) && (op2 == null) && (op3 == null))
						return new VariableNoArgTerm(id);
					if ((op1 == null) || (op2 == null) || (op3 == null))
						throw new CodeEE(string.Format(trerror.CanNotOmit1DCharaVarArg1.Text, id.Name));
					terms = new IOperandTerm[3];
					terms[0] = op1;
					terms[1] = op2;
					terms[2] = op3;
				}
				else if (id.IsArray1D)
				{
					if (op3 != null)
						throw new CodeEE(string.Format(trerror.TooMany1DCharaVarArg.Text, id.Name));
					if ((op1 == null) && (op2 == null) && (op3 == null) && Config.SystemNoTarget)
						return new VariableNoArgTerm(id);
					if (op2 == null)
					{
						if (Config.SystemNoTarget)
							throw new CodeEE(string.Format(trerror.CanNotOmit1DCharaVarArg2.Text, id.Name));
						if (op1 == null)
							op2 = ZeroTerm;
						else
							op2 = op1;
						op1 = TARGET;
					}
					terms = new IOperandTerm[2];
					terms[0] = op1;
					terms[1] = op2;
				}
				else
				{
					if (op2 != null)
						throw new CodeEE(string.Format(trerror.TooManyCharaVarArg.Text, id.Name));
					if ((op1 == null) && (op2 == null) && (op3 == null) && Config.SystemNoTarget)
						return new VariableNoArgTerm(id);
					if (op1 == null)
					{
						if (Config.SystemNoTarget)
							throw new CodeEE(string.Format(trerror.CanNotOmitCharaVarArg2.Text, id.Name));
						op1 = TARGET;
					}
					terms = new IOperandTerm[1];
					terms[0] = op1;
				}
			}
			else if (id.IsArray3D)
			{
				if ((op1 == null) && (op2 == null) && (op3 == null))
					return new VariableNoArgTerm(id);
				if ((op1 == null) || (op2 == null) || (op3 == null))
					throw new CodeEE(string.Format(trerror.CanNotOmit3DVarArg.Text, id.Name));
				terms = new IOperandTerm[3];
				terms[0] = op1;
				terms[1] = op2;
				terms[2] = op3;
			}
			else if (id.IsArray2D)
			{
				if ((op1 == null) && (op2 == null) && (op3 == null))
					return new VariableNoArgTerm(id);
				if ((op1 == null) || (op2 == null))
					throw new CodeEE(string.Format(trerror.CanNotOmit2DVarArg.Text, id.Name));
				if (op3 != null)
					throw new CodeEE(string.Format(trerror.TooMany2DVarArg.Text, id.Name));
				terms = new IOperandTerm[2];
				terms[0] = op1;
				terms[1] = op2;
			}
			else if (id.IsArray1D)
			{
				if (op2 != null)
					throw new CodeEE(string.Format(trerror.TooMany1DVarArg.Text, id.Name));
                if (op1 == null)
                {
                    op1 = ZeroTerm;
                    if (!Config.CompatiRAND && id.Code == VariableCode.RAND)
                    {
                        throw new CodeEE(trerror.OmittedRandArg.Text);
                    }
                }
                if (!Config.CompatiRAND && op1 is SingleTerm && id.Code == VariableCode.RAND)
                {
                    if (((SingleTerm)op1).Int == 0)
                        throw new CodeEE(trerror.RandArgIsZero.Text);
                }
				terms = new IOperandTerm[1];
				terms[0] = op1;
			}
			else if (op1 != null)
			{
				throw new CodeEE(string.Format(trerror.ZeroDVarHasArg.Text, id.Name));
			}
			else
				terms = new IOperandTerm[0];
			for (int i = 0; i < terms.Length; i++)
				if (terms[i].IsString)
					#region EE_ERD
					// terms[i] = new VariableStrArgTerm(id.Code, terms[i], i);
					terms[i] = new VariableStrArgTerm(id.Code, terms[i], i, id.Name);
					#endregion

			return new VariableTerm(id, terms);
		}
		//public static string ErrorMes = null;
		//public static void ResetError()
		//{
		//    ErrorMes = null;
		//}

	}
}
