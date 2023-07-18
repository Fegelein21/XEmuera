using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.GameProc.Function;
using MinorShift._Library;
using MinorShift.Emuera.GameData;
using XEmuera;
using trsl = EvilMask.Emuera.Lang.SystemLine;
using trerror = EvilMask.Emuera.Lang.Error;

namespace MinorShift.Emuera.GameProc
{
	internal sealed class ErbLoader
	{
		public ErbLoader(EmueraConsole main, ExpressionMediator exm, Process proc)
		{
			output = main;
			parentProcess = proc;
			this.exm = exm;
		}
		readonly Process parentProcess;
		readonly ExpressionMediator exm;
		readonly EmueraConsole output;
        readonly List<string> ignoredFNFWarningFileList = new List<string>();
		int ignoredFNFWarningCount = 0;

		int enabledLineCount = 0;
		LabelDictionary labelDic;

		bool noError = true;

		/// <summary>
		/// 複数のファイルを読む
		/// </summary>
		/// <param name="filepath"></param>
		public bool LoadErbFiles(string erbDir, bool displayReport, LabelDictionary labelDictionary)
		{
			//1.713 labelDicをnewする位置を変更。
			//checkScript();の時点でExpressionPerserがProcess.instance.LabelDicを必要とするから。
			labelDic = labelDictionary;
			labelDic.Initialized = false;
			List<KeyValuePair<string, string>> erbFiles = Config.GetFiles(erbDir, "*.ERB");
            List<string> isOnlyEvent = new List<string>();
            noError = true;
			uint starttime = WinmmTimer.TickCount;
			try
			{
				labelDic.RemoveAll();
				for (int i = 0; i < erbFiles.Count; i++)
				{
					string filename = erbFiles[i].Key;
					string file = erbFiles[i].Value;
#if DEBUG
					if (displayReport)
						output.PrintSystemLine(string.Format(trsl.ElapsedTimeLoad.Text, (WinmmTimer.TickCount - starttime).ToString("D4"), filename));
#else
					if (displayReport)
						output.PrintSystemLine(string.Format(trsl.LoadingFile.Text, filename));
#endif
					App.DoEvents();
					loadErb(file, filename, isOnlyEvent);
				}
				ParserMediator.FlushWarningList();
#if DEBUG
				output.PrintSystemLine(string.Format(trsl.ElapsedTime.Text, (WinmmTimer.TickCount - starttime).ToString("D4")));
#endif
				if (displayReport)
					output.PrintSystemLine(trsl.BuildingUserFunc.Text);
				setLabelsArg();
				ParserMediator.FlushWarningList();
				labelDic.Initialized = true;
#if DEBUG
				output.PrintSystemLine(string.Format(trsl.ElapsedTime.Text, (WinmmTimer.TickCount - starttime).ToString("D4")));
#endif
				if (displayReport)
					output.PrintSystemLine(trsl.CheckingSyntax.Text);
				checkScript();
				ParserMediator.FlushWarningList();

#if DEBUG
				output.PrintSystemLine(string.Format(trsl.ElapsedTime.Text, (WinmmTimer.TickCount - starttime).ToString("D4")));
#endif
				if (displayReport)
					output.PrintSystemLine(trsl.LoadComplete.Text);
			}
			catch (Exception e)
			{
				ParserMediator.FlushWarningList();
				// System.Media.SystemSounds.Hand.Play();
				output.PrintError(string.Format(trerror.UnexpectedErrorFrom.Text, Program.ExeName));
				output.PrintError(e.GetType().ToString() + ":" + e.Message);
				return false;
			}
			finally
			{
				parentProcess.scaningLine = null;
			}
            isOnlyEvent.Clear();
			return noError;
		}

		/// <summary>
		/// 指定されたファイルを読み込む
		/// </summary>
		/// <param name="filename"></param>
		public bool loadErbs(List<string> path, LabelDictionary labelDictionary)
		{
			string fname;
            List<string> isOnlyEvent = new List<string>();
            noError = true;
			labelDic = labelDictionary;
			labelDic.Initialized = false;
			foreach (string fpath in path)
			{
				if (fpath.StartsWith(Program.ErbDir, Config.SCIgnoreCase) && !Program.AnalysisMode)
					fname = fpath.Substring(Program.ErbDir.Length);
				else
					fname = fpath;
				if (Program.AnalysisMode)
					output.PrintSystemLine(string.Format(trsl.LoadingFile.Text, fname));
				App.DoEvents();
                loadErb(fpath, fname, isOnlyEvent);
			}
            if (Program.AnalysisMode)
                output.NewLine();
            ParserMediator.FlushWarningList();
			setLabelsArg();
			ParserMediator.FlushWarningList();
			labelDic.Initialized = true;
            checkScript();
			ParserMediator.FlushWarningList();
			parentProcess.scaningLine = null;
            isOnlyEvent.Clear();
            return noError;
		}

		private sealed class PPState
		{
			bool skip = false;
			bool done = false;
			public bool Disabled = false;
            readonly Stack<bool> disabledStack = new Stack<bool>();
            readonly Stack<bool> doneStack = new Stack<bool>();
            readonly Stack<string> ppMatch = new Stack<string>();

			internal void AddKeyWord(string token, string token2, ScriptPosition position)
			{
				//bool token2enabled = string.IsNullOrEmpty(token2);
				switch (token)
				{
					case "SKIPSTART":
						if (!string.IsNullOrEmpty(token2))
						{
							ParserMediator.Warn(string.Format(trerror.HasTooManyArg.Text, token), position, 1);
							break;
						}
						if (skip)
						{
							ParserMediator.Warn(trerror.DuplicateSkipstart.Text, position, 1);
							break;
						}
						ppMatch.Push("SKIPEND");
						disabledStack.Push(Disabled);
						doneStack.Push(done);
						skip = true;
						Disabled = true;
						done = false;
						break;
					case "IF_DEBUG":
						if (!string.IsNullOrEmpty(token2))
						{
							ParserMediator.Warn(string.Format(trerror.HasTooManyArg.Text, token), position, 1);
							break;
						}
						ppMatch.Push("ELSEIF");
						disabledStack.Push(Disabled);
						doneStack.Push(done);
						Disabled = !Program.DebugMode;
						done = !Disabled;
						break;
					case "IF_NDEBUG":
						if (!string.IsNullOrEmpty(token2))
						{
							ParserMediator.Warn(string.Format(trerror.HasTooManyArg.Text, token), position, 1);
							break;
						}
						ppMatch.Push("ELSEIF");
						disabledStack.Push(Disabled);
						doneStack.Push(done);
						Disabled = Program.DebugMode;
						done = !Disabled;
						break;
					case "IF":
						if (string.IsNullOrEmpty(token2))
						{
							ParserMediator.Warn(string.Format(trerror.MissingArguments.Text, token), position, 1);
							break;
						}
						ppMatch.Push("ELSEIF");
						disabledStack.Push(Disabled);
						doneStack.Push(done);
						Disabled = GlobalStatic.IdentifierDictionary.GetMacro(token2) == null;
						done = !Disabled;
						break;
					case "ELSEIF":
						if (string.IsNullOrEmpty(token2))
						{
							ParserMediator.Warn(string.Format(trerror.MissingArguments.Text, token), position, 1);
							break;
						}
						if (ppMatch.Count == 0 || ppMatch.Pop() != "ELSEIF")
						{
							ParserMediator.Warn(string.Format(trerror.IsInvalid.Text, "[ELSEIF]"), position, 1);
							break;
						}
						ppMatch.Push("ELSEIF");
						Disabled = done || (GlobalStatic.IdentifierDictionary.GetMacro(token2) == null);
						done |= !Disabled;
						break;
					case "ELSE":
						if (!string.IsNullOrEmpty(token2))
						{
							ParserMediator.Warn(string.Format(trerror.HasTooManyArg.Text, token), position, 1);
							break;
						}
						if (ppMatch.Count == 0 || ppMatch.Pop() != "ELSEIF")
						{
							ParserMediator.Warn(string.Format(trerror.IsInvalid.Text, "[ELSE]"), position, 1);
							break;
						}
						ppMatch.Push("ENDIF");
						Disabled = done;
						done = true;
						break;

					case "SKIPEND":
						{
							if (!string.IsNullOrEmpty(token2))
							{
								ParserMediator.Warn(string.Format(trerror.HasTooManyArg.Text, token), position, 1);
								break;
							}
							string match = ppMatch.Count == 0 ? "" : ppMatch.Pop();
							if (match != "SKIPEND")
							{
								ParserMediator.Warn(trerror.UnexpectedSkipend.Text, position, 1);
								break;
							}
							skip = false;
							Disabled = disabledStack.Pop();
							done = doneStack.Pop();
						}
						break;
					case "ENDIF":
						{
							if (!string.IsNullOrEmpty(token2))
							{
								ParserMediator.Warn(string.Format(trerror.HasTooManyArg.Text, token), position, 1);
								break;
							}
							string match = ppMatch.Count == 0 ? "" : ppMatch.Pop();
							if (match != "ENDIF" && match != "ELSEIF")
							{
								ParserMediator.Warn(trerror.UnexpectedMacroEndif.Text, position, 1);
								break;
							}
							Disabled = disabledStack.Pop();
							done = doneStack.Pop();
						}
						break;
					default:
						ParserMediator.Warn(trerror.UnrecognizedPreprosessor.Text, position, 1);
						break;
				}
				if (skip)
					Disabled = true;
			}

