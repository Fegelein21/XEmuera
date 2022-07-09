using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XEmuera.Models
{
	public class GameFolderModel
	{
		public static GameFolderModel Instance { get; private set; }

		public string Name { get; set; }
		public string Path { get; set; }

		public static void Load()
		{
			string path = GameUtils.PlatformService.GetStoragePath() + System.IO.Path.DirectorySeparatorChar + "emuera";

			Instance = new GameFolderModel()
			{
				Name = System.IO.Path.GetFileName(path),
				Path = path
			};
		}
	}
}
