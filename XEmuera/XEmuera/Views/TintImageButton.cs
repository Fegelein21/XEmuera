using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XEmuera.Views
{
	public class TintImageButton : ImageButton
	{
		public static readonly BindableProperty TintColorProperty = BindableProperty.Create(
			nameof(TintColor), typeof(Color), typeof(TintImageButton), Color.Transparent, propertyChanged: RedrawCanvas);

		public Color TintColor
		{
			get => (Color)GetValue(TintColorProperty);
			set => SetValue(TintColorProperty, value);
		}

		private static async void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
		{
			if (!(bindable is TintImageButton button))
				return;

			if (button.Source == null)
				return;

			var source = button.Source as StreamImageSource;
			var stream = await source.Stream(default);

			var bitmap = SKBitmap.Decode(stream);
			var image = SKImage.FromBitmap(bitmap);

			using SKPaint paint = new SKPaint
			{
				ColorFilter = SKColorFilter.CreateBlendMode(DisplayUtils.ToSKColor(button.TintColor), SKBlendMode.SrcIn),
			};

			using (var canvas = new SKCanvas(bitmap))
			{
				canvas.Clear();
				canvas.DrawImage(image, SKPoint.Empty, paint);
			}

			var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
			button.Source = ImageSource.FromStream(() => data.AsStream());
		}
	}
}
