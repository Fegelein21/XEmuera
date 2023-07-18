using System;
//using System.Drawing;
using System.Collections.Generic;
using XEmuera.Forms;
using MinorShift._Library;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.GameData.Expression;
using System.IO;
using XEmuera.Resources;
using EvilMask.Emuera;

namespace MinorShift.Emuera
{
	internal static class Program
	{
		/*
		コードの開始地点。
		ここでMainWindowを作り、
		MainWindowがProcessを作り、
		ProcessがGameBase・ConstantData・Variableを作る。
		
		
		*.ERBの読み込み、実行、その他の処理をProcessが、
		入出力をMainWindowが、
		定数の保存をConstantDataが、
		変数の管理をVariableが行う。
		 
		と言う予定だったが改変するうちに境界が曖昧になってしまった。
		 
		後にEmueraConsoleを追加し、それに入出力を担当させることに。
        
        1750 DebugConsole追加
         Debugを全て切り離すことはできないので一部EmueraConsoleにも担当させる
		
		TODO: 1819 MainWindow & Consoleの入力・表示組とProcess&Dataのデータ処理組だけでも分離したい

		*/
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		//[STAThread]
		//static void Main(string[] args)
		internal static bool Init()
		{
			ExeDir = Sys.ExeDir;
#if DEBUG
			debugMode = true;

			//ExeDirにバリアントのパスを代入することでテスト実行するためのコード。
			//ローカルパスの末尾には\必須。
			//ローカルパスを記載した場合は頒布前に削除すること。
			//ExeDir = @"";
			
#endif
			//CsvDir = ExeDir + "csv\\";
			//ErbDir = ExeDir + "erb\\";
			//DebugDir = ExeDir + "debug\\";
			//DatDir = ExeDir + "dat\\";
			//ContentDir = ExeDir + "resources\\";

			CsvDir = ExeDir + "csv" + Path.DirectorySeparatorChar;
			ErbDir = ExeDir + "erb" + Path.DirectorySeparatorChar;
			DebugDir = ExeDir + "debug" + Path.DirectorySeparatorChar;
			DatDir = ExeDir + "dat" + Path.DirectorySeparatorChar;
			ContentDir = ExeDir + "resources" + Path.DirectorySeparatorChar;
			MusicDir = ExeDir + "sound" + Path.DirectorySeparatorChar;

			if (!Directory.Exists(CsvDir))
				CsvDir = ExeDir + "CSV" + Path.DirectorySeparatorChar;
			if (!Directory.Exists(ErbDir))
				ErbDir = ExeDir + "ERB" + Path.DirectorySeparatorChar;
			if (!Directory.Exists(DebugDir))
				DebugDir = ExeDir + "DEBUG" + Path.DirectorySeparatorChar;
			if (!Directory.Exists(DatDir))
				DatDir = ExeDir + "DAT" + Path.DirectorySeparatorChar;
			if (!Directory.Exists(ContentDir))
				ContentDir = ExeDir + "RESOURCES" + Path.DirectorySeparatorChar;
			if (!Directory.Exists(MusicDir))
				MusicDir = ExeDir + "SOUND" + Path.DirectorySeparatorChar;

			//エラー出力用
			//1815 .exeが東方板のNGワードに引っかかるそうなので除去
			ExeName = Path.GetFileNameWithoutExtension(Sys.ExeName);

			//解析モードの判定だけ先に行う
			//int argsStart = 0;
			#region EM_私家版_Emuera多言語化改造
			// List<string> otherArgs = new List<string>();
			// foreach (var arg in args)
            // {
			// 	//if ((args.Length > 0) && (args[0].Equals("-DEBUG", StringComparison.CurrentCultureIgnoreCase)))
			// 	if (arg.Equals("-DEBUG", StringComparison.CurrentCultureIgnoreCase))
			// 	{
			// 		// argsStart = 1;//デバッグモードかつ解析モード時に最初の1っこ(-DEBUG)を飛ばす
			// 		debugMode = true;
			// 	}
			// 	else if (arg.Equals("-GENLANG", StringComparison.CurrentCultureIgnoreCase))
			// 	{
			// 		Lang.GenerateDefaultLangFile();
			// 	}
			// 	else otherArgs.Add(arg);
			// }
			// //if (args.Length > argsStart)
			// if (otherArgs.Count > 0)
			#endregion

			#region EM_私家版_Emuera多言語化改造
			//Lang.LoadLanguageFile();
			#endregion
			#region EM_私家版_Icon指定機能
			// Icon icon = null;
			// {
			// 	var bmp = Utils.LoadImage(Utils.GetValidPath(Config.EmueraIcon));
			// 	if (bmp != null)
			// 	{
			// 		icon = Utils.MakeIconFromBmpFile(bmp);
			// 		bmp.Dispose();
			// 	}
			// }
			#endregion


			if (!Directory.Exists(CsvDir))
			{
				MessageBox.Show(Lang.UI.MainWindow.MsgBox.NoCsvFolder.Text, Lang.UI.MainWindow.MsgBox.FolderNotFound.Text);
				return false;
			}
			if (!Directory.Exists(ErbDir))
			{
				MessageBox.Show(Lang.UI.MainWindow.MsgBox.NoErbFolder.Text, Lang.UI.MainWindow.MsgBox.FolderNotFound.Text);
				return false;
			}

			ConfigData.Instance.LoadConfig();
			//二重起動の禁止かつ二重起動
			if ((!Config.AllowMultipleInstances) && (Sys.PrevInstance()))
			{
				//MessageBox.Show("多重起動を許可する場合、emuera.configを書き換えて下さい", "既に起動しています");
				MessageBox.Show(Lang.UI.MainWindow.MsgBox.MultiInstanceInfo.Text, Lang.UI.MainWindow.MsgBox.InstaceExists.Text);
				return false;
			}
			
			// if (debugMode)
			// {
			// 	ConfigData.Instance.LoadDebugConfig();
			// 	if (!Directory.Exists(DebugDir))
			// 	{
			// 		try
			// 		{
			// 			Directory.CreateDirectory(DebugDir);
			// 		}
			// 		catch
			// 		{
			// 			MessageBox.Show(Lang.UI.MainWindow.MsgBox.FailedCreateDebugFolder.Text, Lang.UI.MainWindow.MsgBox.FolderNotFound.Text);
			// 			return;
			// 		}
			// 	}
			// }
			// if (AnalysisMode)
			// {
			// 	AnalysisFiles = new List<string>();
				#region EM_私家版_Emuera多言語化改造
			// 	// for (int i = argsStart; i < args.Length; i++)
			// 	foreach (var arg in otherArgs)
			// 	{
			// 		//if (!File.Exists(args[i]) && !Directory.Exists(args[i]))
			// 		if (!File.Exists(arg) && !Directory.Exists(arg))
			// 		{
			// 			MessageBox.Show(Lang.UI.MainWindow.MsgBox.ArgPathNotExists.Text);
			// 			return;
			// 		}
			// 		//if ((File.GetAttributes(args[i]) & FileAttributes.Directory) == FileAttributes.Directory)
			// 		if ((File.GetAttributes(arg) & FileAttributes.Directory) == FileAttributes.Directory)
			// 		{
			// 			//List<KeyValuePair<string, string>> fnames = Config.GetFiles(args[i] + "\\", "*.ERB");
			// 			List<KeyValuePair<string, string>> fnames = Config.GetFiles(arg + "\\", "*.ERB");
			// 			for (int j = 0; j < fnames.Count; j++)
			// 			{
			// 				AnalysisFiles.Add(fnames[j].Value);
			// 			}
			// 		}
			// 		else
			// 		{
			// 			//if (Path.GetExtension(args[i]).ToUpper() != ".ERB")
			// 			if (Path.GetExtension(arg).ToUpper() != ".ERB")
			// 			{
			// 				MessageBox.Show(Lang.UI.MainWindow.MsgBox.InvalidArg.Text);
			// 				return;
			// 			}
			// 			//AnalysisFiles.Add(args[i]);
			// 			AnalysisFiles.Add(arg);
			// 		}
			// 	}
				#endregion
			// }

			//MainWindow win = null;
			//while (true)
			//{
			StartTime = WinmmTimer.TickCount;
			// 	using (win = new MainWindow())
			// 	{
					#region EM_私家版_Emuera多言語化改造
			// 		win.TranslateUI();
					#endregion
					#region EM_私家版_Icon指定機能
			// 		if (icon != null)
			// 			win.SetupIcon(icon);
					#endregion
			// 		Application.Run(win);
			// 		Content.AppContents.UnloadContents();
			// 		if (!Reboot)
			// 			break;
			// 		RebootWinState = win.WindowState;
			// 		if (win.WindowState == FormWindowState.Normal)
			// 		{
			// 			RebootClientY = win.ClientSize.Height;
			// 			RebootLocation = win.Location;
			// 		}
			// 		else
			// 		{
			// 			RebootClientY = 0;
			// 			RebootLocation = new Point();
			// 		}
			// 	}
			// 	//条件次第ではParserMediatorが空でない状態で再起動になる場合がある
			// 	ParserMediator.ClearWarningList();
			// 	ParserMediator.Initialize(null);
			// 	GlobalStatic.Reset();
			// 	//GC.Collect();
			// 	Reboot = false;
			// 	ConfigData.Instance.ReLoadConfig();
			// }

			return true;
		}

