using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEmuera.Resources
{
	[ContentProperty(nameof(Source))]
	public class ImageResourceExtension : IMarkupExtension<ImageSource>
	{
		public string Source { get; set; }

		private static readonly Type Type = typeof(ImageResourceExtension);

		private static readonly Assembly Assembly = Type.GetTypeInfo().Assembly;

		public ImageSource ProvideValue(IServiceProvider serviceProvider)
		{
			if (string.IsNullOrEmpty(Source))
				return null;

			string resId = $"{Type.Namespace}.Images.{Source}";
			var stream = Assembly.GetManifestResourceStream(resId);
			if (stream == null)
				return null;

			if (Source.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
			{
				var picture = new SkiaSharp.Extended.Svg.SKSvg().Load(stream);
				var image = SKImage.FromPicture(picture, new SKSizeI(200, 200));

				return ImageSource.FromStream(() => image.Encode().AsStream());
			}

			return ImageSource.FromStream(() => stream);
		}

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		{
			return ProvideValue(serviceProvider);
		}
	}
}
