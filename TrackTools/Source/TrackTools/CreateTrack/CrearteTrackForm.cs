/*
 * 由SharpDevelop创建。
 * 用户： Jack
 * 日期: 2019/7/19
 * 时间: 下午 12:02
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
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
using System.Xml.XPath;
using System.Xml;

namespace TrackTools
{
	/// <summary>
	/// Description of CrearteTrackForm.
	/// </summary>
	public partial class CrearteTrackForm : System.Windows.Forms.Form
	{
//		UIDocument m_uidoc;
//		UIApplication m_uiapp;
	    private RequestHandler m_Handler;
	    private ExternalEvent m_ExEvent;

		CCreateTrack m_ct = null;
		
		
		public CrearteTrackForm(CCreateTrack ct, ExternalEvent exEvent, RequestHandler handler)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			m_Handler = handler;
			m_ExEvent = exEvent;
			m_ct = ct;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			GetAllSystem(listSystem);
			textChainage.Text = Interaction.GetSetting("AlignmentTools","AppData","textChainage","1234.567800");
			textLength.Text = Interaction.GetSetting("AlignmentTools","AppData","textLength","104.567800");
			textQuantity.Text = Interaction.GetSetting("AlignmentTools","AppData","textQuantity","1");
			textName.Text = Interaction.GetSetting("AlignmentTools","AppData","textName","DN-001");
			textElevation.Text = Interaction.GetSetting("AlignmentTools","AppData","textElevation","83.800");
			textInvertElevation.Text = Interaction.GetSetting("AlignmentTools","AppData","textInvertElevation","400");
			RailSectionArea.Text = Interaction.GetSetting("AlignmentTools","AppData","RailSectionArea","77.30986");
			bFitRailLength.Checked = bool.Parse(Interaction.GetSetting("AlignmentTools","AppData","bFitRailLength","True"));
			bFixedElevation.Checked = bool.Parse(Interaction.GetSetting("AlignmentTools","AppData","bFixedElevation","True"));
			bReverse.Checked = bool.Parse(Interaction.GetSetting("AlignmentTools","AppData","bReverse","True"));
			InfoLabel.Text = "目前作用中的線形檔為：\"" + Interaction.GetSetting("AlignmentTools","AppData","ALDName1","Y06UH.ALD")+"\"。";
			
			GetAllLandXmlName(cbLandXml, "XMLName1");												
			GetAlignments(comboBox1, VName1, "ALDName1");
//			GetAllALDFileName(comboBox1, VName1, "ALDName1");
			GetAllALDFileName(comboBox2, VName2, "ALDName2");
			GetAllALDFileName(comboBox3, VName3, "ALDName3");
			
			textBox1.Text = Interaction.GetSetting("AlignmentTools","AppData","OffsetLimit","45.00000");
		}
		
		public void GetAlignments(System.Windows.Forms.ComboBox obj, Label lbl, string AldNameNo)
		{
			string myPath=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\DataTable\\";
			string LandXmlName = Interaction.GetSetting("AlignmentTools","AppData", "XMLName1","alignments.xml");
            XmlTextReader reader = new XmlTextReader(myPath + LandXmlName);
            reader.Namespaces = false;
            XPathDocument document = new XPathDocument(reader);
            XPathNavigator navigator = document.CreateNavigator();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(reader.NameTable);
            nsmgr.AddNamespace("ns", "http://www.landxml.org/schema/LandXML-1.1");
            XPathNodeIterator nodes = navigator.Select("//LandXML/Alignments/*", nsmgr);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes.MoveNext();
				obj.Items.Add(nodes.Current.GetAttribute("name", nodes.Current.GetNamespace("ns")));
            }

            int n=obj.FindString(Interaction.GetSetting("AlignmentTools","AppData",AldNameNo,"Y06UH.ALD"));
            if (n != -1) obj.SelectedIndex=n;
			lbl.Text=obj.SelectedItem.ToString().Replace("H.ALD","V.ALD");
		}
		
		public void GetAllALDFileName(System.Windows.Forms.ComboBox obj, Label lbl, string AldNameNo)
		{
			string myPath=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\DataTable\\";
			string myName=Microsoft.VisualBasic.FileSystem.Dir(myPath + "*.ald");
			try{				
			while (myName != "")
			{
				if (myName != "." && myName != "..")
				{
					if (myName.Substring(4,1).ToUpper() == "H")
					{
						obj.Items.Add(myName);
					}
					
				}
				myName=Microsoft.VisualBasic.FileSystem.Dir();
			}
			}
			catch
			{
				//MessageBox.Show(n.ToString(),"");
			}
			int n=obj.FindString(Interaction.GetSetting("AlignmentTools","AppData",AldNameNo,"Y06UH.ALD"));
			obj.SelectedIndex=n;
			lbl.Text=obj.SelectedItem.ToString().Replace("H.ALD","V.ALD");
		}

		public void GetAllLandXmlName(System.Windows.Forms.ComboBox obj, string AldNameNo)
		{
			string myPath=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\DataTable\\";
			string myName=Microsoft.VisualBasic.FileSystem.Dir(myPath + "*.xml");
			try{				
			while (myName != "")
			{
				if (myName != "." && myName != "..")
				{
					obj.Items.Add(myName);
				}
				myName=Microsoft.VisualBasic.FileSystem.Dir();
			}
			}
			catch
			{
				//MessageBox.Show(n.ToString(),"");
			}
			int n=obj.FindString(Interaction.GetSetting("AlignmentTools","AppData", AldNameNo,"alignments.xml"));
			obj.SelectedIndex=n;
			//obj.Text = Interaction.GetSetting("AlignmentTools","AppData",AldNameNo,"alignments.xml");
		}
	
		
		public void GetAllSystem(ListBox obj)
		{
			string myPath=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\TrackSystem\\";
			foreach (var foundDir in Microsoft.VisualBasic.FileIO.FileSystem.GetDirectories(myPath)) {
				obj.Items.Add(System.IO.Path.GetFileName(foundDir));
			}
			int n=obj.FindString(Interaction.GetSetting("AlignmentTools","AppData","SystemName","Slab"));
			obj.SelectedIndex=n;

		}

		
		void CrearteTrackFormFormClosed(object sender, FormClosedEventArgs e)
		{
			SaveSettings();
		}
		
		void SaveSettings()
		{
			Interaction.SaveSetting("AlignmentTools","AppData","SystemName",listSystem.SelectedItem.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","ComponentName",listComponent.SelectedItem.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","textChainage",textChainage.Text);
			Interaction.SaveSetting("AlignmentTools","AppData","textLength",textLength.Text);
			Interaction.SaveSetting("AlignmentTools","AppData","textQuantity",textQuantity.Text);
			Interaction.SaveSetting("AlignmentTools","AppData","textName",textName.Text);
			Interaction.SaveSetting("AlignmentTools","AppData","textElevation", textElevation.Text);
			Interaction.SaveSetting("AlignmentTools","AppData","textInvertElevation", textInvertElevation.Text);
			Interaction.SaveSetting("AlignmentTools","AppData","bFitRailLength", bFitRailLength.Checked.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","bFixedElevation", bFixedElevation.Checked.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","bReverse", bReverse.Checked.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","RailSectionArea", RailSectionArea.Text);
			
			Interaction.SaveSetting("AlignmentTools","AppData","XMLName1",cbLandXml.SelectedItem.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","AldName1",comboBox1.SelectedItem.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","AldName2",comboBox2.SelectedItem.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","AldName3",comboBox3.SelectedItem.ToString());
			Interaction.SaveSetting("AlignmentTools","AppData","OffsetLimit",textBox1.Text);
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			VName1.Text=comboBox1.SelectedItem.ToString().Replace("H.ALD","V.ALD");
			Interaction.SaveSetting("AlignmentTools","AppData","AldName1",comboBox1.SelectedItem.ToString());
			InfoLabel.Text = "目前作用中的線形檔為：\"" + comboBox1.SelectedItem.ToString() +"\"。";
			BtnDrawCenterLine.Text = "繪製中心線 " + comboBox1.SelectedItem.ToString();
		}
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			VName2.Text=comboBox2.SelectedItem.ToString().Replace("H.ALD","V.ALD");
		}
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			VName3.Text=comboBox3.SelectedItem.ToString().Replace("H.ALD","V.ALD");
		}
		
		
		void ListSystemSelectedIndexChanged(object sender, EventArgs e)
		{
			listComponent.Items.Clear();
			GetAllComponent(listComponent);
		}
		
		public void GetAllComponent(ListBox obj)
		{
			
			string myPath=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\TrackSystem\\"+listSystem.SelectedItem.ToString()+"\\";
			if ("Component" == listSystem.SelectedItem.ToString())
			{
				foreach (var foundDir in Microsoft.VisualBasic.FileIO.FileSystem.GetFiles(myPath)) {
					obj.Items.Add(System.IO.Path.GetFileName(foundDir));
				}
			}
			else
			{
				foreach (var foundDir in Microsoft.VisualBasic.FileIO.FileSystem.GetDirectories(myPath)) {
					obj.Items.Add(System.IO.Path.GetFileName(foundDir));
				}
			}
			int n=obj.FindString(Interaction.GetSetting("AlignmentTools","AppData","ComponentName","Plinth"));
			obj.SelectedIndex=n;

		}
		

		
		void GetChainageBtnClick(object sender, EventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "GetChainageBtnClick");
			Close();
		}
		
		void GetLengthBtnClick(object sender, EventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "GetLengthBtnClick");
			Close();			
		}
		
		void CbLandXmlSelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
	}
	
}
