/*
 * 由SharpDevelop创建。
 * 用户： jack.tsao
 * 日期: 2018/7/4
 * 时间: 上午 11:31
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
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
	/// <summary>
	/// Description of LevelIsolator.
	/// </summary>
	public partial class LevelIsolator : System.Windows.Forms.Form
	{
		private Document m_doc;
		private UIDocument m_uidoc;
		private IList<Level> m_levels=null;
		public LevelIsolator(Document doc, UIDocument uidoc)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			m_doc = doc;
			m_uidoc = uidoc;
			
			m_levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(l => l.Elevation).ToList();
			foreach (Level level in m_levels) {
				cbLevel.Items.Add(level.Name);
			}
			
			cbLevel.SelectedIndex=0;
		}
		
		void BtnApplyClick(object sender, EventArgs e)
		{
			Level curLevel = m_levels.ElementAt(cbLevel.SelectedIndex);
			
			IEnumerable<Element> Allelem= JtElementExtensionMethods.SelectAllPhysicalElements(m_doc);
			List<ElementId> eIds= new List<ElementId>();
			Parameter p;
			IEnumerator<Element> a = Allelem.GetEnumerator();
			List<BuiltInParameter> bip1 = new List<BuiltInParameter>(){
				BuiltInParameter.FAMILY_BASE_LEVEL_PARAM,
				BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM,
				BuiltInParameter.SCHEDULE_LEVEL_PARAM,
				BuiltInParameter.WALL_BASE_CONSTRAINT,
				BuiltInParameter.FAMILY_LEVEL_PARAM,
				BuiltInParameter.STAIRS_BASE_LEVEL_PARAM,
				BuiltInParameter.ROOF_BASE_LEVEL_PARAM
			};
			
			List<BuiltInParameter> bip2 = new List<BuiltInParameter>(){
				BuiltInParameter.STAIRS_RAILING_BASE_LEVEL_PARAM    //Should set host in bip3
			};
			List<BuiltInParameter> bip3 = new List<BuiltInParameter>(){
				BuiltInParameter.STAIRS_BASE_LEVEL_PARAM  //Hosts for bip2
			};
			
			while (a.MoveNext()) {
				Element e1 = a.Current;
				try
				{
					foreach (BuiltInParameter bip in bip1) {
						p = e1.get_Parameter(bip);
						if (null != p)
						{
							Level p_Level = m_doc.GetElement(p.AsElementId())  as Level;
							if(null != p_Level)
							if(p_Level.Id != curLevel.Id)
								eIds.Add(e1.Id);
						}					
					}
					for(int i=0; i < bip2.Count;i++) {
						p = e1.get_Parameter(bip2[i]);
						if (null != p)
						{
							Railing e2 = e1 as Railing;
							if(e2.HasHost)
							{
								Element e3 = m_doc.GetElement(e2.HostId);
								p = e3.get_Parameter(bip3[i]);
							}					
							Level a7_Level = m_doc.GetElement(p.AsElementId())  as Level;
							if(null != a7_Level)
							if(a7_Level.Id != curLevel.Id)
								eIds.Add(e1.Id);
						}					
					}		
				}
				catch (Exception ex)
				{
				     TaskDialog.Show("Exception", ex.ToString());
				}
			}
			BtnShowAllClick(sender, e);
			using (Transaction t = new Transaction(m_doc, "Hide element"))
			{
				t.Start();
				this.Hide();

				m_uidoc.ActiveView.HideElements(eIds);
				m_uidoc.RefreshActiveView();
				
				t.Commit();
					
			}
			this.Show();
		}
		
		void BtnShowAllClick(object sender, EventArgs e)
		{
			IEnumerable<Element> Allelem= JtElementExtensionMethods.SelectAllPhysicalElements(m_doc);
			IEnumerator<Element> a = Allelem.GetEnumerator();
			List<ElementId> eIds= new List<ElementId>();
			while (a.MoveNext()) {
				Element e1 = a.Current;
				eIds.Add(e1.Id);
			}
			using (Transaction t = new Transaction(m_doc, "ShowAll element"))
			{
				t.Start();
				this.Hide();
				m_uidoc.ActiveView.UnhideElements(eIds);
				m_uidoc.RefreshActiveView();
				t.Commit();
				this.Show();
			}
			
		}
		
		void DetachFromPlane(Element beam){
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

		
		void BtnApplyLevelClick(object sender, EventArgs e)
		{
			
			ICollection<ElementId> selection = m_uidoc.Selection.GetElementIds();
			if (selection.Count==0 ){
				this .Hide();
				m_uidoc.Selection.PickObjects(ObjectType.Element, "請窗選構件：");				
				selection = m_uidoc.Selection.GetElementIds();
			}
			
			Level curBtmLevel = m_levels.ElementAt(cbLevel.SelectedIndex);
			
//			Parameter p;
			
			List<BuiltInParameter> bipBase = new List<BuiltInParameter>(){
				BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM,
				BuiltInParameter.STAIRS_RAILING_BASE_LEVEL_PARAM,
				BuiltInParameter.FAMILY_BASE_LEVEL_PARAM,
				BuiltInParameter.LEVEL_PARAM,
				BuiltInParameter.WALL_BASE_CONSTRAINT,
				BuiltInParameter.FAMILY_LEVEL_PARAM,
				BuiltInParameter.STAIRS_BASE_LEVEL_PARAM,
				BuiltInParameter.LEVEL_PARAM,
				BuiltInParameter.FAMILY_LEVEL_PARAM,
				BuiltInParameter.FAMILY_LEVEL_PARAM,
				BuiltInParameter.ROOF_BASE_LEVEL_PARAM
			};
			
			List<BuiltInParameter> bipTop = new List<BuiltInParameter>(){
				BuiltInParameter.INVALID,
				BuiltInParameter.INVALID,
				BuiltInParameter.FAMILY_TOP_LEVEL_PARAM,
				BuiltInParameter.INVALID,
				BuiltInParameter.WALL_HEIGHT_TYPE,
				BuiltInParameter.INVALID,
				BuiltInParameter.STAIRS_TOP_LEVEL_PARAM,
				BuiltInParameter.INVALID,
				BuiltInParameter.INVALID,
				BuiltInParameter.INVALID,
				BuiltInParameter.INVALID
			};

			List<BuiltInParameter> bipBaseOffset1 = new List<BuiltInParameter>(){
				BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION,
				BuiltInParameter.STAIRS_RAILING_HEIGHT_OFFSET,
				BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM,
				BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM,
				BuiltInParameter.WALL_BASE_OFFSET,
				BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM,
				BuiltInParameter.STAIRS_BASE_OFFSET,
				BuiltInParameter.CEILING_HEIGHTABOVELEVEL_PARAM,
				BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM,
				BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM,
				BuiltInParameter.ROOF_LEVEL_OFFSET_PARAM
			};
			
			List<BuiltInParameter> bipBaseOffset2 = new List<BuiltInParameter>(){
				BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION,
				BuiltInParameter.INVALID,
				BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM,
				BuiltInParameter.INVALID,
				BuiltInParameter.WALL_TOP_OFFSET,
				BuiltInParameter.INVALID,
				BuiltInParameter.STAIRS_TOP_OFFSET,
				BuiltInParameter.INVALID,
				BuiltInParameter.INVALID,
				BuiltInParameter.INVALID,
				BuiltInParameter.INVALID
			};
			this.Hide();
			using (Transaction t = new Transaction(m_doc, "Hide element"))
			{
				t.Start();

			foreach (ElementId eId in selection) {
				int i=0;
				Element e1 = m_doc.GetElement(eId);
				switch ((int)e1.Category.Id.IntegerValue) {
					case (int)BuiltInCategory.OST_StructuralFraming:
						FamilyInstance fi = e1 as FamilyInstance;
						switch (fi.StructuralType) {
							case StructuralType.Beam:
								i=0;
								Parameter p=e1.get_Parameter(bipBase[i]);
								if (p.IsReadOnly)
								{
									DetachFromPlane(e1);
								}
								p.Set(curBtmLevel.Id);
								break;
							default:
								
								break;
						}
						
						break;
					case (int)BuiltInCategory.OST_StairsRailing:
						i=1;
						if(!(e1 as Railing).HasHost){
							Parameter p = e1.get_Parameter(bipBase[i]);
							Parameter q = e1.get_Parameter(bipBaseOffset1[i]);
			                    	
			            	Level BaseLevel = m_doc.GetElement(p.AsElementId()) as Level;
			            	p.Set(curBtmLevel.Id);
		            		q.Set(q.AsDouble() + BaseLevel.Elevation - curBtmLevel.Elevation);
						}
						break;
					case (int)BuiltInCategory.OST_StructuralColumns:
						i=2;
						break;
					case (int)BuiltInCategory.OST_Floors: 
						i=3;
						break;
					case (int)BuiltInCategory.OST_Walls:
						i=4;
						break;
					case (int)BuiltInCategory.OST_Doors:
					case (int)BuiltInCategory.OST_Windows:
						i=5;
						break;
					case (int)BuiltInCategory.OST_Stairs:
						i=6;
						break;
					case (int)BuiltInCategory.OST_Ceilings:
						i=7;
						break;
					case (int)BuiltInCategory.OST_StructuralFoundation:
						i=8;
						break;
					case (int)BuiltInCategory.OST_Site:
						i=9;
						break;
					case (int)BuiltInCategory.OST_Roofs:
						i=10;
						break;
					default:
						
						break;
				}
				
            	
            	if(i>1){
					Parameter p = e1.get_Parameter(bipBase[i]);
					if (null != p){
						Parameter q = e1.get_Parameter(bipBaseOffset1[i]);
		            	Level BaseLevel = m_doc.GetElement(p.AsElementId()) as Level;
		            	p.Set(curBtmLevel.Id);
	            		q.Set(q.AsDouble() + BaseLevel.Elevation - curBtmLevel.Elevation);
					}
            	}
				
				t.Commit();
				}
				this.Show();
	            

			}

			
		}
	}

	public static class JtElementExtensionMethods
	{
		public static bool IsPhysicalElement(this Element e)
		{
			if( e.Category == null ) return false;
			if( e.ViewSpecific ) return false;
			// exclude specific unwanted categories
			//if( ( (BuiltInCategory) e.Category.Id.IntegerValue ) == BuiltInCategory.OST_HVAC_Zones ) return false;
			
			return e.Category.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory;
		}
	
		public static IEnumerable<Element> SelectAllPhysicalElements(Document doc)
		{
		  return new FilteredElementCollector( doc )
		    .WhereElementIsNotElementType()
		    .Where( e => e.IsPhysicalElement() );
		}
	}
}
