using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using MinorShift._Library;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameData;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.Forms;
using MinorShift.Emuera;
using XEmuera;
using Xamarin.Essentials;
using Xamarin.Forms;
using XEmuera.Drawing;
using System.Threading;

namespace MinorShift.Emuera
{
	public partial class MainWindow
	{
		private void InitEmuera()
		{
			//if (Program.DebugMode)
			//	デバッグToolStripMenuItem.Visible = true;

			//mainPicBox.SetStyle();
			//initControlSizeAndLocation();
			//richTextBox1.ForeColor = Config.ForeColor;
			//richTextBox1.BackColor = Config.BackColor;
            //mainPicBox.BackColor = Config.BackColor;//これは実際には使用されないはず
            
			//this.BackColor = Config.BackColor;
			this.BackgroundColor = Config.BackColor;

			//richTextBox1.Font = Config.Font;
			//richTextBox1.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
			//folderSelectDialog.SelectedPath = Program.ErbDir;
			//folderSelectDialog.ShowNewFolderButton = false;

			//openFileDialog.InitialDirectory = Program.ErbDir;
			//openFileDialog.Filter = "ERBファイル (*.erb)|*.erb";
			//openFileDialog.FileName = "";
			//openFileDialog.Multiselect = true;
			//openFileDialog.RestoreDirectory = true;

			//string Emuera_verInfo = "Emuera Ver. " + emueraVer.FileVersion.Remove(5);
			//if (emueraVer.FileBuildPart > 0)
			//Emuera_verInfo += "+v" + emueraVer.FileBuildPart.ToString() + ((emueraVer.FilePrivatePart > 0) ? "." + emueraVer.FilePrivatePart.ToString() : "");
			//EmuVerToolStripTextBox.Text = Emuera_verInfo;

			//timer = new System.Timers.Timer();
			//timer.Elapsed += Timer_Elapsed;

			//console = new EmueraConsole(this);
			//timer.Enabled = true;

			Task.Run(InitConsole);

			//macroMenuItems[0] = マクロ01ToolStripMenuItem;
			//macroMenuItems[1] = マクロ02ToolStripMenuItem;
			//macroMenuItems[2] = マクロ03ToolStripMenuItem;
			//macroMenuItems[3] = マクロ04ToolStripMenuItem;
			//macroMenuItems[4] = マクロ05ToolStripMenuItem;
			//macroMenuItems[5] = マクロ06ToolStripMenuItem;
			//macroMenuItems[6] = マクロ07ToolStripMenuItem;
			//macroMenuItems[7] = マクロ08ToolStripMenuItem;
			//macroMenuItems[8] = マクロ09ToolStripMenuItem;
			//macroMenuItems[9] = マクロ10ToolStripMenuItem;
			//macroMenuItems[10] = マクロ11ToolStripMenuItem;
			//macroMenuItems[11] = マクロ12ToolStripMenuItem;
			//foreach(ToolStripMenuItem item in macroMenuItems)
			//	item.Click += new EventHandler(マクロToolStripMenuItem_Click);

			//this.richTextBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseWheel);
			//this.mainPicBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseWheel);
			//this.vScrollBar.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseWheel);
		}

		//private ToolStripMenuItem[] macroMenuItems = new ToolStripMenuItem[KeyMacro.MaxFkey];
		//private FileVersionInfo emueraVer = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
		public RelativeLayout MainLayout { get { return mainLayout; } }
		public EraPictureBox MainPicBox { get { return mainPicBox; } }
		public Slider ScrollBar { get { return vScrollBar; } }
		public Entry TextBox { get { return richTextBox1; } }

		//public string InternalEmueraVer { get { return emueraVer.FileVersion; } }
		public string InternalEmueraVer { get { return "1.824.0.0"; } }

		//public string EmueraVerText { get { return EmuVerToolStripTextBox.Text; } }
		public string EmueraVerText { get { return "1.824.*"; } }

		//public ToolTip ToolTip { get { return toolTipButton; } }

		private EmueraConsole console = null;

		//System.Timers.Timer timer;

		//protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		//{
		//	//1823 INPUTMOUSEKEY Key入力全てを捕まえてERB側で処理する
		//	//if (console != null && console.IsWaitingPrimitiveKey)
		//	if (console != null && console.IsWaitingPrimitive)
		//	{
		//		return false;
		//	}
		//	if ((keyData & Keys.KeyCode) == Keys.B && ((keyData & Keys.Modifiers) & Keys.Control) == Keys.Control)
		//	{
		//		if (WindowState != FormWindowState.Minimized)
		//		{
		//			WindowState = FormWindowState.Minimized;
		//			return true;
		//		}
		//	}
		//	else if (((keyData & Keys.KeyCode) == Keys.C && ((keyData & Keys.Modifiers) & Keys.Control) == Keys.Control) || (keyData & Keys.KeyCode) == Keys.Insert && ((keyData & Keys.Modifiers) & Keys.Control) == Keys.Control)
		//	{
		//		if (this.richTextBox1.SelectedText == "")
		//		{
		//			ClipBoardDialog dialog = new ClipBoardDialog();
		//			dialog.StartPosition = FormStartPosition.CenterParent;
		//			dialog.Setup(console);
		//			dialog.ShowDialog();
		//			return true;
		//		}
		//	}
		//	else if (((keyData & Keys.KeyCode) == Keys.V && ((keyData & Keys.Modifiers) & Keys.Control) == Keys.Control) || (keyData & Keys.KeyCode) == Keys.Insert && ((keyData & Keys.Modifiers) & Keys.Shift) == Keys.Shift)
		//	{
		//		if (Clipboard.GetDataObject() == null || !Clipboard.ContainsText())
		//			return true;
		//		else
		//		{
		//			if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
		//				richTextBox1.Paste(DataFormats.GetFormat(DataFormats.UnicodeText));
		//			return true;
		//		}
		//	}
		//	//else if (((int)keyData == (int)Keys.Control + (int)Keys.D) && Program.DebugMode)
		//	//{
		//	//    console.OpenDebugDialog();
		//	//    return true;
		//	//}
		//	//else if (((int)keyData == (int)Keys.Control + (int)Keys.R) && Program.DebugMode)
		//	//{
		//	//    if ((console.DebugDialog != null) && (console.DebugDialog.Created))
		//	//        console.DebugDialog.UpdateData();
		//	//}
		//	else if (Config.UseKeyMacro)
		//	{
		//		int keyCode = (int)(keyData & Keys.KeyCode);
		//		bool shiftPressed = (keyData & Keys.Modifiers) == Keys.Shift;
		//		bool ctrlPressed = (keyData & Keys.Modifiers) == Keys.Control;
		//		bool unPressed = (int)(keyData & Keys.Modifiers) == 0;
		//		if (keyCode >= (int)Keys.F1 && keyCode <= (int)Keys.F12)
		//		{
		//			int macroNum = keyCode - (int)Keys.F1;
		//			if (shiftPressed)
		//			{
		//				if (this.richTextBox1.Text != "")
		//					KeyMacro.SetMacro(macroNum, macroGroup, this.richTextBox1.Text);
		//				return true;
		//			}
		//			else if (unPressed)
		//			{
		//				this.richTextBox1.Text = KeyMacro.GetMacro(macroNum, macroGroup);
		//				this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
		//				return true;
		//			}
		//		}
		//		else if (ctrlPressed)
		//		{
		//			int newGroupNum = -1;
		//			if (keyCode >= (int)Keys.D0 && keyCode <= (int)Keys.D9)
		//				newGroupNum = keyCode - (int)Keys.D0;
		//			else if (keyCode >= (int)Keys.NumPad0 && keyCode <= (int)Keys.NumPad9)
		//				newGroupNum = keyCode - (int)Keys.NumPad0;
		//			if (newGroupNum >= 0)
		//			{
		//				setNewMacroGroup(newGroupNum);
		//			}
		//		}
		//	}
		//	return base.ProcessCmdKey(ref msg, keyData);
		//}


