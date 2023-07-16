//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using XEmuera.Forms;
//using System.Drawing.Text;

//namespace MinorShift.Emuera.Forms
//{
//	[global::System.Reflection.Obfuscation(Exclude=false)]
//	internal enum ConfigDialogResult
//	{
//		Cancel = 0,
//		Save = 1,
//		SaveReboot = 2,
//	}

//	internal partial class ConfigDialog : Form
//	{
//		public ConfigDialog()
//		{
//			InitializeComponent();
//            numericUpDown1.Minimum = 1;//PrintCPerLine
//            numericUpDown1.Maximum = 100;
//            numericUpDown2.Minimum = 128;//ConfigCode.WindowX(Width)
//			numericUpDown2.Maximum = 5000;
//            numericUpDown3.Minimum = 128;//ConfigCode.WindowY(Height)
//			numericUpDown3.Maximum = 5000;
//            numericUpDown4.Minimum = 500;//MaxLog
//			numericUpDown4.Maximum = 1000000;
//            numericUpDown5.Minimum = 8;//FontSize
//            numericUpDown5.Maximum = 144;
//            numericUpDown6.Minimum = 8;//LineHeight
//            numericUpDown6.Maximum = 144;
//            numericUpDown7.Minimum = 1;//FPS
//			numericUpDown7.Maximum = 240;
//            numericUpDown8.Minimum = 1;//ScrollHeight
//			numericUpDown8.Maximum = 10;
//            numericUpDown9.Minimum = 1;//PrintCLength
//            numericUpDown9.Maximum = 100;
//			numericUpDown10.Minimum = 0;//InfiniteLoopAlertTime
//			numericUpDown10.Maximum = 100000;
//            numericUpDown11.Minimum = 20;//SaveDataNos
//            numericUpDown11.Maximum = 80;
//			numericUpDownPosX.Maximum = 10000;//WindowPosX
//			numericUpDownPosY.Maximum = 10000;

//		}

//		private void buttonSave_Click(object sender, EventArgs e)
//		{
//			SaveConfig();
//			Result = ConfigDialogResult.Save;
//			this.Close();
//		}

//		private void buttonReboot_Click(object sender, EventArgs e)
//		{
//			SaveConfig();
//			Result = ConfigDialogResult.SaveReboot;
//			this.Close();
//		}

//		private void buttonCancel_Click(object sender, EventArgs e)
//		{
//			Result = ConfigDialogResult.Cancel;
//			this.Close();
//		}
//		public ConfigDialogResult Result = ConfigDialogResult.Cancel;

//		void setCheckBox(CheckBox checkbox, ConfigCode code)
//		{
//			ConfigItem item = ConfigData.Instance.GetConfigItem(code);
//			checkbox.Checked = (bool)item.Value;
//			checkbox.Enabled = !item.Fixed;
//		}
//		void setNumericUpDown(NumericUpDown updown, ConfigCode code)
//		{
//			ConfigItem item = ConfigData.Instance.GetConfigItem(code);
//			decimal value = (int)item.Value;
//			if (updown.Maximum < value)
//				updown.Maximum = value;
//			if (updown.Minimum > value)
//				updown.Minimum = value;
//			updown.Value = value;
//			updown.Enabled = !item.Fixed;
//		}

