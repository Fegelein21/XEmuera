using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Forms;
using XEmuera.Models;
using XEmuera.Resources;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LanguagePage : ContentPage
	{
		public LanguagePage()
		{
			InitializeComponent();

			LanguageListView.ItemsSource = LanguageModel.LanguageList;
		}

		private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (!(LanguageListView.SelectedItem is CultureInfo cultureInfo))
				return;

			LanguageListView.SelectedItem = null;

			LanguageModel.Current = cultureInfo;

			if (!GameUtils.PlatformService.NeedRebootIfLanguageChanged())
				return;

			var message = string.Format(StringsText.LanguageChangedNeedReboot, LanguageModel.Current.NativeName);
			MessageBox.ShowOnMainThread(message, null, (result) =>
			{
				if (result)
					GameUtils.PlatformService.CloseApplication();
			}, MessageBoxButtons.YesNo);
		}
	}
}