		//protected override void WndProc(ref Message m)
		//{
		//	const int WM_SYSCOMMAND = 0x112;
		//	//const int WM_MOUSEWHEEL = 0x020A;
		//	const int SC_MOVE = 0xf010;
		//	const int SC_MAXIMIZE = 0xf030;

		//	// WM_SYSCOMMAND (SC_MOVE) を無視することでフォームを移動できないようにする
		//	switch (m.Msg)
		//	{
		//	case WM_SYSCOMMAND:
		//		{
		//			int wparam = m.WParam.ToInt32() & 0xfff0;
		//			switch (wparam)
		//			{
		//			case SC_MOVE:
		//				if (WindowState == FormWindowState.Maximized)
		//					return;
		//				break;
		//			case SC_MAXIMIZE:
		//				if (Screen.AllScreens.Length == 1)
		//				{
		//					this.MaximizedBounds = SKRectI.Create(this.Left, 0, Config.WindowX, Screen.PrimaryScreen.WorkingArea.Height);
		//				}
		//				else
		//				{
		//					for (int i = 0; i < Screen.AllScreens.Length; i++)
		//					{
		//						if (this.Left >= Screen.AllScreens[i].Bounds.Left && this.Left < Screen.AllScreens[i].Bounds.Right)
		//						{
		//							this.MaximizedBounds = SKRectI.Create(this.Left - Screen.AllScreens[i].Bounds.Left, Screen.AllScreens[i].Bounds.Top, Config.WindowX, Screen.AllScreens[i].WorkingArea.Height);
		//							break;
		//						}
		//					}
		//				}
		//				break;
		//			}
		//			break;
		//		}

		//	//MouseWheelイベントをここで処理しようと思ったけどなんかここまで来ない (Windows 7)
		//	//case WM_MOUSEWHEEL:
		//	//	{
		//	//		if (!vScrollBar.Enabled)
		//	//			break;
		//	//		if (console == null)
		//	//			break;
		//	//		//int wparam_hiword = m.WParam.ToInt32() >> 16;
		//	//		int move = (m.WParam.ToInt32() >> 16) / 120 * -1;
		//	//		if ((vScrollBar.Value == vScrollBar.Maximum && move > 0) || (vScrollBar.Value == vScrollBar.Minimum && move < 0))
		//	//			break;
		//	//		int value = vScrollBar.Value + move;
		//	//		if (value >= vScrollBar.Maximum)
		//	//			vScrollBar.Value = vScrollBar.Maximum;
		//	//		else if (value <= vScrollBar.Minimum)
		//	//			vScrollBar.Value = vScrollBar.Minimum;
		//	//		else
		//	//			vScrollBar.Value = value;
		//	//		bool force_refresh = (vScrollBar.Value == vScrollBar.Maximum) || (vScrollBar.Value == vScrollBar.Minimum);

		//	//		//ボタンとの関係をチェック
		//	//		if (Config.UseMouse)
		//	//			force_refresh = console.MoveMouse(mainPicBox.PointToClient(Control.MousePosition)) || force_refresh;
		//	//		//上端でも下端でもなくボタン選択状態のアップデートも必要ないなら描画を控えめに。
		//	//		console.RefreshStrings(force_refresh);

		//	//		break;
		//	//	}
		//	}
		//	base.WndProc(ref m);
		//}

		//private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		//{
		//	//if (!this.Created)
		//	//	return;
		//	timer.Enabled = false;
		//	console.Initialize();
		//}

