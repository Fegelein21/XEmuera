using Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XEmuera.iOS.Renderer;
using XEmuera.Views;

[assembly: ExportRenderer(typeof(SelectableLabel), typeof(SelectableLabelRenderer))]
namespace XEmuera.iOS.Renderer
{
	public class SelectableLabelRenderer : ViewRenderer<SelectableLabel, UITextView>
	{
		UITextView uiTextView;

		protected override void OnElementChanged(ElementChangedEventArgs<SelectableLabel> e)
		{
			base.OnElementChanged(e);

			var label = Element;
			if (label == null)
				return;

			if (Control == null)
			{
				uiTextView = new UITextView();
			}

			uiTextView.Selectable = true;
			uiTextView.Editable = false;
			uiTextView.ScrollEnabled = false;
			uiTextView.TextContainerInset = UIEdgeInsets.Zero;
			uiTextView.TextContainer.LineFragmentPadding = 0;
			uiTextView.BackgroundColor = UIColor.Clear;

			// Initial properties Set
			uiTextView.Text = label.Text;
			uiTextView.TextColor = label.TextColor.ToUIColor();
			switch (label.FontAttributes)
			{
				case FontAttributes.None:
					uiTextView.Font = UIFont.SystemFontOfSize(new nfloat(label.FontSize));
					break;
				case FontAttributes.Bold:
					uiTextView.Font = UIFont.BoldSystemFontOfSize(new nfloat(label.FontSize));
					break;
				case FontAttributes.Italic:
					uiTextView.Font = UIFont.ItalicSystemFontOfSize(new nfloat(label.FontSize));
					break;
				default:
					uiTextView.Font = UIFont.BoldSystemFontOfSize(new nfloat(label.FontSize));
					break;
			}

			SetNativeControl(uiTextView);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == SelectableLabel.TextProperty.PropertyName)
			{
				if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
				{
					uiTextView.Text = Element.Text;
				}
			}
			else if (e.PropertyName == SelectableLabel.TextColorProperty.PropertyName)
			{
				if (Control != null && Element != null)
				{
					uiTextView.TextColor = Element.TextColor.ToUIColor();
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
							uiTextView.Font = UIFont.SystemFontOfSize(new nfloat(Element.FontSize));
							break;
						case FontAttributes.Bold:
							uiTextView.Font = UIFont.BoldSystemFontOfSize(new nfloat(Element.FontSize));
							break;
						case FontAttributes.Italic:
							uiTextView.Font = UIFont.ItalicSystemFontOfSize(new nfloat(Element.FontSize));
							break;
						default:
							uiTextView.Font = UIFont.BoldSystemFontOfSize(new nfloat(Element.FontSize));
							break;
					}
				}
			}
		}
	}
}