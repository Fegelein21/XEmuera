﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using XEmuera.Forms;
using System.IO;
using MinorShift._Library;
using MinorShift.Emuera.Sub;
//using MinorShift.Emuera.GameData;
using MinorShift.Emuera.GameProc;
//using System.Drawing.Imaging;
using MinorShift.Emuera.Forms;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameProc.Function;
using MinorShift.Emuera.Content;
using System.Timers;
using SkiaSharp.Views.Forms;
using XEmuera.Drawing;
using SkiaSharp;
using XEmuera;
using Xamarin.Essentials;
using EvilMask.Emuera;

using trmb = EvilMask.Emuera.Lang.MessageBox;
using trerror = EvilMask.Emuera.Lang.Error;
using trsl = EvilMask.Emuera.Lang.SystemLine;

namespace MinorShift.Emuera.GameView
{
	//入出力待ちの状況。
	//難読化用属性。enum.ToString()やenum.Parse()を行うなら(Exclude=true)にすること。
	[global::System.Reflection.Obfuscation(Exclude=false)]
	internal enum ConsoleState
	{
		Initializing = 0,
		Quit = 5,//QUIT
		Error = 6,//Exceptionによる強制終了
		Running = 7,
		WaitInput = 20,
		Sleep = 21,//DoEvents

		//WaitKey = 1,//WAIT
		//WaitSystemInteger = 2,//Systemが要求するInput
		//WaitInteger = 3,//INPUT
		//WaitString = 4,//INPUTS
		//WaitIntegerWithTimer = 8,
		//WaitStringWithTimer = 9,
		//Timeout = 10,
		//Timeouts = 11,
		//WaitKeyWithTimer = 12,
		//WaitKeyWithTimerF = 13,
		//WaitOneInteger = 14,
		//WaitOneString = 15,
		//WaitOneIntegerWithTimer = 16,
		//WaitOneStringWithTimer = 17,
		//WaitAnyKey = 18,

    }

	//難読化用属性。enum.ToString()やenum.Parse()を行うなら(Exclude=true)にすること。
	[global::System.Reflection.Obfuscation(Exclude=false)]
	internal enum ConsoleRedraw
	{
		None = 0,
		Normal = 1,
	}

	internal sealed partial class EmueraConsole :IDisposable
	{
		public EmueraConsole(MainWindow parent)
		{
			window = parent;

			//1.713 この段階でsetStBarを使用してはいけない
			//setStBar(StaticConfig.DrawLineString);
			state = ConsoleState.Initializing;
			if (Config.FPS > 0)
				msPerFrame = 1000 / (uint)Config.FPS;
			displayLineList = new List<ConsoleDisplayLine>();
			printBuffer = new PrintStringBuffer(this);

			timer = new Timer();
			timer.Enabled = false;
			timer.Elapsed += tickTimer;
			timer.Interval = 10;
			CBG_Clear();//文字列描画用ダミー追加

			redrawTimer = new Timer();
			redrawTimer.Enabled = false;//TODO:1824アニメ用再描画タイマー有効化関数の追加
			redrawTimer.Elapsed += tickRedrawTimer;
			redrawTimer.Interval = 10;
        }
#region 1823 cbg関連
		private readonly List<ClientBackGroundImage> cbgList = new List<ClientBackGroundImage>();
		private GraphicsImage cbgButtonMap = null;
		private int selectingCBGButtonInt = -1;
		private int lastSelectingCBGButtonInt = -1;
		//ConsoleButtonString selectingButton = null;
		//ConsoleButtonString lastSelectingButton = null;
		class ClientBackGroundImage : IComparable<ClientBackGroundImage>
		{
			/// <summary>
			/// zdepth == 0は文字列用ダミーなので他で使ってはいけない
			/// </summary>
			/// <param name="zdepth"></param>
			internal ClientBackGroundImage(int zdepth)
			{ this.zdepth = zdepth; }
			public ASprite Img = null;
			public ASprite ImgB = null;
			public int x;
			public int y;
			public readonly int zdepth;
			public bool isButton = false;
			public int buttonValue;
			public string tooltipString = null;
			public int CompareTo(ClientBackGroundImage other)
			{
				if (other == null)
					return -1;
				//逆順でSort
				return -zdepth.CompareTo(other.zdepth);
			}
		}
		public void CBG_Clear()
		{
			foreach(ClientBackGroundImage cimg in cbgList)
			{
				//使い捨て無名Imageを一応disposeしておく
				if (cimg.Img != null && cimg.Img.Name.Length == 0)
					cimg.Img.Dispose();
			}
			cbgList.Clear();
			CBG_ClearBMap();
			cbgList.Add(new ClientBackGroundImage(0));
		}

		public void CBG_ClearRange(int zmin, int zmax)
		{
			if (zmin > zmax)
				return;
			for (int i = 0; i < cbgList.Count;i++)
			{
				ClientBackGroundImage cimg = cbgList[i];
				if (cimg.zdepth < zmin || cimg.zdepth > zmax || cimg.zdepth == 0)//0はダミーなので削除しない
					continue;

				//使い捨て無名Imageを一応disposeしておく
				if (cimg.Img != null && cimg.Img.Name.Length == 0)
					cimg.Img.Dispose();
				cbgList.RemoveAt(i);
				i--;
			}
		}

		public void CBG_ClearButton()
		{
			for (int i = 0; i < cbgList.Count; i++)
			{
				ClientBackGroundImage cimg = cbgList[i];
				if (!cimg.isButton)
					continue;

				//使い捨て無名Imageを一応disposeしておく
				if (cimg.Img != null && cimg.Img.Name.Length == 0)
					cimg.Img.Dispose();
				cbgList.RemoveAt(i);
				i--;
			}
			CBG_ClearBMap();
		}

		public void CBG_ClearBMap()
		{
			cbgButtonMap = null;
			selectingCBGButtonInt = -1;
			lastSelectingCBGButtonInt = -1;
		}

		public bool CBG_SetGraphics(GraphicsImage gra, int x, int y, int zdepth)
		{
			if (gra == null || !gra.IsCreated)
				return false;
			return CBG_SetImage(new SpriteG("", gra, new Rectangle(0, 0, gra.Width, gra.Height)), x, y, zdepth);
		}
		public bool CBG_SetImage(ASprite image, int x, int y, int zdepth)
		{
			if (image == null || !image.IsCreated)
				return false;
			if (zdepth == 0)
				throw new ArgumentOutOfRangeException();
			ClientBackGroundImage cbg = new ClientBackGroundImage(zdepth);
			cbg.Img = image;
			cbg.x = x;
			cbg.y = y;
			//cbg.zdepth = zdepth;
			cbgList.Add(cbg);
			cbgList.Sort();
			return true;
		}

		public bool CBG_SetButtonMap(GraphicsImage gra)
		{
			if (gra == null || !gra.IsCreated)
				return false;
			if (cbgButtonMap == gra)
				return false;
			cbgButtonMap = gra;
			selectingCBGButtonInt = -1;
			lastSelectingCBGButtonInt = -1;
			return true;
		}

		public bool CBG_SetButtonImage(int buttonValue, ASprite imageN, ASprite imageB, int x, int y, int zdepth, string tooltip = null)
		{
			if (zdepth == 0)
				throw new ArgumentOutOfRangeException();
			ClientBackGroundImage cbg = new ClientBackGroundImage(zdepth);
			cbg.Img = imageN;
			cbg.ImgB = imageB;
			cbg.x = x;
			cbg.y = y;
			//cbg.zdepth = zdepth;
			cbg.isButton = true;
			cbg.buttonValue = buttonValue;
			cbg.tooltipString = tooltip;
			cbgList.Add(cbg);
			cbgList.Sort();
			return true;
		}
		public int ClientWidth { get { return DisplayUtils.PicBoxWidth; } }
		public int ClientHeight { get { return DisplayUtils.PicBoxHeight; } }
#endregion

		const string ErrorButtonsText = "__openFileWithDebug__";
        private readonly MainWindow window;

		MinorShift.Emuera.GameProc.Process emuera;
		ConsoleState state = ConsoleState.Initializing;
		//public bool Enabled { get { return window.Created; } }
		public bool Enabled { get { return true; } }

		/// <summary>
		/// 現在、Emueraがアクティブかどうか
		/// </summary>
		internal bool IsActive
		//{ get { return !(window == null || !window.Created || Form.ActiveForm == null); } }
		{ get { return true; } }

