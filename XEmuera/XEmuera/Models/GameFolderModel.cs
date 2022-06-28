using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XEmuera.Models
{
	/// <summary>
	/// 玩家自行设置的游戏目录
	/// </summary>
	public class GameFolderModel
	{
		public const string PrefKeyGameFolder = nameof(PrefKeyGameFolder);

		private const string separator = "_|_";

		public static readonly ObservableCollection<GameFolderModel> AllModels = new ObservableCollection<GameFolderModel>();

		public static Action GameFolderChanged;

		public string Name { get; set; }
		public string Path { get; set; }

		public static void Load()
		{
			string paths = GameUtils.GetPreferences(PrefKeyGameFolder, null);
			List<string> lists = new List<string>();

			if (paths == null)
				lists.Add("/storage/emulated/0/emuera");
			else
				lists.AddRange(paths.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries));

			lists.Sort();

			AllModels.Clear();

			foreach (var path in lists)
				Add(path);
		}

		public static void Save()
		{
			GameUtils.SetPreferences(PrefKeyGameFolder, string.Join(separator, AllModels.Select(item => item.Path)));
		}

		public static void Add(string path)
		{

			if (string.IsNullOrEmpty(path))
				return;

			if (AllModels.Any(item => item.Path == path))
				return;

			if (!GameUtils.RequestExternalAccess())
				return;

			GameFolderModel gameDirItem = new GameFolderModel()
			{
				Name = System.IO.Path.GetFileName(path),
				Path = path
			};

			AllModels.Add(gameDirItem);
		}

		public static void Remove(GameFolderModel gameFolderModel)
		{
			AllModels.Remove(gameFolderModel);
		}



	}
}
