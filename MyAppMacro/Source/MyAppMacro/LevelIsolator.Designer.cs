/*
 * 由SharpDevelop创建。
 * 用户： jack.tsao
 * 日期: 2018/7/4
 * 时间: 上午 11:31
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace MyAppMacro
{
	partial class LevelIsolator
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
			this.cbLevel = new System.Windows.Forms.ComboBox();
			this.btnApply = new System.Windows.Forms.Button();
			this.btnShowAll = new System.Windows.Forms.Button();
			this.btnApplyLevel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cbLevel
			// 
			this.cbLevel.FormattingEnabled = true;
			this.cbLevel.Location = new System.Drawing.Point(33, 40);
			this.cbLevel.Name = "cbLevel";
			this.cbLevel.Size = new System.Drawing.Size(162, 23);
			this.cbLevel.TabIndex = 0;
			this.cbLevel.SelectedIndexChanged += new System.EventHandler(this.CbLevelSelectedIndexChanged);
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(109, 200);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(117, 29);
			this.btnApply.TabIndex = 1;
			this.btnApply.Text = "只顯示";
			this.btnApply.UseCompatibleTextRendering = true;
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.BtnApplyClick);
			// 
			// btnShowAll
			// 
			this.btnShowAll.Location = new System.Drawing.Point(109, 165);
			this.btnShowAll.Name = "btnShowAll";
			this.btnShowAll.Size = new System.Drawing.Size(117, 29);
			this.btnShowAll.TabIndex = 1;
			this.btnShowAll.Text = "顯示全部";
			this.btnShowAll.UseCompatibleTextRendering = true;
			this.btnShowAll.UseVisualStyleBackColor = true;
			this.btnShowAll.Click += new System.EventHandler(this.BtnShowAllClick);
			// 
			// btnApplyLevel
			// 
			this.btnApplyLevel.Location = new System.Drawing.Point(33, 116);
			this.btnApplyLevel.Name = "btnApplyLevel";
			this.btnApplyLevel.Size = new System.Drawing.Size(193, 29);
			this.btnApplyLevel.TabIndex = 1;
			this.btnApplyLevel.Text = "改變構件的參考樓層至";
			this.btnApplyLevel.UseCompatibleTextRendering = true;
			this.btnApplyLevel.UseVisualStyleBackColor = true;
			this.btnApplyLevel.Click += new System.EventHandler(this.BtnApplyLevelClick);
			// 
			// LevelIsolator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(282, 253);
			this.Controls.Add(this.btnApplyLevel);
			this.Controls.Add(this.btnShowAll);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.cbLevel);
			this.Name = "LevelIsolator";
			this.Text = "LevelIsolator";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button btnApplyLevel;
		private System.Windows.Forms.Button btnShowAll;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.ComboBox cbLevel;
		
		void CbLevelSelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.btnApply.Text = "只顯示"+ this.cbLevel.Text;
			this.btnApplyLevel.Text = "改變構件的參考樓層至"+ this.cbLevel.Text;
		}
	}
}
