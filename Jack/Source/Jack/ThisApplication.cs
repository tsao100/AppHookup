/*
 * Created by SharpDevelop.
 * User: SY.design
 * Date: 2019/2/15
 * Time: 下午 05:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using Jack.CreatRSSlab;

namespace Jack
{
	
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("47D2CBCA-2EEE-458F-BC2B-E55DD7F58B29")]
	public partial class ThisApplication
	{
		
		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}

		#region Revit Macros generated code
		private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
		public void HelloWorld()
		{
			TaskDialog.Show("Hello, World!","OK");
			CreateRSSlabForm f=new CreateRSSlabForm();
			f.Show();
		}
	}
}