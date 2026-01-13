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
//using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using Autodesk.Revit.Creation;
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
 
 
namespace TrackTools
{
	partial class CrearteTrackForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.listSystem = new System.Windows.Forms.ListBox();
			this.listComponent = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textChainage = new System.Windows.Forms.TextBox();
			this.GetChainageBtn = new System.Windows.Forms.Button();
			this.BatchBuildBtn = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textLength = new System.Windows.Forms.TextBox();
			this.ManualBuildBtn = new System.Windows.Forms.Button();
			this.textName = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.btnExit = new System.Windows.Forms.Button();
			this.bFitRailLength = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.RailSectionArea = new System.Windows.Forms.TextBox();
			this.bReverse = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.createTrackFamilyBtn = new System.Windows.Forms.Button();
			this.GenerateComponentFamiliesBtn = new System.Windows.Forms.Button();
			this.InfoLabel = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.VName3 = new System.Windows.Forms.Label();
			this.VName2 = new System.Windows.Forms.Label();
			this.VName1 = new System.Windows.Forms.Label();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.BtnDrawCenterLine = new System.Windows.Forms.Button();
			this.bFixedElevation = new System.Windows.Forms.CheckBox();
			this.label10 = new System.Windows.Forms.Label();
			this.textElevation = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.textInvertElevation = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.textQuantity = new System.Windows.Forms.TextBox();
			this.GetLengthBtn = new System.Windows.Forms.Button();
			this.label15 = new System.Windows.Forms.Label();
			this.cbLandXml = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listSystem
			// 
			this.listSystem.FormattingEnabled = true;
			this.listSystem.ItemHeight = 12;
			this.listSystem.Location = new System.Drawing.Point(12, 63);
			this.listSystem.Name = "listSystem";
			this.listSystem.Size = new System.Drawing.Size(188, 172);
			this.listSystem.TabIndex = 0;
			this.listSystem.SelectedIndexChanged += new System.EventHandler(this.ListSystemSelectedIndexChanged);
			// 
			// listComponent
			// 
			this.listComponent.FormattingEnabled = true;
			this.listComponent.ItemHeight = 12;
			this.listComponent.Location = new System.Drawing.Point(206, 63);
			this.listComponent.Name = "listComponent";
			this.listComponent.Size = new System.Drawing.Size(207, 172);
			this.listComponent.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(148, 26);
			this.label1.TabIndex = 1;
			this.label1.Text = "軌道系統列表";
			this.label1.UseCompatibleTextRendering = true;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(206, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(148, 26);
			this.label2.TabIndex = 1;
			this.label2.Text = "組件列表";
			this.label2.UseCompatibleTextRendering = true;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(11, 425);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(148, 26);
			this.label3.TabIndex = 1;
			this.label3.Text = "手動建模";
			this.label3.UseCompatibleTextRendering = true;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(267, 425);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(148, 26);
			this.label4.TabIndex = 1;
			this.label4.Text = "依Excel資料建模";
			this.label4.UseCompatibleTextRendering = true;
			// 
			// textChainage
			// 
			this.textChainage.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textChainage.Location = new System.Drawing.Point(66, 443);
			this.textChainage.Name = "textChainage";
			this.textChainage.Size = new System.Drawing.Size(111, 27);
			this.textChainage.TabIndex = 2;
			this.textChainage.Text = "110123.45678";
			// 
			// GetChainageBtn
			// 
			this.GetChainageBtn.Location = new System.Drawing.Point(190, 443);
			this.GetChainageBtn.Name = "GetChainageBtn";
			this.GetChainageBtn.Size = new System.Drawing.Size(41, 29);
			this.GetChainageBtn.TabIndex = 3;
			this.GetChainageBtn.Text = "點選";
			this.GetChainageBtn.UseCompatibleTextRendering = true;
			this.GetChainageBtn.UseVisualStyleBackColor = true;
			this.GetChainageBtn.Click += new System.EventHandler(this.GetChainageBtnClick);
			// 
			// BatchBuildBtn
			// 
			this.BatchBuildBtn.Location = new System.Drawing.Point(270, 448);
			this.BatchBuildBtn.Name = "BatchBuildBtn";
			this.BatchBuildBtn.Size = new System.Drawing.Size(60, 26);
			this.BatchBuildBtn.TabIndex = 4;
			this.BatchBuildBtn.Text = "開始";
			this.BatchBuildBtn.UseCompatibleTextRendering = true;
			this.BatchBuildBtn.UseVisualStyleBackColor = true;
			this.BatchBuildBtn.Click += new System.EventHandler(this.BatchBuildBtnClick);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(11, 450);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 20);
			this.label5.TabIndex = 1;
			this.label5.Text = "里程：";
			this.label5.UseCompatibleTextRendering = true;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(10, 485);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(97, 20);
			this.label6.TabIndex = 1;
			this.label6.Text = "元件長度/間距：";
			this.label6.UseCompatibleTextRendering = true;
			// 
			// textLength
			// 
			this.textLength.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textLength.Location = new System.Drawing.Point(99, 478);
			this.textLength.Name = "textLength";
			this.textLength.Size = new System.Drawing.Size(78, 27);
			this.textLength.TabIndex = 2;
			this.textLength.Text = "140.700";
			// 
			// ManualBuildBtn
			// 
			this.ManualBuildBtn.Location = new System.Drawing.Point(190, 544);
			this.ManualBuildBtn.Name = "ManualBuildBtn";
			this.ManualBuildBtn.Size = new System.Drawing.Size(41, 26);
			this.ManualBuildBtn.TabIndex = 4;
			this.ManualBuildBtn.Text = "開始";
			this.ManualBuildBtn.UseCompatibleTextRendering = true;
			this.ManualBuildBtn.UseVisualStyleBackColor = true;
			this.ManualBuildBtn.Click += new System.EventHandler(this.ManualBuildBtnClick);
			// 
			// textName
			// 
			this.textName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textName.Location = new System.Drawing.Point(77, 545);
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(100, 27);
			this.textName.TabIndex = 6;
			this.textName.Text = "0102-DN-001";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(10, 552);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 20);
			this.label7.TabIndex = 5;
			this.label7.Text = "元件名稱：";
			this.label7.UseCompatibleTextRendering = true;
			// 
			// btnExit
			// 
			this.btnExit.Location = new System.Drawing.Point(568, 638);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(60, 26);
			this.btnExit.TabIndex = 4;
			this.btnExit.Text = "結束";
			this.btnExit.UseCompatibleTextRendering = true;
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.BtnExitClick);
			// 
			// bFitRailLength
			// 
			this.bFitRailLength.Checked = true;
			this.bFitRailLength.CheckState = System.Windows.Forms.CheckState.Checked;
			this.bFitRailLength.Location = new System.Drawing.Point(24, 252);
			this.bFitRailLength.Name = "bFitRailLength";
			this.bFitRailLength.Size = new System.Drawing.Size(301, 24);
			this.bFitRailLength.TabIndex = 7;
			this.bFitRailLength.Text = "建模時考慮內外軌的長度";
			this.bFitRailLength.UseCompatibleTextRendering = true;
			this.bFitRailLength.UseVisualStyleBackColor = true;
			this.bFitRailLength.CheckedChanged += new System.EventHandler(this.BFitRailLengthCheckedChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(10, 385);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(81, 20);
			this.label9.TabIndex = 1;
			this.label9.Text = "鋼軌斷面積：";
			this.label9.UseCompatibleTextRendering = true;
			// 
			// RailSectionArea
			// 
			this.RailSectionArea.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.RailSectionArea.Location = new System.Drawing.Point(88, 378);
			this.RailSectionArea.Name = "RailSectionArea";
			this.RailSectionArea.Size = new System.Drawing.Size(89, 27);
			this.RailSectionArea.TabIndex = 2;
			this.RailSectionArea.Text = "77.30986";
			// 
			// bReverse
			// 
			this.bReverse.Checked = true;
			this.bReverse.CheckState = System.Windows.Forms.CheckState.Checked;
			this.bReverse.Location = new System.Drawing.Point(24, 274);
			this.bReverse.Name = "bReverse";
			this.bReverse.Size = new System.Drawing.Size(301, 24);
			this.bReverse.TabIndex = 7;
			this.bReverse.Text = "反向鋪軌";
			this.bReverse.UseCompatibleTextRendering = true;
			this.bReverse.UseVisualStyleBackColor = true;
			this.bReverse.CheckedChanged += new System.EventHandler(this.BFitRailLengthCheckedChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(183, 385);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(81, 20);
			this.label11.TabIndex = 1;
			this.label11.Text = "cm^2";
			this.label11.UseCompatibleTextRendering = true;
			// 
			// listBox2
			// 
			this.listBox2.FormattingEnabled = true;
			this.listBox2.ItemHeight = 12;
			this.listBox2.Location = new System.Drawing.Point(-7, 582);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(442, 4);
			this.listBox2.TabIndex = 8;
			// 
			// createTrackFamilyBtn
			// 
			this.createTrackFamilyBtn.Location = new System.Drawing.Point(12, 598);
			this.createTrackFamilyBtn.Name = "createTrackFamilyBtn";
			this.createTrackFamilyBtn.Size = new System.Drawing.Size(135, 26);
			this.createTrackFamilyBtn.TabIndex = 4;
			this.createTrackFamilyBtn.Text = "第一階段產生族";
			this.createTrackFamilyBtn.UseCompatibleTextRendering = true;
			this.createTrackFamilyBtn.UseVisualStyleBackColor = true;
			this.createTrackFamilyBtn.Click += new System.EventHandler(this.CreateTrackFamilyBtnClick);
			// 
			// GenerateComponentFamiliesBtn
			// 
			this.GenerateComponentFamiliesBtn.Location = new System.Drawing.Point(12, 638);
			this.GenerateComponentFamiliesBtn.Name = "GenerateComponentFamiliesBtn";
			this.GenerateComponentFamiliesBtn.Size = new System.Drawing.Size(135, 26);
			this.GenerateComponentFamiliesBtn.TabIndex = 4;
			this.GenerateComponentFamiliesBtn.Text = "創建多點自適應元件";
			this.GenerateComponentFamiliesBtn.UseCompatibleTextRendering = true;
			this.GenerateComponentFamiliesBtn.UseVisualStyleBackColor = true;
			this.GenerateComponentFamiliesBtn.Click += new System.EventHandler(this.GenerateComponentFamiliesBtnClick);
			// 
			// InfoLabel
			// 
			this.InfoLabel.Location = new System.Drawing.Point(12, 8);
			this.InfoLabel.Name = "InfoLabel";
			this.InfoLabel.Size = new System.Drawing.Size(401, 26);
			this.InfoLabel.TabIndex = 1;
			this.InfoLabel.Text = "目前作用中的線形檔為\"Y06UH.ALD\"";
			this.InfoLabel.UseCompatibleTextRendering = true;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(460, 295);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(82, 19);
			this.label8.TabIndex = 11;
			this.label8.Text = "外移距限值：";
			this.label8.UseCompatibleTextRendering = true;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(548, 291);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(53, 22);
			this.textBox1.TabIndex = 10;
			this.textBox1.Text = "35.00000";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.VName3);
			this.groupBox1.Controls.Add(this.VName2);
			this.groupBox1.Controls.Add(this.VName1);
			this.groupBox1.Controls.Add(this.comboBox3);
			this.groupBox1.Controls.Add(this.comboBox2);
			this.groupBox1.Controls.Add(this.comboBox1);
			this.groupBox1.Location = new System.Drawing.Point(454, 66);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(174, 214);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "作用中的線形檔";
			this.groupBox1.UseCompatibleTextRendering = true;
			// 
			// VName3
			// 
			this.VName3.Location = new System.Drawing.Point(8, 177);
			this.VName3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VName3.Name = "VName3";
			this.VName3.Size = new System.Drawing.Size(139, 24);
			this.VName3.TabIndex = 1;
			this.VName3.Text = "VName3";
			this.VName3.UseCompatibleTextRendering = true;
			// 
			// VName2
			// 
			this.VName2.Location = new System.Drawing.Point(9, 115);
			this.VName2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VName2.Name = "VName2";
			this.VName2.Size = new System.Drawing.Size(139, 24);
			this.VName2.TabIndex = 1;
			this.VName2.Text = "VName2";
			this.VName2.UseCompatibleTextRendering = true;
			// 
			// VName1
			// 
			this.VName1.Location = new System.Drawing.Point(10, 54);
			this.VName1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VName1.Name = "VName1";
			this.VName1.Size = new System.Drawing.Size(139, 24);
			this.VName1.TabIndex = 1;
			this.VName1.Text = "VName1";
			this.VName1.UseCompatibleTextRendering = true;
			// 
			// comboBox3
			// 
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Location = new System.Drawing.Point(8, 142);
			this.comboBox3.Margin = new System.Windows.Forms.Padding(2);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(140, 20);
			this.comboBox3.TabIndex = 0;
			this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.ComboBox3SelectedIndexChanged);
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(9, 80);
			this.comboBox2.Margin = new System.Windows.Forms.Padding(2);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(140, 20);
			this.comboBox2.TabIndex = 0;
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.ComboBox2SelectedIndexChanged);
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(10, 18);
			this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(140, 20);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1SelectedIndexChanged);
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(432, 0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(4, 688);
			this.listBox1.TabIndex = 12;
			// 
			// BtnDrawCenterLine
			// 
			this.BtnDrawCenterLine.Location = new System.Drawing.Point(454, 332);
			this.BtnDrawCenterLine.Name = "BtnDrawCenterLine";
			this.BtnDrawCenterLine.Size = new System.Drawing.Size(150, 26);
			this.BtnDrawCenterLine.TabIndex = 4;
			this.BtnDrawCenterLine.Text = "繪製中心線 Y06UH.ALD";
			this.BtnDrawCenterLine.UseCompatibleTextRendering = true;
			this.BtnDrawCenterLine.UseVisualStyleBackColor = true;
			this.BtnDrawCenterLine.Click += new System.EventHandler(this.BtnDrawCenterLineClick);
			// 
			// bFixedElevation
			// 
			this.bFixedElevation.Checked = true;
			this.bFixedElevation.CheckState = System.Windows.Forms.CheckState.Checked;
			this.bFixedElevation.Location = new System.Drawing.Point(24, 300);
			this.bFixedElevation.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.bFixedElevation.Name = "bFixedElevation";
			this.bFixedElevation.Size = new System.Drawing.Size(83, 24);
			this.bFixedElevation.TabIndex = 7;
			this.bFixedElevation.Text = "高程固定 =";
			this.bFixedElevation.UseCompatibleTextRendering = true;
			this.bFixedElevation.UseVisualStyleBackColor = true;
			this.bFixedElevation.CheckedChanged += new System.EventHandler(this.BFitRailLengthCheckedChanged);
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label10.Location = new System.Drawing.Point(197, 302);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(26, 20);
			this.label10.TabIndex = 1;
			this.label10.Text = "m";
			this.label10.UseCompatibleTextRendering = true;
			// 
			// textElevation
			// 
			this.textElevation.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textElevation.Location = new System.Drawing.Point(107, 300);
			this.textElevation.Name = "textElevation";
			this.textElevation.Size = new System.Drawing.Size(89, 27);
			this.textElevation.TabIndex = 2;
			this.textElevation.Text = "83.800";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(10, 349);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(81, 20);
			this.label12.TabIndex = 1;
			this.label12.Text = "仰拱為T/R -";
			this.label12.UseCompatibleTextRendering = true;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(117, 349);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(81, 20);
			this.label13.TabIndex = 1;
			this.label13.Text = "mm";
			this.label13.UseCompatibleTextRendering = true;
			// 
			// textInvertElevation
			// 
			this.textInvertElevation.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textInvertElevation.Location = new System.Drawing.Point(81, 342);
			this.textInvertElevation.Name = "textInvertElevation";
			this.textInvertElevation.Size = new System.Drawing.Size(32, 27);
			this.textInvertElevation.TabIndex = 2;
			this.textInvertElevation.Text = "400";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(10, 519);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(97, 20);
			this.label14.TabIndex = 1;
			this.label14.Text = "元件數量：";
			this.label14.UseCompatibleTextRendering = true;
			// 
			// textQuantity
			// 
			this.textQuantity.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textQuantity.Location = new System.Drawing.Point(99, 512);
			this.textQuantity.Name = "textQuantity";
			this.textQuantity.Size = new System.Drawing.Size(78, 27);
			this.textQuantity.TabIndex = 2;
			this.textQuantity.Text = "1";
			// 
			// GetLengthBtn
			// 
			this.GetLengthBtn.Location = new System.Drawing.Point(190, 480);
			this.GetLengthBtn.Name = "GetLengthBtn";
			this.GetLengthBtn.Size = new System.Drawing.Size(41, 29);
			this.GetLengthBtn.TabIndex = 3;
			this.GetLengthBtn.Text = "點選";
			this.GetLengthBtn.UseCompatibleTextRendering = true;
			this.GetLengthBtn.UseVisualStyleBackColor = true;
			this.GetLengthBtn.Click += new System.EventHandler(this.GetLengthBtnClick);
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(454, 6);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(174, 20);
			this.label15.TabIndex = 1;
			this.label15.Text = "作用中的LandXML：";
			this.label15.UseCompatibleTextRendering = true;
			// 
			// cbLandXml
			// 
			this.cbLandXml.FormattingEnabled = true;
			this.cbLandXml.Location = new System.Drawing.Point(454, 31);
			this.cbLandXml.Margin = new System.Windows.Forms.Padding(2);
			this.cbLandXml.Name = "cbLandXml";
			this.cbLandXml.Size = new System.Drawing.Size(140, 20);
			this.cbLandXml.TabIndex = 0;
			this.cbLandXml.SelectedIndexChanged += new System.EventHandler(this.CbLandXmlSelectedIndexChanged);
			// 
			// CrearteTrackForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(640, 678);
			this.ControlBox = false;
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.listBox2);
			this.Controls.Add(this.cbLandXml);
			this.Controls.Add(this.bReverse);
			this.Controls.Add(this.bFixedElevation);
			this.Controls.Add(this.bFitRailLength);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.ManualBuildBtn);
			this.Controls.Add(this.GenerateComponentFamiliesBtn);
			this.Controls.Add(this.createTrackFamilyBtn);
			this.Controls.Add(this.BtnDrawCenterLine);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.BatchBuildBtn);
			this.Controls.Add(this.GetLengthBtn);
			this.Controls.Add(this.GetChainageBtn);
			this.Controls.Add(this.textElevation);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.textInvertElevation);
			this.Controls.Add(this.RailSectionArea);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textQuantity);
			this.Controls.Add(this.textLength);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.textChainage);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.InfoLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listComponent);
			this.Controls.Add(this.listSystem);
			this.Name = "CrearteTrackForm";
			this.Text = "軌道建模";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CrearteTrackFormFormClosed);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ComboBox cbLandXml;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Button GetLengthBtn;
		private System.Windows.Forms.TextBox textQuantity;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textInvertElevation;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox textElevation;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.CheckBox bFixedElevation;
		private System.Windows.Forms.Button BtnDrawCenterLine;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.Label VName1;
		private System.Windows.Forms.Label VName2;
		private System.Windows.Forms.Label VName3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label InfoLabel;
		private System.Windows.Forms.Button GenerateComponentFamiliesBtn;
		private System.Windows.Forms.Button createTrackFamilyBtn;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox bReverse;
		private System.Windows.Forms.TextBox RailSectionArea;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox bFitRailLength;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textName;
		private System.Windows.Forms.Button ManualBuildBtn;
		private System.Windows.Forms.TextBox textLength;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button BatchBuildBtn;
		private System.Windows.Forms.Button GetChainageBtn;
		private System.Windows.Forms.TextBox textChainage;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listComponent;
		private System.Windows.Forms.ListBox listSystem;
		
		void BatchBuildBtnClick(object sender, System.EventArgs e)
		{
		//	this.Hide();
		//	m_ct.AddInstance();
            //this.Show();
//            SetActiveFolder(listSystem.SelectedItem.ToString());
//            SetActiveComponent(listComponent.SelectedItem.ToString());
//			m_Handler.Request.Make(RequestId.Test);
//			m_Handler.Request.Make(RequestId.BatchBuild);
//			m_ExEvent.Raise();
			//DozeOff();
			
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "BatchBuildBtnClick");
			SaveSettings();
			this.Close();
			
			

		}
		
		private void DozeOff()
		{
			EnableCommands(false);
		}

		private void EnableCommands(bool status)
		{
			foreach (System.Windows.Forms.Control ctrl in this.Controls)
			{
				ctrl.Enabled = status;
			}
			if (!status)
			{
				this.btnExit.Enabled = true;
			}
		}

		void ManualBuildBtnClick(object sender, EventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "ManualBuildBtnClick");
			SaveSettings();
			this.Close();
		}
		
		void BtnExitClick(object sender, EventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "BtnExitClick");
			Close();
		}
		
		public void SetActiveComponent(string ActiveComponent)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveComponent", ActiveComponent);
		}

		public void SetActiveFolder(string ActiveFolder)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFolder", ActiveFolder);
		}

		
		void BFitRailLengthCheckedChanged(object sender, EventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","bFitRailLength", bFitRailLength.Checked.ToString());
		}
		
		void CreateTrackFamilyBtnClick(object sender, EventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "createTrackFamilyBtnClick");
			Close();
		}
		
		void GenerateComponentFamiliesBtnClick(object sender, EventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "GenerateComponentFamiliesBtnClick");
			Close();
		}
		
		void AppSetupBtnClick(object sender, EventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "AppSetupBtnClick");
			Close();
		}
		
		void BtnDrawCenterLineClick(object sender, EventArgs e)
		{
			Interaction.SaveSetting("AlignmentTools","AppData","ActiveFunction", "BtnDrawCenterLineClick");
			SaveSettings();
			this.Close();
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

}
