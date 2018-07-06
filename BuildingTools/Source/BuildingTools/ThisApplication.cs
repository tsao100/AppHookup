/*
 * Created by SharpDevelop.
 * User: Jack
 * Date: 2018/6/19
 * Time: 下午 09:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using MyTools.CreateBeamsFromCAD;

namespace BuildingTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("212496CD-E9A9-48E5-9AE0-B2FB9ADD015A")]
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
		public void CreateBeamsFromCAD()
		{
        	using(Transaction trans = new Transaction(ActiveUIDocument.Document, "CreateBeamsFromCAD"))
        	{
        		trans.Start();
        		CreateBeamsFromCAD cb = new CreateBeamsFromCAD(ActiveUIDocument);
        		cb.Run();
        		trans.Commit();
        	}
		}
	}
}