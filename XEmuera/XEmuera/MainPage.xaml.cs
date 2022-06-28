using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEmuera
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : FlyoutPage
	{
		public MainPage()
		{
			InitializeComponent();

			GameUtils.MainPage = this;

			MainMenu.MainMenuListView.ItemSelected -= MainMenuListView_ItemSelected;
			MainMenu.MainMenuListView.ItemSelected += MainMenuListView_ItemSelected;
		}

		private void MainMenuListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			IsPresented = false;
		}
	}
}
