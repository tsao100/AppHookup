/*
 * Created by SharpDevelop.
 * User: SY.design
 * Date: 2019/3/8
 * Time: 上午 11:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using TrackTools;
using TrackTools.TrackAlignments;
using TrackTools.Options;

namespace TrackTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("6697CB96-164C-4658-9A32-1C7D5F6DCEE9")]
	public partial class ThisApplication  //: IExternalCommand
	{
//		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
//        {
//
//            myMacro(commandData.Application.ActiveUIDocument);
//            return Result.Succeeded;
//        }


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
		public void GetXYZ(/*UIDocument uidoc*/)
		{
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
			TrackCenterLine tcl2 = new TrackCenterLine("ALDName2");
			TrackCenterLine tcl3 = new TrackCenterLine("ALDName3");
			double[] a=tcl1.Getxyz(500, 0);
			TaskDialog.Show("GetXYZ",string.Format("X={0:f5}, Y={1:f5}, Z={2:f5}.", a[0],a[1],a[2]));
			
//			Transaction transaction = new Transaction( uidoc.Document);
//     		transaction.Start( "Draw Line Patterns or Weights" );	
//
//	     		DrawLines myThis = new DrawLines();
//				if(Yes)	myThis._99_DrawLinePatterns(true, false, uidoc);
//				if(!Yes)	myThis._99_DrawLinePatterns(false, false, uidoc);
//  	  		transaction.Commit();    
	

		}
		
		public void AppSetup()
		{
			
			frmOptions opt = new frmOptions();
			opt.Show();
		}
	}
}