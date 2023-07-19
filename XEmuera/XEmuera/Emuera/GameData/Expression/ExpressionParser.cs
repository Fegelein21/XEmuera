﻿using System;
using System.Collections.Generic;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.GameData.Function;
using XEmuera.Forms;
using trerror = EvilMask.Emuera.Lang.Error;

namespace MinorShift.Emuera.GameData.Expression
{

	internal enum ArgsEndWith
	{
		None,
		EoL,
		RightParenthesis,//)終端
		RightBracket,//]終端
	}

	internal enum TermEndWith
	{
		None = 0x0000,
		EoL = 0x0001,
		Comma = 0x0002,//','終端
		RightParenthesis = 0x0004,//')'終端
		RightBracket = 0x0008,//')'終端
		Assignment = 0x0010,//')'終端


		#region EM_私家版_HTMLパラメータ拡張
		KeyWordPx = 0x0020,//'px'終端
		#endregion

		RightParenthesis_Comma = RightParenthesis | Comma,//',' or ')'終端
		RightBracket_Comma = RightBracket | Comma,//',' or ']'終端
		Comma_Assignment = Comma | Assignment,//',' or '='終端
		RightParenthesis_Comma_Assignment = RightParenthesis | Comma | Assignment,//',' or ')' or '='終端
		RightBracket_Comma_Assignment = RightBracket | Comma | Assignment,//',' or ']' or '='終端
	}

    internal static class ExpressionParser
	{
		#region public Reduce
		/// <summary>
		/// カンマで区切られた引数を一括して取得。
		/// return時にはendWithの次の文字がCurrentになっているはず。終端の適切さの検証はExpressionParserがが行う。
		/// 呼び出し元はCodeEEを適切に処理すること
		/// </summary>
		/// <returns></returns>
		public static IOperandTerm[] ReduceArguments(WordCollection wc, ArgsEndWith endWith, bool isDefine)
		{
			if(wc == null)
				throw new ExeEE(trerror.EmptyStream.Text);
			List<IOperandTerm> terms = new List<IOperandTerm>();
			TermEndWith termEndWith = TermEndWith.EoL;
			switch (endWith)
			{
				case ArgsEndWith.EoL:
					termEndWith = TermEndWith.Comma;
					break;
                //case ArgsEndWith.RightBracket:
                //    termEndWith = TermEndWith.RightBracket_Comma;
                //    break;
				case ArgsEndWith.RightParenthesis:
					termEndWith = TermEndWith.RightParenthesis_Comma;
					break;
			}
			TermEndWith termEndWith_Assignment = termEndWith | TermEndWith.Assignment;
			while (true)
			{
				Word word = wc.Current;
				switch (word.Type)
				{
					case '\0':
                        if (endWith == ArgsEndWith.RightBracket)
                            throw new CodeEE(trerror.NotCloseSBrackets.Text);
						if (endWith == ArgsEndWith.RightParenthesis)
							throw new CodeEE(trerror.NotCloseBrackets.Text);
						goto end;
					case ')':
						if (endWith == ArgsEndWith.RightParenthesis)
						{
							wc.ShiftNext();
							goto end;
						}
						throw new CodeEE(trerror.UnexpectedBrackets.Text);
                    case ']':
                        if (endWith == ArgsEndWith.RightBracket)
                        {
                            wc.ShiftNext();
                            goto end;
                        }
                        throw new CodeEE(trerror.UnexpectedSBrackets.Text);
				}
				if(!isDefine)
					terms.Add(ReduceExpressionTerm(wc, termEndWith));
				else
				{
					terms.Add(ReduceExpressionTerm(wc, termEndWith_Assignment));
                    if (terms[terms.Count - 1] == null)
                        throw new CodeEE(trerror.CannotOmitFuncArg.Text);
					if (wc.Current is OperatorWord)
					{//=がある
						wc.ShiftNext();
						IOperandTerm term = reduceTerm(wc, false, termEndWith, VariableCode.__NULL__);
						if (term == null)
							throw new CodeEE(trerror.NoExpressionAfterEqual.Text);
						if (term.GetOperandType() != terms[terms.Count - 1].GetOperandType())
							throw new CodeEE(trerror.DoesNotMatchEqual.Text);
						terms.Add(term);
					}
					else
					{
						if (terms[terms.Count - 1].GetOperandType() == typeof(Int64))
							terms.Add(new NullTerm(0));
						else
							terms.Add(new NullTerm(""));
					}
				}
				if (wc.Current.Type == ',')
					wc.ShiftNext();
			}
		end:
            IOperandTerm[] ret = new IOperandTerm[terms.Count];
			terms.CopyTo(ret);
			return ret;
		}


