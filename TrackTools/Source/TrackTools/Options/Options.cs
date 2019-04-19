/*
 * 由SharpDevelop创建。
 * 用户： SY.design
 * 日期: 2019/4/16
 * 时间: 下午 03:56
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace TrackTools.Options
{
	/// <summary>
	/// Description of Options.
	/// </summary>
	public partial class frmOptions : Form
	{
		public frmOptions()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			GetAllALDFileName(comboBox1, label1, "ALDName1");
			GetAllALDFileName(comboBox2, label2, "ALDName2");
			GetAllALDFileName(comboBox3, label3, "ALDName3");
		}
		
		public void GetAllALDFileName(ComboBox obj, Label lbl, string AldNameNo)
		{
			string myPath=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\DataTable\\";
			string myName=FileSystem.Dir(myPath + "*.ald");
			try{				
			while (myName != "")
			{
				if (myName != "." && myName != "..")
				{
					if (myName.Substring(4,1).ToUpper() == "H")
					{
						obj.Items.Add(myName);
					}
					
				}
				myName=FileSystem.Dir();
			}
			}
			catch
			{
				//MessageBox.Show(n.ToString(),"");
			}
			int n=obj.FindString(Interaction.GetSetting("AlignmentTools","AppData",AldNameNo,"Y06UH.ALD"));
			obj.SelectedIndex=n;
			lbl.Text=obj.SelectedItem.ToString().Replace("H.ALD","V.ALD");
		}
		
		void FrmOptionsFormClosed(object sender, FormClosedEventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","AldName1",comboBox1.SelectedItem.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","AldName2",comboBox2.SelectedItem.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","AldName3",comboBox3.SelectedItem.ToString());
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			label1.Text=comboBox1.SelectedItem.ToString().Replace("H.ALD","V.ALD");
		}
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			label2.Text=comboBox2.SelectedItem.ToString().Replace("H.ALD","V.ALD");
		}
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			label3.Text=comboBox3.SelectedItem.ToString().Replace("H.ALD","V.ALD");
		}
	}
}
