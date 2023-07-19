using System;
using System.Collections.Generic;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.GameProc.Function;
using MinorShift._Library;
using MinorShift.Emuera.GameData;
using MinorShift.Emuera.GameData.Function;
using XEmuera;
using System.IO;
using trsl = EvilMask.Emuera.Lang.SystemLine;
using trerror = EvilMask.Emuera.Lang.Error;

namespace MinorShift.Emuera.GameProc
{
	internal sealed class HeaderFileLoader
	{
		public HeaderFileLoader(EmueraConsole main, IdentifierDictionary idDic, Process proc)
		{
			output = main;
			parentProcess = proc;
			this.idDic = idDic;
		}
		readonly Process parentProcess;
		readonly EmueraConsole output;
		readonly IdentifierDictionary idDic;

		bool noError = true;
		Queue<DimLineWC> dimlines;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="erbDir"></param>
		/// <param name="displayReport"></param>
		/// <returns></returns>
		public bool LoadHeaderFiles(string headerDir, bool displayReport)
		{
			List<KeyValuePair<string, string>> headerFiles = Config.GetFiles(headerDir, "*.ERH");
			bool noError = true;
			dimlines = new Queue<DimLineWC>();
			#region EE_ERD
			//ERD読み込み
			if (Config.UseERD)
				PrepareERDFileNames();
			#endregion
			try
			{
				for (int i = 0; i < headerFiles.Count; i++)
				{
					string filename = headerFiles[i].Key;
					string file = headerFiles[i].Value;
					if (displayReport)
						output.PrintSystemLine(string.Format(trsl.LoadingFile.Text, filename));
					noError = loadHeaderFile(file, filename);
					if (!noError)
						break;
					App.DoEvents();
				}	
				//エラーが起きてる場合でも読み込めてる分だけはチェックする
				if (dimlines.Count > 0)
				{
					//&=でないと、ここで起きたエラーをキャッチできない
					noError &= analyzeSharpDimLines();
				}

				dimlines.Clear();
			}
			finally
			{
				ParserMediator.FlushWarningList();
				#region EE_ERD
				erdFileNames = null;
				#endregion
			}
			return noError;
		}


		private bool loadHeaderFile(string filepath, string filename)
		{
			StringStream st;
			ScriptPosition position = null;
			//EraStreamReader eReader = new EraStreamReader(false);
			//1815修正 _rename.csvの適用
			//eramakerEXの仕様的には.ERHに適用するのはおかしいけど、もうEmueraの仕様になっちゃってるのでしかたないか
			EraStreamReader eReader = new EraStreamReader(true);

			if (!eReader.Open(filepath, filename))
			{
				throw new CodeEE(string.Format(trerror.FailedOpenFile.Text, eReader.Filename));
				//return false;
			}
			try
			{
				while ((st = eReader.ReadEnabledLine()) != null)
				{
					if (!noError)
						return false;
					position = new ScriptPosition(filename, eReader.LineNo);
					LexicalAnalyzer.SkipWhiteSpace(st);
					if (st.Current != '#')
						throw new CodeEE(trerror.NotStartedSharpLineInHeader.Text, position);
					st.ShiftNext();
					string sharpID = LexicalAnalyzer.ReadSingleIdentifier(st);
					if (sharpID == null)
					{
						ParserMediator.Warn(trerror.CanNotInterpretSharpLine.Text, position, 1);
						return false;
					}
					if (Config.ICFunction)
						sharpID = sharpID.ToUpper();
					LexicalAnalyzer.SkipWhiteSpace(st);
					switch (sharpID)
					{
						case "DEFINE":
							analyzeSharpDefine(st, position);
							break;
						case "FUNCTION":
						case "FUNCTIONS":
							analyzeSharpFunction(st, position, sharpID == "FUNCTIONS");
							break;
						case "DIM":
						case "DIMS":
							//1822 #DIMは保留しておいて後でまとめてやる
							{
								WordCollection wc = LexicalAnalyzer.Analyse(st, LexEndWith.EoL, LexAnalyzeFlag.AllowAssignment);
								dimlines.Enqueue(new DimLineWC(wc, sharpID == "DIMS", false, position));
							}
							//analyzeSharpDim(st, position, sharpID == "DIMS");
							break;
						default:
							throw new CodeEE(string.Format(trerror.UnknownPreprocessorInSharpLine.Text, sharpID), position);
					}
				}
			}
			catch (CodeEE e)
			{
				if (e.Position != null)
					position = e.Position;
				ParserMediator.Warn(e.Message, position, 2);
				return false;
			}
			finally
			{
				eReader.Close();
			}
			return true;
		}

