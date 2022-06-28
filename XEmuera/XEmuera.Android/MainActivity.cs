using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using Android.Views;

namespace XEmuera.Droid
{
	[Activity(Label = "XEmuera", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Sensor)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		public static Activity Instance;

		private static bool Init;

		private static int UIOptions;
		private static int EmueraUIOptions;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Instance = this;

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			if (!Init)
			{
				Init = true;

				UIOptions = (int)Window.DecorView.SystemUiVisibility;
				EmueraUIOptions = UIOptions
					| (int)SystemUiFlags.HideNavigation
					| (int)SystemUiFlags.LayoutHideNavigation
					| (int)SystemUiFlags.Fullscreen
					| (int)SystemUiFlags.LayoutFullscreen
					| (int)SystemUiFlags.LayoutStable
					| (int)SystemUiFlags.ImmersiveSticky;
			}

			//最后运行app
			LoadApplication(new App());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		public override void OnWindowFocusChanged(bool hasFocus)
		{
			base.OnWindowFocusChanged(hasFocus);

			if (!hasFocus)
				return;

			if (GameUtils.IsEmueraPage)
				Window.DecorView.SystemUiVisibility = (StatusBarVisibility)EmueraUIOptions;
			else
				Window.DecorView.SystemUiVisibility = (StatusBarVisibility)UIOptions;
		}
	}
}