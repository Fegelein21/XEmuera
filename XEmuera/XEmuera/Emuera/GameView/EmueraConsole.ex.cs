using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MinorShift.Emuera.GameView
{
	internal sealed partial class EmueraConsole
	{
		StackLayout quickButtonGroup;

		public void ClearQuickButton()
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				ClearQuickButtonAsync();
			});
		}

		public void ClearQuickButtonAsync()
		{
			quickButtonGroup = window.quickButtonGroup;

			if (quickButtonGroup.Children.Count > 0)
			{
				quickButtonGroup.Children.Clear();
				quickButtonGroup.WidthRequest = -1;
				quickButtonGroup.HeightRequest = -1;

				window.quickButtonScrollView.WidthRequest = -1;
				window.quickButtonScrollView.HeightRequest = -1;
			}
		}

		public void RefreshQuickButton()
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				RefreshQuickButtonAsync();
			});
		}

		public void RefreshQuickButtonAsync()
		{
			if (!window.quickButtonGroup.IsVisible)
				return;

			if (state != ConsoleState.WaitInput)
				return;

			if (inputReq.InputType != GameProc.InputType.IntValue && inputReq.InputType != GameProc.InputType.StrValue)
				return;

			bool lockTaken = false;
			displayLineListSpinLock.Enter(ref lockTaken);

			int bottomLineNo = displayLineList.Count - 1;
			int topLineNo = Math.Max(bottomLineNo - (ClientHeight / Config.LineHeight) - 10, 0);

			int row;
			int column = 0;

			ConsoleDisplayLine curLine;
			ConsoleButtonString button;

			StackLayout layout;
			Button quickButton;

			System.Drawing.Color color;
			System.Drawing.Color bgcolor;
			AConsoleColoredPart coloredPart;

			quickButtonGroup = window.quickButtonGroup;
			quickButtonGroup.Spacing = Config.QuickButtonSpacing;

			for (int i = topLineNo; i <= bottomLineNo; i++)
			{
				curLine = displayLineList[i];
				row = 0;
				layout = new StackLayout
				{
					Orientation = StackOrientation.Horizontal,
					Spacing = Config.QuickButtonSpacing,
				};

				for (int b = 0; b < curLine.Buttons.Length; b++)
				{
					button = curLine.Buttons[b];
					if (button == null || !button.IsButton)
						continue;
					if (button.StrArray == null || button.StrArray.Length == 0)
						continue;
					if (button.Generation != lastButtonGeneration)
						continue;

					coloredPart = button.StrArray[button.StrArray.Length - 1] as AConsoleColoredPart;
					color = coloredPart?.Color ?? Config.ForeColor;

					bgcolor = Math.Abs(color.R - 0x7f) <= 0x10 && Math.Abs(color.G - 0x7f) <= 0x10 && Math.Abs(color.B - 0x7f) <= 0x10
						? Config.BackColor : color;

					quickButton = new Button
					{
						BindingContext = button.Inputs,
						Text = button.ToString().Trim(),
						TextColor = color,
						BackgroundColor = System.Drawing.Color.FromArgb(bgcolor.ToArgb() ^ 0xffffff).WithAlpha(0xc0),
						FontSize = Config.QuickButtonFontSize,
						WidthRequest = Config.QuickButtonWidth,
						HeightRequest = Config.QuickButtonHeight,
						Padding = Config.QuickButtonPadding,
					};
					quickButton.Clicked += window.quickButton_Clicked;

					if (quickButtonGroup.Children.Count - 1 < column)
						quickButtonGroup.Children.Add(layout);

					layout.Children.Add(quickButton);

					row++;
				}
				if (row > 0)
					column++;
			}

			

			quickButtonGroup.ResolveLayoutChanges();
			window.RefreshQuickButtonGroup();

			if (lockTaken)
				displayLineListSpinLock.Exit();
		}
	}
}