// 		void setColorBox(ColorBox colorBox, ConfigCode code)
// 		{
// 			ConfigItem<Color> item = (ConfigItem<Color>)ConfigData.Instance.GetConfigItem(code);
// 			colorBox.SelectingColor = item.Value;
// 			colorBox.Enabled = !item.Fixed;
// 		}
// /*		void setTextBox(TextBox textBox, ConfigCode code)
// 		{
// 			ConfigItem<string> item = (ConfigItem<string>)ConfigData.Instance.GetConfigItem(code);
// 			textBox.Text = item.Value;
// 			textBox.Enabled = !item.Fixed;
// 		}
// */
// 		MainWindow parent = null;
// 		public void SetConfig(MainWindow mainWindow)
// 		{
// 			parent = mainWindow;
// 			//ConfigData config = ConfigData.Instance;
// 			setCheckBox(checkBox1, ConfigCode.IgnoreCase);
// 			setCheckBox(checkBox2, ConfigCode.UseRenameFile);
// 			setCheckBox(checkBox3, ConfigCode.UseMouse);
// 			setCheckBox(checkBox4, ConfigCode.UseMenu);
// 			setCheckBox(checkBox5, ConfigCode.UseDebugCommand);
// 			setCheckBox(checkBox6, ConfigCode.AllowMultipleInstances);
// 			setCheckBox(checkBox7, ConfigCode.AutoSave);
// 			setCheckBox(checkBox8, ConfigCode.SizableWindow);
// 			setCheckBox(checkBox10, ConfigCode.UseReplaceFile);
// 			setCheckBox(checkBox11, ConfigCode.IgnoreUncalledFunction);
// 			//setCheckBox(checkBox12, ConfigCode.ReduceFormattedStringOnLoad);
// 			setCheckBox(checkBox13, ConfigCode.DisplayReport);
// 			setCheckBox(checkBox14, ConfigCode.ButtonWrap);
// 			setCheckBox(checkBox15, ConfigCode.SearchSubdirectory);
// 			setCheckBox(checkBox16, ConfigCode.SortWithFilename);
// 			setCheckBox(checkBox17, ConfigCode.SetWindowPos);
// 			setCheckBox(checkBox18, ConfigCode.UseKeyMacro);
// 			setCheckBox(checkBox20, ConfigCode.AllowFunctionOverloading);
// 			setCheckBox(checkBox19, ConfigCode.WarnFunctionOverloading);
//             setCheckBox(checkBox21, ConfigCode.WindowMaximixed);
//             setCheckBox(checkBox22, ConfigCode.WarnNormalFunctionOverloading);
// 			setCheckBox(checkBox23, ConfigCode.WarnBackCompatibility);
// 			setCheckBox(checkBoxCompatiErrorLine, ConfigCode.CompatiErrorLine);
// 			setCheckBox(checkBoxCompatiCALLNAME, ConfigCode.CompatiCALLNAME);
// 			setCheckBox(checkBox24, ConfigCode.UseSaveFolder);
// 			setCheckBox(checkBox27, ConfigCode.SystemSaveInUTF8);
// 			setCheckBox(checkBoxCompatiRAND, ConfigCode.CompatiRAND);
// 			setCheckBox(checkBoxCompatiLinefeedAs1739, ConfigCode.CompatiLinefeedAs1739);
// 			setCheckBox(checkBox28, ConfigCode.CompatiCallEvent);
// 			setCheckBox(checkBoxFuncNoIgnoreCase, ConfigCode.CompatiFunctionNoignoreCase);
// 			setCheckBox(checkBoxSystemFullSpace, ConfigCode.SystemAllowFullSpace);
// 			setCheckBox(checkBox12, ConfigCode.CompatiFuncArgOptional);
// 			setCheckBox(checkBox25, ConfigCode.CompatiFuncArgAutoConvert);
// 			setCheckBox(checkBox26, ConfigCode.SystemSaveInBinary);
// 			setCheckBox(checkBoxSystemTripleSymbol, ConfigCode.SystemIgnoreTripleSymbol);
// 			setCheckBox(checkBoxCompatiSP, ConfigCode.CompatiSPChara);
// 			setCheckBox(checkBox9, ConfigCode.TimesNotRigorousCalculation);
// 			setCheckBox(checkBox29, ConfigCode.SystemNoTarget);
// 			setCheckBox(checkBox30, ConfigCode.ForbidUpdateCheck);
// 			setCheckBox(checkBox31, ConfigCode.UseERD);
// 			setNumericUpDown(numericUpDown2, ConfigCode.WindowX);
// 			setNumericUpDown(numericUpDown3, ConfigCode.WindowY);
// 			setNumericUpDown(numericUpDown4, ConfigCode.MaxLog);
// 			setNumericUpDown(numericUpDown1, ConfigCode.PrintCPerLine);
// 			setNumericUpDown(numericUpDown9, ConfigCode.PrintCLength);
// 			setNumericUpDown(numericUpDown6, ConfigCode.LineHeight);
// 			setNumericUpDown(numericUpDown7, ConfigCode.FPS);
// 			setNumericUpDown(numericUpDown8, ConfigCode.ScrollHeight);
// 			setNumericUpDown(numericUpDown5, ConfigCode.FontSize);
// 			setNumericUpDown(numericUpDown10, ConfigCode.InfiniteLoopAlertTime);
//             setNumericUpDown(numericUpDown11, ConfigCode.SaveDataNos);

