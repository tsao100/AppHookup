/*
 * 由SharpDevelop创建。
 * 用户： SY.design
 * 日期: 2019/4/16
 * 时间: 下午 03:56
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
 
 using Microsoft.VisualBasic;
 
namespace TrackTools.Options
{
	partial class frmOptions
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.BtnExit = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(10, 18);
			this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(140, 20);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 54);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(139, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			this.label1.UseCompatibleTextRendering = true;
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(9, 80);
			this.comboBox2.Margin = new System.Windows.Forms.Padding(2);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(140, 20);
			this.comboBox2.TabIndex = 0;
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.ComboBox2SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(9, 115);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(139, 24);
			this.label2.TabIndex = 1;
			this.label2.Text = "label1";
			this.label2.UseCompatibleTextRendering = true;
			// 
			// comboBox3
			// 
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Location = new System.Drawing.Point(8, 142);
			this.comboBox3.Margin = new System.Windows.Forms.Padding(2);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(140, 20);
			this.comboBox3.TabIndex = 0;
			this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.ComboBox3SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 177);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(139, 24);
			this.label3.TabIndex = 1;
			this.label3.Text = "label1";
			this.label3.UseCompatibleTextRendering = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.comboBox3);
			this.groupBox1.Controls.Add(this.comboBox2);
			this.groupBox1.Controls.Add(this.comboBox1);
			this.groupBox1.Location = new System.Drawing.Point(12, 10);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(174, 214);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Current Alignment";
			this.groupBox1.UseCompatibleTextRendering = true;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(106, 235);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(53, 22);
			this.textBox1.TabIndex = 3;
			this.textBox1.Text = "35.00000";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(18, 239);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(82, 19);
			this.label4.TabIndex = 4;
			this.label4.Text = "Offset Limit:";
			this.label4.UseCompatibleTextRendering = true;
			// 
			// BtnExit
			// 
			this.BtnExit.Location = new System.Drawing.Point(343, 265);
			this.BtnExit.Name = "BtnExit";
			this.BtnExit.Size = new System.Drawing.Size(89, 36);
			this.BtnExit.TabIndex = 5;
			this.BtnExit.Text = "Finish";
			this.BtnExit.UseCompatibleTextRendering = true;
			this.BtnExit.UseVisualStyleBackColor = true;
			this.BtnExit.Click += new System.EventHandler(this.BtnExitClick);
			// 
			// frmOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(453, 313);
			this.ControlBox = false;
			this.Controls.Add(this.BtnExit);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.groupBox1);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "frmOptions";
			this.Text = "Options";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmOptionsFormClosed);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button BtnExit;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox1;
		
		void BtnExitClick(object sender, System.EventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "BtnExitClick");
			Close();
		}
	}
}