		/// <summary>
		/// スクリプトが継続中かどうか
		/// 入力系はメッセージスキップやマクロも含めてIsInProcessを参照すべき
		/// </summary>
		internal bool IsRunning
		{
			get
			{
				if (state == ConsoleState.Initializing)
					return true;
				return (state == ConsoleState.Running || runningERBfromMemory);
			}
		}
		#region EM_私家版_INPUT系機能拡張
		internal bool IsWaintingInputWithMouse
		{
			get
			{
				return (state == ConsoleState.WaitInput && inputReq.MouseInput);
			}
		}
		#endregion
		internal bool IsInProcess
		{
			get
			{
				if (state == ConsoleState.Initializing)
					return true;
				if (state == ConsoleState.Sleep)
					return true;
				if (inProcess)
					return true;
				return (state == ConsoleState.Running || runningERBfromMemory);
			}
		}

		internal bool IsError
		{
			get
			{
				return state == ConsoleState.Error;
			}
		}
		#region EE_連続FORCE_QUIT_AND_RESTAR対策
		internal bool IsWaitingEnterKey
		{
			get
			{
				if ((state == ConsoleState.Quit) || (state == ConsoleState.Error))
				{
					GlobalStatic.ForceQuitAndRestart = false;
					return true;
				}
				if (state == ConsoleState.WaitInput)
				{
					GlobalStatic.ForceQuitAndRestart = false;
					return (inputReq.InputType == InputType.AnyKey || inputReq.InputType == InputType.EnterKey);
				}
				return false;
			}
		}

        internal bool IsWaitAnyKey
        {
            get
			{
				GlobalStatic.ForceQuitAndRestart = false;
				return (state == ConsoleState.WaitInput && inputReq.InputType == InputType.AnyKey);
            }
        }
		#endregion
		internal bool IsWaintingOnePhrase
        {
            get
            {
				return (state == ConsoleState.WaitInput && inputReq.OneInput);
            }
        }

		internal bool IsRunningTimer
		{
			get
			{
				return (state == ConsoleState.WaitInput && inputReq.Timelimit > 0 && !isTimeout);
			}
		}

		internal bool IsWaitingPrimitive
		{
			get
			{
				if (state == ConsoleState.WaitInput)
					return (inputReq.InputType == InputType.PrimitiveMouseKey);
				return false;
			}
		}
		
		internal string SelectedString
		{
			get
			{
				if (selectingButton == null)
					return null;
				if (state == ConsoleState.Error)
					return selectingButton.Inputs;
				if (state != ConsoleState.WaitInput)
					return null;
				if (inputReq.InputType == InputType.IntValue && (selectingButton.IsInteger))
					return selectingButton.Input.ToString();
				if (inputReq.InputType == InputType.StrValue)
					return selectingButton.Inputs;
                #region EE_INPUTANY
                if (inputReq.InputType == InputType.AnyValue && (selectingButton.IsInteger))
					return selectingButton.Input.ToString();
				if (inputReq.InputType == InputType.AnyValue)
					return selectingButton.Inputs;
                #endregion
                return null;
			}
		}

		public void Initialize()
		{
			GlobalStatic.Console = this;
			GlobalStatic.MainWindow = window;
            emuera = new GameProc.Process(this);
			GlobalStatic.Process = emuera;

			//if (Program.DebugMode && Config.DebugShowWindow)
			//{
			//	OpenDebugDialog();
			//	window.Focus();
			//}
			ClearDisplay();
			if (!emuera.Initialize())
			{
				state = ConsoleState.Error;
				OutputLog(null);
				PrintFlush(false);
				RefreshStrings(true);
				return;
			}
			callEmueraProgram("");
			RefreshStrings(true);

			RefreshQuickButton();
		}
		

        public void Quit() { state = ConsoleState.Quit; }
		#region EE_FORCE_QUIT系
		public void ForceQuit()
		{

			if (GlobalStatic.ForceQuitAndRestart == true)
			{
				DialogResult result = MessageBox.Show(trmb.ForceQuitAndRestart.Text,
					"FORCE_QUIT_AND_RESTART",
					MessageBoxButtons.YesNo
					);
				if (result == DialogResult.Yes)
				{
					Program.Reboot = false;
					throw new CodeEE(trerror.ForceQuitAndRestartError.Text);
				}
			}
			GlobalStatic.ForceQuitAndRestart = true;
			window.Close();
			return;
		}
		#endregion

		public void ThrowTitleError(bool error)
		{
			state = ConsoleState.Error;
			notToTitle = true;
			byError = error;
		}
		public void ThrowError(bool playSound)
		{
			//if (playSound)
			//	System.Media.SystemSounds.Hand.Play();
			forceUpdateGeneration();
			UseUserStyle = false;
			PrintFlush(false);
			RefreshStrings(false);
			state = ConsoleState.Error;
		}

        public bool notToTitle = false;
        public bool byError = false;
        //public ScriptPosition ErrPos = null;

		#region button関連
		bool lastButtonIsInput = true;
        public bool updatedGeneration = false;
		int lastButtonGeneration = 0;//最後に追加された選択肢の世代。これと世代が一致しない選択肢は選択できない。
		int newButtonGeneration = 0;//次に追加される選択肢の世代。Input又はInputsごとに増加
		//public int LastButtonGeneration { get { return lastButtonGeneration; } }
		public int NewButtonGeneration { get { return newButtonGeneration; } }
        public void UpdateGeneration() { lastButtonGeneration = newButtonGeneration; updatedGeneration = true; }
        public void forceUpdateGeneration() { newButtonGeneration++; lastButtonGeneration = newButtonGeneration; updatedGeneration = true; }
        LogicalLine lastInputLine;

		private void newGeneration()
		{
            //値の入力を求められない時は更新は必要ないはず
			if (state != ConsoleState.WaitInput || !inputReq.NeedValue)
				return;
            if (!updatedGeneration && emuera.getCurrentLine != lastInputLine)
            {
                //ボタン無しで次の入力に来たなら強制で世代更新
                lastButtonGeneration = newButtonGeneration;
            }
            else
                updatedGeneration = false;
            lastInputLine = emuera.getCurrentLine;
			//古い選択肢を選択できないように。INPUTで使った選択肢をINPUTSには流用できないように。
			if (inputReq.InputType == InputType.IntValue)
			{
				if (lastButtonGeneration == newButtonGeneration)
					unchecked { newButtonGeneration++; }
				else if (!lastButtonIsInput)
					lastButtonGeneration = newButtonGeneration;
				lastButtonIsInput = true;
			}
            #region EE_INPUTANY
            if (inputReq.InputType == InputType.StrValue || inputReq.InputType == InputType.AnyValue)
			#endregion
			{
				if (lastButtonGeneration == newButtonGeneration)
					unchecked { newButtonGeneration++; }
				else if (lastButtonIsInput)
					lastButtonGeneration = newButtonGeneration;
				lastButtonIsInput = false;
			}
		}

		/// <summary>
		/// 選択中のボタン。INPUTやINPUTSに対応したものでなければならない
		/// </summary>
		ConsoleButtonString selectingButton = null;
		ConsoleButtonString lastSelectingButton = null;
		public ConsoleButtonString SelectingButton { get { return selectingButton; } }
		public bool ButtonIsSelected(ConsoleButtonString button) { return selectingButton == button; }

		/// <summary>
		/// ToolTip表示したフラグ
		/// </summary>
		bool tooltipUsed = false;
		/// <summary>
		/// マウスの直下にあるテキスト。ボタンであってもよい。
		/// ToolTip表示用。世代無視、履歴中も表示
		/// </summary>
		ConsoleButtonString pointingString = null;
		ConsoleButtonString lastPointingString = null;
		#endregion

		#region Input & Timer系

		//bool hasDefValue = false;
		//Int64 defNum;
		//string defStr;

		private InputRequest inputReq = null;
		public void Await(int time)
		{
			if (!Enabled || state != ConsoleState.Running)
			{
				this.Quit();
				return;
			}
			RefreshStrings(true);
			state = ConsoleState.Sleep;
			emuera.UpdateCheckInfiniteLoopState();
			App.DoEvents();
			if (time > 0)
				System.Threading.Thread.Sleep(time);
			////DoEvents()の間にウインドウが閉じられたらおしまい。
			//if (!Enabled || state != ConsoleState.Sleep)
			//{
			//	ReadAnyKey();
			//	return;
			//}

			state = ConsoleState.Running;
		}

		public void WaitInput(InputRequest req)
		{
			state = ConsoleState.WaitInput;
			inputReq = req;
			if (req.Timelimit > 0)
			{
				if (req.OneInput)
					window.update_lastinput();
				presetTimer();
//				setTimer();
			}
			//updateMousePosition();
			//Point point = window.MainPicBox.PointToClient(Control.MousePosition);
			//if (window.MainPicBox.ClientRectangle.Contains(point))
			//{
			//	PrintFlush(false);
			//	MoveMouse(point);
			//}
		}