//			setNumericUpDown(numericUpDownPosX, ConfigCode.WindowPosX);
//			setNumericUpDown(numericUpDownPosY, ConfigCode.WindowPosY);

//			setColorBox(colorBoxFG, ConfigCode.ForeColor);
//			setColorBox(colorBoxBG, ConfigCode.BackColor);
//			setColorBox(colorBoxSelecting, ConfigCode.FocusColor);
//			setColorBox(colorBoxBacklog, ConfigCode.LogColor);

//			ConfigItem itemTDM = ConfigData.Instance.GetConfigItem(ConfigCode.TextDrawingMode);
//			switch ((TextDrawingMode)itemTDM.Value)
//			{
//				case TextDrawingMode.WINAPI:
//					comboBoxTextDrawingMode.SelectedIndex = 0; break;
//				case TextDrawingMode.TEXTRENDERER:
//					comboBoxTextDrawingMode.SelectedIndex = 1; break;
//				case TextDrawingMode.GRAPHICS:
//					comboBoxTextDrawingMode.SelectedIndex = 2; break;
//			}
//			comboBoxTextDrawingMode.Enabled = !itemTDM.Fixed;

//			ConfigItem itemStr = ConfigData.Instance.GetConfigItem(ConfigCode.FontName);
//			string fontname = (string)itemStr.Value;
//			int nameIndex = comboBox2.Items.IndexOf(fontname);
//			if (nameIndex >= 0)
//				comboBox2.SelectedIndex = nameIndex;
//			else
//			{
//				comboBox2.Text = fontname;
//				//nameIndex = comboBox2.Items.IndexOf("ＭＳ ゴシック");
//				//if (nameIndex >= 0)
//				//    comboBox2.SelectedIndex = nameIndex;
//			}
//			comboBox2.Enabled = !itemStr.Fixed;


//			ConfigItem itemRA = ConfigData.Instance.GetConfigItem(ConfigCode.ReduceArgumentOnLoad);
//			switch ((ReduceArgumentOnLoadFlag) itemRA.Value)
//			{
//				case ReduceArgumentOnLoadFlag.NO:
//					comboBoxReduceArgumentOnLoad.SelectedIndex = 0; break;
//				case ReduceArgumentOnLoadFlag.ONCE:
//					comboBoxReduceArgumentOnLoad.SelectedIndex = 1; break;
//				case ReduceArgumentOnLoadFlag.YES:
//					comboBoxReduceArgumentOnLoad.SelectedIndex = 2; break;
//			}
//			comboBoxReduceArgumentOnLoad.Enabled = !itemRA.Fixed;


//			ConfigItem itemInt = ConfigData.Instance.GetConfigItem(ConfigCode.DisplayWarningLevel);
//			if ((int)itemInt.Value <= 0)
//				comboBox5.SelectedIndex = 0;
//			else if ((int)itemInt.Value >= 3)
//				comboBox5.SelectedIndex = 3;
//			else
//				comboBox5.SelectedIndex = itemInt.Value;
//			comboBox5.Enabled = !itemInt.Fixed;


//			ConfigItem itemDWF = ConfigData.Instance.GetConfigItem(ConfigCode.FunctionNotFoundWarning);
//			switch ((DisplayWarningFlag)itemDWF.Value)
//			{
//				case DisplayWarningFlag.IGNORE:
//					comboBox3.SelectedIndex = 0; break;
//				case DisplayWarningFlag.LATER:
//					comboBox3.SelectedIndex = 1; break;
//				case DisplayWarningFlag.ONCE:
//					comboBox3.SelectedIndex = 2; break;
//				case DisplayWarningFlag.DISPLAY:
//					comboBox3.SelectedIndex = 3; break;
//			}
//			comboBox3.Enabled = !itemDWF.Fixed;

//			itemDWF = ConfigData.Instance.GetConfigItem(ConfigCode.FunctionNotCalledWarning);
//			switch ((DisplayWarningFlag)itemDWF.Value)
//			{
//				case DisplayWarningFlag.IGNORE:
//					comboBox4.SelectedIndex = 0; break;
//				case DisplayWarningFlag.LATER:
//					comboBox4.SelectedIndex = 1; break;
//				case DisplayWarningFlag.ONCE:
//					comboBox4.SelectedIndex = 2; break;
//				case DisplayWarningFlag.DISPLAY:
//					comboBox4.SelectedIndex = 3; break;
//			}
//			comboBox4.Enabled = !itemDWF.Fixed;