			internal void FileEnd(ScriptPosition position)
			{
				if (ppMatch.Count != 0)
				{
					string match = ppMatch.Pop();
					if (match == "ELSEIF")
						match = "ENDIF";
					ParserMediator.Warn(string.Format(trerror.TheresNo.Text, match), position, 1);
				}
			}
		}

		/// <summary>
		/// ファイル一つを読む
		/// </summary>
		/// <param name="filepath"></param>
		private void loadErb(string filepath, string filename, List<string> isOnlyEvent)
		{
			//読み込んだファイルのパスを記録
			//一部ファイルの再読み込み時の処理用
			labelDic.AddFilename(filename);
			EraStreamReader eReader = new EraStreamReader(Config.UseRenameFile && ParserMediator.RenameDic != null);
			if (!eReader.Open(filepath, filename))
			{
				output.PrintError(string.Format(trerror.FailedOpenFile.Text, eReader.Filename));
				return;
			}
			try
			{
				PPState ppstate = new PPState();
				LogicalLine nextLine = new NullLine();
				LogicalLine lastLine = new NullLine();
				FunctionLabelLine lastLabelLine = null;
				StringStream st = null;
				ScriptPosition position = null;
				int funcCount = 0;
				if (Program.AnalysisMode)
					output.PrintSystemLine("　");
				while ((st = eReader.ReadEnabledLine(ppstate.Disabled)) != null)
				{
					position = new ScriptPosition(eReader.Filename, eReader.LineNo);
					//rename処理をEraStreamReaderに移管
					//変換できなかった[[～～]]についてはLexAnalyzerがエラーを投げる
					if (st.Current == '[' && st.Next != '[')
					{
						st.ShiftNext();
						string token = LexicalAnalyzer.ReadSingleIdentifier(st);
						LexicalAnalyzer.SkipWhiteSpace(st);
						string token2 = LexicalAnalyzer.ReadSingleIdentifier(st);
						if ((string.IsNullOrEmpty(token)) || (st.Current != ']'))
							ParserMediator.Warn(trerror.InvalidSBrackets.Text, position, 1);
						ppstate.AddKeyWord(token, token2, position);
						st.ShiftNext();
						if (!st.EOS)
							ParserMediator.Warn(string.Format(trerror.IgnoreAfterPreprosessor.Text, token), position, 1);
						continue;
					}
					//if ((skip) || (Program.DebugMode && ifndebug) || (!Program.DebugMode && ifdebug))
					//	continue;
					if (ppstate.Disabled)
						continue;
					//ここまでプリプロセッサ

					if (st.Current == '#')
					{
						if ((lastLine == null) || !(lastLine is FunctionLabelLine))
						{
							ParserMediator.Warn(trerror.InvalidSharp.Text, position, 1);
							continue;
						}
						if (!LogicalLineParser.ParseSharpLine((FunctionLabelLine)lastLine, st, position, isOnlyEvent))
							noError = false;
						continue;
					}
					if ((st.Current == '$') || (st.Current == '@'))
					{
						bool isFunction = (st.Current == '@');
						nextLine = LogicalLineParser.ParseLabelLine(st, position, output);
						if (isFunction)
						{
							FunctionLabelLine label = (FunctionLabelLine)nextLine;
							lastLabelLine = label;
							if (label is InvalidLabelLine)
							{
								noError = false;
								ParserMediator.Warn(nextLine.ErrMes, position, 2);
								labelDic.AddInvalidLabel(label);
							}
							else// if (label is FunctionLabelLine)
							{
								labelDic.AddLabel(label);
								if (!label.IsEvent && (Config.WarnNormalFunctionOverloading || Program.AnalysisMode))
								{
									FunctionLabelLine seniorLabel = labelDic.GetSameNameLabel(label);
                                    if (seniorLabel != null)
                                    {
                                        //output.NewLine();
                                        ParserMediator.Warn(string.Format(trerror.FuncIsAlreadyDefined.Text, label.LabelName, seniorLabel.Position.Filename, seniorLabel.Position.LineNo.ToString()), position, 1);
                                        funcCount = -1;
                                    }
								}
								funcCount++;
								if (Program.AnalysisMode && (Config.PrintCPerLine > 0 && (funcCount % Config.PrintCPerLine) == 0))
								{
									output.NewLine();
									output.PrintSystemLine("　");
								}
							}
						}
						else
						{
                            if (nextLine is GotoLabelLine gotoLabel)
                            {
                                gotoLabel.ParentLabelLine = lastLabelLine;
                                if (lastLabelLine != null && !labelDic.AddLabelDollar(gotoLabel))
                                {
                                    ScriptPosition pos = labelDic.GetLabelDollar(gotoLabel.LabelName, lastLabelLine).Position;
                                    ParserMediator.Warn(string.Format(trerror.LabelIsAlreadyDefined.Text, gotoLabel.LabelName, pos.Filename, pos.LineNo.ToString()), position, 2);
                                }
                            }
                        }
						if (nextLine is InvalidLine)
						{
							noError = false;
							ParserMediator.Warn(nextLine.ErrMes, position, 2);
						}
					}
					else
					{
						//1808alpha006 処理位置変更
                        ////全置換はここで対応
                        ////1756beta1+++　最初に全置換してしまうと関数定義を_Renameでとか論外なことができてしまうので永久封印した
                        //if (ParserMediator.RenameDic != null && st.CurrentEqualTo("[[") && (rowLine.TrimEnd().IndexOf("]]") == rowLine.TrimEnd().Length - 2))
                        //{
                        //    string replacedLine = st.Substring();
                        //    foreach (KeyValuePair<string, string> pair in ParserMediator.RenameDic)
                        //        replacedLine = replacedLine.Replace(pair.Key, pair.Value);
                        //    st = new StringStream(replacedLine);
                        //}
                        nextLine = LogicalLineParser.ParseLine(st, position, output);
						if (nextLine == null)
							continue;
						if (nextLine is InvalidLine)
						{
							noError = false;
                            ParserMediator.Warn(nextLine.ErrMes, position, 2);
						}
					}
					if (lastLabelLine == null)
						ParserMediator.Warn(trerror.LineBeforeFunc.Text, position, 1);
					nextLine.ParentLabelLine = lastLabelLine;
					lastLine = addLine(nextLine, lastLine);
				}
				addLine(new NullLine(), lastLine);
				position = new ScriptPosition(eReader.Filename, -1);
				ppstate.FileEnd(position);
			}
			finally
			{
				eReader.Close();
			}
			return;
		}
		
