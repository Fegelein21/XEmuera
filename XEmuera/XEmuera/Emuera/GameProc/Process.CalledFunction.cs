using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameData;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameData.Function;
using MinorShift.Emuera.GameData.Variable;
using trerror = EvilMask.Emuera.Lang.Error;

namespace MinorShift.Emuera.GameProc
{

	internal sealed class UserDefinedFunctionArgument
	{
		public UserDefinedFunctionArgument(IOperandTerm[] srcArgs, VariableTerm[] destArgs)
		{
			Arguments = srcArgs;
			TransporterInt = new Int64[Arguments.Length];
			TransporterStr = new string[Arguments.Length];
			TransporterRef = new Array[Arguments.Length];
			isRef = new bool[Arguments.Length];
			for (int i = 0; i < Arguments.Length; i++)
			{
				isRef[i] = destArgs[i].Identifier.IsReference;
			}
		}
		public readonly IOperandTerm[] Arguments;
		public readonly Int64[] TransporterInt;
		public readonly string[] TransporterStr;
		public readonly Array[] TransporterRef;
		public readonly bool[] isRef;
		public void SetTransporter(ExpressionMediator exm)
		{
			for (int i = 0; i < Arguments.Length; i++)
			{
				if (Arguments[i] == null)
					continue;
				if (isRef[i])
				{
					VariableTerm vTerm = (VariableTerm)Arguments[i];
					if (vTerm.Identifier.IsCharacterData)
					{
						Int64 charaNo = vTerm.GetElementInt(0, exm);
						if ((charaNo < 0) || (charaNo >= GlobalStatic.VariableData.CharacterList.Count))
							throw new CodeEE(string.Format(trerror.OoRCharaVarArg.Text, vTerm.Identifier.Name, "1", charaNo.ToString()));
						TransporterRef[i] = (Array)vTerm.Identifier.GetArrayChara((int)charaNo);
					}
					else
						TransporterRef[i] = (Array)vTerm.Identifier.GetArray();

				}
				else if (Arguments[i].GetOperandType() == typeof(Int64))
					TransporterInt[i] = Arguments[i].GetIntValue(exm);
				else
					TransporterStr[i] = Arguments[i].GetStrValue(exm);
			}
		}
		public UserDefinedFunctionArgument Restructure(ExpressionMediator exm)
		{
			for (int i = 0; i < Arguments.Length; i++)
			{
				if (Arguments[i] == null)
					continue;
				if(isRef[i])
					Arguments[i].Restructure(exm);
				else
					Arguments[i] = Arguments[i].Restructure(exm);
			}
			return this;
		}
	}

	/// <summary>
	/// 現在呼び出し中の関数
	/// イベント関数を除いて実行中に内部状態は変化しないので使いまわしても良い
	/// </summary>
	internal sealed class CalledFunction
	{
		private CalledFunction(string label) { FunctionName = label; }
		public static CalledFunction CallEventFunction(Process parent, string label, LogicalLine retAddress)
		{
			CalledFunction called = new CalledFunction(label);
			//List<FunctionLabelLine> newLabelList = new List<FunctionLabelLine>();
			called.Finished = false;
			called.eventLabelList = parent.LabelDictionary.GetEventLabels(label);
			if (called.eventLabelList == null)
			{
				FunctionLabelLine line = parent.LabelDictionary.GetNonEventLabel(label);
				if (parent.LabelDictionary.GetNonEventLabel(label) != null)
				{
					throw new CodeEE(string.Format(trerror.CalleventToNonEventFunc.Text, label, line.Position.Filename, line.Position.LineNo));
				}
				return null;
			}
			called.counter = -1;
			called.group = 0;
			called.ShiftNext();
			called.TopLabel = called.CurrentLabel;
			called.returnAddress = retAddress;
			called.IsEvent = true;
			return called;
		}

		public static CalledFunction CallFunction(Process parent, string label, LogicalLine retAddress)
		{
			CalledFunction called = new CalledFunction(label);
			called.Finished = false;
			FunctionLabelLine labelline = parent.LabelDictionary.GetNonEventLabel(label);
			if (labelline == null)
			{
				if (parent.LabelDictionary.GetEventLabels(label) != null)
				{
					throw new CodeEE(string.Format(trerror.CallToEventFunc.Text, label, Config.GetConfigName(ConfigCode.CompatiCallEvent)));
				}
				return null;
			}
            else if (labelline.IsMethod)
            {
                throw new CodeEE(string.Format(trerror.CallToUserFunc.Text, labelline.LabelName, labelline.Position.Filename, labelline.Position.LineNo.ToString()));
            }
			called.TopLabel = labelline;
			called.CurrentLabel = labelline;
			called.returnAddress = retAddress;
			called.IsEvent = false;
            return called;
		}

