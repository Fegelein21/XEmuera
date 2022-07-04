using CoreFoundation;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using XEmuera.iOS;

[assembly: Dependency(typeof(IOSDependencyService))]
namespace XEmuera.iOS
{
	internal class IOSDependencyService : IPlatformService
	{
		public void CloseApplication()
		{
			Thread.CurrentThread.Abort();
		}

		public void EmueraPageAppearing()
		{
			throw new NotImplementedException();
		}

		public void EmueraPageDisappearing()
		{
			throw new NotImplementedException();
		}

		public string GetStoragePath()
		{
			throw new NotImplementedException();
		}

		public void LockScreenOrientation()
		{
			UIInterfaceOrientation orientation;

			switch (DeviceDisplay.MainDisplayInfo.Rotation)
			{
				case DisplayRotation.Rotation0:
					orientation = UIInterfaceOrientation.Portrait;
					break;
				case DisplayRotation.Rotation90:
					orientation = UIInterfaceOrientation.LandscapeLeft;
					break;
				case DisplayRotation.Rotation180:
					orientation = UIInterfaceOrientation.PortraitUpsideDown;
					break;
				case DisplayRotation.Rotation270:
					orientation = UIInterfaceOrientation.LandscapeRight;
					break;
				default:
					orientation = UIInterfaceOrientation.Unknown;
					break;
			}

			SetScreenOrientation(orientation);
		}

		public bool NeedManageFilesPermissions()
		{
			throw new NotImplementedException();
		}

		public bool NeedStoragePermissions()
		{
			throw new NotImplementedException();
		}

		public void RequestManageFilesPermissions()
		{
			throw new NotImplementedException();
		}

		public void UnlockScreenOrientation()
		{
			UIDeviceOrientation deviceOrientation = UIDevice.CurrentDevice.Orientation;
			UIInterfaceOrientation orientation;

			switch (deviceOrientation)
			{
				case UIDeviceOrientation.Portrait:
					orientation = UIInterfaceOrientation.PortraitUpsideDown;
					break;
				case UIDeviceOrientation.PortraitUpsideDown:
					orientation = UIInterfaceOrientation.Portrait;
					break;
				case UIDeviceOrientation.LandscapeLeft:
					orientation = UIInterfaceOrientation.LandscapeRight;
					break;
				case UIDeviceOrientation.LandscapeRight:
					orientation = UIInterfaceOrientation.LandscapeLeft;
					break;
				default:
					orientation = UIInterfaceOrientation.Unknown;
					break;
			}

			SetScreenOrientation(orientation);
		}

		private void SetScreenOrientation(UIInterfaceOrientation orientation)
		{
			DispatchQueue.MainQueue.DispatchAsync(() =>
			{
				UIDevice.CurrentDevice.SetValueForKey(
					NSObject.FromObject(orientation),
					new NSString("orientation"));
				UIViewController.AttemptRotationToDeviceOrientation();
			});
		}
	}
}