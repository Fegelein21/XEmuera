using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TxtFilePage : ContentPage
	{
		public TxtFilePage(string resource)
		{
			InitializeComponent();

			BindingContext = this;

			var stream = GameUtils.GetManifestResourceStream(resource);
			using (StreamReader streamReader = new StreamReader(stream))
			{
				TextView.Text = streamReader.ReadToEnd();
			}
		}
	}
}