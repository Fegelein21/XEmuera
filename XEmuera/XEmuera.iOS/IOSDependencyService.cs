using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UIKit;
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
	}
}