		/// <summary>
		/// 数式または文字列式。CALLの引数などを扱う。nullを返すことがある。
		/// return時にはendWithの文字がCurrentになっているはず。終端の適切さの検証は呼び出し元が行う。
		/// </summary>
		/// <param name="st"></param>
		/// <returns></returns>
        public static IOperandTerm ReduceExpressionTerm(WordCollection wc, TermEndWith endWith)
        {
			IOperandTerm term = reduceTerm(wc, false, endWith, VariableCode.__NULL__);
            return term;
        }


		///// <summary>
		///// 単純文字列、書式付文字列、文字列式のうち、文字列式を取り扱う。
		///// 終端記号が正しいかどうかは呼び出し元で調べること
		///// </summary>
		///// <param name="st"></param>
		///// <returns></returns>
		//public static IOperandTerm ReduceStringTerm(WordCollection wc, TermEndWith endWith)
		//{
		//    IOperandTerm term = reduceTerm(wc, false, endWith, VariableCode.__NULL__);
		//    if (term.GetOperandType() != typeof(string))
		//        throw new CodeEE("式の結果が文字列ではありません");
		//    return term;
		//}

		public static IOperandTerm ReduceIntegerTerm(WordCollection wc, TermEndWith endwith)
		{
			IOperandTerm term = reduceTerm(wc, false, endwith, VariableCode.__NULL__);
            if (term == null)
                throw new CodeEE(trerror.CanNotInterpretedExpression.Text);
			if (term.GetOperandType() != typeof(Int64))
				throw new CodeEE(trerror.ExpressionResultIsNotNumeric.Text);
			return term;
		}

		
        /// <summary>
        /// 結果次第ではSingleTermを返すことがある。
        /// </summary>
        /// <returns></returns>
		public static IOperandTerm ToStrFormTerm(StrFormWord sfw)
		{
			StrForm strf = StrForm.FromWordToken(sfw);
			if(strf.IsConst)
				return new SingleTerm(strf.GetString(null));
			return new StrFormTerm(strf);
		}

		/// <summary>
		/// カンマで区切られたCASEの引数を一括して取得。行端で終わる。
		/// </summary>
		/// <param name="st"></param>
		/// <returns></returns>
		public static CaseExpression[] ReduceCaseExpressions(WordCollection wc)
		{
			List<CaseExpression> terms = new List<CaseExpression>();
			while (!wc.EOL)
			{
				terms.Add(reduceCaseExpression(wc));
				wc.ShiftNext();
			}
			CaseExpression[] ret = new CaseExpression[terms.Count];
			terms.CopyTo(ret);
			return ret;
		}
		#region EE_ERD
		// public static IOperandTerm ReduceVariableArgument(WordCollection wc, VariableCode varCode)
		public static IOperandTerm ReduceVariableArgument(WordCollection wc, VariableCode varCode, VariableToken id)
		{
			// IOperandTerm ret = reduceTerm(wc, false, TermEndWith.EoL, varCode);
			IOperandTerm ret = reduceTerm(wc, false, TermEndWith.EoL, varCode, id);
			if (ret == null)
                throw new CodeEE(trerror.MissingArgAfterColon.Text);
			return ret;
		}
		#endregion

		public static VariableToken ReduceVariableIdentifier(WordCollection wc, string idStr)
		{
			string subId = null;
			if (wc.Current.Type == '@')
			{
				wc.ShiftNext();
				IdentifierWord subidWT = wc.Current as IdentifierWord;
				if (subidWT == null)
					throw new CodeEE(trerror.InvalidAt.Text);
				wc.ShiftNext();
				subId = subidWT.Code;
			}
			return GlobalStatic.IdentifierDictionary.GetVariableToken(idStr, subId, true);
		}


