using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Models;
using MinorShift.Emuera;
using Xamarin.CommunityToolkit.Extensions;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GameItemPage : ContentPage
	{
		public GameItemPage()
		{
			InitializeComponent();

			GameItemListView.ItemsSource = GameItemModel.AllModels;

			Task.Run(() =>
			{
				GameUtils.Load();
			});
		}

		private void ListView_Refreshing(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				GameItemModel.Load();
				GameItemListView.EndRefresh();
			});
		}

		bool CloseApplication;

		protected override bool OnBackButtonPressed()
		{
			if (CloseApplication)
				GameUtils.PlatformService.CloseApplication();

			Task.Run(async () =>
			{
				CloseApplication = true;
				CloseApplication = await this.DisplaySnackBarAsync("再按一次返回键退出应用。", null, null);
			});

			return true;
		}

		private void GameItemListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (!(GameItemListView.SelectedItem is GameItemModel item))
				return;

			GameItemListView.SelectedItem = null;

			GameUtils.StartEmuera(item.Path);
		}
	}
}