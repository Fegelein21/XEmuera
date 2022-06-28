using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
		public const string AiFaDianUrl = "https://afdian.net/@fegelein21";

		public string AppName { get; private set; } = AppInfo.Name;

		public string Version { get; private set; } = AppInfo.VersionString;

		public AboutPage()
		{
			InitializeComponent();

			BindingContext = this;

			List<AboutModelGroup> group = new List<AboutModelGroup>
			{
				new AboutModelGroup("项目地址")
				{
					new AboutModel
					{
						Text = "Github",
						Detail = "首发地址",
						Url = "https://github.com/Fegelein21/XEmuera"
					},
					new AboutModel
					{
						Text = "Gitee",
						Detail = "国内中转",
						Url = "https://gitee.com/fegelein21/XEmuera"
					},
				},
				new AboutModelGroup("Emuera")
				{
					new AboutModel
					{
						Text = "内核版本",
						Detail = "Emuera1824+v15 私家改造版",
						Url = "https://ux.getuploader.com/ninnohito/index"
					},
					new AboutModel
					{
						Text = "参考文档",
						Detail = "EmueraWiki",
						Url = "http://osdn.jp/projects/emuera/wiki/FrontPage"
					},
					new AboutModel
					{
						Text = "Emuera_readme",
						File = "Emuera_readme",
					},
					new AboutModel
					{
						Text = "私家改造版Emuera_readme",
						File = "私家改造版Emuera_readme",
					},
				},
				new AboutModelGroup("代码清单")
				{
					new AboutModel
					{
						Text = "开源代码使用清单",
						File = "开源代码使用清单",
					},
				},
				new AboutModelGroup("支持作者")
				{
					new AboutModel
					{
						Text = "爱发电主页",
						Detail = AiFaDianUrl,
						Url = AiFaDianUrl
					},
				},
			};

			AboutListView.ItemsSource = group;
		}

		private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (!(e.SelectedItem is AboutModel model))
				return;

			AboutListView.SelectedItem = null;

			if (!string.IsNullOrEmpty(model.Url))
			{
				await Launcher.OpenAsync(model.Url);
			}
			else if (!string.IsNullOrEmpty(model.File))
			{
				string resource = $"XEmuera.Resources.{model.File}.txt";
				await Navigation.PushAsync(new TxtFilePage(model.File, resource));
			}
		}

		public class AboutModelGroup : List<AboutModel>
		{
			public string GroupTitle { get; }
			public AboutModelGroup(string groupTitle)
			{
				GroupTitle = groupTitle;
			}
		}

		public class AboutModel
		{
			public string Text { get; set; }
			public string Detail { get; set; }
			public string Url { get; set; }
			public string File { get; set; }
		}
	}
}