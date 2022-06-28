using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using MinorShift.Emuera.Sub;
using System.Text.RegularExpressions;
using MinorShift.Emuera.GameData.Expression;
using System.Linq;
using XEmuera;
using XEmuera.Models;

namespace MinorShift.Emuera
{
	/// <summary>
	/// プログラム全体で使用される値でWindow作成前に設定して以後変更されないもの
	/// (という予定だったが今は違う)
	/// 1756 Config → ConfigDataへ改名
	/// </summary>
	internal sealed class ConfigData
	{
		static string configPath;
		static string configdebugPath;

		static ConfigData() { }
		private static ConfigData instance = new ConfigData();
		public static ConfigData Instance { get { return instance; } }

		private ConfigData() { SetDefault(); }

		//適当に大き目の配列を作っておく。
		private List<ConfigItem> configArray = new List<ConfigItem>();
		private List<ConfigItem> replaceArray = new List<ConfigItem>();
		private List<ConfigItem> debugArray = new List<ConfigItem>();

		private void SetDefault()
		{
			int i = 0;
			configArray.Add(new ConfigItem(ConfigCode.IgnoreCase, "大文字小文字の違いを無視する", true));
			configArray.Add(new ConfigItem(ConfigCode.UseRenameFile, "_Rename.csvを利用する", false));
			configArray.Add(new ConfigItem(ConfigCode.UseReplaceFile, "_Replace.csvを利用する", true));
			configArray.Add(new ConfigItem(ConfigCode.UseMouse, "マウスを使用する", true));
			configArray.Add(new ConfigItem(ConfigCode.UseMenu, "メニューを使用する", true));
			configArray.Add(new ConfigItem(ConfigCode.UseDebugCommand, "デバッグコマンドを使用する", false));
			configArray.Add(new ConfigItem(ConfigCode.AllowMultipleInstances, "多重起動を許可する", true));
			configArray.Add(new ConfigItem(ConfigCode.AutoSave, "オートセーブを行なう", true));
			configArray.Add(new ConfigItem(ConfigCode.UseKeyMacro, "キーボードマクロを使用する", true));
			configArray.Add(new ConfigItem(ConfigCode.SizableWindow, "ウィンドウの高さを可変にする", true));
			configArray.Add(new ConfigItem(ConfigCode.TextDrawingMode, "描画インターフェース", TextDrawingMode.GRAPHICS));
			//configArray.Add(new ConfigItem(ConfigCode.UseImageBuffer, "イメージバッファを使用する", true));
			configArray.Add(new ConfigItem(ConfigCode.WindowX, "ウィンドウ幅", 760));
			configArray.Add(new ConfigItem(ConfigCode.WindowY, "ウィンドウ高さ", 480));
			configArray.Add(new ConfigItem(ConfigCode.WindowPosX, "ウィンドウ位置X", 0));
			configArray.Add(new ConfigItem(ConfigCode.WindowPosY, "ウィンドウ位置Y", 0));
			configArray.Add(new ConfigItem(ConfigCode.SetWindowPos, "起動時のウィンドウ位置を指定する", false));
			configArray.Add(new ConfigItem(ConfigCode.WindowMaximixed, "起動時にウィンドウを最大化する", false));
			configArray.Add(new ConfigItem(ConfigCode.MaxLog, "履歴ログの行数", 5000));
			configArray.Add(new ConfigItem(ConfigCode.PrintCPerLine, "PRINTCを並べる数", 3));
			configArray.Add(new ConfigItem(ConfigCode.PrintCLength, "PRINTCの文字数", 25));
			//configArray.Add(new ConfigItem(ConfigCode.FontName, "フォント名", "ＭＳ ゴシック"));
			configArray.Add(new ConfigItem(ConfigCode.FontName, "フォント名", "MS Gothic"));
			configArray.Add(new ConfigItem(ConfigCode.FontSize, "フォントサイズ", 18));
			configArray.Add(new ConfigItem(ConfigCode.LineHeight, "一行の高さ", 19));
			configArray.Add(new ConfigItem(ConfigCode.ForeColor, "文字色", Color.FromArgb(192, 192, 192)));//LIGHTGRAY
			configArray.Add(new ConfigItem(ConfigCode.BackColor, "背景色", Color.FromArgb(0, 0, 0)));//BLACK
			configArray.Add(new ConfigItem(ConfigCode.FocusColor, "選択中文字色", Color.FromArgb(255, 255, 0)));//YELLOW
			configArray.Add(new ConfigItem(ConfigCode.LogColor, "履歴文字色", Color.FromArgb(192, 192, 192)));//LIGHTGRAY//Color.FromArgb(128, 128, 128));//GRAY
			configArray.Add(new ConfigItem(ConfigCode.FPS, "フレーム毎秒", 5));
			configArray.Add(new ConfigItem(ConfigCode.SkipFrame, "最大スキップフレーム数", 3));
			configArray.Add(new ConfigItem(ConfigCode.ScrollHeight, "スクロール行数", 1));
			configArray.Add(new ConfigItem(ConfigCode.InfiniteLoopAlertTime, "無限ループ警告までのミリ秒数", 5000));
			configArray.Add(new ConfigItem(ConfigCode.DisplayWarningLevel, "表示する最低警告レベル", 1));
			configArray.Add(new ConfigItem(ConfigCode.DisplayReport, "ロード時にレポートを表示する", false));
			configArray.Add(new ConfigItem(ConfigCode.ReduceArgumentOnLoad, "ロード時に引数を解析する", ReduceArgumentOnLoadFlag.NO));
			//configArray.Add(new ConfigItem(ConfigCode.ReduceFormattedStringOnLoad, "ロード時にFORM文字列を解析する", true));
			configArray.Add(new ConfigItem(ConfigCode.IgnoreUncalledFunction, "呼び出されなかった関数を無視する", true));
			configArray.Add(new ConfigItem(ConfigCode.FunctionNotFoundWarning, "関数が見つからない警告の扱い", DisplayWarningFlag.IGNORE));
			configArray.Add(new ConfigItem(ConfigCode.FunctionNotCalledWarning, "関数が呼び出されなかった警告の扱い", DisplayWarningFlag.IGNORE));
			//configArray.Add(new ConfigItem(ConfigCode.IgnoreWarningFiles, "指定したファイル中の警告を無視する", new List<string>()));
			configArray.Add(new ConfigItem(ConfigCode.ChangeMasterNameIfDebug, "デバッグコマンドを使用した時にMASTERの名前を変更する", true));
			configArray.Add(new ConfigItem(ConfigCode.ButtonWrap, "ボタンの途中で行を折りかえさない", false));
			configArray.Add(new ConfigItem(ConfigCode.SearchSubdirectory, "サブディレクトリを検索する", false));
			configArray.Add(new ConfigItem(ConfigCode.SortWithFilename, "読み込み順をファイル名順にソートする", false));
			configArray.Add(new ConfigItem(ConfigCode.LastKey, "最終更新コード", 0L));
			configArray.Add(new ConfigItem(ConfigCode.SaveDataNos, "表示するセーブデータ数", 20));
			configArray.Add(new ConfigItem(ConfigCode.WarnBackCompatibility, "eramaker互換性に関する警告を表示する", true));
			configArray.Add(new ConfigItem(ConfigCode.AllowFunctionOverloading, "システム関数の上書きを許可する", true));
			configArray.Add(new ConfigItem(ConfigCode.WarnFunctionOverloading, "システム関数が上書きされたとき警告を表示する", true));
			configArray.Add(new ConfigItem(ConfigCode.TextEditor, "関連づけるテキストエディタ", "notepad"));
			configArray.Add(new ConfigItem(ConfigCode.EditorType, "テキストエディタコマンドライン指定", TextEditorType.USER_SETTING));
			configArray.Add(new ConfigItem(ConfigCode.EditorArgument, "エディタに渡す行指定引数", ""));
			configArray.Add(new ConfigItem(ConfigCode.WarnNormalFunctionOverloading, "同名の非イベント関数が複数定義されたとき警告する", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiErrorLine, "解釈不可能な行があっても実行する", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiCALLNAME, "CALLNAMEが空文字列の時にNAMEを代入する", false));
			configArray.Add(new ConfigItem(ConfigCode.UseSaveFolder, "セーブデータをsavフォルダ内に作成する", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiRAND, "擬似変数RANDの仕様をeramakerに合わせる", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiDRAWLINE, "DRAWLINEを常に新しい行で行う", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiFunctionNoignoreCase, "関数・属性については大文字小文字を無視しない", false));
			configArray.Add(new ConfigItem(ConfigCode.SystemAllowFullSpace, "全角スペースをホワイトスペースに含める", true));
			configArray.Add(new ConfigItem(ConfigCode.SystemSaveInUTF8, "セーブデータをUTF-8で保存する", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiLinefeedAs1739, "ver1739以前の非ボタン折り返しを再現する", false));
			configArray.Add(new ConfigItem(ConfigCode.useLanguage, "内部で使用する東アジア言語", UseLanguage.JAPANESE));
			configArray.Add(new ConfigItem(ConfigCode.AllowLongInputByMouse, "ONEINPUT系命令でマウスによる2文字以上の入力を許可する", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiCallEvent, "イベント関数のCALLを許可する", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiSPChara, "SPキャラを使用する", false));

			configArray.Add(new ConfigItem(ConfigCode.SystemSaveInBinary, "セーブデータをバイナリ形式で保存する", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiFuncArgOptional, "ユーザー関数の全ての引数の省略を許可する", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiFuncArgAutoConvert, "ユーザー関数の引数に自動的にTOSTRを補完する", false));
			configArray.Add(new ConfigItem(ConfigCode.SystemIgnoreTripleSymbol, "FORM中の三連記号を展開しない", false));
			configArray.Add(new ConfigItem(ConfigCode.TimesNotRigorousCalculation, "TIMESの計算をeramakerにあわせる", false));
			//一文字変数の禁止オプションを考えた名残
			//configArray.Add(new ConfigItem(ConfigCode.ForbidOneCodeVariable, "一文字変数の使用を禁止する", false));
			configArray.Add(new ConfigItem(ConfigCode.SystemNoTarget, "キャラクタ変数の引数を補完しない", false));
			configArray.Add(new ConfigItem(ConfigCode.SystemIgnoreStringSet, "文字列変数の代入に文字列式を強制する", false));

			configArray.Add(new ConfigItem(ConfigCode.QuickButtonColumn, "快捷按钮显示列数", 5));
			configArray.Add(new ConfigItem(ConfigCode.FontScale, "文字和图像放大倍数", 2.0f));
			configArray.Add(new ConfigItem(ConfigCode.PanSpeed, "横向平移速度", 2.0f));

			i = 0;
			debugArray.Add(new ConfigItem(ConfigCode.DebugShowWindow, "起動時にデバッグウインドウを表示する", true));
			debugArray.Add(new ConfigItem(ConfigCode.DebugWindowTopMost, "デバッグウインドウを最前面に表示する", true));
			debugArray.Add(new ConfigItem(ConfigCode.DebugWindowWidth, "デバッグウィンドウ幅", 400));
			debugArray.Add(new ConfigItem(ConfigCode.DebugWindowHeight, "デバッグウィンドウ高さ", 300));
			debugArray.Add(new ConfigItem(ConfigCode.DebugSetWindowPos, "デバッグウィンドウ位置を指定する", false));
			debugArray.Add(new ConfigItem(ConfigCode.DebugWindowPosX, "デバッグウィンドウ位置X", 0));
			debugArray.Add(new ConfigItem(ConfigCode.DebugWindowPosY, "デバッグウィンドウ位置Y", 0));

			i = 0;
			replaceArray.Add(new ConfigItem(ConfigCode.MoneyLabel, "お金の単位", "$"));
			replaceArray.Add(new ConfigItem(ConfigCode.MoneyFirst, "単位の位置", true));
			replaceArray.Add(new ConfigItem(ConfigCode.LoadLabel, "起動時簡略表示", "Now Loading..."));
			replaceArray.Add(new ConfigItem(ConfigCode.MaxShopItem, "販売アイテム数", 100));
			replaceArray.Add(new ConfigItem(ConfigCode.DrawLineString, "DRAWLINE文字", "-"));
			replaceArray.Add(new ConfigItem(ConfigCode.BarChar1, "BAR文字1", '*'));
			replaceArray.Add(new ConfigItem(ConfigCode.BarChar2, "BAR文字2", '.'));
			replaceArray.Add(new ConfigItem(ConfigCode.TitleMenuString0, "システムメニュー0", "最初からはじめる"));
			replaceArray.Add(new ConfigItem(ConfigCode.TitleMenuString1, "システムメニュー1", "ロードしてはじめる"));
			replaceArray.Add(new ConfigItem(ConfigCode.ComAbleDefault, "COM_ABLE初期値", 1));
			replaceArray.Add(new ConfigItem(ConfigCode.StainDefault, "汚れの初期値", new List<Int64>(new Int64[] { 0, 0, 2, 1, 8 })));
			replaceArray.Add(new ConfigItem(ConfigCode.TimeupLabel, "時間切れ表示", "時間切れ"));
			replaceArray.Add(new ConfigItem(ConfigCode.ExpLvDef, "EXPLVの初期値", new List<long>(new Int64[] { 0, 1, 4, 20, 50, 200 })));
			replaceArray.Add(new ConfigItem(ConfigCode.PalamLvDef, "PALAMLVの初期値", new List<long>(new Int64[] { 0, 100, 500, 3000, 10000, 30000, 60000, 100000, 150000, 250000 })));
			replaceArray.Add(new ConfigItem(ConfigCode.pbandDef, "PBANDの初期値", 4L));
            replaceArray.Add(new ConfigItem(ConfigCode.RelationDef, "RELATIONの初期値", 0L));
		}
        