		private LogicalLine addLine(LogicalLine nextLine, LogicalLine lastLine)
		{
			if (nextLine == null)
				return null;
			enabledLineCount++;
			lastLine.NextLine = nextLine;
			return nextLine;
		}

		private void setLabelsArg()
		{
			List<FunctionLabelLine> labelList = labelDic.GetAllLabels(false);
			foreach (FunctionLabelLine label in labelList)
			{
				try
				{
					if (label.Arg != null)
						continue;
					parentProcess.scaningLine = label;
					parseLabel(label);
				}
				catch (Exception exc)
				{
					//System.Media.SystemSounds.Hand.Play();
					string errmes = exc.Message;
					if (!(exc is EmueraException))
						errmes = exc.GetType().ToString() + ":" + errmes;
					ParserMediator.Warn(string.Format(trerror.FuncArgError.Text, label.LabelName, errmes), label, 2, true, false);
					label.ErrMes = trerror.CalledFailedFunc.Text;
                    label.IsError = true;
				}
				finally
				{
					parentProcess.scaningLine = null;
				}
			}
			labelDic.SortLabels();
		}

		private void parseLabel(FunctionLabelLine label)
		{
			WordCollection wc = label.PopRowArgs();
			string errMes;
			SingleTerm[] subNames;
			VariableTerm[] args = new VariableTerm[0];
			SingleTerm[] defs = new SingleTerm[0];
			int maxArg = -1;
			int maxArgs = -1;
			//1807 非イベント関数のシステム関数については警告レベル低下＆エラー解除＆引数を設定するように。
			if (label.IsEvent)
			{
				if (!wc.EOL)
					ParserMediator.Warn(string.Format(trerror.EventFuncHasArg.Text, label.LabelName), label, 2, true, false);
				//label.SubNames = subNames;
				label.Arg = args;
				label.Def = defs;
				label.ArgLength = -1;
				label.ArgsLength = -1;
				return;
			}

			if (!wc.EOL)
			{
				if (label.IsSystem)
					ParserMediator.Warn(string.Format(trerror.SystemFuncHasArg.Text, label.LabelName), label, 2, true, false);
				SymbolWord symbol = wc.Current as SymbolWord;
				wc.ShiftNext();
                if (symbol == null)
				{ errMes = trerror.WrongArgFormat.Text; goto err; }
				if (symbol.Type == '[')//TODO:subNames 結局実装しないかも
				{
					IOperandTerm[] subNamesRow = ExpressionParser.ReduceArguments(wc, ArgsEndWith.RightBracket, false);
					if (subNamesRow.Length == 0)
					{ errMes = trerror.CanNotEmptyFuncSBrackets.Text; goto err; }
					subNames = new SingleTerm[subNamesRow.Length];
					for (int i = 0; i < subNamesRow.Length; i++)
					{
						if (subNamesRow[i] == null)
						{ errMes = trerror.CanNotOmitFuncDefineArg.Text; goto err; }
						IOperandTerm term = subNamesRow[i].Restructure(exm);
						subNames[i] = term as SingleTerm;
						if (subNames[i] == null)
						{ errMes = trerror.FuncDefineArgOnlyConst.Text; goto err; }
					}
					symbol = wc.Current as SymbolWord;
					if ((!wc.EOL) && (symbol == null))
					{ errMes = trerror.WrongArgFormat.Text; goto err; }
					wc.ShiftNext();
				}
				if (!wc.EOL)
				{
					IOperandTerm[] argsRow;
                    if (symbol.Type == ',')
						argsRow = ExpressionParser.ReduceArguments(wc, ArgsEndWith.EoL, true);
					else if (symbol.Type == '(')
						argsRow = ExpressionParser.ReduceArguments(wc, ArgsEndWith.RightParenthesis, true);
					else
					{ errMes = trerror.WrongArgFormat.Text; goto err; }
					int length = argsRow.Length / 2;
					args = new VariableTerm[length];
                    defs = new SingleTerm[length];
					for (int i = 0; i < length; i++)
					{
						SingleTerm def = null;
						IOperandTerm term = argsRow[i * 2];
                        //引数読み取り時点で判別されないといけない
                        //if (term == null)
                        //{ errMes = "関数定義の引数は省略できません"; goto err; }
                        if ((!(term.Restructure(exm) is VariableTerm vTerm)) || (vTerm.Identifier.IsConst))
                        { errMes = trerror.ArgCanOnlyAssignableVar.Text; goto err; }
                        else if (!vTerm.Identifier.IsReference)//参照型なら添え字不要
                        {
                            if (vTerm is VariableNoArgTerm)
                            { errMes = string.Format(trerror.ArgHasNotSubscript.Text, vTerm.Identifier.Name); goto err; }
                            if (!vTerm.isAllConst)
                            { errMes = trerror.ArgSubscriptOnlyConst.Text; goto err; }
                        }
                        for (int j = 0; j < i; j++)
                        {
                            if (vTerm.checkSameTerm(args[j]))
                                ParserMediator.Warn(string.Format(trerror.DuplicateArg.Text, Strings.StrConv((i + 1).ToString(), VbStrConv.Wide, Config.Language), vTerm.GetFullString(), Strings.StrConv((j + 1).ToString(), VbStrConv.Wide, Config.Language)), label, 1, false, false);
                        }
						if (vTerm.Identifier.Code == VariableCode.ARG)
						{
							if (maxArg < vTerm.getEl1forArg + 1)
								maxArg = vTerm.getEl1forArg + 1;
						}
						else if (vTerm.Identifier.Code == VariableCode.ARGS)
						{
							if (maxArgs < vTerm.getEl1forArg + 1)
								maxArgs = vTerm.getEl1forArg + 1;
						}
						bool canDef = (vTerm.Identifier.Code == VariableCode.ARG || vTerm.Identifier.Code == VariableCode.ARGS || vTerm.Identifier.IsPrivate);
						term = argsRow[i * 2 + 1];
						if (term is NullTerm)
						{
							if (canDef)// && label.ArgOptional)
							{
								if (vTerm.GetOperandType() == typeof(Int64))
									def = new SingleTerm(0);
								else
									def = new SingleTerm("");
							}
						}
						else
						{
							def = term.Restructure(exm) as SingleTerm;
							if (def == null)
							{ errMes = trerror.ArgCanOnlyConst.Text; goto err; }
							if (!canDef)
							{ errMes = trerror.ArgCanOnlyPrivVar.Text; goto err; }
							else if (vTerm.Identifier.IsReference)
							{ errMes = trerror.RefArgCanNotInitialize.Text; goto err; }
							if (vTerm.GetOperandType() != def.GetOperandType())
							{ errMes = trerror.NotMatchTypeArgAndInitialValue.Text; goto err; }
						}
						args[i] = vTerm;
						defs[i] = def;
					}

				}
			}
			if (!wc.EOL)
			{ errMes = trerror.WrongArgFormat.Text; goto err; }

            //label.SubNames = subNames;
			label.Arg = args;
			label.Def = defs;
			label.ArgLength = maxArg;
			label.ArgsLength = maxArgs;
			return;
		err:
			ParserMediator.Warn(string.Format(trerror.FuncArgError.Text, label.LabelName, errMes), label, 2, true, false);
			return;
		}


