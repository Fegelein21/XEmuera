//need check

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
using System.ComponentModel;
using XEmuera.Views;
using XEmuera.Resources;
using System.Timers;
using XEmuera.Models;

namespace MinorShift.Emuera
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainWindow : ContentPage, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler NotifyPropertyChanged;

		public Color MainColor
		{
			get => _mainColor;
			set
			{
				if (_mainColor == value)
					return;
				_mainColor = value;
				InvertMainColor = DisplayUtils.InvertColor(_mainColor).WithAlpha(0x80);
				NotifyPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MainColor)));
			}
		}
		private Color _mainColor;

		public Color InvertMainColor
		{
			get => _invertMainColor;
			set
			{
				if (_invertMainColor == value)
					return;
				_invertMainColor = value;
				NotifyPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InvertMainColor)));
			}
		}
		private Color _invertMainColor;

		bool IsInitGameView;

		bool IsWindowClosing;

		List<TintImageButton> InvisibleToolButtonList;

		private MainWindow()
		{
			InitializeComponent();

			GameUtils.MainLayout = mainLayout;
			GameUtils.MainPicBox = mainPicBox;

			GlobalStatic.MainWindow = this;

			Sys.Init();
			if (!Program.Init())
			{
				LoadSuccess = false;
				return;
			}

			this.SetBinding(BackgroundColorProperty, nameof(MainColor), BindingMode.TwoWay);

			MainColor = Config.ForeColor;

			ToolButtonGroup.BindingContext = this;
			entryGroup.BindingContext = this;

			InvisibleToolButtonList = new List<TintImageButton>
			{
				scroll_vertical_button,
				gallery_view_button,
			};

			LongPressTimer = new Timer(Config.LongPressSkipTime);
			LongPressTimer.Elapsed += LongPressTimer_Elapsed;

			InitGameView();
			InitEmuera();
		}

		private void LongPressTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			LongPressTimer.Enabled = false;

			if (IsMouseMove(PrevPoint))
				return;

			IsLongPressed = true;
			//PressEnterKey(null);
		}

		static bool LoadSuccess;

		public static MainWindow Load()
		{
			LoadSuccess = true;
			var mainWindow = new MainWindow();
			return LoadSuccess ? mainWindow : null;
		}

		private double ScaledWindowX;

		private void InitGameView()
		{
			int originalWindowX = ConfigData.Instance.GetConfigValue<int>(ConfigCode.WindowX);
			int originalFontSize = ConfigData.Instance.GetConfigValue<int>(ConfigCode.FontSize);
			ScaledWindowX = originalWindowX * Config.FontScale * Config.FontSize / originalFontSize / DisplayUtils.ScreenDensity;

			mainLayout.Children.Clear();

			mainLayout.Children.Add(mainPicBox,
				null,
				null,
				Constraint.RelativeToParent(parent => Math.Max(parent.Width, ScaledWindowX)),
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
			GameUtils.PlatformService.UnlockScreenOrientation();
		}

		private void uiLayout_SizeChanged(object sender, EventArgs e)
		{
			if (IsWindowClosing)
				return;

			RefreshQuickButtonGroup();
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

			//InitEmuera();
		}

		private void UpdateView()
		{
			Config.RefreshDisplayConfig();
			console.setStBar(Config.DrawLineString);
			mainPicBox.TranslationX = 0;
		}

		public void RefreshQuickButtonGroup()
		{
			quickButtonScrollView.WidthRequest = Math.Min(quickButtonGroup.Width, uiLayout.Width * 3 / 5);
			quickButtonScrollView.HeightRequest = Math.Min(quickButtonGroup.Height, uiLayout.Height - ToolButtonGroup.Height - 30d);

			quickButtonScrollView.ScrollToAsync(0, quickButtonGroup.Height, false);
		}

		private void RefreshScrollBarLayout()
		{
			ScrollBarLayout.WidthRequest = Math.Min(400d, uiLayout.Height - ToolButtonGroup.Height - 40d);
		}

		protected override bool OnBackButtonPressed()
		{
			if (console != null && console.IsError)
			{
				Close();
			}
			else if (!IsInitializing(true))
			{
				MessageBox.ShowOnMainThread(StringsText.QuitGameConfirm, null, result =>
				{
					if (result)
						Close();
				}, MessageBoxButtons.YesNo);
			}

			return true;
		}

		private bool IsInitializing(bool showMessage = false)
		{
			// Process is null mostly because of closing
			if (GlobalStatic.Process != null && GlobalStatic.Process.inInitializeing)
			{
				if (showMessage)
					this.DisplaySnackBarAsync(StringsText.GameIsProcessing, null, null);
				return true;
			}
			return false;
		}

		public void Close()
		{
			if (IsInitializing(false))
            {
				return;
            }
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
				if (!Program.Reboot)
					GC.Collect();
			});
		}

		bool IsMouseMoveAction;

		SKPoint StartPoint;
		SKPoint PrevPoint;
		System.Drawing.Point MouseLocation;
		Point MoveDistance;

		double moveX;
		double moveY;

		double PicBoxMinX;
		double PicBoxMaxX;

		Timer LongPressTimer;
		bool IsLongPressed;

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

					// when rebooting, clicking may cause unexpected closing
					if (IsInitializing(false) || GlobalStatic.Process == null)
                    {
						return;
                    }

					IsMouseMoveAction = false;
					IsDragScrollBar = true;
					PrevPoint = e.Location;
					StartPoint = PrevPoint;

					IsLongPressed = false;
					if (Config.LongPressSkip)
						LongPressTimer.Enabled = true;

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

					if (Config.LongPressSkip)
						LongPressTimer.Enabled = false;

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
			if (IsMouseMove(e.Location))
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
				if (IsLongPressed) console.MouseDown(MouseLocation, SKMouseButton.Right);
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
					if (e.MouseButton == SKMouseButton.Right || IsLongPressed)
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

		private bool IsMouseMove(SKPoint endPoint)
		{
			return IsMouseMoveAction && SKPoint.Distance(StartPoint, endPoint) >= 10;
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

		private void InvisibleToolButton(TintImageButton button)
		{
			if (InvisibleToolButtonList == null)
				return;
			if (!(button.IsToggled ?? false))
				return;

			foreach (var view in InvisibleToolButtonList)
			{
				if (view != button)
					view.IsToggled = false;
			}
		}

		private void lock_rotation_button_Toggled(object sender, ToggledEventArgs e)
		{
			if (e.Value)
				GameUtils.PlatformService.LockScreenOrientation();
			else
				GameUtils.PlatformService.UnlockScreenOrientation();

			SetButtonOpacity(lock_rotation_button, e.Value);
		}

		private void edit_button_Toggled(object sender, ToggledEventArgs e)
		{
			entryGroup.IsVisible = e.Value;
			SetButtonOpacity(edit_button, e.Value);
		}

		private void scroll_vertical_button_Toggled(object sender, ToggledEventArgs e)
		{
			InvisibleToolButton((TintImageButton)sender);

			ScrollBarLayout.IsVisible = e.Value;
			SetButtonOpacity(scroll_vertical_button, e.Value);
		}

		private void gallery_view_button_Toggled(object sender, ToggledEventArgs e)
		{
			InvisibleToolButton((TintImageButton)sender);

			quickButtonScrollView.IsVisible = e.Value;
			SetButtonOpacity(gallery_view_button, e.Value);

			if (quickButtonScrollView.IsVisible)
				console?.RefreshQuickButtonAsync();
			else
				console?.ClearQuickButtonAsync();
		}

		private void ButtonVisibleGroup_Clicked(object sender, EventArgs e)
		{
			var visible = !edit_button.IsVisible;

			lock_rotation_button.IsVisible = visible;
			edit_button.IsVisible = visible;
			scroll_vertical_button.IsVisible = visible;
			gallery_view_button.IsVisible = visible;

			SetButtonOpacity(menu_show_button, visible);
		}

		public void quickButton_Clicked(object sender, EventArgs e)
		{
			PressEnterKey(((View)sender).BindingContext as string);
		}

		private void PressEnterKey(string inputs)
		{
			bool isBacklog = vScrollBar.Value != vScrollBar.Maximum;
			if (isBacklog)
			{
				vScrollBar.Value = vScrollBar.Maximum;
				RefreshStrings(true);
			}

			if (inputs == null)
			{
				PressEnterKey(true, false);
			}
			else
			{
				richTextBox1.Text = inputs;
				PressEnterKey(false, false);
			}
		}

		private void publish_button_Clicked(object sender, EventArgs e)
		{
			if (console.IsInProcess)
				return;

			PressEnterKey(false, false);
		}

		private static void SetButtonOpacity(View button, bool visible)
		{
			button.Opacity = visible ? 1 : 0.5d;
		}

		public void MainMenu_Reboot()
		{
			if (IsInitializing(true))
				return;

			MessageBox.ShowOnMainThread(StringsText.RebootConfirm, StringsText.Reboot, result =>
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

			MessageBox.ShowOnMainThread(StringsText.GotoTitleConfirm, StringsText.GotoTitle, result =>
			{
				if (result)
					this.GotoTitle();
			}, MessageBoxButtons.OKCancel);
		}
	}
}