		private void InitConsole()
		{
			console = new EmueraConsole(this);
			//1823 INPUTMOUSEKEY Key入力全てを捕まえてERB側で処理する
			//if (console != null && console.IsWaitingPrimitiveKey)
			// if (console != null && console.IsWaitingPrimitive)
			// {
			// 	return false;
			// }
            #region EE_ホットキー拡張
        //     DialogResult result;
		// 	switch (keyData & Keys.KeyCode)
		// 	{
		// 		case Keys.B when (keyData & Keys.Modifiers & Keys.Control) == Keys.Control:
		// 			if (WindowState != FormWindowState.Minimized)
		// 			{
		// 				WindowState = FormWindowState.Minimized;
		// 				return true;
		// 			}
		// 			break;
		// 		case Keys.C when (keyData & Keys.Modifiers & Keys.Control) == Keys.Control:
		// 		case Keys.Insert when (keyData & Keys.Modifiers & Keys.Control) == Keys.Control:
		// 			if (TextBox.SelectedText == "")
		// 			{
		// 				var dialog = new ClipBoardDialog { StartPosition = FormStartPosition.CenterParent };
		// 				dialog.Setup(console);
		// 				dialog.ShowDialog();
		// 				return true;
		// 			}
		// 			break;
		// 		case Keys.O when (keyData & Keys.Modifiers & Keys.Control) == Keys.Control:
		// 		case Keys.Insert when (keyData & Keys.Modifiers & Keys.Control) == Keys.Control:
		// 			{
		// 				var doit = console != null;
		// 				if (console != null && console.IsInProcess)
		// 				{
		// 					MessageBox.Show("スクリプト動作中には使用できません");
		// 					doit = false;
		// 				}
		// 				if (doit)
		// 				{
		// 					result = openFileDialog.ShowDialog();
		// 					var filepath = new List<string>();
		// 					if (result == DialogResult.OK)
		// 					{
		// 						foreach (var fname in openFileDialog.FileNames)
		// 						{
		// 							if (!File.Exists(fname))
		// 							{
		// 								MessageBox.Show("ファイルがありません");
		// 								doit = false;
		// 							}
		// 							else if (Path.GetExtension(fname).ToUpper() != ".ERB")
		// 							{
		// 								MessageBox.Show("ERBファイル以外は読み込めません  , ファイル形式エラー"); //
		// 								doit = false;
		// 							}
		// 							if (fname.StartsWith(Program.ErbDir, StringComparison.OrdinalIgnoreCase))
		// 								filepath.Add(Program.ErbDir + fname.Substring(Program.ErbDir.Length));
		// 							else
		// 								filepath.Add(fname);
		// 						}
		// 						if (doit)
		// 						{
		// 							console.ReloadPartialErb(filepath);
		// 							return true;
		// 						}
		// 					}
		// 				}
		// 				break;
		// 			}
		// 		case Keys.T when (keyData & Keys.Modifiers & Keys.Control) == Keys.Control:
		// 		case Keys.Insert when (keyData & Keys.Modifiers & Keys.Control) == Keys.Control:
		// 			{
		// 				var doit = true;
		// 				{
		// 					if (console == null)
		// 						doit = false;
		// 					if (console != null && console.IsInProcess)
		// 					{
		// 						MessageBox.Show(
		// 							"スクリプト動作中には使用できません");
		// 						doit = false;
		// 					}
		// 					if (console.notToTitle)
		// 					{
		// 						if (console.byError)
		// 							MessageBox.Show(
		// 								"コード解析でエラーが発見されたため、タイトルへは飛べません");
		// 						else
		// 							MessageBox.Show(
		// 								"解析モードのためタイトルへは飛べません");
		// 						doit = false;
		// 					}
		// 					if (doit)
		// 					{
		// 						result = MessageBox.Show("タイトル画面に戻りますか？", "タイトルに戻る",
		// 							MessageBoxButtons.OKCancel);
		// 						if (result != DialogResult.OK)
		// 							doit = false;
		// 						if (doit)
		// 						{
		// 							GotoTitle();
		// 							return true;
		// 						}
		// 					}
		// 				}
		// 				break;
		// 			}
		// 		case Keys.R when (keyData & Keys.Modifiers & Keys.Control) == Keys.Control:
		// 		case Keys.Insert when (keyData & Keys.Modifiers & Keys.Control) == Keys.Control:
		// 			result = MessageBox.Show("再起動しますか？", "再起動", MessageBoxButtons.OKCancel);
		// 			if (result == DialogResult.OK)
		// 			{
		// 				Reboot();
		// 				return true;
		// 			}
		// 			break;
		// 		case Keys.V when (keyData & Keys.Modifiers & Keys.Control) == Keys.Control:
		// 		case Keys.Insert when (keyData & Keys.Modifiers & Keys.Shift) == Keys.Shift:
		// 			if (Clipboard.GetDataObject() == null || !Clipboard.ContainsText())
		// 			{
		// 				return true;
		// 			}
		// 			else
		// 			{
		// 				if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
		// 					TextBox.Paste(DataFormats.GetFormat(DataFormats.UnicodeText));
		// 				return true;
		// 			}
		// 		//else if (((int)keyData == (int)Keys.Control + (int)Keys.D) && Program.DebugMode)
		// 		//{
		// 		//    console.OpenDebugDialog();
		// 		//    return true;
		// 		//}
		// 		//else if (((int)keyData == (int)Keys.Control + (int)Keys.R) && Program.DebugMode)
		// 		//{
		// 		//    if ((console.DebugDialog != null) && (console.DebugDialog.Created))
		// 		//        console.DebugDialog.UpdateData();
		// 		//}
		// 		default:
		// 			if (Config.UseKeyMacro)
		// 			{
		// 				int keyCode = (int)(keyData & Keys.KeyCode);
		// 				bool shiftPressed = (keyData & Keys.Modifiers) == Keys.Shift;
		// 				bool ctrlPressed = (keyData & Keys.Modifiers) == Keys.Control;
		// 				bool unPressed = (int)(keyData & Keys.Modifiers) == 0;
		// 				if (keyCode >= (int)Keys.F1 && keyCode <= (int)Keys.F12)
		// 				{
		// 					int macroNum = keyCode - (int)Keys.F1;
		// 					if (shiftPressed)
		// 					{
		// 						if (this.richTextBox1.Text != "")
		// 							KeyMacro.SetMacro(macroNum, macroGroup, this.richTextBox1.Text);
		// 						return true;
		// 					}
		// 					else if (unPressed)
		// 					{
		// 						this.richTextBox1.Text = KeyMacro.GetMacro(macroNum, macroGroup);
		// 						this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
		// 						return true;
		// 					}
		// 				}
		// 				else if (ctrlPressed)
		// 				{
		// 					int newGroupNum = -1;
		// 					if (keyCode >= (int)Keys.D0 && keyCode <= (int)Keys.D9)
		// 						newGroupNum = keyCode - (int)Keys.D0;
		// 					else if (keyCode >= (int)Keys.NumPad0 && keyCode <= (int)Keys.NumPad9)
		// 						newGroupNum = keyCode - (int)Keys.NumPad0;
		// 					if (newGroupNum >= 0)
		// 					{
		// 						setNewMacroGroup(newGroupNum);
		// 					}
		// 				}
		// 			}
		// 			break;
		// 	}
		// 	return base.ProcessCmdKey(ref msg, keyData);
		// }
        #endregion

			DrawTextUtils.Reset();
			DrawBitmapUtils.Reset();

			console.Initialize();
		}