		public bool useCallForm = false;
		/// <summary>
		/// 読込終わったファイルをチェックする
		/// </summary>
		private void checkScript()
		{
			int usedLabelCount = 0;
			int labelDepth = -1;
			List<FunctionLabelLine> labelList = labelDic.GetAllLabels(true);

			while (true)
			{
				labelDepth++;
				int countInDepth = 0;
				foreach (FunctionLabelLine label in labelList)
				{
					if (label.Depth != labelDepth)
						continue;
					//1756beta003 なんで追加したんだろう デバグ中になんかやったのか とりあえずコメントアウトしておく
					//if (label.LabelName == "EVENTTURNEND")
					//    useCallForm = true;
					usedLabelCount++;
					countInDepth++;
					checkFunctionWithCatch(label);
				}
				if (countInDepth == 0)
					break;
			}
            labelDepth = -1;
			List<string> ignoredFNCWarningFileList = new List<string>();
			int ignoredFNCWarningCount = 0;

			bool ignoreAll = false;
			DisplayWarningFlag notCalledWarning = Config.FunctionNotCalledWarning;
			switch (notCalledWarning)
			{
				case DisplayWarningFlag.IGNORE:
				case DisplayWarningFlag.LATER:
					ignoreAll = true;
					break;
			}
			if (useCallForm)
			{//callform系が使われたら全ての関数が呼び出されたとみなす。
                if (Program.AnalysisMode)
					output.PrintSystemLine(trerror.BeNotFuncCheckBecauseUseCallform.Text);
				foreach (FunctionLabelLine label in labelList)
				{
					if (label.Depth != labelDepth)
						continue;
					checkFunctionWithCatch(label);
				}
			}
			else
			{
				bool ignoreUncalledFunction = Config.IgnoreUncalledFunction;
				foreach (FunctionLabelLine label in labelList)
				{
					if (label.Depth != labelDepth)
						continue;
                    //解析モード時は呼ばれなかったものをここで解析
                    if (Program.AnalysisMode)
                        checkFunctionWithCatch(label);
					bool ignore = false;
					if (notCalledWarning == DisplayWarningFlag.ONCE)
					{
						string filename = label.Position.Filename.ToUpper();

						if (!string.IsNullOrEmpty(filename))
						{
							if (ignoredFNCWarningFileList.Contains(filename))
							{
								ignore = true;
							}
							else
							{
								ignore = false;
								ignoredFNCWarningFileList.Add(filename);
							}
						}
                        //break;
					}
					if (ignoreAll || ignore)
						ignoredFNCWarningCount++;
					else
						ParserMediator.Warn(string.Format(trerror.FuncNeverCalled.Text, label.LabelName), label, 1, false, false);
					if (!ignoreUncalledFunction)
						checkFunctionWithCatch(label);
					else
					{
						if (!(label.NextLine is NullLine) && !(label.NextLine is FunctionLabelLine))
						{
							if (!label.NextLine.IsError)
							{
								label.NextLine.IsError = true;
								label.NextLine.ErrMes = "呼び出されないはずの関数が呼ばれた";
							}
						}
					}
				}
			}
			if (Program.AnalysisMode && (warningDic.Keys.Count > 0 || GlobalStatic.tempDic.Keys.Count > 0))
			{
				output.PrintError(trerror.UndefinedFunctions.Text);
				if (warningDic.Keys.Count > 0)
				{
					output.PrintError(trerror.GeneralFunc.Text);
					foreach (string labelName in warningDic.Keys)
					{
						output.PrintError("　　" + labelName + ": " + warningDic[labelName].ToString() + trerror.Occurrences.Text);
					}
				}
				if (GlobalStatic.tempDic.Keys.Count > 0)
				{
					output.PrintError(trerror.SentenceFunc.Text);
					foreach (string labelName in GlobalStatic.tempDic.Keys)
					{
						output.PrintError("　　" + labelName + ": " + GlobalStatic.tempDic[labelName].ToString() + trerror.Occurrences.Text);
					}
				}
			}
			else
			{
				if ((ignoredFNCWarningCount > 0) && (Config.DisplayWarningLevel <= 1) && (notCalledWarning != DisplayWarningFlag.IGNORE))
					output.PrintError(string.Format(trerror.IgnoredFuncNeverCalled.Text, ignoredFNCWarningCount));
				if ((ignoredFNFWarningCount > 0) && (Config.DisplayWarningLevel <= 2) && (notCalledWarning != DisplayWarningFlag.IGNORE))
					output.PrintError(string.Format(trerror.IgnoredUndefinedFuncCall.Text, ignoredFNFWarningCount));
			}
			ParserMediator.FlushWarningList();
			if (Config.DisplayReport)
				output.PrintError(string.Format(trerror.TotalFunc.Text, enabledLineCount, labelDic.Count, usedLabelCount));
			if (Config.AllowFunctionOverloading && Config.WarnFunctionOverloading)
			{
				List<string> overloadedList = GlobalStatic.IdentifierDictionary.GetOverloadedList(labelDic);
				if (overloadedList.Count > 0)
				{
					output.NewLine();
					output.PrintError(trerror.OverWriteSystemFuncWarn1.Text);
					foreach (string funcname in overloadedList)
					{
						output.PrintSystemLine(string.Format(trerror.OverWriteSystemFuncWarn2.Text, funcname));
					}
					output.PrintSystemLine(trerror.OverWriteSystemFuncWarn3.Text);
					output.NewLine();
					output.PrintSystemLine(trerror.OverWriteSystemFuncWarn4.Text);
					output.PrintSystemLine(trerror.OverWriteSystemFuncWarn5.Text);
					output.PrintSystemLine(trerror.OverWriteSystemFuncWarn6.Text);
					output.PrintSystemLine(trerror.OverWriteSystemFuncWarn7.Text);
				}
			}
		}


		public Dictionary<string, Int64> warningDic = new Dictionary<string, Int64>();
		private void printFunctionNotFoundWarning(string str, LogicalLine line, int level, bool isError)
		{
			if (Program.AnalysisMode)
			{
				if (warningDic.ContainsKey(str))
					warningDic[str]++;
				else
					warningDic.Add(str, 1);
				return;
			}
			if (isError)
			{
				line.IsError = true;
				line.ErrMes = str;
			}
			if (level < Config.DisplayWarningLevel)
				return;
			bool ignore = false;
			DisplayWarningFlag warnFlag = Config.FunctionNotFoundWarning;
			if (warnFlag == DisplayWarningFlag.IGNORE)
				ignore = true;
			else if (warnFlag == DisplayWarningFlag.DISPLAY)
				ignore = false;
			else if (warnFlag == DisplayWarningFlag.ONCE)
			{

				string filename = line.Position.Filename.ToUpper();
				if (!string.IsNullOrEmpty(filename))
				{
					if (ignoredFNFWarningFileList.Contains(filename))
					{
						ignore = true;
					}
					else
					{
						ignore = false;
						ignoredFNFWarningFileList.Add(filename);
					}
				}
			}
			if (ignore && !Program.AnalysisMode)
			{
				ignoredFNFWarningCount++;
				return;
			}
			ParserMediator.Warn(str, line, level, isError, false);
		}

