/*
 * 由SharpDevelop创建。
 * 用户： jack.tsao
 * 日期: 2018/7/6
 * 时间: 下午 02:40
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
	/// Description of ChangeObjRefLevel.
	/// </summary>
	public partial class ChangeObjRefLevel : System.Windows.Forms.Form
	{
		private Document m_doc;
		private UIDocument m_uidoc;
		private IList<Level> m_levels=null;
		public ChangeObjRefLevel(Document doc, UIDocument uidoc)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			m_doc = doc;
			m_uidoc = uidoc;
			cbTopLevel.Items.Add("None");
			m_levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(l => -l.Elevation).ToList();
			foreach (Level level in m_levels) {
				cbTopLevel.Items.Add(level.Name);
				cbBtmLevel.Items.Add(level.Name);
			}
			
			cbTopLevel.SelectedIndex=0;
			cbBtmLevel.SelectedIndex=0;
		}
		
		void BtnApplyClick(object sender, EventArgs e)
		{
			Level curBtmLevel = m_levels.ElementAt(cbBtmLevel.SelectedIndex);
			Level curTopLevel = m_levels.ElementAt(cbTopLevel.SelectedIndex);
			
			ICollection<ElementId> selection = m_uidoc.Selection.GetElementIds();
//			Parameter p;
			
			List<BuiltInParameter> bipBase = new List<BuiltInParameter>(){
				BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM,
				BuiltInParameter.STAIRS_RAILING_BASE_LEVEL_PARAM,
				BuiltInParameter.FAMILY_BASE_LEVEL_PARAM,
				BuiltInParameter.LEVEL_PARAM,
				BuiltInParameter.WALL_BASE_CONSTRAINT,
				BuiltInParameter.FAMILY_LEVEL_PARAM,
				BuiltInParameter.STAIRS_BASE_LEVEL_PARAM
			};
			
			List<BuiltInParameter> bipTop = new List<BuiltInParameter>(){
				BuiltInParameter.INVALID,
				BuiltInParameter.INVALID,
				BuiltInParameter.FAMILY_TOP_LEVEL_PARAM,
				BuiltInParameter.INVALID,
				BuiltInParameter.WALL_HEIGHT_TYPE,
				BuiltInParameter.INVALID,
				BuiltInParameter.STAIRS_TOP_LEVEL_PARAM
			};

			List<BuiltInParameter> bipBaseOffset1 = new List<BuiltInParameter>(){
				BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION,
				BuiltInParameter.STAIRS_RAILING_HEIGHT_OFFSET,
				BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM,
				BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM,
				BuiltInParameter.WALL_BASE_OFFSET,
				BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM,
				BuiltInParameter.STAIRS_BASE_OFFSET
			};
			
			List<BuiltInParameter> bipBaseOffset2 = new List<BuiltInParameter>(){
				BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION,
				BuiltInParameter.INVALID,
				BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM,
				BuiltInParameter.INVALID,
				BuiltInParameter.WALL_TOP_OFFSET,
				BuiltInParameter.INVALID,
				BuiltInParameter.STAIRS_TOP_OFFSET
			};
			
			int i=0;
			foreach (ElementId eId in selection) {
				Element e1 = m_doc.GetElement(eId);
				switch ((int)e1.Category.Id.IntegerValue) {
					case (int)BuiltInCategory.OST_StructuralFraming:
						FamilyInstance fi = e1 as FamilyInstance;
						switch (fi.StructuralType) {
							case StructuralType.Beam:
								i=0;
								e1.get_Parameter(bipBase[i]).Set(curBtmLevel.Id);
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
					default:
						
						break;
				}
				
            	
            	if(i>1){
					Parameter p = e1.get_Parameter(bipBase[i]);
					Parameter q = e1.get_Parameter(bipBaseOffset1[i]);
	                    	
	            	Level BaseLevel = m_doc.GetElement(p.AsElementId()) as Level;
	            	p.Set(curBtmLevel.Id);
            		q.Set(q.AsDouble() + BaseLevel.Elevation - curBtmLevel.Elevation);
            	}
	            

//				try
//				{
//					for(int i=0; i < bipBase.Count;i++) {
//						p = e1.get_Parameter(bipBase[i]);
//						if (null != p)
//						{
//							Level p_Level = m_doc.GetElement(p.AsElementId())  as Level;
//							if(null != p_Level){
//								if(!e1.get_Parameter(bipBase[i]).IsReadOnly)
//								e1.get_Parameter(bipBase[i]).Set(curBtmLevel.Id);
//								
//							}
//						}					
//					}
//				}
////				catch (Exception ex)
//				catch (Exception)
//				{
//				     //TaskDialog.Show("Exception", ex.ToString());
//				}
			}

			this.Close();	
		}
	}
}
