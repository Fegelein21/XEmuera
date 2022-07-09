using MinorShift.Emuera;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace XEmuera
{
	public static class VersionUpdate
	{
		public const string PrefKeyAppVersion = "AppVersion";

		public static void Check()
		{
			string pref = GameUtils.GetPreferences(PrefKeyAppVersion, null);
			bool hasVersion = pref != null;
			hasVersion &= int.TryParse(pref, out var version);

			if (!hasVersion || version < 4)
			{
				GameUtils.RemovePreferences(ConfigCode.FontScale.ToString());
			}

			GameUtils.SetPreferences(PrefKeyAppVersion, AppInfo.BuildString);
		}
	}
}