//            ConfigItem itemLang = ConfigData.Instance.GetConfigItem(ConfigCode.useLanguage);
//            switch ((UseLanguage)itemLang.Value)
//            {
//                case UseLanguage.JAPANESE:
//                    comboBox1.SelectedIndex = 0; break;
//                case UseLanguage.KOREAN:
//                    comboBox1.SelectedIndex = 1; break;
//                case UseLanguage.CHINESE_HANS:
//                    comboBox1.SelectedIndex = 2; break;
//                case UseLanguage.CHINESE_HANT:
//                    comboBox1.SelectedIndex = 3; break;
//            }

//            ConfigItem itemET = ConfigData.Instance.GetConfigItem(ConfigCode.EditorType);
//            switch ((TextEditorType)itemET.Value)
//            {
//                case TextEditorType.SAKURA:
//                    comboBox6.SelectedIndex = 0; break;
//                case TextEditorType.TERAPAD:
//                    comboBox6.SelectedIndex = 1; break;
//                case TextEditorType.EMEDITOR:
//                    comboBox6.SelectedIndex = 2; break;
//                case TextEditorType.USER_SETTING:
//                    comboBox6.SelectedIndex = 3; break;
//            }
//            comboBox6.Enabled = !itemET.Fixed;


//            textBox1.Text = Config.TextEditor;
//            textBox2.Text = Config.EditorArg;
//            textBox2.Enabled = (TextEditorType)itemET.Value == TextEditorType.USER_SETTING;
//		}

//		private void SaveConfig()
//		{
//			ConfigData config = ConfigData.Instance.Copy();
//			config.GetConfigItem(ConfigCode.IgnoreCase).SetValue(checkBox1.Checked);
//			config.GetConfigItem(ConfigCode.UseRenameFile).SetValue(checkBox2.Checked);
//			config.GetConfigItem(ConfigCode.UseMouse).SetValue(checkBox3.Checked);
//			config.GetConfigItem(ConfigCode.UseMenu).SetValue(checkBox4.Checked);
//			config.GetConfigItem(ConfigCode.UseDebugCommand).SetValue(checkBox5.Checked);
//			config.GetConfigItem(ConfigCode.AllowMultipleInstances).SetValue(checkBox6.Checked);
//			config.GetConfigItem(ConfigCode.AutoSave).SetValue(checkBox7.Checked);
//			config.GetConfigItem(ConfigCode.SizableWindow).SetValue(checkBox8.Checked);
//			config.GetConfigItem(ConfigCode.UseReplaceFile).SetValue(checkBox10.Checked);
//			config.GetConfigItem(ConfigCode.IgnoreUncalledFunction).SetValue(checkBox11.Checked);
//			//config.GetConfigItem(ConfigCode.ReduceFormattedStringOnLoad).SetValue(checkBox12.Checked);
//			config.GetConfigItem(ConfigCode.DisplayReport).SetValue(checkBox13.Checked);
//			config.GetConfigItem(ConfigCode.ButtonWrap).SetValue(checkBox14.Checked);
//			config.GetConfigItem(ConfigCode.SearchSubdirectory).SetValue(checkBox15.Checked);
//			config.GetConfigItem(ConfigCode.SortWithFilename).SetValue(checkBox16.Checked);
//			config.GetConfigItem(ConfigCode.SetWindowPos).SetValue(checkBox17.Checked);
//			config.GetConfigItem(ConfigCode.UseKeyMacro).SetValue(checkBox18.Checked);
//			config.GetConfigItem(ConfigCode.AllowFunctionOverloading).SetValue(checkBox20.Checked);
//			config.GetConfigItem(ConfigCode.WarnFunctionOverloading).SetValue(checkBox19.Checked);
//            config.GetConfigItem(ConfigCode.WindowMaximixed).SetValue(checkBox21.Checked);
//            config.GetConfigItem(ConfigCode.WarnNormalFunctionOverloading).SetValue(checkBox22.Checked);
//			config.GetConfigItem(ConfigCode.WarnBackCompatibility).SetValue(checkBox23.Checked);
//			config.GetConfigItem(ConfigCode.CompatiErrorLine).SetValue(checkBoxCompatiErrorLine.Checked);
//			config.GetConfigItem(ConfigCode.CompatiCALLNAME).SetValue(checkBoxCompatiCALLNAME.Checked);
//			config.GetConfigItem(ConfigCode.UseSaveFolder).SetValue(checkBox24.Checked);
//			config.GetConfigItem(ConfigCode.CompatiRAND).SetValue(checkBoxCompatiRAND.Checked);
//			config.GetConfigItem(ConfigCode.CompatiLinefeedAs1739).SetValue(checkBoxCompatiLinefeedAs1739.Checked);
//			config.GetConfigItem(ConfigCode.CompatiCallEvent).SetValue(checkBox28.Checked);
//			config.GetConfigItem(ConfigCode.SystemSaveInUTF8).SetValue(checkBox27.Checked);

