using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Models;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FontSettingsPage : ContentPage
	{
		public FontSettingsPage()
		{
			InitializeComponent();

			FontCollectionView.ItemsSource = new List<FontGroup>
			{
				FontModel.EnabledList,
				FontModel.DisabledList,
			};
		}

		protected override bool OnBackButtonPressed()
		{
			FontModel.Save();
			return base.OnBackButtonPressed();
		}
	}
}