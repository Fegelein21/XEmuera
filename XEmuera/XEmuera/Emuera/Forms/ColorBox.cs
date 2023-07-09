using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MinorShift.Emuera.Forms
{
	public partial class ColorBox : UserControl
	{
		public ColorBox()
		{
			InitializeComponent();
		}
		public Color SelectingColor
		{
			get { return pictureBox1.BackColor; }
			set { pictureBox1.BackColor = value; }
		}
		public string ButtonText
		{
			get { return button.Text; }
			set { button.Text = value; }
		}

		private void button_Click(object sender, EventArgs e)
		{
			colorDialog.Color = pictureBox1.BackColor;
			if (colorDialog.ShowDialog() == DialogResult.OK)
				pictureBox1.BackColor = colorDialog.Color;
		}
	}
}
