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
	}
}
