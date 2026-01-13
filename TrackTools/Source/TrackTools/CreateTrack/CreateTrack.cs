/*
 * 由SharpDevelop创建。
 * 用户： Jack
 * 日期: 2019/7/24
 * 时间: 上午 09:29
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using System.Linq;
using TrackTools;
using TrackTools.TrackAlignments;
using TrackTools.Options;
using TrackTools.CreateTrack;
using X = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace TrackTools.CreateTrack
{
	/// <summary>
	/// Description of CreateTrack.
	/// </summary>
	
	
	public class CCreateTrack
	{
		UIApplication m_uiapp;

		private CCreateTrack()
		{
		}
		
		public CCreateTrack(UIApplication uiapp)
		{
			m_uiapp = uiapp;
		}
		public void Run()
		{
		    // A new handler to handle request posting by the dialog
            RequestHandler handler = new RequestHandler();

            // External Event for the dialog to use (to post requests)
            ExternalEvent exEvent = ExternalEvent.Create(handler);

			CrearteTrackForm ctf = new CrearteTrackForm(this, exEvent, handler);
			
			DialogResult dr = ctf.ShowDialog();
//			if (dr == DialogResult.Cancel)
//				Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "");
//			ctf.Show();
			
		}
		
		public void AddInstance()
		{
            UIDocument uidoc = m_uiapp.ActiveUIDocument;
			UIApplication uiapp = m_uiapp;
			Autodesk.Revit.DB.Document doc = uidoc.Document;
			
            //Get current selection and store it
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
            ISelectionFilter selFilter = new ModelLineSelectionFilter();
            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取軌道中心線：");
			
			
			FamilySymbol symbol;
			string FamilyPath_first="D:\\Tsao\\Developement\\RevitTrackModeling\\6pt.rfa";
            using(Transaction tr= new Transaction(doc, "BatchBuild"))
            {
				tr.Start();
			bool PlinthZhu = doc.LoadFamilySymbol(FamilyPath_first, System.IO.Path
				                            .GetFileNameWithoutExtension(FamilyPath_first), out symbol);   //这里的族一定要有类型才行
            symbol.Activate();   //激活族类型

            
		    // Create a new instance of an adaptive component family		
		    FamilyInstance instance = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, symbol);
		
		
		
		    // Get the placement points of this instance		
		    IList<ElementId> placePointIds = new List<ElementId>();		
		    placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(instance);		
		    
            
			
			
            

            // Retrieve needed information from reference object
            ModelCurve mc = doc.GetElement(hasPickOne.ElementId) as ModelCurve ;
	    
            //IntersectionResult ir = mc.GeometryCurve.Project(new XYZ(E, N, Z));
           
	            int i = 25;
	            foreach (ElementId id in placePointIds)
	            {
	                var t = i*2;
	                var point = doc.GetElement(id) as ReferencePoint;
	                var ploc = new PointLocationOnCurve(PointOnCurveMeasurementType.NonNormalizedCurveParameter, t,
	                                                    PointOnCurveMeasureFrom.Beginning);
	                var peref = m_uiapp.Application.Create.NewPointOnEdge(mc.GeometryCurve.Reference, ploc);
	                
	                point.SetPointElementReference(peref);
	                i++;
	            }
	            tr.Commit();
             }

		}
	}
}
