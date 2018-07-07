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
			m_uidoc.ActiveView.HideElements(eIds);
			m_uidoc.RefreshActiveView();

			this.Close();
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
