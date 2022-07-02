using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using MinorShift.Emuera;
using Xamarin.Forms;

namespace XEmuera.Models
{
	/// <summary>
	/// 游戏配置管理
	/// </summary>
	internal sealed class ConfigModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public const string PrefKeyConfig = nameof(PrefKeyConfig);

		private const string separator = "_|_";

		private static readonly Dictionary<ConfigCode, ConfigModel> AllModels = new Dictionary<ConfigCode, ConfigModel>();

		public static List<ConfigCodeGroup> ConfigCodeGroups { get; private set; }

		public readonly ConfigItem ConfigItem;

		public ConfigCode Code { get => ConfigItem.Code; }

		public string Text { get => ConfigItem.Text; }

		public object Value
		{
			get { return _value; }
			set
			{
				if (_value == value)
					return;
				_value = value;
				ValueText = ConfigItem.ValueToString(value);
				if (_value is System.Drawing.Color color)
				{
					IsValueColor = true;
					ValueColor = color;
				}
				else
					IsValueColor = false;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}
		object _value;

		public string ValueText
		{
			get { return _valueText; }
			private set
			{
				if (_valueText == value)
					return;
				_valueText = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueText)));
			}
		}
		string _valueText;

		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				if (_enabled == value)
					return;
				_enabled = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enabled)));
			}
		}
		bool _enabled;

		public bool IsValueColor { get; private set; }

		public Color ValueColor
		{
			get { return _valueColor; }
			private set
			{
				if (_valueColor == value)
					return;
				_valueColor = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueColor)));
			}
		}
		Color _valueColor = Color.Gray;

		public ConfigModel(ConfigItem configItem)
		{
			ConfigItem = configItem;
		}

		public static void Load()
		{
			AllModels.Clear();

			InitConfigCodeGroups();

			foreach (var group in ConfigCodeGroups)
			{
				foreach (var configCode in group.Code)
				{
					ConfigItem configItem = ConfigData.Instance.GetItem(configCode);
					if (configItem == null)
						continue;

					ConfigModel configModel = new ConfigModel(configItem);

					string pref = GameUtils.GetPreferences(PrefKeyConfig, null);
					string[] data = pref == null
						? new string[0] : pref.Split(new[] { separator }, StringSplitOptions.None);

					if (data.Length >= 2 && configItem.TryParse(data[1]))
					{
						configModel.Enabled = bool.Parse(data[0]);
						configModel.ResetConfig();
						configItem.ResetValue();
					}
					else
					{
						configModel.Enabled = false;
						configModel.Value = configItem.Value;
					}

					AllModels[configCode] = configModel;
					configModel.PropertyChanged += ConfigItem_PropertyChanged;
				}
			}
		}

		private static void ConfigItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			ConfigModel configModel = sender as ConfigModel;
			configModel.Save();
		}

		public void Save()
		{
			string[] data = new[]
			{
				Enabled.ToString(),
				ValueText
			};
			GameUtils.SetPreferences(Code.ToString(), string.Join(separator, data));
		}

		public void ResetConfig()
		{
			Value = ConfigItem.Value;
		}

		public static ConfigModel Get(ConfigCode code)
		{
			if (AllModels.TryGetValue(code, out ConfigModel model))
				return model;
			return null;
		}

		private static void InitConfigCodeGroups()
		{
			ConfigCodeGroups = new List<ConfigCodeGroup>
			{
				new ConfigCodeGroup
				{
					ID = "Environment",
					Name = "环境",
					Code = new ConfigCode[]
					{
						ConfigCode.AutoSave,
						ConfigCode.UseSaveFolder,
						ConfigCode.MaxLog,
						ConfigCode.InfiniteLoopAlertTime,
						ConfigCode.SaveDataNos,
					}
				},
				new ConfigCodeGroup
				{
					ID = "Display",
					Name = "显示",
					Code = new ConfigCode[]
					{
						//ConfigCode.TextDrawingMode,

						ConfigCode.FPS,
						ConfigCode.PrintCPerLine,
						ConfigCode.PrintCLength,
						ConfigCode.ButtonWrap,
					}
				},
				new ConfigCodeGroup
				{
					ID = "Window",
					Name = "窗口",
					Code = new ConfigCode[]
					{
						ConfigCode.WindowX,

						ConfigCode.ScrollHeight,
						ConfigCode.PanSpeed,
					}
				},
				new ConfigCodeGroup
				{
					ID = "Text",
					Name = "文字",
					Code = new ConfigCode[]
					{
						ConfigCode.FontScale,
						ConfigCode.TextAntialias,
						ConfigCode.TextFilterQuality,
						ConfigCode.ShapeAntialias,
						ConfigCode.ShapeFilterQuality,

						ConfigCode.ForeColor,
						ConfigCode.BackColor,
						ConfigCode.FocusColor,
						ConfigCode.LogColor,

						ConfigCode.FontName,
						ConfigCode.FontSize,
						ConfigCode.LineHeight,
					}
				},
				new ConfigCodeGroup
				{
					ID = "System",
					Name = "系统",
					Code = new ConfigCode[]
					{
						ConfigCode.IgnoreCase,
						ConfigCode.UseRenameFile,
						ConfigCode.UseReplaceFile,
						ConfigCode.SearchSubdirectory,
						ConfigCode.SortWithFilename,
						ConfigCode.AllowFunctionOverloading,
						ConfigCode.WarnFunctionOverloading,
						ConfigCode.WarnFunctionOverloading,
						ConfigCode.WarnNormalFunctionOverloading,
						ConfigCode.SystemAllowFullSpace,
						ConfigCode.useLanguage,
					}
				},
				new ConfigCodeGroup
				{
					ID = "System2",
					Name = "系统2",
					Code = new ConfigCode[]
					{
						ConfigCode.SystemIgnoreTripleSymbol,
						ConfigCode.SystemSaveInBinary,
						ConfigCode.SystemSaveInUTF8,
						ConfigCode.SystemNoTarget,
					}
				},
				new ConfigCodeGroup
				{
					ID = "Debug",
					Name = "调试",
					Code = new ConfigCode[]
					{
						ConfigCode.WarnBackCompatibility,
						ConfigCode.DisplayReport,
						ConfigCode.ReduceArgumentOnLoad,
						ConfigCode.DisplayWarningLevel,
						ConfigCode.IgnoreUncalledFunction,
						ConfigCode.FunctionNotFoundWarning,
						ConfigCode.FunctionNotCalledWarning,
					}
				},
				new ConfigCodeGroup
				{
					ID = "QuickButton",
					Name = "快捷按钮",
					Code = new ConfigCode[]
					{
						ConfigCode.QuickButtonColumn,
						ConfigCode.QuickButtonFontSize,
						ConfigCode.QuickButtonWidth,
						ConfigCode.QuickButtonSpacing,
					}
				},
			};
		}

		public class ConfigCodeGroup
		{
			public string ID { get; set; }
			public string Name { get; set; }
			public ConfigCode[] Code { get; set; }
		}
	}
}
