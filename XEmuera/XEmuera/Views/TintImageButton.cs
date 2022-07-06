using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace XEmuera.Views
{
	public class TintImageButton : ImageButton
	{
		public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image), typeof(string),
			typeof(TintImageButton), null, propertyChanged: RefreshImageSource);

		public static readonly BindableProperty ToggledImageProperty = BindableProperty.Create(nameof(ToggledImage), typeof(string),
			typeof(TintImageButton), null, propertyChanged: RefreshImageSource);

		public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled), typeof(bool?),
			typeof(TintImageButton), null, propertyChanged: (bindable, oldValue, newValue) =>
			{
				var button = bindable as TintImageButton;
				button.Toggled?.Invoke(bindable, new ToggledEventArgs((bool)newValue));
			});

		public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color),
			typeof(TintImageButton), Color.Transparent, propertyChanged: RefreshTintColor);

		public string Image
		{
			get { return (string)GetValue(ImageProperty); }
			set { SetValue(ImageProperty, value); }
		}

		public string ToggledImage
		{
			get { return (string)GetValue(ToggledImageProperty); }
			set { SetValue(ToggledImageProperty, value); }
		}

		public bool? IsToggled
		{
			get { return (bool?)GetValue(IsToggledProperty); }
			set { SetValue(IsToggledProperty, value); }
		}

		public Color TintColor
		{
			get => (Color)GetValue(TintColorProperty);
			set => SetValue(TintColorProperty, value);
		}

		public event EventHandler<ToggledEventArgs> Toggled;

		public TintImageButton() : base()
		{
			Clicked += (sender, args) =>
			{
				IsToggled = !IsToggled;
			};

			Toggled += (sender, args) => ToggleImageSource();
		}

		private void ToggleImageSource(bool forceRefresh = false)
		{
			if (string.IsNullOrEmpty(ToggledImage))
			{
				if (Source == null || forceRefresh)
					Source = CreateImageSource(Image);

				return;
			}

			if (IsToggled ?? false)
				Source = CreateImageSource(ToggledImage);
			else
				Source = CreateImageSource(Image);
		}

		private static void RefreshImageSource(BindableObject bindable, object oldvalue, object newvalue)
		{
			var button = bindable as TintImageButton;

			button.ToggleImageSource(true);
		}

		private ImageSource CreateImageSource(string source)
		{
			if (string.IsNullOrEmpty(source))
				return null;

			string resId = $"XEmuera.Resources.Images.{source}";
			var stream = GameUtils.GetManifestResourceStream(resId);
			if (stream == null)
				return null;

			if (source.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
			{
				var picture = new SkiaSharp.Extended.Svg.SKSvg().Load(stream);
				var image = SKImage.FromPicture(picture, new SKSizeI(200, 200));

				stream = image.Encode().AsStream();
			}

			if (TintColor != Color.Transparent)
				stream = DrawTintColor(stream, TintColor);

			return ImageSource.FromStream(() => stream);
		}

		private static void RefreshTintColor(BindableObject bindable, object oldvalue, object newvalue)
		{
			var button = bindable as TintImageButton;

			button.ToggleImageSource(true);
		}

		private static Stream DrawTintColor(Stream stream, Color tintColor)
		{
			var bitmap = SKBitmap.Decode(stream);
			var image = SKImage.FromBitmap(bitmap);

			using SKPaint paint = new SKPaint
			{
				ColorFilter = SKColorFilter.CreateBlendMode(DisplayUtils.ToSKColor(tintColor), SKBlendMode.SrcIn),
			};

			using (var canvas = new SKCanvas(bitmap))
			{
				canvas.Clear();
				canvas.DrawImage(image, SKPoint.Empty, paint);
			}

			var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
			return data.AsStream();
		}
	}
}
