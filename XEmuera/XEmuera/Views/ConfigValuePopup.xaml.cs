using System;
using System.Globalization;
using MinorShift.Emuera;
using Spillman.Xamarin.Forms.ColorPicker;
using Xamarin.Forms;
using XEmuera.Models;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using XEmuera.Forms;
using SkiaSharp;
using XEmuera.Resources;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfigValuePopup : Xamarin.CommunityToolkit.UI.Views.Popup
	{
		private readonly ConfigModel ConfigModel;

		internal ConfigValuePopup(ConfigModel model)
		{
			InitializeComponent();

			ConfigModel = model;
			ConfigModel.ConfigItem.Fixed = false;
			TitleLabel.Text = ConfigModel.Title;

			ConfigToggleGroup.IsVisible = ConfigModel.HasSwitch;

			ConfigToggleLabel.BindingContext = ConfigToggle;
			ConfigToggleLabel.SetBinding(Label.TextProperty, nameof(Switch.IsToggled), default, new ConfigToggleConverter());
			ConfigToggle.IsToggled = ConfigModel.Enabled;

			ConfigEntry.Text = ConfigModel.ValueText;

			InitContentLayout();
		}

		private void MainLayout_SizeChanged(object sender, EventArgs e)
		{
			var info = DeviceDisplay.MainDisplayInfo;
			Size = new Size(MainLayout.Width, Math.Min(info.Height / info.Density - 40d, MainLayout.Height));
		}

		protected override void LightDismiss()
		{
			base.LightDismiss();
			UnSubscribeRadioButtonGroup();
		}

		private void Close()
		{
			Dismiss(null);
			UnSubscribeRadioButtonGroup();
		}

		private void OnDefault(object sender, EventArgs e)
		{
			if (ConfigModel.Value is Enum)
				RadioButtonGroup.SetSelectedValue(ContentLayout, ConfigModel.ConfigItem.DefaultValue);
			else
				ConfigEntry.Text = ConfigItem.ValueToString(ConfigModel.ConfigItem.DefaultValue);
		}

		protected virtual void OnClose(object sender, EventArgs e)
		{
			Close();
		}

		protected virtual void OkButton_Clicked(object sender, EventArgs e)
		{
			if (ConfigItem.TryParse(ConfigModel.ConfigItem, ConfigEntry.Text))
			{
				ConfigModel.Enabled = ConfigToggle.IsToggled;
				ConfigModel.UpdateValue();
				Close();
			}
			else
			{
				MessageBox.Show("无效的输入值。");
			}

			ConfigModel.ConfigItem.ResetDefault();
		}

		private void InitContentLayout()
		{
			object Value = ConfigModel.Value;

			if (Value is bool b)
			{
				Switch valueSwitch = new Switch
				{
					IsToggled = b,
				};

				Label label = new Label
				{
					BindingContext = valueSwitch,
				};
				label.SetBinding(Label.TextProperty, nameof(Switch.IsToggled), default, new SwitchBoolConverter());

				StackLayout layout = new StackLayout
				{
					Orientation = StackOrientation.Horizontal
				};
				layout.Children.Add(valueSwitch);
				layout.Children.Add(label);

				ContentLayout.Children.Add(layout);

				ConfigEntry.BindingContext = valueSwitch;
				ConfigEntry.SetBinding(Entry.TextProperty, nameof(Switch.IsToggled));
			}
			else if (Value is float f)
			{
				Slider valueSlider = new Slider
				{
					Maximum = 5f,
					Minimum = 1f,
					Value = f,
					MaximumTrackColor = Color.LightGray,
					MinimumTrackColor = Color.LightGray,
					Margin = new Thickness(0, 6, 0, 4),
				};
				valueSlider.ValueChanged += (sender, e) =>
				{
					valueSlider.Value = Math.Round(e.NewValue, 2);
				};

				ContentLayout.Children.Add(valueSlider);
				EntryGroup.IsVisible = true;
				ConfigEntry.BindingContext = valueSlider;
				ConfigEntry.SetBinding(Entry.TextProperty, nameof(Slider.Value), BindingMode.TwoWay);
			}
			else if (Value is System.Drawing.Color color)
			{
				ColorPickerViewModel model = new ColorPickerViewModel
				{
					IsHexEnabled = true,
					Color = color,
				};

				ColorPickerView colorPicker = new ColorPickerView
				{
					ViewModel = model
				};
				model.PropertyChanged += (sender, e) =>
				{
					if (e.PropertyName != nameof(ColorPickerViewModel.Hex))
						return;
					string text = ConfigItem.ValueToString((System.Drawing.Color)model.Color);
					if (ConfigEntry.Text != text)
						ConfigEntry.Text = text;
				};

				ContentLayout.Children.Add(colorPicker);
				EntryGroup.IsVisible = true;
				ConfigEntry.BindingContext = model;
			}
			else if (Value is UseLanguage useLanguage)
			{
				AddEnumContentLayout(useLanguage);
			}
			else if (Value is ReduceArgumentOnLoadFlag reduceArgumentOnLoadFlag)
			{
				AddEnumContentLayout(reduceArgumentOnLoadFlag);
			}
			else if (Value is DisplayWarningFlag displayWarningFlag)
			{
				AddEnumContentLayout(displayWarningFlag);
			}
			else if (Value is SKFilterQuality sKFilterQuality)
			{
				AddEnumContentLayout(sKFilterQuality);
			}
			else
			{
				ContentLayout.IsVisible = false;
				EntryGroup.IsVisible = true;
			}
		}

		private void AddEnumContentLayout<T>(T selectedValue) where T : Enum
		{
			ContentLayout.Spacing = 2;

			foreach (T item in Enum.GetValues(typeof(T)))
			{
				RadioButton radioButton = new RadioButton
				{
					Content = item,
					Value = item,
				};

				ContentLayout.Children.Add(radioButton);
				radioButton.CheckedChanged += (sender, e) =>
				{
					if (radioButton.IsChecked)
						ConfigEntry.Text = radioButton.Value.ToString();
				};
			}
			RadioButtonGroup.SetGroupName(ContentLayout, nameof(T));
			RadioButtonGroup.SetSelectedValue(ContentLayout, selectedValue);

			ConfigEntry.BindingContext = ContentLayout;
		}

		private void UnSubscribeRadioButtonGroup()
		{
			RadioButtonGroup.SetGroupName(ContentLayout, null);
		}

		private void ConfigEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(ConfigEntry.Text))
				return;

			if (ConfigEntry.BindingContext is ColorPickerViewModel model)
			{
				if (ConfigItem.TryStringsToColor(ConfigEntry.Text, out System.Drawing.Color color))
				{
					if (model.Color != (Color)color)
						model.Color = color;
				}
			}
		}
	}

	public class ConfigToggleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? StringsText.OverwriteGameConfig : StringsText.UseGameConfig;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return false;
		}
	}

	public class SwitchBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? StringsText.Enabled : StringsText.Disabled;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return false;
		}
	}
}
