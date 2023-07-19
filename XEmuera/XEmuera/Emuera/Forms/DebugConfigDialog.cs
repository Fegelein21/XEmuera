// ﻿using System;
// using System.Collections.Generic;
// using System.ComponentModel;
// using System.Data;
// using System.Drawing;
// using System.Text;
// using System.Windows.Forms;
// using System.Drawing.Text;
// using EvilMask.Emuera;

//namespace MinorShift.Emuera.Forms
//{

//	internal partial class DebugConfigDialog : Form
//	{
//		public DebugConfigDialog()
//		{
//			InitializeComponent();

//			numericUpDownDWW.Maximum = 10000;
//			numericUpDownDWH.Maximum = 10000;
//			numericUpDownDWX.Maximum = 10000;
//			numericUpDownDWY.Maximum = 10000;
//		}

		// public void TranslateUI()
        // {
		// 	this.Text = Lang.UI.DebugConfigDialog.Text;
		// 	this.tabPageDebug3.Text = Lang.UI.DebugConfigDialog.Name.Text;
		// 	this.label29.Text = Lang.UI.DebugConfigDialog.Warning.Text;
		// 	this.checkBoxShowDW.Text = Lang.UI.DebugConfigDialog.OpenDebugWindowOnStartup.Text;
		// 	this.checkBoxDWTM.Text = Lang.UI.DebugConfigDialog.AlwaysOnTop.Text;
		// 	this.label28.Text = Lang.UI.DebugConfigDialog.WindowWidth.Text;
		// 	this.label27.Text = Lang.UI.DebugConfigDialog.WindowHeight.Text;
		// 	this.button6.Text = Lang.UI.ConfigDialog.Window.GetWindowSize.Text;
		// 	this.checkBoxSetDWPos.Text = Lang.UI.DebugConfigDialog.SetWindowPos.Text;
		// 	this.label26.Text = Lang.UI.DebugConfigDialog.WindowX.Text;
		// 	this.label25.Text = Lang.UI.DebugConfigDialog.WindowY.Text;
		// 	this.button5.Text = Lang.UI.ConfigDialog.Window.GetWindowPos.Text;
		// 	this.label16.Text = Lang.UI.ConfigDialog.ChangeWontTakeEffectUntilRestart.Text;
		// 	this.buttonSave.Text = Lang.UI.ConfigDialog.Save.Text;
		// 	this.buttonCancel.Text = Lang.UI.ConfigDialog.Cancel.Text;


		// 	var diff = tabControl.Size - tabControl.DisplayRectangle.Size + ((Size)tabControl.Padding);
		// 	Size size;
		// 	{
		// 		var page = flowLayoutPanel5;
		// 		page.Dock = DockStyle.None;
		// 		size = page.Size + page.Margin.Size;
		// 		page.Dock = DockStyle.Fill;
		// 	}
		// 	tabControl.Size = size + diff;
		// }

		// private void buttonSave_Click(object sender, EventArgs e)
		// {
		// 	SaveConfig();
		// 	Result = ConfigDialogResult.Save;
		// 	this.Close();
		// }

//		private void buttonCancel_Click(object sender, EventArgs e)
//		{
//			Result = ConfigDialogResult.Cancel;
//			this.Close();
//		}
//		public ConfigDialogResult Result = ConfigDialogResult.Cancel;

//		void setCheckBox(CheckBox checkbox, ConfigCode code)
//		{
//			ConfigItem<bool> item = (ConfigItem<bool>)ConfigData.Instance.GetDebugItem(code);
//			checkbox.Checked = item.Value;
//			checkbox.Enabled = !item.Fixed;
//		}
//		void setNumericUpDown(NumericUpDown updown, ConfigCode code)
//		{
//			ConfigItem<int> item = (ConfigItem<int>)ConfigData.Instance.GetDebugItem(code);
//			decimal value = item.Value;
//			if (updown.Maximum < value)
//				updown.Maximum = value;
//			if (updown.Minimum > value)
//				updown.Minimum = value;
//			updown.Value = value;
//			updown.Enabled = !item.Fixed;
//		}

