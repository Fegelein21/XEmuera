namespace MinorShift.Emuera.Forms
{
	partial class DebugConfigDialog
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.buttonSave = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageDebug3 = new System.Windows.Forms.TabPage();
			this.label29 = new System.Windows.Forms.Label();
			this.checkBoxDWTM = new System.Windows.Forms.CheckBox();
			this.checkBoxShowDW = new System.Windows.Forms.CheckBox();
			this.checkBoxSetDWPos = new System.Windows.Forms.CheckBox();
			this.button5 = new System.Windows.Forms.Button();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.numericUpDownDWY = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownDWX = new System.Windows.Forms.NumericUpDown();
			this.button6 = new System.Windows.Forms.Button();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.numericUpDownDWH = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownDWW = new System.Windows.Forms.NumericUpDown();
			this.label16 = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.tabControl.SuspendLayout();
			this.tabPageDebug3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWH)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWW)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonSave
			// 
			this.buttonSave.Location = new System.Drawing.Point(189, 356);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(49, 24);
			this.buttonSave.TabIndex = 1;
			this.buttonSave.Text = "保存";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(244, 356);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(78, 24);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPageDebug3);
			this.tabControl.Location = new System.Drawing.Point(12, 12);
			this.tabControl.Multiline = true;
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(313, 326);
			this.tabControl.TabIndex = 35;
			// 
			// tabPageDebug3
			// 
			this.tabPageDebug3.Controls.Add(this.label29);
			this.tabPageDebug3.Controls.Add(this.checkBoxDWTM);
			this.tabPageDebug3.Controls.Add(this.checkBoxShowDW);
			this.tabPageDebug3.Controls.Add(this.checkBoxSetDWPos);
			this.tabPageDebug3.Controls.Add(this.button5);
			this.tabPageDebug3.Controls.Add(this.label25);
			this.tabPageDebug3.Controls.Add(this.label26);
			this.tabPageDebug3.Controls.Add(this.numericUpDownDWY);
			this.tabPageDebug3.Controls.Add(this.numericUpDownDWX);
			this.tabPageDebug3.Controls.Add(this.button6);
			this.tabPageDebug3.Controls.Add(this.label27);
			this.tabPageDebug3.Controls.Add(this.label28);
			this.tabPageDebug3.Controls.Add(this.numericUpDownDWH);
			this.tabPageDebug3.Controls.Add(this.numericUpDownDWW);
			this.tabPageDebug3.Location = new System.Drawing.Point(4, 22);
			this.tabPageDebug3.Name = "tabPageDebug3";
			this.tabPageDebug3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageDebug3.Size = new System.Drawing.Size(305, 300);
			this.tabPageDebug3.TabIndex = 7;
			this.tabPageDebug3.Text = "デバッグ";
			this.tabPageDebug3.UseVisualStyleBackColor = true;
			// 
			// label29
			// 
			this.label29.AutoSize = true;
			this.label29.Location = new System.Drawing.Point(6, 13);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(272, 24);
			this.label29.TabIndex = 98;
			this.label29.Text = "※デバッグ関連のオプションはコマンドライン引数に-Debug\r\nを指定して起動した時のみ有効です";
			// 
			// checkBoxDWTM
			// 
			this.checkBoxDWTM.AutoSize = true;
			this.checkBoxDWTM.Location = new System.Drawing.Point(21, 73);
			this.checkBoxDWTM.Name = "checkBoxDWTM";
			this.checkBoxDWTM.Size = new System.Drawing.Size(202, 16);
			this.checkBoxDWTM.TabIndex = 97;
			this.checkBoxDWTM.Text = "デバッグウインドウを最前面に表示する";
			this.checkBoxDWTM.UseVisualStyleBackColor = true;
			// 
			// checkBoxShowDW
			// 
			this.checkBoxShowDW.AutoSize = true;
			this.checkBoxShowDW.Location = new System.Drawing.Point(21, 51);
			this.checkBoxShowDW.Name = "checkBoxShowDW";
			this.checkBoxShowDW.Size = new System.Drawing.Size(202, 16);
			this.checkBoxShowDW.TabIndex = 96;
			this.checkBoxShowDW.Text = "起動時にデバッグウインドウを表示する";
			this.checkBoxShowDW.UseVisualStyleBackColor = true;
			// 
			// checkBoxSetDWPos
			// 
			this.checkBoxSetDWPos.AutoSize = true;
			this.checkBoxSetDWPos.Location = new System.Drawing.Point(21, 180);
			this.checkBoxSetDWPos.Name = "checkBoxSetDWPos";
			this.checkBoxSetDWPos.Size = new System.Drawing.Size(179, 16);
			this.checkBoxSetDWPos.TabIndex = 95;
			this.checkBoxSetDWPos.Text = "デバッグウィンドウ位置を指定する";
			this.checkBoxSetDWPos.UseVisualStyleBackColor = true;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(67, 252);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(178, 24);
			this.button5.TabIndex = 94;
			this.button5.Text = "現在のウィンドウ位置を取得";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(147, 229);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(115, 12);
			this.label25.TabIndex = 93;
			this.label25.Text = "デバッグウィンドウ位置Y";
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(147, 204);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(115, 12);
			this.label26.TabIndex = 92;
			this.label26.Text = "デバッグウィンドウ位置X";
			// 
			// numericUpDownDWY
			// 
			this.numericUpDownDWY.Location = new System.Drawing.Point(21, 227);
			this.numericUpDownDWY.Name = "numericUpDownDWY";
			this.numericUpDownDWY.Size = new System.Drawing.Size(120, 19);
			this.numericUpDownDWY.TabIndex = 91;
			// 
			// numericUpDownDWX
			// 
			this.numericUpDownDWX.Location = new System.Drawing.Point(21, 202);
			this.numericUpDownDWX.Name = "numericUpDownDWX";
			this.numericUpDownDWX.Size = new System.Drawing.Size(120, 19);
			this.numericUpDownDWX.TabIndex = 90;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(67, 150);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(178, 24);
			this.button6.TabIndex = 89;
			this.button6.Text = "現在のウィンドウサイズを取得";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Location = new System.Drawing.Point(147, 127);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(104, 12);
			this.label27.TabIndex = 88;
			this.label27.Text = "デバッグウィンドウ高さ";
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Location = new System.Drawing.Point(147, 102);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(96, 12);
			this.label28.TabIndex = 87;
			this.label28.Text = "デバッグウィンドウ幅";
			// 
			// numericUpDownDWH
			// 
			this.numericUpDownDWH.Location = new System.Drawing.Point(21, 125);
			this.numericUpDownDWH.Name = "numericUpDownDWH";
			this.numericUpDownDWH.Size = new System.Drawing.Size(120, 19);
			this.numericUpDownDWH.TabIndex = 86;
			// 
			// numericUpDownDWW
			// 
			this.numericUpDownDWW.Location = new System.Drawing.Point(21, 100);
			this.numericUpDownDWW.Name = "numericUpDownDWW";
			this.numericUpDownDWW.Size = new System.Drawing.Size(120, 19);
			this.numericUpDownDWW.TabIndex = 85;
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(81, 341);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(197, 12);
			this.label16.TabIndex = 36;
			this.label16.Text = "※変更は再起動するまで反映されません";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// DebugConfigDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(334, 385);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonSave);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DebugConfigDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "ConfigDialog";
			this.tabControl.ResumeLayout(false);
			this.tabPageDebug3.ResumeLayout(false);
			this.tabPageDebug3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWH)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDWW)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TabPage tabPageDebug3;
		private System.Windows.Forms.CheckBox checkBoxShowDW;
		private System.Windows.Forms.CheckBox checkBoxSetDWPos;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.NumericUpDown numericUpDownDWY;
		private System.Windows.Forms.NumericUpDown numericUpDownDWX;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.NumericUpDown numericUpDownDWH;
		private System.Windows.Forms.NumericUpDown numericUpDownDWW;
		private System.Windows.Forms.CheckBox checkBoxDWTM;
		private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ToolTip toolTip1;
	}
}