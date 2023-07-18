//namespace MinorShift.Emuera.Forms
//{
//	partial class DebugConfigDialog
//	{
//		/// <summary>
//		/// 必要なデザイナ変数です。
//		/// </summary>
//		private System.ComponentModel.IContainer components = null;

//		/// <summary>
//		/// 使用中のリソースをすべてクリーンアップします。
//		/// </summary>
//		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
//		protected override void Dispose(bool disposing)
//		{
//			if (disposing && (components != null))
//			{
//				components.Dispose();
//			}
//			base.Dispose(disposing);
//		}

//		#region Windows フォーム デザイナで生成されたコード

		// /// <summary>
		// /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		// /// コード エディタで変更しないでください。
		// /// </summary>
		// private void InitializeComponent()
		// {
        //     this.components = new System.ComponentModel.Container();
        //     this.buttonSave = new System.Windows.Forms.Button();
        //     this.buttonCancel = new System.Windows.Forms.Button();
        //     this.tabControl = new System.Windows.Forms.TabControl();
        //     this.tabPageDebug3 = new System.Windows.Forms.TabPage();
        //     this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
        //     this.label29 = new System.Windows.Forms.Label();
        //     this.checkBoxShowDW = new System.Windows.Forms.CheckBox();
        //     this.checkBoxDWTM = new System.Windows.Forms.CheckBox();
        //     this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
        //     this.numericUpDownDWW = new System.Windows.Forms.NumericUpDown();
        //     this.label28 = new System.Windows.Forms.Label();
        //     this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
        //     this.numericUpDownDWH = new System.Windows.Forms.NumericUpDown();
        //     this.label27 = new System.Windows.Forms.Label();
        //     this.button6 = new System.Windows.Forms.Button();
        //     this.checkBoxSetDWPos = new System.Windows.Forms.CheckBox();
        //     this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
        //     this.numericUpDownDWX = new System.Windows.Forms.NumericUpDown();
        //     this.label26 = new System.Windows.Forms.Label();
        //     this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
        //     this.numericUpDownDWY = new System.Windows.Forms.NumericUpDown();
        //     this.label25 = new System.Windows.Forms.Label();
        //     this.button5 = new System.Windows.Forms.Button();
        //     this.label16 = new System.Windows.Forms.Label();
        //     this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        //     this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
        //     this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
        //     this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
        //     this.tabControl.SuspendLayout();
        //     this.tabPageDebug3.SuspendLayout();
        //     this.flowLayoutPanel5.SuspendLayout();
        //     this.flowLayoutPanel1.SuspendLayout();
        //     ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWW)).BeginInit();
        //     this.flowLayoutPanel2.SuspendLayout();
        //     ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWH)).BeginInit();
        //     this.flowLayoutPanel3.SuspendLayout();
        //     ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWX)).BeginInit();
        //     this.flowLayoutPanel4.SuspendLayout();
        //     ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWY)).BeginInit();
        //     this.flowLayoutPanel6.SuspendLayout();
        //     this.flowLayoutPanel7.SuspendLayout();
        //     this.SuspendLayout();
        //     // 
        //     // buttonSave
        //     // 
        //     this.buttonSave.AutoSize = true;
        //     this.buttonSave.Location = new System.Drawing.Point(3, 3);
        //     this.buttonSave.Name = "buttonSave";
        //     this.buttonSave.Size = new System.Drawing.Size(49, 24);
        //     this.buttonSave.TabIndex = 1;
        //     this.buttonSave.Text = "保存";
        //     this.buttonSave.UseVisualStyleBackColor = true;
        //     this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
        //     // 
        //     // buttonCancel
        //     // 
        //     this.buttonCancel.AutoSize = true;
        //     this.buttonCancel.Location = new System.Drawing.Point(58, 3);
        //     this.buttonCancel.Name = "buttonCancel";
        //     this.buttonCancel.Size = new System.Drawing.Size(78, 24);
        //     this.buttonCancel.TabIndex = 2;
        //     this.buttonCancel.Text = "キャンセル";
        //     this.buttonCancel.UseVisualStyleBackColor = true;
        //     this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        //     // 
        //     // tabControl
        //     // 
        //     this.tabControl.Controls.Add(this.tabPageDebug3);
        //     this.tabControl.Location = new System.Drawing.Point(3, 3);
        //     this.tabControl.Multiline = true;
        //     this.tabControl.Name = "tabControl";
        //     this.tabControl.SelectedIndex = 0;
        //     this.tabControl.Size = new System.Drawing.Size(372, 353);
        //     this.tabControl.TabIndex = 35;
        //     // 
        //     // tabPageDebug3
        //     // 
        //     this.tabPageDebug3.Controls.Add(this.flowLayoutPanel5);
        //     this.tabPageDebug3.Location = new System.Drawing.Point(4, 22);
        //     this.tabPageDebug3.Name = "tabPageDebug3";
        //     this.tabPageDebug3.Padding = new System.Windows.Forms.Padding(3);
        //     this.tabPageDebug3.Size = new System.Drawing.Size(364, 327);
        //     this.tabPageDebug3.TabIndex = 7;
        //     this.tabPageDebug3.Text = "デバッグ";
        //     this.tabPageDebug3.UseVisualStyleBackColor = true;
        //     // 
        //     // flowLayoutPanel5
        //     // 
        //     this.flowLayoutPanel5.AutoSize = true;
        //     this.flowLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //     this.flowLayoutPanel5.Controls.Add(this.label29);
        //     this.flowLayoutPanel5.Controls.Add(this.checkBoxShowDW);
        //     this.flowLayoutPanel5.Controls.Add(this.checkBoxDWTM);
        //     this.flowLayoutPanel5.Controls.Add(this.flowLayoutPanel1);
        //     this.flowLayoutPanel5.Controls.Add(this.flowLayoutPanel2);
        //     this.flowLayoutPanel5.Controls.Add(this.button6);
        //     this.flowLayoutPanel5.Controls.Add(this.checkBoxSetDWPos);
        //     this.flowLayoutPanel5.Controls.Add(this.flowLayoutPanel3);
        //     this.flowLayoutPanel5.Controls.Add(this.flowLayoutPanel4);
        //     this.flowLayoutPanel5.Controls.Add(this.button5);
        //     this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
        //     this.flowLayoutPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
        //     this.flowLayoutPanel5.Location = new System.Drawing.Point(3, 3);
        //     this.flowLayoutPanel5.Name = "flowLayoutPanel5";
        //     this.flowLayoutPanel5.Size = new System.Drawing.Size(358, 321);
        //     this.flowLayoutPanel5.TabIndex = 37;
        //     // 
        //     // label29
        //     // 
        //     this.label29.AutoSize = true;
        //     this.label29.Location = new System.Drawing.Point(3, 3);
        //     this.label29.Margin = new System.Windows.Forms.Padding(3);
        //     this.label29.Name = "label29";
        //     this.label29.Size = new System.Drawing.Size(329, 24);
        //     this.label29.TabIndex = 98;
        //     this.label29.Text = "※デバッグ関連のオプションはコマンドライン引数に-Debug\r\nを指定して起動した時のみ有効です";
        //     // 
        //     // checkBoxShowDW
        //     // 
        //     this.checkBoxShowDW.AutoSize = true;
        //     this.checkBoxShowDW.Location = new System.Drawing.Point(3, 33);
        //     this.checkBoxShowDW.Name = "checkBoxShowDW";
        //     this.checkBoxShowDW.Size = new System.Drawing.Size(240, 16);
        //     this.checkBoxShowDW.TabIndex = 96;
        //     this.checkBoxShowDW.Text = "起動時にデバッグウインドウを表示する";
        //     this.checkBoxShowDW.UseVisualStyleBackColor = true;
        //     // 
        //     // checkBoxDWTM
        //     // 
        //     this.checkBoxDWTM.AutoSize = true;
        //     this.checkBoxDWTM.Location = new System.Drawing.Point(3, 55);
        //     this.checkBoxDWTM.Name = "checkBoxDWTM";
        //     this.checkBoxDWTM.Size = new System.Drawing.Size(240, 16);
        //     this.checkBoxDWTM.TabIndex = 97;
        //     this.checkBoxDWTM.Text = "デバッグウインドウを最前面に表示する";
        //     this.checkBoxDWTM.UseVisualStyleBackColor = true;
        //     // 
        //     // flowLayoutPanel1
        //     // 
        //     this.flowLayoutPanel1.AutoSize = true;
        //     this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //     this.flowLayoutPanel1.Controls.Add(this.numericUpDownDWW);
        //     this.flowLayoutPanel1.Controls.Add(this.label28);
        //     this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 74);
        //     this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
        //     this.flowLayoutPanel1.Name = "flowLayoutPanel1";
        //     this.flowLayoutPanel1.Size = new System.Drawing.Size(257, 27);
        //     this.flowLayoutPanel1.TabIndex = 37;
        //     // 
        //     // numericUpDownDWW
        //     // 
        //     this.numericUpDownDWW.Location = new System.Drawing.Point(3, 3);
        //     this.numericUpDownDWW.Name = "numericUpDownDWW";
        //     this.numericUpDownDWW.Size = new System.Drawing.Size(120, 21);
        //     this.numericUpDownDWW.TabIndex = 85;
        //     // 
        //     // label28
        //     // 
        //     this.label28.Anchor = System.Windows.Forms.AnchorStyles.Left;
        //     this.label28.AutoSize = true;
        //     this.label28.Location = new System.Drawing.Point(129, 7);
        //     this.label28.Name = "label28";
        //     this.label28.Size = new System.Drawing.Size(125, 12);
        //     this.label28.TabIndex = 87;
        //     this.label28.Text = "デバッグウィンドウ幅";
        //     // 
        //     // flowLayoutPanel2
        //     // 
        //     this.flowLayoutPanel2.AutoSize = true;
        //     this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //     this.flowLayoutPanel2.Controls.Add(this.numericUpDownDWH);
        //     this.flowLayoutPanel2.Controls.Add(this.label27);
        //     this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 101);
        //     this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
        //     this.flowLayoutPanel2.Name = "flowLayoutPanel2";
        //     this.flowLayoutPanel2.Size = new System.Drawing.Size(269, 27);
        //     this.flowLayoutPanel2.TabIndex = 37;
        //     // 
        //     // numericUpDownDWH
        //     // 
        //     this.numericUpDownDWH.Location = new System.Drawing.Point(3, 3);
        //     this.numericUpDownDWH.Name = "numericUpDownDWH";
        //     this.numericUpDownDWH.Size = new System.Drawing.Size(120, 21);
        //     this.numericUpDownDWH.TabIndex = 86;
        //     // 
        //     // label27
        //     // 
        //     this.label27.Anchor = System.Windows.Forms.AnchorStyles.Left;
        //     this.label27.AutoSize = true;
        //     this.label27.Location = new System.Drawing.Point(129, 7);
        //     this.label27.Name = "label27";
        //     this.label27.Size = new System.Drawing.Size(137, 12);
        //     this.label27.TabIndex = 88;
        //     this.label27.Text = "デバッグウィンドウ高さ";
        //     // 
        //     // button6
        //     // 
        //     this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        //     this.button6.AutoSize = true;
        //     this.button6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //     this.button6.Location = new System.Drawing.Point(149, 131);
        //     this.button6.Name = "button6";
        //     this.button6.Size = new System.Drawing.Size(183, 22);
        //     this.button6.TabIndex = 89;
        //     this.button6.Text = "現在のウィンドウサイズを取得";
        //     this.button6.UseVisualStyleBackColor = true;
        //     this.button6.Click += new System.EventHandler(this.button6_Click);
        //     // 
        //     // checkBoxSetDWPos
        //     // 
        //     this.checkBoxSetDWPos.AutoSize = true;
        //     this.checkBoxSetDWPos.Location = new System.Drawing.Point(3, 159);
        //     this.checkBoxSetDWPos.Name = "checkBoxSetDWPos";
        //     this.checkBoxSetDWPos.Size = new System.Drawing.Size(216, 16);
        //     this.checkBoxSetDWPos.TabIndex = 95;
        //     this.checkBoxSetDWPos.Text = "デバッグウィンドウ位置を指定する";
        //     this.checkBoxSetDWPos.UseVisualStyleBackColor = true;
        //     // 
        //     // flowLayoutPanel3
        //     // 
        //     this.flowLayoutPanel3.AutoSize = true;
        //     this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //     this.flowLayoutPanel3.Controls.Add(this.numericUpDownDWX);
        //     this.flowLayoutPanel3.Controls.Add(this.label26);
        //     this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 178);
        //     this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
        //     this.flowLayoutPanel3.Name = "flowLayoutPanel3";
        //     this.flowLayoutPanel3.Size = new System.Drawing.Size(275, 27);
        //     this.flowLayoutPanel3.TabIndex = 37;
        //     // 
        //     // numericUpDownDWX
        //     // 
        //     this.numericUpDownDWX.Location = new System.Drawing.Point(3, 3);
        //     this.numericUpDownDWX.Name = "numericUpDownDWX";
        //     this.numericUpDownDWX.Size = new System.Drawing.Size(120, 21);
        //     this.numericUpDownDWX.TabIndex = 90;
        //     // 
        //     // label26
        //     // 
        //     this.label26.Anchor = System.Windows.Forms.AnchorStyles.Left;
        //     this.label26.AutoSize = true;
        //     this.label26.Location = new System.Drawing.Point(129, 7);
        //     this.label26.Name = "label26";
        //     this.label26.Size = new System.Drawing.Size(143, 12);
        //     this.label26.TabIndex = 92;
        //     this.label26.Text = "デバッグウィンドウ位置X";
        //     // 
        //     // flowLayoutPanel4
        //     // 
        //     this.flowLayoutPanel4.AutoSize = true;
        //     this.flowLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //     this.flowLayoutPanel4.Controls.Add(this.numericUpDownDWY);
        //     this.flowLayoutPanel4.Controls.Add(this.label25);
        //     this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 205);
        //     this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
        //     this.flowLayoutPanel4.Name = "flowLayoutPanel4";
        //     this.flowLayoutPanel4.Size = new System.Drawing.Size(275, 27);
        //     this.flowLayoutPanel4.TabIndex = 37;
        //     // 
        //     // numericUpDownDWY
        //     // 
        //     this.numericUpDownDWY.Location = new System.Drawing.Point(3, 3);
        //     this.numericUpDownDWY.Name = "numericUpDownDWY";
        //     this.numericUpDownDWY.Size = new System.Drawing.Size(120, 21);
        //     this.numericUpDownDWY.TabIndex = 91;
        //     // 
        //     // label25
        //     // 
        //     this.label25.Anchor = System.Windows.Forms.AnchorStyles.Left;
        //     this.label25.AutoSize = true;
        //     this.label25.Location = new System.Drawing.Point(129, 7);
        //     this.label25.Name = "label25";
        //     this.label25.Size = new System.Drawing.Size(143, 12);
        //     this.label25.TabIndex = 93;
        //     this.label25.Text = "デバッグウィンドウ位置Y";
        //     // 
        //     // button5
        //     // 
        //     this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        //     this.button5.AutoSize = true;
        //     this.button5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //     this.button5.Location = new System.Drawing.Point(161, 235);
        //     this.button5.Name = "button5";
        //     this.button5.Size = new System.Drawing.Size(171, 22);
        //     this.button5.TabIndex = 94;
        //     this.button5.Text = "現在のウィンドウ位置を取得";
        //     this.button5.UseVisualStyleBackColor = true;
        //     this.button5.Click += new System.EventHandler(this.button5_Click);
        //     // 
        //     // label16
        //     // 
        //     this.label16.Anchor = System.Windows.Forms.AnchorStyles.Right;
        //     this.label16.AutoSize = true;
        //     this.label16.Location = new System.Drawing.Point(154, 362);
        //     this.label16.Margin = new System.Windows.Forms.Padding(3);
        //     this.label16.Name = "label16";
        //     this.label16.Size = new System.Drawing.Size(221, 12);
        //     this.label16.TabIndex = 36;
        //     this.label16.Text = "※変更は再起動するまで反映されません";
        //     // 
        //     // openFileDialog1
        //     // 
        //     this.openFileDialog1.FileName = "openFileDialog1";
        //     // 
        //     // flowLayoutPanel6
        //     // 
        //     this.flowLayoutPanel6.AutoScroll = true;
        //     this.flowLayoutPanel6.AutoSize = true;
        //     this.flowLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //     this.flowLayoutPanel6.Controls.Add(this.tabControl);
        //     this.flowLayoutPanel6.Controls.Add(this.label16);
        //     this.flowLayoutPanel6.Controls.Add(this.flowLayoutPanel7);
        //     this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
        //     this.flowLayoutPanel6.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
        //     this.flowLayoutPanel6.Location = new System.Drawing.Point(0, 0);
        //     this.flowLayoutPanel6.Name = "flowLayoutPanel6";
        //     this.flowLayoutPanel6.Size = new System.Drawing.Size(713, 648);
        //     this.flowLayoutPanel6.TabIndex = 37;
        //     // 
        //     // flowLayoutPanel7
        //     // 
        //     this.flowLayoutPanel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        //     this.flowLayoutPanel7.AutoSize = true;
        //     this.flowLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //     this.flowLayoutPanel7.Controls.Add(this.buttonSave);
        //     this.flowLayoutPanel7.Controls.Add(this.buttonCancel);
        //     this.flowLayoutPanel7.Location = new System.Drawing.Point(236, 380);
        //     this.flowLayoutPanel7.Name = "flowLayoutPanel7";
        //     this.flowLayoutPanel7.Size = new System.Drawing.Size(139, 30);
        //     this.flowLayoutPanel7.TabIndex = 38;
        //     // 
        //     // DebugConfigDialog
        //     // 
        //     this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        //     this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //     this.AutoSize = true;
        //     this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //     this.ClientSize = new System.Drawing.Size(713, 648);
        //     this.Controls.Add(this.flowLayoutPanel6);
        //     this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        //     this.MaximizeBox = false;
        //     this.MinimizeBox = false;
        //     this.Name = "DebugConfigDialog";
        //     this.ShowIcon = false;
        //     this.ShowInTaskbar = false;
        //     this.Text = "ConfigDialog";
        //     this.tabControl.ResumeLayout(false);
        //     this.tabPageDebug3.ResumeLayout(false);
        //     this.tabPageDebug3.PerformLayout();
        //     this.flowLayoutPanel5.ResumeLayout(false);
        //     this.flowLayoutPanel5.PerformLayout();
        //     this.flowLayoutPanel1.ResumeLayout(false);
        //     this.flowLayoutPanel1.PerformLayout();
        //     ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWW)).EndInit();
        //     this.flowLayoutPanel2.ResumeLayout(false);
        //     this.flowLayoutPanel2.PerformLayout();
        //     ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWH)).EndInit();
        //     this.flowLayoutPanel3.ResumeLayout(false);
        //     this.flowLayoutPanel3.PerformLayout();
        //     ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWX)).EndInit();
        //     this.flowLayoutPanel4.ResumeLayout(false);
        //     this.flowLayoutPanel4.PerformLayout();
        //     ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWY)).EndInit();
        //     this.flowLayoutPanel6.ResumeLayout(false);
        //     this.flowLayoutPanel6.PerformLayout();
        //     this.flowLayoutPanel7.ResumeLayout(false);
        //     this.flowLayoutPanel7.PerformLayout();
        //     this.ResumeLayout(false);
        //     this.PerformLayout();