		/// <summary>
		/// 1819 リサイズ時の処理を全廃しAnchor&Dock処理にマルナゲ
		/// 初期設定のみここで行う。ついでに再起動時の位置・サイズ処理も追加
		/// </summary>
		//private void initControlSizeAndLocation()
		//{
		//	//Windowのサイズ設定
		//	int winWidth = Config.WindowX + vScrollBar.Width;
		//	int winHeight = Config.WindowY;
		//	bool winMaximize = false;
		//	if (Config.SizableWindow)
		//	{
		//		this.FormBorderStyle = FormBorderStyle.Sizable;
		//		this.MaximizeBox = true;
		//		winMaximize = (Config.WindowMaximixed || Program.RebootWinState == FormWindowState.Maximized);
		//	}
		//	else
		//	{
		//		this.FormBorderStyle = FormBorderStyle.Fixed3D;
		//		this.MaximizeBox = false;
		//	}

		//	int menuHeight;
		//	if (Config.UseMenu)
		//	{
		//		menuStrip.Enabled = true;
		//		menuStrip.Visible = true;
		//		winHeight += menuStrip.Height;
		//		menuHeight = menuStrip.Height;
		//	}
		//	else
		//	{
		//		menuStrip.Enabled = false;
		//		menuStrip.Visible = false;
		//		menuHeight = 0;
		//	}
		//	//Windowの位置設定
		//	if (Config.SetWindowPos)
		//	{
		//		this.StartPosition = FormStartPosition.Manual;
		//		this.Location = new Point(Config.WindowPosX, Config.WindowPosY);
		//	}
		//	else if(!winMaximize && Program.RebootLocation != new Point())
		//	{
		//		this.StartPosition = FormStartPosition.Manual;
		//		this.Location = Program.RebootLocation;
		//	}
		//	//Windowのサイズ設定・再起動時
		//	if (!winMaximize &&	(Program.RebootClientY > 0))
		//		winHeight = Program.RebootClientY;
		//	this.ClientSize = new Size(winWidth, winHeight);

		//	//EmuVerToolStripTextBox.Location = new Point(Config.WindowX - vScrollBar.Width - EmuVerToolStripTextBox.Width, 3);

		//	mainPicBox.Location = new Point(0, menuHeight);
		//	mainPicBox.Size = new Size(Config.WindowX, winHeight - menuHeight - Config.LineHeight);

		//	richTextBox1.Location = new Point(0, winHeight - Config.LineHeight);
		//	richTextBox1.Size = new Size(Config.WindowX, Config.LineHeight);
		//	vScrollBar.Location = new Point(winWidth - vScrollBar.Size.Width, menuHeight);
		//	vScrollBar.Size = new Size(vScrollBar.Size.Width, winHeight - menuHeight);

		//	int minimamY = 100;
		//	if (minimamY < menuHeight + Config.LineHeight * 2)
		//		minimamY = menuHeight + Config.LineHeight * 2;
		//	if (minimamY > this.Height)
		//		minimamY = this.Height;
		//	int maximamY = 2560;
		//	if (maximamY < this.Height)
		//		maximamY = this.Height;
		//	this.MinimumSize = new Size(this.Width, minimamY);
		//	this.MaximumSize = new Size(this.Width, maximamY);
		//	if (winMaximize)
		//		this.WindowState = FormWindowState.Maximized;
		//}


		//private void mainPicBox_MouseMove(object sender, MouseEventArgs e)
		//{
		//	if (!Config.UseMouse)
		//		return;
		//	if (console == null)
		//		return;
		//	if (console.MoveMouse(e.Location))
		//		console.RefreshStrings(true);
		//}

		bool changeTextbyMouse = false;
		//private void mainPicBox_MouseDown(object sender, MouseEventArgs e)
		//{
		//	if (!Config.UseMouse)
		//		return;
		//	if (console == null || console.IsInProcess)
		//		return;
		//	if (console.IsWaitingPrimitive)
		//	//			if (console.IsWaitingPrimitiveMouse)
		//	{
		//		console.MouseDown(e.Location, e.Button);
		//		return;
		//	}
		//	bool isBacklog = vScrollBar.Value != vScrollBar.Maximum;
		//	string str = console.SelectedString;

		//	if (isBacklog)
		//		if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
		//		{
		//			vScrollBar.Value = vScrollBar.Maximum;
		//			console.RefreshStrings(true);
		//		}
		//	if (console.IsWaitingEnterKey && !console.IsError && str == null)
		//	{
		//		if (isBacklog)
		//			return;
		//		if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
		//		{
		//			if (e.Button == MouseButtons.Right)
		//				PressEnterKey(true, true);
		//			else
		//				PressEnterKey(false, true);
		//			return;
		//		}
		//	}
		// 	#region EM_私家版_INPUT系機能拡張
		// 	else if (console.IsWaintingInputWithMouse && (!console.IsError && str != null))
		// 	{
		// 		if ((e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Right))
		// 		{
		// 			if (modifiersWhileWaintingInputWithMouse != null)
		// 			{
		// 				GlobalStatic.Process.InputInteger(2, (long)modifiersWhileWaintingInputWithMouse);
		// 			}
		// 			GlobalStatic.Process.InputString(1, str);
		// 			if (e.Button == MouseButtons.Right)
		// 			{
		// 				GlobalStatic.Process.InputInteger(1, 2);
		// 				PressEnterKey(true, true);
		// 			}
		// 			else
		// 			{
		// 				GlobalStatic.Process.InputInteger(1, 1);
		// 				PressEnterKey(false, true);
		// 			}
		// 			return;
		// 		}
		// 	}
		// 	#endregion
		// 	//左が押されたなら選択。
		// 	if (str != null && ((e.Button & MouseButtons.Left) == MouseButtons.Left))
		// 	{ 
		// 		changeTextbyMouse = console.IsWaintingOnePhrase;
		// 		richTextBox1.Text = str;
        //         //念のため
        //         if (console.IsWaintingOnePhrase)
        //             last_inputed = "";
		// 		//右が押しっぱなしならスキップ追加。
		// 		if ((Control.MouseButtons & MouseButtons.Right) == MouseButtons.Right)
		// 			PressEnterKey(true, true);
		// 		else
		// 			PressEnterKey(false, true);
		// 		return;
		// 	}
		// }

