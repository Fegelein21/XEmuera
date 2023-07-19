using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using MinorShift.Emuera;
using Xamarin.Forms;
using XEmuera.Resources;

namespace XEmuera.Models
{
	/// <summary>
	/// 游戏配置管理
	/// </summary>
	internal sealed class ConfigModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		//public const string PrefKeyConfig = nameof(PrefKeyConfig);

		private const string separator = "_|_";

		private static readonly Dictionary<ConfigCode, ConfigModel> AllModels = new Dictionary<ConfigCode, ConfigModel>();

		public static List<ConfigCodeGroup> ConfigCodeGroups { get; private set; }

		public static List<ConfigCodeGroup> OtherConfigCodeGroups { get; private set; }

		public ConfigItem ConfigItem { get; private set; }

		public ConfigCode Code { get => ConfigItem.Code; }

		//public string Text { get => ConfigItem.Text; }

		public string Title { get => ConfigsText.ResourceManager.GetString(Code.ToString()); }

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

		public bool HasSwitch { get; private set; }

		private ConfigModel()
		{
		}

		public static void Load()
		{
			AllModels.Clear();

			InitConfigCodeGroups();

			InitConfigModel(ConfigCodeGroups, true);

			InitConfigModel(OtherConfigCodeGroups);
		}

		private static void InitConfigModel(List<ConfigCodeGroup> codeGroups, bool hasSwitch = false)
		{
			foreach (var group in codeGroups)
			{
				foreach (var configCode in group.Code)
				{
					ConfigItem configItem = ConfigData.Instance.GetItem(configCode);
					if (configItem == null)
						continue;

					ConfigModel configModel = new ConfigModel
					{
						ConfigItem = configItem,
						HasSwitch = hasSwitch,
					};

					switch (configCode)
					{
						case ConfigCode.PanSpeed:
							configModel.HasSwitch = false;
							break;
					}

					string pref = GameUtils.GetPreferences(configCode.ToString(), null);
					string[] data = pref?.Split(new[] { separator }, StringSplitOptions.None) ?? new string[0];

					if (data.Length >= 2 && ConfigItem.TryParse(configItem, data[1]))
					{
						configModel.Enabled = bool.Parse(data[0]);
						configModel.UpdateValue();

						configItem.ResetDefault();
					}
					else
					{
						configModel.Enabled = false;
						configModel.ResetDefault();
					}

					if (!configModel.HasSwitch)
						configModel.Enabled = true;

					AllModels[configCode] = configModel;
					configModel.PropertyChanged += ConfigItem_PropertyChanged;
				}
			}
		}

		private static void ConfigItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			((ConfigModel)sender).Save();
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

		public void UpdateValue()
		{
			Value = ConfigItem.Value;
		}

		public void ResetDefault()
		{
			Value = ConfigItem.DefaultValue;
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
					Name = StringsText.Environment,
					Code = new ConfigCode[]
					{
						ConfigCode.AutoSave,
						ConfigCode.UseSaveFolder,
						ConfigCode.EnglishConfigOutput,
						ConfigCode.MaxLog,
						ConfigCode.InfiniteLoopAlertTime,
						ConfigCode.SaveDataNos,
					}
				},
				new ConfigCodeGroup
				{
					ID = "View",
					Name = StringsText.View,
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
					Name = StringsText.Window,
					Code = new ConfigCode[]
					{
						ConfigCode.WindowX,

						ConfigCode.ScrollHeight,
						ConfigCode.PanSpeed,
					}
				},
				new ConfigCodeGroup
				{
					ID = "Font",
					Name = StringsText.Font,
					Code = new ConfigCode[]
					{
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
					Name = StringsText.System,
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
					Name = StringsText.System2,
					Code = new ConfigCode[]
					{
						ConfigCode.SystemIgnoreTripleSymbol,
						ConfigCode.SystemSaveInBinary,
						ConfigCode.SystemSaveInUTF8,
						ConfigCode.ZipSaveData,
						ConfigCode.SystemNoTarget,
						ConfigCode.ForbidUpdateCheck,
						ConfigCode.UseERD,
						ConfigCode.VarsizeDimConfig,
						ConfigCode.ValidExtension,
					}
				},
				new ConfigCodeGroup
				{
					ID = "Compati",
					Name = StringsText.Compati,
					Code = new ConfigCode[]
					{
						ConfigCode.CompatiErrorLine,
						ConfigCode.CompatiCALLNAME,
						ConfigCode.CompatiRAND,
						ConfigCode.TimesNotRigorousCalculation,
						ConfigCode.CompatiFunctionNoignoreCase,
						ConfigCode.CompatiCallEvent,
						ConfigCode.CompatiSPChara,
						ConfigCode.CompatiLinefeedAs1739,
						ConfigCode.CompatiFuncArgOptional,
						ConfigCode.CompatiFuncArgAutoConvert,
					}
				},
				new ConfigCodeGroup
				{
					ID = "Debug",
					Name = StringsText.Debug,
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
			};

			OtherConfigCodeGroups = new List<ConfigCodeGroup>
			{
				new ConfigCodeGroup
				{
					ID = "QuickButton",
					Name = StringsText.QuickButton,
					Code = new ConfigCode[]
					{
						ConfigCode.QuickButtonFontSize,
						ConfigCode.QuickButtonWidth,
						ConfigCode.QuickButtonSpacing,
					}
				},
				new ConfigCodeGroup
				{
					ID = "FontAndShape",
					Name = StringsText.FontAndShape,
					Code = new ConfigCode[]
					{
						ConfigCode.AdaptiveFont,
						ConfigCode.AdaptiveFontSize,
						ConfigCode.FontScale,
						ConfigCode.TextAntialias,
						ConfigCode.TextFilterQuality,
						ConfigCode.ShapeAntialias,
						ConfigCode.ShapeFilterQuality,
					}
				},
				new ConfigCodeGroup
				{
					ID = "LongPressSkip",
					Name = ConfigsText.LongPressSkip,
					Code = new ConfigCode[]
					{
						ConfigCode.LongPressSkip,
						ConfigCode.LongPressSkipTime,
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