//		}

//		#endregion

// 		private System.Windows.Forms.Button buttonSave;
// 		private System.Windows.Forms.Button buttonCancel;
// 		private System.Windows.Forms.TabControl tabControl;
// 		private System.Windows.Forms.Label label16;
// 		private System.Windows.Forms.OpenFileDialog openFileDialog1;
// 		private System.Windows.Forms.TabPage tabPageDebug3;
// 		private System.Windows.Forms.CheckBox checkBoxShowDW;
// 		private System.Windows.Forms.CheckBox checkBoxSetDWPos;
// 		private System.Windows.Forms.Button button5;
// 		private System.Windows.Forms.Label label25;
// 		private System.Windows.Forms.Label label26;
// 		private System.Windows.Forms.NumericUpDown numericUpDownDWY;
// 		private System.Windows.Forms.NumericUpDown numericUpDownDWX;
// 		private System.Windows.Forms.Button button6;
// 		private System.Windows.Forms.Label label27;
// 		private System.Windows.Forms.Label label28;
// 		private System.Windows.Forms.NumericUpDown numericUpDownDWH;
// 		private System.Windows.Forms.NumericUpDown numericUpDownDWW;
// 		private System.Windows.Forms.CheckBox checkBoxDWTM;
// 		private System.Windows.Forms.Label label29;
//         private System.Windows.Forms.ToolTip toolTip1;
//         private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
//         private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
//         private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
//         private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
//         private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
//         private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
//         private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
//     }
// }