		//private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
		//{
		//	//上端でも下端でもないなら描画を控えめに。
		//	if (console == null)
		//		return;
		//	console.RefreshStrings(vScrollBar.Value == vScrollBar.Maximum || vScrollBar.Value == vScrollBar.Minimum);
		//}


		// EM_私家版_INPUT系機能拡張!

		bool PressEnterKeyInProcess;

		public void PressEnterKey(bool mesSkip, bool inputsByMouse)
		{
			if (IsInitializing(false))
				return;
			if (PressEnterKeyInProcess)
				return;
            if (console == null || console.IsInProcess)
                return;
			//if (console.inProcess)
			//{
			//	richTextBox1.Text = "";
			//	return;
			//}

			PressEnterKeyInProcess = true;
			Task.Run(() =>
			{
				PressEnterKeyAsync(mesSkip, inputsByMouse);
				PressEnterKeyInProcess = false;
			});
		}

		public void PressEnterKeyAsync(bool mesSkip, bool inputsByMouse)
		{
			string str = richTextBox1.Text ?? "";
            if (console.IsWaintingOnePhrase && last_inputed.Length > 0)
            {
                str = str.Remove(0, last_inputed.Length);
                last_inputed = "";
            }
            changeTextbyMouse = false;
			updateInputs(str);
			console.PressEnterKey(mesSkip, str, inputsByMouse);
		}

		string[] prevInputs = new string[100];
		int selectedInputs = 100;
		int lastSelected = 100;
		void updateInputs(string cur)
		{
            if (string.IsNullOrEmpty(cur))
            {
                return;
            }
            if (selectedInputs == prevInputs.Length || cur != prevInputs[prevInputs.Length - 1])
            {
                for (int i = 0; i < prevInputs.Length - 1; i++)
                {
                    prevInputs[i] = prevInputs[i + 1];
                }
                prevInputs[prevInputs.Length - 1] = cur;
                //1729a eramakerと同じ処理系に変更 1730a 再修正
                if (selectedInputs > 0 && selectedInputs != prevInputs.Length && cur == prevInputs[selectedInputs - 1])
                    lastSelected = --selectedInputs;
                else
                    lastSelected = 100;
            }
            else
            {
                lastSelected = selectedInputs;
            }
			//richTextBox1.Text = "";
			clear_richText();
			selectedInputs = prevInputs.Length;
		}

		//void movePrev(int move)
		//{
		//	if (move == 0)
		//		return;
		//	//if((selectedInputs != prevInputs.Length) &&(prevInputs[selectedInputs] != richTextBox1.Text))
		//	//	selectedInputs =  prevInputs.Length;
		//	int next;
		//	if (lastSelected != prevInputs.Length && selectedInputs == prevInputs.Length)
		//	{
		//		if (move == -1)
		//			move = 0;
		//		next = lastSelected + move;
		//		lastSelected = prevInputs.Length;
		//	}
		//	else
		//		next = selectedInputs + move;
		//	if (next < 0 || next > prevInputs.Length)
		//		return;
		//	if (next == prevInputs.Length)
		//	{
		//		selectedInputs = next;
		//		richTextBox1.Text = "";
		//		return;
		//	}
		//	if (string.IsNullOrEmpty(prevInputs[next]))
		//		if (++next == prevInputs.Length)
		//			return;

		//	selectedInputs = next;
		//	richTextBox1.Text = prevInputs[next];
		//	richTextBox1.SelectionStart = 0;
		//	richTextBox1.SelectionLength = richTextBox1.Text.Length;
		//	return;
		//}

		//private async void exitToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	DialogResult result = await MessageBox.Show("ゲームを終了します", "終了", MessageBoxButtons.OKCancel);
		//	if (result != DialogResult.OK)
		//		return;
		//	this.Close();

		//}

		//private async void rebootToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	DialogResult result = await MessageBox.Show("ゲームを再起動します", "再起動", MessageBoxButtons.OKCancel);
		//	if (result != DialogResult.OK)
		//		return;
		//	this.Reboot();
		//}

		//private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//    openFileDialog.InitialDirectory = StaticConfig.SavDir;
		//    DialogResult result = openFileDialog.ShowDialog();
		//    string filepath = openFileDialog.FileName;
		//    if (!File.Exists(filepath))
		//    {
		//        MessageBox.Show("ファイルがありません", "File Not Found");
		//        return;
		//    }
		//}