		public ConfigData Copy()
		{
			ConfigData config = new ConfigData();
			for (int i = 0; i < configArray.Count; i++)
				if ((this.configArray[i] != null) && (config.configArray[i] != null))
					this.configArray[i].CopyTo(config.configArray[i]);
			for (int i = 0; i < configArray.Count; i++)
				if ((this.configArray[i] != null) && (config.configArray[i] != null))
					this.configArray[i].CopyTo(config.configArray[i]);
			for (int i = 0; i < replaceArray.Count; i++)
				if ((this.replaceArray[i] != null) && (config.replaceArray[i] != null))
					this.replaceArray[i].CopyTo(config.replaceArray[i]);
			return config;
		}

		public Dictionary<ConfigCode,string> GetConfigNameDic()
		{
			Dictionary<ConfigCode, string> ret = new Dictionary<ConfigCode, string>();
			foreach (ConfigItem item in configArray)
			{
				if (item != null)
					ret.Add(item.Code, item.Text);
			}
			return ret;
		}

		public T GetConfigValue<T>(ConfigCode code)
		{
			//AConfigItem item = GetItem(code);
			//if ((item != null) && (item is ConfigItem<T>))
			//	return ((ConfigItem<T>)item).Value;
			//throw new ExeEE("GetConfigValueのCodeまたは型が不適切");

			ConfigModel model = ConfigModel.Get(code);
			ConfigItem item;
			if (model == null)
				item = GetItem(code);
			else
			{
				item = model.ConfigItem;
				if (model.Enabled)
					return (T)model.Value;
			}

			return (T)item.Value;
		}

#region getitem
		public ConfigItem GetItem(ConfigCode code)
		{
			ConfigItem item = GetConfigItem(code);
            if (item == null)
            {
                item = GetReplaceItem(code);
	            if (item == null)
	            {
	                item = GetDebugItem(code);
	            }
            }
			return item;
		}
		public ConfigItem GetItem(string key)
		{
			ConfigItem item = GetConfigItem(key);
			if (item == null)
			{
				item = GetReplaceItem(key);
	            if (item == null)
	            {
					item = GetDebugItem(key);
	            }
	        }
			return item;
		}

