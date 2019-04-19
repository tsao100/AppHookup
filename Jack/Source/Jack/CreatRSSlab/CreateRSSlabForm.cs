/*
 * 由SharpDevelop创建。
 * 用户： SY.design
 * 日期: 2019/3/8
 * 时间: 上午 09:58
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Jack.CreatRSSlab
{
	/// <summary>
	/// Description of CreateRSSlabForm.
	/// </summary>
	public partial class CreateRSSlabForm : System.Windows.Forms.Form
	{
		public CreateRSSlabForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			TaskDialog.Show("Jack","Hello, Jack San.");
		}
	}
}
