using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MinorShift._Library;
using XEmuera;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Forms;
using Xamarin.CommunityToolkit.Extensions;

namespace MinorShift.Emuera
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainWindow : ContentPage
	{
		public Color InvertForeColor { get; set; }

		bool IsInitGameView;

		bool IsWindowClosing;

		public MainWindow()
		{
			InitializeComponent();

			GameUtils.MainLayout = mainLayout;
			GameUtils.MainPicBox = mainPicBox;

			GlobalStatic.MainWindow = this;

			Sys.Init();
			Program.Main();

			InvertForeColor = DisplayUtils.InvertColor(Config.ForeColor).WithAlpha(0x60);

			InitGameView();
		}

		private void InitGameView()
		{
			ScreenOrientation = Width > Height;

			mainLayout.Children.Clear();

			mainLayout.Children.Add(mainPicBox,
				null,
				null,
				Constraint.RelativeToParent(parent =>
				{
					return Math.Max(parent.Width, ConfigData.Instance.GetConfigValue<int>(ConfigCode.WindowX) * Config.FontScale / DisplayUtils.ScreenDensity);
				}),
				Constraint.RelativeToParent(parent => parent.Height));

			mainLayout.Children.Add(uiLayout,
				null,
				null,
				Constraint.RelativeToParent(parent => parent.Width),
				Constraint.RelativeToParent(parent => parent.Height));
		}

		protected override void OnAppearing()
		{
			GameUtils.IsEmueraPage = true;
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			GameUtils.IsEmueraPage = false;
		}

		bool ScreenOrientation;

		private void uiLayout_SizeChanged(object sender, EventArgs e)
		{
			if (ScreenOrientation != Width > Height)
			{
				ScreenOrientation = Width > Height;
				RefreshQuickButtonGroup();
			}

			RefreshScrollBarLayout();
		}

		private void ContentPage_SizeChanged(object sender, EventArgs e)
		{
			if (IsWindowClosing)
				return;

			if (IsInitGameView)
			{
				UpdateView();
				return;
			}

			IsInitGameView = true;

			Config.RefreshDisplayConfig();

			InitEmuera();
		}

		private void UpdateView()
		{
			Config.RefreshDisplayConfig();
			console.setStBar(Config.DrawLineString);
			mainPicBox.TranslationX = 0;
		}

		public void RefreshQuickButtonGroup()
		{
			int width = DisplayUtils.QuickButtonWidth * Config.QuickButtonColumn + DisplayUtils.QuickButtonSpacing * (Config.QuickButtonColumn - 1);
			buttonScrollView.WidthRequest = Math.Min(uiLayout.Width * 3 / 5, Math.Min(width, quickButtonGroup.Width));
			buttonScrollView.HeightRequest = Math.Min(400d, Math.Min(uiLayout.Height - buttonGroup.Height - 40d, quickButtonGroup.Height));

			buttonScrollView.ScrollToAsync(0, quickButtonGroup.Height, false);
		}

		private void RefreshScrollBarLayout()
		{
			ScrollBarLayout.WidthRequest = Math.Min(400d, uiLayout.Height - buttonGroup.Height - 40d);
		}

		protected override bool OnBackButtonPressed()
		{
			if (console.IsError)
			{
				Close();
			}
			else if (!IsInitializeing())
			{
				MessageBox.ShowOnMainThread("确定要退出游戏吗?", "", result =>
				{
					if (result)
						Close();
				}, MessageBoxButtons.YesNo);
			}

			return true;
		}

		private bool IsInitializeing()
		{
			if (GlobalStatic.Process == null || GlobalStatic.Process.inInitializeing)
			{
				this.DisplaySnackBarAsync("请等待加载完成后再重启或退出游戏。", null, null);
				return true;
			}
			return false;
		}

		public void Close()
		{
			IsWindowClosing = true;
			MainThread.BeginInvokeOnMainThread(() =>
			{
				//if (Config.UseKeyMacro)
				//	KeyMacro.SaveMacro();
				if (console != null)
				{
					console.Quit();
					console.Dispose();
					//console = null;
				}
				Program.RebootMain();
				Navigation.PopAsync();
			});
		}

		bool IsMouseMoveAction;

		SKPoint PrevPoint;
		System.Drawing.Point MouseLocation;
		Point MoveDistance;

		double moveX;
		double moveY;

		double PicBoxMinX;
		double PicBoxMaxX;

		/// <summary>
		/// 获取画板的点击位置和滑动距离
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EraPictureBox_Touch(object sender, SKTouchEventArgs e)
		{
			MouseLocation = new System.Drawing.Point((int)e.Location.X, (int)e.Location.Y);

			if (console.MoveMouse(MouseLocation))
				RefreshStrings(true);

			switch (e.ActionType)
			{
				//case SKTouchAction.Entered:
				//	break;
				case SKTouchAction.Pressed:

					IsMouseMoveAction = false;
					IsDragScrollBar = true;
					PrevPoint = e.Location;
					//StartPoint = PrevPoint;

					PicBoxMinX = mainLayout.Width - mainPicBox.Width;
					PicBoxMaxX = 0;

					MoveDistance = Point.Zero;
					moveY = (double)Config.LineHeight / Config.ScrollHeight;

					e.Handled = true;
					return;

				case SKTouchAction.Moved:

					IsMouseMoveAction = true;

					//mainPicBox
					MoveDistance.X += e.Location.X - PrevPoint.X;

					if (Config.PanSpeed > 1f && mainPicBox.TranslationX > PicBoxMinX && mainPicBox.TranslationX < PicBoxMaxX)
						MoveDistance.X += MoveDistance.X * (Config.PanSpeed - 1f);

					moveX = MoveDistance.X / DisplayUtils.ScreenDensity;

					if (DisplayUtils.DirectionLimitX((int)moveX, mainPicBox.TranslationX, PicBoxMinX, PicBoxMaxX))
						mainPicBox.TranslationX = Math.Clamp(mainPicBox.TranslationX + moveX, PicBoxMinX, PicBoxMaxX);

					//vScrollBar
					MoveDistance.Y += e.Location.Y - PrevPoint.Y;
					int sign = (int)(MoveDistance.Y / moveY);
					if (sign != 0 && DisplayUtils.DirectionLimitY(sign, vScrollBar.Value, vScrollBar.Minimum, vScrollBar.Maximum))
					{
						vScrollBar.Value -= sign;
						MoveDistance.Y %= moveY;
					}

					PrevPoint = e.Location;
					e.Handled = true;
					return;

				case SKTouchAction.Released:

					IsDragScrollBar = false;

					MouseReleased(e);
					LeaveMouse();

					e.Handled = true;
					break;

				//case SKTouchAction.Cancelled:
				//	break;
				//case SKTouchAction.Exited:
				//	break;
				//case SKTouchAction.WheelChanged:
				//	break;
				default:
					return;
			}
		}

		private void MouseReleased(SKTouchEventArgs e)
		{
			if (IsMouseMoveAction)
				return;

			//if (!Config.UseMouse)
			//	return;
			//if (console == null || console.IsInProcess)
			//	return;
			if (console.IsInProcess)
				return;
			if (console.IsWaitingPrimitive)
			//			if (console.IsWaitingPrimitiveMouse)
			{
				console.MouseDown(MouseLocation, e.MouseButton);
				return;
			}

			bool isBacklog = vScrollBar.Value != vScrollBar.Maximum;
			string str = console.SelectedString;

			if (isBacklog)
				if (e.MouseButton == SKMouseButton.Left || e.MouseButton == SKMouseButton.Right)
				{
					vScrollBar.Value = vScrollBar.Maximum;
					RefreshStrings(true);
				}
			if (console.IsWaitingEnterKey && !console.IsError && str == null)
			{
				if (isBacklog)
					return;
				if (e.MouseButton == SKMouseButton.Left || e.MouseButton == SKMouseButton.Right)
				{
					if (e.MouseButton == SKMouseButton.Right)
						PressEnterKey(true, true);
					else
						PressEnterKey(false, true);
					return;
				}
			}
			//左が押されたなら選択。
			if (str != null && e.MouseButton == SKMouseButton.Left)
			{
				changeTextbyMouse = console.IsWaintingOnePhrase;
				richTextBox1.Text = str;
				//念のため
				if (console.IsWaintingOnePhrase)
					last_inputed = "";
				//右が押しっぱなしならスキップ追加。
				//if ((Control.MouseButtons & SKMouseButton.Right) == SKMouseButton.Right)
				//	PressEnterKey(true, true);
				//else
				PressEnterKey(false, true);
				return;
			}
		}

		public bool IsDragScrollBar { get; private set; }

		private void vScrollBar_DragStarted(object sender, EventArgs e)
		{
			IsDragScrollBar = true;
		}

		private void vScrollBar_DragCompleted(object sender, EventArgs e)
		{
			IsDragScrollBar = false;
		}

		/// <summary>
		/// 指定画板的绘制行数位置
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void scrollBar_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			if (!IsDragScrollBar)
				return;

			//if (console == null)
			//	return;

			int newValue = (int)e.NewValue;

			if (newValue == vScrollBar.Minimum || newValue == vScrollBar.Maximum || (int)e.OldValue != newValue)
				RefreshStrings((vScrollBar.Value == vScrollBar.Maximum) || (vScrollBar.Value == vScrollBar.Minimum));
		}

		private void LeaveMouse()
		{
			Task.Run(() =>
			{
				console.LeaveMouse();
			});
		}

		private void RefreshStrings(bool force_Paint)
		{
			Task.Run(() =>
			{
				console.RefreshStrings(force_Paint);
			});
		}

		public void Refresh()
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				mainPicBox.InvalidateSurface();
			});
		}

		/// <summary>
		/// 点击输入法上的确定按钮即可提交输入
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void richTextBox1_Completed(object sender, EventArgs e)
		{
			if (console.IsInProcess)
				return;

			PressEnterKey(false, false);
		}

		private void InvisibleAllControlView()
		{
			ScrollBarLayout.IsVisible = false;
			buttonScrollView.IsVisible = false;
		}

		/// <summary>
		/// 控制显示或隐藏滑动条
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ScrollBarButton_Clicked(object sender, EventArgs e)
		{
			bool visible = ScrollBarLayout.IsVisible;
			InvisibleAllControlView();
			ScrollBarLayout.IsVisible = !visible;
		}

		/// <summary>
		/// 控制显示或隐藏输入栏
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EntryButton_Clicked(object sender, EventArgs e)
		{
			entryGroup.IsVisible = !entryGroup.IsVisible;
		}

		private void inputButton_Clicked(object sender, EventArgs e)
		{
			if (console.IsInProcess)
				return;

			PressEnterKey(false, false);
		}

		private void SkipButton_Clicked(object sender, EventArgs e)
		{
			PressEnterKey(true, true);
		}

		public void QuickButtonGroup_Clicked(object sender, EventArgs e)
		{
			bool visible = buttonScrollView.IsVisible;
			InvisibleAllControlView();
			buttonScrollView.IsVisible = !visible;

			if (buttonScrollView.IsVisible)
				console.RefreshQuickButtonAsync();
		}

		public void quickButton_Clicked(object sender, EventArgs e)
		{
			if (!(((Button)sender).BindingContext is string inputs))
				return;

			richTextBox1.Text = inputs;
			PressEnterKey(false, false);
		}

		public void MainMenu_Reboot()
		{
			if (IsInitializeing())
				return;

			MessageBox.ShowOnMainThread("ゲームを再起動します", "再起動", result =>
			{
				if (result)
					this.Reboot();
			}, MessageBoxButtons.OKCancel);
		}

		public void MainMenu_GotoTitle()
		{
			if (console == null)
				return;
			if (console.IsInProcess)
			{
				MessageBox.Show("スクリプト動作中には使用できません");
				return;
			}
			if (console.notToTitle)
			{
				if (console.byError)
					MessageBox.Show("コード解析でエラーが発見されたため、タイトルへは飛べません");
				else
					MessageBox.Show("解析モードのためタイトルへは飛べません");
				return;
			}

			MessageBox.ShowOnMainThread("タイトル画面へ戻ります", "タイトル画面に戻る", result =>
			{
				if (result)
					this.GotoTitle();
			}, MessageBoxButtons.OKCancel);
		}
	}
}