//			config.GetConfigItem(ConfigCode.CompatiFuncArgOptional).SetValue(checkBox12.Checked);
//			config.GetConfigItem(ConfigCode.CompatiFuncArgAutoConvert).SetValue(checkBox25.Checked);
//			config.GetConfigItem(ConfigCode.SystemSaveInBinary).SetValue(checkBox26.Checked);
//			config.GetConfigItem(ConfigCode.SystemIgnoreTripleSymbol).SetValue(checkBoxSystemTripleSymbol.Checked);

			// config.GetConfigItem(ConfigCode.CompatiFunctionNoignoreCase).SetValue<bool>(checkBoxFuncNoIgnoreCase.Checked);
			// config.GetConfigItem(ConfigCode.SystemAllowFullSpace).SetValue<bool>(checkBoxSystemFullSpace.Checked);
			// config.GetConfigItem(ConfigCode.CompatiSPChara).SetValue<bool>(checkBoxCompatiSP.Checked);
			// config.GetConfigItem(ConfigCode.TimesNotRigorousCalculation).SetValue<bool>(checkBox9.Checked);
			// config.GetConfigItem(ConfigCode.SystemNoTarget).SetValue<bool>(checkBox29.Checked);
			// config.GetConfigItem(ConfigCode.ForbidUpdateCheck).SetValue<bool>(checkBox30.Checked);
			// config.GetConfigItem(ConfigCode.UseERD).SetValue<bool>(checkBox31.Checked);


			// config.GetConfigItem(ConfigCode.WindowX).SetValue<int>((int)numericUpDown2.Value);
			// config.GetConfigItem(ConfigCode.WindowY).SetValue<int>((int)numericUpDown3.Value);
			// config.GetConfigItem(ConfigCode.MaxLog).SetValue<int>((int)numericUpDown4.Value);
			// config.GetConfigItem(ConfigCode.PrintCPerLine).SetValue<int>((int)numericUpDown1.Value);
			// config.GetConfigItem(ConfigCode.PrintCLength).SetValue<int>((int)numericUpDown9.Value);
			// config.GetConfigItem(ConfigCode.LineHeight).SetValue<int>((int)numericUpDown6.Value);
			// config.GetConfigItem(ConfigCode.FPS).SetValue<int>((int)numericUpDown7.Value);
			// config.GetConfigItem(ConfigCode.ScrollHeight).SetValue<int>((int)numericUpDown8.Value);
			// config.GetConfigItem(ConfigCode.InfiniteLoopAlertTime).SetValue<int>((int)numericUpDown10.Value);
            // config.GetConfigItem(ConfigCode.SaveDataNos).SetValue<int>((int)numericUpDown11.Value);

//			config.GetConfigItem(ConfigCode.WindowPosX).SetValue((int)numericUpDownPosX.Value);
//			config.GetConfigItem(ConfigCode.WindowPosY).SetValue((int)numericUpDownPosY.Value);

//			config.GetConfigItem(ConfigCode.FontSize).SetValue((int)numericUpDown5.Value);
//			int nameIndex = comboBox2.SelectedIndex;
//			if (nameIndex >= 0)
//				config.GetConfigItem(ConfigCode.FontName).SetValue((string)comboBox2.SelectedItem);
//			else
//				config.GetConfigItem(ConfigCode.FontName).SetValue(comboBox2.Text);



//			config.GetConfigItem(ConfigCode.ForeColor).SetValue(colorBoxFG.SelectingColor);
//			config.GetConfigItem(ConfigCode.BackColor).SetValue(colorBoxBG.SelectingColor);
//			config.GetConfigItem(ConfigCode.FocusColor).SetValue(colorBoxSelecting.SelectingColor);
//			config.GetConfigItem(ConfigCode.LogColor).SetValue(colorBoxBacklog.SelectingColor);