		private void checkFunctionWithCatch(FunctionLabelLine label)
		{//ここでエラーを捕まえることは本来はないはず。ExeEE相当。
			try
			{
				App.DoEvents();
				string filename = label.Position.Filename.ToUpper();
				setArgument(label);
				nestCheck(label);
                setJumpTo(label);
			}
			catch (Exception exc)
			{
				//System.Media.SystemSounds.Hand.Play();
                //1756beta2+v6.1 修正の効率化のために何かパース関係でハンドリングできてないエラーが出た場合はスタックトレースを投げるようにした
                string errmes = (exc is EmueraException) ? exc.Message : exc.GetType().ToString() + ":" + exc.Message;
                ParserMediator.Warn(string.Format(trerror.FuncAnalysisError.Text, label.LabelName, errmes), label, 2, true, false, !(exc is EmueraException) ? exc.StackTrace : null);
                label.ErrMes = trerror.CalledFailedFunc.Text;
			}
			finally
			{
				parentProcess.scaningLine = null;
			}

		}

		private void setArgument(FunctionLabelLine label)
		{
			//1周目/3周
			//引数の解析とか
			LogicalLine nextLine = label;
			bool inMethod = label.IsMethod;
			while (true)
			{
				nextLine = nextLine.NextLine;
				parentProcess.scaningLine = nextLine;
                if (!(nextLine is InstructionLine func))
                {
                    if ((nextLine is NullLine) || (nextLine is FunctionLabelLine))
                        break;
                    continue;
                }
                if (inMethod)
				{
					if (!func.Function.IsMethodSafe())
					{
						ParserMediator.Warn(string.Format(trerror.CanNotUseInUserFunc.Text, func.Function.Name), nextLine, 2, true, false);
						continue;
					}
				}
                if (Config.NeedReduceArgumentOnLoad || Program.AnalysisMode || func.Function.IsForceSetArg())
                    ArgumentParser.SetArgumentTo(func);
			}
		}

