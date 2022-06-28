using MinorShift._Library;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MinorShift.Emuera.Content
{
	internal abstract class AbstractImage : AContentFile
	{
		public const int MAX_IMAGESIZE = 8192;
		public SKBitmap Bitmap;
		//public IntPtr GDIhDC { get; protected set; }
		protected SKCanvas g;
		//protected IntPtr hBitmap;
		//protected IntPtr hDefaultImg;
		//protected bool gdi;
	}

	internal sealed class ConstImage : AbstractImage
	{
		public ConstImage(string name)
		{ Name = name; }


		public readonly string Name;

		internal void CreateFrom(SKBitmap bmp, bool useGDI)
		{
			if (Bitmap != null)
				throw new Exception();
			try
			{
				Bitmap = bmp;
				//if (useGDI)
				//{
				//	gdi = true;
				//	hBitmap = Bitmap.GetHbitmap();
				//	g = Graphics.FromImage(Bitmap);
				//	GDIhDC = g.GetHdc();
				//	hDefaultImg = GDI.SelectObject(GDIhDC, hBitmap);
				//}
			}
			catch
			{
				return;
			}
			return;
		}
		//public void Load(bool useGDI)
		//{
		//	if (Loaded)
		//		return;
		//	try
		//	{
		//		Bitmap = new Bitmap(Filepath);
		//		if (useGDI)
		//		{
		//			hBitmap = Bitmap.GetHbitmap();
		//			g = Graphics.FromImage(Bitmap);
		//			GDIhDC = g.GetHdc();
		//			hDefaultImg = GDI.SelectObject(GDIhDC, hBitmap);
		//		}
		//		Loaded = true;
		//		Enabled = true;
		//	}
		//	catch
		//	{
		//		return;
		//	}
		//	return;
		//}

		public override void Dispose()
		{
			if (Bitmap == null)
				return;
			//if (gdi)
			//{
			//	GDI.SelectObject(GDIhDC, hDefaultImg);
			//	GDI.DeleteObject(hBitmap);
			//	//gがすでに死んでると例外になる
			//	if(g != null)
			//	g.ReleaseHdc(GDIhDC);
			//}
			if (g != null)
			{
				g.Dispose();
				g = null;
			}
			if (Bitmap != null)
			{
				Bitmap.Dispose();
				Bitmap = null;
			}
		}

        ~ConstImage()
        {
            Dispose();
        }


		public override bool IsCreated
		{
			get { return Bitmap != null; }
		}
	}
}
