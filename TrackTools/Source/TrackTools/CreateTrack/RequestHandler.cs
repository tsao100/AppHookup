/*
 * 由SharpDevelop创建。
 * 用户： Jack
 * 日期: 2019/8/2
 * 时间: 下午 12:58
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Linq;
using TrackTools;
using TrackTools.TrackAlignments;
using X = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using Microsoft.VisualBasic;

namespace TrackTools.CreateTrack
{
	/// <summary>
	/// Description of RequestHandler.
	/// </summary>
	public class RequestHandler : IExternalEventHandler
	{
		private Request m_request = new Request();

        /// <summary>
        /// A public property to access the current request value
        /// </summary>
        public Request Request
        {
            get { return m_request; }
        }

		public string GetName()
		{
			return "CreateTrack";
		}
		
		public void Execute(UIApplication uiapp)
		{
                switch (Request.Take())
                {
                    case RequestId.None:
                        {
                            //return;  // no request at this time -> we can leave immediately
                            break;
                        }
                    case RequestId.Test:
                        {
							UIDocument uidoc = uiapp.ActiveUIDocument;
				
							//string ALDName = Interaction.GetSetting("AlignmentTools","AppData","ALDName1","Y06UH.ALD");
							
							//Get current selection and store it
				            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
				            ISelectionFilter selFilter = new ModelLineSelectionFilter();
				            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取軌道中心線：");
				            
				            TaskDialog.Show("Create Track", hasPickOne.ElementId.ToString());
                            break;
                        }
                	case RequestId.BatchBuild:
                		{
                			AddPlinthByList(uiapp);
                			break;
                		}
                	default:
                		{
                			break;
                		}
                }


		}
		
		private void AddPlinthByList(UIApplication uiapp)	//Create Plinth by the list get from excel sheet	
		{
			
			Document doc = uiapp.ActiveUIDocument.Document;
            UIDocument uidoc = uiapp.ActiveUIDocument;

			
//			//Get current selection and store it
//            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
//            ISelectionFilter selFilter = new ModelLineSelectionFilter();
//            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取軌道中心線：");
//            
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
//
//            // Retrieve needed information from reference object
//            ModelCurve mc = doc.GetElement(hasPickOne.ElementId) as ModelCurve ;
            ModelCurve mc = doc.GetElement(GetActiveMC()) as ModelCurve ;

			X.Application excel = (X.Application)Marshal.GetActiveObject("Excel.Application");
			if( null == excel )
			{
				
				TaskDialog.Show("Getpw", "Failed to get or start Excel." );
				
			}
			
			X.Worksheet worksheet = (X.Worksheet) excel.ActiveSheet;
			double StartChainage, EndChainage;
			string PlinthNo;
			int i = excel.ActiveCell.Row;
			while((worksheet.Cells[i,1] as X.Range).Text as string != "")
			{
				
				PlinthNo = (worksheet.Cells[i,1] as X.Range).Text as string;
				StartChainage = (double)(worksheet.Cells[i,2] as X.Range).Value2;
				EndChainage = (double)(worksheet.Cells[i,3] as X.Range).Value2;
				
				AddOnePlinth(uiapp, PlinthNo, StartChainage, EndChainage, mc, tcl1);
				i++;
			}
		}
		
		public ElementId GetActiveMC()
		{
			string ALDName = Interaction.GetSetting("AlignmentTools","AppData","ALDName1","Y06UH.ALD");
			string MCid = Interaction.GetSetting("AlignmentTools","AppData", ALDName,"Y06UH.ALD");
			
			return new ElementId(int.Parse(MCid));
		}
		
		public string GetActiveComponent()
		{
			string ActiveComponent = Interaction.GetSetting("AlignmentTools","AppData","ActiveComponent","Plinth");
			
			return ActiveComponent ;
		}

		public string GetActiveFolder()
		{
			string ActiveFolder = Interaction.GetSetting("AlignmentTools","AppData","ActiveFolder","Slab");
			
			return ActiveFolder;
		}
		

		//這是之前配合族群沒採用類型參數與族群內部實例參數關聯的程序，後來改採用有族群參數關聯的方法，故改寫 AddOnePlinth()
		public void AddOnePlinth_OLD(UIApplication uiapp, string PlinthNo, double StartChainage, double EndChainage, ModelCurve mc, TrackCenterLine tcl1)		
		{
			
			double PlinthLength = EndChainage - StartChainage;
			//手動添加需要有模型線才可以使用。
			Document doc = uiapp.ActiveUIDocument.Document;
            UIDocument uidoc = uiapp.ActiveUIDocument;
			double interval;
			IList<ElementId> placePointIds;
			FamilyOption opt = new FamilyOption();
			FamilySymbol symbol;
			FamilyInstance instance;
			interval = PlinthLength / 5.0;
			//Family family;			
			
			using( Transaction tr = new Transaction(doc, "CreatePlinthInstance"))
			{
				tr.Start();
				doc.LoadFamilySymbol("D:\\Tsao\\Developement\\RevitTrackModeling\\6pt.rfa", "6pt", opt, out symbol);
				//document.LoadFamily("D:\\Tsao\\Developement\\RevitTrackModeling\\6pt.rfa", opt, out family);
				symbol.Family.Name = PlinthNo + "-" + PlinthLength.ToString("0.000");
				symbol.Name= PlinthNo + "-" + PlinthLength.ToString("0.000");
			    tr.Commit();
			}
			
			int i=0;		
			XYZ pl = getprojectPosition(uiapp);			
		    Family family = symbol.Family;
	        // Get Family document for family
	        Document familyDoc = doc.EditFamily(family);
	        if (null != familyDoc && familyDoc.IsFamilyDocument == true)
	        {
	            //String loadedFamilies = "FamilySymbols in " + family.Name + ":\n";
	        	using(Transaction tr = new Transaction(familyDoc, "EditFamily"))
	        	{
	        		tr.Start();
		            FilteredElementCollector collector = new FilteredElementCollector(familyDoc);
		            ICollection<Element> collection = 
		                collector.OfClass(typeof(FamilySymbol))
		            		            	.ToElements();
		            for(i=0; i<6; i++)
		            {
						var t = StartChainage + i * interval;
						double cant = Math.Round(tcl1.getAppliedCant(t, true));
						double el = (tcl1.Getz(t) - pl.Z) * 1000;
		            	FamilySymbol famSym = GetSymbol(familyDoc, "LeftPlinth", (i+1).ToString());
						SetElementParameterInMm( famSym, "C", cant );
						SetElementParameterInMm( famSym, "EL", el );
						famSym = GetSymbol(familyDoc, "RightPlinth", (i+1).ToString());	
						SetElementParameterInMm( famSym, "C", cant );				
						SetElementParameterInMm( famSym, "EL", el );				
		            }
		            
		            
		            tr.Commit();
	        	}
	        	
	        	using(Transaction tr = new Transaction(familyDoc, "EditFamily"))
	        	{
	        		tr.Start();	        	
	        		familyDoc.LoadFamily(doc, opt);
		            tr.Commit();
	        	}
	        	
	        	familyDoc.Close(false);
	        	
				//TaskDialog.Show("Revit", loadedFamilies);
	        }
		    // Get the placement points of this instance		
			
	        
			using (Transaction tr= new Transaction(doc, "CreatePlinthInstance"))
			{
			tr.Start();

			FamilySymbol symbol1 = GetSymbol(doc, PlinthNo + "-" + PlinthLength.ToString("0.000"), PlinthNo + "-" + PlinthLength.ToString("0.000"));
            
			instance = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, symbol1);  
			placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(instance);
	    
            //IntersectionResult ir = mc.GeometryCurve.Project(new XYZ(E, N, Z));

            i = 0;
            foreach (ElementId id in placePointIds)
            {
                var t = StartChainage + i * interval;
                var point = doc.GetElement(id) as ReferencePoint;
                var ploc = new PointLocationOnCurve(PointOnCurveMeasurementType.NonNormalizedCurveParameter, m2ft(t),
                                                    PointOnCurveMeasureFrom.Beginning);
                var peref = uiapp.Application.Create.NewPointOnEdge(mc.GeometryCurve.Reference, ploc);
                point.SetPointElementReference(peref);
                i++;
            }
			tr.Commit();
			}
		}
		
	
		
		public void AddOnePlinth(UIApplication uiapp, string PlinthNo, double StartChainage, double EndChainage, ModelCurve mc, TrackCenterLine tcl1)		
		{
			
			string myPath=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\TrackSystem\\" + GetActiveFolder() + "\\" + GetActiveComponent() + "\\";

			double PlinthLength = EndChainage - StartChainage;
			//手動添加需要有模型線才可以使用。
			Document doc = uiapp.ActiveUIDocument.Document;
            UIDocument uidoc = uiapp.ActiveUIDocument;
			double interval;
			IList<ElementId> placePointIds;
			FamilyOption opt = new FamilyOption();
			FamilySymbol symbol;
			FamilyInstance instance;
			int ptNum = (int)Math.Ceiling(PlinthLength / 3.0 + 1);
			interval = PlinthLength / (ptNum-1);
			//Family family;
			
			//檢查族群是否已載入
			string FamilyName = GetActiveComponent() + "-" + ptNum.ToString()+"pt";
			
			Family family = Utility.FindElementByName(doc, typeof( Family ), FamilyName ) as Family;
			
 
		    if( null == family )
		    {						
				using( Transaction tr = new Transaction(doc, "CreatePlinthInstance"))
				{
					tr.Start();
					doc.LoadFamilySymbol(myPath + FamilyName + ".rfa", "001", opt, out symbol);
					symbol.Name= PlinthNo + "-" + PlinthLength.ToString("0.000");
					tr.Commit();
				}
		    }
		    else
		    {
				symbol = GetSymbol(family.Document, FamilyName);		    	
				using( Transaction tr = new Transaction(doc, "CreateNewElement"))
				{
					tr.Start();
				    symbol = CreateNewElement(uiapp, symbol, PlinthNo, StartChainage, EndChainage, ptNum, tcl1);		    
				   	symbol.Name= PlinthNo + "-" + PlinthLength.ToString("0.000");
				    tr.Commit();
				}
		    }

			XYZ pl = getprojectPosition(uiapp);	
			using (Transaction tr= new Transaction(doc, "SetElementParameter"))
			{
				tr.Start();			
			
				for (int i = 0; i < ptNum; i++) {
					var t = StartChainage + i * interval;
					double cant = Math.Round(tcl1.getAppliedCant(t, true));
					double el = (tcl1.Getz(t) - pl.Z) * 1000;
					double slope = tcl1.Gets(t);
					
					SetElementParameterInMm( symbol, "C"+(i+1).ToString(), cant );
					SetElementParameterInMm( symbol, "Slope"+(i+1).ToString(), slope );
					SetElementParameterInMm( symbol, "EL"+(i+1).ToString(), el );
				}
				tr.Commit();
			}
		
			
			using (Transaction tr= new Transaction(doc, "CreatePlinthInstance"))
			{
				tr.Start();
				instance = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, symbol);  
				placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(instance);
		    
	            //IntersectionResult ir = mc.GeometryCurve.Project(new XYZ(E, N, Z));
	
	            int i = 0;
	            foreach (ElementId id in placePointIds)
	            {
	                var t = StartChainage + i * interval;
	                var point = doc.GetElement(id) as ReferencePoint;
	                var ploc = new PointLocationOnCurve(PointOnCurveMeasurementType.SegmentLength, m2ft(t),
	                                                    PointOnCurveMeasureFrom.Beginning);
	                var peref = uiapp.Application.Create.NewPointOnEdge(mc.GeometryCurve.Reference, ploc);
	                point.SetPointElementReference(peref);
	                i++;
	            }
				tr.Commit();
			}
			
		}

		
		private double ft2m(double ft)
		{
			return UnitUtils.Convert( ft, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_METERS);
		}
		
		private double m2ft(double m)
		{
			return UnitUtils.Convert( m, DisplayUnitType.DUT_METERS, DisplayUnitType.DUT_DECIMAL_FEET);
		}
		
	    public void SetElementParameterInMm(Element e, string parameter_name, double lengthInMm )
	    {
	    	e.GetParameters( parameter_name ).First()
	        .Set( UnitUtils.Convert( lengthInMm, DisplayUnitType.DUT_MILLIMETERS,DisplayUnitType.DUT_DECIMAL_FEET) );
	    }

		public FamilySymbol GetSymbol(Document document, string familyName, string symbolName)
        {
            return new FilteredElementCollector(document).OfClass(typeof(Family)).OfType<Family>().FirstOrDefault(f => f.Name.Equals(familyName))
            				.GetFamilySymbolIds().Select(id => document.GetElement(id)).OfType<FamilySymbol>().FirstOrDefault(symbol => symbol.Name.Equals(symbolName));
        }
		
		public FamilySymbol GetSymbol(Document document, string familyName)
        {
            return new FilteredElementCollector(document).OfClass(typeof(Family)).OfType<Family>().FirstOrDefault(f => f.Name.Equals(familyName))
            				.GetFamilySymbolIds().Select(id => document.GetElement(id)).OfType<FamilySymbol>().FirstOrDefault();
        }

		//Create new Type of track element
		private FamilySymbol CreateNewElement(UIApplication uiapp, FamilySymbol oldType, string PlinthNo, double StartChainage, double EndChainage, int ptNum, TrackCenterLine tcl1)
	    {
			FamilySymbol sym = oldType.Duplicate("002") as FamilySymbol;
			
			
			return sym;
	    }
		
		public XYZ getprojectPosition(UIApplication uiapp)
		{
			Document doc = uiapp.ActiveUIDocument.Document;
			XYZ pt=null;
		    foreach( ProjectLocation location
		      in doc.ProjectLocations )
		    {
		      ProjectPosition projectPosition
		        = location.GetProjectPosition( XYZ.Zero );
		      pt=new XYZ(ft2m(projectPosition.EastWest), ft2m(projectPosition.NorthSouth), ft2m(projectPosition.Elevation));
			}

			return pt;
		}

	}
	
	public class ModelLineSelectionFilter : ISelectionFilter
	{
	    public bool AllowElement(Element element)
	    {
	        if (element.Category.Name == "線")
	        {
	            return true;
	        }
	        return false;
	    }
	
	    public bool AllowReference(Reference refer, XYZ point)
	    {
	        return false;
	    }
	}		

}
