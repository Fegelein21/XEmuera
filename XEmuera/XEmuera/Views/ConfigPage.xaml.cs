using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Models;
using Xamarin.CommunityToolkit.Extensions;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfigPage : ContentPage
	{
		internal ConfigPage(string groupId)
		{
			InitializeComponent();

			var group = ConfigModel.ConfigCodeGroups.FirstOrDefault(group => group.ID == groupId);
			if (group != null && group.Code != null)
				ConfigListView.ItemsSource = group.Code.Select(code => ConfigModel.Get(code)).Where(model => model != null);
		}

		private async void ConfigListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (!(ConfigListView.SelectedItem is ConfigModel model))
				return;

			ConfigListView.SelectedItem = null;

			await Navigation.ShowPopupAsync(new ConfigValuePopup(model));
		}
	}
}