		public ConfigItem GetConfigItem(ConfigCode code)
		{
			return configArray.FirstOrDefault(item => item.Code == code);
		}

		public ConfigItem GetConfigItem(string key)
		{
			return configArray.FirstOrDefault(item => item.Name == key || item.Text == key);
		}

		public ConfigItem GetReplaceItem(ConfigCode code)
		{
			return replaceArray.FirstOrDefault(item => item.Code == code);
		}

		public ConfigItem GetReplaceItem(string key)
		{
			return replaceArray.FirstOrDefault(item => item.Name == key || item.Text == key);
		}
		
		public ConfigItem GetDebugItem(ConfigCode code)
		{
			return debugArray.FirstOrDefault(item => item.Code == code);
		}

		public ConfigItem GetDebugItem(string key)
		{
			return debugArray.FirstOrDefault(item => item.Name == key || item.Text == key);
		}
		
		public SingleTerm GetConfigValueInERB(string text, ref string errMes)
		{
			ConfigItem item = Instance.GetItem(text);
			if(item == null)
			{
				errMes = "文字列\"" + text + "\"は適切なコンフィグ名ではありません";
				return null;
			}
			SingleTerm term;
			switch(item.Code)
			{
				//<bool>
				case ConfigCode.AutoSave://"オートセーブを行なう"
				case ConfigCode.MoneyFirst://"単位の位置"
					if(item.GetValue<bool>())
						term = new SingleTerm(1);
					else
						term = new SingleTerm(0);
					break;
				//<int>
				case ConfigCode.WindowX:// "ウィンドウ幅"
				case ConfigCode.PrintCPerLine:// "PRINTCを並べる数"
				case ConfigCode.PrintCLength:// "PRINTCの文字数"
				case ConfigCode.FontSize:// "フォントサイズ"
				case ConfigCode.LineHeight:// "一行の高さ"
				case ConfigCode.SaveDataNos:// "表示するセーブデータ数"
				case ConfigCode.MaxShopItem:// "販売アイテム数"
				case ConfigCode.ComAbleDefault:// "COM_ABLE初期値"
					term = new SingleTerm(item.GetValue<int>());
					break;
				//<Color>
				case ConfigCode.ForeColor://"文字色"
				case ConfigCode.BackColor://"背景色"
				case ConfigCode.FocusColor://"選択中文字色"
				case ConfigCode.LogColor://"履歴文字色"
					{
						Color color = item.GetValue<Color>();
						term = new SingleTerm( ((color.R * 256) + color.G) * 256 + color.B);
					}
					break;

				//<Int64>
				case ConfigCode.pbandDef:// "PBANDの初期値"
				case ConfigCode.RelationDef:// "RELATIONの初期値"
					term = new SingleTerm(item.GetValue<Int64>());
					break;

				//<string>
				case ConfigCode.FontName:// "フォント名"
				case ConfigCode.MoneyLabel:// "お金の単位"
				case ConfigCode.LoadLabel:// "起動時簡略表示"
				case ConfigCode.DrawLineString:// "DRAWLINE文字"
				case ConfigCode.TitleMenuString0:// "システムメニュー0"
				case ConfigCode.TitleMenuString1:// "システムメニュー1"
				case ConfigCode.TimeupLabel:// "時間切れ表示"
					term = new SingleTerm(item.GetValue<string>());
					break;
				
				//<char>
				case ConfigCode.BarChar1:// "BAR文字1"
				case ConfigCode.BarChar2:// "BAR文字2"
					term = new SingleTerm(item.GetValue<char>().ToString());
					break;
				//<TextDrawingMode>
				case ConfigCode.TextDrawingMode:// "描画インターフェース"
					term = new SingleTerm(item.GetValue<TextDrawingMode>().ToString());
					break;
				default:
				{
					errMes = "コンフィグ文字列\"" + text + "\"の値の取得は許可されていません";
					return null;
				}
			}
			return term;
		}
#endregion


