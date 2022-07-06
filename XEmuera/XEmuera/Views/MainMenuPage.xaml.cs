using MinorShift.Emuera;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Resources;

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
					Title = StringsText.Reboot,
					Value = nameof(StringsText.Reboot),
				});

				MenuList.Add(new MainMenuItem
				{
					Title = StringsText.GotoTitle,
					Value = nameof(StringsText.GotoTitle),
				});
			}

			MenuList.Add(new MainMenuItem
			{
				Title = StringsText.Settings,
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
				case nameof(StringsText.Reboot):
					if (GameUtils.IsEmueraPage)
						GlobalStatic.MainWindow.MainMenu_Reboot();
					break;
				case nameof(StringsText.GotoTitle):
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