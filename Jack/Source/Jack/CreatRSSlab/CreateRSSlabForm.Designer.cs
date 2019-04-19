/*
 * 由SharpDevelop创建。
 * 用户： SY.design
 * 日期: 2019/3/8
 * 时间: 上午 09:58
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace Jack.CreatRSSlab
{
	partial class CreateRSSlabForm
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
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(45, 74);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(184, 63);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseCompatibleTextRendering = true;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// CreateRSSlabForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(282, 253);
			this.Controls.Add(this.button1);
			this.Name = "CreateRSSlabForm";
			this.Text = "CreateRSSlabForm";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button button1;
	}
}
