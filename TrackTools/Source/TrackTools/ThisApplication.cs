/*
 * Created by SharpDevelop.
 * User:  Jack Tsao
 * Date: 2019/3/8
 * Time: 上午 11:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
//using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.ExtensibleStorage;
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
using Microsoft.VisualBasic;

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
//			TrackCenterLine tcl2 = new TrackCenterLine("ALDName2");
//			TrackCenterLine tcl3 = new TrackCenterLine("ALDName3");
			double[] a;
			string s="";
//			double p=1500;
			for(double p=0; p<=1100; p+=10)
			{
				a=tcl1.Getxyz(p,0);
				s=s+string.Format("{0:0000}, X={1:f5}, Y={2:f5}, Z={3:f5}.\n", p, a[0],a[1],a[2]);
			}
			TaskDialog.Show("GetXYZ",s);
			
//			Transaction transaction = new Transaction( uidoc.Document);
//     		transaction.Start( "Draw Line Patterns or Weights" );	
//
//	     		DrawLines myThis = new DrawLines();
//				if(Yes)	myThis._99_DrawLinePatterns(true, false, uidoc);
//				if(!Yes)	myThis._99_DrawLinePatterns(false, false, uidoc);
//  	  		transaction.Commit();    
	

		}

		public void GetpwTest()
		{
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
			
			X.Application excel = (X.Application)Marshal.GetActiveObject("Excel.Application");
			if( null == excel )
			{
				
				TaskDialog.Show("Getpw", "Failed to get or start Excel." );
				
			}
			
			X.Worksheet worksheet = (X.Worksheet) excel.ActiveSheet;
			double E, N;
			double[] PW;
			int i = excel.ActiveCell.Row;
			while((worksheet.Cells[i,3] as X.Range).Text as string != "")
			{
				E = (double)(worksheet.Cells[i,3] as X.Range).Value2;
				N = (double)(worksheet.Cells[i,4] as X.Range).Value2;
				PW = tcl1.Getpw(E, N);
				worksheet.Cells[i,5] = PW[0];
				worksheet.Cells[i,6] = PW[1];
				
				E = (double)(worksheet.Cells[i,7] as X.Range).Value2;
				N = (double)(worksheet.Cells[i,8] as X.Range).Value2;
				PW = tcl1.Getpw(E, N);
				worksheet.Cells[i,9] = PW[0];
				worksheet.Cells[i,10] = PW[1];
				i++;
			} 
			
			//s=(worksheet.Cells[i,1] as X.Range).SpecialCells(X.XlCellType.xlCellTypeLastCell).Row.ToString();
			
			//TaskDialog.Show("Getpw", s);
				
//			double[] a;
//			for(double p=0; p<=1100; p+=10)
//			{
//				a=tcl1.Getas(p);
//				s=s+string.Format("{0:0000}, A={1:f5}, S={2:f5}.\n", p, a[0],a[1]);
//			}
//			TaskDialog.Show("GetXYZ",s);
		}

		public void GetCantTest()
		{
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
			
			X.Application excel = (X.Application)Marshal.GetActiveObject("Excel.Application");
			if( null == excel )
			{
				
				TaskDialog.Show("Getpw", "Failed to get or start Excel." );
				
			}
			
			X.Worksheet worksheet = (X.Worksheet) excel.ActiveSheet;
			double ch1;
			int i = excel.ActiveCell.Row;
			while((worksheet.Cells[i,1] as X.Range).Text as string != "")
			{
				ch1 = (double)(worksheet.Cells[i,1] as X.Range).Value2;
				worksheet.Cells[i,2] = tcl1.getAppliedCant(ch1, true);
				i++;
			} 
			
		}
		
		public void GetCurvePoints(Curve spline)
		{
			X.Application excel = (X.Application)Marshal.GetActiveObject("Excel.Application");
			if( null == excel )
			{
				
				TaskDialog.Show("Getpw", "Failed to get or start Excel." );
				
			}
			
			X.Worksheet worksheet = (X.Worksheet) excel.ActiveSheet;
			XYZ pl = getprojectPosition();
			
			int i = excel.ActiveCell.Row;
			for (double ch=0; ch < ft2m(spline.GetEndParameter(1)); ch+=1) {
				XYZ pt=spline.Evaluate(m2ft(ch),false);
				XYZ pt100m=new XYZ(ft2m( pt.X+ pl.X) ,ft2m( pt.Y+ pl.Y) , 0);
				
				worksheet.Cells[i,1] = ch;
				worksheet.Cells[i,3] = pt100m.X;
				worksheet.Cells[i,4] = pt100m.Y;
				worksheet.Cells[i,7] = pt100m.X;
				worksheet.Cells[i,8] = pt100m.Y;
				i++;
				
			}
		}
		
		public void GetCurveEl()
		{
			Document doc = ActiveUIDocument.Document;
			ModelCurve mc = doc.GetElement(GetActiveMC()) as ModelCurve ;
			XYZ pl = getprojectPosition();
			XYZ pt= mc.GeometryCurve.Evaluate(0,false);
		}

		private double ft2m(double ft)
		{
			return UnitUtils.Convert( ft, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_METERS);
		}
		
		private double m2ft(double m)
		{
			return UnitUtils.Convert( m, DisplayUnitType.DUT_METERS, DisplayUnitType.DUT_DECIMAL_FEET);
		}
		private double mm2ft(double mm)
		{
			return UnitUtils.Convert( mm, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET);
		}
		
		private double cms2fts(double cms) //cm^2 to ft^2
		{
			return UnitUtils.Convert( cms, DisplayUnitType.DUT_SQUARE_CENTIMETERS, DisplayUnitType.DUT_SQUARE_FEET);
		}

		private double fts2cms(double fts) //ft^2 to cm^2
		{
			return UnitUtils.Convert( fts, DisplayUnitType.DUT_SQUARE_FEET, DisplayUnitType.DUT_SQUARE_CENTIMETERS);
		}
		
		public void Getas()
		{
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
			double[] a;
			string s="";
			for(double p=0; p<=1100; p+=10)
			{
				a=tcl1.Getas(p);
				s=s+string.Format("{0:0000}, A={1:f5}, S={2:f5}.\n", p, a[0],a[1]);
			}
			TaskDialog.Show("GetXYZ",s);
		}
		
		public void GetOneXYZ(/*UIDocument uidoc*/)
		{
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
//			TrackCenterLine tcl2 = new TrackCenterLine("ALDName2");
//			TrackCenterLine tcl3 = new TrackCenterLine("ALDName3");
			double[] a;
			string s="";
			double p=280;
//			for(double p=0; p<=1590; p+=10)
//			{
				a=tcl1.Getxyz(p,0);
				s=s+string.Format("{0:0000}, X={1:f5}, Y={2:f5}, Z={3:f5}.\n", p, a[0],a[1],a[2]);
//			}
			TaskDialog.Show("GetXYZ",s);
			
//			Transaction transaction = new Transaction( uidoc.Document);
//     		transaction.Start( "Draw Line Patterns or Weights" );	
//
//	     		DrawLines myThis = new DrawLines();
//				if(Yes)	myThis._99_DrawLinePatterns(true, false, uidoc);
//				if(!Yes)	myThis._99_DrawLinePatterns(false, false, uidoc);
//  	  		transaction.Commit();    
	

		}
		
		public void TestXSD()
		{
			
		}
		
		public void TestBool()
		{
			bool b = GetbFitRailLength();
		}

		public void testGetDataStorage()
	    {
			UIDocument uidoc = ActiveUIDocument;
			Document doc = uidoc.Document;
			string message="";
			// Retrieve data storage
			
			FilteredElementCollector collector =
			  new FilteredElementCollector( doc );
			
			var dataStorage =
			  collector
			  .OfClass( typeof( DataStorage ) )
			  .FirstElement();
			
			if( dataStorage == null )
			{
			message = "No data storage found "
			  + "in current project";
			
			}
			
			// Retrieve entity from the data storage
			
			Entity createdInfoEntity = dataStorage.GetEntity( 
			CreatedInfoSchema.GetSchema() );
			
			if( !createdInfoEntity.IsValid() )
			{
			message = "Data storage doesn't "
			  + "have CreatedInfoSchema";
			
			}
			
			var createdUser = createdInfoEntity.Get<string>( 
			"CreatedUser" );
			
			var createdDate = createdInfoEntity.Get<string>( 
			"CreatedDate" );
			
			StringBuilder sb = new StringBuilder();
			
			sb.AppendFormat( "Created user: {0}\r\n", 
			createdUser );
			
			sb.AppendFormat( "Created date: {0}", 
			createdDate );
			
			TaskDialog.Show( "Project created info" + message, 
			sb.ToString() );
			
	    }
		
		public void testGetMCid()
		{
			string a;
			a = GetMCidByDataStorage("R03UH.ALD");
			a = GetMCidByDataStorage("R03DH.ALD");
		}

		public string GetMCidByDataStorage(string ALDName)
	    {
			UIDocument uidoc = ActiveUIDocument;
			Document doc = uidoc.Document;
			// Retrieve data storage
			
			FilteredElementCollector collector =
			  new FilteredElementCollector( doc );
			
			var dataStorage =
			  collector
			  .OfClass( typeof( DataStorage ) )
				.Where(q=>q.GetEntity(CreatedMCidSchema.GetSchema()).IsValid())
				.Where(q=>q.GetEntity(CreatedMCidSchema.GetSchema()).Get<string>("ALDName" ) == ALDName).First();
			
			if( dataStorage == null )
			{
				return "";
			}
						
			return dataStorage.GetEntity(CreatedMCidSchema.GetSchema()).Get<string>("MCid" );
	    }


		
		public void testGetMCidByDataStorage()
	    {
			UIDocument uidoc = ActiveUIDocument;
			Document doc = uidoc.Document;
			string message="";
			// Retrieve data storage
			
			FilteredElementCollector collector =
			  new FilteredElementCollector( doc );
			
			var dataStorage =
			  collector
			  .OfClass( typeof( DataStorage ) )
				.Where(q=>q.GetEntity(CreatedMCidSchema.GetSchema()).IsValid())
				.Where(q=>q.GetEntity(CreatedMCidSchema.GetSchema()).Get<string>("ALDName" )=="R03DH.ALD").First();
			
			if( dataStorage == null )
			{
			message = "No data storage found "
			  + "in current project";
			
			}
			
			// Retrieve entity from the data storage
			
			Entity createdInfoEntity = dataStorage.GetEntity( 
			CreatedMCidSchema.GetSchema() );
			
			if( !createdInfoEntity.IsValid() )
			{
			message = "Data storage doesn't "
			  + "have CreatedInfoSchema";
			
			}
			
			var ALDName = createdInfoEntity.Get<string>( 
			"ALDName" );
			
			var MCid = createdInfoEntity.Get<string>( 
			"MCid" );
			
			StringBuilder sb = new StringBuilder();
			
			sb.AppendFormat( "ALDName: {0}\r\n", 
			ALDName );
			
			sb.AppendFormat( "MCid: {0}", 
			MCid );
			
			TaskDialog.Show( "Project created info" + message, 
			sb.ToString() );
			
	    }

		public void SaveMCidByDataStorage(string ALDName, string MCid)
		{
			Document doc = this.ActiveUIDocument.Document;
			try {
				FilteredElementCollector collector =
			  new FilteredElementCollector( doc );
			
			var dataStorage =
			  collector
			  .OfClass( typeof( DataStorage ) )
				.Where(q=>q.GetEntity(CreatedMCidSchema.GetSchema()).IsValid())
				.Where(q=>q.GetEntity(CreatedMCidSchema.GetSchema()).Get<string>("ALDName") == ALDName).First();
			
				if( dataStorage != null )
				{
//					dataStorage.GetEntity(CreatedMCidSchema.GetSchema()).Set("MCid", MCid );
//					return ;			
				using( Transaction t = new Transaction( doc, "Delete Data" ) )
			  	{
				    t.Start();
				    doc.Delete(dataStorage.Id);
				    t.Commit();
				}
				}
					
			} catch (System.InvalidOperationException) {}			
				using( Transaction t = new Transaction( doc, "Create created MCid" ) )
			  	{
				    t.Start();
				 
				    // Create data storage in new document
				 
				    DataStorage createdInfoStorage 
				      = DataStorage.Create( doc );
				 
				    // Create entity which store created info
				 
				    Entity entity = new Entity( 
				      CreatedMCidSchema.GetSchema() );
				 
				    entity.Set( "ALDName", 
				      ALDName );
				 
				    entity.Set( "MCid", 
				      MCid );
				 
				    // Set entity to the data storage element
				 
				    createdInfoStorage.SetEntity( entity );
				 
				    t.Commit();
			
			}
		
		}
	
		public void TestMCidByDataStorage()
		{
			Document doc = this.ActiveUIDocument.Document;
			using( Transaction t = new Transaction( doc, "Create created MCid" ) )
		  	{
			    t.Start();
			 
			    // Create data storage in new document
			 
			    DataStorage createdInfoStorage 
			      = DataStorage.Create( doc );
			 
			    // Create entity which store created info
			 
			    Entity entity = new Entity( 
			      CreatedMCidSchema.GetSchema() );
			 
			    entity.Set( "ALDName", 
			      "R03UH.ALD" );
			 
			    entity.Set( "MCid", 
			      "1144957" );
			 
			    // Set entity to the data storage element
			 
			    createdInfoStorage.SetEntity( entity );
			 
			    t.Commit();
		  	}			
		}
		
		
		public void TestDataStorage()
		{
			Document doc = this.ActiveUIDocument.Document;
			using( Transaction t = new Transaction( doc, "Create created info" ) )
		  	{
			    t.Start();
			 
			    // Create data storage in new document
			 
			    DataStorage createdInfoStorage 
			      = DataStorage.Create( doc );
			 
			    // Create entity which store created info
			 
			    Entity entity = new Entity( 
			      CreatedInfoSchema.GetSchema() );
			 
			    entity.Set( "CreatedUser", 
			      Environment.UserName );
			 
			    entity.Set( "CreatedDate", 
			      DateTime.Now.ToString() );
			 
			    // Set entity to the data storage element
			 
			    createdInfoStorage.SetEntity( entity );
			 
			    t.Commit();
		  	}			
		}
		
		public void Test()
		{
			Document doc = this.ActiveUIDocument.Document;
			Transaction tr= new Transaction(doc, "ReferencePoint");
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
			double[] a,b;
			ReferencePoint rp;
			b=tcl1.Getxyz(0,0);
			tr.Start();
			for(double p=0; p<=1100; p+=10)
			{
				a=tcl1.Getxyz(p, 0);
				rp = doc.FamilyCreate.NewReferencePoint(new XYZ(a[0]-b[0],a[1]-b[1],a[2]-b[2]));
			}
			tr.Commit();
		}
		
		public void TestBasePoint()
		{
			XYZ pt=getprojectPosition();
			TaskDialog.Show("TestBasePoint", pt.X.ToString()+", "+ pt.Y.ToString()+", "+ pt.Z.ToString());
						
		}
		
		public XYZ getprojectPosition()
		{
			Document doc = this.ActiveUIDocument.Document;
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

		public void setprojectPosition()
		{
			Document doc = this.ActiveUIDocument.Document;
			XYZ pt=new XYZ(-m2ft(2765*100),-m2ft(27738*100),-m2ft(100));
			Transaction tr= new Transaction(doc, "SetProjectPosition");
			tr.Start();
		    foreach( ProjectLocation location
		      in doc.ProjectLocations )
		    {
		      ProjectPosition projectPosition
		        = location.GetProjectPosition( XYZ.Zero );
//		      projectPosition.EastWest = pt.X;
//		      projectPosition.NorthSouth = pt.Y;
//		      projectPosition.Elevation = pt.Z;
		      //SiteLocation siteLocation =
		      	location.SetProjectPosition(pt, projectPosition);
//		        location.SetSiteLocation( pt, projectPosition);
			}
			tr.Commit();

		}

		
		
		//Minimum points to draw spiral curve by doc.FamilyCreate.NewCurveByPoints, i.e. Hermit Spline
		public void TestSpiralCurve()
		{
			Document doc = this.ActiveUIDocument.Document;
			Transaction tr= new Transaction(doc, "ReferencePoint");
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
			double[] a,b;
			ReferencePoint rp;
			b=tcl1.Getxyz(885,0);
			string s="";
			tr.Start();
			for (int N = 1; N < 71; N++) {
				double ch=885;
				ReferencePointArray rpa = new ReferencePointArray();
				double interval= 70.0/N;
				for(int i=0; i<=N; i++)
				{
					a=tcl1.Getxyz(ch, 0);
					rp = doc.FamilyCreate.NewReferencePoint(new XYZ(a[0]-b[0]+N,a[1]-b[1],0.0));
					rpa.Append(rp);
					ch+=interval;
				}
				CurveByPoints curve = doc.FamilyCreate.NewCurveByPoints(rpa);
				s=s+string.Format("N={0:00}, Ls={1:f5}.\n", N, curve.GeometryCurve.Length);				
			}
			tr.Commit();
			TaskDialog.Show("Test",s);
		}
		
		public void AddTrackCenterline()
		{
			Document doc = this.ActiveUIDocument.Document;
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
			List <XYZ> pts = tcl1.Getxy();
			pts = TransformAndConvert(pts);
			ModelHermiteSpline spline = createModelHermitSpline(pts);
		    
			//Save ID to DataStorage.
			string ALDName = Interaction.GetSetting("AlignmentTools","AppData","ALDName1","Y06UH.ALD");
			SaveMCidByDataStorage(ALDName, spline.Id.ToString());
			
		}
		
		private List <XYZ> TransformAndConvert(List <XYZ> pts)
		{
			List <XYZ> ptc = new List<XYZ>();
			XYZ pl = getprojectPosition();
			foreach (XYZ pt in pts) {
					ptc.Add(new XYZ(UnitUtils.Convert( pt.X - pl.X, DisplayUnitType.DUT_METERS, DisplayUnitType.DUT_DECIMAL_FEET),
				               UnitUtils.Convert( pt.Y - pl.Y, DisplayUnitType.DUT_METERS, DisplayUnitType.DUT_DECIMAL_FEET), 0));
			}
			
			return ptc;
		}
		
		
		public void AddPlinth(FamilySymbol TempFamily)
		{			
			Document revitDoc = this.ActiveUIDocument.Document;
			Application revitApp = this.Application;
			FamilySymbol familySymbol;
			FamilyInstance famIns1;
			ReferencePoint point;
			ModelCurve modelCurve=null;
			string FamilyPath_first="";
			Transaction tr= new Transaction(revitDoc, "AddPlinth");

				bool modelCurveZhu = revitDoc.LoadFamilySymbol(FamilyPath_first, System.IO.Path
					                            .GetFileNameWithoutExtension(FamilyPath_first), out familySymbol);   //这里的族一定要有类型才行
                familySymbol.Activate();   //激活族类型
				famIns1 = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(revitDoc, familySymbol);   //自适应族实例化
                IList<ElementId> placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(famIns1);
                point = revitDoc.GetElement(placePointIds[0]) as ReferencePoint;                //自适应构件族的自适应点
                point.Position = modelCurve.GeometryCurve.GetEndPoint(0); 


                PointLocationOnCurve pointLocationOnCurve00 = new PointLocationOnCurve(PointOnCurveMeasurementType.NormalizedCurveParameter, 
                    0, PointOnCurveMeasureFrom.Beginning);
                PointOnEdge poe00 = revitApp.Create.NewPointOnEdge(modelCurve.GeometryCurve.Reference, pointLocationOnCurve00);  //将自适应点和模型线关联起来
                point.SetPointElementReference(poe00 as PointElementReference);


			tr.Commit();
			
			
		}
		
		public void BuildPlinth()
		{
			Document doc = this.ActiveUIDocument.Document;
			// find the first placement point that will host the adaptive component
			IEnumerable<ReferencePoint> points0 = from obj in new FilteredElementCollector(doc).OfClass(typeof(ReferencePoint)).Cast<ReferencePoint>()
			    let type = obj as ReferencePoint
			    where type.Name == "PlacementPoint0" // these names were manually assigned to the points
			    select obj;
			ReferencePoint placementPoint0 = points0.First();
			
			// find the 2nd placement point that will host the adaptive component
			IEnumerable<ReferencePoint> points1 = from obj in new FilteredElementCollector(doc).OfClass(typeof(ReferencePoint)).Cast<ReferencePoint>()
			    let type = obj as ReferencePoint
			    where type.Name == "PlacementPoint1"
			    select obj;
			ReferencePoint placementPoint1 = points1.First();
			
			FamilySymbol symbol;
			string FamilyPath_first="";
			Transaction tr= new Transaction(doc, "BuildPlinth");
			tr.Start();
			bool PlinthZhu = doc.LoadFamilySymbol(FamilyPath_first, System.IO.Path
				                            .GetFileNameWithoutExtension(FamilyPath_first), out symbol);   //这里的族一定要有类型才行
            symbol.Activate();   //激活族类型
			
			if (AdaptiveComponentInstanceUtils.IsAdaptiveFamilySymbol(symbol))
			{
			    // create an instance of the adaptive component
			    FamilyInstance familyInstance = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, symbol);
			
			    // find the adaptive points in the family instance
			    IList<ElementId> pointList = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(familyInstance);
			    ReferencePoint point0 = doc.GetElement(pointList.ElementAt(0)) as ReferencePoint;
			    ReferencePoint point1 = doc.GetElement(pointList.ElementAt(1)) as ReferencePoint;
			
			    // move the adaptive component's points (point0 & point1) to match the position of the placement points
			    point0.Position = placementPoint0.Position;
			    point1.Position = placementPoint1.Position;
			}	
			tr.Commit();
		}
		
		public void DeleteParametrs()
		{
			Document doc = ActiveUIDocument.Document;
			FamilyParameterSet fps = doc.FamilyManager.Parameters;
			using (Transaction tr = new Transaction(doc, "DeleteParameter")) {
				tr.Start();
				foreach (FamilyParameter fp in fps) 
				{
					try {
						
						doc.FamilyManager.RemoveParameter(fp);
							
					} catch (Autodesk.Revit.Exceptions.ArgumentException) {
						
						//throw;
					}
				}
				tr.Commit();
			}
		}
		
		public void deleteAllElement()
		{
			Document doc = ActiveUIDocument.Document;
			ElementClassFilter f1 = new ElementClassFilter(typeof(FamilySymbol));
			ElementClassFilter f2 = new ElementClassFilter(typeof(Form));
			ElementCategoryFilter f3 = new ElementCategoryFilter(BuiltInCategory.OST_AdaptivePoints);
			LogicalOrFilter f4 = new LogicalOrFilter(f1, f2);
			LogicalOrFilter f5 = new LogicalOrFilter(f4, f3);
		    FilteredElementCollector collector = new FilteredElementCollector( doc );
		    ICollection<ElementId> allElemants = collector.WherePasses(f5).ToElementIds();
			using (Transaction tr = new Transaction(doc, "deleteAllElement"))
			{
				tr.Start();
				for (int i = 0; i <  allElemants.Count; i++) {					
					try {
						doc.Delete(allElemants.ElementAt(i));						
					} catch (Exception) {
					}				
				}
	    		tr.Commit();
			}			
		}
		
		public void CreateFamilySymbol()
		{
			Document doc = ActiveUIDocument.Document;
			FamilyOption opt = new FamilyOption();
			Family f = null;
			Document fDoc= null;
			ElementClassFilter f1 = new ElementClassFilter(typeof(Family));
		    FilteredElementCollector collector = new FilteredElementCollector( doc );
		    IEnumerable<Element> allElemants = collector.WherePasses(f1).Where(q=>(q.Name.Equals("Left") || q.Name.Equals("Right")));
			for(int i=0; i < allElemants.Count(); i++) {
				f = allElemants.ElementAt(i) as Family;
				fDoc = doc.EditFamily(f);
				string path = Path.GetTempPath();
				string name = f.Name;
				string fName = name + ".rfa";
				string fPath = path + fName;
				using (Transaction tr = new Transaction(fDoc, "CreateFamilySymbol"))
				{

					tr.Start();	
						try {
						fDoc.FamilyManager.NewType("0");
						fDoc.LoadFamily(doc, opt);
						} catch (Autodesk.Revit.Exceptions.ArgumentException) {							
//						TaskDialog.Show("Hey", "Error");
						}
		    		tr.Commit();
				}
				fDoc.SaveAs( fPath );
				System.IO.File.Delete(fPath);
				fDoc.Close(false);
				f = null;
				fDoc = null;				
			}			
		}
		
		
		public void deleteFamilyType()
		{
			Document doc = ActiveUIDocument.Document;
            UIDocument uidoc = ActiveUIDocument;
            
		    FamilyTypeSet familyTypes= doc.FamilyManager.Types;
		    int fts = familyTypes.Size;
			using (Transaction tr = new Transaction(doc, "DeleteFamilyType"))
			{
				tr.Start();
				    while(fts>1)
		            {
		            	doc.FamilyManager.DeleteCurrentType();
						fts--;		            	
		            }
				    doc.FamilyManager.RenameCurrentType("001");
				tr.Commit();
			}			
		}
		
		public void ClearTrackFamily()
		{
			DeleteParametrs();
			deleteAllElement();
			CreateFamilySymbol();
		}

		// Generate 3 pts through n-1 pts from n pts track adaptive component
		// i.e. 6pt.rfa --> 5pt.rfa --> 4pt.rfa -->3pt.rfa 
		public void GenerateComponentFamilies()
		{
			Document doc = ActiveUIDocument.Document;
            UIDocument uidoc = ActiveUIDocument;
            
            string fileName = System.IO.Path.GetFileNameWithoutExtension(doc.PathName);
            int ptNum = Utility.GetReferencePointQuantity(doc);
            string pathName = System.IO.Path.GetDirectoryName(doc.PathName)+"\\" + fileName +"-"+ ptNum.ToString()+"pt.rfa";
           	doc.SaveAs(pathName);
           	while(ptNum > 1)
           	{
				IEnumerable<FamilySymbol> fis = Utility.GetFamilySymbolByType(doc, ptNum.ToString());
				IEnumerable<ReferencePoint> rps = Utility.GetReferencePoint(doc, ptNum);
				//Selection sel = null;
				List<ElementId> eIds = new List<ElementId>();
				foreach (FamilySymbol e in fis) {
					eIds.Add(e.Id);				
				}
				foreach (ReferencePoint e in rps) {
					eIds.Add(e.Id);				
				}
				//sel.SetElementIds(eIds);
				using (Transaction tr = new Transaction(doc, "DeleteFamilyInstance"))
				{
					tr.Start();
					doc.Delete(eIds);
					tr.Commit();
				}
				
				IEnumerable<Form> fs = Utility.GetForm(doc);
				Form f1 = fs.ElementAt(0);
				Form f2=null;
				if (fs.Count() == 2)
					f2 = fs.ElementAt(1);
				using (Transaction tr= new Transaction(doc, "DeleteProfile"))
				{
					tr.Start();	
					ReferenceArray ref_Array = f1.get_CurveLoopReferencesOnProfile(0, 0);
					int pclc=f1.get_ProfileCurveLoopCount(0);
					Reference loft_ref = ref_Array.get_Item( 0 );
					var line = doc.GetElement(loft_ref.ElementId);
					if (fs.Count() == 1)
						f1.DeleteProfile(0);
//						f1.DeleteProfile(ptNum-1);
					if (fs.Count() == 2)
					{
						f1.DeleteProfile(0);
						f2.DeleteProfile(0);
//						f2.DeleteProfile(ptNum-1);
					}
					
					if (doc.FamilyManager.get_Parameter("C"+ptNum.ToString()) != null)
					doc.Delete(doc.FamilyManager.get_Parameter("C"+ptNum.ToString()).Id);
					if (doc.FamilyManager.get_Parameter("Depth"+ptNum.ToString()) != null)
					doc.Delete(doc.FamilyManager.get_Parameter("Depth"+ptNum.ToString()).Id);
					if (doc.FamilyManager.get_Parameter("Slope"+ptNum.ToString()) != null)
					doc.Delete(doc.FamilyManager.get_Parameter("Slope"+ptNum.ToString()).Id);
					if (doc.FamilyManager.get_Parameter("EL"+ptNum.ToString()) != null)
					doc.Delete(doc.FamilyManager.get_Parameter("EL"+ptNum.ToString()).Id);
					if (doc.FamilyManager.get_Parameter("H"+ptNum.ToString()) != null)
					doc.Delete(doc.FamilyManager.get_Parameter("H"+ptNum.ToString()).Id);
					if (doc.FamilyManager.get_Parameter("W"+ptNum.ToString()) != null)
					doc.Delete(doc.FamilyManager.get_Parameter("W"+ptNum.ToString()).Id);
					if (doc.FamilyManager.get_Parameter("Side"+ptNum.ToString()) != null)
					doc.Delete(doc.FamilyManager.get_Parameter("Side"+ptNum.ToString()).Id);
					tr.Commit();
				}
				pathName = System.IO.Path.GetDirectoryName(doc.PathName)+"\\" + fileName +"-"+ (ptNum-1).ToString()+"pt.rfa";
	           	doc.SaveAs(pathName);
				ptNum--;
           	}
		}
		public void createTrackFamily()
		{
			//使用步驟：
			// 1.新建族群, 使用"公制自適應通用模型.rft"
			// 2.載入左右(Left.rfa, Right.rfa)族群，族群中必需有"0"的類型。(左右分開時, 則只載入其中一個, 建立完成後使用相同步驟建立另一個。)
			// 3.執行此程式以建立自適應元件。
			// 4.存檔。檔名為軌道元件名稱，如:Plinth, RightEN60E1, LeftEN60E1.
			
			
			Document doc = ActiveUIDocument.Document;
			
			Transaction transaction = new Transaction(doc);
			if (doc.FamilyManager.CurrentType.Name != "001"){ 					
				    transaction.SetName("NewType");
				    transaction.Start();
	//				doc.FamilyManager.NewType("001");
					doc.FamilyManager.RenameCurrentType("001");
					transaction.Commit();
			}
			FamilySymbol fs1, fs2, fs=null;
			FamilyParameter C=null, EL=null, Depth=null, Slope=null, H=null, W=null, Side=null;
			fs1 = GetSymbol(doc, "Right");
			fs2 = GetSymbol(doc, "Left");
			if (fs1 !=null) fs=fs1;
			if (fs2 !=null) fs=fs2;
			string strptNum = Interaction.InputBox("請輸入要產生的自適應元件的斷面數量", "軌道自適應元件製造機","6",100,100);
			for (int i = 0; i < int.Parse(strptNum); i++)
			{
			    transaction.SetName("Point" + i);
			    transaction.Start();
					if (fs.GetParameters("C").Count != 0)
				    C = doc.FamilyManager.AddParameter("C" + (i+1).ToString(), BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length,false);
					if (fs.GetParameters("EL").Count != 0)
				    EL = doc.FamilyManager.AddParameter("EL" + (i+1).ToString(), BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length,false);
					if (fs.GetParameters("Depth").Count != 0)
				    Depth = doc.FamilyManager.AddParameter("Depth" + (i+1).ToString(), BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length,false);
					if (fs.GetParameters("Slope").Count != 0)
				    Slope = doc.FamilyManager.AddParameter("Slope" + (i+1).ToString(), BuiltInParameterGroup.INVALID, ParameterType.Number,false);
					if (fs.GetParameters("H").Count != 0)
				    H = doc.FamilyManager.AddParameter("H" + (i+1).ToString(), BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length,false);
					if (fs.GetParameters("W").Count != 0)
				    W = doc.FamilyManager.AddParameter("W" + (i+1).ToString(), BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length,false);
					if (fs.GetParameters("Side").Count != 0)
				    Side = doc.FamilyManager.AddParameter("Side" + (i+1).ToString(), BuiltInParameterGroup.INVALID, ParameterType.Integer,false);
					if (fs.GetParameters("C").Count != 0)
					doc.FamilyManager.SetValueString(C, "0");
					if (fs.GetParameters("EL").Count != 0)
					doc.FamilyManager.SetValueString(EL, "800");
					if (fs.GetParameters("Depth").Count != 0)
					doc.FamilyManager.SetValueString(Depth, "700");
					if (fs.GetParameters("Slope").Count != 0)
					doc.FamilyManager.Set(Slope, 0);
					if (fs.GetParameters("H").Count != 0)
					doc.FamilyManager.SetValueString(H, "60");
					if (fs.GetParameters("W").Count != 0)
					doc.FamilyManager.SetValueString(W, "0");
					if (fs.GetParameters("Side").Count != 0)
					doc.FamilyManager.Set(Side, 1);
					
				    ReferencePoint referencePoint = doc.FamilyCreate.NewReferencePoint(new XYZ(0, UnitUtils.Convert(-i*1000, DisplayUnitType.DUT_MILLIMETERS,DisplayUnitType.DUT_DECIMAL_FEET), 0));
			        AdaptiveComponentFamilyUtils.MakeAdaptivePoint(doc, referencePoint.Id, AdaptivePointType.PlacementPoint);
			        AdaptiveComponentFamilyUtils.SetPlacementNumber(doc, referencePoint.Id, i+1);
			        AdaptiveComponentFamilyUtils.SetPointOrientationType(doc, referencePoint.Id, AdaptivePointOrientationType.ToGlobalZthenHost);
			        referencePoint.CoordinatePlaneVisibility=CoordinatePlaneVisibility.Always;
			        
					
			        if (fs1 != null)
			        {
						fs1 = CreateNewType( fs1,i*20,400,800, i+1);
						if (fs1.GetParameters("C").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("C").First(), C);
						if (fs1.GetParameters("EL").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("EL").First(), EL);
						if (fs1.GetParameters("Depth").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("Depth").First(), Depth);
						if (fs1.GetParameters("Slope").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("Slope").First(), Slope);
						if (fs1.GetParameters("H").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("H").First(), H);
						if (fs1.GetParameters("W").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("W").First(), W);
						if (fs1.GetParameters("Side").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("Side").First(), Side);
			        }
			        if (fs2 != null)
			        {
						fs2 = CreateNewType( fs2,i*20,400,800, i+1);		        
						if (fs2.GetParameters("C").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("C").First(), C);
						if (fs2.GetParameters("EL").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("EL").First(), EL);
						if (fs2.GetParameters("Depth").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("Depth").First(), Depth);
						if (fs2.GetParameters("Slope").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("Slope").First(), Slope);
						if (fs2.GetParameters("H").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("H").First(), H);
						if (fs2.GetParameters("W").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("W").First(), W);
						if (fs2.GetParameters("Side").Count != 0)
						doc.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("Side").First(), Side);
			        }
			        if (fs1 != null)
			        {
				        FamilyInstance fi1 = doc.FamilyCreate.NewFamilyInstance(referencePoint.GetCoordinatePlaneReferenceXY(),new XYZ(0, UnitUtils.Convert(-i*1000, DisplayUnitType.DUT_MILLIMETERS,DisplayUnitType.DUT_DECIMAL_FEET), 0),new XYZ(-1,0,0),fs1);
			        }
			        if (fs2 != null)
			        {
			        	FamilyInstance fi2 = doc.FamilyCreate.NewFamilyInstance(referencePoint.GetCoordinatePlaneReferenceXY(),new XYZ(0, UnitUtils.Convert(-i*1000, DisplayUnitType.DUT_MILLIMETERS,DisplayUnitType.DUT_DECIMAL_FEET), 0),new XYZ(-1,0,0),fs2);
			        }
			        
		        transaction.Commit();

			}
			
		}
		
		public void createPlinthFamily()
		{
			Document familyDocument = Application.NewFamilyDocument("C:\\ProgramData\\Autodesk\\RVT 2020\\Family Templates\\Traditional Chinese\\公制自適應通用模型.rft");
			
//			familyDocument
			
			if (null == familyDocument)			
			{			
			    throw new Exception("Cannot open family document");			
			}		
			// check if this family is an Adaptive Component family
			if (!(AdaptiveComponentFamilyUtils.IsAdaptiveComponentFamily(familyDocument.OwnerFamily))) return; 
			
			Transaction transaction = new Transaction(familyDocument); 
			transaction.SetName("LoadFamilySymbol");
			transaction.Start();
			FamilySymbol fs1, fs2;
			familyDocument.LoadFamilySymbol("D:\\Tsao\\Developement\\RevitTrackModeling\\RightPlinth.rfa","Cant 0", out fs1);
			familyDocument.LoadFamilySymbol("D:\\Tsao\\Developement\\RevitTrackModeling\\LeftPlinth.rfa","Cant 0", out fs2);
			familyDocument.FamilyManager.NewType("001");
			transaction.Commit();
			
			ReferenceArrayArray refCurveArray1 = new ReferenceArrayArray();
			ReferenceArrayArray refCurveArray2 = new ReferenceArrayArray();
			for (int i = 0; i < 6; i++)
			{
			    transaction.SetName("Point" + i);
			    transaction.Start();
			    FamilyParameter C = familyDocument.FamilyManager.AddParameter("C" + (i+1).ToString(), BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length,false);
			    FamilyParameter EL =familyDocument.FamilyManager.AddParameter("EL" + (i+1).ToString(), BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length,false);
			    FamilyParameter Depth =familyDocument.FamilyManager.AddParameter("Depth" + (i+1).ToString(), BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length,false);
			    FamilyParameter Slope =familyDocument.FamilyManager.AddParameter("Slope" + (i+1).ToString(), BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length,false);
			    ReferencePoint referencePoint = familyDocument.FamilyCreate.NewReferencePoint(new XYZ(0, UnitUtils.Convert(-i*1000, DisplayUnitType.DUT_MILLIMETERS,DisplayUnitType.DUT_DECIMAL_FEET), 0));
		        AdaptiveComponentFamilyUtils.MakeAdaptivePoint(familyDocument, referencePoint.Id, AdaptivePointType.PlacementPoint);
		        AdaptiveComponentFamilyUtils.SetPlacementNumber(familyDocument, referencePoint.Id, i+1);
		        AdaptiveComponentFamilyUtils.SetPointOrientationType(familyDocument, referencePoint.Id, AdaptivePointOrientationType.ToGlobalZthenHost);
		        referencePoint.CoordinatePlaneVisibility=CoordinatePlaneVisibility.Always;
		        
				fs1 = CreateNewType( fs1,i*20,400,800, i+1);
				fs2 = CreateNewType( fs2,i*20,400,800, i+1);
				
//				familyDocument.FamilyManager.Set(C, (double)i*20);
//				familyDocument.FamilyManager.Set(EL, 800.0);
//				familyDocument.FamilyManager.Set(Depth, 400.0);
//				familyDocument.FamilyManager.SetFormula(Depth, "400");
				familyDocument.FamilyManager.SetValueString(C, (i*20).ToString());
				familyDocument.FamilyManager.SetValueString(EL, "800");
				familyDocument.FamilyManager.SetValueString(Depth, "400");

				familyDocument.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("C").First(), C);
				familyDocument.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("EL").First(), EL);
				familyDocument.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("Depth").First(), Depth);
				familyDocument.FamilyManager.AssociateElementParameterToFamilyParameter(fs1.GetParameters("Slope").First(), Depth);
				familyDocument.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("C").First(), C);
				familyDocument.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("EL").First(), EL);
				familyDocument.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("Depth").First(), Depth);
				familyDocument.FamilyManager.AssociateElementParameterToFamilyParameter(fs2.GetParameters("Slope").First(), Depth);
				
				
		        
		        FamilyInstance fi1 = familyDocument.FamilyCreate.NewFamilyInstance(referencePoint.GetCoordinatePlaneReferenceXY(),new XYZ(0, UnitUtils.Convert(-i*1000, DisplayUnitType.DUT_MILLIMETERS,DisplayUnitType.DUT_DECIMAL_FEET), 0),new XYZ(-1,0,0),fs1);
		        FamilyInstance fi2 = familyDocument.FamilyCreate.NewFamilyInstance(referencePoint.GetCoordinatePlaneReferenceXY(),new XYZ(0, UnitUtils.Convert(-i*1000, DisplayUnitType.DUT_MILLIMETERS,DisplayUnitType.DUT_DECIMAL_FEET), 0),new XYZ(-1,0,0),fs2);
		        
		        transaction.Commit();
		        
		        ReferenceArray curves1=new ReferenceArray(), curves2=new ReferenceArray();
		        GetFamilyInstanceReferenceArray(fi1, ref curves1);
		        GetFamilyInstanceReferenceArray(fi2, ref curves2);
		        refCurveArray1.Append(curves1);
		        refCurveArray2.Append(curves2);
			}
			
