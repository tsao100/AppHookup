/*
 * 由SharpDevelop创建。
 * 用户： Jack
 * 日期: 2018/6/19
 * 时间: 下午 10:04
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using BuildingTools;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using HelperFunctions;



namespace MyTools.CreateBeamsFromCAD
{
	/// <summary>
	/// Description of CreateBeamsFromCAD.
	/// </summary>
	public class CreateBeamsFromCAD
	{
        private Document m_doc = null;
        
		public CreateBeamsFromCAD()
		{
		}
		
        public CreateBeamsFromCAD(UIDocument hostDoc)
        {
            m_doc = hostDoc.Document;
        }

        /// <summary>
        /// run this marco now
        /// </summary>
        public void Run()
        {
        	Level lvl = Fn.getViewLevel(m_doc);
			
//        	 AutoCAD.AcadApplication a 
//        = Interaction.GetObject(null,"AutoCAD.Application") as AutoCAD.AcadApplication;
        	

        }
		
	}
}
