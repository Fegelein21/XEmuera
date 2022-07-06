using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace XEmuera.Models
{
	public static class LanguageModel
	{
		public const string PrefKeyLanguage = nameof(PrefKeyLanguage);

		public static readonly List<CultureInfo> LanguageList = new List<CultureInfo>
		{
			new CultureInfo("ja-JP"),
			new CultureInfo("zh-CN"),
		}.OrderBy(item => item.Name).ToList();

		public static CultureInfo Current
		{
			get => _current;
			set
			{
				_current = value;
				CultureInfo.CurrentUICulture = value;
				Save();
			}
		}
		private static CultureInfo _current;

		public static void Load()
		{
			string languageName = GameUtils.GetPreferences(PrefKeyLanguage, null);
			Current = Get(languageName ?? CultureInfo.CurrentUICulture.Name) ?? Get("zh-CN");
		}

		public static void Save()
		{
			GameUtils.SetPreferences(PrefKeyLanguage, Current.Name);
		}

		public static CultureInfo Get(string languageName)
		{
			if (string.IsNullOrEmpty(languageName))
				return null;
			return LanguageList.FirstOrDefault(item => item.Name == languageName);
		}
	}
}
