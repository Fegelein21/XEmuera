using System;
using System.Collections.Generic;
using System.Text;
using XEmuera.Forms;
//using System.Drawing;
using SkiaSharp.Views.Forms;

namespace MinorShift.Emuera.Forms
{
	public sealed class EraPictureBox : SKCanvasView
	{
		//public EraPictureBox()
		//{
		//	//1819 なんとなくわかった
		//	//DoubleBufferedを使いたければPaintイベントかOnPaintが渡すe.Graphicsを使用しなければならないらしい
		//	//全てをOnPaintにやらせてRefreshで更新するシステムが正しい？
		//	//↓はつまりCreateGraphics()を使ってたせい
		//	////紆余曲折の残骸。
		//	////ConfigのUseImageBufferがfalseの場合、
		//	////OptimizedDoubleBufferをtrueにするとリサイズした時に再描画がうまくいかなかった。
		//	////Opaqueをtrue、OptimizedDoubleBufferをfalseにするとゴミが残る。
		//	////OnPaintBackgroundを上書きしてもゴミが残る。
		//	////this.SetStyle(ControlStyles.Opaque, true);
		//	////this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		//	////this.SetStyle(ControlStyles.UserPaint, true);
		//	////this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		//	//////this.DoubleBuffered = true;
		//	////this.SetStyle(ControlStyles.ResizeRedraw, true);
		//}

		//public void SetStyle()
		//{
		//	//if (StaticConfig.UseImageBuffer)
		//	//{
		//	//    this.SetStyle(ControlStyles.Opaque, true);
		//	//    this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		//	//    this.SetStyle(ControlStyles.UserPaint, true);
		//	//    this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		//	//    this.SetStyle(ControlStyles.ResizeRedraw, false);
		//	//}
		//	//else
		//	//{
		//	//    this.SetStyle(ControlStyles.Opaque, false);
		//	//    this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		//	//    this.SetStyle(ControlStyles.UserPaint, true);
		//	//    this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
		//	//    this.SetStyle(ControlStyles.ResizeRedraw, false);
		//	//}
		//	//背景描画カット
		//	this.SetStyle(ControlStyles.Opaque, true);
		//	//以下3つでダブルバッファリング
		//	//ただしOnPaintかPaintイベントのe.Graphicsを使用する場合のみ
		//	this.SetStyle(ControlStyles.UserPaint, true);
		//	this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		//	this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		//	//リサイズ時に自動再描画
		//	this.SetStyle(ControlStyles.ResizeRedraw, true);
		//}

		//protected override void OnPaintBackground(PaintEventArgs pevent)
		//{
		//	//SetStyle(ControlStyles.Opaque, true);だとそもそもここにはこない
		//	//SetStyleを適切にやれば普通のPictureBoxでよかった
		//	//base.OnPaintBackground(pevent);
		//}

	}
}