		//#define FOO (～～)     id to wc
		//#define BAR($1) (～～)     idwithargs to wc(replaced)
		//#diseble FOOBAR             
		//#dim piyo, i
		//#dims puyo, j
		//static List<string> keywordsList = new List<string>();

		private void analyzeSharpDefine(StringStream st, ScriptPosition position)
		{
			//LexicalAnalyzer.SkipWhiteSpace(st);呼び出し前に行う。
			string srcID = LexicalAnalyzer.ReadSingleIdentifier(st);
			if (srcID == null)
				throw new CodeEE(trerror.MissingReplacementSource.Text, position);
			if (Config.ICVariable)
				srcID = srcID.ToUpper();

			//ここで名称重複判定しないと、大変なことになる
			string errMes = "";
			int errLevel = -1;
			idDic.CheckUserMacroName(ref errMes, ref errLevel, srcID);
			if (errLevel >= 0)
			{
				ParserMediator.Warn(errMes, position, errLevel);
				if (errLevel >= 2)
				{
					noError = false;
					return;
				}
			}
			
			bool hasArg = st.Current == '(';//引数を指定する場合には直後に(が続いていなければならない。ホワイトスペースも禁止。
			//1808a3 代入演算子許可（関数宣言用）
			WordCollection wc = LexicalAnalyzer.Analyse(st, LexEndWith.EoL, LexAnalyzeFlag.AllowAssignment);
			if (wc.EOL)
			{
				//throw new CodeEE("置換先の式がありません", position);
				//1808a3 空マクロの許可
				DefineMacro nullmac = new DefineMacro(srcID, new WordCollection(), 0);
				idDic.AddMacro(nullmac);
				return;
			}

			List<string> argID = new List<string>();
			if (hasArg)//関数型マクロの引数解析
			{
				wc.ShiftNext();//'('を読み飛ばす
				if (wc.Current.Type == ')')
					throw new CodeEE(trerror.FuncMacroArgIs0.Text, position);
				while (!wc.EOL)
				{
					IdentifierWord word = wc.Current as IdentifierWord;
					if (word == null)
						throw new CodeEE(trerror.WrongFormatReplacementSource.Text, position);
					word.SetIsMacro();
					string id = word.Code;
					if (argID.Contains(id))
						throw new CodeEE(trerror.DuplicateCharacterReplcaementSource.Text, position);
					argID.Add(id);
					wc.ShiftNext();
					if (wc.Current.Type == ',')
					{
						wc.ShiftNext();
						continue;
					}
					if (wc.Current.Type == ')')
						break;
					throw new CodeEE(trerror.WrongFormatReplacementSource.Text, position);
				}
				if (wc.EOL)
					throw new CodeEE(trerror.NotCloseBrackets.Text, position);

				wc.ShiftNext();
			}
			if (wc.EOL)
				throw new CodeEE(trerror.MissingSubstitution.Text, position);
			WordCollection destWc = new WordCollection();
			while (!wc.EOL)
			{
				destWc.Add(wc.Current);
				wc.ShiftNext();
			}
			if (hasArg)//関数型マクロの引数セット
			{
				while (!destWc.EOL)
				{
					IdentifierWord word = destWc.Current as IdentifierWord;
					if (word == null)
					{
						destWc.ShiftNext();
						continue;
					}
					for (int i = 0; i < argID.Count; i++)
					{
						if (string.Equals(word.Code, argID[i], Config.SCVariable))
						{
							destWc.Remove();
							destWc.Insert(new MacroWord(i));
							break;
						}
					}
					destWc.ShiftNext();
				}
				destWc.Pointer = 0;
			}
			if (hasArg)//1808a3 関数型マクロの封印
				throw new CodeEE(trerror.CanNotDeclaredFuncMacro.Text, position);
			DefineMacro mac = new DefineMacro(srcID, destWc, argID.Count);
			idDic.AddMacro(mac);
		}

		//private void analyzeSharpDim(StringStream st, ScriptPosition position, bool dims)
		//{
		//	//WordCollection wc = LexicalAnalyzer.Analyse(st, LexEndWith.EoL, LexAnalyzeFlag.AllowAssignment);
		//	//UserDefinedVariableData data = UserDefinedVariableData.Create(wc, dims, false, position);
		//	//if (data.Reference)
		//	//	throw new NotImplCodeEE();
		//	//VariableToken var = null;
		//	//if (data.CharaData)
		//	//	var = parentProcess.VEvaluator.VariableData.CreateUserDefCharaVariable(data);
		//	//else
		//	//	var = parentProcess.VEvaluator.VariableData.CreateUserDefVariable(data);
		//	//idDic.AddUseDefinedVariable(var);
		//}