//		void setColorBox(ColorBox colorBox, ConfigCode code)
//		{
//			ConfigItem<Color> item = (ConfigItem<Color>)ConfigData.Instance.GetDebugItem(code);
//			colorBox.SelectingColor = item.Value;
//			colorBox.Enabled = !item.Fixed;
//		}
//		void setTextBox(TextBox textBox, ConfigCode code)
//		{
//			ConfigItem<string> item = (ConfigItem<string>)ConfigData.Instance.GetDebugItem(code);
//			textBox.Text = item.Value;
//			textBox.Enabled = !item.Fixed;
//		}

//		public void SetConfig(DebugDialog debugDialog)
//		{
//			dd = debugDialog;
//			//ConfigData config = ConfigData.Instance;

//			setCheckBox(checkBoxShowDW, ConfigCode.DebugShowWindow);
//			setCheckBox(checkBoxDWTM, ConfigCode.DebugWindowTopMost);
//			setCheckBox(checkBoxSetDWPos, ConfigCode.DebugSetWindowPos);
//			setNumericUpDown(numericUpDownDWW, ConfigCode.DebugWindowWidth);
//			setNumericUpDown(numericUpDownDWH, ConfigCode.DebugWindowHeight);
//			setNumericUpDown(numericUpDownDWX, ConfigCode.DebugWindowPosX);
//			setNumericUpDown(numericUpDownDWY, ConfigCode.DebugWindowPosY);
//		}

//		private void SaveConfig()
//		{

//			//ConfigData config = ConfigData.Instance.Copy();
//			ConfigData config = ConfigData.Instance;
//			config.GetDebugItem(ConfigCode.DebugShowWindow).SetValue<bool>(checkBoxShowDW.Checked);
//			config.GetDebugItem(ConfigCode.DebugWindowTopMost).SetValue<bool>(checkBoxDWTM.Checked);
//			config.GetDebugItem(ConfigCode.DebugSetWindowPos).SetValue<bool>(checkBoxSetDWPos.Checked);
//			config.GetDebugItem(ConfigCode.DebugWindowWidth).SetValue<int>((int)numericUpDownDWW.Value);
//			config.GetDebugItem(ConfigCode.DebugWindowHeight).SetValue<int>((int)numericUpDownDWH.Value);
//			config.GetDebugItem(ConfigCode.DebugWindowPosX).SetValue<int>((int)numericUpDownDWX.Value);
//			config.GetDebugItem(ConfigCode.DebugWindowPosY).SetValue<int>((int)numericUpDownDWY.Value);
//			config.SaveDebugConfig();
//		}



//		private void comboBoxReduceArgumentOnLoad_SelectedIndexChanged(object sender, EventArgs e)
//		{
//		}

//		DebugDialog dd;
//		private void button6_Click(object sender, EventArgs e)
//		{
//			if ((dd == null)||(!dd.Created))
//				return;
//			if (numericUpDownDWW.Enabled)
//				numericUpDownDWW.Value = dd.Width;
//			if (numericUpDownDWH.Enabled)
//				numericUpDownDWH.Value = dd.Height;
//		}

//		private void button5_Click(object sender, EventArgs e)
//		{

//			if ((dd == null) || (!dd.Created))
//				return;
//			if (numericUpDownDWX.Enabled)
//			{
//				if (numericUpDownDWX.Maximum < dd.Location.X)
//					numericUpDownDWX.Maximum = dd.Location.X;
//				if (numericUpDownDWX.Minimum > dd.Location.X)
//					numericUpDownDWX.Minimum = dd.Location.X;
//				numericUpDownDWX.Value = dd.Location.X;
//			}
//			if (numericUpDownDWY.Enabled)
//			{
//				if (numericUpDownDWY.Maximum < dd.Location.Y)
//					numericUpDownDWY.Maximum = dd.Location.Y;
//				if (numericUpDownDWY.Minimum > dd.Location.Y)
//					numericUpDownDWY.Minimum = dd.Location.Y;
//				numericUpDownDWY.Value = dd.Location.Y;
//			}
//		}


//	}
//}