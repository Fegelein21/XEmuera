using MinorShift.Emuera;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMenuPage : ContentPage
	{
		readonly ObservableCollection<MainMenuItem> MenuList = new ObservableCollection<MainMenuItem>();

		public MainMenuPage()
		{
			InitializeComponent();

			MainMenuListView.ItemsSource = MenuList;

			GameUtils.EmueraSwitched -= RefreshMainMenuListView;
			GameUtils.EmueraSwitched += RefreshMainMenuListView;
		}

		private void RefreshMainMenuListView()
		{
			MenuList.Clear();

			if (GameUtils.IsEmueraPage)
			{
				MenuList.Add(new MainMenuItem
				{
					Title = "重新加载",
					Value = "ReloadEmuera",
				});

				MenuList.Add(new MainMenuItem
				{
					Title = "回到标题界面",
					Value = "GotoTitle",
				});
			}

			MenuList.Add(new MainMenuItem
			{
				Title = "设置",
				Value = nameof(SettingsPage),
			});
		}

		private async void MainMenuListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (!(MainMenuListView.SelectedItem is MainMenuItem item))
				return;

			MainMenuListView.SelectedItem = null;

			switch (item.Value)
			{
				case nameof(SettingsPage):
					await GameUtils.MainPage.Detail.Navigation.PushAsync(new SettingsPage());
					break;
				case "ReloadEmuera":
					if (GameUtils.IsEmueraPage)
						GlobalStatic.MainWindow.MainMenu_Reboot();
					break;
				case "GotoTitle":
					if (GameUtils.IsEmueraPage)
						GlobalStatic.MainWindow.MainMenu_GotoTitle();
					break;
				default:
					break;
			}
		}

		private class MainMenuItem
		{
			public string Title { get; set; }
			public string Value { get; set; }
		}
	}
}