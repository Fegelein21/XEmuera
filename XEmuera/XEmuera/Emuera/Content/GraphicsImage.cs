using MinorShift._Library;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using XEmuera;
using XEmuera.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MinorShift.Emuera.Content
{
	internal sealed class GraphicsImage : AbstractImage
	{
		//public Bitmap Bitmap;
		//public IntPtr GDIhDC { get; protected set; }
		//protected Graphics g;
		//protected IntPtr hBitmap;
		//protected IntPtr hDefaultImg;

		public GraphicsImage(int id)
		{
			ID = id;
			g = null;
			Bitmap = null;
			//created = false;
			//locked = false;
		}
		public readonly int ID;
		Size size;
		Brush brush = null;
		Pen pen = null;
        #region EE_GDRAWTEXT
        Font font = null;
		//FontStyle style = default;
		#endregion
		
		//Bitmap b;
		//Graphics g;


		////bool created;
		////bool locked;
		//public void LockGraphics()
		//{
		//	//if (locked)
		//	//	return;
		//	//g = Graphics.FromImage(b);
		//	//locked = true;
		//}
		//public void UnlockGraphics()
		//{
		//	//if (!locked)
		//	//	return;
		//	//g.Dispose();
		//	//g = null;
		//	//locked = false;
		//}

		#region Bitmap書き込み・作成

		/// <summary>
		/// GCREATE(int ID, int width, int height)
		/// Graphicsの基礎となるBitmapを作成する。エラーチェックは呼び出し元でのみ行う
		/// </summary>
		public void GCreate(int x, int y, bool useGDI)
		{
			if (useGDI)
				throw new NotImplementedException();
			this.GDispose();
			//Bitmap = new Bitmap(x, y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			Bitmap = new SKBitmap(x, y);
			size = new Size(x, y);
			//g = SKCanvas.FromImage(Bitmap);
			g = new SKCanvas(Bitmap);
		}

		internal void GCreateFromF(SKBitmap bmp, bool useGDI)
		{
			if (useGDI)
				throw new NotImplementedException();
			this.GDispose();
			//Bitmap = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			Bitmap = new SKBitmap(bmp.Width, bmp.Height);
			size = new Size(bmp.Width, bmp.Height);

			//g = SKCanvas.FromImage(Bitmap);
			//g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);

			g = new SKCanvas(Bitmap);
			DrawBitmapUtils.DrawBitmap(g, bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
		}

		/// <summary>
		/// GCLEAR(int ID, int cARGB)
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		public void GClear(Color c)
		{
			if (g == null)
				throw new NullReferenceException();
			g.Clear(DisplayUtils.ToSKColor(c));
		}

		/// <summary>
		/// GDRAWTEXTGDRAWTEXT int ID, str text, int x, int y
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		#region EE_GDRAWTEXT 元のソースコードにあったものを改良
		public void GDrawString(string text, int x, int y)
		{
			if (g == null)
				throw new NullReferenceException();
			Font usingFont = font;
			if (usingFont == null)
				usingFont = new Font(Config.FontName, 100, GlobalStatic.Console.StringStyle.FontStyle, GraphicsUnit.Pixel);
			if (brush != null)
			{
				//g.DrawString(text, usingFont, brush, x, y);
				DrawTextUtils.DrawText(g, text, usingFont, x, y + font.Size, brush.DrawingColor); // add font.Size to be compatible with SKCanvas.DrawText
			}
			else
			{
				//using (SolidBrush b = new SolidBrush(Config.ForeColor))
				//	g.DrawString(text, usingFont, b, x, y);
				DrawTextUtils.DrawText(g, text, usingFont, x, y + font.Size, Config.ForeColor); // add font.Size to be compatible with SKCanvas.DrawText
			}
		}
		#endregion

		/// <summary>
		/// GDRAWTEXTGDRAWTEXT int ID, str text, int x, int y, int width, int height
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		// public void GDrawString(string text, int x, int y, int width, int height)
		// {
		// 	if (g == null)
		// 		throw new NullReferenceException();
		// 	Font usingFont = font;
		// 	if (usingFont == null)
		// 		usingFont = Config.Font;
		// 	if (brush != null)
		// 	{
		// 		g.DrawString(text, usingFont, brush, new RectangleF(x,y,width,height));
		// 	}
		// 	else
		// 	{
		// 		using (SolidBrush b = new SolidBrush(Config.ForeColor))
		// 			g.DrawString(text, usingFont, b, new RectangleF(x, y, width, height));
		// 	}
		// }

		/// <summary>
		/// GDRAWRECTANGLE(int ID, int x, int y, int width, int height)
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		public void GDrawRectangle(Rectangle rect)
		{
			if (g == null)
				throw new NullReferenceException();
			if (pen != null)
			{
				//g.DrawRectangle(pen, rect);
				DrawBitmapUtils.DrawRect(g, pen, rect);
			}
			else
			{
				using (Pen p = new Pen(Config.ForeColor))
				{
					//g.DrawRectangle(p, rect);
					DrawBitmapUtils.DrawRect(g, p, rect);
				}
			}
		}

		/// <summary>
		/// GFILLRECTANGLE(int ID, int x, int y, int width, int height)
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		public void GFillRectangle(Rectangle rect)
		{
			if (g == null)
				throw new NullReferenceException();
			if (brush != null)
			{
				//g.FillRectangle(brush, rect);
				DrawBitmapUtils.DrawRect(g, brush, rect);
			}
			else
			{
				using (SolidBrush b = new SolidBrush(Config.BackColor))
				{
					//g.FillRectangle(b, rect);
					DrawBitmapUtils.DrawRect(g, b, rect);
				}
			}
		}

		/// <summary>
		/// GDRAWCIMG(int ID, str imgName, int destX, int destY, int destWidth, int destHeight)
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		public void GDrawCImg(ASprite img, Rectangle destRect)
		{
			if (g == null)
				throw new NullReferenceException();
			img.GraphicsDraw(g, destRect);
		}

		/// <summary>
		/// GDRAWCIMG(int ID, str imgName, int destX, int destY, int destWidth, int destHeight, float[] cm)
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		public void GDrawCImg(ASprite img, Rectangle destRect, float[] cm)
		{
			if (g == null)
				throw new NullReferenceException();

			//ImageAttributes imageAttributes = new ImageAttributes();
			//ColorMatrix colorMatrix = new ColorMatrix(cm);
			//imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			SKColorFilter colorFilter = SKColorFilter.CreateColorMatrix(cm);

			//img.GraphicsDraw(g, destRect, imageAttributes);
			img.GraphicsDraw(g, destRect, colorFilter);
		}

		/// <summary>
		/// GDRAWG(int ID, int srcID, int destX, int destY, int destWidth, int destHeight, int srcX, int srcY, int srcWidth, int srcHeight)
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		public void GDrawG(GraphicsImage srcGra, Rectangle destRect, Rectangle srcRect)
		{
			if (g == null)
				throw new NullReferenceException();
			SKBitmap src = srcGra.GetBitmap();

			//g.DrawImage(src, destRect, srcRect, GraphicsUnit.Pixel);
			DrawBitmapUtils.DrawBitmap(g, src, srcRect, destRect);
		}


		/// <summary>
		/// GDRAWG(int ID, int srcID, int destX, int destY, int destWidth, int destHeight, int srcX, int srcY, int srcWidth, int srcHeight, float[] cm)
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		public void GDrawG(GraphicsImage srcGra, Rectangle destRect, Rectangle srcRect, float[] cm)
		{
			if (g == null)
				throw new NullReferenceException();
			SKBitmap src = srcGra.GetBitmap();

			//ImageAttributes imageAttributes = new ImageAttributes();
			//ColorMatrix colorMatrix = new ColorMatrix(cm);
			//imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default,ColorAdjustType.Bitmap);
			SKColorFilter colorFilter = SKColorFilter.CreateColorMatrix(cm);

			//g.DrawImage(img.Bitmap, destRect, srcRect, GraphicsUnit.Pixel, imageAttributes);なんでこのパターンないのさ
			//g.DrawImage(src, destRect, srcRect.Left, srcRect.Top, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, imageAttributes);
			DrawBitmapUtils.DrawBitmap(g, src, srcRect, destRect, colorFilter);
		}


		/// <summary>
		/// GDRAWGWITHMASK(int ID, int srcID, int maskID, int destX, int destY)
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		public void GDrawGWithMask(GraphicsImage srcGra, GraphicsImage maskGra, Point destPoint)
		{
			if (g == null)
				throw new NullReferenceException();
			SKBitmap destImg = this.GetBitmap();
			byte[] srcBytes = BytesFromBitmap(srcGra.GetBitmap());
			byte[] srcMaskBytes = BytesFromBitmap(maskGra.GetBitmap());
			//Rectangle destRect = new Rectangle(destPoint.X, destPoint.Y, srcGra.Width, srcGra.Height);

			//System.Drawing.Imaging.BitmapData bmpData =
			//	destImg.LockBits(new Rectangle(0,0, destImg.Width,destImg.Height),
			//	System.Drawing.Imaging.ImageLockMode.ReadWrite,
			//	PixelFormat.Format32bppArgb);
			try
			{
				//IntPtr ptr = bmpData.Scan0;
				//byte[] pixels = new byte[bmpData.Stride * destImg.Height];
				IntPtr ptr = destImg.GetPixels();
				byte[] pixels = new byte[destImg.Info.BytesSize];

				System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, pixels.Length);


				for (int y = 0; y < srcGra.Height; y++)
				{

					int destIndex = ((destPoint.Y + y) * destImg.Width + destPoint.X) * 4;
					int srcIndex = ((0 + y) * srcGra.Width + 0) * 4;
					for (int x = 0; x < srcGra.Width; x++)
					{
						if (srcMaskBytes[srcIndex] == 255)//完全不透明
						{
							pixels[destIndex++] = srcBytes[srcIndex++];
							pixels[destIndex++] = srcBytes[srcIndex++];
							pixels[destIndex++] = srcBytes[srcIndex++];
							pixels[destIndex++] = srcBytes[srcIndex++];
						}
						else if (srcMaskBytes[srcIndex] == 0)//完全透明
						{
							destIndex += 4;
							srcIndex += 4;
						}
						else//半透明 alpha/255ではなく（alpha+1）/256で計算しているがたぶん誤差
						{
							int mask = srcMaskBytes[srcIndex]; mask++;
							pixels[destIndex] = (byte)((srcBytes[srcIndex] * mask + pixels[destIndex] * (256 - mask)) >> 8); srcIndex++; destIndex++;
							pixels[destIndex] = (byte)((srcBytes[srcIndex] * mask + pixels[destIndex] * (256 - mask)) >> 8); srcIndex++; destIndex++;
							pixels[destIndex] = (byte)((srcBytes[srcIndex] * mask + pixels[destIndex] * (256 - mask)) >> 8); srcIndex++; destIndex++;
							pixels[destIndex] = (byte)((srcBytes[srcIndex] * mask + pixels[destIndex] * (256 - mask)) >> 8); srcIndex++; destIndex++;
						}
					}
				}

				// Bitmapへコピー
				System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, pixels.Length);
			}
			finally
			{
				//destImg.UnlockBits(bmpData);
			}
		}

		#region EE_GDRAWGWITHROTATE
		/// <summary>
		/// GROTATE(int ID, int angle, int x, int y)
		/// </summary>
		public void GRotate(Int64 a, int x, int y)
		{
		 	if (g == null)
		 		throw new NullReferenceException();
		 	float angle = a;

			g.RotateDegrees(angle, x, y);
			DrawBitmapUtils.DrawBitmap(g, Bitmap, new Rectangle(Bitmap.Width, Bitmap.Height, Bitmap.Width, Bitmap.Height));
		// 	g.TranslateTransform(-x, -y, System.Drawing.Drawing2D.MatrixOrder.Append);
		// 	g.RotateTransform(angle, System.Drawing.Drawing2D.MatrixOrder.Append);
		// 	g.TranslateTransform(x, y, System.Drawing.Drawing2D.MatrixOrder.Append);

		// 	g.DrawImageUnscaled(Bitmap, 0, 0);
		// 	//g.DrawImage(Bitmap, new Rectangle(Bitmap.Width, Bitmap.Height, Bitmap.Width, Bitmap.Height));
		}
		/// <summary>
		/// GDRAWGWITHROTATE
		/// </summary>
		public void GDrawGWithRotate(GraphicsImage srcGra, Int64 a, int x, int y)
		{
		 	if (g == null || srcGra == null)
		 		throw new NullReferenceException();
		 	float angle = a;
			g.RotateDegrees(angle, x, y);
			SKBitmap src = srcGra.GetBitmap();
			DrawBitmapUtils.DrawBitmap(g, src, new Rectangle(0, 0, src.Width, src.Height), new Rectangle(0, 0, Bitmap.Width, Bitmap.Height));
			// 	g.TranslateTransform(-x, -y, System.Drawing.Drawing2D.MatrixOrder.Append);
			// 	g.RotateTransform(angle, System.Drawing.Drawing2D.MatrixOrder.Append);
			// 	g.TranslateTransform(x, y, System.Drawing.Drawing2D.MatrixOrder.Append);
			// 	Bitmap src = srcGra.GetBitmap();
			// 	g.DrawImage(src, 0, 0);
		}
		#endregion

		#region EE_GDRAWTEXT フォントスタイルも指定できるように
		// public void GSetFont(Font r)
		public void GSetFont(Font r, FontStyle fs)
		{
			//if (font != null)
			//	font.Dispose();
			font = r;
			#region EE_GDRAWTEXT フォントスタイルも指定できるように
			font.SetStyle(fs);
			#endregion
		}
		#endregion
		public void GSetBrush(Brush r)
		{
			if (brush != null)
				brush.Dispose();
			brush = r;
		}
		public void GSetPen(Pen r)
		{
			if (pen != null)
				pen.Dispose();
			pen = r;
		}




		private static byte[] BytesFromBitmap(SKBitmap bmp)
		{
			//BitmapData bmpData = bmp.LockBits(
			//  new Rectangle(0, 0, bmp.Width, bmp.Height),
			//  ImageLockMode.ReadOnly,  // 書き込むときはReadAndWriteで
			//  PixelFormat.Format32bppArgb
			//);
			//if (bmpData.Stride < 0)
			//	throw new Exception();//変な形式のが送られてくることはありえないはずだが一応
			//byte[] pixels = new byte[bmpData.Stride * bmp.Height];
			//try
			//{ 
			//	IntPtr ptr = bmpData.Scan0;
			//	Marshal.Copy(ptr, pixels, 0, pixels.Length);
			//}
			//finally
			//{
			//	bmp.UnlockBits(bmpData);

			//}
			//return pixels;

			return bmp.Bytes;
		}

		/// <summary>
		/// GTOARRAY int ID, var array
		/// エラーチェックは呼び出し元でのみ行う
		/// <returns></returns>
		public bool GBitmapToInt64Array(Int64[,] array, int xstart, int ystart)
		{
		//	if (g == null || Bitmap == null)
		//		throw new NullReferenceException();
		//	int w = Bitmap.Width;
		//	int h = Bitmap.Height;
		//	if (xstart + w > array.GetLength(0) || ystart + h > array.GetLength(1))
		//		return false;
		//	Rectangle rect = new Rectangle(0, 0, w, h);
		//	System.Drawing.Imaging.BitmapData bmpData =
		//		Bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
		//		PixelFormat.Format32bppArgb);
		//	IntPtr ptr = bmpData.Scan0;
		//	byte[] rgbValues = new byte[w * h * 4];
		//	Marshal.Copy(ptr, rgbValues, 0, rgbValues.Length);
		//	Bitmap.UnlockBits(bmpData);
		//	int i = 0;
		//	for (int y = 0; y < h; y++)
		//	{
		//		for (int x = 0; x < w; x++)
		//		{
		//			array[x + xstart, y + ystart] =
		//			rgbValues[i++] + //B
		//			(((Int64)rgbValues[i++]) << 8) + //G
		//			(((Int64)rgbValues[i++]) << 16) + //R
		//			(((Int64)rgbValues[i++]) << 24);  //A
		//		}
		//	}
		//	return true;
			return false;
		}


		/// <summary>
		/// GFROMARRAY int ID, var array
		/// エラーチェックは呼び出し元でのみ行う
		/// <returns></returns>
		public bool GByteArrayToBitmap(Int64[,] array, int xstart, int ystart)
		{
		//	if (g == null || Bitmap == null)
		//		throw new NullReferenceException();
		//	int w = Bitmap.Width;
		//	int h = Bitmap.Height;
		//	if (xstart + w > array.GetLength(0) || ystart + h > array.GetLength(1))
		//		return false;

		//	byte[] rgbValues = new byte[w * h * 4];
		//	int i = 0;
		//	for (int y = 0; y < h; y++)
		//	{
		//		for (int x = 0; x < w; x++)
		//		{
		//			Int64 c = array[x + xstart, y + ystart];
		//			rgbValues[i++] = (byte)(c & 0xFF);//B
		//			rgbValues[i++] = (byte)((c >> 8) & 0xFF);//G
		//			rgbValues[i++] = (byte)((c >> 16) & 0xFF);//R
		//			rgbValues[i++] = (byte)((c >> 24) & 0xFF);//A
		//		}
		//	}
		//	Rectangle rect = new Rectangle(0, 0, w, h);
		//	System.Drawing.Imaging.BitmapData bmpData =
		//		Bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly,
		//		PixelFormat.Format32bppArgb);
		//	IntPtr ptr = bmpData.Scan0;
		//	Marshal.Copy(rgbValues, 0, ptr, rgbValues.Length);
		//	Bitmap.UnlockBits(bmpData);
		//	return true;
			return false;
		}
#endregion
#region Bitmap読み込み・削除
		/// <summary>
		/// 未作成ならエラー
		/// </summary>
		public SKBitmap GetBitmap()
		{
			if (Bitmap == null)
				throw new NullReferenceException();
			//UnlockGraphics();
			return Bitmap;
		}
		/// <summary>
		/// GSETCOLOR(int ID, int cARGB, int x, int y)
		/// エラーチェックは呼び出し元でのみ行う
		/// </summary>
		public void GSetColor(Color c, int x, int y)
		{
			if (Bitmap == null)
				throw new NullReferenceException();
			//UnlockGraphics();
			Bitmap.SetPixel(x, y, DisplayUtils.ToSKColor(c));
		}

		/// <summary>
		/// GGETCOLOR(int ID, int x, int y)
		/// エラーチェックは呼び出し元でのみ行う。特に画像範囲内であるかどうかチェックすること
		/// </summary>
		public Color GGetColor(int x, int y)
		{
			if (Bitmap == null)
				throw new NullReferenceException();
			//UnlockGraphics();
			return DisplayUtils.ToColor(Bitmap.GetPixel(x, y));
		}


		/// <summary>
		/// GDISPOSE(int ID)
		/// </summary>
		public void GDispose()
		{
			size = new Size(0, 0);
			if (Bitmap == null)
				return;
			//if (gdi)
			//{
			//	GDI.SelectObject(GDIhDC, hDefaultImg);
			//	GDI.DeleteObject(hBitmap);
			//	g.ReleaseHdc(GDIhDC);
			//}
			if (g != null)
				g.Dispose();
			if (Bitmap != null)
				Bitmap.Dispose();
			if (brush != null)
				brush.Dispose();
			if (pen != null)
				pen.Dispose();
			//if (font != null)
			//	font.Dispose();
			g = null;
			Bitmap = null;
			brush = null;
			pen = null;
			font = null;
		}

		public override void Dispose()
		{
			this.GDispose();
		}

        ~GraphicsImage()
        {
            Dispose();
        }
#endregion

#region 状態判定（Bitmap読み書きを伴わない）
		public override bool IsCreated { get { return g != null; } }
		/// <summary>
		/// int GWIDTH(int ID)
		/// </summary>
		public int Width { get { return size.Width; } }
		/// <summary>
		/// int GHEIGHT(int ID)
		/// </summary>
		public int Height { get { return size.Height; } }
        #region EE_GDRAWTEXTに付随する様々な要素
        public string Fontname { get { return font.FontModel.Name; } }
		public int Fontsize { get { return (int)font.Size; } }

		//public int Fontstyle
  //      {
  //          get
  //          {
		//		int ret = 0;
		//		if ((style & FontStyle.Bold) == FontStyle.Bold)
		//			ret |= 1;
		//		if ((style & FontStyle.Italic) == FontStyle.Italic)
		//			ret |= 2;
		//		if ((style & FontStyle.Strikeout) == FontStyle.Strikeout)
		//			ret |= 4;
		//		if ((style & FontStyle.Underline) == FontStyle.Underline)
		//			ret |= 8;
		//		return (ret);
		//	}
		//}

		public Font Fnt { get { return font; } }
		#endregion




#endregion


	}
}