		/// <summary>
		/// 識別子一つを解決
		/// </summary>
		/// <param name="wc"></param>
		/// <param name="idStr">識別子文字列</param>
		/// <param name="varCode">変数の引数の場合はその変数のCode。連想配列的につかう</param>
		/// <returns></returns>
		#region EE_ERD
		//private static IOperandTerm reduceIdentifier(WordCollection wc, string idStr, VariableCode varCode)
		private static IOperandTerm reduceIdentifier(WordCollection wc, string idStr, VariableCode varCode, VariableToken varId = null)
		#endregion

		{
			wc.ShiftNext();
			SymbolWord symbol = wc.Current as SymbolWord;
			if (symbol != null && symbol.Type == '.')
			{//名前空間
				throw new NotImplCodeEE();
			}
			else if (symbol != null && (symbol.Type == '(' || symbol.Type == '['))
			{//関数
				wc.ShiftNext();
				if (symbol.Type == '[')//1810 多分永久に実装されない
					throw new CodeEE(trerror.SBracketsFuncNotImprement.Text);
				//引数を処理
				IOperandTerm[] args = ReduceArguments(wc, ArgsEndWith.RightParenthesis, false);
				IOperandTerm mToken = GlobalStatic.IdentifierDictionary.GetFunctionMethod(GlobalStatic.LabelDictionary, idStr, args, false);
				if (mToken == null)
				{
					if (!Program.AnalysisMode)
						GlobalStatic.IdentifierDictionary.ThrowException(idStr, true);
					else
					{
						if (GlobalStatic.tempDic.ContainsKey(idStr))
							GlobalStatic.tempDic[idStr]++;
						else
							GlobalStatic.tempDic.Add(idStr, 1);
						return new NullTerm(0);
					}
				}
				return mToken;
			}
			else
			{//変数 or キーワード
				VariableToken id = ReduceVariableIdentifier(wc, idStr);
				if (id != null)//idStrが変数名の場合、
				{
					if (varCode != VariableCode.__NULL__)//変数の引数が引数を持つことはない
						return VariableParser.ReduceVariable(id, null, null, null);
					else
						return VariableParser.ReduceVariable(id, wc);
				}
				//idStrが変数名でない場合、
				IOperandTerm refToken = GlobalStatic.IdentifierDictionary.GetFunctionMethod(GlobalStatic.LabelDictionary, idStr, null, false);
				if (refToken != null)//関数参照と名前が一致したらそれを返す。実際に使うとエラー
					return refToken;
				if (varCode != VariableCode.__NULL__ && GlobalStatic.ConstantData.isDefined(varCode, idStr))//連想配列的な可能性アリ
					return new SingleTerm(idStr);
				#region EE_ERD
				else if (varId != null)
                {
					switch (varId.Code)
                    {
						case VariableCode.VAR:
						case VariableCode.VARS:
						case VariableCode.CVAR:
						case VariableCode.CVARS:
							if (GlobalStatic.ConstantData.isUserDefined(varId.Name, idStr, 1))//ユーザー定義変数は名前付けられるようになったので通す
								return new SingleTerm(idStr);
							break;
						case VariableCode.VAR2D:
						case VariableCode.VARS2D:
						case VariableCode.CVAR2D:
						case VariableCode.CVARS2D:
							if (GlobalStatic.ConstantData.isUserDefined(varId.Name, idStr, 2))//ユーザー定義変数は名前付けられるようになったので通す
								return new SingleTerm(idStr);
							break;
						case VariableCode.VAR3D:
						case VariableCode.VARS3D:
							if (GlobalStatic.ConstantData.isUserDefined(varId.Name, idStr, 3))//ユーザー定義変数は名前付けられるようになったので通す
								return new SingleTerm(idStr);
							break;
					}
				}
				#endregion

				GlobalStatic.IdentifierDictionary.ThrowException(idStr, false);
			}
			throw new ExeEE(trerror.ThrowFailed.Text);//ここまででthrowかreturnのどちらかをするはず。
		}

		#endregion

