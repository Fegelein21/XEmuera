using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Models;
using XEmuera.Views.Popup;
using Xamarin.CommunityToolkit.Extensions;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfigPage : ContentPage
	{
		internal ConfigPage(List<ConfigModel> configModelList)
		{
			InitializeComponent();

			ConfigListView.ItemsSource = configModelList;
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
