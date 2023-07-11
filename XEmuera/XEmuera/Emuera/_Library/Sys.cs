using System;
using System.Collections.Generic;
using System.Text;
using XEmuera.Forms;
using System.IO;
using XEmuera;

namespace MinorShift._Library
{
	public static class Sys
	{
		public static void Init()
		{
			//ExePath = Application.ExecutablePath;
			ExePath = GameUtils.CurrentGamePath + Path.DirectorySeparatorChar;
			//ExeDir = Path.GetDirectoryName(ExePath) + "\\";
			ExeDir = Path.GetDirectoryName(ExePath) + Path.DirectorySeparatorChar;
			ExeName = Path.GetFileName(ExePath);
		}

		/// <summary>
		/// 実行ファイルのパス
		/// </summary>
		public static string ExePath { get; private set; }

		/// <summary>
		/// 実行ファイルのディレクトリ。最後に\を付けたstring
		/// </summary>
		public static string ExeDir { get; private set; }

		/// <summary>
		/// 実行ファイルの名前。ディレクトリなし
		/// </summary>
		public static string ExeName { get; private set; }

		/// <summary>
		/// 2重起動防止。既に同名exeが実行されているならばtrueを返す
		/// </summary>
		/// <returns></returns>
		public static bool PrevInstance()
		{
			//string thisProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
			//if (System.Diagnostics.Process.GetProcessesByName(thisProcessName).Length > 1)
			//{
			//	return true;
			//}
			return false;

		}
	}
}

