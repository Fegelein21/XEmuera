using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using XEmuera.Droid;

[assembly: Dependency(typeof(DroidDependencyService))]
namespace XEmuera.Droid
{
	internal class DroidDependencyService : IPlatformService
	{
		public void CloseApplication()
		{
			MainActivity.Instance.FinishAffinity();
		}

		public void EmueraPageAppearing()
		{
			MainActivity.Instance.OnWindowFocusChanged(true);
		}

		public void EmueraPageDisappearing()
		{
			MainActivity.Instance.OnWindowFocusChanged(true);
		}

		public string GetStoragePath()
		{
			return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
		}

		public void LockScreenOrientation()
		{
			ScreenOrientation orientation;

			switch (DeviceDisplay.MainDisplayInfo.Rotation)
			{
				case DisplayRotation.Rotation0:
					orientation = ScreenOrientation.Portrait;
					break;
				case DisplayRotation.Rotation90:
					orientation = ScreenOrientation.Landscape;
					break;
				case DisplayRotation.Rotation180:
					orientation = ScreenOrientation.ReversePortrait;
					break;
				case DisplayRotation.Rotation270:
					orientation = ScreenOrientation.ReverseLandscape;
					break;
				default:
					orientation = ScreenOrientation.Unspecified;
					break;
			}

			MainActivity.Instance.RequestedOrientation = orientation;
		}

		public bool NeedManageFilesPermissions()
		{
			return (int)Build.VERSION.SdkInt >= 30 && !Android.OS.Environment.IsExternalStorageManager;
		}

		public bool NeedRebootIfLanguageChanged()
		{
			return (int)Build.VERSION.SdkInt <= 32;
		}

		public bool NeedStoragePermissions()
		{
			return (int)Build.VERSION.SdkInt >= 24;
		}

		public void RequestManageFilesPermissions()
		{
			Intent intent = new Intent(Android.Provider.Settings.ActionManageAppAllFilesAccessPermission);
			intent.SetData(Android.Net.Uri.Parse("package:" + MainActivity.Instance.PackageName));

			MainActivity.Instance.StartActivityForResult(intent, GameUtils.ManageFilesPermissionsRequestCode);
		}

		public void UnlockScreenOrientation()
		{
			MainActivity.Instance.RequestedOrientation = ScreenOrientation.Sensor;
		}
	}
}