		public void ReadAnyKey(bool anykey = false, bool stopMesskip = false)
		{
			InputRequest req = new InputRequest();
			if (!anykey)
				req.InputType = InputType.EnterKey;
			else
				req.InputType = InputType.AnyKey;
			req.StopMesskip = stopMesskip;
			inputReq = req;
			state = ConsoleState.WaitInput;
			emuera.NeedWaitToEventComEnd = false;
		}


		/// <summary>
		/// INPUT中のアニメーション用タイマー
		/// </summary>
		Timer redrawTimer = null;

		private void tickRedrawTimer(object sender, EventArgs e)
		{
			if (!redrawTimer.Enabled)
				return;
			//INPUT待ちでないとき、又はタイマー付きINPUT状態の場合はこれ以外の処理に任せる
			if (state != ConsoleState.WaitInput || timer.Enabled)
			{
				return;
			}
			window.Refresh();//OnPaint発行
		}

		/// <summary>
		/// アニメーション用タイマーの設定。0以下の値を指定するとタイマー停止
		/// </summary>
		public void setRedrawTimer(int tickcount)
		{
			if (tickcount <= 0)
			{
				redrawTimer.Enabled = false;
				return;
			}
			if (tickcount < 10)
				tickcount = 10;
			redrawTimer.Interval = tickcount;
			redrawTimer.Enabled = true;
		}



		Timer timer = null;
		Int64 timerID = -1;
		Int64 timer_startTime;//現在のタイマーを開始した時のミリ秒数（WinmmTimer.TickCount基準）
		Int64 timer_nextDisplayTime;//TINPUT系で次に残り時間を表示する時のTickCountミリ秒数
		Int64 timer_endTime;//現在のタイマーを終了する時のTickCountミリ秒数
        bool wait_timeout = false;
        bool isTimeout = false;
        public bool IsTimeOut { get { return isTimeout; } }

		/// <summary>
		/// 1824 TINPUT時に直接タイマーをセットせずに最初の再描画が終わってからタイマーをセットする（そうしないとTINPUTと再描画だけでループしてしまうので）
		/// </summary>
		bool need_settimer = false;

		private void presetTimer()
		{
			need_settimer = true;
			if (inputReq.DisplayTime)
			{
				//100ms未満の場合、一瞬だけ残り0が表示されて終了
				//timer_nextDisplayTime = timer_startTime + 100;
				long start = inputReq.Timelimit / 100;
				string timeString1 = trsl.Remaining.Text;
				string timeString2 = ((double)start / 10.0).ToString();
				PrintSingleLine(timeString1 + timeString2);
			}
		}
		private void setTimer()
		{
			isTimeout = false;
			timerID = inputReq.ID;
			timer.Enabled = true;
			timer_startTime = WinmmTimer.TickCount;
			timer_endTime = timer_startTime + inputReq.Timelimit;
			//if (inputReq.DisplayTime)
			//次に残り時間を表示するタイミングの設定。inputReq.DisplayTime==tureでないなら設定するだけで参照はされない（はず
			timer_nextDisplayTime = timer_startTime + 100;

		}

		//汎用
		private void tickTimer(object sender, EventArgs e)
		{
			if (!timer.Enabled)
				return;
			if (state != ConsoleState.WaitInput || inputReq.Timelimit <= 0 || timerID != inputReq.ID)
			{
#if DEBUG
				throw new ExeEE("");
#else
				stopTimer();
				return;
#endif
			}
			long curtime = WinmmTimer.TickCount;
			if (curtime >= timer_endTime)
			{
				endTimer();
				return;
			}
			if (inputReq.DisplayTime && curtime >= timer_nextDisplayTime)
			{
				//表示に時間がかかってタイマーが止まるので次の描画は100ms後。場合によっては表示が0.2一気に飛ぶ。
				timer_nextDisplayTime = curtime + 100;
				long time = (timer_endTime - curtime) / 100;
				string timeString1 = trsl.Remaining.Text;
				string timeString2 = ((double)time / 10.0).ToString();
				changeLastLine(timeString1 + timeString2);
			}
		}

		private void stopTimer()
		{
			//if (state == ConsoleState.WaitKeyWithTimerF && countTime < timeLimit)
			//{
			//	wait_timeout = true;
			//	while (countTime < timeLimit)
			//	{
			//		Application.DoEvents();
			//	}
			//	wait_timeout = false;
			//}
			timer.Enabled = false;
            //timer.Dispose();
		}

		/// <summary>
		/// tickTimerからのみ呼ぶ
		/// </summary>
		private void endTimer()
		{
            if (wait_timeout)
                return;
			stopTimer();
            isTimeout = true;

			ClearQuickButton();

			if(IsWaitingPrimitive)
			{
				//callEmueraProgramは呼び出し先で行う。
				#region EE_INPUTMOUSEKEY拡張
				// InputMouseKey(4, 0, 0, 0, 0);
				InputMouseKey(4, 0, 0, 0, 0, 0);
				//if (state == ConsoleState.WaitInput && inputReq.NeedValue)
				//{
				//	Point point = window.MainPicBox.PointToClient(Control.MousePosition);
				//	if (window.MainPicBox.ClientRectangle.Contains(point))
				//		MoveMouse(point);
				//}
				//RefreshStrings(true);
				#endregion
				return;
			}
			if (inputReq.DisplayTime)
				changeLastLine(inputReq.TimeUpMes);
			else if (inputReq.TimeUpMes != null)
				PrintSingleLine(inputReq.TimeUpMes);
			callEmueraProgram("");//ディフォルト入力の処理はcallEmueraProgram側で
			//if (state == ConsoleState.WaitInput && inputReq.NeedValue)
			//{
			//	Point point = window.MainPicBox.PointToClient(Control.MousePosition);
			//	if (window.MainPicBox.ClientRectangle.Contains(point))
			//		MoveMouse(point);
			//}
			RefreshStrings(true);

			RefreshQuickButton();
		}

