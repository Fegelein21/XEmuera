using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.IO;
using Xamarin.Forms;
using XEmuera.Models;
using System.Reflection;
using System.Text.RegularExpressions;
using MinorShift.Emuera.Forms;
using System.Linq;
using SkiaSharp;
using XEmuera.Forms;
using System.Threading;
using MinorShift.Emuera;
using XEmuera.Resources;

namespace XEmuera
{
	public static class GameUtils
	{
		public const int ManageFilesPermissionsRequestCode = 100;

		public const int FileSelectorRequestCode = 101;

		private static bool Init;

		public static MainPage MainPage { get; set; }

		public static RelativeLayout MainLayout { get; set; }

		public static EraPictureBox MainPicBox { get; set; }

		private static readonly Assembly MainAssembly = typeof(MainPage).GetTypeInfo().Assembly;

		public static string CurrentGamePath { get; set; }

		public static IPlatformService PlatformService { get; private set; }

		public static bool IsEmueraPage
		{
			get => _IsEmueraPage;
			set
			{
				if (_IsEmueraPage == value && Init)
					return;
				_IsEmueraPage = value;
				EmueraSwitched?.Invoke();
				if (_IsEmueraPage)
					PlatformService.EmueraPageAppearing();
				else
					PlatformService.EmueraPageDisappearing();
			}
		}
		private static bool _IsEmueraPage;

		public static Action EmueraSwitched { get; set; }

		public static PermissionStatus StorageAccess { get; set; }

		public static void Load()
		{
			Init = false;

			PlatformService = DependencyService.Get<IPlatformService>();

			if (!RequestExternalPermissions())
			{
				MessageBox.Show(StringsText.PermissionsRequestFailed);
				PlatformService.CloseApplication();
				return;
			}

			GameFolderModel.Load();

			LanguageModel.Load();

			FontModel.Load();

			ConfigModel.Load();

			GameItemModel.Load();

			IsEmueraPage = false;

			Init = true;
		}

		public static void StartEmuera(string gamePath)
		{
			if (IsEmueraPage)
				return;

			CurrentGamePath = gamePath;

			StartEmuera();
		}

		public static void StartEmuera()
		{
			var mainWindow = MainWindow.Load();
			if (mainWindow != null)
			{
				IsEmueraPage = true;
				MainPage.Detail.Navigation.PushAsync(mainWindow, false);
			}
		}

		public static bool RequestExternalPermissions()
		{
			if (PlatformService.NeedStoragePermissions())
			{
				var task = CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
				task.Wait();
				StorageAccess = task.Result;
			}
			else
			{
				StorageAccess = PermissionStatus.Granted;
			}

			if (StorageAccess != PermissionStatus.Granted)
				return false;

			if (PlatformService.NeedManageFilesPermissions())
			{
				StorageAccess = PermissionStatus.Unknown;

				MessageBox.Show(StringsText.NeedManageFilesPermissions);
				PlatformService.RequestManageFilesPermissions();

				SpinWait.SpinUntil(() => StorageAccess != PermissionStatus.Unknown);

				if (PlatformService.NeedManageFilesPermissions())
					return false;
			}

			return true;
		}