		public static CalledFunction CreateCalledFunctionMethod(FunctionLabelLine labelline, string label)
		{
			CalledFunction called = new CalledFunction(label);
			called.TopLabel = labelline;
			called.CurrentLabel = labelline;
			called.returnAddress = null;
			called.IsEvent = false;
			return called;
		}
		
		
		static FunctionMethod tostrMethod = null;
		/// <summary>
		/// 1803beta005 予め引数の数を合わせて規定値を代入しておく
        /// 1806+v6.99 式中関数の引数に無効な#DIM変数を与えている場合に例外になるのを修正
		/// 1808beta009 REF型に対応
		/// </summary>
		public UserDefinedFunctionArgument ConvertArg(IOperandTerm[] srcArgs, out string errMes)
		{
			errMes = null;
            if (TopLabel.IsError)
            {
                errMes = TopLabel.ErrMes;
                return null;
            }
            FunctionLabelLine func = TopLabel;
            IOperandTerm[] convertedArg = new IOperandTerm[func.Arg.Length];
			if(convertedArg.Length < srcArgs.Length)
			{
				errMes = string.Format(trerror.TooManyFuncArgs.Text, func.LabelName);
				return null;
			}
			IOperandTerm term;
			VariableTerm destArg;
			//bool isString = false;
			for (int i = 0; i < func.Arg.Length; i++)
			{
				term = (i < srcArgs.Length) ? srcArgs[i] : null;
				destArg = func.Arg[i];
				//isString = destArg.IsString;
				if (destArg.Identifier.IsReference)//参照渡しの場合
				{
					if (term == null)
					{
						errMes = string.Format(trerror.CanNotOmitRefArg.Text, func.LabelName, (i + 1).ToString());
						return null;
					}
					VariableTerm vTerm = term as VariableTerm;
					if (vTerm == null || vTerm.Identifier.Dimension == 0)
					{
						errMes = string.Format(trerror.RequireArrayBecauseRefArg.Text, func.LabelName, (i + 1).ToString());
						return null;
					}
					//TODO 1810alpha007 キャラ型を認めるかどうかはっきりしたい 今のところ認めない方向
					//型チェック
					if (!((ReferenceToken)destArg.Identifier).MatchType(vTerm.Identifier, false, out errMes))
					{
						errMes = string.Format(trerror.NumberOfArg.Text, func.LabelName, (i + 1).ToString(), errMes);
						return null;
					}
				}
				else if (term == null)//引数が省略されたとき
				{
					term = func.Def[i];//デフォルト値を代入
					//1808beta001 デフォルト値がない場合はエラーにする
					//一応逃がす
					if (term == null && !Config.CompatiFuncArgOptional)
					{
						errMes = string.Format(trerror.CanNotOmitArgWithMessage.Text, func.LabelName, (i + 1).ToString(), Config.GetConfigName(ConfigCode.CompatiFuncArgOptional));
						return null;
					}
				}
				else if (term.GetOperandType() != destArg.GetOperandType())
				{
					if (term.GetOperandType() == typeof(string))
					{
						errMes = string.Format(trerror.CanNotConvertStrToInt.Text, func.LabelName, (i + 1).ToString());
						return null;
					}
					else
					{
						if (!Config.CompatiFuncArgAutoConvert)
						{
							errMes = string.Format(trerror.CanNotConvertIntToStr.Text, func.LabelName, (i + 1).ToString(), Config.GetConfigName(ConfigCode.CompatiFuncArgAutoConvert));
							return null;
						}
						if (tostrMethod == null)
							tostrMethod = FunctionMethodCreator.GetMethodList()["TOSTR"];
						term = new FunctionMethodTerm(tostrMethod, new IOperandTerm[] { term });
					}
				}
				convertedArg[i] = term;
			}
			return new UserDefinedFunctionArgument(convertedArg, func.Arg);
		}

		public LogicalLine CallLabel(Process parent, string label)
		{
			return parent.LabelDictionary.GetLabelDollar(label, this.CurrentLabel);
		}

        public void updateRetAddress(LogicalLine line)
        {
            returnAddress = line;
        }

		public CalledFunction Clone()
		{
			CalledFunction called = new CalledFunction(this.FunctionName);
			called.eventLabelList = this.eventLabelList;
			called.CurrentLabel = this.CurrentLabel;
			called.TopLabel = this.TopLabel;
			called.group = this.group;
			called.IsEvent = this.IsEvent;

			called.counter = this.counter;
			called.returnAddress = this.returnAddress;
			return called;
		}

		List<FunctionLabelLine>[] eventLabelList;
		public FunctionLabelLine CurrentLabel { get; private set; }
		public FunctionLabelLine TopLabel { get; private set; }
		int counter = -1;
		int group = 0;
		LogicalLine returnAddress;
		public readonly string FunctionName = "";
		public bool IsJump { get; set; }
		public bool Finished { get; private set; }
		public LogicalLine ReturnAddress
		{
			get { return returnAddress; }
		}
		public bool IsEvent{get; private set;}

		public bool HasSingleFlag
		{
			get
			{
				if (CurrentLabel == null)
					return false;
				return CurrentLabel.IsSingle;
			}
		}


		#region イベント関数専用
		public void ShiftNext()
		{
			while (true)
			{
				counter++;
				if (eventLabelList[group].Count > counter)
				{
					CurrentLabel = (eventLabelList[group])[counter];
					return;
				}
				group++;
				counter = -1;
				if (group >= 4)
				{
					CurrentLabel = null;
					return;
				}
			}
		}

		public void ShiftNextGroup()
		{
			counter = -1;
			group++;
            if (group >= 4)
            {
                CurrentLabel = null;
                return;
            }
			ShiftNext();
		}

        public void FinishEvent()
        {
            group = 4;
            counter = -1;
            CurrentLabel = null;
            return;
        }

        public bool IsOnly
        {
            get { return CurrentLabel.IsOnly; }
        }
		#endregion
	}
}
