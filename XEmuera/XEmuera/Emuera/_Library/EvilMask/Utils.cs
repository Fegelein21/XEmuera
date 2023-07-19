using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.Sub;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace EvilMask.Emuera
{
	internal sealed class Utils
	{
		static Stopwatch stopwatch = new Stopwatch();
		static Int64 stopwatch_base = DateTime.Now.Ticks;
		public static Int64 TimePoint()
		{
			if (!stopwatch.IsRunning) stopwatch.Start();
			return stopwatch_base + stopwatch.Elapsed.Ticks;
		}
		public static string GetValidPath(string path)
		{
			path = path.Replace('\\', '/').Replace("../", "").Replace("./", "");
			path = MinorShift.Emuera.Program.ExeDir + path;
			try
			{ 
				if (Path.GetPathRoot(path) != string.Empty)
					return null;
			}
			catch
			{
				return null;
			}
			return path;
		}

		// filepathの安全性(ゲームフォルダ以外のフォルダか)を確認しない
		// static public Bitmap LoadImage(string filepath)
		// {
		// 	Bitmap bmp = null;
		// 	FileStream fs = null;
		// 	if (!File.Exists(filepath)) return null;

		// 	try
		// 	{
		// 		fs = new FileStream(filepath, FileMode.Open);
		// 		var factory = new ImageProcessor.ImageFactory();
		// 		factory.Load(fs);
		// 		bmp = (Bitmap)factory.Image;
		// 	}
		// 	catch { }
		// 	finally
		// 	{
		// 		fs?.Close();
		// 		fs?.Dispose();
		// 	}
		// 	return bmp;

		// }
		// ビットマップファイルからアイコンファイルをつくる
		// public static Icon MakeIconFromBmpFile(Bitmap bmp)
		// {
		// 	Image img = bmp;
 
		// 	Bitmap bitmap = new Bitmap(256, 256, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		// 	Graphics g = Graphics.FromImage(bitmap);
		// 	g.DrawImage(img, new Rectangle(0, 0, 256, 256));
		// 	g.Dispose();
 
		// 	Icon icon = Icon.FromHandle(bitmap.GetHicon());
 
		// 	img.Dispose();
		// 	bitmap.Dispose();
		// 	return icon;
		// }

		public sealed class DataTable
		{
			static Type[] builtInDTTypes = { typeof(sbyte), typeof(Int16), typeof(Int32), typeof(Int64), typeof(string) };
			static Dictionary<Type, string> builtInDictDTTypeNames = new Dictionary<Type, string>{
				{ typeof(sbyte), "int8" },
				{ typeof(Int16), "int16" },
				{ typeof(Int32), "int32" },
				{ typeof(Int64), "int64" },
				{ typeof(string), "string" },
			};
			static Dictionary<string, Type> builtInDictDTTypeNames_R = new Dictionary<string, Type>{
				{ "int8", typeof(sbyte) },
				{ "int16", typeof(Int16) },
				{ "int32", typeof(Int32) },
				{ "int64", typeof(Int64) },
				{ "string", typeof(string) },
			};
			public static Int64 TypeToInt(Type t)
			{
				if (t == typeof(sbyte)) return 1;
				if (t == typeof(Int16)) return 2;
				if (t == typeof(Int32)) return 3;
				if (t == typeof(Int64)) return 4;
				if (t == typeof(string)) return 5;
				return Int64.MaxValue;
			}
			public static Type IntToType(Int64 i)
			{
				if (i > 0 && i <= builtInDTTypes.Length) return builtInDTTypes[i - 1];
				return null;
			}
			public static Type NameToType(string n)
			{
				if (builtInDictDTTypeNames_R.ContainsKey(n)) return builtInDictDTTypeNames_R[n];
				return null;
			}
			public static string TypeToName(Type t)
			{
				if (builtInDictDTTypeNames.ContainsKey(t)) return builtInDictDTTypeNames[t];
				return null;
			}
			public static object ConvertInt(Int64 v, Type t)
			{
				if (t == typeof(sbyte)) return (sbyte)Math.Min(Math.Max(v, sbyte.MinValue), sbyte.MaxValue);
				if (t == typeof(Int16)) return (Int16)Math.Min(Math.Max(v, Int16.MinValue), Int16.MaxValue);
				if (t == typeof(Int32)) return (Int32)Math.Min(Math.Max(v, Int32.MinValue), Int32.MaxValue);
				return v;
			}
		}
	}
}