//			switch (comboBoxTextDrawingMode.SelectedIndex)
//			{
//				case 0:
//					config.GetConfigItem(ConfigCode.TextDrawingMode).SetValue(TextDrawingMode.WINAPI); break;
//				case 1:
//					config.GetConfigItem(ConfigCode.TextDrawingMode).SetValue(TextDrawingMode.TEXTRENDERER); break;
//				case 2:
//					config.GetConfigItem(ConfigCode.TextDrawingMode).SetValue(TextDrawingMode.GRAPHICS); break;
//			}

//			switch (comboBoxReduceArgumentOnLoad.SelectedIndex)
//			{
//				case 0:
//					config.GetConfigItem(ConfigCode.ReduceArgumentOnLoad).SetValue(ReduceArgumentOnLoadFlag.NO); break;
//				case 1:
//					config.GetConfigItem(ConfigCode.ReduceArgumentOnLoad).SetValue(ReduceArgumentOnLoadFlag.ONCE); break;
//				case 2:
//					config.GetConfigItem(ConfigCode.ReduceArgumentOnLoad).SetValue(ReduceArgumentOnLoadFlag.YES); break;
//			}
//			config.GetConfigItem(ConfigCode.DisplayWarningLevel).SetValue(comboBox5.SelectedIndex);


//			switch (comboBox3.SelectedIndex)
//			{
//				case 0:
//					config.GetConfigItem(ConfigCode.FunctionNotFoundWarning).SetValue(DisplayWarningFlag.IGNORE); break;
//				case 1:
//					config.GetConfigItem(ConfigCode.FunctionNotFoundWarning).SetValue(DisplayWarningFlag.LATER); break;
//				case 2:
//					config.GetConfigItem(ConfigCode.FunctionNotFoundWarning).SetValue(DisplayWarningFlag.ONCE); break;
//				case 3:
//					config.GetConfigItem(ConfigCode.FunctionNotFoundWarning).SetValue(DisplayWarningFlag.DISPLAY); break;
//			}
//			switch (comboBox4.SelectedIndex)
//			{
//				case 0:
//					config.GetConfigItem(ConfigCode.FunctionNotCalledWarning).SetValue(DisplayWarningFlag.IGNORE); break;
//				case 1:
//					config.GetConfigItem(ConfigCode.FunctionNotCalledWarning).SetValue(DisplayWarningFlag.LATER); break;
//				case 2:
//					config.GetConfigItem(ConfigCode.FunctionNotCalledWarning).SetValue(DisplayWarningFlag.ONCE); break;
//				case 3:
//					config.GetConfigItem(ConfigCode.FunctionNotCalledWarning).SetValue(DisplayWarningFlag.DISPLAY); break;
//			}
//            switch (comboBox1.SelectedIndex)
//            {
//                case 0:
//                    config.GetConfigItem(ConfigCode.useLanguage).SetValue(UseLanguage.JAPANESE); break;
//                case 1:
//                    config.GetConfigItem(ConfigCode.useLanguage).SetValue(UseLanguage.KOREAN); break;
//                case 2:
//                    config.GetConfigItem(ConfigCode.useLanguage).SetValue(UseLanguage.CHINESE_HANS); break;
//                case 3:
//                    config.GetConfigItem(ConfigCode.useLanguage).SetValue(UseLanguage.CHINESE_HANT); break;
//            }
//            switch (comboBox6.SelectedIndex)
//            {
//                case 0:
//                    config.GetConfigItem(ConfigCode.EditorType).SetValue(TextEditorType.SAKURA); break;
//                case 1:
//                    config.GetConfigItem(ConfigCode.EditorType).SetValue(TextEditorType.TERAPAD); break;
//                case 2:
//                    config.GetConfigItem(ConfigCode.EditorType).SetValue(TextEditorType.EMEDITOR); break;
//                case 3:
//                    config.GetConfigItem(ConfigCode.EditorType).SetValue(TextEditorType.USER_SETTING); break;
//            }

//            config.GetConfigItem(ConfigCode.TextEditor).SetValue(textBox1.Text);
//            config.GetConfigItem(ConfigCode.EditorArgument).SetValue(textBox2.Text);