		public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : Permissions.BasePermission
		{
			return await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				var status = await permission.CheckStatusAsync();
				if (status != PermissionStatus.Granted)
				{
					status = await permission.RequestAsync();
				}

				return status;
			});
		}

		public static Stream GetManifestResourceStream(string resourceID)
		{
			return MainAssembly.GetManifestResourceStream(resourceID);
		}

		public static void SetPreferences(string key, string value)
		{
			Preferences.Set(key, value, AppInfo.PackageName);
		}

		public static string GetPreferences(string key, string defaultValue)
		{
			return Preferences.Get(key, defaultValue, AppInfo.PackageName);
		}

		public static bool HasPreferences(string key)
		{
			return Preferences.ContainsKey(key, AppInfo.PackageName);
		}

		public static void RemovePreferences(string key)
		{
			Preferences.Remove(key, AppInfo.PackageName);
		}
	}

	public static class DisplayUtils
	{
		public const int HeightOffset = 4;

		public static readonly double ScreenDensity = DeviceDisplay.MainDisplayInfo.Density;

		public static int ShapeHeightOffset { get; set; }

		public static int MainLayoutWidth { get => GetRealLength(GameUtils.MainLayout.Width); }

		public static int MainLayoutHeight { get => GetRealLength(GameUtils.MainLayout.Height); }

		public static int PicBoxWidth { get => GetRealLength(GameUtils.MainPicBox.Width); }

		public static int PicBoxHeight { get => GetRealLength(GameUtils.MainPicBox.Height); }

		public static bool DirectionLimitX(int sign, double value, double min, double max)
		{
			sign = Math.Sign(sign);
			return (sign == -1 && value > min) || (sign == 1 && value < max);
		}

		public static bool DirectionLimitY(int sign, double value, double min, double max)
		{
			sign = Math.Sign(sign);
			return (sign == -1 && value < max) || (sign == 1 && value > min);
		}

		public static int GetRealLength(double length)
		{
			return (int)(length * ScreenDensity);
		}

		public static System.Drawing.Size ToSize(SKSizeI sizeI)
		{
			return new System.Drawing.Size(sizeI.Width, sizeI.Height);
		}

		public static System.Drawing.Color ToColor(SKColor color)
		{
			return System.Drawing.Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
		}

		public static SKColor ToSKColor(System.Drawing.Color color)
		{
			return new SKColor(color.R, color.G, color.B, color.A);
		}

		public static Color InvertColor(System.Drawing.Color color)
		{
			return Color.FromUint((uint)(color.ToArgb() ^ 0xffffff));
		}
	}

	public static class FileUtils
	{
		public static string[] GetFiles(string path, string pattern, SearchOption searchOption)
		{
			if (string.IsNullOrEmpty(pattern) || !Directory.Exists(path))
				return new string[0];

			var patterns = pattern.Split(new[] { '*' }).Select(p => Regex.Escape(p));
			Regex regex = new Regex("^" + string.Join(".*", patterns) + "$", RegexOptions.IgnoreCase);

			return Directory.EnumerateFiles(path, "*", searchOption).Where(file => regex.IsMatch(Path.GetFileName(file))).ToArray();
		}

		public static bool Exists(ref string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				return false;

			string directoryName = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(directoryName))
			{
				directoryName = directoryName.ToUpper();
				if (!Directory.Exists(directoryName))
				{
					directoryName = directoryName.ToLower();
					if (!Directory.Exists(directoryName))
						return false;
				}
			}

			if (File.Exists(filePath))
				return true;

			filePath = filePath.ToUpper();
			if (File.Exists(filePath))
				return true;

			filePath = filePath.ToLower();
			if (File.Exists(filePath))
				return true;

			Regex regex = new Regex("^" + Regex.Escape(Path.GetFileName(filePath)) + "$", RegexOptions.IgnoreCase);
			string path = Directory.EnumerateFiles(directoryName).FirstOrDefault(file => regex.IsMatch(Path.GetFileName(file)));

			if (path != null)
			{
				filePath = path;
				return true;
			}

			return false;
		}
	}

	public class Mapping<T1, T2>
	{
		readonly Dictionary<T1, T2> KeyDictionary = new Dictionary<T1, T2>();
		readonly Dictionary<T2, T1> ValueDictionary = new Dictionary<T2, T1>();

		public void Add(T1 key, T2 value)
		{
			KeyDictionary.Add(key, value);
			ValueDictionary.Add(value, key);
		}

		public bool GetByValue(T2 value, out T1 result, T1 defaultKey = default)
		{
			bool success = ValueDictionary.TryGetValue(value, out result);
			if (!success)
				result = defaultKey;
			return success;
		}

		public bool GetByKey(T1 key, out T2 result, T2 defaultValue = default)
		{
			bool success = KeyDictionary.TryGetValue(key, out result);
			if (!success)
				result = defaultValue;
			return success;
		}

		public bool RemoveByKey(T1 key)
		{
			if (!KeyDictionary.TryGetValue(key, out var result))
				return false;

			KeyDictionary.Remove(key);
			return ValueDictionary.Remove(result);
		}

		public bool RemoveByValue(T2 value)
		{
			if (!ValueDictionary.TryGetValue(value, out var result))
				return false;

			ValueDictionary.Remove(value);
			return KeyDictionary.Remove(result);
		}

		public void Clear()
		{
			KeyDictionary.Clear();
			ValueDictionary.Clear();
		}
	}

	public interface IPlatformService
	{
		void CloseApplication();

		void EmueraPageAppearing();

		void EmueraPageDisappearing();

		string GetStoragePath();

		void LockScreenOrientation();

		bool NeedManageFilesPermissions();

		bool NeedRebootIfLanguageChanged();

		bool NeedStoragePermissions();

		void RequestManageFilesPermissions();

		void UnlockScreenOrientation();
	}

	public static class Strings
	{
		public static string StrConv(string str, VbStrConv Conversion, int LocaleID = 0)
		{

			switch (Conversion & (VbStrConv.Katakana | VbStrConv.Hiragana))
			{
				//case VbStrConv.Katakana | VbStrConv.Hiragana:
				//	throw new ArgumentException(Utils.GetResourceString("Argument_IllegalKataHira"));
				case VbStrConv.Katakana:
					str = ToKatakana(str);
					break;
				case VbStrConv.Hiragana:
					str = ToHiragana(str);
					break;
			}

			switch (Conversion & (VbStrConv.Wide | VbStrConv.Narrow))
			{
				//case VbStrConv.Wide | VbStrConv.Narrow:
				//	throw new ArgumentException(Utils.GetResourceString("Argument_IllegalWideNarrow"));
				case VbStrConv.Wide:
					str = ToSBC(str);
					break;
				case VbStrConv.Narrow:
					str = ToDBC(str);
					break;
			}

			return str;
		}

		/// <summary>
		/// 转全角的函数。
		/// 全角空格为12288，半角空格为32。
		/// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248。
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ToSBC(string input)
		{
			// 半角转全角：
			char[] c = input.ToCharArray();
			for (int i = 0; i < c.Length; i++)
			{
				if (c[i] == 0x20)
					c[i] = (char)0x3000;
				else if (c[i] >= 0x21 && c[i] <= 0x7E)
					c[i] = (char)(c[i] + 0xFEE0);
			}
			return new string(c);
		}

		/// <summary>
		/// 转半角的函数。
		/// 全角空格为12288，半角空格为32。
		/// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248。
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ToDBC(string input)
		{
			char[] c = input.ToCharArray();
			for (int i = 0; i < c.Length; i++)
			{
				if (c[i] == 0x3000)
					c[i] = (char)0x20;
				else if (c[i] >= 0xFF01 && c[i] <= 0xFF5E)
					c[i] = (char)(c[i] - 0xFEE0);
			}
			return new string(c);
		}

		/// <summary>
		/// 转为片假名
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ToKatakana(string input)
		{
			char[] c = input.ToCharArray();
			for (int i = 0; i < c.Length; i++)
			{
				if (c[i] >= 0x3041 && c[i] <= 0x3096)
					c[i] = (char)(c[i] + 0x60);
			}
			return new string(c);
		}

		/// <summary>
		/// 转为平假名
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ToHiragana(string input)
		{
			char[] c = input.ToCharArray();
			for (int i = 0; i < c.Length; i++)
			{
				if (c[i] >= 0x30A1 && c[i] <= 0x30F6)
					c[i] = (char)(c[i] - 0x60);
			}
			return new string(c);
		}
	}

	[Flags]
	public enum VbStrConv
	{
		None = 0x0,
		Uppercase = 0x1,
		Lowercase = 0x2,
		ProperCase = 0x3,
		Wide = 0x4,
		Narrow = 0x8,
		Katakana = 0x10,
		Hiragana = 0x20,
		SimplifiedChinese = 0x100,
		TraditionalChinese = 0x200,
		LinguisticCasing = 0x400
	}
}
