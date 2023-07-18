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
using SkiaSharp;
using XEmuera.Resources;
using trerror = EvilMask.Emuera.Lang.Error;

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
		#region EM_私家版_Emuera多言語化改造
		//適当に大き目の配列を作っておく。
		private List<ConfigItem> configArray = new List<ConfigItem>();
		private List<ConfigItem> replaceArray = new List<ConfigItem>();
		private List<ConfigItem> debugArray = new List<ConfigItem>();
		#endregion

		#region EM_私家版_Emuera多言語化改造
		private void SetDefault()
		{
			ConfigsText.Culture = LanguageModel.Get("ja-JP");


			configArray.Add(new ConfigItem(ConfigCode.IgnoreCase, ConfigsText.IgnoreCase, "Ignore case", true));
			configArray.Add(new ConfigItem(ConfigCode.UseRenameFile, ConfigsText.UseRenameFile, "Use _Rename.csv file", false));
			configArray.Add(new ConfigItem(ConfigCode.UseReplaceFile, ConfigsText.UseReplaceFile, "Use _Replace.csv file", true));
			configArray.Add(new ConfigItem(ConfigCode.UseMouse, ConfigsText.UseMouse, "Use mouse", true));
			configArray.Add(new ConfigItem(ConfigCode.UseMenu, ConfigsText.UseMenu, "Show menu", true));
			configArray.Add(new ConfigItem(ConfigCode.UseDebugCommand, ConfigsText.UseDebugCommand, "Allow debug commands", false));
			configArray.Add(new ConfigItem(ConfigCode.AllowMultipleInstances, ConfigsText.AllowMultipleInstances, "Allow multiple instances", true));
			configArray.Add(new ConfigItem(ConfigCode.AutoSave, ConfigsText.AutoSave, "Make autosaves", true));
			configArray.Add(new ConfigItem(ConfigCode.UseKeyMacro, ConfigsText.UseKeyMacro, "Use keyboard macros", true));
			configArray.Add(new ConfigItem(ConfigCode.SizableWindow, ConfigsText.SizableWindow, "Changeable window height", true));
			configArray.Add(new ConfigItem(ConfigCode.TextDrawingMode, ConfigsText.TextDrawingMode, "Drawing interface", TextDrawingMode.GRAPHICS));

			configArray.Add(new ConfigItem(ConfigCode.WindowX, ConfigsText.WindowX, "Window width", 760));
			configArray.Add(new ConfigItem(ConfigCode.WindowY, ConfigsText.WindowY, "Window height", 480));
			configArray.Add(new ConfigItem(ConfigCode.WindowPosX, ConfigsText.WindowPosX, "Window X position", 0));
			configArray.Add(new ConfigItem(ConfigCode.WindowPosY, ConfigsText.WindowPosY, "Window Y position", 0));
			configArray.Add(new ConfigItem(ConfigCode.SetWindowPos, ConfigsText.SetWindowPos, "Fixed window starting position", false));
			configArray.Add(new ConfigItem(ConfigCode.WindowMaximixed, ConfigsText.WindowMaximixed, "Maximize window on startup", false));
			configArray.Add(new ConfigItem(ConfigCode.MaxLog, ConfigsText.MaxLog, "Max history log lines", 5000));
			configArray.Add(new ConfigItem(ConfigCode.PrintCPerLine, ConfigsText.PrintCPerLine, "Items per line for PRINTC", 3));
			configArray.Add(new ConfigItem(ConfigCode.PrintCLength, ConfigsText.PrintCLength, "Number of Item characters for PRINTC", 25));
			configArray.Add(new ConfigItem(ConfigCode.FontName, ConfigsText.FontName, "Font name", "ＭＳ ゴシック"));
			configArray.Add(new ConfigItem(ConfigCode.FontSize, ConfigsText.FontSize, "Font size", 18));
			configArray.Add(new ConfigItem(ConfigCode.LineHeight, ConfigsText.LineHeight, "Line height", 19));
			configArray.Add(new ConfigItem(ConfigCode.ForeColor, ConfigsText.ForeColor, "Text color", Color.FromArgb(192, 192, 192)));//LIGHTGRAY
			configArray.Add(new ConfigItem(ConfigCode.BackColor, ConfigsText.BackColor, "Background color", Color.FromArgb(0, 0, 0)));//BLACK
			configArray.Add(new ConfigItem(ConfigCode.FocusColor, ConfigsText.FocusColor, "Highlight color", Color.FromArgb(255, 255, 0)));//YELLOW
			configArray.Add(new ConfigItem(ConfigCode.LogColor, ConfigsText.LogColor, "History log color", Color.FromArgb(192, 192, 192)));//LIGHTGRAY//Color.FromArgb(128, 128, 128));//GRAY
			configArray.Add(new ConfigItem(ConfigCode.FPS, ConfigsText.FPS, "FPS", 5));
			configArray.Add(new ConfigItem(ConfigCode.SkipFrame, ConfigsText.SkipFrame, "Skip frames", 3));
			configArray.Add(new ConfigItem(ConfigCode.ScrollHeight, ConfigsText.ScrollHeight, "Lines per scroll", 1));
			configArray.Add(new ConfigItem(ConfigCode.InfiniteLoopAlertTime, ConfigsText.InfiniteLoopAlertTime, "Milliseconds for infinite loop warning", 5000));
			configArray.Add(new ConfigItem(ConfigCode.DisplayWarningLevel, ConfigsText.DisplayWarningLevel, "Minimum warning level", 1));
			configArray.Add(new ConfigItem(ConfigCode.DisplayReport, ConfigsText.DisplayReport, "Display loading report", false));
			configArray.Add(new ConfigItem(ConfigCode.ReduceArgumentOnLoad, ConfigsText.ReduceArgumentOnLoad, "Reduce argument on load", ReduceArgumentOnLoadFlag.NO));

			configArray.Add(new ConfigItem(ConfigCode.IgnoreUncalledFunction, ConfigsText.IgnoreUncalledFunction, "Ignore uncalled functions", true));
			configArray.Add(new ConfigItem(ConfigCode.FunctionNotFoundWarning, ConfigsText.FunctionNotFoundWarning, "Function is not found warning", DisplayWarningFlag.IGNORE));
			configArray.Add(new ConfigItem(ConfigCode.FunctionNotCalledWarning, ConfigsText.FunctionNotCalledWarning, "Function not called warning", DisplayWarningFlag.IGNORE));


			configArray.Add(new ConfigItem(ConfigCode.ChangeMasterNameIfDebug, ConfigsText.ChangeMasterNameIfDebug, "Change MASTER mame in debug", true));
			configArray.Add(new ConfigItem(ConfigCode.ButtonWrap, ConfigsText.ButtonWrap, "Button wrapping", false));
			configArray.Add(new ConfigItem(ConfigCode.SearchSubdirectory, ConfigsText.SearchSubdirectory, "Search subfolders", false));
			configArray.Add(new ConfigItem(ConfigCode.SortWithFilename, ConfigsText.SortWithFilename, "Sort filenames", false));
			configArray.Add(new ConfigItem(ConfigCode.LastKey, ConfigsText.LastKey, "Latest identify code", 0L));
			configArray.Add(new ConfigItem(ConfigCode.SaveDataNos, ConfigsText.SaveDataNos, "Save data count per page", 20));
			configArray.Add(new ConfigItem(ConfigCode.WarnBackCompatibility, ConfigsText.WarnBackCompatibility, "Eramaker compatibility warning", true));
			configArray.Add(new ConfigItem(ConfigCode.AllowFunctionOverloading, ConfigsText.AllowFunctionOverloading, "Allow overriding system functions", true));
			configArray.Add(new ConfigItem(ConfigCode.WarnFunctionOverloading, ConfigsText.WarnFunctionOverloading, "System function override warning", true));
			configArray.Add(new ConfigItem(ConfigCode.TextEditor, ConfigsText.TextEditor, "Text editor", "notepad"));
			configArray.Add(new ConfigItem(ConfigCode.EditorType, ConfigsText.EditorType, "Text editor command line setting", TextEditorType.USER_SETTING));
			configArray.Add(new ConfigItem(ConfigCode.EditorArgument, ConfigsText.EditorArgument, "Text editor command line arguments", ""));
			configArray.Add(new ConfigItem(ConfigCode.WarnNormalFunctionOverloading, ConfigsText.WarnNormalFunctionOverloading, "Duplicated functions warning", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiErrorLine, ConfigsText.CompatiErrorLine, "Execute error lines", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiCALLNAME, ConfigsText.CompatiCALLNAME, "Use NAME if CALLNAME is empty", false));
			configArray.Add(new ConfigItem(ConfigCode.UseSaveFolder, ConfigsText.UseSaveFolder, "Use sav folder", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiRAND, ConfigsText.CompatiRAND, "Imitate behavior for RAND", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiDRAWLINE, ConfigsText.CompatiDRAWLINE, "Always start DRAWLINE in a new line", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiFunctionNoignoreCase, ConfigsText.CompatiFunctionNoignoreCase, "Do not ignore case for functions and attributes", false));
			configArray.Add(new ConfigItem(ConfigCode.SystemAllowFullSpace, ConfigsText.SystemAllowFullSpace, "Whitespace includes full-width space", true));
			configArray.Add(new ConfigItem(ConfigCode.SystemSaveInUTF8, ConfigsText.SystemSaveInUTF8, "Use UTF8 for save data", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiLinefeedAs1739, ConfigsText.CompatiLinefeedAs1739, "Reproduce wrapping behavior like in pre ver1739", false));
			configArray.Add(new ConfigItem(ConfigCode.useLanguage, ConfigsText.useLanguage, "Default ANSI encoding", UseLanguage.JAPANESE));
			configArray.Add(new ConfigItem(ConfigCode.AllowLongInputByMouse, ConfigsText.AllowLongInputByMouse, "Allow long input by mouse for ONEINPUT", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiCallEvent, ConfigsText.CompatiCallEvent, "Allow CALL on event functions", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiSPChara, ConfigsText.CompatiSPChara, "Allow SP characters", false));

			configArray.Add(new ConfigItem(ConfigCode.SystemSaveInBinary, ConfigsText.SystemSaveInBinary, "Use the binary format for saving data", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiFuncArgOptional, ConfigsText.CompatiFuncArgOptional, "Allow arguments omission for user functions", false));
			configArray.Add(new ConfigItem(ConfigCode.CompatiFuncArgAutoConvert, ConfigsText.CompatiFuncArgAutoConvert, "Auto TOSTR conversion for user function arguments", false));
			configArray.Add(new ConfigItem(ConfigCode.SystemIgnoreTripleSymbol, ConfigsText.SystemIgnoreTripleSymbol, "Do not process triple symbols inside FORM", false));
			configArray.Add(new ConfigItem(ConfigCode.TimesNotRigorousCalculation, ConfigsText.TimesNotRigorousCalculation, "Imitate behavior for TIMES", false));

			configArray.Add(new ConfigItem(ConfigCode.SystemNoTarget, ConfigsText.SystemNoTarget, "Do not auto-complete arguments for character variables", false));
			configArray.Add(new ConfigItem(ConfigCode.SystemIgnoreStringSet, ConfigsText.SystemIgnoreStringSet, "String variable assignment on valid with string expression", false));

			#region EE_UPDATECHECK
			configArray.Add(new ConfigItem(ConfigCode.ForbidUpdateCheck, ConfigsText.ForbidUpdateCheck, "Disallow UPDATECHECK", false));
			#endregion
			#region EE_ERDConfig
			configArray.Add(new ConfigItem(ConfigCode.UseERD, ConfigsText.UseERD, "Use ERD", true));
			#endregion
			#region EE_ERDNAME
			configArray.Add(new ConfigItem(ConfigCode.VarsizeDimConfig, ConfigsText.VarsizeDimConfig, "Imitate ERD to VARSIZE dimension specification", false));
			#endregion
			#region EM_私家版_LoadText＆SaveText機能拡張
			configArray.Add(new ConfigItem(ConfigCode.ValidExtension, ConfigsText.ValidExtension, "Valid extensions for LOADTEXT and SAVETEXT", new List<string> { "txt" }));
			#endregion
			#region EM_私家版_セーブ圧縮
			configArray.Add(new ConfigItem(ConfigCode.ZipSaveData, ConfigsText.ZipSaveData, "Compress save data", false));
			#endregion
			#region EM_私家版_Emuera多言語化改造
			configArray.Add(new ConfigItem(ConfigCode.EnglishConfigOutput, ConfigsText.EnglishConfigOutput, "Output English items in the config file", false));
			configArray.Add(new ConfigItem(ConfigCode.EmueraLang, "Emueraの表示言語", "Emuera interface language", string.Empty));
			#endregion
			#region EM_私家版_Icon指定機能
			configArray.Add(new ConfigItem(ConfigCode.EmueraIcon, "Emueraのアイコンのパス", "Path to a custom window icon", string.Empty));
			#endregion

			debugArray.Add(new ConfigItem(ConfigCode.DebugShowWindow, ConfigsText.DebugShowWindow, "Show debug window on startup", true));
			debugArray.Add(new ConfigItem(ConfigCode.DebugWindowTopMost, ConfigsText.DebugWindowTopMost, "Debug window always on top", true));
			debugArray.Add(new ConfigItem(ConfigCode.DebugWindowWidth, ConfigsText.DebugWindowWidth, "Debug window width", 400));
			debugArray.Add(new ConfigItem(ConfigCode.DebugWindowHeight, ConfigsText.DebugWindowHeight, "Debug window height", 300));
			debugArray.Add(new ConfigItem(ConfigCode.DebugSetWindowPos, ConfigsText.DebugSetWindowPos, "Fixed debug window starting position", false));
			debugArray.Add(new ConfigItem(ConfigCode.DebugWindowPosX, ConfigsText.DebugWindowPosX, "Debug window X position", 0));
			debugArray.Add(new ConfigItem(ConfigCode.DebugWindowPosY, ConfigsText.DebugWindowPosY, "Debug window Y position", 0));

			replaceArray.Add(new ConfigItem(ConfigCode.MoneyLabel, ConfigsText.MoneyLabel, "Currency symbol", "$"));
			replaceArray.Add(new ConfigItem(ConfigCode.MoneyFirst, ConfigsText.MoneyFirst, "Currency symbol position", true));
			replaceArray.Add(new ConfigItem(ConfigCode.LoadLabel, ConfigsText.LoadLabel, "Loading message", "Now Loading..."));
			replaceArray.Add(new ConfigItem(ConfigCode.MaxShopItem, ConfigsText.MaxShopItem, "Max shop item storage", 100));
			replaceArray.Add(new ConfigItem(ConfigCode.DrawLineString, ConfigsText.DrawLineString, "DRAWLINE character", "-"));
			replaceArray.Add(new ConfigItem(ConfigCode.BarChar1, ConfigsText.BarChar1, "BAR character 1", '*'));
			replaceArray.Add(new ConfigItem(ConfigCode.BarChar2, ConfigsText.BarChar2, "BAR character 2", '.'));
			replaceArray.Add(new ConfigItem(ConfigCode.TitleMenuString0, ConfigsText.TitleMenuString0, "System menu 0", "最初からはじめる"));
			replaceArray.Add(new ConfigItem(ConfigCode.TitleMenuString1, ConfigsText.TitleMenuString1, "System menu 1", "ロードしてはじめる"));
			replaceArray.Add(new ConfigItem(ConfigCode.ComAbleDefault, ConfigsText.ComAbleDefault, "Default COM_ABLE", 1));
			replaceArray.Add(new ConfigItem(ConfigCode.StainDefault, ConfigsText.StainDefault, "Default Stain", new List<Int64>(new Int64[] { 0, 0, 2, 1, 8 })));
			replaceArray.Add(new ConfigItem(ConfigCode.TimeupLabel, ConfigsText.TimeupLabel, "Time up message", "時間切れ"));
			replaceArray.Add(new ConfigItem(ConfigCode.ExpLvDef, ConfigsText.ExpLvDef, "Default EXPLV", new List<long>(new Int64[] { 0, 1, 4, 20, 50, 200 })));
			replaceArray.Add(new ConfigItem(ConfigCode.PalamLvDef, ConfigsText.PalamLvDef, "Default PALAMLV", new List<long>(new Int64[] { 0, 100, 500, 3000, 10000, 30000, 60000, 100000, 150000, 250000 })));
			replaceArray.Add(new ConfigItem(ConfigCode.pbandDef, ConfigsText.pbandDef, "Default PBAND", 4L));
			replaceArray.Add(new ConfigItem(ConfigCode.RelationDef, ConfigsText.RelationDef, "Default RELATION", 0L));


			#region XEmuera_Config
			configArray.Add(new ConfigItem(ConfigCode.PanSpeed, ConfigsText.PanSpeed, "", 2.0f));

			configArray.Add(new ConfigItem(ConfigCode.QuickButtonFontSize, ConfigsText.QuickButtonFontSize, "", 12));
			configArray.Add(new ConfigItem(ConfigCode.QuickButtonWidth, ConfigsText.QuickButtonWidth, "", 90));
			configArray.Add(new ConfigItem(ConfigCode.QuickButtonSpacing, ConfigsText.QuickButtonSpacing, "", 3));

			configArray.Add(new ConfigItem(ConfigCode.AdaptiveFont, ConfigsText.AdaptiveFont, "", true));
			configArray.Add(new ConfigItem(ConfigCode.AdaptiveFontSize, ConfigsText.AdaptiveFontSize, "", 32));
			configArray.Add(new ConfigItem(ConfigCode.FontScale, ConfigsText.FontScale, "", 1f));
			configArray.Add(new ConfigItem(ConfigCode.TextAntialias, ConfigsText.TextAntialias, "", true));
			configArray.Add(new ConfigItem(ConfigCode.ShapeAntialias, ConfigsText.ShapeAntialias, "", true));
			configArray.Add(new ConfigItem(ConfigCode.TextFilterQuality, ConfigsText.TextFilterQuality, "", SKFilterQuality.Low));
			configArray.Add(new ConfigItem(ConfigCode.ShapeFilterQuality, ConfigsText.ShapeFilterQuality, "", SKFilterQuality.Low));

			configArray.Add(new ConfigItem(ConfigCode.LongPressSkip, ConfigsText.LongPressSkip, "", true));
			configArray.Add(new ConfigItem(ConfigCode.LongPressSkipTime, ConfigsText.LongPressSkipTime, "", 800));
			#endregion

			ConfigsText.Culture = LanguageModel.Current;
		}
		#endregion

		public ConfigData Copy()
		{
			#region EM_私家版_Emuera多言語化改造
			ConfigData config = new ConfigData();
			for (int i = 0; i < configArray.Count; i++)
				if ((this.configArray[i] != null) && (config.configArray[i] != null))
					this.configArray[i].CopyTo(config.configArray[i]);
			for (int i = 0; i < configArray.Count; i++)
				if ((this.debugArray[i] != null) && (config.debugArray[i] != null))
					this.debugArray[i].CopyTo(config.debugArray[i]);
			for (int i = 0; i < replaceArray.Count; i++)
				if ((this.replaceArray[i] != null) && (config.replaceArray[i] != null))
					this.replaceArray[i].CopyTo(config.replaceArray[i]);
			return config;
			#endregion
		}

		public Dictionary<ConfigCode,string> GetConfigNameDic()
		{
			Dictionary<ConfigCode, string> ret = new Dictionary<ConfigCode, string>();
			#region EM_私家版_Emuera多言語化改造
			foreach (ConfigItem item in configArray)
			{
				if (item != null)
					// ret.Add(item.Code, item.Text);
					ret.Add(item.Code, $"{item.Text}/{item.EngText}");
			}
			#endregion
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
				if (model.Enabled)
					return (T)model.Value;
				item = model.ConfigItem;
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
			#region EM_私家版_Emuera多言語化改造
			key = key.ToUpper();
			return configArray.FirstOrDefault(item => item.Name == key || item.Text == key || item.EngText == key);
			#endregion
		}

		public ConfigItem GetReplaceItem(ConfigCode code)
		{
			return replaceArray.FirstOrDefault(item => item.Code == code);
		}

		public ConfigItem GetReplaceItem(string key)
		{
			#region EM_私家版_Emuera多言語化改造
			key = key.ToUpper();
			return replaceArray.FirstOrDefault(item => item.Name == key || item.Text == key);
			#endregion
		}
		
		public ConfigItem GetDebugItem(ConfigCode code)
		{
			return debugArray.FirstOrDefault(item => item.Code == code);
		}

		public ConfigItem GetDebugItem(string key)
		{
			#region EM_私家版_Emuera多言語化改造
			key = key.ToUpper();
			return debugArray.FirstOrDefault(item => item.Name == key || item.Text == key);
			#endregion
		}

		public SingleTerm GetConfigValueInERB(string text, ref string errMes)
		{
			ConfigItem item = Instance.GetItem(text);
			if(item == null)
			{
				errMes = string.Format(trerror.InvalidConfigName.Text, text);
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
				// 
				case ConfigCode.FontSize:// "フォントサイズ"
					term = new SingleTerm(Config.FontSize);
					break;
				case ConfigCode.LineHeight:// "一行の高さ"
					term = new SingleTerm(Config.LineHeight);
					break;
				case ConfigCode.WindowX:// "ウィンドウ幅"
				case ConfigCode.PrintCPerLine:// "PRINTCを並べる数"
				case ConfigCode.PrintCLength:// "PRINTCの文字数"
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
						errMes = string.Format(trerror.NotAllowGetConfigValue.Text, text);
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
				#region EM_私家版_Emuera多言語化改造
				// writer = new StreamWriter(configPath, false, Config.Encode);
				writer = new StreamWriter(configPath, false, new UTF8Encoding(true));

				// for (int i = 0; i < configArray.Length; i++)
				for (int i = 0; i < configArray.Count; i++)
				#endregion
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
					#region EM_私家版_LoadText＆SaveText機能拡張
					if ((item.Code == ConfigCode.ValidExtension))
					{
						var ex = item;
						var sb = new System.Text.StringBuilder();
						#region EM_私家版_Emuera多言語化改造
						// sb.Append(ex.Text).Append(":");
						sb.Append(Config.EnglishConfigOutput ? ex.EngText : ex.Text).Append(":");
						#endregion
						foreach (var str in (List<String>)ex.Value)
						{
							sb.Append(str).Append(",");
						}
						sb.Remove(sb.Length - 1, 1);
						writer.WriteLine(sb.ToString());
						continue;
					}
					#endregion
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
					#region EM_私家版_Emuera多言語化改造
					ConfigItem item = GetConfigItem(tokens[0].Trim());
					#endregion
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
						item.Fixed = false;
						if ((ConfigItem.TryParse(item, tokens[1])) && (fix))
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
                        ConfigItem.TryParse(item, tokens[1]);
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
				#region EM_私家版_Emuera多言語化改造
				// writer = new StreamWriter(configdebugPath, false, Config.Encode);
				writer = new StreamWriter(configdebugPath, false, new UTF8Encoding(true));

				// for (int i = 0; i < debugArray.Length; i++)
				for (int i = 0; i < debugArray.Count; i++)
				#endregion
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
						ConfigItem.TryParse(item, tokens[1]);
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