//			config.SaveConfig();
//		}


//		private void comboBoxReduceArgumentOnLoad_SelectedIndexChanged(object sender, EventArgs e)
//		{
//			//いちいち切り替えるのが面倒なのでまとめて却下
//			/*if (comboBoxReduceArgumentOnLoad.SelectedIndex == 0)
//			{
//				comboBox3.Enabled = false;
//				comboBox4.Enabled = false;
//				comboBox5.Enabled = false;
//				checkBox12.Enabled = false;
//				checkBox11.Enabled = false;
//			}
//			else
//			{
//				comboBox3.Enabled = true;
//				comboBox4.Enabled = true;
//				comboBox5.Enabled = true;
//				checkBox12.Enabled = true;
//				checkBox11.Enabled = true;
//			}*/


//		}


//		private void button1_Click(object sender, EventArgs e)
//		{
//			if (parent == null)
//				return;
//			if (numericUpDown2.Enabled)
//				numericUpDown2.Value = parent.MainPicBox.Width;
//			if (numericUpDown3.Enabled)
//				numericUpDown3.Value = parent.MainPicBox.Height + (int)Config.LineHeight;
//		}

//		private void button3_Click(object sender, EventArgs e)
//		{
//			if (parent == null)
//				return;
//			if (numericUpDownPosX.Enabled)
//			{
//				if (numericUpDownPosX.Maximum < parent.Location.X)
//					numericUpDownPosX.Maximum = parent.Location.X;
//				if (numericUpDownPosX.Minimum > parent.Location.X)
//					numericUpDownPosX.Minimum = parent.Location.X;
//				numericUpDownPosX.Value = parent.Location.X;
//			}
//			if (numericUpDownPosY.Enabled)
//			{
//				if (numericUpDownPosY.Maximum < parent.Location.Y)
//					numericUpDownPosY.Maximum = parent.Location.Y;
//				if (numericUpDownPosY.Minimum > parent.Location.Y)
//					numericUpDownPosY.Minimum = parent.Location.Y;
//				numericUpDownPosY.Value = parent.Location.Y;
//			}

//		}

//		private void button2_Click(object sender, EventArgs e)
//		{
//			if (!comboBox2.Enabled)
//				return;
//			InstalledFontCollection ifc = new InstalledFontCollection();
//			foreach (FontFamily ff in ifc.Families)
//			{
//				if (!ff.IsStyleAvailable(FontStyle.Regular))
//					continue;
//				if (!ff.IsStyleAvailable(FontStyle.Bold))
//					continue;
//				if (!ff.IsStyleAvailable(FontStyle.Italic))
//					continue;
//				if (!ff.IsStyleAvailable(FontStyle.Strikeout))
//					continue;
//				if (!ff.IsStyleAvailable(FontStyle.Underline))
//					continue;
//				comboBox2.Items.Add(ff.Name);
//			}
//			string fontname = comboBox2.Text;
//			if (!string.IsNullOrEmpty(fontname))
//			{
//				int nameIndex = comboBox2.Items.IndexOf(fontname);
//				if (nameIndex >= 0)
//					comboBox2.SelectedIndex = nameIndex;
//			}
//		}

//        private void button4_Click(object sender, EventArgs e)
//        {
//            openFileDialog1.InitialDirectory = @"c:\Program Files";
//            openFileDialog1.FileName = "";
//            DialogResult res = openFileDialog1.ShowDialog();
//            if (res == DialogResult.OK)
//            {
//                textBox1.Text = openFileDialog1.FileName;
//            }
//        }
        

//		int setCheckBoxChecked(CheckBox checkbox, bool flag)
//		{
//			if(checkbox.Checked == flag)
//				return 0;//変更不要
//			if(!checkbox.Enabled)
//				return -1;//変更したいけど許可されなかった
//			checkbox.Checked = flag;
//			return 1;//変更した
//		}

//        int setComboBoxChanged(ComboBox combobox, int value)
//        {
//            if (combobox.SelectedIndex == value)
//                return 0;//変更不要
//            if (!combobox.Enabled)
//                return -1;//変更したいけど許可されなかった
//            combobox.SelectedIndex = value;
//            return 1;//変更した
//        }
		
