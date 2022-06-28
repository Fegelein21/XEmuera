using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XEmuera.Droid.Renderer;
using XEmuera.Views;

[assembly: ExportRenderer(typeof(SelectableLabel), typeof(SelectableLabelRenderer))]
namespace XEmuera.Droid.Renderer
{
	public class SelectableLabelRenderer : ViewRenderer<SelectableLabel, TextView>
	{
		TextView textView;

		public SelectableLabelRenderer(Context context) : base(context)
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<SelectableLabel> e)
		{
			base.OnElementChanged(e);

			var label = Element;
			if (label == null)
				return;

			if (Control == null)
			{
				textView = new TextView(this.Context);
			}

			textView.Enabled = true;
			textView.Focusable = true;
			textView.LongClickable = true;
			textView.SetTextIsSelectable(true);

			// Initial properties Set
			textView.Text = label.Text;
			textView.SetTextColor(label.TextColor.ToAndroid());
			switch (label.FontAttributes)
			{
				case FontAttributes.None:
					textView.SetTypeface(null, Android.Graphics.TypefaceStyle.Normal);
					break;
				case FontAttributes.Bold:
					textView.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);
					break;
				case FontAttributes.Italic:
					textView.SetTypeface(null, Android.Graphics.TypefaceStyle.Italic);
					break;
				default:
					textView.SetTypeface(null, Android.Graphics.TypefaceStyle.Normal);
					break;
			}

			textView.TextSize = (float)label.FontSize;

			SetNativeControl(textView);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == SelectableLabel.TextProperty.PropertyName)
			{
				if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
				{
					textView.Text = Element.Text;
				}
			}
			else if (e.PropertyName == SelectableLabel.TextColorProperty.PropertyName)
			{
				if (Control != null && Element != null)
				{
					textView.SetTextColor(Element.TextColor.ToAndroid());
				}
			}
			else if (e.PropertyName == SelectableLabel.FontAttributesProperty.PropertyName
						|| e.PropertyName == SelectableLabel.FontSizeProperty.PropertyName)
			{
				if (Control != null && Element != null)
				{
					switch (Element.FontAttributes)
					{
						case FontAttributes.None:
							textView.SetTypeface(null, Android.Graphics.TypefaceStyle.Normal);
							break;
						case FontAttributes.Bold:
							textView.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);
							break;
						case FontAttributes.Italic:
							textView.SetTypeface(null, Android.Graphics.TypefaceStyle.Italic);
							break;
						default:
							textView.SetTypeface(null, Android.Graphics.TypefaceStyle.Normal);
							break;
					}

					textView.TextSize = (float)Element.FontSize;
				}
			}
		}
	}
}