		//1822 #DIMだけまとめておいて後で処理
		private bool analyzeSharpDimLines()
		{
			bool noError = true;
			bool tryAgain = true;
			while (dimlines.Count > 0)
			{
				int count = dimlines.Count;
				for (int i = 0; i < count; i++)
				{
					DimLineWC dimline = dimlines.Dequeue();
					try
					{
						UserDefinedVariableData data = UserDefinedVariableData.Create(dimline);
						if (data.Reference)
							throw new NotImplCodeEE();
						VariableToken var = null;
						if (data.CharaData)
							var = parentProcess.VEvaluator.VariableData.CreateUserDefCharaVariable(data);
						else
							var = parentProcess.VEvaluator.VariableData.CreateUserDefVariable(data);
						idDic.AddUseDefinedVariable(var);
						#region EE_ERD
						if (Config.UseERD)
						{
							string key;
							if (data.Dimension == 1)
                            {
								key = data.Name.ToUpper();
								if (erdFileNames.ContainsKey(key))
								{
									var info = erdFileNames[key];
									GlobalStatic.ConstantData.UserDefineLoadData(info, data.Name, data.Lengths[0], Config.DisplayReport, dimline.SC);
								}
								App.DoEvents();
							}
							else if (data.Dimension == 2)
                            {
								for (int dim = 1; dim < 3; dim++)
								{
									key = data.Name.ToUpper() + "@" + dim;
									if (erdFileNames.ContainsKey(key))
									{
										var info = erdFileNames[key];
										GlobalStatic.ConstantData.UserDefineLoadData(info, data.Name + "@" + dim, data.Lengths[dim-1], Config.DisplayReport, dimline.SC);
									}
									App.DoEvents();
								}
							}
							else if (data.Dimension == 3)
							{
								for (int dim = 1; dim < 4; dim++)
								{
									key = data.Name.ToUpper() + "@" + dim;
									if (erdFileNames.ContainsKey(key))
									{
										var info = erdFileNames[key];
										GlobalStatic.ConstantData.UserDefineLoadData(info, data.Name + "@" + dim, data.Lengths[dim-1], Config.DisplayReport, dimline.SC);
									}
									App.DoEvents();
								}
							}
						}
						#endregion

					}
					catch (IdentifierNotFoundCodeEE e)
					{
						//繰り返すことで解決する見込みがあるならキューの最後に追加
						if (tryAgain)
						{
							dimline.WC.Pointer = 0;
							dimlines.Enqueue(dimline);
						}
						else
						{
							ParserMediator.Warn(e.Message, dimline.SC, 2);
							noError = true;
						}
					}
					catch (CodeEE e)
					{
						ParserMediator.Warn(e.Message, dimline.SC, 2);
						noError = false;
					}
				}
				if (dimlines.Count == count)
					tryAgain = false;
			}
			return noError;
		}
		#region EE_ERD

		private Dictionary<string, List<string>> erdFileNames;

		private void PrepareERDFileNames()
		{
			if (erdFileNames == null) erdFileNames = new Dictionary<string, List<string>>();
			foreach (var path in FileUtils.GetFiles(Program.ErbDir, "*.erd", SearchOption.AllDirectories))
			{
				var key = Path.GetFileNameWithoutExtension(path).ToUpper();
				if (!erdFileNames.ContainsKey(key))
					erdFileNames[key] = new List<string> { path };
				else
					erdFileNames[key].Add(path);
			}
			foreach (var path in FileUtils.GetFiles(Program.CsvDir, "*.csv", SearchOption.TopDirectoryOnly))
			{
				var key = Path.GetFileNameWithoutExtension(path).ToUpper();
				if (!erdFileNames.ContainsKey(key))
					erdFileNames[key] = new List<string> { path };
				else
					erdFileNames[key].Add(path);
			}
		}
		#endregion
		private void analyzeSharpFunction(StringStream st, ScriptPosition position, bool funcs)
		{
			throw new NotImplCodeEE();
			//WordCollection wc = LexicalAnalyzer.Analyse(st, LexEndWith.EoL, LexAnalyzeFlag.AllowAssignment);
			//UserDefinedFunctionData data = UserDefinedFunctionData.Create(wc, funcs, position);
			//idDic.AddRefMethod(UserDefinedRefMethod.Create(data));
		}
	}
}
