using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Models;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfigSettingsPage : ContentPage
	{
		private static readonly List<SettingsModel> Settings = new List<SettingsModel>();

		private readonly List<ConfigModel.ConfigCodeGroup> CodeGroups;

		internal ConfigSettingsPage(List<ConfigModel.ConfigCodeGroup> groups)
		{
			InitializeComponent();

			CodeGroups = groups;

			Settings.Clear();

			foreach (var item in CodeGroups)
			{
				Settings.Add(new SettingsModel
				{
					Title = item.Name,
					Value = item.ID,
				});
			}

			ConfigListView.ItemsSource = Settings;
		}

		private async void ConfigListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (!(e.Item is SettingsModel model))
				return;

			ConfigListView.SelectedItem = null;

			var configPage = new ConfigPage(CodeGroups, model.Value)
			{
				Title = model.Title,
			};
			await Navigation.PushAsync(configPage);
		}
	}
}