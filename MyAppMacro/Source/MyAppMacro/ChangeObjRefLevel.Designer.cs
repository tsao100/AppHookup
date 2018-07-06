/*
 * 由SharpDevelop创建。
 * 用户： jack.tsao
 * 日期: 2018/7/6
 * 时间: 下午 02:40
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace MyAppMacro
{
	partial class ChangeObjRefLevel
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
			this.cbBtmLevel = new System.Windows.Forms.ComboBox();
			this.cbTopLevel = new System.Windows.Forms.ComboBox();
			this.btnApply = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cbBtmLevel
			// 
			this.cbBtmLevel.FormattingEnabled = true;
			this.cbBtmLevel.Location = new System.Drawing.Point(12, 78);
			this.cbBtmLevel.Name = "cbBtmLevel";
			this.cbBtmLevel.Size = new System.Drawing.Size(151, 23);
			this.cbBtmLevel.TabIndex = 0;
			// 
			// cbTopLevel
			// 
			this.cbTopLevel.FormattingEnabled = true;
			this.cbTopLevel.Location = new System.Drawing.Point(12, 33);
			this.cbTopLevel.Name = "cbTopLevel";
			this.cbTopLevel.Size = new System.Drawing.Size(151, 23);
			this.cbTopLevel.TabIndex = 0;
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(78, 191);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(135, 31);
			this.btnApply.TabIndex = 1;
			this.btnApply.Text = "套用";
			this.btnApply.UseCompatibleTextRendering = true;
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.BtnApplyClick);
			// 
			// ChangeObjRefLevel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(282, 253);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.cbTopLevel);
			this.Controls.Add(this.cbBtmLevel);
			this.Name = "ChangeObjRefLevel";
			this.Text = "ChangeObjRefLevel";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.ComboBox cbTopLevel;
		private System.Windows.Forms.ComboBox cbBtmLevel;
	}
}
