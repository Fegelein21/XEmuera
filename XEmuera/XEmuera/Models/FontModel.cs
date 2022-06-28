using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using XEmuera.Drawing;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace XEmuera.Models
{
	public class FontModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private static bool Init;

		public const string PrefKeyEnabledFont = nameof(PrefKeyEnabledFont);

		private const string separator = "_|_";

		private const string DefaultFontName = "MS Gothic";

		private const string FinalFontName = "Microsoft YaHei";

		public static readonly List<FontModel> UserList = new List<FontModel>();

		public static FontGroup EnabledList;

		public static FontGroup DisabledList;

		private static readonly Dictionary<string, FontModel> AllModels = new Dictionary<string, FontModel>();

		public static FontModel Default { get; private set; }

		public static FontModel Final { get; private set; }

		private static readonly Mapping<string, string> FontNameMapping = new Mapping<string, string>();

		public string Name { get; private set; }

		public string OtherName { get; private set; }

		public SKTypeface Typeface { get; private set; }

		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				if (_enabled == value && Init)
					return;
				_enabled = value;
				Sort();
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enabled)));
			}
		}
		bool _enabled;

		public FontModel(SKTypeface typeface)
		{
			Typeface = typeface;
			Name = typeface.FamilyName;
		}

		private void Sort()
		{
			EnabledList.Remove(this);
			DisabledList.Remove(this);

			if (_enabled)
				EnabledList.Add(this);
			else
				DisabledList.Insert(0, this);
		}

		public static void Load()
		{
			Init = false;

			InitFontMapping();

			AddFontFromResource(DefaultFontName);
			AddFontFromResource(FinalFontName);

			Default = AllModels[DefaultFontName];

			foreach (var item in GameFolderModel.AllModels)
			{
				string path = item.Path + Path.DirectorySeparatorChar + "fonts";
				if (!Directory.Exists(path))
					continue;

				var list = FileUtils.GetFiles(path, "*.ttf", SearchOption.AllDirectories);
				if (list.Length == 0)
					continue;

				foreach (var fontFile in list)
				{
					string fileName = Path.GetFileNameWithoutExtension(fontFile);
					if (AllModels.ContainsKey(fileName))
						continue;

					var typeface = SKTypeface.FromFile(fontFile);
					if (typeface == null)
						continue;

					if (AllModels.ContainsKey(typeface.FamilyName))
						typeface.Dispose();
					else
						AddFontModel(typeface, fileName);
				}
			}

			EnabledList = new FontGroup
			{
				Name = "主动按顺序接替",
			};
			DisabledList = new FontGroup(AllModels.Values)
			{
				Name = "仅由游戏自行调用",
			};

			string enabledFont = GameUtils.GetPreferences(PrefKeyEnabledFont, null);
			if (string.IsNullOrEmpty(enabledFont))
			{
				Default.Enabled = true;
				AllModels[FinalFontName].Enabled = true;
			}
			else
			{
				foreach (var item in enabledFont.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries))
				{
					if (AllModels.TryGetValue(item, out var fontModel))
						fontModel.Enabled = true;
				}
			}

			Save();

			Init = true;
		}

		public static void Save()
		{
			UserList.Clear();
			foreach (var item in EnabledList)
				UserList.Add(item);
			foreach (var item in DisabledList)
				UserList.Add(item);

			if (EnabledList.Count > 0)
				Final = EnabledList.Last();
			else
				Final = Default;

			GameUtils.SetPreferences(PrefKeyEnabledFont, string.Join(separator, EnabledList.Select(item => item.Name)));

			DrawTextUtils.Load();
		}

		private static void InitFontMapping()
		{
			FontNameMapping.Clear();

			FontNameMapping.Add("ＭＳ ゴシック", "MS Gothic");
			FontNameMapping.Add("ＭＳ Ｐゴシック", "MS PGothic");
			FontNameMapping.Add("ＭＳ 明朝", "MS Mincho");
			FontNameMapping.Add("ＭＳ Ｐ明朝", "MS PMincho");
			FontNameMapping.Add("微软雅黑", "Microsoft YaHei");
		}

		private static void AddFontFromResource(string fileName)
		{
			if (AllModels.ContainsKey(fileName))
				return;

			string path = $"XEmuera.Resources.Fonts.{fileName}.ttf";
			SKTypeface sKTypeface = SKTypeface.FromStream(GameUtils.GetManifestResourceStream(path));

			if (sKTypeface == null)
				throw new ArgumentNullException("找不到字体文件。");

			AddFontModel(sKTypeface, fileName);
		}

		private static void AddFontModel(SKTypeface sKTypeface, string fileName)
		{
			var fontModel = new FontModel(sKTypeface);
			AllModels.Add(fontModel.Name, fontModel);

			FontNameMapping.GetByValue(fontModel.Name, out var otherName, fileName);
			fontModel.OtherName = otherName;
		}

		public static bool HasFont(string fontName)
		{
			if (FontNameMapping.GetByKey(fontName, out string newFontName))
				fontName = newFontName;
			return AllModels.ContainsKey(fontName);
		}

		public static FontModel GetFont(string fontName)
		{
			if (FontNameMapping.GetByKey(fontName, out string newFontName))
				fontName = newFontName;
			if (AllModels.TryGetValue(fontName, out FontModel model))
				return model;
			return Default;
		}
	}

	public class FontGroup : ObservableCollection<FontModel>
	{
		public string Name { get; set; }

		public FontGroup() : base() { }

		public FontGroup(IEnumerable<FontModel> models) : base(models) { }
	}
}
