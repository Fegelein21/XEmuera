using MinorShift.Emuera;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Models;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		static List<SettingsModel> Settings = null;

		public SettingsPage()
		{
			InitializeComponent();

			if (Settings == null)
			{
				Settings = new List<SettingsModel>();
				foreach (var item in ConfigModel.ConfigSettings.Keys)
				{
					Settings.Add(new SettingsModel
					{
						Title = Convert(item),
						Value = item,
					});
				}
				Settings.Add(new SettingsModel
				{
					Title = "字体接替顺序",
					Value = nameof(FontSettingsPage),
				});
				Settings.Add(new SettingsModel
				{
					Title = "关于",
					Value = nameof(AboutPage),
				});
			}

			SettingsListView.ItemsSource = Settings;
		}

		private async void SettingsListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (!(e.Item is SettingsModel model))
				return;

			SettingsListView.SelectedItem = null;

			switch (model.Value)
			{
				case nameof(FontSettingsPage):
					await Navigation.PushAsync(new FontSettingsPage
					{
						Title = model.Title
					});
					break;
				case nameof(AboutPage):
					await Navigation.PushAsync(new AboutPage
					{
						Title = model.Title
					});
					break;
				default:
					var configPage = new ConfigPage(ConfigModel.ConfigSettings[model.Value])
					{
						Title = model.Title
					};
					await Navigation.PushAsync(configPage);
					break;
			}
		}
		private static string Convert(string text)
		{
			switch (text)
			{
				case "Environment":
					return "环境";
				case "Window":
					return "窗口";
				case "Text":
					return "文字";
				default:
					throw new System.InvalidCastException();
			}
		}

		public class SettingsModel
		{
			public string Title { get; set; }
			public string Value { get; set; }
		}
	}
}