//		private void button7_Click(object sender, EventArgs e)
//		{//eramaker仕様
//			bool disenabled = false;
//			disenabled |= setCheckBoxChecked(checkBoxCompatiErrorLine, true) < 0;
//			disenabled |= setCheckBoxChecked(checkBoxCompatiCALLNAME, true) < 0;
//			disenabled |= setCheckBoxChecked(checkBoxCompatiRAND, true) < 0;
//			disenabled |= setCheckBoxChecked(checkBoxFuncNoIgnoreCase, true) < 0;
//			disenabled |= setCheckBoxChecked(checkBox28, true) < 0;
//			disenabled |= setCheckBoxChecked(checkBoxCompatiSP, true) < 0;
//			disenabled |= setCheckBoxChecked(checkBoxCompatiLinefeedAs1739, false) < 0;
//			disenabled |= setCheckBoxChecked(checkBox12, false) < 0;
//			disenabled |= setCheckBoxChecked(checkBox25, false) < 0;
//            disenabled |= setCheckBoxChecked(checkBox9, true) < 0;
//			if(disenabled)
//                MessageBox.Show("いくつかの設定は_fixed.configにより変更を許可されていないために変更できませんでした", "設定変更不可");
//        }

//		private void button8_Click(object sender, EventArgs e)
//		{//最新Emuera仕様 - 全部false
//			bool disenabled = false;
//			disenabled |= setCheckBoxChecked(checkBoxCompatiErrorLine, false) < 0;
//			disenabled |= setCheckBoxChecked(checkBoxCompatiCALLNAME, false) < 0;
//			disenabled |= setCheckBoxChecked(checkBoxCompatiRAND, false) < 0;
//			disenabled |= setCheckBoxChecked(checkBoxFuncNoIgnoreCase, false) < 0;
//			disenabled |= setCheckBoxChecked(checkBox28, false) < 0;
//			disenabled |= setCheckBoxChecked(checkBoxCompatiLinefeedAs1739, false) < 0;
//			disenabled |= setCheckBoxChecked(checkBox12, false) < 0;
//			disenabled |= setCheckBoxChecked(checkBox25, false) < 0;
//			disenabled |= setCheckBoxChecked(checkBoxCompatiSP, false) < 0;
//            disenabled |= setCheckBoxChecked(checkBox9, false) < 0;
//			if (disenabled)
//                MessageBox.Show("いくつかの設定は_fixed.configにより変更を許可されていないために変更できませんでした", "設定変更不可");
//		}

//        //互換性チェックはいじらないように変更
//        private void button5_Click(object sender, EventArgs e)
//        {//解析のユーザー向け設定（デフォルト設定と同じ）
//            bool disenabled = false;
//            //disenabled |= setCheckBoxChecked(checkBox23, true) < 0;
//            disenabled |= setCheckBoxChecked(checkBox13, false) < 0;
//            disenabled |= setComboBoxChanged(comboBoxReduceArgumentOnLoad, 0) < 0;
//            disenabled |= setComboBoxChanged(comboBox5, 1) < 0;
//            disenabled |= setCheckBoxChecked(checkBox11, true) < 0;
//            disenabled |= setComboBoxChanged(comboBox3, 0) < 0;
//            disenabled |= setComboBoxChanged(comboBox4, 0) < 0;
//            if (disenabled)
//                MessageBox.Show("いくつかの設定は_fixed.configにより変更を許可されていないために変更できませんでした", "設定変更不可");
//        }

//        private void button6_Click(object sender, EventArgs e)
//        {//解析の開発者向け設定（関数名以外はしっかりチェックする）
//            bool disenabled = false;
//            //disenabled |= setCheckBoxChecked(checkBox23, true) < 0;
//            disenabled |= setCheckBoxChecked(checkBox13, true) < 0;
//            disenabled |= setComboBoxChanged(comboBoxReduceArgumentOnLoad, 2) < 0;
//            disenabled |= setComboBoxChanged(comboBox5, 0) < 0;
//            disenabled |= setCheckBoxChecked(checkBox11, true) < 0;
//            disenabled |= setComboBoxChanged(comboBox3, 0) < 0;
//            disenabled |= setComboBoxChanged(comboBox4, 0) < 0;
//            if (disenabled)
//                MessageBox.Show("いくつかの設定は_fixed.configにより変更を許可されていないために変更できませんでした", "設定変更不可");
//        }

//        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            textBox2.Enabled = ((ComboBox)sender).SelectedIndex == 3;
//        }
//	}
//}