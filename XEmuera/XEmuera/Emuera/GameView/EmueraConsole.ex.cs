using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using XEmuera;

namespace MinorShift.Emuera.GameView
{
	internal sealed partial class EmueraConsole
	{
		int lastRefreshQuickButtonGeneration = -1;

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

			if (lastRefreshQuickButtonGeneration == lastButtonGeneration)
				return;

			if (state != ConsoleState.WaitInput)
				return;

			StackLayout quickButtonGroup = window.quickButtonGroup;

			if (quickButtonGroup.Children.Count > 0)
			{
				quickButtonGroup.Children.Clear();
				quickButtonGroup.WidthRequest = -1;
				quickButtonGroup.HeightRequest = -1;

				window.buttonScrollView.WidthRequest = -1;
				window.buttonScrollView.HeightRequest = -1;
			}

			if (inputReq.InputType == GameProc.InputType.Void || inputReq.InputType == GameProc.InputType.AnyKey)
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

			string text;
			System.Drawing.Color color;
			AConsoleColoredPart coloredPart;

			for (int i = topLineNo; i <= bottomLineNo; i++)
			{
				curLine = displayLineList[i];
				row = 0;
				layout = new StackLayout
				{
					Orientation = StackOrientation.Horizontal,
					Spacing = DisplayUtils.QuickButtonSpacing,
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

					coloredPart = button.StrArray[0] as AConsoleColoredPart;
					if (coloredPart == null)
					{
						text = button.ToString();
						color = Config.ForeColor;
					}
					else
					{
						text = coloredPart.Str;
						color = coloredPart.Color;
					}

					quickButton = new Button
					{
						BindingContext = button.Inputs,
						Text = text.Trim(),
						TextColor = color,
						FontSize = 10,
						WidthRequest = DisplayUtils.QuickButtonWidth,
						HeightRequest = DisplayUtils.QuickButtonHeight,
						BackgroundColor = System.Drawing.Color.FromArgb(color.ToArgb() ^ 0xffffff).WithAlpha(0xc0),
						Padding = 2,
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

			if (quickButtonGroup.Children.Count > 0)
				lastRefreshQuickButtonGeneration = lastButtonGeneration;

			quickButtonGroup.ResolveLayoutChanges();
			window.RefreshQuickButtonGroup();

			if (lockTaken)
				displayLineListSpinLock.Exit();
		}
	}
}
