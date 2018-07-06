/*
 * Created by SharpDevelop.
 * User: jack.tsao
 * Date: 2018/5/23
 * Time: 上午 09:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace MyAppMacro
{
	public class mySelectionFilter : ISelectionFilter
	{
		
		
		public bool AllowElement(Element elem)
		{
			return elem.Category.Name == "房間";
		}
		
		public bool AllowReference(Reference reference, XYZ position)
		{
			return true;
		}
	}
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("D0E49DF8-F6F6-4EAE-ACF3-F221C8201636")]
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
		public void PutRemarkToElement()
		{
			Document doc=ActiveUIDocument.Document;
			UIDocument uidoc=ActiveUIDocument;
			
			ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
			
            foreach (ElementId e in selection)
            {
                //Element obj = doc.GetElement(e);
                HiLighted(e, uidoc, doc);
                uidoc.ShowElements(e);
                uidoc.RefreshActiveView();
                try {
	 				Reference Pick2 = uidoc.Selection.PickObject(ObjectType.Element, "選取標示文字：");                		
                } catch  {
                	
                	break;
                }
 				
 				UnHiLighted(e, uidoc, doc);
                uidoc.RefreshActiveView();
           }
			
//			Reference PickOnce = uidoc.Selection.PickObject(ObjectType.Element, "選取元件：");
//			Element ref_object=doc.GetElement(PickOnce.ElementId);
//			//IList<Parameter> paras = ref_object.GetAllParameters();
//			Parameter Remark = ref_object.get_Parameter(BuiltInParameter.ALL_MODEL_MARK);
//			//String str=Remark.AsString(); //取出標註值。
			
			
			//Get the Mark Text to be put into object Mark
			//TextNote ref_object2=doc.GetElement(Pick2.ElementId) as TextNote;
			
			//FormattedText formatText = ref_object2.GetFormattedText();
			
//			using (Transaction tx = new Transaction(doc))
//            {
//			 	tx.Start("Change Object Mark");
//				Remark.Set("Hello");
//				tx.Commit();
//			}
			
			
		}
		
		public void TestAllParameters()
		{
			Document doc=ActiveUIDocument.Document;
			UIDocument uidoc=ActiveUIDocument;

			StringBuilder levelInformation = new StringBuilder();
		    int levelNumber = 0;
		    FilteredElementCollector collector = new FilteredElementCollector(doc);
		    ICollection<Element> collection = collector.OfClass(typeof(Level)).ToElements();
		    foreach (Element e in collection)
		    {
		        Level level = e as Level;
		
		        if (null != level)
		        {
		            // keep track of number of levels
		            levelNumber++;
		
		            //get the name of the level
		            levelInformation.Append("\nLevel Name: " + level.Name);
		
		            //get the elevation of the level
		            levelInformation.Append("\n\tElevation: " + UnitUtils.Convert(level.Elevation, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS).ToString());
		
		            // get the project elevation of the level
		            levelInformation.Append("\n\tProject Elevation: " + UnitUtils.Convert(level.ProjectElevation, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS).ToString());
		        }
		    }
		
		    //number of total levels in current document
		    levelInformation.Append("\n\n There are " + levelNumber + " levels in the document!");
		
		    //show the level information in the messagebox
		    TaskDialog.Show("Revit",levelInformation.ToString());
		}
		public void TestColumnParameters()
		{
			Document doc=ActiveUIDocument.Document;
			UIDocument uidoc=ActiveUIDocument;
			ElementClassFilter f1 = new ElementClassFilter(typeof( FamilyInstance ));
			StringBuilder ColumnInformation = new StringBuilder();
//		    FilteredElementCollector collector = GetStructuralElements(doc);
		    FilteredElementCollector collector = new FilteredElementCollector(doc);
		    collector.WherePasses(f1);
//		    foreach (var e in collector) {
//		    	ColumnInformation.Append("\nCategory Name: " + e.Category.Name);
//		    }
//		    TaskDialog.Show("Category Info", ColumnInformation.ToString());
//		    ColumnInformation.Clear();
		    
		    List<Element> collection = collector.Where(q=>q.Category.Name == "結構柱").ToList();
		    foreach (Element e in collection) { //First element will be done only do to the break; in the last.
		    	ColumnInformation.Clear();
		    	FamilyInstance column = e as FamilyInstance;
		    	ColumnInformation.Append("\nColumn Name: " + column.Name);
		    	foreach (Parameter param in column.Parameters) {
		    	
		    		switch(param.StorageType){
		    		case StorageType.String:
		    			ColumnInformation.Append("\n" + param.Definition.Name + enumName(param) +" = "+ param.AsString());
			    		break;
		    		case StorageType.Double:
			    		ColumnInformation.Append("\n" + param.Definition.Name + enumName(param) +" = "+ param.AsDouble().ToString());
			    		break;
		    		case StorageType.Integer:
			    		ColumnInformation.Append("\n" + param.Definition.Name + enumName(param) +" = "+ param.AsInteger().ToString());
			    		break;
		    		case StorageType.ElementId:
			    		ColumnInformation.Append("\n" + param.Definition.Name + enumName(param) +" = "+ param.AsElementId().ToString());
			    		break;
		    		case StorageType.None:
			    		ColumnInformation.Append("\n" + param.Definition.Name + enumName(param) +" = "+ "None");
			    		break;
			    	default:
			    		break;	
		    		}
		    	}
		    TaskDialog.Show("Column Info", ColumnInformation.ToString());
		    break;
		    }
		    
		    
		}
		
		private string enumName(Parameter param)
		{
			int dbValue = param.Id.IntegerValue;
			return "("+Enum.GetName(typeof(BuiltInParameter), dbValue)+")";
		}

		public void LevelIsolation()
		{
            Document doc = doc=ActiveUIDocument.Document; 
            UIDocument uidoc=ActiveUIDocument;
 			using (Transaction t = new Transaction(doc, "Highlight element"))
			{
				  t.Start();
		            LevelIsolator li = new LevelIsolator(doc, uidoc);
		            li.ShowDialog();
			      t.Commit();
			}
		}
		
		public void LevelIsolation2()
		{
            Document doc = doc=ActiveUIDocument.Document; 
            UIDocument uidoc=ActiveUIDocument;
 			using (Transaction t = new Transaction(doc, "Change Object Reference Level"))
			{
				  t.Start();
		            ChangeObjRefLevel corl = new ChangeObjRefLevel(doc, uidoc);
		            corl.ShowDialog();
			      t.Commit();
			}
 			

		}
		public void ChangeObjRefLevel()
		{
//			UIApplication uiapp = commandData.Application;
//            UIDocument uidoc = uiapp.ActiveUIDocument;
//            Application app = uiapp.Application;
            Document doc = doc=ActiveUIDocument.Document; 
            UIDocument uidoc=ActiveUIDocument;
            
			
			
            //Get current selection and store it
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();
            Reference hasPickOne = uidoc.Selection.PickObject(ObjectType.Element, "選取物件，之後程式會抓取它所參考的樓層：");
            

            // Retrieve needed information from reference object

            Element ref_object = doc.GetElement(hasPickOne.ElementId);
            ElementId ref_levelid = doc.GetElement(hasPickOne.ElementId).LevelId;
            Level ref_level = doc.GetElement(ref_levelid) as Level;
//            TaskDialog.Show("Revit Tools", ref_level.Name);
            IList<Parameter> paras = ref_object.GetParameters("起始樓層偏移");
            Parameter object_ref_level = ref_object.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Enable reference level");
                foreach (ElementId e in selection)
                {
                    Element beam = doc.GetElement(e);
                    if (null != beam.get_Parameter(BuiltInParameter.SKETCH_PLANE_PARAM ))
                    {
	                    Parameter object_param_level = beam.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);                    
	                    if (object_param_level!= null)   //Beam
	                    {
	                          double elevOldSta = beam.get_Parameter(
						        BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION )
						          .AsDouble();
						 
						      double elevOldEnd = beam.get_Parameter(
						        BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION )
						          .AsDouble();
						 
						      double elevTmpSta = elevOldSta + 1.0;
						      double elevTmpEnd = elevOldEnd + 1.0;
						 
						      // This will "detach from plane"...
						 
						      beam.get_Parameter(
						        BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION )
						          .Set( elevTmpSta );
						 
						      beam.get_Parameter(
						        BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION )
						          .Set( elevTmpEnd );
						 
						      // ...and this move back to the 
						      // same original position
						 
						      beam.get_Parameter(
						        BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION )
						          .Set( elevOldSta );
						 
						      beam.get_Parameter(
						        BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION )
						          .Set( elevOldEnd );
	                    }
                    }
                }
                tx.Commit();
            }            

            
            // Modify document within a transaction

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Change reference level");
                foreach (ElementId e in selection)
                {
                    Element Object = doc.GetElement(e);
                    
                    Parameter object_param_level = Object.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);                    
                    if (object_param_level!= null)   //Beam
                    {
                    	
                    	Level object_Level = doc.GetElement(object_param_level.AsElementId())  as Level;
                    	Parameter object_LevelElevation = Object.get_Parameter(BuiltInParameter.STRUCTURAL_REFERENCE_LEVEL_ELEVATION);
                        Parameter object_param_offset1 = Object.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION);
                        double object_newoffset1 = object_param_offset1.AsDouble() + object_LevelElevation.AsDouble() - ref_level.Elevation;
                        Parameter object_param_offset2 = Object.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION);
                        double object_newoffset2 = object_param_offset2.AsDouble() + object_LevelElevation.AsDouble() - ref_level.Elevation;
                        
                        object_param_level.Set(ref_levelid);
                        object_param_offset1.Set(object_newoffset1);
                        object_param_offset2.Set(object_newoffset2);
                    }
                    
                    object_param_level = Object.get_Parameter(BuiltInParameter.LEVEL_PARAM);                    
                    if (object_param_level != null)  //Floor                  	
                    {
                    	
                        Level object_Level = doc.GetElement(Object.LevelId) as Level;
                        Parameter object_param_offset = Object.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM);
                        double object_newoffset = object_param_offset.AsDouble() + object_Level.Elevation - ref_level.Elevation;
                        object_param_level.Set(ref_levelid);
                        object_param_offset.Set(object_newoffset);
                    }
                    
                    if (Object.GetType().Name == "Wall") //Wall
                    {
                    	Level object_Level = doc.GetElement(Object.LevelId) as Level;
                     	double a = Object.GetParameters("基準偏移")[0].AsDouble();
                    	double b = Object.GetParameters("頂部偏移")[0].AsDouble();
                    	
                    	Wall wall=Object as Wall;
                    	Level BaseLevel = doc.GetElement(wall.LookupParameter("底部約束").AsElementId()) as Level;
                    	Level TopLevel = doc.GetElement(wall.LookupParameter("頂部約束").AsElementId()) as Level;
                    	double BaseLevelElevation = BaseLevel.Elevation;
                    	
                    	wall.LookupParameter("底部約束").Set(ref_levelid);
                    	Object.GetParameters("基準偏移")[0].Set(a+BaseLevelElevation-ref_level.Elevation);
                    }
                }
                tx.Commit();
            }            

		}
				
		private void HiLighted(ElementId elementId, UIDocument uiDoc, Document doc)
		{
			OverrideGraphicSettings ogs = new OverrideGraphicSettings();
//			Color red = new Color(255, 0, 0);
			Element solidFill = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).Where(q => q.Name.Contains("單色填滿")).First();
			
//			ogs.SetProjectionLineColor(red);
//			ogs.SetProjectionLineWeight(8);
			ogs.SetProjectionFillPatternId(solidFill.Id);
			ogs.SetProjectionFillColor(new Color(0, 255, 0));
			ogs.SetCutFillPatternId(solidFill.Id);
			ogs.SetCutFillColor(new Color(0, 255, 0));
			
			using (Transaction t = new Transaction(doc, "Highlight element"))
			{
				  t.Start();
			      try
			      {
			             // elementId = Id of element you wish to highlight
			             uiDoc.ActiveView.SetElementOverrides(elementId, ogs);
			      }
			      catch (Exception ex)
			      {
			             TaskDialog.Show("Exception", ex.ToString());
			      }
			      uiDoc.RefreshActiveView();
			      t.Commit();
			}
		
		}
		
		private void UnHiLighted(ElementId elementId, UIDocument uiDoc, Document doc)
		{
			OverrideGraphicSettings ogs = new OverrideGraphicSettings();
//			Color red = new Color(255, 0, 0);
			Element solidFill = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).Where(q => q.Name.Contains("單色填滿")).First();
			
//			ogs.SetProjectionLineColor(red);
//			ogs.SetProjectionLineWeight(8);
			ogs.SetProjectionFillPatternId(ElementId.InvalidElementId);
			ogs.SetProjectionFillColor(Color.InvalidColorValue);
			ogs.SetCutFillPatternId(ElementId.InvalidElementId);
			ogs.SetCutFillColor(Color.InvalidColorValue);
			
			using (Transaction t = new Transaction(doc, "Highlight element"))
			{
				  t.Start();
			      try
			      {
			             // elementId = Id of element you wish to highlight
			             uiDoc.ActiveView.SetElementOverrides(elementId, ogs);
			      }
			      catch (Exception ex)
			      {
			             TaskDialog.Show("Exception", ex.ToString());
			      }
			      uiDoc.RefreshActiveView();
			      t.Commit();
			}
		
		}

		public void HiLight()
		{
			UIDocument uidoc=ActiveUIDocument;
			
			Reference r = uidoc.Selection.PickObject (ObjectType.Element, "Select an element");
			
			Element elem= uidoc.Document.GetElement(r);
			
			// Add the element to the selection set to highlight it
			//
			
			ICollection<ElementId> ids = new List<ElementId>();

			ids.Add(elem.Id);
			
			UIDocument uiDoc = new UIDocument(ActiveUIDocument.Document);
			
			uiDoc.Selection.SetElementIds(ids);
			
			uiDoc.ShowElements(ids);
			
			
			
			r = uidoc.Selection.PickObject (ObjectType.Element, "Select an element again:");
			     
 		}
		public void ChangeColor()
		{
		    Document doc = this.ActiveUIDocument.Document;
		    UIDocument uidoc = this.ActiveUIDocument;
		    Element solidFill = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).Where(q => q.Name.Contains("單色填滿")).First();
		    ElementId id = uidoc.Selection.PickObject(ObjectType.Element,"Select an element").ElementId;
		    OverrideGraphicSettings ogs = new OverrideGraphicSettings();
		    ogs.SetProjectionLineColor(new Color(0,255,0));
		    ogs.SetProjectionFillPatternId(solidFill.Id);
		    using (Transaction t = new Transaction(doc,"Set Element Override"))
		    {
		        t.Start();
		        doc.ActiveView.SetElementOverrides(id, ogs);
		        t.Commit();
		    }
		}
		public void TestFilter()
		{
		    Document doc = this.ActiveUIDocument.Document;
		    UIDocument uidoc = this.ActiveUIDocument;
			
			// get the familyInstance we want
			
			ElementClassFilter f1
            = new ElementClassFilter(
              typeof( FamilyInstance ) );
			
			
			ElementCategoryFilter f2       // Can get all family symbol
            = new ElementCategoryFilter(
              BuiltInCategory.OST_Columns );
			
			LogicalAndFilter f3
            = new LogicalAndFilter( f1, f2 );
			
			ElementClassFilter f4
            = new ElementClassFilter(
              typeof( Floor ) );
			

          FilteredElementCollector collector
            = new FilteredElementCollector( doc );
          
          collector = GetStructuralElements(doc);
			
          List<Element> columns = collector
            .WherePasses( f1 )
            .ToElements() as List<Element>;
          
//          List<Element> columnInstances
//            = ( from instances in columns
//                where instances is FamilyInstance
//                select instances ).ToList<Element>();
          int n=columns.Count;
          
          var query = from element in collector
          	//where element.Category.Name == "結構柱"
          	where element.Category.Name == "結構構架"
            select element;

// Cast found elements to family instances, 
// this cast to FamilyInstance is safe because ElementClassFilter for FamilyInstance was used
List<FamilyInstance> familyInstances = query.Cast<FamilyInstance>().ToList<FamilyInstance>();
			
		}
	
		public void CategorySample()
		{
			Element selectedElement = null;
			UIDocument uidoc = this.ActiveUIDocument;
			Document document = this.ActiveUIDocument.Document;
			foreach (ElementId id in uidoc.Selection.GetElementIds())
			{
			    selectedElement = document.GetElement(id);
			    break;  // just get one selected element
			}
			
			// Get the category instance from the Category property
			Category category = selectedElement.Category;
			
			BuiltInCategory enumCategory = (BuiltInCategory)category.Id.IntegerValue;
			
			// Format the prompt string, which contains the category information
			String prompt = "The category information of the selected element is: ";
			prompt += "\n\tName:\t" + category.Name;   // Name information
			
			prompt += "\n\tId:\t" + enumCategory.ToString();    // Id information
			prompt += "\n\tParent:\t";
			if (null == category.Parent)
			{
			    prompt += "No Parent Category";   // Parent information, it may be null
			}
			else
			{
			    prompt += category.Parent.Name;
			}
			
			prompt += "\n\tSubCategories:"; // SubCategories information, 
			CategoryNameMap subCategories = category.SubCategories;
			if (null == subCategories || 0 == subCategories.Size) // It may be null or has no item in it
			{
			    prompt += "No SubCategories;";
			}
			else
			{
			    foreach (Category ii in subCategories)
			    {
			        prompt += "\n\t\t" + ii.Name;
			    }
			}
			
			// Give the user some information
			TaskDialog.Show("Revit",prompt);

		}
	
		private FilteredElementCollector GetStructuralElements(Document doc)
		{
		  // What categories of family instances
		  // are we interested in?
		 
		  BuiltInCategory[] bics = new BuiltInCategory[] {
		    BuiltInCategory.OST_StructuralColumns,
		    BuiltInCategory.OST_StructuralFraming,
		    BuiltInCategory.OST_StructuralFoundation,
		    BuiltInCategory.OST_Floors,
		    BuiltInCategory.OST_Ramps
		  };
		 
		  IList<ElementFilter> a
		    = new List<ElementFilter>( bics.Length );
		 
		  foreach( BuiltInCategory bic in bics )
		  {
		    a.Add( new ElementCategoryFilter( bic ) );
		  }
		 
		  LogicalOrFilter categoryFilter
		    = new LogicalOrFilter( a );
		 
		  // Filter only for structural family 
		  // instances using concrete or precast 
		  // concrete structural material:
		 
		  List<ElementFilter> b
		    = new List<ElementFilter>( 2 );
		 
		  b.Add( new StructuralMaterialTypeFilter( 
		    StructuralMaterialType.Steel ) );
		 
		  b.Add( new StructuralMaterialTypeFilter( 
		    StructuralMaterialType.Steel ) );
		 
		  LogicalOrFilter structuralMaterialFilter 
		    = new LogicalOrFilter( b );
		 
		  List<ElementFilter> c
		    = new List<ElementFilter>( 3 );
		 
		  c.Add( new ElementClassFilter( 
		    typeof( FamilyInstance ) ) );
		 
		  c.Add( structuralMaterialFilter );
		  c.Add( categoryFilter );
		 
		  LogicalAndFilter familyInstanceFilter
		    = new LogicalAndFilter( c );
		 
		  IList<ElementFilter> d
		    = new List<ElementFilter>( 6 );
		 
		  d.Add( new ElementClassFilter(
		    typeof( Wall ) ) );
		 
		  d.Add( new ElementClassFilter(
		    typeof( Floor ) ) );
		 
		  //d.Add( new ElementClassFilter(
		  //  typeof( ContFooting ) ) );
		 
		#if NEED_LOADS
		  d.Add( new ElementClassFilter(
		    typeof( PointLoad ) ) );
		 
		  d.Add( new ElementClassFilter(
		    typeof( LineLoad ) ) );
		 
		  d.Add( new ElementClassFilter(
		    typeof( AreaLoad ) ) );
		#endif
		 
		  d.Add( familyInstanceFilter );
		 
		  LogicalOrFilter classFilter
		    = new LogicalOrFilter( d );
		 
		  FilteredElementCollector col
		    = new FilteredElementCollector( doc )
		      .WhereElementIsNotElementType()
		      .WherePasses( classFilter );
		 
		  return col;
		}
		
		
		public void RoomRenumbering()
		{
			UIDocument uidoc= ActiveUIDocument;
			Document doc = uidoc.Document;
			
			try {
				int roomNumber = 1;			
				while (true) {
					Reference selRef = uidoc.Selection.PickObject(ObjectType.Element,new mySelectionFilter(),"Select a room");
					
					Room room = (Room) doc.GetElement(selRef.ElementId);
					FilteredElementCollector collector = new FilteredElementCollector(doc);
					collector.OfClass(typeof(SpatialElement));
					collector.WherePasses(new RoomFilter());
					
					ParameterValueProvider provider = new ParameterValueProvider(new ElementId(BuiltInParameter.ROOM_NUMBER));
					FilterStringEquals evaluator = new FilterStringEquals();
					FilterStringRule rule = new FilterStringRule(provider, evaluator, roomNumber.ToString(), false);
					ElementParameterFilter filter = new ElementParameterFilter(rule);
					collector.WherePasses(filter);
					
					IList<Element> rooms = collector.ToElements();
					using (Transaction t = new Transaction(doc, "Modify room number")) {
						t.Start();
						if (rooms.Count > 0) 
							((Room) rooms[0]).Number = room.Number;					

						room.Number = roomNumber.ToString();
						doc.Regenerate();
						t.Commit();
					}
					
					
					roomNumber++;
				}

			} catch (Autodesk.Revit.Exceptions.OperationCanceledException)
			{ }
		}
		public void MarkReverseColumn()
		{
			UIDocument uidoc = ActiveUIDocument;
			Document doc = uidoc.Document;
			
			ElementClassFilter f1 = new ElementClassFilter(typeof(FamilyInstance));
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			collector.WherePasses(f1);
			List<Element> columns = collector.Where(q=>q.Category.Name == "結構柱").ToList();
			
			foreach (Element e in columns) {
				Level BottomLevel = doc.GetElement(e.LookupParameter("基準樓層").AsElementId()) as Level;
				Level TopLevel = doc.GetElement(e.LookupParameter("頂部樓層").AsElementId()) as Level;
				
				double BottomElevation = e.LookupParameter("基準偏移").AsDouble() + BottomLevel.Elevation;
				double TopElevation = e.LookupParameter("頂部偏移").AsDouble() + TopLevel.Elevation;
				
				if (TopElevation < BottomElevation)
				{
					HiLighted(e.Id, uidoc, doc);
				}
				else
				{
					UnHiLighted(e.Id, uidoc, doc);
				}
				
			}
		}
		public void MarkUntagedFrame()
		{
			UIDocument uidoc = ActiveUIDocument;
			Document doc = uidoc.Document;
			
			ElementClassFilter f1 = new ElementClassFilter(typeof( FamilyInstance ));
			StructuralMaterialTypeFilter f2 = new StructuralMaterialTypeFilter(StructuralMaterialType.Concrete);
			ElementClassFilter f3 = new ElementClassFilter(typeof( Floor ));
			ElementClassFilter f4 = new ElementClassFilter(typeof( Wall ));
			LogicalAndFilter f5 = new LogicalAndFilter(f1, f2);
			LogicalOrFilter f6 = new LogicalOrFilter(f3, f4);
			LogicalOrFilter memberFilter = new LogicalOrFilter(f5, f6);

			FilteredElementCollector collector = new FilteredElementCollector(doc);
			collector.WherePasses(memberFilter);
			List<Element> members = collector.ToList();
			String Tag = null;
			foreach (Element e in members) {
				Tag = e.get_Parameter(BuiltInParameter.DOOR_NUMBER).AsString();
				
				if (Tag == "" || Tag == null)
				{
					HiLighted(e.Id, uidoc, doc);
				}
				else
				{
					UnHiLighted(e.Id, uidoc, doc);
				}
				
				Tag = null;
				
			}

		}
		
		public void MarkUntagedFrame2()
		{
			UIDocument uidoc = ActiveUIDocument;
			Document doc = uidoc.Document;
			
			BuiltInCategory[] bics = new BuiltInCategory[] {
		    BuiltInCategory.OST_StructuralColumns,
		    BuiltInCategory.OST_StructuralFraming,
		    BuiltInCategory.OST_StructuralFoundation,
		    BuiltInCategory.OST_Floors,
		    BuiltInCategory.OST_Ramps
		  };
		 
		  IList<ElementFilter> a
		    = new List<ElementFilter>( bics.Length );
		 
		  foreach( BuiltInCategory bic in bics )
		  {
		    a.Add( new ElementCategoryFilter( bic ) );
		  }
		 
		  LogicalOrFilter categoryFilter
		    = new LogicalOrFilter( a );
		  
		  ElementClassFilter f1 = new ElementClassFilter(typeof(FamilyInstance));
		  ElementClassFilter f2 = new ElementClassFilter(typeof(Floor));
		  
  
		  LogicalAndFilter f3
		    = new LogicalAndFilter( categoryFilter, f1);
		  LogicalAndFilter f4
		    = new LogicalAndFilter( categoryFilter, f2);
		  //StructuralMaterialTypeFilter 會把不是OST_StructuralFraming的物件過濾掉，即使他們的材料也是Concrete!!!!
		  StructuralMaterialTypeFilter f5 = new StructuralMaterialTypeFilter(StructuralMaterialType.Concrete);

		  LogicalOrFilter f6
		    = new LogicalOrFilter( f3, f4);
		  LogicalAndFilter memberFilter
		    = new LogicalAndFilter( f5, f6);
		

			FilteredElementCollector collector = new FilteredElementCollector(doc);
			collector.WherePasses(memberFilter);
			
			List<Element> members = collector.ToList();
			String Tag = null;
			foreach (Element e in members) {
				Tag = e.get_Parameter(BuiltInParameter.DOOR_NUMBER).AsString();
				
				if (Tag == "" || Tag == null)
				{
					HiLighted(e.Id, uidoc, doc);
				}
				else
				{
					UnHiLighted(e.Id, uidoc, doc);
				}
				
				Tag = null;
				
			}

		}

		public void LevelSelect()
		{
			UIDocument uidoc = ActiveUIDocument;
			Document doc = uidoc.Document;
			
			ElementId curLevel = uidoc.Selection.PickObject(ObjectType.Element, "Select Level for selected elements").ElementId;
			
			//LevelSelector ls = new LevelSelector(doc);
			
			//ls.Show();

		}
		public void create3DViewsWithSectionBox()
		{
			UIDocument uidoc = ActiveUIDocument;
			Document doc = uidoc.Document;
    // get list of all levels
    IList<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(l => l.Elevation).ToList();

    // get a ViewFamilyType for a 3D View
    ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
                                     OfClass(typeof(ViewFamilyType)).
                                     Cast<ViewFamilyType>()
                                     where v.ViewFamily == ViewFamily.ThreeDimensional
                                     select v).First();

    using (Transaction t = new Transaction(doc,"Create view"))
    {
        int ctr = 0;
        // loop through all levels
        foreach (Level level in levels)
        {
            t.Start(); 

            // Create the 3d view
            View3D view = View3D.CreateIsometric(doc, viewFamilyType.Id);

            // Set the name of the view
            view.Name = level.Name + " Section Box";

            // Set the name of the transaction
            // A transaction can be renamed after it has been started
            t.SetName("Create view " + view.Name);

            // Create a new BoundingBoxXYZ to define a 3D rectangular space
            BoundingBoxXYZ boundingBoxXYZ = new BoundingBoxXYZ();

            // Set the lower left bottom corner of the box
            // Use the Z of the current level.
            // X & Y values have been hardcoded based on this RVT geometry
            boundingBoxXYZ.Min = new XYZ(-50, -100, level.Elevation);

            // Determine the height of the bounding box
            double zOffset = 0;
            // If there is another level above this one, use the elevation of that level
            if (levels.Count > ctr + 1)
                zOffset = levels.ElementAt(ctr+1).Elevation;
            // If this is the top level, use an offset of 10 feet
            else
                zOffset = level.Elevation + 10;
            boundingBoxXYZ.Max = new XYZ(200, 125, zOffset);

            // Apply this bouding box to the view's section box
            //view.SectionBox = boundingBoxXYZ;
            view.SetSectionBox(boundingBoxXYZ);

            t.Commit();

            // Open the just-created view
            // There cannot be an open transaction when the active view is set
            uidoc.ActiveView = view;

            ctr++;            
        }
    }
			
		}
		public void CopyMarkToSN()
		{
		    Document doc = this.ActiveUIDocument.Document;			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			ElementClassFilter f1 = new ElementClassFilter(typeof( FamilyInstance ));
			StructuralMaterialTypeFilter f2 = new StructuralMaterialTypeFilter(StructuralMaterialType.Concrete);
			ElementClassFilter f3 = new ElementClassFilter(typeof( Floor ));
			ElementClassFilter f4 = new ElementClassFilter(typeof( Wall ));
			LogicalAndFilter f5 = new LogicalAndFilter(f1, f2);
			LogicalOrFilter f6 = new LogicalOrFilter(f3, f4);
			LogicalOrFilter f7 = new LogicalOrFilter(f5, f6);
			
			ICollection<Element> collection = collector.WherePasses(f7).ToElements();
			
			using( Transaction tx = new Transaction(doc, "CopyMarkToSN"))
			{
				tx.Start();
			    foreach (Element e in collection)
			    {
			    	Parameter pSN = e.LookupParameter("編號");
			    	
			    	if (null != pSN)
			    	{
//			    		Parameter pMark = e.LookupParameter("標註");
			    		Parameter pMark = e.get_Parameter(BuiltInParameter.DOOR_NUMBER);
			    		pSN.Set(pMark.AsString());
//			    		pSN.Set("");
			    	}
			    }
			    tx.Commit();
			}
			
		}
		public void ClearMarks()
		{
		    Document doc = this.ActiveUIDocument.Document;			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			ElementClassFilter f1 = new ElementClassFilter(typeof( FamilyInstance ));
			StructuralMaterialTypeFilter f2 = new StructuralMaterialTypeFilter(StructuralMaterialType.Concrete);
			ElementClassFilter f3 = new ElementClassFilter(typeof( Floor ));
			ElementClassFilter f4 = new ElementClassFilter(typeof( Wall ));
			LogicalAndFilter f5 = new LogicalAndFilter(f1, f2);
			LogicalOrFilter f6 = new LogicalOrFilter(f3, f4);
			LogicalOrFilter f7 = new LogicalOrFilter(f5, f6);
			
			ICollection<Element> collection = collector.WherePasses(f7).ToElements();
			
			using( Transaction tx = new Transaction(doc, "CopyMarkToSN"))
			{
				tx.Start();
			    foreach (Element e in collection)
			    {
			    	Parameter pSN = e.LookupParameter("編號");
			    	
			    	if (null != pSN)
			    	{
//			    		Parameter pMark = e.LookupParameter("標註");
			    		Parameter pMark = e.get_Parameter(BuiltInParameter.DOOR_NUMBER);
			    		pMark.Set("");
			    	}
			    }
			    tx.Commit();
			}
			
		}
	}
}