		public bool SaveConfig()
		{
			StreamWriter writer = null;

			try
			{
				writer = new StreamWriter(configPath, false, Config.Encode);
				for (int i = 0; i < configArray.Count; i++)
				{
					ConfigItem item = configArray[i];
					if (item == null)
						continue;
					
					//1806beta001 CompatiDRAWLINEの廃止、CompatiLinefeedAs1739へ移行
					if (item.Code == ConfigCode.CompatiDRAWLINE)
						continue;
					if ((item.Code == ConfigCode.ChangeMasterNameIfDebug) && (item.GetValue<bool>()))
						continue;
					if ((item.Code == ConfigCode.LastKey) && (item.GetValue<long>() == 0))
						continue;
					//if (item.Code == ConfigCode.IgnoreWarningFiles)
					//{
					//    List<string> files = item.GetValue<List<string>>();
					//    foreach (string filename in files)
					//        writer.WriteLine(item.Text + ":" + filename.ToString());
					//    continue;
					//}
					writer.WriteLine(item.ToString());
				}
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
			return true;
		}

        public bool ReLoadConfig()
        {
            //_fixed.configの中身が変わった場合、非固定になったものが保持されてしまうので、ここで一旦すべて解除
            foreach (ConfigItem item in configArray)
            {
                if (item == null)
                    continue;
                if (item.Fixed)
                    item.Fixed = false;
            }
            LoadConfig();
            return true;
        }

		public bool LoadConfig()
		{
			configPath = Program.ExeDir + "emuera.config";
			configdebugPath = Program.DebugDir + "debug.config";

			Config.ClearFont();
			string defaultConfigPath = Program.CsvDir + "_default.config";
			string fixedConfigPath = Program.CsvDir + "_fixed.config";
			if(!FileUtils.Exists(ref defaultConfigPath))
				defaultConfigPath = Program.CsvDir + "default.config";
			if (!FileUtils.Exists(ref fixedConfigPath))
				fixedConfigPath = Program.CsvDir + "fixed.config";

			LoadConfig(defaultConfigPath, false);
			LoadConfig(configPath, false);
			LoadConfig(fixedConfigPath, true);
			
			Config.SetConfig(this);
			bool needSave = false;
			if (!FileUtils.Exists(ref configPath))
				needSave = true;
			if (Config.CheckUpdate())
			{
				GetItem(ConfigCode.LastKey).SetValue(Config.LastKey);
				needSave = true;
			}
			if (needSave)
				SaveConfig();

			return true;
		}

		private bool LoadConfig(string confPath, bool fix)
		{
			if (!FileUtils.Exists(ref confPath))
				return false;
			EraStreamReader eReader = new EraStreamReader(false);
			if (!eReader.Open(confPath))
				return false;
			ScriptPosition pos = null;
			try
			{
				string line = null;
				//bool defineIgnoreWarningFiles = false;
				while ((line = eReader.ReadLine()) != null)
				{
					if ((line.Length == 0) || (line[0] == ';'))
						continue;
					pos = new ScriptPosition(eReader.Filename, eReader.LineNo);
					string[] tokens = line.Split(new char[] { ':' });
					if (tokens.Length < 2)
						continue;
					ConfigItem item = GetConfigItem(tokens[0].Trim());
					if (item != null)
					{
						if (item.Code == ConfigCode.TextDrawingMode)
							item.Value = TextDrawingMode.GRAPHICS;

						//1806beta001 CompatiDRAWLINEの廃止、CompatiLinefeedAs1739へ移行
						if(item.Code == ConfigCode.CompatiDRAWLINE)
						{
							item = GetConfigItem(ConfigCode.CompatiLinefeedAs1739);
						}
						//if ((item.Code == ConfigCode.IgnoreWarningFiles))
						//{ 
						//    if (!defineIgnoreWarningFiles)
						//        (item.GetValue<List<string>>()).Clear();
						//    defineIgnoreWarningFiles = true;
						//    if ((item.Fixed) && (fix))
						//        item.Fixed = false;
						//}
						
						if (item.Code == ConfigCode.TextEditor)
						{
							//パスの関係上tokens[2]は使わないといけない
							if (tokens.Length > 2)
							{
								if (tokens[2].StartsWith("\\"))
									tokens[1] += ":" + tokens[2];
								if (tokens.Length > 3)
								{
									for (int i = 3; i < tokens.Length; i++)
									{
										tokens[1] += ":" + tokens[i];
									}
								}
							}
						}
						if (item.Code == ConfigCode.EditorArgument)
						{
							//半角スペースを要求する引数が必要なエディタがあるので別処理で
							item.Value = tokens[1];
							continue;
						}
                        if (item.Code == ConfigCode.MaxLog && Program.AnalysisMode)
                        {
                            //解析モード時はここを上書きして十分な長さを確保する
                            tokens[1] = "10000";
                        }
						if ((item.TryParse(tokens[1])) && (fix))
							item.Fixed = true;
					}
#if DEBUG
					//else
					//	throw new Exception("コンフィグファイルが変");
#endif
				}
			}
			catch (EmueraException ee)
			{
				ParserMediator.ConfigWarn(ee.Message, pos, 1, null);
			}
			catch (Exception exc)
			{
				ParserMediator.ConfigWarn(exc.GetType().ToString() + ":" + exc.Message, pos, 1, exc.StackTrace);
			}
			finally { eReader.Dispose(); }
			return true;
		}

#region replace
		// 1.52a改変部分　（単位の差し替えおよび前置、後置のためのコンフィグ処理）
		public void LoadReplaceFile(string filename)
		{
			EraStreamReader eReader = new EraStreamReader(false);
			if (!eReader.Open(filename))
				return;
			ScriptPosition pos = null;
			try
			{
				string line = null;
				while ((line = eReader.ReadLine()) != null)
				{
					if ((line.Length == 0) || (line[0] == ';'))
						continue;
					pos = new ScriptPosition(eReader.Filename, eReader.LineNo);
                    string[] tokens = line.Split(new char[] { ',', ':' });
					if (tokens.Length < 2)
						continue;
                    string itemName = tokens[0].Trim();
                    tokens[1] = line.Substring(tokens[0].Length + 1);
                    if (string.IsNullOrEmpty(tokens[1].Trim()))
                        continue;
                    ConfigItem item = GetReplaceItem(itemName);
                    if (item != null)
                        item.TryParse(tokens[1]);
				}
			}
			catch (EmueraException ee)
			{
				ParserMediator.Warn(ee.Message, pos, 1);
			}
			catch (Exception exc)
			{
				ParserMediator.Warn(exc.GetType().ToString() + ":" + exc.Message, pos, 1, exc.StackTrace);
			}
			finally { eReader.Dispose(); }
		}

#endregion 

#region debug


		public bool SaveDebugConfig()
		{
			StreamWriter writer = null;
			try
			{
				writer = new StreamWriter(configdebugPath, false, Config.Encode);
				for (int i = 0; i < debugArray.Count; i++)
				{
					ConfigItem item = debugArray[i];
					if (item == null)
						continue;
					writer.WriteLine(item.ToString());
				}
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
			return true;
		}
		
		public bool LoadDebugConfig()
		{
			if (!FileUtils.Exists(ref configdebugPath))
				goto err;
			EraStreamReader eReader = new EraStreamReader(false);
			if (!eReader.Open(configdebugPath))
				goto err;
			ScriptPosition pos = null;
			try
			{
				string line = null;
				while ((line = eReader.ReadLine()) != null)
				{
					if ((line.Length == 0) || (line[0] == ';'))
						continue;
					pos = new ScriptPosition(eReader.Filename, eReader.LineNo);
					string[] tokens = line.Split(new char[] { ':' });
					if (tokens.Length < 2)
						continue;
					ConfigItem item = GetDebugItem(tokens[0].Trim());
					if (item != null)
					{
						item.TryParse(tokens[1]);
					}
#if DEBUG
					//else
					//	throw new Exception("コンフィグファイルが変");
#endif
				}
			}
			catch (EmueraException ee)
			{
				ParserMediator.ConfigWarn(ee.Message, pos, 1, null);
				goto err;
			}
			catch (Exception exc)
			{
				ParserMediator.ConfigWarn(exc.GetType().ToString() + ":" + exc.Message, pos, 1, exc.StackTrace);
				goto err;
			}
			finally { eReader.Dispose(); }
			Config.SetDebugConfig(this);
            return true;
		err:
			Config.SetDebugConfig(this);
			return false;
		}

#endregion
	}
}