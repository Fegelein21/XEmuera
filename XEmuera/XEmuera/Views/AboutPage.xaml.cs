using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEmuera.Resources;
using Xamarin.CommunityToolkit.Extensions;

namespace XEmuera.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
		public const string GithubUrl = "https://github.com/Fegelein21/XEmuera";
		public const string GiteeUrl = "https://gitee.com/fegelein21/XEmuera";
		public const string AiFaDianUrl = "https://afdian.net/@fegelein21";

		private static List<AboutModelGroup> AboutList;

		public string AppName { get; private set; } = AppInfo.Name;

		public string Version { get; private set; } = AppInfo.VersionString;

		public AboutPage()
		{
			InitializeComponent();

			BindingContext = this;

			InitList();

			AboutListView.ItemsSource = AboutList;
		}

		private static void InitList()
		{
			AboutList = new List<AboutModelGroup>
			{
				new AboutModelGroup(StringsText.ProjectWebSite)
				{
					new AboutModel
					{
						Text = "Github",
						Detail = GithubUrl,
						Url = GithubUrl
					},
					new AboutModel
					{
						Text = "Gitee",
						Detail = GiteeUrl,
						Url = GiteeUrl
					},
				},
				new AboutModelGroup("Emuera")
				{
					new AboutModel
					{
						Text = StringsText.Version,
						Detail = "Emuera1824+v15 私家改造版",
						Url = "https://ux.getuploader.com/ninnohito/index"
					},
					new AboutModel
					{
						Text = StringsText.Document,
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
				new AboutModelGroup(StringsText.CodeManifest)
				{
					new AboutModel
					{
						Text = StringsText.OpenSourceCodeManifest,
						File = "OpenSourceCodeManifest",
					},
				},
				new AboutModelGroup(StringsText.SupportMe)
				{
					new AboutModel
					{
						Text = "爱发电",
						Detail = AiFaDianUrl,
						Url = AiFaDianUrl
					},
				},
			};
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
				var page = new TxtFilePage($"XEmuera.Resources.Docs.{model.File}.txt")
				{
					Title = model.File
				};
				await Navigation.PushAsync(page);
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

		int AvatarClick;

		private void AvatarImageButton_Clicked(object sender, EventArgs e)
		{
			AvatarClick++;

			if (AvatarClick >= 10)
				GameUtils.MainPage.DisplayToastAsync("你成为了一名Super面筋人！");
			else if (AvatarClick > 1)
				GameUtils.MainPage.DisplayToastAsync($"还有{10 - AvatarClick}步成为Super面筋人！");
		}
	}
}