		#region private reduce
		private static CaseExpression reduceCaseExpression(WordCollection wc)
		{
			CaseExpression ret = new CaseExpression();
			IdentifierWord id = wc.Current as IdentifierWord;
			if ((id != null) && (id.Code.Equals("IS", Config.SCVariable)))
			{
				wc.ShiftNext();
				ret.CaseType = CaseExpressionType.Is;
				OperatorWord opWT = wc.Current as OperatorWord;
				if (opWT == null)
					throw new CodeEE(trerror.NoOpAfterIs.Text);

				OperatorCode op = opWT.Code;
				if (!OperatorManager.IsBinary(op))
					throw new CodeEE(trerror.NotBinaryOpAfterThis.Text);
				wc.ShiftNext();
				ret.Operator = op;
				ret.LeftTerm = reduceTerm(wc, false, TermEndWith.Comma, VariableCode.__NULL__);
				if (ret.LeftTerm == null)
					throw new CodeEE(trerror.NothingAfterIs.Text);
				//Type type = ret.LeftTerm.GetOperandType();
				return ret;
			}
			ret.LeftTerm = reduceTerm(wc, true, TermEndWith.Comma, VariableCode.__NULL__);
			if (ret.LeftTerm == null)
				throw new CodeEE(trerror.CanNotOmitCaseArg.Text);
			id = wc.Current as IdentifierWord;
			if ((id != null) && (id.Code.Equals("TO", Config.SCVariable)))
			{
				ret.CaseType = CaseExpressionType.To;
				wc.ShiftNext();
				ret.RightTerm = reduceTerm(wc, true, TermEndWith.Comma, VariableCode.__NULL__);
				if (ret.RightTerm == null)
					throw new CodeEE(trerror.NoExpressionAfterTo.Text);
				id = wc.Current as IdentifierWord;
				if ((id != null) && (id.Code.Equals("TO", Config.SCVariable)))
					throw new CodeEE(trerror.DuplicateTo.Text);
				if (ret.LeftTerm.GetOperandType() != ret.RightTerm.GetOperandType())
					throw new CodeEE(trerror.DoesNotMatchTo.Text);
				return ret;
			}
			ret.CaseType = CaseExpressionType.Normal;
			return ret;
		}


		/// <summary>
		/// 解析器の本体
		/// </summary>
		/// <param name="wc"></param>
		/// <param name="allowKeywordTo">TOキーワードが見つかっても良いか</param>
		/// <param name="endWith">終端記号</param>
		/// <returns></returns>