        public void forceStopTimer()
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
            }
        }
		#endregion

		#region Call系
		/// <summary>
		/// スクリプト実行。RefreshStringsはしないので呼び出し側がすること
		/// </summary>
		/// <param name="str"></param>
		private void callEmueraProgram(string str)
		{
			//入力文字列の表示処理を行わない場合はstr == null
			if (str != null)
			{
				//INPUT文字列をPRINTする処理など
				if (!doInputToEmueraProgram(str))
					return;
				if (state == ConsoleState.Error)
					return;
			}
			state = ConsoleState.Running;
			emuera.DoScript();
			if (state == ConsoleState.Running)
			{//RunningならProcessは処理を継続するべき
				state = ConsoleState.Error;
                PrintError(trerror.ProgramStatusError.Text);
			}
			if (state == ConsoleState.Error && !noOutputLog)
				#region EE_OUTPUTLOG
				//OutputLog(Program.ExeDir + "emuera.log");
				OutputSystemLog(Program.ExeDir + "emuera.log");
				#endregion

			PrintFlush(false);
			//1819 Refreshは呼び出し側で行う
			//RefreshStrings(false);
			newGeneration();
		}

		private bool doInputToEmueraProgram(string str)
		{
			if (state == ConsoleState.WaitInput)
			{
				Int64 inputValue;

				switch (inputReq.InputType)
				{
					case InputType.IntValue:
						if (string.IsNullOrEmpty(str) && inputReq.HasDefValue && !IsRunningTimer)
						{
							inputValue = inputReq.DefIntValue;
							str = inputValue.ToString();
						}
						else if (!Int64.TryParse(str, out inputValue))
							return false;
						if (inputReq.IsSystemInput)
							emuera.InputSystemInteger(inputValue);
						else
							emuera.InputInteger(inputValue);
						break;
					case InputType.StrValue:
						if (string.IsNullOrEmpty(str) && inputReq.HasDefValue && !IsRunningTimer)
							str = inputReq.DefStrValue;
						//空入力と時間切れ
						if (str == null)
							str = "";
						emuera.InputString(str);
						break;
                    #region EE_INPUTANY
                    case InputType.AnyValue:
						if (Int64.TryParse(str, out inputValue))
                        {
							if (inputReq.IsSystemInput)
								emuera.InputSystemInteger(inputValue);
							else
								emuera.InputInteger(inputValue);
						}
						else
                        {
							emuera.InputString(str);
						}
						break;
                    #endregion

                }
                stopTimer();
			}
			Print(str);
			PrintFlush(false);
			return true;
		}
		#endregion

		#region 入力系
		readonly string[] spliter = new string[] { "\\n", "\r\n", "\n", "\r" };//本物の改行コードが来ることは無いはずだけど一応

		public bool MesSkip = false;
		private bool inProcess = false;
		volatile public bool KillMacro = false;
		
		internal void MouseWheel(Point point, int delta)
		{
			if (!IsWaitingPrimitive)
				return;
			//pointはクライアント左上基準の座標。
			//clientPointをクライアント左下基準の座標に置き換え
			Point clientPoint = point;
			clientPoint.Y = point.Y - ClientHeight;
			#region EE_INPUTMOUSEKEY拡張
			// InputMouseKey(2, delta, clientPoint.X, clientPoint.Y, 0);
			InputMouseKey(2, delta, clientPoint.X, clientPoint.Y, 0, 0);
			#endregion
		}

		internal void MouseDown(Point point, SKMouseButton button)
		{
			if (!IsWaitingPrimitive)
				return;
			//pointはクライアント左上基準の座標。
			//clientPointをクライアント左下基準の座標に置き換え
			Point clientPoint = point;
			clientPoint.Y = point.Y - ClientHeight;
			int buttonNum = -1;
			if(cbgButtonMap != null && cbgButtonMap.IsCreated)
			{
				//マップ画像の左上基準の座標に置き換え
				Point mapPoint = clientPoint;
				mapPoint.Y = clientPoint.Y + cbgButtonMap.Height;
				if(mapPoint.X >= 0 && mapPoint.Y >= 0 && mapPoint.X < cbgButtonMap.Width && mapPoint.Y < cbgButtonMap.Height)
				{
					Color c = DisplayUtils.ToColor(cbgButtonMap.Bitmap.GetPixel(mapPoint.X, mapPoint.Y));
					if(c.A == 255)
					{
						buttonNum = c.ToArgb() & 0xFFFFFF;
					}
				}

			}
			#region EE_INPUTMOUSEKEY拡張
			// InputMouseKey(1, (int)button, clientPoint.X, clientPoint.Y, buttonNum);
			//ボタン押された場合にRESULT:5にボタンの値が代入される
			if (selectingButton != null)
			{
				if (!selectingButton.IsInteger)
                {
					GlobalStatic.VEvaluator.RESULTS = selectingButton.Inputs;
					InputMouseKey(1, (int)button, clientPoint.X, clientPoint.Y, buttonNum, 0);
				}
                else
                {
					InputMouseKey(1, (int)button, clientPoint.X, clientPoint.Y, buttonNum, selectingButton.Input);
				}
			}
			else
			{
				InputMouseKey(1, (int)button, clientPoint.X, clientPoint.Y, buttonNum, 0);
			}
			#endregion
		}

		//1823 Key入力を捕まえる
		// internal void PressPrimitiveKey(Keys keycode, Keys keydata, Keys keymod)
		// {
		// 	if (IsWaitingPrimitive)
		// 		#region EE_INPUTMOUSEKEY拡張
		// 		// InputMouseKey(3, (int)keycode, (int)keydata, 0, 0);
		// 		InputMouseKey(3, (int)keycode, (int)keydata, 0, 0, 0);
		// 		#endregion
		// }

		//1823 Key入力を捕まえる
		#region EE_INPUTMOUSEKEY拡張
		//internal void InputMouseKey(int type, int result1, int result2, int result3, int result4)
		internal void InputMouseKey(int type, int result1, int result2, int result3, int result4, long result5)
		{
			// emuera.InputResult5(type, result1, result2, result3, result4);
			emuera.InputResult5(type, result1, result2, result3, result4, result5);
			inProcess = true;
			try
			{
				//1823 Escキーもマクロも右クリックも不可。単純に押されたキーを送るのみ。
				callEmueraProgram(null);
				//if (state == ConsoleState.WaitInput && inputReq.NeedValue)
				//{
				//	Point point = window.MainPicBox.PointToClient(Control.MousePosition);
				//	if (window.MainPicBox.ClientRectangle.Contains(point))
				//		MoveMouse(point);
				//}
			}
			finally
			{
				inProcess = false;
			}
			RefreshStrings(true);
		}
		#endregion

		public void PressEnterKey(bool keySkip, string str, bool changedByMouse)
		{
			MesSkip = keySkip;
			ClearQuickButton();

			if ((state == ConsoleState.Running) || (state == ConsoleState.Initializing))
				return;
			else if ((state == ConsoleState.Quit))
			{
				window.Close();
				return;
			}
			else if (state == ConsoleState.Error)
			{
				if (str == ErrorButtonsText && selectingButton != null && selectingButton.ErrPos != null)
				{
					openErrorFile(selectingButton.ErrPos);
					return;
				}
				window.Close();
				return;
			}
#if DEBUG
			if (state != ConsoleState.WaitInput || inputReq == null)
				throw new ExeEE("");
#endif
			KillMacro = false;
			try
			{
				string[] text;
				if(changedByMouse)//1823 マウスによって入力されたならマクロ解析を行わない
				{ text = new string[] { str }; }
				else
				{
					if (str.StartsWith("@") && !inputReq.OneInput)
					{
						doSystemCommand(str);
						return;
					}
					if (inputReq.InputType == InputType.Void)
						return;
					if (timer.Enabled &&
						(inputReq.InputType == InputType.AnyKey || inputReq.InputType == InputType.EnterKey))
						stopTimer();
					//if((inputReq.InputType == InputType.IntValue || inputReq.InputType == InputType.StrValue)
					if (str.Contains("("))
						str = parseInput(new StringStream(str), false);
					text = str.Split(spliter, StringSplitOptions.None);
				}
				
				inProcess = true;
				for (int i = 0; i < text.Length; i++)
				{
					string inputs = text[i];
					if (inputs.IndexOf("\\e") >= 0)
					{
						inputs = inputs.Replace("\\e", "");//\eの除去
						MesSkip = true;
					}

					if (inputReq.OneInput && (!Config.AllowLongInputByMouse || !changedByMouse) && inputs.Length > 1)
						inputs = inputs.Remove(1);
					//1819 TODO:入力無効系（強制待ちTWAIT）でスキップとマクロを止めるかそのままか
					//現在はそのまま。強制待ち中はスキップの開始もできないのにスキップ中なら飛ばせる。
					if (inputReq.InputType == InputType.Void)
					{
						i--;
						inputs = "";
					}
					callEmueraProgram(inputs);
					RefreshStrings(false);
					while (MesSkip && state == ConsoleState.WaitInput)
					{
						//TODO:入力無効を通していいか？スキップ停止をマクロでは飛ばせていいのか？
						if (inputReq.NeedValue)
							break;
						if (inputReq.StopMesskip)
							break;
						callEmueraProgram("");
						RefreshStrings(false);
						//DoEventを呼ばないと描画処理すらまったく行われない
						App.DoEvents();
						//EscがマクロストップかつEscがスキップ開始だからEscでスキップを止められても即開始しちゃったりするからあんまり意味ないよね
						//if (KillMacro)
						//	goto endMacro;
					}
					MesSkip = false;
					if (state != ConsoleState.WaitInput)
						break;
					//マクロループ時は待ち処理が起こらないのでここでシステムキューを捌く
					App.DoEvents();
#if DEBUG
					if (state != ConsoleState.WaitInput || inputReq == null)
						throw new ExeEE("");
#endif
					if (KillMacro)
						goto endMacro;
				}
			}
			finally
			{
				inProcess = false;

				RefreshQuickButton();
			}
			endMacro:
			//if(state == ConsoleState.WaitInput && inputReq.NeedValue)
			//{
			//	Point point = window.MainPicBox.PointToClient(Control.MousePosition);
			//	if (window.MainPicBox.ClientRectangle.Contains(point))
			//		MoveMouse(point);
			//}
			RefreshStrings(true);
		}

		private void openErrorFile(ScriptPosition pos)
		{
			ProcessStartInfo pInfo = new ProcessStartInfo();
			pInfo.FileName = Config.TextEditor;
			string fname = pos.Filename.ToUpper();
			if (fname.EndsWith(".CSV"))
			{
				if (fname.Contains(Program.CsvDir.ToUpper()))
					fname = fname.Replace(Program.CsvDir.ToUpper(), "");
				fname = Program.CsvDir + fname;
			}
			else
			{
				//解析モードの場合は見ているファイルがERB\の下にあるとは限らないかつフルパスを持っているのでこの補正はしなくてよい
				if (!Program.AnalysisMode)
				{
					if (fname.Contains(Program.ErbDir.ToUpper()))
						fname = fname.Replace(Program.ErbDir.ToUpper(), "");
					fname = Program.ErbDir + fname;
				}
			}
			switch (Config.EditorType)
			{
				case TextEditorType.SAKURA:
					pInfo.Arguments = "-Y=" + pos.LineNo.ToString() + " \"" + fname + "\"";
					break;
				case TextEditorType.TERAPAD:
					pInfo.Arguments = "/jl=" + pos.LineNo.ToString() + " \"" + fname + "\"";
					break;
				case TextEditorType.EMEDITOR:
					pInfo.Arguments = "/l " + pos.LineNo.ToString() + " \"" + fname + "\"";
					break;
				case TextEditorType.USER_SETTING:
					if (Config.EditorArg != "" && Config.EditorArg != null)
						pInfo.Arguments = Config.EditorArg + pos.LineNo.ToString() + " \"" + fname + "\"";
					else
						pInfo.Arguments = fname;
					break;
			}
			try
			{
				System.Diagnostics.Process.Start(pInfo);
			}
			catch (System.ComponentModel.Win32Exception)
			{
				//System.Media.SystemSounds.Hand.Play();
				PrintError("エディタを開くことができませんでした");
				forceUpdateGeneration();
			}
			return;
		}

        string parseInput(StringStream st, bool isNest)
        {
            StringBuilder sb = new StringBuilder(20);
            StringBuilder num = new StringBuilder(20);
            bool hasRet = false;
            int res;
            while (!st.EOS && (!isNest || st.Current != ')'))
            {
                if (st.Current == '(')
                {
                    st.ShiftNext();
                    string tstr = parseInput(st, true);

                    if (!st.EOS)
                    {
                        st.ShiftNext();
                        if (st.Current == '*')
                        {
                            st.ShiftNext();
                            while (char.IsNumber(st.Current))
                            {
                                num.Append(st.Current);
                                st.ShiftNext();
                            }
                            if (num.ToString() != "" && num.ToString() != null)
                            {
                                int.TryParse(num.ToString(), out res);
                                for (int i = 0; i < res; i++)
                                    sb.Append(tstr);
                                num.Remove(0, num.Length);
                            }
                        }
                        else
                            sb.Append(tstr);
                        continue;
                    }
                    else
                    {
                        sb.Append(tstr);
                        break;
                    }
                }
                else if (st.Current == '\\')
                {
                    st.ShiftNext();
                    switch (st.Current)
                    {
                        case 'n':
                            if (!hasRet)
                                sb.Append('\n');
                            else
                                hasRet = false;
                            break;
                        case 'r':
                            sb.Append('\r');
                            break;
                        case 'e':
                            sb.Append("\\e\n");
                            hasRet = true;
                            break;
                        case '\n':
                            break;
                        default:
                            sb.Append(st.Current);
                            break;
                    }
                }
                else
                    sb.Append(st.Current);
                st.ShiftNext();
            }
            return sb.ToString();
        }


		bool runningERBfromMemory = false;
		/// <summary>
		/// 通常コンソールからのDebugコマンド、及びデバッグウインドウの変数ウォッチなど、
		/// *.ERBファイルが存在しないスクリプトを実行中
		/// 1750 IsDebugから改名
		/// </summary>
		public bool RunERBFromMemory { get { return runningERBfromMemory; } set { runningERBfromMemory = value; } }
		void doSystemCommand(string command)
		{
			if(timer.Enabled)
			{
				PrintError(trerror.CanNotInputTimerWait.Text);
				PrintError("");//タイマー表示処理に消されちゃうかもしれないので
				RefreshStrings(true);
				return;
			}
			if (IsInProcess)
			{
				PrintError(trerror.CanNotInputScriptRunning.Text);
				RefreshStrings(true);
				return;
			}
			StringComparison sc = Config.SCVariable;
			Print(command);
			PrintFlush(false);
			RefreshStrings(true);
			string com = command.Substring(1);
			if (com.Length == 0)
				return;
			if (com.Equals("REBOOT", sc))
			{
				window.Reboot();
				return;
			}
			else if (com.Equals("OUTPUT", sc) || com.Equals("OUTPUTLOG", sc))
			{
				#region EE_OUTPUTLOG
				// this.OutputLog(Program.ExeDir + "emuera.log");
				this.OutputSystemLog(Program.ExeDir + "emuera.log");
				#endregion

				return;
			}
			else if ((com.Equals("QUIT", sc)) || (com.Equals("EXIT", sc)))
			{
				window.Close();
				return;
			}
			else if (com.Equals("CONFIG", sc))
			{
				//window.ShowConfigDialog();
				return;
			}
			else if (com.Equals("DEBUG", sc))
			{
				if (!Program.DebugMode)
				{
					PrintError(trerror.CanNotUseDebugWindow.Text);
					RefreshStrings(true);
					return;
				}
				//OpenDebugDialog();
			}
			else
			{
				if (!Config.UseDebugCommand)
				{
					PrintError(trerror.CanNotUseDebugCommand.Text);
					RefreshStrings(true);
					return;
				}
				//処理をDebugMode系へ移動
				DebugCommand(com, Config.ChangeMasterNameIfDebug, false);
				PrintFlush(false);
			}
			RefreshStrings(true);
		}
		#endregion

		#region 描画系
		uint lastUpdate = 0;
		uint msPerFrame = 1000 / 60;//60FPS
		ConsoleRedraw redraw = ConsoleRedraw.Normal;
        public ConsoleRedraw Redraw { get { return redraw; } }
		public void SetRedraw(Int64 i)
		{
			if ((i & 1) == 0)
				redraw = ConsoleRedraw.None;
			else
				redraw = ConsoleRedraw.Normal;
			if ((i & 2) != 0)
				RefreshStrings(true);
		}

		string debugTitle = null;
		public void SetWindowTitle(string str)
		{
			//if (Program.DebugMode)
			//{
			//	debugTitle = str;
			//	window.Text = str + " (Debug Mode)";
			//}
			//else
			//	window.Text = str;
		}

        public void SetEmueraVersionInfo(string str)
        {
            //window.TextBox.Text = str;
        }
		public string GetWindowTitle()
		{
			if (Program.DebugMode && debugTitle != null)
				return debugTitle;
			return window.Title;
		}


		/// <summary>
		/// 1818以前のRefreshStringsからselectingButton部分を抽出
		/// ここでOnPaintを発行
		/// </summary>
		public void RefreshStrings(bool force_Paint)
		{
			bool isBackLog = window.ScrollBar.Value != window.ScrollBar.Maximum;
			//ログ表示はREDRAWの設定に関係なく行うようにする
			if ((redraw == ConsoleRedraw.None) && (!force_Paint) && (!isBackLog))
				return;
			//選択中ボタンの適性チェック
			if (selectingButton != null)
			{
				//履歴表示中は選択肢無効→画面外に出てしまったボタンも履歴から選択できるように
				//if (isBackLog)
				//	selectingButton = null;
				//数値か文字列の入力待ち状態でなければ無効
				if(state != ConsoleState.Error && state != ConsoleState.WaitInput)
					selectingButton = null;
				else if((state == ConsoleState.WaitInput) && !inputReq.NeedValue)
					selectingButton = null;
				//選択肢が最新でないなら無効
				else if (selectingButton.Generation != lastButtonGeneration)
					selectingButton = null;
			}
			if (!force_Paint)
			{//forceならば確実に再描画。
				//履歴表示中でなく、最終行を表示済みであり、選択中ボタンが変更されていないなら更新不要
				if ((!isBackLog) && (lastDrawnLineNo == lineNo) && (lastSelectingButton == selectingButton))
					return;
				//Environment.TickCountは分解能が悪すぎるのでwinmmのタイマーを呼んで来る
				uint sec = WinmmTimer.TickCount - lastUpdate;
				//まだ書き換えるタイミングでないなら次の更新を待ってみる
				//ただし、入力待ちなど、しばらく更新のタイミングがない場合には強制的に書き換えてみる
				if (sec < msPerFrame && (state == ConsoleState.Running || state == ConsoleState.Initializing))
					return;
			}
			if (forceTextBoxColor)
			{
				uint sec = WinmmTimer.TickCount - lastBgColorChange;
				//色変化が速くなりすぎないように一定時間以内の再呼び出しは強制待ちにする
				while (sec < 200)
				{
					App.DoEvents();
					sec = WinmmTimer.TickCount - lastBgColorChange;
				}
				//window.TextBox.BackColor = this.bgColor;
				MainThread.BeginInvokeOnMainThread(() =>
				{
					window.BackgroundColor = this.bgColor;
				});
				lastBgColorChange = WinmmTimer.TickCount;
			}
			verticalScrollBarUpdate();
			window.Refresh();//OnPaint発行

		}

		public System.Threading.SpinLock displayLineListSpinLock = new System.Threading.SpinLock();

		/// <summary>
		/// 1818以前のRefreshStringsの後半とm_RefreshStringsを融合
		/// 全面Clear法のみにしたのでさっぱりした。ダブルバッファリングはOnPaintが勝手にやるはず
		/// </summary>
		/// <param name="graph"></param>
		public void OnPaint(SKCanvas graph)
		{

			//デバッグ用。描画が超重い環境を想定1
			//System.Threading.Thread.Sleep(100);
			
			//描画中にEmueraが閉じられると廃棄されたPictureBoxにアクセスしてしまったりするので
			//OnPaintからgraphをもらった直後だから大丈夫だとは思うけど一応
			if (!this.Enabled)
				return;

			bool lockTaken = false;
			displayLineListSpinLock.Enter(ref lockTaken);

			//1824 アニメスプライト用・現在フレームの時間を決定
			WinmmTimer.FrameStart();
			lastUpdate = WinmmTimer.CurrentFrameTime;//WinmmTimer.TickCount;

			bool isBackLog = window.ScrollBar.Value != window.ScrollBar.Maximum;
			//int pointY = window.MainPicBox.Height - Config.LineHeight;
			int pointY = ClientHeight - DisplayUtils.HeightOffset;


			int bottomLineNo = (int)window.ScrollBar.Value - 1;
			if (displayLineList.Count - 1 < bottomLineNo)
				bottomLineNo = displayLineList.Count - 1;//1820 この処理不要な気がするけどエラー報告があったので入れとく
			int topLineNo = bottomLineNo - (pointY / Config.LineHeight + 1);
			if (topLineNo < 0)
				topLineNo = 0;
			pointY -= (bottomLineNo - topLineNo) * Config.LineHeight;

			//if (Config.TextDrawingMode == TextDrawingMode.WINAPI)
			//{
			//	GDI.GDIStart(graph, this.bgColor);
			//	GDI.FillRect(new Rectangle(0, 0, window.MainPicBox.Width, window.MainPicBox.Height));
			//	//for (int i = bottomLineNo; i >= topLineNo; i--)
			//	//{
			//	//	displayLineList[i].GDIDrawTo(pointY, isBackLog);
			//	//	pointY -= Config.LineHeight;
			//	//}
			//	//1820a12 上から下へ描画する方向へ変更
			//	for (int i = topLineNo; i <= bottomLineNo; i++)
			//	{
			//		displayLineList[i].GDIDrawTo(pointY, isBackLog);
			//		pointY += Config.LineHeight;
			//	}
			//	GDI.GDIEnd(graph);
			//}
			//else
			//{
				graph.Clear(DisplayUtils.ToSKColor(this.bgColor));
				//1823 cbg追加
				for (int j = 0; j < cbgList.Count; j++)
				{
					if (cbgList[j].zdepth == 0)
					{
						//1823以前の文字列描画
						for (int i = topLineNo; i <= bottomLineNo; i++)
						{
							displayLineList[i].DrawTo(graph, pointY, isBackLog, true, Config.TextDrawingMode);
							pointY += Config.LineHeight;
						}
						if (lockTaken)
							displayLineListSpinLock.Exit();
						continue;
					}
					ASprite img = cbgList[j].Img;
					if (cbgList[j].isButton && cbgList[j].buttonValue == selectingCBGButtonInt)
						img = cbgList[j].ImgB;
					if (img == null || !img.IsCreated)
						continue;
					//img.GraphicsDraw(graph, new Point(cbgList[j].x, cbgList[j].y + window.MainPicBox.Height - img.DestBaseSize.Height));
					img.GraphicsDraw(graph, new Point(cbgList[j].x, cbgList[j].y + ClientHeight - img.DestBaseSize.Height));
					//Bitmap bmp = img.Bitmap;
					//graph.DrawImage(bmp,
					//	new Rectangle(cbgList[j].x + img.DestBasePosition.X, window.MainPicBox.Height - img.SrcRectangle.Height + cbgList[j].y + img.DestBasePosition.Y, img.SrcRectangle.Width, img.SrcRectangle.Height),
					//	img.SrcRectangle, GraphicsUnit.Pixel);
				}

			//}
			//ToolTip描画
			//if (lastPointingString != pointingString || lastSelectingCBGButtonInt != selectingCBGButtonInt)
			//{
			//	if (tooltipUsed)
			//		window.ToolTip.RemoveAll();
			//	string title = null;
			//	if (pointingString != null)
			//		title = pointingString.Title;
			//	else if (selectingCBGButtonInt > 0)
			//	{
			//		foreach (var cbg in cbgList)
			//		{
			//			if (!cbg.isButton || cbg.buttonValue != selectingCBGButtonInt)
			//				continue;
			//			if (string.IsNullOrEmpty(cbg.tooltipString))
			//				continue;
			//			title = cbg.tooltipString;
			//			break;
			//		}
			//	}
			//	if (!string.IsNullOrEmpty(title))
			//	{
			//		if (tooltip_duration == 0)
			//			window.ToolTip.SetToolTip(window.MainPicBox, title);
			//		else
			//		{
			//			if (window.ToolTip.InitialDelay == 0)
			//			{
			//				Point mousePos = window.MainPicBox.PointToClient(MainWindow.MousePosition);
			//				window.ToolTip.Show(title, window.MainPicBox, new Point(mousePos.X, mousePos.Y + 18), tooltip_duration);
			//			}
			//			else
			//			{
			//				System.Threading.SynchronizationContext context = System.Threading.SynchronizationContext.Current;
			//				System.Threading.Tasks.Task.Run(async () =>
			//				{
			//					ConsoleButtonString savedPointingString = pointingString;
			//					await System.Threading.Tasks.Task.Delay(window.ToolTip.InitialDelay);
			//					context.Post((state) =>
			//					{
			//						MoveMouse(GetMousePosition());
			//						if (lastPointingString == savedPointingString)
			//						{
			//							Point mousePos = window.MainPicBox.PointToClient(MainWindow.MousePosition);
			//							window.ToolTip.Show(title, window.MainPicBox, new Point(mousePos.X, mousePos.Y + 18), tooltip_duration);
			//						}
			//					}, null);
			//				});
			//			}
			//		}
			//		tooltipUsed = true;
			//	}
			//	lastPointingString = pointingString;
			//	lastSelectingCBGButtonInt = selectingCBGButtonInt;
			//}
			if (isBackLog)
				lastDrawnLineNo = -1;
			else
				lastDrawnLineNo = lineNo;
			lastSelectingButton = selectingButton;
			/*デバッグ用。描画が超重い環境を想定2
			System.Threading.Thread.Sleep(50);
			*/
			forceTextBoxColor = false;
			if(need_settimer)
			{
				need_settimer = false;
				setTimer();
			}
		}

		public void SetToolTipColor(Color foreColor, Color backColor)
		{
			//window.ToolTip.ForeColor = foreColor;
			//window.ToolTip.BackColor = backColor;

		}
		public void SetToolTipDelay(int delay)
		{
			//window.ToolTip.InitialDelay = delay;
		}

        int tooltip_duration = 0;
        public void SetToolTipDuration(int duration)
        {
            //tooltip_duration = duration;
        }


        //private Graphics getGraphics()
        //{
        //	//消したいが怖いので残し
        //	if (!window.Created)
        //		throw new ExeEE("存在しないウィンドウにアクセスした");
        //	//if (Config.UseImageBuffer)
        //	//	return Graphics.FromImage(window.MainPicBox.Image);
        //	//else
        //		return window.MainPicBox.CreateGraphics();
        //}

        #endregion

        #region DebugMode系
		//DebugDialog dd = null;
		//public DebugDialog DebugDialog { get { return dd; } }
		//StringBuilder dConsoleLog = new StringBuilder("");
		//public string DebugConsoleLog { get { return dConsoleLog.ToString(); } }
		List<string> dTraceLogList = new List<string>();