//			transaction.SetName("NewLoftForm");
//			transaction.Start();
//			familyDocument.FamilyCreate.NewLoftForm(true, refCurveArray1);
//			familyDocument.FamilyCreate.NewLoftForm(true, refCurveArray2);
//			transaction.Commit();
			
			SaveAsOptions opt = new SaveAsOptions();
			opt.OverwriteExistingFile=true;
			familyDocument.SaveAs("D:\\Tsao\\Developement\\RevitTrackModeling\\6pt.rfa", opt);
//			familyDocument.LoadFamily(ActiveUIDocument.Document);
//			TaskDialog.Show("NewFamilyDocument", familyDocument.PathName);
			familyDocument.Close();
//			System.IO.File.Delete("D:\\Tsao\\Developement\\RevitTrackModeling\\test.rfa");
		}

		public void CreatePlinthInstance()		
		{
			//手動添加需要有模型線才可以使用。
			Document doc = this.ActiveUIDocument.Document;
            UIDocument uidoc = this.ActiveUIDocument;
			
			FamilySymbol symbol;
			string FamilyPath_first="D:\\Tsao\\Developement\\RevitTrackModeling\\6pt.rfa";
			Transaction tr= new Transaction(doc, "CreatePlinthInstance");
			tr.Start();
			bool PlinthZhu = doc.LoadFamilySymbol(FamilyPath_first, System.IO.Path
				                            .GetFileNameWithoutExtension(FamilyPath_first), out symbol);   //这里的族一定要有类型才行
            symbol.Activate();   //激活族类型

            
		    // Create a new instance of an adaptive component family		
		    FamilyInstance instance = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, symbol);
		
		
		
		    // Get the placement points of this instance		
		    IList<ElementId> placePointIds = new List<ElementId>();		
		    placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(instance);		