		private void nestCheck(FunctionLabelLine label)
		{
			//2周目/3周
			//IF-ELSEIF-ENDIF、REPEAT-RENDの対応チェックなど
			//PRINTDATA系もここでチェック
			LogicalLine nextLine = label;
			List<InstructionLine> tempLineList = new List<InstructionLine>();
			Stack<InstructionLine> nestStack = new Stack<InstructionLine>();
            Stack<InstructionLine> SelectcaseStack = new Stack<InstructionLine>();
			InstructionLine pairLine = null;
			while (true)
			{
				nextLine = nextLine.NextLine;
				parentProcess.scaningLine = nextLine;
                if ((nextLine is NullLine) || (nextLine is FunctionLabelLine))
                    break;
                if (!(nextLine is InstructionLine))
                {
                    if (nextLine is GotoLabelLine)
                    {
                        InstructionLine currentBaseFunc = nestStack.Count == 0 ? null : nestStack.Peek();
                        if (currentBaseFunc != null)
                        {
                            if ((currentBaseFunc.FunctionCode == FunctionCode.PRINTDATA)
                                || (currentBaseFunc.FunctionCode == FunctionCode.PRINTDATAL)
                                || (currentBaseFunc.FunctionCode == FunctionCode.PRINTDATAW)
                                || (currentBaseFunc.FunctionCode == FunctionCode.PRINTDATAD)
                                || (currentBaseFunc.FunctionCode == FunctionCode.PRINTDATADL)
                                || (currentBaseFunc.FunctionCode == FunctionCode.PRINTDATADW)
                                || (currentBaseFunc.FunctionCode == FunctionCode.PRINTDATAK)
                                || (currentBaseFunc.FunctionCode == FunctionCode.PRINTDATAKL)
                                || (currentBaseFunc.FunctionCode == FunctionCode.PRINTDATAKW)
                                || (currentBaseFunc.FunctionCode == FunctionCode.STRDATA)
                                || (currentBaseFunc.FunctionCode == FunctionCode.DATALIST)
                                || (currentBaseFunc.FunctionCode == FunctionCode.TRYCALLLIST)
                                || (currentBaseFunc.FunctionCode == FunctionCode.TRYJUMPLIST)
                                || (currentBaseFunc.FunctionCode == FunctionCode.TRYGOTOLIST))
                                //|| (currentBaseFunc.FunctionCode == FunctionCode.SELECTCASE))
                            {
                                ParserMediator.Warn(string.Format(trerror.CanNotLabelDefineInSyntax.Text, currentBaseFunc.Function.Name), nextLine, 2, true, false);
                            }
                        }
                    }
                    continue;
                }
				InstructionLine func = (InstructionLine)nextLine;
				InstructionLine baseFunc = nestStack.Count == 0 ? null : nestStack.Peek();
				if (baseFunc != null)
				{
					if ((baseFunc.Function.IsPrintData() || baseFunc.FunctionCode == FunctionCode.STRDATA) )
					{
						if ((func.FunctionCode != FunctionCode.DATA) && (func.FunctionCode != FunctionCode.DATAFORM) && (func.FunctionCode != FunctionCode.DATALIST)
							&& (func.FunctionCode != FunctionCode.ENDLIST) && (func.FunctionCode != FunctionCode.ENDDATA))
						{
							ParserMediator.Warn(string.Format(trerror.InvalidInstructionInSyntax.Text, baseFunc.Function.Name, func.Function.Name), func, 2, true, false);
							continue;
						}
					}
					else if (baseFunc.FunctionCode == FunctionCode.DATALIST)
					{
						if ((func.FunctionCode != FunctionCode.DATA) && (func.FunctionCode != FunctionCode.DATAFORM) && (func.FunctionCode != FunctionCode.ENDLIST))
						{
							ParserMediator.Warn(string.Format(trerror.InvalidInstructionInSyntax.Text, "DATALIST", func.Function.Name), func, 2, true, false);
							continue;
						}
					}
					else if ((baseFunc.FunctionCode == FunctionCode.TRYCALLLIST) || (baseFunc.FunctionCode == FunctionCode.TRYJUMPLIST) || (baseFunc.FunctionCode == FunctionCode.TRYGOTOLIST))
					{
						if ((func.FunctionCode != FunctionCode.FUNC) && (func.FunctionCode != FunctionCode.ENDFUNC))
						{
							ParserMediator.Warn(string.Format(trerror.InvalidInstructionInSyntax.Text, baseFunc.Function.Name, func.Function.Name), func, 2, true, false);
							continue;
						}
					}
					else if (baseFunc.FunctionCode == FunctionCode.SELECTCASE)
					{
						if ((baseFunc.IfCaseList.Count == 0) && (func.FunctionCode != FunctionCode.CASE) && (func.FunctionCode != FunctionCode.CASEELSE) && (func.FunctionCode != FunctionCode.ENDSELECT))
						{
							ParserMediator.Warn(string.Format(trerror.OutsideSelectcase.Text, func.Function.Name), func, 2, true, false);
							continue;
						}
					}
				}
				switch (func.FunctionCode)
				{
					case FunctionCode.REPEAT:
						if (GlobalStatic.IdentifierDictionary.getVarTokenIsForbid("COUNT"))
						{
							ParserMediator.Warn(trerror.CanNotUseRepeat.Text, func, 1, false, false);
						}
						foreach (InstructionLine iLine in nestStack)
						{
							if (iLine.FunctionCode == FunctionCode.REPEAT)
							{
								ParserMediator.Warn(trerror.NestedRepeat.Text, func, 1, false, false);
							}
                            else if (iLine.FunctionCode == FunctionCode.FOR)
                            {
                                VariableTerm cnt = ((SpForNextArgment)iLine.Argument).Cnt;
                                if (cnt.Identifier.Name == "COUNT" && (cnt.isAllConst && cnt.getEl1forArg == 0))
                                {
                                    ParserMediator.Warn(string.Format(trerror.RepeatInsideFor.Text, "0"), func, 1, false, false);
                                }
                            }
                        }
                        if (func.IsError)
                            break;
						nestStack.Push(func);
						break;
					case FunctionCode.IF:
						nestStack.Push(func);
                        func.IfCaseList = new List<InstructionLine>
                        {
                            func
                        };
                        break;
					case FunctionCode.SELECTCASE:
						nestStack.Push(func);
						func.IfCaseList = new List<InstructionLine>();
                        SelectcaseStack.Push(func);
						break;
					case FunctionCode.FOR:
                        //ネストエラーチェックのためにコストはかかるが、ここでチェックする
                        if (func.Argument == null)
                            ArgumentParser.SetArgumentTo(func);
                        //上で引数解析がなされていることは保証されているので、
                        //それでこれがfalseになるのは、引数解析でエラーが起きた場合のみ
                        if (func.Argument != null)
                        {
                            VariableTerm Cnt = ((SpForNextArgment)func.Argument).Cnt;
                            if (Cnt.Identifier.Name == "COUNT")
                            {
                                foreach (InstructionLine iLine in nestStack)
                                {
                                    if (iLine.FunctionCode == FunctionCode.REPEAT && (Cnt.isAllConst && Cnt.getEl1forArg == 0))
                                    {
                                        ParserMediator.Warn(string.Format(trerror.RepeatInsideFor.Text, "0"), func, 1, false, false);
                                    }
                                    else if (iLine.FunctionCode == FunctionCode.FOR)
                                    {
                                        VariableTerm destCnt = ((SpForNextArgment)iLine.Argument).Cnt;
                                        if (destCnt.Identifier.Name == "COUNT" && (Cnt.isAllConst && destCnt.isAllConst && destCnt.getEl1forArg == Cnt.getEl1forArg))
                                        {
											ParserMediator.Warn(string.Format(trerror.RepeatInsideFor.Text, Cnt.getEl1forArg.ToString()), func, 1, false, false);
                                        }
                                    }
                                }
                            }
                        }
                        if (func.IsError)
                            break;
                        nestStack.Push(func);
                        break;
                    case FunctionCode.WHILE:
					case FunctionCode.TRYCGOTO:
					case FunctionCode.TRYCJUMP:
					case FunctionCode.TRYCCALL:
					case FunctionCode.TRYCGOTOFORM:
					case FunctionCode.TRYCJUMPFORM:
					case FunctionCode.TRYCCALLFORM:
					case FunctionCode.DO:
						nestStack.Push(func);
						break;
					case FunctionCode.BREAK:
					case FunctionCode.CONTINUE:
						InstructionLine[] array = nestStack.ToArray();
						for (int i = 0; i < array.Length; i++)
						{
							if ((array[i].FunctionCode == FunctionCode.REPEAT)
								|| (array[i].FunctionCode == FunctionCode.FOR)
								|| (array[i].FunctionCode == FunctionCode.WHILE)
								|| (array[i].FunctionCode == FunctionCode.DO))
							{
								pairLine = array[i];
								break;
							}
						}
						if (pairLine == null)
						{
							ParserMediator.Warn(string.Format(trerror.InvalidLoopInstruction.Text, func.Function.Name), func, 2, true, false);
							break;
						}
						func.JumpTo = pairLine;
						break;

					case FunctionCode.ELSEIF:
					case FunctionCode.ELSE:
						{
							//1.725 Stack<T>.Peek()はStackが空の時はnullを返す仕様だと思いこんでおりました。
							InstructionLine ifLine = nestStack.Count == 0 ? null : nestStack.Peek();
							if ((ifLine == null) || (ifLine.FunctionCode != FunctionCode.IF))
							{
								ParserMediator.Warn(string.Format(trerror.InvalidElse.Text, func.Function.Name), func, 2, true, false);
                                break;
							}
							if (ifLine.IfCaseList[ifLine.IfCaseList.Count - 1].FunctionCode == FunctionCode.ELSE)
								ParserMediator.Warn(string.Format(trerror.InvalidElseAfterElse.Text, func.Function.Name), func, 1, false, false);
							ifLine.IfCaseList.Add(func);
						}
						break;
					case FunctionCode.ENDIF:
						{
							InstructionLine ifLine = nestStack.Count == 0 ? null : nestStack.Peek();
							if ((ifLine == null) || (ifLine.FunctionCode != FunctionCode.IF))
							{
								ParserMediator.Warn(trerror.UnexpectedEndif.Text, func, 2, true, false);
								break;
							}
							foreach (InstructionLine ifelseifLine in ifLine.IfCaseList)
							{
								ifelseifLine.JumpTo = func;
							}
							nestStack.Pop();
						}
						break;
					case FunctionCode.CASE:
					case FunctionCode.CASEELSE:
						{
							InstructionLine selectLine = nestStack.Count == 0 ? null : nestStack.Peek();
							if ((selectLine == null) || (selectLine.FunctionCode != FunctionCode.SELECTCASE && SelectcaseStack.Count == 0))
							{
								ParserMediator.Warn(string.Format(trerror.OutsideSelectcase.Text, func.Function.Name), func, 2, true, false);
								break;
							}
                            else if (selectLine.FunctionCode != FunctionCode.SELECTCASE && SelectcaseStack.Count > 0)
                            {
                                do
                                {
                                    ParserMediator.Warn(string.Format(trerror.InstructionNotClosed.Text, selectLine.Function.Name, FunctionIdentifier.getMatchFunction(selectLine.FunctionCode), func.Function.Name), func, 2, true, false);
                                    //これを跨いでIF等が閉じられることがないようにする。
                                    nestStack.Pop();
                                    //if (nestStack.Count > 0)　//空になってるかは下で判定できるので、これを見る必要がない
                                    selectLine = nestStack.Count == 0 ? null : nestStack.Peek(); //ちなみにnullになることはない（SELECTCASEがない場合は上で弾けるから）
                                } while (selectLine != null && selectLine.FunctionCode != FunctionCode.SELECTCASE);
                                break;
                            }
							if ((selectLine.IfCaseList.Count > 0) &&
								(selectLine.IfCaseList[selectLine.IfCaseList.Count - 1].FunctionCode == FunctionCode.CASEELSE))
								ParserMediator.Warn(string.Format(trerror.InvalidCaseAfterCaseelse.Text, func.Function.Name), func, 1, false, false);
							selectLine.IfCaseList.Add(func);
						}
						break;
					case FunctionCode.ENDSELECT:
						{
							InstructionLine selectLine = nestStack.Count == 0 ? null : nestStack.Peek();
							if ((selectLine == null) || (selectLine.FunctionCode != FunctionCode.SELECTCASE && SelectcaseStack.Count == 0))
							{
								ParserMediator.Warn(trerror.UnexpectedEndselect.Text, func, 2, true, false);
                                break;
							}
                            else if (selectLine.FunctionCode != FunctionCode.SELECTCASE && SelectcaseStack.Count > 0)
                            {
                                do
                                {
                                    ParserMediator.Warn(string.Format(trerror.InstructionNotClosed.Text, selectLine.Function.Name, FunctionIdentifier.getMatchFunction(selectLine.FunctionCode), func.Function.Name), func, 2, true, false);
                                    //これを跨いでIF等が閉じられることがないようにする。
                                    nestStack.Pop();
                                    //if (nestStack.Count > 0)　//空になってるかは下で判定できるので、これを見る必要がない
                                    selectLine = nestStack.Count == 0 ? null : nestStack.Peek(); //ちなみにnullになることはない（SELECTCASEがない場合は上で弾けるから）
                                } while (selectLine != null && selectLine.FunctionCode != FunctionCode.SELECTCASE);　
                                //とりあえず、対応するSELECTCASE跨ぎは閉じる
                                SelectcaseStack.Pop();
                                //こっちでも抜かないとSELECTCASEが2つのENDSELECTに対応してしまう
                                nestStack.Pop();
                                break;
                            }
                            nestStack.Pop();
                            SelectcaseStack.Pop();
							selectLine.JumpTo = func;
							if (selectLine.IsError)
								break;
							IOperandTerm term = ((ExpressionArgument)selectLine.Argument).Term;
							if (term == null)
							{
								ParserMediator.Warn(trerror.MissingArg.Text, selectLine, 2, true, false);
								break;
							}
							foreach (InstructionLine caseLine in selectLine.IfCaseList)
							{
								caseLine.JumpTo = func;
								if (caseLine.IsError)
									continue;
								if (caseLine.FunctionCode == FunctionCode.CASEELSE)
									continue;
								CaseExpression[] caseExps = ((CaseArgument)caseLine.Argument).CaseExps;
								if (caseExps.Length == 0)
									ParserMediator.Warn(trerror.MissingArg.Text, caseLine, 2, true, false);

								foreach (CaseExpression exp in caseExps)
								{
									if (exp.GetOperandType() != term.GetOperandType())
										ParserMediator.Warn(trerror.NotMatchCaseTypeAndSelectcaseType.Text, caseLine, 2, true, false);
								}

							}
						}
						break;
					case FunctionCode.REND:
					case FunctionCode.NEXT:
					case FunctionCode.WEND:
					case FunctionCode.LOOP:
						FunctionCode parentFunc = FunctionIdentifier.getParentFunc(func.FunctionCode);
						//if (parentFunc == FunctionCode.__NULL__)
						//    throw new ExeEE("何か変？");
						if ((nestStack.Count == 0)
							|| (nestStack.Peek().FunctionCode != parentFunc))
						{
                            ParserMediator.Warn(string.Format(trerror.MissingCorresponding.Text, parentFunc.ToString(), func.Function.Name), func, 2, true, false);
							break;
						}
						pairLine = nestStack.Pop();//REPEAT
						func.JumpTo = pairLine;
						pairLine.JumpTo = func;
						break;
					case FunctionCode.CATCH:
						pairLine = nestStack.Count == 0 ? null : nestStack.Peek();
						if ((pairLine == null)
							|| ((pairLine.FunctionCode != FunctionCode.TRYCGOTO)
							&& (pairLine.FunctionCode != FunctionCode.TRYCCALL)
							&& (pairLine.FunctionCode != FunctionCode.TRYCJUMP)
							&& (pairLine.FunctionCode != FunctionCode.TRYCGOTOFORM)
							&& (pairLine.FunctionCode != FunctionCode.TRYCCALLFORM)
							&& (pairLine.FunctionCode != FunctionCode.TRYCJUMPFORM)))
						{
							ParserMediator.Warn(trerror.MissingTryc.Text, func, 2, true, false);
							break;
						}
						pairLine = nestStack.Pop();//TRYC
						pairLine.JumpToEndCatch = func;//TRYCにCATCHの位置を教える
						nestStack.Push(func);
						break;
					case FunctionCode.ENDCATCH:
						if ((nestStack.Count == 0)
							|| (nestStack.Peek().FunctionCode != FunctionCode.CATCH))
						{
							ParserMediator.Warn(trerror.UnexpectedEndcatch.Text, func, 2, true, false);
							break;
						}
						pairLine = nestStack.Pop();//CATCH
						pairLine.JumpToEndCatch = func;//CATCHにENDCATCHの位置を教える
						break;
                    case FunctionCode.PRINTDATA:
                    case FunctionCode.PRINTDATAL:
                    case FunctionCode.PRINTDATAW:
                    case FunctionCode.PRINTDATAD:
                    case FunctionCode.PRINTDATADL:
                    case FunctionCode.PRINTDATADW:
                    case FunctionCode.PRINTDATAK:
                    case FunctionCode.PRINTDATAKL:
                    case FunctionCode.PRINTDATAKW:
                        {
                            foreach (InstructionLine iLine in nestStack)
                            {
                                if (iLine.Function.IsPrintData())
                                {
                                    ParserMediator.Warn(trerror.NestedPrintdata.Text, func, 2, true, false);
                                    break;
                                }
                                if (iLine.FunctionCode == FunctionCode.STRDATA)
                                {
                                    ParserMediator.Warn(trerror.StrdataInsidePrintdata.Text, func, 2, true, false);
                                    break;
                                }
                            }
                            if (func.IsError)
                                break;
                            func.dataList = new List<List<InstructionLine>>();
                            nestStack.Push(func);
                            break;
                        }
                    case FunctionCode.STRDATA:
                        {
                            foreach (InstructionLine iLine in nestStack)
                            {
                                if (iLine.FunctionCode == FunctionCode.STRDATA)
                                {
                                    ParserMediator.Warn(trerror.NestedStrdata.Text, func, 2, true, false);
                                    break;
                                }
                                if (iLine.Function.IsPrintData())
                                {
                                    ParserMediator.Warn(trerror.PrintdataInsideStrdata.Text, func, 2, true, false);
                                    break;
                                }
                            }
                            if (func.IsError)
                                break;
                            func.dataList = new List<List<InstructionLine>>();
                            nestStack.Push(func);
                            break;
                        }
                    case FunctionCode.DATALIST:
                        {
                            InstructionLine pline = (nestStack.Count == 0) ? null : nestStack.Peek();
                            if ((pline == null) || ((!pline.Function.IsPrintData()) && (pline.FunctionCode != FunctionCode.STRDATA)))
                            {
                                ParserMediator.Warn(trerror.UnexpectedDatalist.Text, func, 2, true, false);
                                break;
                            }
                            tempLineList = new List<InstructionLine>();
                            nestStack.Push(func);

                            break;
                        }
                    case FunctionCode.ENDLIST:
                        {
                            if ((nestStack.Count == 0) || (nestStack.Peek().FunctionCode != FunctionCode.DATALIST))
                            {
                                ParserMediator.Warn(trerror.UnexpectedEndlist.Text, func, 2, true, false);
                                break;
                            }
                            if (tempLineList.Count == 0)
                                ParserMediator.Warn(trerror.DatalistDataIsMissing.Text, func, 1, false, false);
                            nestStack.Pop();
                            nestStack.Peek().dataList.Add(tempLineList);
                            break;
                        }
                    case FunctionCode.DATA:
                    case FunctionCode.DATAFORM:
                        {
                            InstructionLine pdata = (nestStack.Count == 0) ? null : nestStack.Peek();
                            if ((pdata == null) || (!pdata.Function.IsPrintData() && pdata.FunctionCode != FunctionCode.DATALIST && pdata.FunctionCode != FunctionCode.STRDATA))
                            {
                                ParserMediator.Warn(string.Format(trerror.MissingPrintdata.Text, func.Function.Name), func, 2, true, false);
                                break;
                            }
                            List<InstructionLine> iList = new List<InstructionLine>();
                            if (pdata.FunctionCode != FunctionCode.DATALIST)
                            {
                                iList.Add(func);
                                pdata.dataList.Add(iList);
                            }
                            else
                                tempLineList.Add(func);
                            break;
                        }
                    case FunctionCode.ENDDATA:
                        {
                            InstructionLine pline = (nestStack.Count == 0) ? null : nestStack.Peek();
                            if ((pline == null) || ((!pline.Function.IsPrintData()) && (pline.FunctionCode != FunctionCode.STRDATA)))
                            {
                                ParserMediator.Warn(string.Format(trerror.MissingPrintdataStrdata.Text, func.Function.Name), func, 2, true, false);
                                break;
                            }
                            if (pline.FunctionCode == FunctionCode.DATALIST)
                                ParserMediator.Warn(trerror.DatalistNotClosed.Text, func, 2, true, false);
                            if (pline.dataList.Count == 0)
                                ParserMediator.Warn(string.Format(trerror.InstructionDataIsMissing.Text, pline.Function.Name), func, 1, false, false);
                            pline.JumpTo = func;
                            nestStack.Pop();
                            break;
                        }
					case FunctionCode.TRYCALLLIST:
					case FunctionCode.TRYJUMPLIST:
					case FunctionCode.TRYGOTOLIST:
						foreach (InstructionLine iLine in nestStack)
						{
							if (iLine.FunctionCode == FunctionCode.TRYCALLLIST || iLine.FunctionCode == FunctionCode.TRYJUMPLIST || iLine.FunctionCode == FunctionCode.TRYGOTOLIST)
							{
								ParserMediator.Warn(trerror.NestedTrycalllist.Text, func, 2, true, false);
								break;
							}
						}
						if (func.IsError)
							break;
						func.callList = new List<InstructionLine>();
						nestStack.Push(func);
						break;
					case FunctionCode.FUNC:
						{
							InstructionLine pFunc = (nestStack.Count == 0) ? null : nestStack.Peek();
							if ((pFunc == null) ||
								(pFunc.FunctionCode != FunctionCode.TRYCALLLIST && pFunc.FunctionCode != FunctionCode.TRYJUMPLIST && pFunc.FunctionCode != FunctionCode.TRYGOTOLIST))
							{
								ParserMediator.Warn(string.Format(trerror.MissingTrycalllist.Text, func.Function.Name), func, 2, true, false);
								break;
							}
                            if (func.Argument == null)
                            {
                                ParserMediator.Warn(string.Format(trerror.InvalidInstructionInTrycalllist.Text, func.Function.Name), pFunc, 2, true, false);
                                break;
                            }
							if (pFunc.FunctionCode == FunctionCode.TRYGOTOLIST)
							{
								if (((SpCallArgment)func.Argument).SubNames.Length != 0)
								{
									ParserMediator.Warn(trerror.TrygotolistToSBrackets.Text, func, 2, true, false);
									break;
								}
								if (((SpCallArgment)func.Argument).RowArgs.Length != 0)
								{
									ParserMediator.Warn(trerror.TrygotolistTargetHasArg.Text, func, 2, true, false);
									break;
								}
							}
							pFunc.callList.Add(func);
							break;
						}
					case FunctionCode.ENDFUNC:
						InstructionLine pf = (nestStack.Count == 0) ? null : nestStack.Peek();
						if ((pf == null) ||
							(pf.FunctionCode != FunctionCode.TRYCALLLIST && pf.FunctionCode != FunctionCode.TRYJUMPLIST && pf.FunctionCode != FunctionCode.TRYGOTOLIST))
						{
							ParserMediator.Warn(string.Format(trerror.MissingTrycalllist.Text, func.Function.Name), func, 2, true, false);
							break;
						}
						pf.JumpTo = func;
						nestStack.Pop();
						break;
					case FunctionCode.NOSKIP:
						foreach (InstructionLine iLine in nestStack)
						{
							if (iLine.FunctionCode == FunctionCode.NOSKIP)
							{
								ParserMediator.Warn(trerror.NestedNoskip.Text, func, 2, true, false);
								break;
							}
						}
						if (func.IsError)
							break;
						nestStack.Push(func);
						break;
					case FunctionCode.ENDNOSKIP:
						InstructionLine pfunc = (nestStack.Count == 0) ? null : nestStack.Peek();
						if ((pfunc == null) ||
							(pfunc.FunctionCode != FunctionCode.NOSKIP))
						{
							ParserMediator.Warn(string.Format(trerror.MissingNoskip.Text, func.Function.Name), func, 2, true, false);
							break;
						}
						//エラーハンドリング用
						pfunc.JumpTo = func;
						func.JumpTo = pfunc;
						nestStack.Pop();
						break;
				}

			}

			while (nestStack.Count != 0)
			{
				InstructionLine func = nestStack.Pop();
				string funcName = func.Function.Name;
				string funcMatch = FunctionIdentifier.getMatchFunction(func.FunctionCode);
				if (func != null)
					ParserMediator.Warn(string.Format(trerror.MissingCorresponding.Text, funcMatch, funcName), func, 2, true, false);
				else
					ParserMediator.Warn(trerror.DefaultError.Text, func, 2, true, false);
			}
            //使ったスタックをクリア
            SelectcaseStack.Clear();
		}

