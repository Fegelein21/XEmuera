using MinorShift.Emuera;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using XEmuera.Models;

namespace XEmuera.Drawing
{
	public static class DrawTextUtils
	{
		private static readonly ObservableCollection<FontCache> FontCaches = new ObservableCollection<FontCache>();

		private static readonly List<FontCache> EnabledFontCaches = new List<FontCache>();

		private static SKPaint Paint;

		private static char[] CharList;

		private static int CharRemain;

		private static TextUnit[] TextUnits;

		private static FontCache UserCache;

		private static char Char;

		private static float Advance;

		public static void Load()
		{
			for (int i = FontCaches.Count - 1; i >= 0; i--)
			{
				if (!FontModel.UserList.Contains(FontCaches[i].FontModel))
					FontCaches.RemoveAt(i);
			}

			foreach (var fontModel in FontModel.UserList)
			{
				if (FontCaches.Any(item => item.FontModel == fontModel))
					continue;
				FontCaches.Add(new FontCache
				{
					FontModel = fontModel
				});
			}

			EnabledFontCaches.Clear();
			for (int i = 0; i < FontModel.UserList.Count; i++)
			{
				var fontModel = FontModel.UserList[i];
				var fontCache = FontCaches.FirstOrDefault(item => item.FontModel == fontModel);

				int index = FontCaches.IndexOf(fontCache);
				if (index != i)
					FontCaches.Move(index, i);

				if (fontModel.Enabled)
					EnabledFontCaches.Add(fontCache);
			}
		}

		public static void Reset()
		{
			foreach (var fontCache in FontCaches)
				fontCache.Clear();

			if (Paint != null)
				Paint.Dispose();
			Paint = new SKPaint
			{
				StrokeWidth = 2,
				IsAntialias = Config.TextAntialias,
				FilterQuality = Config.TextFilterQuality
			};
		}

		public static void DrawText(SKCanvas canvas, string text, Font font, RectangleF rect, Color color)
		{
			DrawText(canvas, text, font, rect.X, rect.Y, color);
		}

		public static void DrawText(SKCanvas canvas, string text, Font font, float x, float y, Color color)
		{
			if (string.IsNullOrEmpty(text))
				return;

			Paint.Typeface = font.FontModel.Typeface;
			Paint.TextSize = font.Size;
			Paint.FakeBoldText = font.Style.HasFlag(FontStyle.Bold);
			Paint.TextSkewX = font.Style.HasFlag(FontStyle.Italic) ? -0.4f : 0;
			Paint.Color = DisplayUtils.ToSKColor(color);

			CharList = text.ToCharArray();
			CharRemain = CharList.Length;
			TextUnits = new TextUnit[CharRemain];
			UserCache = Paint.Typeface == null
				? FontCaches[0] : FontCaches.FirstOrDefault(item => item.FontModel.Typeface == Paint.Typeface);

			//------------------------------------

			List<FontCache> fontCache = new List<FontCache>(EnabledFontCaches);
			fontCache.Remove(UserCache);
			fontCache.Insert(0, UserCache);

			int count = fontCache.Count;
			for (int i = 0; i < count * 2; i++)
			{
				if (i < count)
					fontCache[i].CheckGlyphs();
				else
					fontCache[i - count].PrepareGlyphs();

				if (CharRemain == 0)
					break;
			}

			//没有任何字可以绘制的大失败情况
			if (CharRemain == CharList.Length)
				return;

			//------------------------------------

			for (int i = TextUnits.Length - 2; i >= 0; i--)
			{
				if (TextUnits[i + 1] == null)
					continue;

				if (TextUnits[i] == null)
				{
					TextUnits[i] = TextUnits[i + 1];
					TextUnits[i + 1] = null;
					continue;
				}

				if (TextUnits[i].FontModel != TextUnits[i + 1].FontModel)
					continue;

				TextUnits[i].Text += TextUnits[i + 1].Text;
				TextUnits[i].Advance += TextUnits[i + 1].Advance;
				TextUnits[i + 1] = null;
			}

			//------------------------------------

			bool drawStrikeout = font.Style.HasFlag(FontStyle.Strikeout);
			bool drawUnderline = font.Style.HasFlag(FontStyle.Underline);
			float Strikeout = drawStrikeout ? (Paint.FontMetrics.StrikeoutPosition ?? 0) : 0;
			float Underline = drawUnderline ? (Paint.FontMetrics.UnderlinePosition ?? 0) : 0;

			TextUnit unit;
			for (int i = 0; i < TextUnits.Length; i++)
			{
				if (TextUnits[i] == null)
					continue;

				unit = TextUnits[i];
				Paint.Typeface = unit.FontModel.Typeface;

				if (drawStrikeout)
					canvas.DrawLine(x, y + Strikeout, x + unit.Advance, y + Strikeout, Paint);
				if (drawUnderline)
					canvas.DrawLine(x, y + Underline, x + unit.Advance, y + Underline, Paint);

				canvas.DrawText(unit.Text, x, y, Paint);

				x += unit.Advance;
			}
		}

		private class FontCache
		{
			public FontModel FontModel;

			private readonly Dictionary<char, float> ValidChar = new Dictionary<char, float>();

			private readonly HashSet<char> InvalidChar = new HashSet<char>();

			public void CheckGlyphs()
			{
				if (ValidChar.Count == 0)
					return;

				for (int i = 0; i < CharList.Length; i++)
				{
					Char = CharList[i];
					if (Char == char.MinValue || InvalidChar.Contains(Char))
						continue;
					if (!ValidChar.TryGetValue(Char, out Advance))
						continue;

					AddTextUnits(i);
				}
			}

			public void PrepareGlyphs()
			{
				Paint.Typeface = FontModel.Typeface;
				var glyphs = Paint.GetGlyphs(CharList);
				var glyphWidths = Paint.GetGlyphWidths(CharList);

				for (int i = 0; i < glyphs.Length; i++)
				{
					Char = CharList[i];
					if (Char == char.MinValue)
						continue;

					if (glyphs[i] == 0)
					{
						if (glyphWidths[i] == 0 || FontModel != FontModel.Final)
						{
							InvalidChar.Add(Char);
							continue;
						}
					}

					Advance = glyphWidths[i];
					ValidChar[Char] = Advance;

					AddTextUnits(i);
				}
			}

			private void AddTextUnits(int i)
			{
				TextUnits[i] = new TextUnit
				{
					Text = Char.ToString(),
					Advance = Advance,
					FontModel = FontModel,
				};
				CharList[i] = char.MinValue;
				CharRemain--;
			}

			public void Clear()
			{
				ValidChar.Clear();
			}
		}

		private class TextUnit
		{
			public string Text;
			public float Advance;
			public FontModel FontModel;
		}
	}

	public enum FontStyle
	{
		Regular = 0,
		Bold = 1 << 0,
		Italic = 1 << 1,
		Strikeout = 1 << 2,
		Underline = 1 << 3
	}

	public enum GraphicsUnit
	{
		World,
		Display,
		Pixel,
		Point,
		Inch,
		Document,
		Millimeter
	}

	public class Font
	{
		public FontModel FontModel { get; }
		public float Size { get; }
		public FontStyle Style { get; }

		public Font(string fontName, float size, FontStyle style, GraphicsUnit unit)
		{
			FontModel = FontModel.GetFont(fontName);
			Size = size;
			Style = style;
		}
	}
}