//		    double x = 0;
		
		
		
		    // Set the position of each placement point		
//		    foreach (ElementId id in placePointIds)		
//		    {		
//		        ReferencePoint point = doc.GetElement(id) as ReferencePoint;		
//		        point.Position = new Autodesk.Revit.DB.XYZ(10*x, 10*Math.Cos(x), 0);		
//		        x += Math.PI/6;		
//		    }
		    
		    //Select the track centerline curve as reference
		    
            
			
			
            //Get current selection and store it
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
            ISelectionFilter selFilter = new ModelLineSelectionFilter();
            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取軌道中心線：");
            

            // Retrieve needed information from reference object
            ModelCurve mc = doc.GetElement(hasPickOne.ElementId) as ModelCurve ;
	    
            //IntersectionResult ir = mc.GeometryCurve.Project(new XYZ(E, N, Z));
            
            int i = 13;
            foreach (ElementId id in placePointIds)
            {
                var t = i*2;
                var point = doc.GetElement(id) as ReferencePoint;
                var ploc = new PointLocationOnCurve(PointOnCurveMeasurementType.NonNormalizedCurveParameter, t,
                                                    PointOnCurveMeasureFrom.Beginning);
                var peref = Application.Create.NewPointOnEdge(mc.GeometryCurve.Reference, ploc);
                point.SetPointElementReference(peref);
                i++;
            }
            tr.Commit();
		}
		public void AddComponents(double StartChainage, ModelCurve mc, TrackCenterLine tcl1)
		{
			int Quantity = int.Parse(Interaction.GetSetting("AlignmentTools","AppData","textQuantity","1"));
            double interval = double.Parse(Interaction.GetSetting("AlignmentTools","AppData","textLength","0.75"));
			for (int i = 0; i < Quantity; i++) {
				AddOneComponent(StartChainage + i*interval,  mc,  tcl1);
			}
		}

		public void AddOneComponent(double StartChainage, ModelCurve mc, TrackCenterLine tcl1)
		{
			string myPath=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\TrackSystem\\" + GetActiveFolder() + "\\" + GetActiveComponent();
			
			//手動添加需要有模型線才可以使用。
			Document doc = ActiveUIDocument.Document;
            UIDocument uidoc = ActiveUIDocument;
			IList<ElementId> placePointIds;
			FamilyOption opt = new FamilyOption();
			FamilySymbol symbol;
			FamilyInstance instance;
			int Quantity = int.Parse(Interaction.GetSetting("AlignmentTools","AppData","textQuantity","1"));
			//Family family;
			
			//檢查族群是否已載入
			string FamilyName = Path.GetFileNameWithoutExtension(myPath);
			
			Family family = Utility.FindElementByName(doc, typeof( Family ), FamilyName ) as Family;
			
 
		    if( null == family )
		    {						
				using( Transaction tr = new Transaction(doc, "CreatePlinthInstance"))
				{
					tr.Start();
					doc.LoadFamilySymbol(myPath, "001", opt, out symbol);
					tr.Commit();
				}
		    }
		    else
		    {
				symbol = GetSymbol(family.Document, FamilyName);		    	
				int sQuantity = GetSymbolQuantity(family.Document, FamilyName);		    	
				using( Transaction tr = new Transaction(doc, "CreateNewElement"))
				{
					tr.Start();
				    symbol = CreateNewElement(symbol, sQuantity);		    
				    tr.Commit();
				}
		    }

			XYZ pl = getprojectPosition();	
			using (Transaction tr= new Transaction(doc, "SetElementParameter"))
			{
				tr.Start();			
			
					var t = StartChainage;
					double cant = Math.Round(tcl1.getAppliedCant(t, true));
					double el = 0, slope = 0;
					double depth = double.Parse(Interaction.GetSetting("AlignmentTools","AppData", "textInvertElevation","400"));
					double H = Math.Round(tcl1.getTunnelOffset(t, true));
					double W = Math.Round(tcl1.getGaugeWidenning(t, true));
					int Side = tcl1.getSideOfWalkway(t);
					if (bool.Parse(Interaction.GetSetting("AlignmentTools","AppData","bFixedElevation","True")))
					{
						el = (double.Parse(Interaction.GetSetting("AlignmentTools","AppData","textElevation","83.800")) - pl.Z) * 1000;
						slope = 0;
					}
					else
					{
						el = (tcl1.Getz(t) - pl.Z) * 1000;
						slope = tcl1.Gets(t);
					}
					
					SetElementParameterInMm( symbol, "C1", cant );
					SetElementParameterDouble( symbol, "Slope1", slope );
					SetElementParameterInMm( symbol, "EL1", el );
					SetElementParameterInMm( symbol, "Depth1", depth );
					SetElementParameterInMm( symbol, "H1", H );
					SetElementParameterInMm( symbol, "W1", W );
					SetElementParameterInt( symbol, "Side1", Side );
					
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
	                var t = StartChainage - tcl1.GetStartLocalChainage();
	                var point = doc.GetElement(id) as ReferencePoint;
	                var ploc = new PointLocationOnCurve(PointOnCurveMeasurementType.SegmentLength, m2ft(t),
	                                                    PointOnCurveMeasureFrom.Beginning);
	                var peref = Application.Create.NewPointOnEdge(mc.GeometryCurve.Reference, ploc);
	                point.SetPointElementReference(peref);
	                i++;
	            }
				tr.Commit();
			}
		}
		
		public void AddOnePlinth(string PlinthNo, double StartChainage, double EndChainage, ModelCurve mc, TrackCenterLine tcl1)		
		{
			
			if ("Component" == GetActiveFolder())
			{
				AddComponents(StartChainage, mc, tcl1);
				return;
			}
			
			string myPath=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\TrackSystem\\" + GetActiveFolder() + "\\" + GetActiveComponent() + "\\";
			
			double PlinthLength = EndChainage - StartChainage;
			//手動添加需要有模型線才可以使用。
			Document doc = ActiveUIDocument.Document;
            UIDocument uidoc = ActiveUIDocument;
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
				    symbol = CreateNewElement(symbol, PlinthNo, StartChainage, EndChainage, ptNum, tcl1);		    
				   	symbol.Name= PlinthNo + "-" + PlinthLength.ToString("0.000");
				    tr.Commit();
				}
		    }

			XYZ pl = getprojectPosition();	
			using (Transaction tr= new Transaction(doc, "SetElementParameter"))
			{
				tr.Start();			
			
				for (int i = 0; i < ptNum; i++) {
					var t = StartChainage + i * interval;
					double cant = Math.Round(tcl1.getAppliedCant(t, true));
					double el = 0, slope = 0;
					double depth = double.Parse(Interaction.GetSetting("AlignmentTools","AppData", "textInvertElevation","400"));
					double H = Math.Round(tcl1.getTunnelOffset(t, true));
					double W = Math.Round(tcl1.getGaugeWidenning(t, true));
					int Side = tcl1.getSideOfWalkway(t);
					if (bool.Parse(Interaction.GetSetting("AlignmentTools","AppData","bFixedElevation","True")))
					{
						el = (double.Parse(Interaction.GetSetting("AlignmentTools","AppData","textElevation","83.800")) - pl.Z) * 1000;
						slope = 0;
					}
					else
					{
						el = (tcl1.Getz(t) - pl.Z) * 1000;
						slope = tcl1.Gets(t);
					}
					
					SetElementParameterInMm( symbol, "C"+(i+1).ToString(), cant );
					SetElementParameterDouble( symbol, "Slope"+(i+1).ToString(), slope );
					SetElementParameterInMm( symbol, "EL"+(i+1).ToString(), el );
					SetElementParameterInMm( symbol, "Depth"+(i+1).ToString(), depth );
					SetElementParameterInMm( symbol, "H"+(i+1).ToString(), H );
					SetElementParameterInMm( symbol, "W"+(i+1).ToString(), W );
					SetElementParameterInt( symbol, "Side"+(i+1).ToString(), Side );
					
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
	                var t = StartChainage + i * interval - tcl1.GetStartLocalChainage();
	                var point = doc.GetElement(id) as ReferencePoint;
	                var ploc = new PointLocationOnCurve(PointOnCurveMeasurementType.SegmentLength, m2ft(t),
	                                                    PointOnCurveMeasureFrom.Beginning);
	                var peref = Application.Create.NewPointOnEdge(mc.GeometryCurve.Reference, ploc);
	                point.SetPointElementReference(peref);
	                i++;
	            }
				tr.Commit();
			}
			
			bool bFitRailLength = GetbFitRailLength(); 
			
			if (bFitRailLength)
			{
				Interaction.SaveSetting("AlignmentTools","AppData","textLength", PlinthLength.ToString());
				AdjustRailLength(instance);	
			}
		}
		
		public void OldAddOnePlinth(string PlinthNo, double StartChainage, double EndChainage, ModelCurve mc, TrackCenterLine tcl1)		
		{
			
			double PlinthLength = EndChainage - StartChainage;
			//手動添加需要有模型線才可以使用。
			Document doc = this.ActiveUIDocument.Document;
            UIDocument uidoc = this.ActiveUIDocument;
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
			XYZ pl = getprojectPosition();			
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
                var peref = Application.Create.NewPointOnEdge(mc.GeometryCurve.Reference, ploc);
                point.SetPointElementReference(peref);
                i++;
            }
			tr.Commit();
			}
		}
		
		public void RegMCid()  
		{
			XmlDocument doc = new XmlDocument();
			doc.Load("MCid.xml");
			XmlNode node = doc.SelectSingleNode("Company/Department");//選擇節點
			if (node == null)
			    return;
			XmlElement main = doc.CreateElement("newPerson"); //添加person節點
			main.SetAttribute("name", "小明");
			main.SetAttribute("sex", "女");
			main.SetAttribute("age", "25");
			node.AppendChild(main);
			XmlElement sub1 = doc.CreateElement("phone");
			sub1.InnerText = "123456778";
			main.AppendChild(sub1);
			XmlElement sub2 = doc.CreateElement("address");
			sub2.InnerText = "高雄";
			main.AppendChild(sub2);
			doc.Save("MCid.xml");		
		}
		
		public void testMCidXml()
		{
			XmlDocument doc = new XmlDocument();
			//建立根節點
			XmlElement AllMCid = doc.CreateElement("AllMCid");
			doc.AppendChild(AllMCid);
			//建立子節點
			XmlElement ALD = doc.CreateElement("Y06UH.ALD");
			ALD.SetAttribute("MCid", "12345678");//設定屬性
			//加入至AllMCid節點底下
			AllMCid.AppendChild(ALD); 
			
//			XmlElement members = doc.CreateElement("Members");//建立節點
//			//加入至department節點底下
//			department.AppendChild(members); 
//			
//			XmlElement info = doc.CreateElement("Information");
//			info.SetAttribute("名字", "余小章");
//			info.SetAttribute("電話", "0806449");
//			//加入至members節點底下
//			members.AppendChild(info);
//			info = doc.CreateElement("Information");
//			info.SetAttribute("名字", "王大明");
//			info.SetAttribute("電話", "080644978");
//			//加入至members節點底下
//			members.AppendChild(info);
			doc.Save("D:\\AlignmentTools\\TrackSystem\\MCid.xml");		
		}

		public void SaveMCid()  
		{
			SaveMCidByDataStorage("X01UH.ALD", "155527");
//			SaveMCidByDataStorage("R03DH.ALD", "1144968");
//			SaveMCidByDataStorage("R3XDH.ALD", "1295196");
//			SaveMCidByDataStorage("R3XUH.ALD", "1295198");
//			SaveMCidByDataStorage("R02UH.ALD", "1295200");
//			SaveMCidByDataStorage("R02DH.ALD", "1295202");
		}

		public string GetMCid(string AldName)  
		{
			XmlDocument doc = new XmlDocument();
			doc.Load("D:\\AlignmentTools\\TrackSystem\\MCid.xml");
			// 1. 先試著搜尋是否已存在, 若在則更新其id。
			// 2. 若不存在,則新增一筆資料。
			//選擇節點
			XmlNode main = doc.SelectSingleNode("AllMCid/" + AldName);
			if (main == null)
			{
				return "Y06UH.ALD";
			}
			else
			{
				//取得節點內的欄位
				XmlElement element = (XmlElement)main;
				//列舉節點內的屬性
				XmlAttributeCollection attributes = element.Attributes;
				foreach (XmlAttribute item in attributes)
				{
				    if (item.Name == "MCid")
				        return item.Value;
				}
			}
			return "Y06UH.ALD";			
		}

		
		public void SaveMCid(string AldName, string MCid)  
		{
			XmlDocument doc = new XmlDocument();
			doc.Load("D:\\AlignmentTools\\TrackSystem\\MCid.xml");
			// 1. 先試著搜尋是否已存在, 若在則更新其id。
			// 2. 若不存在,則新增一筆資料。
			//選擇節點
			XmlNode main = doc.SelectSingleNode("AllMCid/" + AldName);
			if (main == null)
			{
				XmlElement AllMCid = (XmlElement)doc.SelectSingleNode("AllMCid");
				XmlElement ALD = doc.CreateElement(AldName);
				ALD.SetAttribute("MCid", MCid);//設定屬性
				AllMCid.AppendChild(ALD);				
			}
			else
			{
				//取得節點內的欄位
				XmlElement element = (XmlElement)main;
				//列舉節點內的屬性
				XmlAttributeCollection attributes = element.Attributes;
				foreach (XmlAttribute item in attributes)
				{
				    if (item.Name == "MCid")
				        item.Value = MCid;
				}
			}
			
//			XmlNode node = doc.SelectSingleNode(AldName);//選擇節點
//			if (node == null)
//			    return;
//			XmlElement main = doc.CreateElement(AldName); //添加person節點
//			main.SetAttribute("MCid", MCid);
//			node.AppendChild(main);
//			XmlElement sub1 = doc.CreateElement("phone");
//			sub1.InnerText = "123456778";
//			main.AppendChild(sub1);
//			XmlElement sub2 = doc.CreateElement("address");
//			sub2.InnerText = "高雄";
//			main.AppendChild(sub2);
			doc.Save("D:\\AlignmentTools\\TrackSystem\\MCid.xml");		
		}



		public void RegActiveMCid()  //Save ModelCurve ID for current ALDName1 將作用中的線形ModelCurve的ID存放於Registery, 後續不用再選取作用中的線形ModelCurve.
		{
            UIDocument uidoc = this.ActiveUIDocument;

			string ALDName = Interaction.GetSetting("AlignmentTools","AppData","ALDName1","Y06UH.ALD");
			
			//Get current selection and store it
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
            ISelectionFilter selFilter = new ModelLineSelectionFilter();
            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取 "+ ALDName +" 的軌道中心線：");
            
            //Interaction.SaveSetting("AlignmentTools","AppData", ALDName, hasPickOne.ElementId.ToString());
            SaveMCid(ALDName, hasPickOne.ElementId.ToString());
			
		}
		
		
		public void RegMCid(ElementId id)  //Save ModelCurve ID for current ALDName1 將作用中的線形ModelCurve的ID存放於Registery, 後續不用再選取作用中的線形ModelCurve.
		{
            UIDocument uidoc = this.ActiveUIDocument;

			string ALDName = Interaction.GetSetting("AlignmentTools","AppData","ALDName1","Y06UH.ALD");
            
            Interaction.SaveSetting("AlignmentTools","AppData", ALDName, id.ToString());
			
		}

		public string GetActiveComponent()
		{
			string ActiveComponent = Interaction.GetSetting("AlignmentTools","AppData","ComponentName","Plinth");
			
			return ActiveComponent ;
		}
		
		public string GetActiveFunction()
		{
			string ActiveFunction = Interaction.GetSetting("AlignmentTools","AppData","ActiveFunction","");
			
			return ActiveFunction ;
		}

		public string GetActiveFolder()
		{
			string ActiveFolder = Interaction.GetSetting("AlignmentTools","AppData","SystemName","Slab");
			
			return ActiveFolder;
		}

		public bool GetbFitRailLength()
		{
			bool bFitRailLength = bool.Parse(Interaction.GetSetting("AlignmentTools","AppData","bFitRailLength","True"));
			
			return bFitRailLength;
		}
		
		public bool GetbReverse()
		{
			bool bReverse = bool.Parse(Interaction.GetSetting("AlignmentTools","AppData","bReverse","True"));
			
			return bReverse;
		}
		
		public double GetFixedRailLength()
		{
			double FixedRailLength = double.Parse(Interaction.GetSetting("AlignmentTools","AppData","textLength","17.960"));
			
			return FixedRailLength;
		}
		
		public double GetRailSectionArea()
		{
			double RailSectionArea= double.Parse(Interaction.GetSetting("AlignmentTools","AppData","RailSectionArea","77.30986"));
			
			return RailSectionArea;
		}
		
		
		public ElementId GetActiveMC()
		{
			
			//string MCid = Interaction.GetSetting("AlignmentTools","AppData", ALDName,"Y06UH.ALD");
			string ALDName = Interaction.GetSetting("AlignmentTools","AppData","ALDName1","Y06UH.ALD");
			string MCid = GetMCidByDataStorage(ALDName);
//			string MCid = GetMCid(ALDName);
			return new ElementId(int.Parse(MCid));
		}
		

		public void BatchBuild()	//Create Track components by the list get from excel sheet	
		{
			
			Document doc = this.ActiveUIDocument.Document;
            UIDocument uidoc = this.ActiveUIDocument;

			
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
				
				StartChainage = (double)(worksheet.Cells[i,2] as X.Range).Value2;
				if ("Component" == GetActiveFolder())
				{
					
					Interaction.SaveSetting("AlignmentTools","AppData","textLength", (worksheet.Cells[i,4] as X.Range).Text as string);
					Interaction.SaveSetting("AlignmentTools","AppData","textQuantity", (worksheet.Cells[i,5] as X.Range).Text as string);
				
					AddComponents(StartChainage, mc, tcl1);
				}
				else
				{
					PlinthNo = (worksheet.Cells[i,1] as X.Range).Text as string;
					EndChainage = (double)(worksheet.Cells[i,3] as X.Range).Value2;
					
					AddOnePlinth(PlinthNo, StartChainage, EndChainage, mc, tcl1);
				}
				i++;
			}
		}	
		
		
		public void PickPoint2GetLocalChainage()
		{
			UIDocument uidoc = ActiveUIDocument;
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
			XYZ pt1 = uidoc.Selection.PickPoint(ObjectSnapTypes.Nearest, "請點選一個點: ");
			XYZ pp = getprojectPosition();
			XYZ WCSPt = new XYZ(pp.X + ft2m(pt1.X), pp.Y + ft2m(pt1.Y), 0);
			double[] pw = tcl1.Getpw(pp.X + ft2m(pt1.X), pp.Y + ft2m(pt1.Y));
			Interaction.SaveSetting("AlignmentTools","AppData","textChainage", pw[0].ToString("0.000"));
		}
		
		public void PickPoint2GetLength()
		{
			UIDocument uidoc = ActiveUIDocument;
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
			XYZ pt1 = uidoc.Selection.PickPoint(ObjectSnapTypes.Nearest, "請點選一個點: ");
			XYZ pp = getprojectPosition();
			XYZ WCSPt = new XYZ(pp.X + ft2m(pt1.X), pp.Y + ft2m(pt1.Y), 0);
			double[] pw1 = tcl1.Getpw(pp.X + ft2m(pt1.X), pp.Y + ft2m(pt1.Y));
			pt1 = uidoc.Selection.PickPoint(ObjectSnapTypes.Nearest, "請點選二個點: ");
			WCSPt = new XYZ(pp.X + ft2m(pt1.X), pp.Y + ft2m(pt1.Y), 0);
			double[] pw2 = tcl1.Getpw(pp.X + ft2m(pt1.X), pp.Y + ft2m(pt1.Y));
			double Length = Math.Abs(pw2[0] - pw1[0]);
			Interaction.SaveSetting("AlignmentTools","AppData","textLength", Length.ToString("0.000"));
		}

		public void pickObjTest()
		{
			Document doc = this.ActiveUIDocument.Document;
			UIDocument uidoc = ActiveUIDocument;
          	//Get current selection and store it
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
            ISelectionFilter selFilter = new ModelLineSelectionFilter();
            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取軌道中心線：");
			CurveElement mc = doc.GetElement(hasPickOne.ElementId) as CurveElement;
			HermiteSpline hs = mc.GeometryCurve as HermiteSpline;
			IList<XYZ> pts = hs.ControlPoints;
		}
		
		public void TestModifiyMC()   //Update the model curve by given new coordinates.
		{
			Document doc = this.ActiveUIDocument.Document;
			UIDocument uidoc = ActiveUIDocument;
          	//Get current selection and store it
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
            ISelectionFilter selFilter = new ModelLineSelectionFilter();
            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取軌道中心線：");
			ModelHermiteSpline mc = doc.GetElement(hasPickOne.ElementId) as ModelHermiteSpline;
			HermiteSpline hs = mc.GeometryCurve as HermiteSpline;
			
			List <XYZ> pts = new List<XYZ>();;
			pts.Add(hs.ControlPoints.First());
			pts.Add(hs.ControlPoints.First().Add(new XYZ(mc.GeometryCurve.Length, 0, 0)));
			ModelHermiteSpline spline = createModelHermitSpline(pts);
			
			using ( Transaction tr = new Transaction(doc, "TestModifiyMC")) {
				tr.Start();
		       	mc.SetGeometryCurve(spline.GeometryCurve, true);
		       	tr.Commit();
				
			}
		}
		
		
		public void WalkThrougthTest()
		{
			Document doc = this.ActiveUIDocument.Document;
			UIDocument uidoc = ActiveUIDocument;
          	//Get current selection and store it
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
            ISelectionFilter selFilter = new WalkthrougthSelectionFilter();
            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取穿越：");
			var mc = doc.GetElement(hasPickOne.ElementId);
//			HermiteSpline hs = mc.GeometryCurve as HermiteSpline;
//			IList<XYZ> pts = hs.ControlPoints;
		}
		public void TestAdjustInstance()  //Adjust the adaptive point to average distances.
		{
			Document doc = this.ActiveUIDocument.Document;
			UIDocument uidoc = ActiveUIDocument;
			IList<ElementId> placePointIds;
          	//Get current selection and store it
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
            ISelectionFilter selFilter = new ElementSelectionFilter("一般模型");
            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取軌道元件：");
			FamilyInstance instance = doc.GetElement(hasPickOne.ElementId) as FamilyInstance;
			placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(instance);
			int ptnum = placePointIds.Count;
			using (Transaction tr= new Transaction(doc, "CreatePlinthInstance"))
			{
				tr.Start();	
	            for (int i = 0; i < ptnum; i++)
	            {	            	
	            	ReferencePoint pt = doc.GetElement(placePointIds[i]) as ReferencePoint;
	            	pt.GetParameters("測量類型").First().Set(3);
	            	pt.GetParameters("測量自").First().Set(1);
//	            	double ch = pt.GetParameters("區段長度").First().AsDouble();
	            }
				tr.Commit();
			}
			
			ReferencePoint pt1 = doc.GetElement(placePointIds[0]) as ReferencePoint;
			double StartChainage = pt1.GetParameters("區段長度").First().AsDouble();
			pt1 = doc.GetElement(placePointIds[ptnum-1]) as ReferencePoint;
			double EndChainage = pt1.GetParameters("區段長度").First().AsDouble();
			
			double interval = (EndChainage - StartChainage)/(ptnum-1);
	    
			using (Transaction tr= new Transaction(doc, "CreatePlinthInstance"))
			{
				tr.Start();	
	            for (int i = 1; i < (ptnum-1); i++)
	            {	            	
	            	ReferencePoint pt = doc.GetElement(placePointIds[i]) as ReferencePoint;
	            	double t = StartChainage + i * interval;
	            	pt.GetParameters("區段長度").First().Set(t);
	            }
				tr.Commit();
			}

		}

		public void AdjustRailLength()  //Adjust the adaptive point to meet the fixed 18m long rail.
		{
			DateTime begin = DateTime.Now;
			int rept=0;
			bool Reverse = true; //是否反向舖軌
			Document doc = this.ActiveUIDocument.Document;
			UIDocument uidoc = ActiveUIDocument;
			IList<ElementId> placePointIds;
          	//Get current selection and store it
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
            ISelectionFilter selFilter = new ElementSelectionFilter("一般模型");
            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取鋼軌：");
			FamilyInstance instance = doc.GetElement(hasPickOne.ElementId) as FamilyInstance;
			placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(instance);
			int ptnum = placePointIds.Count;
			using (Transaction tr= new Transaction(doc, "CreatePlinthInstance"))
			{
				tr.Start();	
	            for (int i = 0; i < ptnum; i++)
	            {	            	
	            	ReferencePoint pt = doc.GetElement(placePointIds[i]) as ReferencePoint;
	            	pt.GetParameters("測量類型").First().Set(3);
	            	pt.GetParameters("測量自").First().Set(1);
//	            	double ch = pt.GetParameters("區段長度").First().AsDouble();
	            }
				tr.Commit();
			}
			int a,b;
			ReferencePoint pt1 = doc.GetElement(placePointIds[0]) as ReferencePoint;
			double StartChainage = pt1.GetParameters("區段長度").First().AsDouble();
			while(true)
			{
				pt1 = doc.GetElement(placePointIds[ptnum-1]) as ReferencePoint;
				double EndChainage = pt1.GetParameters("區段長度").First().AsDouble();
				double Delta = instance.GetParameters("體積").First().AsDouble()/cms2fts(77.30985577358855)-m2ft(18);
																					   
				if (Math.Abs(Delta)<0.001312) break;
				
				double interval = (EndChainage - StartChainage - Delta)/(ptnum-1);
				
				if (Reverse)
				{
					StartChainage += Delta;
					a=0;
					b=ptnum-1;
				}else
				{
					a=1;
					b=ptnum;
				}
				
				TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");				
    			XYZ pl = getprojectPosition();	
    			
				using (Transaction tr= new Transaction(doc, "CreatePlinthInstance"))
				{
					tr.Start();	
		            for (int i = a; i < b; i++)
		            {	            	
		            	ReferencePoint pt = doc.GetElement(placePointIds[i]) as ReferencePoint;
		            	double t1 = StartChainage + i * interval;
		            	pt.GetParameters("區段長度").First().Set(t1);
		            	
		            	double t = ft2m(t1);
						double cant = Math.Round(tcl1.getAppliedCant(t, true));
						double el = (tcl1.Getz(t) - pl.Z) * 1000;
						double slope = tcl1.Gets(t);
						
						instance.Symbol.GetParameters("C"+(i+1).ToString()).First().Set(mm2ft(cant));
						instance.Symbol.GetParameters("Slope"+(i+1).ToString()).First().Set(mm2ft(slope));
						instance.Symbol.GetParameters("EL"+(i+1).ToString()).First().Set(mm2ft(el));
					
		            }
					tr.Commit();
				}
				
				rept++;
				
			}
			
			DateTime end = DateTime.Now;
			
			TaskDialog.Show("Adjust Rail Length","Iterates "+rept.ToString()+" Times, duration = "+(end - begin).ToString()+ " Seconds.");

		}
		
		public void AdjustRailLength(FamilyInstance instance)  //Adjust the adaptive point to meet the fixed 18m long rail.
		{
			DateTime begin = DateTime.Now;
			double fixedRailLength = GetFixedRailLength(); //m
			double RailSectionArea = GetRailSectionArea(); //cm^2
			bool Reverse = GetbReverse(); //是否反向舖軌
			int rept=0;
			Document doc = this.ActiveUIDocument.Document;
			IList<ElementId> placePointIds;
          	//Get current selection and store it
			placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(instance);
			int ptnum = placePointIds.Count;
			using (Transaction tr= new Transaction(doc, "CreatePlinthInstance"))
			{
				tr.Start();	
	            for (int i = 0; i < ptnum; i++)
	            {	            	
	            	ReferencePoint pt = doc.GetElement(placePointIds[i]) as ReferencePoint;
	            	pt.GetParameters("測量類型").First().Set(3);
	            	pt.GetParameters("測量自").First().Set(1);
//	            	double ch = pt.GetParameters("區段長度").First().AsDouble();
	            }
				tr.Commit();
			}
			int a=0 ,b=0;
			double interval=0;
			ReferencePoint pt1 = doc.GetElement(placePointIds[0]) as ReferencePoint;
			double StartChainage = pt1.GetParameters("區段長度").First().AsDouble();
			while(true)
			{
				pt1 = doc.GetElement(placePointIds[ptnum-1]) as ReferencePoint;
				double EndChainage = pt1.GetParameters("區段長度").First().AsDouble();
				double Delta = instance.GetParameters("體積").First().AsDouble()/cms2fts(RailSectionArea)-m2ft(fixedRailLength);
																					   
				if (Math.Abs(Delta)<0.001312) break;
				
				interval = (EndChainage - StartChainage - Delta)/(ptnum-1);
				
				if (Reverse)
				{
					StartChainage += Delta;
					a=0;
					b=ptnum-1;
				}else
				{
					a=1;
					b=ptnum;
				}
				
				TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");				
    			XYZ pl = getprojectPosition();	
    			
				using (Transaction tr= new Transaction(doc, "CreatePlinthInstance"))
				{
					tr.Start();	
		            for (int i = a; i < b; i++)
		            {	            	
		            	ReferencePoint pt = doc.GetElement(placePointIds[i]) as ReferencePoint;
		            	double t1 = StartChainage + i * interval;
		            	pt.GetParameters("區段長度").First().Set(t1);
		            	
		            	double t = ft2m(t1);
						double cant = Math.Round(tcl1.getAppliedCant(t, true));
						double el = (tcl1.Getz(t) - pl.Z) * 1000;
						double slope = tcl1.Gets(t);
						
						instance.Symbol.GetParameters("C"+(i+1).ToString()).First().Set(mm2ft(cant));
						instance.Symbol.GetParameters("Slope"+(i+1).ToString()).First().Set(mm2ft(slope));
						instance.Symbol.GetParameters("EL"+(i+1).ToString()).First().Set(mm2ft(el));
					
		            }
					tr.Commit();
				}
				
				rept++;
				
			}
			
			DateTime end = DateTime.Now;
			
			TaskDialog.Show("Adjust Rail Length","Iterates "+rept.ToString()+" Times, duration = "+(end - begin).ToString()+ " Seconds.");
			
			bool bReverse = GetbReverse(); 
			if (bReverse)
			{
				Interaction.SaveSetting("AlignmentTools","AppData","textChainage",StartChainage.ToString());
			}
			else
			{
				Interaction.SaveSetting("AlignmentTools","AppData","textChainage",(StartChainage + (b-1) * interval).ToString());
			}

		}
		
		
		public void IRTest() // IntersectionResult class test.
		{
			Document doc = this.ActiveUIDocument.Document;
			UIDocument uidoc = ActiveUIDocument;
          	//Get current selection and store it
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
            ISelectionFilter selFilter = new ModelLineSelectionFilter();
            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "選取軌道中心線：");
            ModelCurve mc = doc.GetElement(hasPickOne.ElementId) as ModelCurve ;
	    
			
		    ObjectSnapTypes snapTypes = ObjectSnapTypes.Endpoints;// | ObjectSnapTypes.Intersections;
    		XYZ point = uidoc.Selection.PickPoint(snapTypes, "Select an end point or intersection");
			IntersectionResult ir = mc.GeometryCurve.Project(point);
			double t=ir.Parameter;  //in feet
			double a=t;
		}

		public class ElementSelectionFilter : ISelectionFilter
		{
			private string m_CategoryName = "";
			public ElementSelectionFilter(string CategoryName)
			{
				m_CategoryName = CategoryName;
			}
		    public bool AllowElement(Element element)
		    {
		        if (element.Category.Name == m_CategoryName)
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

		public class WalkthrougthSelectionFilter : ISelectionFilter
		{
		    public bool AllowElement(Element element)
		    {
//		        if (element.Category.Name == "相機")
//		        {
//		            return true;
//		        }
		        return true;
		    }
		
		    public bool AllowReference(Reference refer, XYZ point)
		    {
		        return false;
		    }
		}		
		
		
		private Reference GetFamilyInstancePointReference(
		FamilyInstance fi )
		{
			Autodesk.Revit.DB.Options _opt = new Autodesk.Revit.DB.Options();
		return fi.get_Geometry( _opt )
		  .OfType<Point>()
		  .Select<Point, Reference>( x => x.Reference )
		  .FirstOrDefault();
		}

		private void GetFamilyInstanceReferenceArray(FamilyInstance fi, ref ReferenceArray curves )
		{
			Autodesk.Revit.DB.Options _opt = new Autodesk.Revit.DB.Options();
			GeometryElement geomElem = fi.get_Geometry( _opt );
			GeometryInstance gi = geomElem.First() as GeometryInstance;
		    
				GeometryElement ge =  gi.GetSymbolGeometry();
				foreach (Curve curve in ge) {
					if (null != curve)
			        {
			        	curves.Append(curve.Reference);
			            //continue;
			        }					
				}
		    
	}
		
		private void AddCurvesAndSolids(Autodesk.Revit.DB.GeometryElement geomElem,
		                                ref Autodesk.Revit.DB.CurveArray curves)
		{
		    foreach (Autodesk.Revit.DB.GeometryObject geomObj in geomElem)
		    {
		        Autodesk.Revit.DB.Curve curve = geomObj as Autodesk.Revit.DB.Curve;
		        if (null != curve)
		        {
		            curves.Append(curve);
		            continue;
		        }
		    }
		}		
		
		
		private Result Loadfamily(Document familyDocument, string FamilyName, out Family family)
		{
			string Path = "D:\\Tsao\\Developement\\RevitTrackModeling\\";
			
			FilteredElementCollector a = new FilteredElementCollector( familyDocument ).OfClass( typeof( Family ) );
			
			family = a.FirstOrDefault<Element>(e => e.Name.Equals(FamilyName)) as Family;
			
			if (null == family)
			{
				if( !File.Exists( Path + FamilyName + ".rfa" ) )
				{
				  ErrorMsg( string.Format(
				    "Please ensure that the sample table "
				    + "family file '{0}' exists in '{1}'.",
				    FamilyName+".rfa", Path ) );
				
				  return Result.Failed;
				}
				
				// Load family from file:
				
				using( Transaction tx = new Transaction( familyDocument ) )
				{
				  tx.Start( "Load Family" );
				  familyDocument.LoadFamily(Path + FamilyName + ".rfa", out family );
				  tx.Commit();
				}
				
			}
			
			return Result.Succeeded;

		}
		
		public static void ErrorMsg( string msg )
	    {
	      //Debug.WriteLine( msg );
	      TaskDialog d = new TaskDialog( "Track Tools" );
	      d.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
	      d.MainInstruction = msg;
	      d.Show();
	    }
		
		private FamilySymbol CreateNewType( FamilySymbol oldType, double Cant, double depth, double elevation, int ptNum )
	    {
	      FamilySymbol sym = oldType.Duplicate(
				ptNum.ToString() ) as FamilySymbol;
	
	      SetElementParameterInMm( sym, "C", Cant );
	      SetElementParameterInMm( sym, "Depth", depth );
	      SetElementParameterInMm( sym, "EL", elevation );
	
	
	      return sym;
	    }

		//CreateNewElement(symbol, Quantity, StartChainage, tcl1);
		private FamilySymbol CreateNewElement( FamilySymbol oldType, int Quantity)
	    {
			FamilySymbol sym = oldType.Duplicate((Quantity+1).ToString("000")) as FamilySymbol;
			return sym;
	    }
		
		//Create new Type of track element
		private FamilySymbol CreateNewElement( FamilySymbol oldType, string PlinthNo, double StartChainage, double EndChainage, int ptNum, TrackCenterLine tcl1)
	    {
			FamilySymbol sym = oldType.Duplicate("002") as FamilySymbol;
//			double 	PlinthLength = EndChainage - StartChainage;		
//			double interval = PlinthLength / (ptNum -1);
//			
//			XYZ pl = getprojectPosition();	
//			
//			for (int i = 0; i < ptNum; i++) {
//				var t = StartChainage + i * interval;
//				double cant = Math.Round(tcl1.getAppliedCant(t, true));
//				double el = (tcl1.Getz(t) - pl.Z) * 1000;
//				double slope = tcl1.Gets(t);
//				
//				SetElementParameterInMm( sym, "C"+(i+1).ToString(), cant );
//				SetElementParameterInMm( sym, "Slope"+(i+1).ToString(), slope );
//				SetElementParameterInMm( sym, "EL"+(i+1).ToString(), el );
//			}
			
			
			return sym;
	    }
		
		
		public void EditPlinthFamily()
		{
			Document document = ActiveUIDocument.Document;
			FamilySymbol symbol;
			FamilyOption opt = new FamilyOption();
			
			using( Transaction tr = new Transaction(document, "CreatePlinthInstance"))
			{
				tr.Start();
				document.LoadFamilySymbol("D:\\Tsao\\Developement\\RevitTrackModeling\\6pt.rfa", "6pt", opt, out symbol);
				symbol.Family.Name="DN-001";
				symbol.Name="DN-001";
			    tr.Commit();
			}
			
		    Family family = symbol.Family;
	        // Get Family document for family
	        Document familyDoc = document.EditFamily(family);
	        if (null != familyDoc && familyDoc.IsFamilyDocument == true)
	        {
	            String loadedFamilies = "FamilySymbols in " + family.Name + ":\n";
	        	using(Transaction tr = new Transaction(familyDoc, "EditFamily"))
	        	{
	        		tr.Start();
		            FilteredElementCollector collector = new FilteredElementCollector(familyDoc);
		            ICollection<Element> collection = 
		                collector.OfClass(typeof(FamilySymbol))
		            		            	.ToElements();
		            for(int i=0; i<6; i++)
		            {
						FamilySymbol famSym = GetSymbol(familyDoc, "LeftPlinth", (i+1).ToString());	
						SetElementParameterInMm( famSym, "C", -i*20 );
						famSym = GetSymbol(familyDoc, "RightPlinth", (i+1).ToString());	
						SetElementParameterInMm( famSym, "C", -i*20 );				
		            }
		            
		            tr.Commit();
	        	}
	            
				TaskDialog.Show("Revit", loadedFamilies);
	        }
			
			
		}
		
		public void checkPlinthCantBylist()
		{			
			Document doc = ActiveUIDocument.Document;
			
			X.Application excel = (X.Application)Marshal.GetActiveObject("Excel.Application");
			if( null == excel )
			{
				
				TaskDialog.Show("Getpw", "Failed to get or start Excel." );
				
			}
			
			X.Worksheet worksheet = (X.Worksheet) excel.ActiveSheet;
			double StartChainage, EndChainage;
			string PlinthNo, FamilyName;
			int i = excel.ActiveCell.Row;

			PlinthNo = (worksheet.Cells[i,1] as X.Range).Text as string;
				StartChainage = (double)(worksheet.Cells[i,2] as X.Range).Value2;
				EndChainage = (double)(worksheet.Cells[i,3] as X.Range).Value2;
				
			
			double PlinthLength = EndChainage - StartChainage;
			
			FamilyName = PlinthNo + "-" + PlinthLength.ToString("0.000");
			
			FilteredElementCollector a = new FilteredElementCollector( doc ).OfClass( typeof( Family ) );
			
			Family family = a.FirstOrDefault<Element>( e => e.Name.Equals( FamilyName ) ) as Family;
	
	        Document familyDoc = doc.EditFamily(family);
	        if (null != familyDoc && familyDoc.IsFamilyDocument == true)
	        {
	            String LCant = "", RCant = "";
	        	using(Transaction tr = new Transaction(familyDoc, "EditFamily"))
	        	{
	        		tr.Start();
		            FilteredElementCollector collector = new FilteredElementCollector(familyDoc);
		            ICollection<Element> collection = 
		                collector.OfClass(typeof(FamilySymbol))
		            		            	.ToElements();
		            for(i=0; i<6; i++)
		            {
		            	FamilySymbol famSym = GetSymbol(familyDoc, "LeftPlinth", (i+1).ToString());
		            	LCant = LCant + " " + GetElementParameterInMm( famSym, "C").ToString("0");
						famSym = GetSymbol(familyDoc, "RightPlinth", (i+1).ToString());	
						RCant = RCant + " " + GetElementParameterInMm( famSym, "C").ToString("0");				
		            }
		            
		            tr.Commit();
	        	}
	            
				TaskDialog.Show("Revit", LCant + "\n" + RCant);
	        }
			
			
		}
		


		public void EditPlinthFamilyByList()
		{
			Document document = ActiveUIDocument.Document;
			
			X.Application excel = (X.Application)Marshal.GetActiveObject("Excel.Application");
			if( null == excel )
			{
				
				TaskDialog.Show("Getpw", "Failed to get or start Excel." );
				
			}
			
			X.Worksheet worksheet = (X.Worksheet) excel.ActiveSheet;
			double StartChainage, EndChainage;
			string PlinthNo;
			int i = excel.ActiveCell.Row;

			PlinthNo = (worksheet.Cells[i,1] as X.Range).Text as string;
				StartChainage = (double)(worksheet.Cells[i,2] as X.Range).Value2;
				EndChainage = (double)(worksheet.Cells[i,3] as X.Range).Value2;
				
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
			
			double PlinthLength = EndChainage - StartChainage;
			double interval = PlinthLength / 5.0;
			//Family family;			
			FamilySymbol symbol;
			FamilyOption opt = new FamilyOption();
			
			using( Transaction tr = new Transaction(document, "CreatePlinthInstance"))
			{
				tr.Start();
				document.LoadFamilySymbol("D:\\Tsao\\Developement\\RevitTrackModeling\\6pt.rfa", "6pt", opt, out symbol);
				//document.LoadFamily("D:\\Tsao\\Developement\\RevitTrackModeling\\6pt.rfa", opt, out family);
				symbol.Family.Name = PlinthNo + "-" + PlinthLength.ToString("0.000");
				symbol.Name= PlinthNo + "-" + PlinthLength.ToString("0.000");
			    tr.Commit();
			}
			
			
		    Family family = symbol.Family;
	        // Get Family document for family
	        Document familyDoc = document.EditFamily(family);
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
		            	double cant = -tcl1.getAppliedCant(t, true);
		            	FamilySymbol famSym = GetSymbol(familyDoc, "LeftPlinth", (i+1).ToString());
						SetElementParameterInMm( famSym, "C", cant );
						famSym = GetSymbol(familyDoc, "RightPlinth", (i+1).ToString());	
						SetElementParameterInMm( famSym, "C", cant );				
		            }
		            
		            familyDoc.LoadFamily(document,opt);
		            
		            tr.Commit();
	        	}
	            
				//TaskDialog.Show("Revit", loadedFamilies);
	        }
			
			
		}
		
		public FamilySymbol GetSymbol(Document document, string familyName, string symbolName)
        {
			Family family = Utility.FindElementByName(document, typeof( Family ), familyName ) as Family;
			if (family == null)
				return null;
            	return new FilteredElementCollector(document).OfClass(typeof(Family)).OfType<Family>().FirstOrDefault(f => f.Name.Equals(familyName)).GetFamilySymbolIds().Select(id => document.GetElement(id)).OfType<FamilySymbol>().FirstOrDefault(symbol => symbol.Name.Equals(symbolName));
        }
		
		public FamilySymbol GetSymbol(Document document, string familyName)
        {
 			Family family = Utility.FindElementByName(document, typeof( Family ), familyName ) as Family;
			if (family == null)
				return null;
           		return new FilteredElementCollector(document).OfClass(typeof(Family)).OfType<Family>().FirstOrDefault(f => f.Name.Equals(familyName))
            				.GetFamilySymbolIds().Select(id => document.GetElement(id)).OfType<FamilySymbol>().FirstOrDefault();
        }

		public int GetSymbolQuantity(Document document, string familyName)
        {
 			Family family = Utility.FindElementByName(document, typeof( Family ), familyName ) as Family;
			if (family == null)
				return 0;
           		return new FilteredElementCollector(document).OfClass(typeof(Family)).OfType<Family>().FirstOrDefault(f => f.Name.Equals(familyName))
           			.GetFamilySymbolIds().Select(id => document.GetElement(id)).OfType<FamilySymbol>().Count();
        }
		
		public void createModelHermitSpline()
		{
			Document document = ActiveUIDocument.Document;
			double x = 0;
			IList<XYZ> ra= new List<XYZ>();
		    // Set the position of each placement point
		    for(int i=0; i<6; i++)
		    {
		        XYZ pt = new  XYZ(10*x, 10*Math.Cos(x), 0);
		        ra.Add(pt);
		        x += Math.PI/6;
		    }
		    
		    using (Transaction tr = new Transaction(document, "createModelHermitSpline"))
		    {
		    	tr.Start();
			    HermiteSpline curve = HermiteSpline.Create(ra, false);
				// Create a geometry plane in Revit application
				XYZ origin = new XYZ(0, 0, 0);
				XYZ normal = new XYZ(0, 0, 1);
				Plane geomPlane = Plane.CreateByNormalAndOrigin(normal, origin);
				
				// Create a sketch plane in current document
				SketchPlane sketch = SketchPlane.Create(document, geomPlane);
				    
			    ModelHermiteSpline spline = document.Create.NewModelCurve(curve, sketch) as ModelHermiteSpline;
			    tr.Commit();
		    }

		}
		
		public ModelHermiteSpline createModelHermitSpline(List<XYZ> pts)
		{
			Document document = ActiveUIDocument.Document;
			IList<XYZ> ra= pts;
			ModelHermiteSpline spline;
		    using (Transaction tr = new Transaction(document, "createModelHermitSpline"))
		    {
		    	tr.Start();
			    HermiteSpline curve = HermiteSpline.Create(ra, false);
				// Create a geometry plane in Revit application
				XYZ origin = new XYZ(0, 0, 0);
				XYZ normal = new XYZ(0, 0, 1);
				Plane geomPlane = Plane.CreateByNormalAndOrigin(normal, origin);
				
				// Create a sketch plane in current document
				SketchPlane sketch = SketchPlane.Create(document, geomPlane);
				    
			    spline = document.Create.NewModelCurve(curve, sketch) as ModelHermiteSpline;
			    tr.Commit();
		    }
			return spline;
		}
		

		public void CreatePlinthInstance2()
		{
			Document document = ActiveUIDocument.Document;
			FamilySymbol symbol;
			FamilyOption opt = new FamilyOption();
			
			using( Transaction tr = new Transaction(document, "CreatePlinthInstance"))
			{
				tr.Start();
				document.LoadFamilySymbol("D:\\Tsao\\Developement\\RevitTrackModeling\\6pt.rfa", "6pt", opt,out symbol);
			    // Create a new instance of an adaptive component family
			    FamilyInstance instance = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(document, symbol);
				
			    // Get the placement points of this instance
			    IList<ElementId> placePointIds = new List<ElementId>();
			    placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(instance);
			    double x = 0;
			
			    // Set the position of each placement point
			    foreach (ElementId id in placePointIds)
			    {
			        ReferencePoint point = document.GetElement(id) as ReferencePoint;
			        point.Position = new Autodesk.Revit.DB.XYZ(10*x, 10*Math.Cos(x), 0);
			        x += Math.PI/6;
			        //point.SetPointElementReference(
			        	//shall be hosted, therefore refer to https://csharp.hotexamples.com/examples/-/PointLocationOnCurve/-/php-pointlocationoncurve-class-examples.html
			    }
			    tr.Commit();
			}
			
		}
		
