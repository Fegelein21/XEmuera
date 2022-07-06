using MinorShift.Emuera;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEmuera
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			Task.Run(() =>
			{
				GameUtils.Load();
			});

			MainPage = new MainPage();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}

		public static void DoEvents()
		{
		}
	}
}