#pragma warning disable CS0414 // フィールド 'EmueraConsole.dTraceLogChanged' が割り当てられていますが、値は使用されていません。
		bool dTraceLogChanged = true;
#pragma warning restore CS0414 // フィールド 'EmueraConsole.dTraceLogChanged' が割り当てられていますが、値は使用されていません。
		public string GetDebugTraceLog(bool force)
		{
			//if (!dTraceLogChanged && !force)
			//	return null;
			StringBuilder builder = new StringBuilder("");
			LogicalLine line = emuera.GetScaningLine();
			builder.AppendLine(trsl.Processing.Text);
			if ((line == null) || (line.Position == null))
			{
				builder.AppendLine(trsl.FileNone.Text);
				builder.AppendLine(trsl.LineFuncNone.Text);
				builder.AppendLine("");
			}
			else
			{
				builder.AppendLine(string.Format(trsl.FileName.Text, line.Position.Filename));
				builder.AppendLine(string.Format(trsl.LineFuncName.Text, line.Position.LineNo.ToString(), line.ParentLabelLine.LabelName));
				builder.AppendLine("");
			}
			builder.AppendLine(trsl.FuncCallStack.Text);
			for (int i = dTraceLogList.Count - 1; i >= 0; i--)
			{
				builder.AppendLine(dTraceLogList[i]);
			}
			return builder.ToString();
		}
		public void OpenDebugDialog()
		{
			// if (!Program.DebugMode)
			// 	return;
			// if (dd != null)
			// {
			// 	if (dd.Created)
			// 	{
			// 		dd.Focus();
			// 		return;
			// 	}
			// 	else
			// 	{
			// 		dd.Dispose();
			// 		dd = null;
			// 	}
			// }
			// dd = new DebugDialog();
			// dd.SetParent(this, emuera);
			// dd.Show();
		}

		public void DebugPrint(string str)
		{
			if (!Program.DebugMode)
				return;
			//dConsoleLog.Append(str);
		}

		public void DebugClear()
		{
			//dConsoleLog.Remove(0, dConsoleLog.Length);
		}

		public void DebugNewLine()
		{
			if (!Program.DebugMode)
				return;
			//dConsoleLog.Append(Environment.NewLine);
		}

		public void DebugAddTraceLog(string str)
		{
			//Emueraがデバッグモードで起動されていないなら無視
			//ERBファイル以外のもの(デバッグコマンド、変数ウォッチ)を実行中なら無視
			if (!Program.DebugMode || runningERBfromMemory)
				return;
			dTraceLogChanged = true;
			dTraceLogList.Add(str);
		}
		public void DebugRemoveTraceLog()
		{
			if (!Program.DebugMode || runningERBfromMemory)
				return;
			dTraceLogChanged = true;
			if(dTraceLogList.Count > 0)
				dTraceLogList.RemoveAt(dTraceLogList.Count-1);
		}
		public void DebugClearTraceLog()
		{
			if (!Program.DebugMode || runningERBfromMemory)
				return;
			dTraceLogChanged = true;
			dTraceLogList.Clear();
		}

		public void DebugCommand(string com, bool munchkin, bool outputDebugConsole)
		{
			ConsoleState temp_state = state;
			runningERBfromMemory = true;
            //スクリプト等が失敗した場合に備えて念のための保存
            GlobalStatic.Process.saveCurrentState(false);
            try
			{
				//デバッグコマンドはReadEnabledLineを通してないのでRename変換を入れる
				if (Config.UseRenameFile && (com.IndexOf("[[") >= 0) && (com.IndexOf("]]") >= 0))
				{
					foreach (KeyValuePair<string, string> pair in ParserMediator.RenameDic)
						com = com.Replace(pair.Key, pair.Value);
				}
				LogicalLine line = null;
				if (!com.StartsWith("@") && !com.StartsWith("\"") && !com.StartsWith("\\"))
					line = LogicalLineParser.ParseLine(com, null);
				if (line == null || (line is InvalidLine))
				{
					WordCollection wc = LexicalAnalyzer.Analyse(new StringStream(com), LexEndWith.EoL, LexAnalyzeFlag.None);
					IOperandTerm term = ExpressionParser.ReduceExpressionTerm(wc, TermEndWith.EoL);
					if (term == null)
						throw new CodeEE(trerror.CanNotInterpretedLine.Text);
					if (term.GetOperandType() == typeof(Int64))
					{
						if (outputDebugConsole)
							com = "DEBUGPRINTFORML {" + com + "}";
						else
							com = "PRINTVL " + com;
					}
					else
					{
						if (outputDebugConsole)
							com = "DEBUGPRINTFORML %" + com + "%";
						else
							com = "PRINTFORMSL " + com;
					}
					line = LogicalLineParser.ParseLine(com, null);
				}
				if (line == null)
					throw new CodeEE(trerror.CanNotInterpretedLine.Text);
				if (line is InvalidLine)
					throw new CodeEE(line.ErrMes);
				if (!(line is InstructionLine))
					throw new CodeEE(trerror.InvalidDebugCommand.Text);
				InstructionLine func = (InstructionLine)line;
				if (func.Function.IsFlowContorol())
					throw new CodeEE(trerror.CanNotUseFlowInstruction.Text);
				//__METHOD_SAFE__をみるならいらないかも
				if (func.Function.IsWaitInput())
					throw new CodeEE(string.Format(trerror.CanNotUseInstruction.Text, func.Function.Name));
				//1750 __METHOD_SAFE__とほぼ条件同じだよねってことで
				if (!func.Function.IsMethodSafe())
					throw new CodeEE(string.Format(trerror.CanNotUseInstruction.Text, func.Function.Name));
				//1756 SIFの次に来てはいけないものはここでも不可。
				if (func.Function.IsPartial())
					throw new CodeEE(string.Format(trerror.CanNotUseInstruction.Text, func.Function.Name));
				switch (func.FunctionCode)
				{//取りこぼし
					//逆にOUTPUTLOG、QUITはDebugCommandの前に捕まえる
					case FunctionCode.PUTFORM:
					case FunctionCode.UPCHECK:
					case FunctionCode.CUPCHECK:
					case FunctionCode.SAVEDATA:
						throw new CodeEE(string.Format(trerror.CanNotUseInstruction.Text, func.Function.Name));
				}
				ArgumentParser.SetArgumentTo(func);
				if (func.IsError)
					throw new CodeEE(func.ErrMes);
				emuera.DoDebugNormalFunction(func, munchkin);
				if (func.FunctionCode == FunctionCode.SET)
				{
					if (!outputDebugConsole)
						PrintSingleLine(com);
					//DebugWindowのほうは少しくどくなるのでいらないかな
				}
			}
			catch (Exception e)
			{
				if (outputDebugConsole)
				{
					DebugPrint(e.Message);
					DebugNewLine();
				}
				else
					PrintError(e.Message);
				emuera.clearMethodStack();
			}
			finally
			{
                //確実に元の状態に戻す
                GlobalStatic.Process.loadPrevState();
                runningERBfromMemory = false;
				state = temp_state;
			}
		}
		#endregion

		#region Window.Form系

		internal Point GetMousePosition()
		{
			//if (window == null || !window.Created)
			//	return new Point();
			////クライアント左上基準の座標取得
			//Point pos = window.MainPicBox.PointToClient(Cursor.Position);
			////クライアント左下基準の座標に置き換え
			//pos.Y -= ClientHeight;
			//return pos;
			return new Point();
		}

		/// <summary>
		/// マウス位置をボタンの選択状態に反映させる
		/// </summary>
		/// <param name="point"></param>
		/// <returns>この後でRefreshStringsが必要かどうか</returns>
		public bool MoveMouse(Point point)
		{

			if (cbgButtonMap != null && cbgButtonMap.IsCreated)
			{
				//pointはクライアント左上基準の座標。
				//clientPointをクライアント左下基準の座標に置き換え
				Point clientPoint = point;
				clientPoint.Y = point.Y - ClientHeight;
				int buttonNum = -1;
				//マップ画像の左上基準の座標に置き換え
				Point mapPoint = clientPoint;
				mapPoint.Y = mapPoint.Y + cbgButtonMap.Height;
				if (mapPoint.X >= 0 && mapPoint.Y >= 0 && mapPoint.X < cbgButtonMap.Width && mapPoint.Y < cbgButtonMap.Height)
				{
					Color c = DisplayUtils.ToColor(cbgButtonMap.Bitmap.GetPixel(mapPoint.X, mapPoint.Y));
					if (c.A == 255)
					{
						buttonNum = c.ToArgb() & 0xFFFFFF;
					}
				}
				if (buttonNum >= 0)
				{
					bool ret = (pointingString != null || selectingButton != null || buttonNum != selectingCBGButtonInt);
					selectingCBGButtonInt = buttonNum;
					pointingString = null;
					selectingButton = null;
					return ret;
				}
				else if (selectingCBGButtonInt >= 0)
				{
					selectingCBGButtonInt = -1;
					pointingString = null;
					selectingButton = null;
					return true;
				}
			}
			selectingCBGButtonInt = -1;
			ConsoleButtonString select = null;
			ConsoleButtonString pointing = null;
			bool canSelect = false;
			//数値か文字列の入力待ち状態でなければ選択中にはならない
			if (state == ConsoleState.Error)
				canSelect = true;
			else if (state == ConsoleState.WaitInput && inputReq.NeedValue)
				canSelect = true;
			//スクリプト実行中は無視//入力・マクロ処理中は無視
			if(this.IsInProcess)
				goto end;
			//履歴表示中は無視
			//if (window.ScrollBar.Value != window.ScrollBar.Maximum)
			//	goto end;
			int pointX = point.X;
			int pointY = point.Y;
			ConsoleDisplayLine curLine;

			int bottomLineNo = (int)window.ScrollBar.Value - 1;
			if (displayLineList.Count - 1 < bottomLineNo)
				bottomLineNo = displayLineList.Count - 1;//1820 この処理不要な気がするけどエラー報告があったので入れとく
			//int topLineNo = bottomLineNo - (window.MainPicBox.Height/ Config.LineHeight);
			int topLineNo = bottomLineNo - (ClientHeight/ Config.LineHeight);
			if (topLineNo < 0)
				topLineNo = 0;

			//int relPointY = pointY - GameUtils.GetRealLength(window.MainPicBox.Height);
			int relPointY = pointY - (ClientHeight - DisplayUtils.HeightOffset);

			//下から上へ探索し発見次第打ち切り
			for (int i = bottomLineNo; i >= topLineNo; i--)
			{
				relPointY += Config.LineHeight;
				curLine = displayLineList[i];
				
				for (int b = 0; b < curLine.Buttons.Length; b++)
				{
					ConsoleButtonString button = curLine.Buttons[curLine.Buttons.Length - b - 1];
					if(button == null || button.StrArray == null)
						continue;
					if ((button.PointX <= pointX) && (button.PointX + button.Width >= pointX))
					{
						//if (relPointY >= 0 && relPointY <= Config.FontSize)
						//{
						//	pointing = button;
						//	if(pointing.IsButton)
						//		goto breakfor;
						//}
						foreach(AConsoleDisplayPart part in button.StrArray)
						{
							if(part == null)
								continue;
							if ((part.PointX <= pointX) && (part.PointX + part.Width >= pointX)
								&& (relPointY >= part.Top) && (relPointY <= part.Bottom))
							{
								pointing = button;
								if (pointing.IsButton)
									goto breakfor;
							}
						}
					}
				}
			}


			//int posy_bottom2up = window.MainPicBox.Height - pointY;
			//int logNum = window.ScrollBar.Maximum - window.ScrollBar.Value;
			////表示中の一番下の行番号
			//int curBottomLineNo = displayLineList.Count - logNum;
			//int curPointingLineNo = curBottomLineNo - (posy_bottom2up / Config.LineHeight + 1);
			//if ((curPointingLineNo < 0) || (curPointingLineNo >= displayLineList.Count))
			//	curLine = null;
			//else
			//	curLine =  displayLineList[curPointingLineNo];
			//if (curLine == null)
			//	goto end;
			
			//pointing = curLine.GetPointingButton(pointX);
		breakfor:
			if ((pointing == null) || (pointing.Generation != lastButtonGeneration))
				canSelect = false;
			else if (!pointing.IsButton)
				canSelect = false;
			else if ((state == ConsoleState.WaitInput && inputReq.InputType == InputType.IntValue) && (!pointing.IsInteger))
				canSelect = false;
		end:
			if (canSelect)
				select = pointing;
			bool needRefresh = select != selectingButton || pointing != pointingString;
			pointingString = pointing;
			selectingButton = select;
			return needRefresh;
		}


		public void LeaveMouse()
		{
			bool needRefresh = selectingButton != null || pointingString != null;
			selectingButton = null;
			pointingString = null;
			if(needRefresh)
			{
				RefreshStrings(true);
			}
		}

		private void verticalScrollBarUpdate()
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				verticalScrollBarUpdateAsync();
			});
		}

		private void verticalScrollBarUpdateAsync()
		{
			int max = displayLineList.Count;
			if (max == 0)
				return;

			int move = max - (int)window.ScrollBar.Maximum;
			if (move == 0)
				return;

			if (move > 0)
			{
				window.ScrollBar.Maximum = max;
				window.ScrollBar.Value += move;
			}
			else
			{
				if (max > window.ScrollBar.Value)
					window.ScrollBar.Value = max;
				window.ScrollBar.Maximum = max;
			}
			//window.ScrollBar.IsEnabled = max > 0;
		}
		#endregion



		public void GotoTitle()
		{
			//if (state == ConsoleState.Error)
			//{
			//    MessageBox.Show("エラー発生時はこの機能は使えません");
			//}
            forceStopTimer();
			ClearDisplay();
			//動的作成の分だけは削除する
			AppContents.UnloadGraphicList();
            redraw = ConsoleRedraw.Normal;
            UseUserStyle = false;
            userStyle = new StringStyle(Config.ForeColor, FontStyle.Regular, null);
            emuera.BeginTitle();
			ReadAnyKey(false, false);
			callEmueraProgram("");
			RefreshStrings(true);
		}

		bool force_temporary = false;
        bool timer_suspended = false;
		ConsoleState prevState;
		InputRequest prevReq;

		public void ReloadErb()
		{
			if (state == ConsoleState.Error)
			{
				MessageBox.Show(trerror.CanNotUseWhenError.Text);
				return;
			}
			if (state == ConsoleState.Initializing)
			{
				MessageBox.Show(trerror.CanNotUseWhenInitialize.Text);
				return;
			}
            bool notRedraw = false;
            if (redraw == ConsoleRedraw.None)
            {
                notRedraw = true;
                redraw = ConsoleRedraw.Normal;
            }
            if (timer.Enabled)
            {
				timer.Enabled = false;
                timer_suspended = true;
            }
            prevState = state;
			prevReq = inputReq;
			state = ConsoleState.Initializing;
			PrintSingleLine(trsl.ReloadingErb.Text, true);
			force_temporary = true;
			emuera.ReloadErb();
			force_temporary = false;
            PrintSingleLine(trsl.ReloadCompleted.Text, true);
			RefreshStrings(true);
            //強制的にボタン世代が切り替わるのを防ぐ
            updatedGeneration = true;
            if (notRedraw)
                redraw = ConsoleRedraw.None;
        }

		public void ReloadErbFinished()
		{
			state = prevState;
			inputReq = prevReq;
			PrintSingleLine(" ");
            if (timer_suspended)
            {
                timer_suspended = false;
                timer.Enabled = true;
				//タイマー待機中の時間ずれは修正しない。タイマー中にリロードしたらほぼ強制タイムアウトする程度は仕様のうちであろう。
            }
		}

		public void ReloadPartialErb(List<string> path)
		{
			if (state == ConsoleState.Error)
			{
				MessageBox.Show(trerror.CanNotUseWhenError.Text);
				return;
			}
			if (state == ConsoleState.Initializing)
			{
				MessageBox.Show(trerror.CanNotUseWhenInitialize.Text);
				return;
			}
            bool notRedraw = false;
            if (redraw == ConsoleRedraw.None)
            {
                notRedraw = true;
                redraw = ConsoleRedraw.Normal;
            }
            if (timer.Enabled)
            {
				timer.Enabled = false;
                timer_suspended = true;
            }
			prevState = state;
			prevReq = inputReq;
			state = ConsoleState.Initializing;
            PrintSingleLine(trsl.ReloadingErb.Text, true);
			force_temporary = true;
			emuera.ReloadPartialErb(path);
			force_temporary = false;
            PrintSingleLine(trsl.ReloadCompleted.Text, true);
			RefreshStrings(true);
            //強制的にボタン世代が切り替わるのを防ぐ
            updatedGeneration = true;
            if (notRedraw)
                redraw = ConsoleRedraw.None;
        }

		public void ReloadFolder(string erbPath)
		{
            if (state == ConsoleState.Error)
			{
				MessageBox.Show(trerror.CanNotUseWhenError.Text);
				return;
			}
			if (state == ConsoleState.Initializing)
			{
				MessageBox.Show(trerror.CanNotUseWhenInitialize.Text);
				return;
			}
            if (timer.Enabled)
            {
				timer.Enabled = false;
                timer_suspended = true;
            }
            List<string> paths = new List<string>();
			SearchOption op = SearchOption.AllDirectories;
			if (!Config.SearchSubdirectory)
				op = SearchOption.TopDirectoryOnly;
			//string[] fnames = Directory.GetFiles(erbPath, "*.ERB", op);
			string[] fnames = FileUtils.GetFiles(erbPath, "*.ERB", op);
			for (int i = 0; i < fnames.Length; i++)
				if (Path.GetExtension(fnames[i]).ToUpper() == ".ERB")
					paths.Add(fnames[i]);
            bool notRedraw = false;
            if (redraw == ConsoleRedraw.None)
            {
                notRedraw = true;
                redraw = ConsoleRedraw.Normal;
            }
			prevState = state;
			prevReq = inputReq;
			state = ConsoleState.Initializing;
            PrintSingleLine(trsl.ReloadingErb.Text, true);
			force_temporary = true;
            emuera.ReloadPartialErb(paths);
			force_temporary = false;
            PrintSingleLine(trsl.ReloadCompleted.Text, true);
			RefreshStrings(true);
            //強制的にボタン世代が切り替わるのを防ぐ
            updatedGeneration = true;
            if (notRedraw)
                redraw = ConsoleRedraw.None;
        }

		public void Dispose()
		{
			if(timer != null)
				timer.Dispose();
			//timer = null;
			//stringMeasure.Dispose();
		}


	}
}