//		public Value Evaluate(FSharpList<Value> args)
//		public void Evaluate()
//        {
//            if (!args[0].IsList)
//                throw new Exception("A list of UVs is required to place the Adaptive Component.");
//
//            FSharpList<Value> parameters = ((Value.List)args[0]).Item;
//
//            var curveRef = ((Value.Container)args[1]).Item as Reference;
//            var c = curveRef == null
//                         ? (Curve)((Value.Container)args[1]).Item
//                         : (Curve)dynRevitSettings.Doc.Document.GetElement(curveRef.ElementId).GetGeometryObjectFromReference(curveRef);
//
//            var fs = (FamilySymbol)((Value.Container)args[2]).Item;
//
//            FamilyInstance ac = null;
//
//            //if the adapative component already exists, then move the points
//            if (Elements.Any())
//            {
//                //...we attempt to fetch it from the document...
//                if (dynUtils.TryGetElement(this.Elements[0], out ac))
//                {
//                    ac.Symbol = fs;
//                }
//                else
//                {
//                    //create
//                    ac = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(dynRevitSettings.Doc.Document, fs);
//                    Elements[0] = ac.Id;
//                }
//            }
//            else
//            {
//                //create
//                ac = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(dynRevitSettings.Doc.Document, fs);
//                Elements.Add(ac.Id);
//            }
//
//            if (ac == null)
//                throw new Exception("An adaptive component could not be found or created.");
//
//            IList<ElementId> placePointIds = new List<ElementId>();
//            placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(ac);
//
//            if (placePointIds.Count() != parameters.Count())
//                throw new Exception("The input list of UVs does not have the same number of values required by the adaptive component.");
//
//            // Set the position of each placement point
//            int i = 0;
//            foreach (ElementId id in placePointIds)
//            {
//                var t = ((Value.Number)parameters.ElementAt(i)).Item;
//                var point = dynRevitSettings.Doc.Document.GetElement(id) as ReferencePoint;
//                var ploc = new PointLocationOnCurve(PointOnCurveMeasurementType.NonNormalizedCurveParameter, t,
//                                                    PointOnCurveMeasureFrom.Beginning);
//                var peref = dynRevitSettings.Revit.Application.Create.NewPointOnEdge(c.Reference, ploc);
//                point.SetPointElementReference(peref);
//                i++;
//            }
//
//            return Value.NewContainer(ac);
//        }
		
	    void SetElementParameterInMm(Element e, string parameter_name, double lengthInMm )
	    {
	    	if(e.GetParameters( parameter_name ).Count != 0)
	    	e.GetParameters( parameter_name ).First()
	        .Set( UnitUtils.Convert( lengthInMm, DisplayUnitType.DUT_MILLIMETERS,DisplayUnitType.DUT_DECIMAL_FEET) );
	    }
	    
	    void SetElementParameterInt(Element e, string parameter_name, int value )
	    {
	    	if(e.GetParameters( parameter_name ).Count != 0)
	    	e.GetParameters( parameter_name ).First()
	        .Set( value );
	    }
	    
	    void SetElementParameterDouble(Element e, string parameter_name, double value )
	    {
	    	if(e.GetParameters( parameter_name ).Count != 0)
	    	e.GetParameters( parameter_name ).First()
	        .Set( value );
	    }
	    

	    double GetElementParameterInMm(Element e, string parameter_name )
	    {
	    	return UnitUtils.Convert(e.GetParameters( parameter_name ).First().AsDouble(), DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS);
	    }

		
		public void AppSetup()
		{
			
			frmOptions opt = new frmOptions();
			bool bExit = false;
				while(true)
				{
					bExit = false;
					opt.ShowDialog();
					switch (GetActiveFunction()) {
					case "RegMCidBtnClick": 
						RegMCid();
						break;
					case "DrawMCBtnClick": 
						//DrawMC();
						break;
					case "BtnExitClick": 
						bExit= true;
						break;
					default:
						
						break;
					}
					
					if (bExit) break;
					
				}
		}
		
		public void CrearteTrack()
		{
            //UIDocument uidoc = this.ActiveUIDocument;
            
            UIApplication uiapp = this as UIApplication;
            
				bool bExit=false;
				CCreateTrack ct = new CCreateTrack(uiapp);
				while(true)
				{
					ct.Run();
					switch (GetActiveFunction()) {
					case "BatchBuildBtnClick": 
						BatchBuild();
						break;
					case "GetChainageBtnClick": 
						PickPoint2GetLocalChainage();
						break;
					case "GetLengthBtnClick": 
						PickPoint2GetLength();
						break;
					case "ManualBuildBtnClick": 
						ManualBuild();
						break;
					case "BtnDrawCenterLineClick": 
						AddTrackCenterline();
						break;
					case "createTrackFamilyBtnClick": 
						createTrackFamily();
						bExit= true;
						break;
					case "GenerateComponentFamiliesBtnClick": 
						GenerateComponentFamilies();
						bExit= true;
						break;
					case "AppSetupBtnClick":
						AppSetup();
						bExit= true;
						break;
					case "BtnExitClick": 
						bExit= true;
						break;
					default:
						
						break;
					}
					
					if (bExit) break;
					
				}
				
		}
		
		public void ManualBuild()
		{
			Document doc = ActiveUIDocument.Document;
			
			double Chainge = double.Parse(Interaction.GetSetting("AlignmentTools","AppData","textChainage","780.15"));
			double Length = double.Parse(Interaction.GetSetting("AlignmentTools","AppData","textLength","14.7"));
			double EndChainage = Chainge + Length;
			string Name = Interaction.GetSetting("AlignmentTools","AppData","textName","0102-UP-053");
			
			TrackCenterLine tcl1 = new TrackCenterLine("ALDName1");
            ModelCurve mc = doc.GetElement(GetActiveMC()) as ModelCurve ;
			
			AddOnePlinth(Name,Chainge, EndChainage, mc, tcl1);
			
		}
		
		public void ReadLandXML() //Read LandXML string into DataStorage
		{
			//Select a LandXML file
			System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.Title = "Browse LandXML Files";
			openFileDialog1.InitialDirectory = @"D:\AlignmentTools\DataTable\";
			openFileDialog1.DefaultExt = "xml";
			openFileDialog1.Filter = "LandXML Files (*.xml)|*.xml|All files (*.*)|*.*";
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
			//Read into a string
				string xmlString =  System.IO.File.ReadAllText(openFileDialog1.FileName);
				//TaskDialog.Show("LandXML", "Selected LandXML file contents: " + xmlString);
			//Attach to a DataStorage
				SaveDataViaDataStorage(System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName), xmlString);
			}
		}

		public void ExportLandXML2Excel()
		{
			string xmlString = GetDataViaDataStorage("新莊機廠");
			LandXML2Excel(xmlString);
		}

		
		private void SaveDataViaDataStorage(string ALDName, string MCid) //in fact this fuction will Save or update the Data.
		{
			Document doc = this.ActiveUIDocument.Document;
			try {
				FilteredElementCollector collector =
			  new FilteredElementCollector( doc );
			
			var dataStorage =
			  collector
			  .OfClass( typeof( DataStorage ) )
				.Where(q=>q.GetEntity(CreatedAlignmentDataSchema.GetSchema()).IsValid())
				.Where(q=>q.GetEntity(CreatedAlignmentDataSchema.GetSchema()).Get<string>("PrjName") == ALDName).First();
			
				if( dataStorage != null )
				{
//					dataStorage.GetEntity(CreatedMCidSchema.GetSchema()).Set("MCid", MCid );
//					return ;			
				using( Transaction t = new Transaction( doc, "Delete Data" ) )
			  	{
				    t.Start();
				    doc.Delete(dataStorage.Id);
				    t.Commit();
				}
				}
				
					
			} catch (System.InvalidOperationException) {}			
				using( Transaction t = new Transaction( doc, "Create created MCid" ) )
			  	{
				    t.Start();
				 
				    // Create data storage in new document
				 
				    DataStorage createdInfoStorage 
				      = DataStorage.Create( doc );
				 
				    // Create entity which store created info
				 
				    Entity entity = new Entity( 
				      CreatedAlignmentDataSchema.GetSchema() );
				 
				    entity.Set( "PrjName", 
				      ALDName );
				 
				    entity.Set( "XML", 
				      MCid );
				 
				    // Set entity to the data storage element
				 
				    createdInfoStorage.SetEntity( entity );
				 
				    t.Commit();
			
			}
		
		}

		private string GetDataViaDataStorage(string ALDName) //in fact this fuction will read xmlString from the DataStorage.
		{
			Document doc = this.ActiveUIDocument.Document;
			try {
				FilteredElementCollector collector =
			  new FilteredElementCollector( doc );
			
			var dataStorage =
			  collector
			  .OfClass( typeof( DataStorage ) )
				.Where(q=>q.GetEntity(CreatedAlignmentDataSchema.GetSchema()).IsValid())
				.Where(q=>q.GetEntity(CreatedAlignmentDataSchema.GetSchema()).Get<string>("PrjName") == ALDName).First();
			
				if( dataStorage != null )
				{
					return dataStorage.GetEntity(CreatedAlignmentDataSchema.GetSchema()).Get<string>("XML");			
				}
				
				return null;
					
			} catch (System.InvalidOperationException) { return null;}			
		}
		
		private void LandXML2Excel(string xmlString)
		{
			//Open Excel Workbook
            System.Diagnostics.Process.Start("EXCEL.EXE", @"D:\AlignmentTools\DataTable\AlignmentDataSheets.xlsx");
            
  			X.Application excel = null;
  			
  			while(null == excel )
  			{
  				try {
	  				excel = (X.Application)Marshal.GetActiveObject("Excel.Application");  						
	  				if( excel.ActiveWorkbook.Name != "AlignmentDataSheets.xlsx")
	  				{
	  					excel = null;
	  					TaskDialog.Show("LandXML to Excel", "AlignmentDataSheets.xlsx Shall be the only opened workbook in advance!");
	  				}
  				} catch (Exception) {  				
					excel = null;				
  				}
  			}
  			
//			if( null != excel )
//			{
//				
//				TaskDialog.Show("LandXML to Excel", excel.ActiveWorkbook.Name);
//				
//			}
          
			//Horizontal Alignment data			
			XmlTextReader reader = new XmlTextReader( new System.IO.StringReader(xmlString) );
            reader.Namespaces = false;
			reader.Read();
            XPathDocument document = new XPathDocument(reader);
            XPathNavigator navigator = document.CreateNavigator();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(reader.NameTable);
            nsmgr.AddNamespace("ns", "http://www.landxml.org/schema/LandXML-1.1");
            XPathNodeIterator nodes = navigator.Select("//LandXML/Alignments/Alignment/CoordGeom/*", nsmgr);
            string dir = "";
            string[] XYZ;
            X.Worksheet worksheet = (X.Worksheet) excel.ActiveSheet;
            int j = excel.ActiveCell.Row;
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes.MoveNext();
                XYZ = nodes.Current.GetChildNodeValue("Start").Trim().Split(new Char[] { ' ' });
                if (nodes.Current.Name.Equals("Line")) dir = "dir"; else dir = "dirStart";
                worksheet.Cells[j,1] = nodes.Current.Name.ToString();
                    worksheet.Cells[j,2] = XYZ[0];
                    worksheet.Cells[j,3] = XYZ[1];
                    worksheet.Cells[j,4] = nodes.Current.GetAttribute("staStart", nodes.Current.GetNamespace("ns"));
                    worksheet.Cells[j,5] = nodes.Current.GetAttribute(dir, nodes.Current.GetNamespace("ns"));
                    worksheet.Cells[j,6] = nodes.Current.GetAttribute("length", nodes.Current.GetNamespace("ns"));
                    worksheet.Cells[j,7] = i;
                    j++;
            }
            //--------------------------------------------------------------------------------------------------*/
			
			//Vertical Alignment data
			//Other data
			
		}


	}
	
	class FamilyOption : IFamilyLoadOptions
	{
	
	    public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)		
	    {		
	        overwriteParameterValues = true;
	        return true;
	    }	
	 
	
	    public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
	    {
	        source = FamilySource.Family;
	        overwriteParameterValues = true;
	        return true;
	    }
	
	}
	
	static class CreatedInfoSchema
	{
		static readonly Guid schemaGuid = new Guid( "{5FAF80CB-901C-4B25-83A7-98FDD2017FB8}" );
		
		public static Schema GetSchema()
		{
		  // looking for existing schema
		  Schema schema = Schema.Lookup( schemaGuid );
		  if( schema != null )
		    return schema;
		
		  //If schema doesn't exists
		  //create new schema
		
		  SchemaBuilder schemaBuilder =
		      new SchemaBuilder( schemaGuid );
		  schemaBuilder.SetSchemaName( "CreatedInfo" );
		
		  schemaBuilder.AddSimpleField( "CreatedUser", typeof( String ) );
		  schemaBuilder.AddSimpleField( "CreatedDate", typeof( String ) );
		
		  return schemaBuilder.Finish();
		}
	}

	static class CreatedMCidSchema
	{
		static readonly Guid schemaGuid = new Guid( "{5438F160-4B59-4D2E-9CBD-D4E6BEC307E5}" );
		
		public static Schema GetSchema()
		{
		  // looking for existing schema
		  Schema schema = Schema.Lookup( schemaGuid );
		  if( schema != null )
		    return schema;
		
		  //If schema doesn't exists
		  //create new schema
		
		  SchemaBuilder schemaBuilder =
		      new SchemaBuilder( schemaGuid );
		  schemaBuilder.SetSchemaName( "CreatedMCid" );
		
		  schemaBuilder.AddSimpleField( "ALDName", typeof( String ) );
		  schemaBuilder.AddSimpleField( "MCid", typeof( String ) );
		
		  return schemaBuilder.Finish();
		}
	}
	
	static class CreatedAlignmentDataSchema //The Schema for the XML alignment data
	{
		static readonly Guid schemaGuid = new Guid( "{A08FC855-7748-4DAC-8ACA-9956AC0E7CEC}" );
		
		public static Schema GetSchema()
		{
		  // looking for existing schema
		  Schema schema = Schema.Lookup( schemaGuid );
		  if( schema != null )
		    return schema;
		
		  //If schema doesn't exists
		  //create new schema
		
		  SchemaBuilder schemaBuilder =
		      new SchemaBuilder( schemaGuid );
		  schemaBuilder.SetSchemaName( "CreatedAlignmentData" );
		
		  schemaBuilder.AddSimpleField( "PrjName", typeof( String ) );
		  schemaBuilder.AddSimpleField( "XML", typeof( String ) );
		
		  return schemaBuilder.Finish();
		}
	}
	
	

}