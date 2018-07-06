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
				BuiltInParameter.FAMILY_BASE_LEVEL_PARAM,
				BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM,
				BuiltInParameter.SCHEDULE_LEVEL_PARAM,
				BuiltInParameter.WALL_BASE_CONSTRAINT,
				BuiltInParameter.FAMILY_LEVEL_PARAM,
				BuiltInParameter.STAIRS_BASE_LEVEL_PARAM,
				BuiltInParameter.STAIRS_RAILING_BASE_LEVEL_PARAM
			};
			
			List<BuiltInParameter> bipTop = new List<BuiltInParameter>(){
				BuiltInParameter.FAMILY_TOP_LEVEL_PARAM,
				BuiltInParameter.INVALID,
				BuiltInParameter.INVALID,
				BuiltInParameter.WALL_HEIGHT_TYPE,
				BuiltInParameter.INVALID,
				BuiltInParameter.STAIRS_TOP_LEVEL_PARAM,
				BuiltInParameter.INVALID
			};

			List<BuiltInParameter> bipBaseOffset = new List<BuiltInParameter>(){
				BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM,
				BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION,
				BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM,
				BuiltInParameter.WALL_BASE_OFFSET,
				BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM,
				BuiltInParameter.STAIRS_BASE_OFFSET,
				BuiltInParameter.STAIRS_RAILING_HEIGHT_OFFSET
			};
			
			List<BuiltInParameter> bipTopOffset = new List<BuiltInParameter>(){
				BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM,
				BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION,
				BuiltInParameter.INVALID,
				BuiltInParameter.WALL_TOP_OFFSET,
				BuiltInParameter.FAMILY_LEVEL_PARAM,
				BuiltInParameter.STAIRS_TOP_OFFSET,
				BuiltInParameter.INVALID
			};
			
			foreach (ElementId eId in selection) {
				Element e1 = m_doc.GetElement(eId);
				switch ((int)e1.Category.Id.IntegerValue) {
					case (int)BuiltInCategory.OST_StructuralFraming:
						FamilyInstance fi = e1 as FamilyInstance;
						switch (fi.StructuralType) {
							case StructuralType.Beam:
								break;
							default:
								
								break;
						}
						
						break;
					case (int)BuiltInCategory.OST_Floors:
						
						break;
					case (int)BuiltInCategory.OST_Walls:
						Parameter p = e1.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT);
						Parameter q = e1.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET);
                    	
                    	Level BaseLevel = m_doc.GetElement(p.AsElementId()) as Level;
                    	
                    	p.Set(curBtmLevel.Id);
                    	q.Set(q.AsDouble() + BaseLevel.Elevation - curBtmLevel.Elevation);

						break;
					case (int)BuiltInCategory.OST_Doors:
						
						break;
					case (int)BuiltInCategory.OST_Windows:
						
						break;
					case (int)BuiltInCategory.OST_StairsRailing:
						
						break;
					case (int)BuiltInCategory.OST_Stairs:
						
						break;
					case (int)BuiltInCategory.OST_StructuralColumns:
						break;
					default:
						
						break;
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