		private void setJumpTo(FunctionLabelLine label)
		{
			//3周目/3周
			//フロー制御命令のジャンプ先を設定
			LogicalLine nextLine = label;
			int depth = label.Depth;
			if (depth < 0)
				depth = -2;
			while (true)
			{
				nextLine = nextLine.NextLine;
                if (!(nextLine is InstructionLine func))
                {
                    if ((nextLine is NullLine) || (nextLine is FunctionLabelLine))
                        break;
                    continue;
                }
                if (func.IsError)
					continue;
				parentProcess.scaningLine = func;

				if (func.Function.Instruction != null)
				{
					string FunctionNotFoundName = null;
					try
					{
						func.Function.Instruction.SetJumpTo(ref useCallForm, func, depth, ref FunctionNotFoundName);
					}
					catch (CodeEE e)
					{
						ParserMediator.Warn(e.Message, func, 2, true, false);
						continue;
					}
					if (FunctionNotFoundName != null)
					{
						if (!Program.AnalysisMode)
							printFunctionNotFoundWarning(string.Format(trerror.NotDefinedFunc.Text, FunctionNotFoundName), func, 2, true);
						else
							printFunctionNotFoundWarning(FunctionNotFoundName, func, 2, true);
					}
                    continue;
				}
				if ((func.FunctionCode == FunctionCode.TRYCALLLIST) || (func.FunctionCode == FunctionCode.TRYJUMPLIST))
					useCallForm = true;
			}
		}

	}
}
