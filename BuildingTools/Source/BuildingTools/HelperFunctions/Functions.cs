/*
 * 由SharpDevelop创建。
 * 用户： Jack
 * 日期: 2018/6/19
 * 时间: 下午 10:45
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using BuildingTools;
using Autodesk.Revit.DB;

namespace HelperFunctions
{
	/// <summary>
	/// Description of Functions.
	/// </summary>
	public class Fn
	{
		public Fn()
		{
		}
		
		public static Level  getViewLevel(Document doc)
		{
			View active = doc.ActiveView;
			ElementId levelId = null;
			
//			Parameter level = active.get_Parameter("關聯的樓層");
			Parameter level = active.get_Parameter(BuiltInParameter.PLAN_VIEW_LEVEL);				
			
			FilteredElementCollector lvlCollector = new FilteredElementCollector(doc);
			ICollection<Element> lvlCollection = lvlCollector.OfClass(typeof(Level)).ToElements();
			Level lvl=null;
			foreach (Element l in lvlCollection)
			{
				lvl = l as Level;				
				if(lvl.Name == level.AsString())
				{
					levelId = lvl.Id;
					break;
					//TaskDialog.Show("test", lvl.Name + "\n"  + lvl.Id.ToString() + level.Id.ToString());
				}
			}
			return lvl;
		}
		

	}
}