		#region EE_ERD
		// private static IOperandTerm reduceTerm(WordCollection wc, bool allowKeywordTo, TermEndWith endWith, VariableCode varCode)
		private static IOperandTerm reduceTerm(WordCollection wc, bool allowKeywordTo, TermEndWith endWith, VariableCode varCode, VariableToken varId = null)
		#endregion
		{
			TermStack stack = new TermStack();
            //int termCount = 0;
            int ternaryCount = 0;
            OperatorCode formerOp = OperatorCode.NULL;
			bool varArg = varCode != VariableCode.__NULL__;
			do
			{
				Word token = wc.Current;
				switch (token.Type)
				{
					case '\0':
						goto end;
					case '"'://LiteralStringWT
						stack.Add(((LiteralStringWord)token).Str);
						break;
					case '0'://LiteralIntegerWT
						stack.Add(((LiteralIntegerWord)token).Int);
						break;
					case 'F'://FormattedStringWT
						stack.Add(ToStrFormTerm((StrFormWord)token));
						break;
					case 'A'://IdentifierWT
						{
							string idStr = (((IdentifierWord)token).Code);
							if (idStr.Equals("TO", Config.SCVariable))
							{
								if (allowKeywordTo)
									goto end;
								else
									throw new CodeEE(trerror.InvalidTo.Text);
							}
							else if (idStr.Equals("IS", Config.SCVariable))
								throw new CodeEE(trerror.InvalidIs.Text);

							#region EM_私家版_HTMLパラメータ拡張
							if ((endWith & TermEndWith.KeyWordPx) == TermEndWith.KeyWordPx && idStr.Equals("px", StringComparison.OrdinalIgnoreCase) && (wc.Next.Type == ',' || wc.Next.Type == '\0'))
							{
								goto end;
							}
							#endregion
							#region EE_ERD
							// stack.Add(reduceIdentifier(wc, idStr, varCode));
							stack.Add(reduceIdentifier(wc, idStr, varCode, varId));
							#endregion
							continue;
						}

					case '='://OperatorWT
						{
							if (varArg)
								throw new CodeEE(trerror.UnexpectedOpInVarArg.Text);
							OperatorCode op = ((OperatorWord)token).Code;
							if (op == OperatorCode.Assignment)
							{
								if ((endWith & TermEndWith.Assignment) == TermEndWith.Assignment)
									goto end;
								throw new CodeEE(trerror.EqualInExpression.Text);
							}

							if (formerOp == OperatorCode.Equal || formerOp == OperatorCode.Greater || formerOp == OperatorCode.Less
								|| formerOp == OperatorCode.GreaterEqual || formerOp == OperatorCode.LessEqual || formerOp == OperatorCode.NotEqual)
							{
								if (op == OperatorCode.Equal || op == OperatorCode.Greater || op == OperatorCode.Less
								|| op == OperatorCode.GreaterEqual || op == OperatorCode.LessEqual || op == OperatorCode.NotEqual)
								{
									ParserMediator.Warn(trerror.ComparisonOpContinuous.Text, GlobalStatic.Process.GetScaningLine(), 0, false, false);
								}
							}
							stack.Add(op);
							formerOp = op;
							if (op == OperatorCode.Ternary_a)
								ternaryCount++;
							else if (op == OperatorCode.Ternary_b)
							{
								if (ternaryCount > 0)
									ternaryCount--;
								else
									throw new CodeEE(trerror.MissingQuestion.Text);
							}
							break;
						}
					case '(':
						wc.ShiftNext();
                        IOperandTerm inTerm = reduceTerm(wc, false, TermEndWith.RightParenthesis, VariableCode.__NULL__);
                        if (inTerm == null)
                            throw new CodeEE(trerror.NoContainExpressionInBrackets.Text);
						stack.Add(inTerm);
						if (wc.Current.Type != ')')
							throw new CodeEE(trerror.NotCloseBrackets.Text);
						//termCount++;
						wc.ShiftNext();
						continue;
					case ')':
						if ((endWith & TermEndWith.RightParenthesis) == TermEndWith.RightParenthesis)
							goto end;
						throw new CodeEE(string.Format(trerror.UnexpectedSymbol.Text, token.Type));
					case ']':
						if ((endWith & TermEndWith.RightBracket) == TermEndWith.RightBracket)
							goto end;
						throw new CodeEE(string.Format(trerror.UnexpectedSymbol.Text, token.Type));
					case ',':
						if ((endWith & TermEndWith.Comma) == TermEndWith.Comma)
							goto end;
						throw new CodeEE(string.Format(trerror.UnexpectedSymbol.Text, token.Type));
					case 'M':
						throw new ExeEE(trerror.FailedSolveMacro.Text);
					default:
						throw new CodeEE(string.Format(trerror.UnexpectedSymbol.Text, token.Type));
				}
				//termCount++;
				wc.ShiftNext();
			} while (!varArg);
		end:
            if (ternaryCount > 0)
                throw new CodeEE(trerror.TernaryBinaryError.Text);
            return stack.ReduceAll();
        }
        
		#endregion

		/// <summary>
        /// 式解決用クラス
        /// </summary>
        private class TermStack
        {
            /// <summary>
            /// 次に来るべきものの種類。
            /// (前置)単項演算子か値待ちなら0、二項・三項演算子待ちなら1、値待ちなら2、++、--、!に対応する値待ちの場合は3。
            /// </summary>
            int state = 0;
            bool hasBefore = false;
            bool hasAfter = false;
            bool waitAfter = false;
            Stack<Object> stack = new Stack<Object>();
            public void Add(OperatorCode op)
            {
                if (state == 2 || state == 3)
                    throw new CodeEE(trerror.UnrecognizedSyntax.Text);
                if (state == 0)
                {
                    if (!OperatorManager.IsUnary(op))
						throw new CodeEE(trerror.UnrecognizedSyntax.Text);
					stack.Push(op);
                    if (op == OperatorCode.Plus || op == OperatorCode.Minus || op == OperatorCode.BitNot)
                        state = 2;
                    else
                        state = 3;
                    return;
                }
                if (state == 1)
                {
                    //後置単項演算子の場合は特殊処理へ
                    if (OperatorManager.IsUnaryAfter(op))
                    {
                        if (hasAfter)
                        {
                            hasAfter = false;
							throw new CodeEE(trerror.MultipleUnaryOp.Text);
                        }
                        if (hasBefore)
                        {
                            hasBefore = false;
                            throw new CodeEE(trerror.DuplicateIncrementDecrement.Text);
                        }
                        stack.Push(op);
                        reduceUnaryAfter();
                        //前置単項演算子が処理を待っている場合はここで解決
                        if (waitAfter)
                            reduceUnary();
                        hasBefore = false;
                        hasAfter = true;
                        waitAfter = false;
                        return;
                    }
                    if (!OperatorManager.IsBinary(op) && !OperatorManager.IsTernary(op))
						throw new CodeEE(trerror.UnrecognizedSyntax.Text);
					//先に未解決の前置演算子解決
					if (waitAfter)
                        reduceUnary();
                    int priority = OperatorManager.GetPriority(op);
                    //直前の計算の優先度が同じか高いなら還元。
                    while (lastPriority() >= priority)
                    {
                        this.reduceLastThree();
                    }
                    stack.Push(op);
                    state = 0;
                    waitAfter = false;
                    hasBefore = false;
                    hasAfter = false;
                    return;
                }
				throw new CodeEE(trerror.UnrecognizedSyntax.Text);
			}
			public void Add(Int64 i) { Add(new SingleTerm(i)); }
            public void Add(string s) { Add(new SingleTerm(s)); }
            public void Add(IOperandTerm term)
            {
                stack.Push(term);
                if (state == 1)
					throw new CodeEE(trerror.UnrecognizedSyntax.Text);
				if (state == 2)
                    waitAfter = true;
                if (state == 3)
                {
                    reduceUnary();
                    hasBefore = true;
                }
                state = 1;
                return;
            }