		public static void RebootMain()
		{
			Content.AppContents.UnloadContents();
			if (Reboot)
			{
				ParserMediator.ClearWarningList();
				ParserMediator.Initialize(null);
				GlobalStatic.Reset();
				//GC.Collect();
				ConfigData.Instance.ReLoadConfig();
			}
			WinmmTimer.Reset();
		}

		/// <summary>
		/// 実行ファイルのディレクトリ。最後に\を付けたstring
		/// </summary>
		public static string ExeDir { get; private set; }
		public static string CsvDir { get; private set; }
		public static string ErbDir { get; private set; }
		public static string DebugDir { get; private set; }
		public static string DatDir { get; private set; }
		public static string ContentDir { get; private set; }
		public static string ExeName { get; private set; }
		#region EE_PLAYSOUND系
		public static string MusicDir {get; private set; }
		//public static IPlayer[] sound = new IPlayer[10];
		//public static IPlayer bgm = null;
		#endregion

		public static bool Reboot = false;
		//public static int RebootClientX = 0;
		public static int RebootClientY = 0;
		//public static FormWindowState RebootWinState = FormWindowState.Normal;
		//public static Point RebootLocation;

		public static bool AnalysisMode = false;
		public static List<string> AnalysisFiles = null;

		public static bool debugMode = false;
		public static bool DebugMode { get { return debugMode; } }


		public static uint StartTime { get; private set; }

	}
}