		public void Reboot()
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				console.forceStopTimer();
				Program.Reboot = true;
				Close();
			});
		}

		public void GotoTitle()
		{
            if (console == null)
                return;
			Task.Run(() =>
			{
				console.GotoTitle();
				console.ClearQuickButton();
				console.RefreshQuickButton();
			});
		}

		public void ReloadErb()
		{
            if (console == null)
                return;
            console.ReloadErb();
		}

		private void mainPicBox_MouseLeave(object sender, EventArgs e)
		{
            if (console == null)
                return;
            if (Config.UseMouse)
				console.LeaveMouse();
		}

		//private void コンフィグCToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	ShowConfigDialog();
		//}

		//public void ShowConfigDialog()
		//{

		//	ConfigDialog dialog = new ConfigDialog();
		//	dialog.StartPosition = FormStartPosition.CenterParent;
		//	dialog.SetConfig(this);
		//	dialog.ShowDialog();
		//	if (dialog.Result == ConfigDialogResult.SaveReboot)
		//	{
		//		console.forceStopTimer();
		//		Program.Reboot = true;
		//		this.Close();
		//	}
		//}

		//private async void タイトルへ戻るTToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	if (console == null)
		//		return;
		//	if (console.IsInProcess)
		//	{
		//		MessageBox.Show("スクリプト動作中には使用できません");
		//		return;
		//	}
		//	if (console.notToTitle)
		//	{
		//		if (console.byError)
		//			MessageBox.Show("コード解析でエラーが発見されたため、タイトルへは飛べません");
		//		else
		//			MessageBox.Show("解析モードのためタイトルへは飛べません");
		//		return;
		//	}
		//	DialogResult result = await MessageBox.Show("タイトル画面へ戻ります", "タイトル画面に戻る", MessageBoxButtons.OKCancel);
		//	if (result != DialogResult.OK)
		//		return;
		//	this.GotoTitle();
		//}

		//private async void コードを読み直すcToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	if (console == null)
		//		return;
		//	if (console.IsInProcess)
		//	{
		//		MessageBox.Show("スクリプト動作中には使用できません");
		//		return;
		//	}
		//	DialogResult result = await MessageBox.Show("ERBファイルを読み直します", "ERBファイル読み直し", MessageBoxButtons.OKCancel);
		//	if (result != DialogResult.OK)
		//		return;
		//	this.ReloadErb();

		//}

		private void mainPicBox_Paint(object sender, SKPaintSurfaceEventArgs e)
		{
			// if (console == null)
			// 	return;
			// saveFileDialog.InitialDirectory = Program.ExeDir;
			// DateTime time = DateTime.Now;
			// string fname = time.ToString("yyyyMMdd-HHmmss");
			// fname += ".log";
			// saveFileDialog.FileName = fname;
			// DialogResult result = saveFileDialog.ShowDialog();
			// if (result == DialogResult.OK)
			// {
			// 	#region EE_OUTPUTLOG
			// 	// console.OutputLog(Path.GetFullPath(saveFileDialog.FileName));
			// 	console.OutputSystemLog(Path.GetFullPath(saveFileDialog.FileName));
			// 	#endregion

			// }

			if (IsWindowClosing)
				return;

			SKCanvas canvas = e.Surface.Canvas;
			console.OnPaint(canvas);
		}

		//private void ログを保存するSToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	if (console == null)
		//		return;
		//	saveFileDialog.InitialDirectory = Program.ExeDir;
		//	DateTime time = DateTime.Now;
		//	string fname = time.ToString("yyyyMMdd-HHmmss");
		//	fname += ".log";
		//	saveFileDialog.FileName = fname;
		//	DialogResult result = saveFileDialog.ShowDialog();
		//	if (result == DialogResult.OK)
		//	{
		//		console.OutputLog(Path.GetFullPath(saveFileDialog.FileName));
		//	}
		//}

		//private void ログをクリップボードにコピーToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	try
		//	{
		//		ClipBoardDialog dialog = new ClipBoardDialog();
		//		dialog.Setup(console);
		//		dialog.ShowDialog();
		//	}
		//	catch (Exception)
		//	{
		//		MessageBox.Show("予期せぬエラーが発生したためクリップボードを開けません");
		//		return;
		//	}
		//}

		//private void ファイルを読み直すFToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	if (console == null)
		//		return;
		//	if (console.IsInProcess)
		//	{
		//		MessageBox.Show("スクリプト動作中には使用できません");
		//		return;
		//	}
		//	DialogResult result = openFileDialog.ShowDialog();
		//	List<string> filepath = new List<string>();
		//	if (result == DialogResult.OK)
		//	{
		//		foreach (string fname in openFileDialog.FileNames)
		//		{
		//			if (!File.Exists(fname))
		//			{
		//				MessageBox.Show("ファイルがありません", "File Not Found");
		//				return;
		//			}
		//			if (Path.GetExtension(fname).ToUpper() != ".ERB")
		//			{
		//				MessageBox.Show("ERBファイル以外は読み込めません", "ファイル形式エラー");
		//				return;
		//			}
		//			if (fname.StartsWith(Program.ErbDir, StringComparison.OrdinalIgnoreCase))
		//				filepath.Add(Program.ErbDir + fname.Substring(Program.ErbDir.Length));
		//			else
		//				filepath.Add(fname);
		//		}
		//		console.ReloadPartialErb(filepath);
		//	}
		//}

		//private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
		//{
		//	if (Config.UseKeyMacro)
		//		KeyMacro.SaveMacro();
		//	if (console != null)
		//	{
		//		//ほっとしても勝手に閉じるが、その場合はDebugDialogのClosingイベントが発生しない
		//		if ((Program.DebugMode) && (console.DebugDialog != null) && (console.DebugDialog.Created))
		//			console.DebugDialog.Close();
		//		console.Dispose();
		//		console = null;
		//	}
		//}

		//private void フォルダを読み直すFToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	if (console == null)
		//		return;
		//	if (console.IsInProcess)
		//	{
		//		MessageBox.Show("スクリプト動作中には使用できません");
		//		return;
		//	}
		//	//List<KeyValuePair<string, string>> filepath = new List<KeyValuePair<string, string>>();
		//	if (folderSelectDialog.ShowDialog() == DialogResult.OK)
		//	{
		//		console.ReloadFolder(folderSelectDialog.SelectedPath);
		//	}
		//}

		//void richTextBox1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		//{
		//	//if (!Config.UseMouse)
		//	//	return;
		//	if (!vScrollBar.IsEnabled)
		//		return;
		//	if (console == null)
		//		return;

		//	if (console.IsWaitingPrimitive)
		//	//			if (console.IsWaitingPrimitiveMouse)
		//	{
		//		console.MouseWheel(mainPicBox.PointToClient(Control.MousePosition), e.Delta);
		//		return;
		//	}
		//	//e.Deltaには大きな値が入っているので符号のみ採用する
		//	int move = -Math.Sign(e.Delta) * vScrollBar.SmallChange * Config.ScrollHeight;
		//	//スクロールが必要ないならリターンする
		//	if (vScrollBar.Value == vScrollBar.Maximum && move > 0 || vScrollBar.Value == vScrollBar.Minimum && move < 0)
		//		return;
		//	int value = vScrollBar.Value + move;
		//	if (value >= vScrollBar.Maximum)
		//		vScrollBar.Value = vScrollBar.Maximum;
		//	else if (value <= vScrollBar.Minimum)
		//		vScrollBar.Value = vScrollBar.Minimum;
		//	else
		//		vScrollBar.Value = value;
		//	bool force_refresh = vScrollBar.Value == vScrollBar.Maximum || vScrollBar.Value == vScrollBar.Minimum;

		//	//ボタンとの関係をチェック
		//	if (Config.UseMouse)
		//		force_refresh = console.MoveMouse(mainPicBox.PointToClient(Control.MousePosition)) || force_refresh;
		//	//上端でも下端でもなくボタン選択状態のアップデートも必要ないなら描画を控えめに。
		//	console.RefreshStrings(force_refresh);
		//}

        private bool textBox_flag = true;
        private string last_inputed = "";

        public void update_lastinput()
        {
			//richTextBox1.TextChanged -= new EventHandler(richTextBox1_TextChanged);
			richTextBox1.TextChanged -= richTextBox1_TextChanged;
			//richTextBox1.KeyDown -= new KeyEventHandler(richTextBox1_KeyDown);
			App.DoEvents();
			//richTextBox1.TextChanged += new EventHandler(richTextBox1_TextChanged);
			richTextBox1.TextChanged += richTextBox1_TextChanged;
			//richTextBox1.KeyDown += new KeyEventHandler(richTextBox1_KeyDown);
			last_inputed = richTextBox1.Text ?? "";
        }

        public void clear_richText()
        {
			//richTextBox1.Clear();
			MainThread.BeginInvokeOnMainThread(() =>
			{
				richTextBox1.Text = "";
			});
        }

		private void richTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (console == null || console.IsInProcess)
                return;
            if (!textBox_flag)
                return;
            if (!console.IsWaintingOnePhrase && !console.IsWaitAnyKey)
                return;
            if (string.IsNullOrEmpty(richTextBox1.Text))
                return;
            if (changeTextbyMouse)
                return;
            //テキストの削除orテキストに変化がない場合は入力されたとみなさない
            if (richTextBox1.Text.Length <= last_inputed.Length)
            {
                last_inputed = richTextBox1.Text;
                return;
            }
            textBox_flag = false;
            if (console.IsWaitAnyKey)
            {
				//richTextBox1.Clear();
				clear_richText();
                last_inputed = "";
            }
            //if (richTextBox1.Text.Length > 1)
            //    richTextBox1.Text = richTextBox1.Text.Remove(1);
            PressEnterKey(false, false);
            textBox_flag = true;
		}
		#region EM_私家版_INPUT系機能拡張
		// Keys? modifiersWhileWaintingInputWithMouse = null;
		// private void richTextBox1_ModifierRecorder_KeyUp(object sender, KeyEventArgs e)
		// {
		// 	if (console == null || !console.IsWaintingInputWithMouse)
		// 		return;
		// 	modifiersWhileWaintingInputWithMouse = null;
		// }
		// private void richTextBox1_ModifierRecorder_KeyDown(object sender, KeyEventArgs e)
		// {
		// 	if (console == null || !console.IsWaintingInputWithMouse)
		// 		return;
		// 	modifiersWhileWaintingInputWithMouse = e.Modifiers;
		// }
		#endregion

		//private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
		//{
		//	if (console == null)
		//		return;
		//	//1823 INPUTMOUSEKEY Key入力全てを捕まえてERB側で処理する
		//	//if (console.IsWaitingPrimitiveKey)
		//	if (console.IsWaitingPrimitive)
		//	{
		//		e.SuppressKeyPress = true;
		//		console.PressPrimitiveKey(e.KeyCode, e.KeyData, e.Modifiers);
		//		return;
		//	}
		//	if ((int)e.KeyData == (int)Keys.PageUp || (int)e.KeyData == (int)Keys.PageDown)
		//	{
		//		e.SuppressKeyPress = true;
		//		int move = 10;
		//		if ((int)e.KeyData == (int)Keys.PageUp)
		//			move *= -1;
		//		//スクロールが必要ないならリターンする
		//		if (vScrollBar.Value == vScrollBar.Maximum && move > 0 || vScrollBar.Value == vScrollBar.Minimum && move < 0)
		//			return;
		//		int value = vScrollBar.Value + move;
		//		if (value >= vScrollBar.Maximum)
		//			vScrollBar.Value = vScrollBar.Maximum;
		//		else if (value <= vScrollBar.Minimum)
		//			vScrollBar.Value = vScrollBar.Minimum;
		//		else
		//			vScrollBar.Value = value;
		//		//上端でも下端でもないなら描画を控えめに。
		//		console.RefreshStrings(vScrollBar.Value == vScrollBar.Maximum || vScrollBar.Value == vScrollBar.Minimum);
		//		return;
		//	}
		//	else if (vScrollBar.Value != vScrollBar.Maximum)
		//	{
		//		vScrollBar.Value = vScrollBar.Maximum;
		//		console.RefreshStrings(true);
		//	}
		//	if (e.KeyCode == Keys.Return)
		//	{
		//		e.SuppressKeyPress = true;
		//		if (!console.IsInProcess)
		//			PressEnterKey(false, false);
		//		return;
		//	}
		//	if (e.KeyCode == Keys.Escape)
		//	{
		//		e.SuppressKeyPress = true;
		//		console.KillMacro = true;
		//		if (!console.IsInProcess)
		//			PressEnterKey(true, false);
		//		return;
		//	}
		//	if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Home || e.KeyCode == Keys.Back)
		//	{
		//		if (richTextBox1.SelectionStart == 0 && richTextBox1.SelectedText.Length == 0 || richTextBox1.Text.Length == 0)
		//		{
		//			e.SuppressKeyPress = true;
		//			return;
		//		}
		//	}
		//	if (e.KeyCode == Keys.Right || e.KeyCode == Keys.End)
		//	{
		//		if (richTextBox1.SelectionStart == richTextBox1.Text.Length || richTextBox1.Text.Length == 0)
		//		{
		//			e.SuppressKeyPress = true;
		//			return;
		//		}
		//	}
		//	if (e.KeyCode == Keys.Up)
		//	{
		//		e.SuppressKeyPress = true;
		//		if (console.IsInProcess)
		//			return;
		//		movePrev(-1);
		//		return;
		//	}
		//	if (e.KeyCode == Keys.Down)
		//	{
		//		e.SuppressKeyPress = true;
		//		if (console.IsInProcess)
		//			return;
		//		movePrev(1);
		//		return;
		//	}
		//	if (e.KeyCode == Keys.Insert)
		//	{
		//		e.SuppressKeyPress = true;
		//		return;
		//	}
		//}

		//private void デバッグウインドウを開くToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	if (!Program.DebugMode)
		//		return;
		//	console.OpenDebugDialog();
		//}

		//private void デバッグ情報の更新ToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	if (!Program.DebugMode)
		//		return;
		//	if ((console.DebugDialog != null) && (console.DebugDialog.Created))
		//		console.DebugDialog.UpdateData();
		//}

		//private void AutoVerbMenu_Opened(object sender, EventArgs e)
		//{
		//	if ((console == null) || (console.IsInProcess))
		//	{
		//		切り取り.Enabled = false;
		//		コピー.Enabled = false;
		//		貼り付け.Enabled = false;
		//		実行.Enabled = false;
		//		削除.Enabled = false;
		//		マクロToolStripMenuItem.Enabled = false;
		//		for (int i = 0; i < macroMenuItems.Length; i++)
		//			macroMenuItems[i].Enabled = false;
		//		return;
		//	}
		//	実行.Enabled = true;
		//	if (Config.UseKeyMacro)
		//	{
		//		マクロToolStripMenuItem.Enabled = true;

		//		for (int i = 0; i < macroMenuItems.Length; i++)
		//			macroMenuItems[i].Enabled = KeyMacro.GetMacro(i, macroGroup).Length > 0;
		//	}
		//	else
		//	{
		//		マクロToolStripMenuItem.Enabled = false;
		//		for (int i = 0; i < macroMenuItems.Length; i++)
		//			macroMenuItems[i].Enabled = false;
		//	}
		//	if( richTextBox1.SelectedText.Length > 0)
		//	{
		//		切り取り.Enabled = true;
		//		コピー.Enabled = true;
		//		削除.Enabled = true;
		//	}
		//	else
		//	{
		//		切り取り.Enabled = false;
		//		コピー.Enabled = false;
		//		削除.Enabled = false;
		//	}
		//	if(Clipboard.ContainsText())
		//		貼り付け.Enabled = true;
		//	else
		//		貼り付け.Enabled = false;

		//}

		//private void 切り取り_Click(object sender, EventArgs e)
		//{
		//	if ((console == null) || (console.IsInProcess) || !切り取り.Enabled)
		//		return;
		//	if (richTextBox1.SelectedText.Length > 0)
		//		richTextBox1.Cut();
		//}

		//private void コピー_Click(object sender, EventArgs e)
		//{
		//	if ((console == null) || (console.IsInProcess) || !コピー.Enabled)
		//		return;
		//	else if (richTextBox1.SelectedText.Length > 0)
		//		richTextBox1.Copy();
		//}

		//private void 貼り付け_Click(object sender, EventArgs e)
		//{
		//	if ((console == null) || (console.IsInProcess) || !貼り付け.Enabled)
		//		return;
		//	if (Clipboard.GetDataObject() != null && Clipboard.ContainsText())
		//	{
		//		if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
		//			//Clipboard.SetText(Clipboard.GetText(TextDataFormat.UnicodeText));
		//			richTextBox1.Paste(DataFormats.GetFormat(DataFormats.UnicodeText));
		//		//richTextBox1.Paste();
		//		//if (richTextBox1.SelectedText.Length > 0)
		//		//    richTextBox1.SelectedText = "";
		//		//richTextBox1.AppendText(Clipboard.GetText());
		//	}
		//}

		//private void 削除_Click(object sender, EventArgs e)
		//{
		//	if ((console == null) || (console.IsInProcess) || !削除.Enabled)
		//		return;
		//	if (richTextBox1.SelectedText.Length > 0)
		//		richTextBox1.SelectedText = "";
		//}

		//private void 実行_Click(object sender, EventArgs e)
		//{
		//	if (console == null || console.IsInProcess || !実行.Enabled)
		//		return;
		//	PressEnterKey(false, false);
		//}


		int macroGroup = 0;
		//private void マクロToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	if ((console == null) || (console.IsInProcess))
		//		return;
		//	if (!Config.UseKeyMacro)
		//		return;
		//	ToolStripMenuItem item = (ToolStripMenuItem)sender;
		//	int fkeynum = (int)item.ShortcutKeys - (int)Keys.F1;
		//	string macro = KeyMacro.GetMacro(fkeynum, macroGroup);
		//          if (macro.Length > 0)
		//          {
		//              richTextBox1.Text = macro;
		//              this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
		//          }
		//}

		//private void グループToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//	if ((console == null) || (console.IsInProcess))
		//		return;
		//	if (!Config.UseKeyMacro)
		//		return;
		//	ToolStripMenuItem item = (ToolStripMenuItem)sender;
		//	setNewMacroGroup(int.Parse((string)item.Tag));//とても無駄なキャスト&Parse
		//}

		//private void timerKeyMacroChanged_Tick(object sender, EventArgs e)
		//{
		//	labelTimerCount++;
		//	if (labelTimerCount > 10)
		//	{
		//		timerKeyMacroChanged.Stop();
		//		timerKeyMacroChanged.Enabled = false;
		//		labelMacroGroupChanged.Visible = false;
		//	}
		//}

		//private void TimerKeyMacroChanged_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		//{
		//	labelTimerCount++;
		//	if (labelTimerCount > 10)
		//	{
		//		timerKeyMacroChanged.Stop();
		//		timerKeyMacroChanged.Enabled = false;
		//		labelMacroGroupChanged.Visible = false;
		//	}
		//}

		int labelTimerCount = 0;

		//private void setNewMacroGroup(int group)
		//{
		//	labelTimerCount = 0;
		//	macroGroup = group;
		//	labelMacroGroupChanged.Text = KeyMacro.GetGroupName(group);
		//	timerKeyMacroChanged.Interval = 200;
		//	timerKeyMacroChanged.Enabled = true;
		//	timerKeyMacroChanged.Start();
		//	labelMacroGroupChanged.Location = new Point(4, richTextBox1.Location.Y - labelMacroGroupChanged.Height - 4);
		//	labelMacroGroupChanged.Visible = true;
		//}
	}
}