            private int lastPriority()
            {
                if (stack.Count < 3)
                    return -1;
                object temp = (object)stack.Pop();
                OperatorCode opCode = (OperatorCode)stack.Peek();
                int priority = OperatorManager.GetPriority(opCode);
                stack.Push(temp);
                return priority;
            }

            public IOperandTerm ReduceAll()
            {
                if (stack.Count == 0)
                    return null;
                if (state != 1)
					throw new CodeEE(trerror.UnrecognizedSyntax.Text);
				//単項演算子の待ちが未解決の時はここで解決
				if (waitAfter)
                    reduceUnary();
                waitAfter = false;
                hasBefore = false;
                hasAfter = false;
                while (stack.Count > 1)
                {
                    reduceLastThree();
                }
                IOperandTerm retTerm = (IOperandTerm)stack.Pop();
                return retTerm;
            }

            private void reduceUnary()
            {
                //if (stack.Count < 2)
                //    throw new ExeEE("不正な時期の呼び出し");
                IOperandTerm operand = (IOperandTerm)stack.Pop();
                OperatorCode op = (OperatorCode)stack.Pop();
                IOperandTerm newTerm = OperatorMethodManager.ReduceUnaryTerm(op, operand);
                stack.Push(newTerm);
            }

            private void reduceUnaryAfter()
            {
                //if (stack.Count < 2)
                //    throw new ExeEE("不正な時期の呼び出し");
                OperatorCode op = (OperatorCode)stack.Pop();
                IOperandTerm operand = (IOperandTerm)stack.Pop();
                
                IOperandTerm newTerm = OperatorMethodManager.ReduceUnaryAfterTerm(op, operand);
                stack.Push(newTerm);
				
            }
            private void reduceLastThree()
            {
                //if (stack.Count < 2)
                //    throw new ExeEE("不正な時期の呼び出し");
                IOperandTerm right = (IOperandTerm)stack.Pop();//後から入れたほうが右側
                OperatorCode op = (OperatorCode)stack.Pop();
                IOperandTerm left = (IOperandTerm)stack.Pop();
                if (OperatorManager.IsTernary(op))
                {
                    if (stack.Count > 1)
                    {
                        reduceTernary(left, right);
                        return;
                    }
                    throw new CodeEE(trerror.InsufficientExpression.Text);
                }
                
                IOperandTerm newTerm = OperatorMethodManager.ReduceBinaryTerm(op, left, right);
                stack.Push(newTerm);
			}

            private void reduceTernary(IOperandTerm left, IOperandTerm right)
            {
                _ = (OperatorCode)stack.Pop();
				IOperandTerm newLeft = (IOperandTerm)stack.Pop();
				
                IOperandTerm newTerm = OperatorMethodManager.ReduceTernaryTerm(newLeft, left, right);
                stack.Push(newTerm);
            }

/*			SingleTerm GetSingle(IOperandTerm oprand)
			{
				return (SingleTerm)oprand;
			}
*/        }

    }
}