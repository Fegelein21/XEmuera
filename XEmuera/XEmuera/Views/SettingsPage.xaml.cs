using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Models;
using XEmuera.Resources;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		private static List<SettingsModel> Settings;

		public SettingsPage()
		{
			InitializeComponent();

			Title = StringsText.Settings;

			InitList();

			SettingsListView.ItemsSource = Settings;
		}

		private static void InitList()
		{
			Settings = new List<SettingsModel>
			{
				new SettingsModel
				{
					Title = StringsText.ConfigSettings,
					Value = nameof(StringsText.ConfigSettings),
				},
				new SettingsModel
				{
					Title = StringsText.OtherConfigSettings,
					Value = nameof(StringsText.OtherConfigSettings),
				},
				new SettingsModel
				{
					Title = StringsText.FontReplaceOrder,
					Value = nameof(FontSettingsPage),
				},
				new SettingsModel
				{
					Title = StringsText.Language,
					Value = nameof(LanguagePage),
				},
				new SettingsModel
				{
					Title = StringsText.About,
					Value = nameof(AboutPage),
				}
			};
		}

		private async void SettingsListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (!(e.Item is SettingsModel model))
				return;

			SettingsListView.SelectedItem = null;

			switch (model.Value)
			{
				case nameof(StringsText.ConfigSettings):
					await Navigation.PushAsync(new ConfigSettingsPage(ConfigModel.ConfigCodeGroups)
					{
						Title = model.Title,
					});
					break;
				case nameof(StringsText.OtherConfigSettings):
					await Navigation.PushAsync(new ConfigSettingsPage(ConfigModel.OtherConfigCodeGroups)
					{
						Title = model.Title,
					});
					break;
				case nameof(FontSettingsPage):
					await Navigation.PushAsync(new FontSettingsPage
					{
						Title = model.Title
					});
					break;
				case nameof(LanguagePage):
					await Navigation.PushAsync(new LanguagePage
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
			}
		}
	}

	public class SettingsModel
	{
		public string Title { get; set; }
		public string Value { get; set; }
	}
}