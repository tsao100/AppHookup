/*
 * 由SharpDevelop创建。
 * 用户： Jack Tsao
 * 日期: 2019/3/8
 * 时间: 上午 11:07
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Windows;
using TrackTools;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Linq;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;


namespace TrackTools.TrackAlignments
{
	/// <summary>
	/// Description of TrackAlignment.
	/// </summary>
	public class TrackCenterLine
	{
		int HIndex;
		int VIndex;
		string ALDPathName;
		string VALDPathName;
		AlignmentData[] ALDData;
		VerticalAlignment[] VALDData;
		double OffsetLimit;  //反求外移距時, 外移距過大可能為不合理情況, 故限制其值。
		public TrackCenterLine()
		{

			ALDPathName=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\DataTable\\"+
				Interaction.GetSetting("AlignmentTools","AppData","ALDName1","Y06UH.ALD")+"";
			
			VALDPathName=ALDPathName.Replace("H.ALD","V.ALD");
			
			
			ReadALDData();
			ReadVALDData();
			OffsetLimit = Convert.ToDouble(Interaction.GetSetting("AlignmentTools","AppData","OffsetLimit","35.00000"));

		}
		
		public TrackCenterLine(string ALDNameNo)
		{

			ALDPathName=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\DataTable\\"+
				Interaction.GetSetting("AlignmentTools","AppData",ALDNameNo,"Y06UH.ALD")+"";
			
			VALDPathName=ALDPathName.Replace("H.ALD","V.ALD");
			
			
			ReadALDData();
			ReadVALDData();
			OffsetLimit = Convert.ToDouble(Interaction.GetSetting("AlignmentTools","AppData","OffsetLimit","35.00000"));

		}
		
		public double[] Txyz(double x, double y, double z)
		{
			
			double[] XYZ=new Double[3];
			XYZ[0]=x+y;
			XYZ[1]=y+z;
			XYZ[2]=z+x;
			
			return XYZ;
		}
		
		public double[] Getxyz(double p, double w) //p: Chainage, w:Offset
		{
			
			double[] XYZ = new double[2];
			XYZ = Getxy(p, w);
			Array.Resize(ref XYZ, 3);
			XYZ[2]= Getz(p);
			
			return XYZ;
		}

		public double[] Getpw(double x, double y) //x: Easting, y:Northing
		{
			GetHIndex(x, y);
			
			double[] PW=new Double[2];
			
			switch (ALDData[HIndex].TSC.Substring(1,1)) {
				case "T":
					PW=Tpw(x, y);
					break;
				case "C":
					PW=Cpw(x, y);
					break;
				case "S":
					switch (ALDData[HIndex-1].TSC.Substring(1,1) + ALDData[HIndex+1].TSC.Substring(1,1)) {
						case "TC":
							PW = SNORpw(x, y); //Normal direction
							break;
						case "CC":
							PW = SEGGpw(x, y); //EGG for Clothoid only.
							break;
						case "CT":
							PW = SREVpw(x, y); //Reverse direction
							break;
						default:
							
							break;
					}
					
					break;
				default:
					
					break;
			}			
			
			return PW;
		}
		
		private double[] Cpw(double x, double y)
		{
			double XC = ALDData[HIndex].Easting;
			double YC = ALDData[HIndex].Northing; 
			double Asta = ALDData[HIndex].Chainage; 
			double ALFAC = ALDData[HIndex].Azimuth; 
			double R = ALDData[HIndex].Radius;
			return Cpw(XC, YC, Asta, ALFAC, R, x, y);
		}
		
		private double[]  Cpw(double XC, double YC, double Asta, double ALFAC, double R, double x, double y)
		{
			double[] XY1 = new double[2], XY2 = new double[2], PW = new double[2];
			double L=0;
		    double ALFAP = ALFAC - Math.PI*0.5*Math.Sign(R);
		    XY2 = PtoRxy(-Math.Abs(R), polar(ALFAP));
		    XY2[0] = XC + XY2[0];
		    XY2[1] = YC + XY2[1];
		    XY1[0] = x - XY2[0];
		    XY1[1] = y - XY2[1];
		    double R1 = rtopr(XY1[0], XY1[1]);
		    double ALFAPP = amz(rtopa(XY1[0], XY1[1]));
		    double SITA = ALFAPP - Normalize(ALFAP);
		    if (R * SITA >= 0)
		        L = R * SITA;
		    else
		        L = (R * SITA + Math.Abs(R) * Math.PI * 2);
	    	PW[0] = L + Asta;
	    	PW[1] = R - R1 * Math.Sign(R);
	    	
	    	return PW;
		}
		    	
	
		
		private double[] SNORpw(double x, double y)
		{
			double XS = ALDData[HIndex].Easting;
			double YS = ALDData[HIndex].Northing;
			double Asta = ALDData[HIndex].Chainage;
			double R = ALDData[HIndex+1].Radius;
			double LS = ALDData[HIndex].Length;
			double angle = ALDData[HIndex].Azimuth;
			
			return Spw(XS, YS, Asta, R, LS, angle, x, y);			
		}

		private double[] SEGGpw(double x, double y)
		{
			//計算蛋形座標X
			//R1,R2 皆需考慮正負號
			double[] PW = new double[2];
			double X1 = ALDData[HIndex].Easting;
			double Y1 = ALDData[HIndex].Northing;
			double Asta = ALDData[HIndex].Chainage;
			double R1 = ALDData[HIndex-1].Radius;
			double R2 = ALDData[HIndex+1].Radius;
			double LE = ALDData[HIndex].Length;
			double ALFA1 = ALDData[HIndex].Azimuth;
			double[] XYS = BEXY(X1, Y1, R1, R2, LE, ALFA1);
			double XS = XYS[0];
		    double YS = XYS[1];
		    double LS = OLS(R1, R2, LE);
		    double angle = BEA(R1, R2, LE, ALFA1);
		    if (Math.Abs(R1) >= Math.Abs(R2))
		    {
		        double R = R2;
		        //double Psta1 = Psta - Asta + LS - LE;
		        PW = Spw(XS, YS, 0, R, LS, angle, x, y);
		        PW[0] = PW[0] - Asta + LS - LE;
		        return PW;
		    }
		    else
		    {
		        double R = -R1;
		        //double Psta1 = Asta + LS - Psta;
		        PW = Spw(XS, YS, 0, R, LS, angle, x, y);
		        PW[0] = Asta + LS - PW[0];
		        PW[1] = - PW[1];
		    	return PW;
		    }						
		}
		
		private double[] SREVpw(double x, double y)
		{
			double XS = ALDData[HIndex+1].Easting;
			double YS = ALDData[HIndex+1].Northing;
			double Asta = 0;
			double p = ALDData[HIndex+1].Chainage;
			double R = - ALDData[HIndex-1].Radius;
			double LS = ALDData[HIndex].Length;
			double angle = ALDData[HIndex+1].Azimuth + Math.PI;
			double[] PW = Spw(XS, YS, Asta, R, LS, angle, x, y);
			PW[0] = p - PW[0];
			PW[1] = - PW[1];
			return PW;
		}
		
		private double[] Spw(double XS, double YS, double Asta, double R, double LS, double Alfa, double x, double y)
		{
			double[] PW = new double[2], XY1 = new double[2];
			double a1;
			double[] PW1 = Tpw(XS, YS, Alfa + Math.PI*0.5, 0, x, y); //與起點法線比較
    
    
			double lens1 = PW1[1];
			while (Math.Abs(PW1[1]) > 0.000001)
			{
			    XY1 = Sxy(XS, YS, Asta, R, LS, Alfa, Asta - lens1, 0);
			    a1 = Sa(Asta, Asta - lens1, R, LS, Alfa);
			    
			    PW1 = Tpw(XY1[0], XY1[1], a1 + Math.PI*0.5, 0, x, y);
			    
			    lens1 = lens1 + PW1[1];
			
			} 
			
			PW[0] = - lens1 + Asta;
			PW[1] = PW1[0];
			return PW;
		}
		
		public List<XYZ> GetPoints()
		{
			List<XYZ> pts = new List<XYZ>();
			pts.Add(new XYZ(0,0,0));
			return pts;
		}

		public List<XYZ> Getxy() //Provide the points to draw the track centerline as ModelHermitSpline
		{
			List<XYZ> pts = new List<XYZ>();
			double[] XY=new Double[2];
			
			for (HIndex = 0; HIndex < ALDData.Count()-1; HIndex++) 
			{
				switch (ALDData[HIndex].TSC.Substring(1,1)) {
					case "T":
						Txy(pts);
						break;
					case "C":
						Cxy(pts);
						break;
					case "S":
						switch (ALDData[HIndex-1].TSC.Substring(1,1) + ALDData[HIndex+1].TSC.Substring(1,1)) {
							case "TC":
								SNORxy(pts); //Normal direction
								break;
							case "CC":
								SEGGxy(pts); //EGG for Clothoid only.
								break;
							case "CT":
								SREVxy(pts); //Reverse direction
								break;
							default:
								
								break;
						}
						
						break;
					default:
						
						break;
				}
			}
			pts.Add(new XYZ(ALDData[HIndex].Easting,ALDData[HIndex].Northing,0));
			return pts;
		}
		
		private int Cxy(List<XYZ> pts)
		{
			double sections = Math.Floor(ALDData[HIndex].Length/3);
			double interval = ALDData[HIndex].Length/sections;
			double p=ALDData[HIndex].Chainage;
			double[] XY=new Double[2];
			for (int i = 0; i < sections; i++) {
				XY = Cxy(p, 0);
				pts.Add(new XYZ(XY[0], XY[1],0));
				p+=interval;
			}
			return 0;	
		}
		
		private int Txy(List<XYZ> pts)
		{
			double sections = Math.Floor(ALDData[HIndex].Length/3);
			double interval = ALDData[HIndex].Length/sections;
			double p=ALDData[HIndex].Chainage;
			double[] XY=new Double[2];
			for (int i = 0; i < sections; i++) {
				XY = Txy(p, 0);
				pts.Add(new XYZ(XY[0], XY[1],0));
				p+=interval;
			}
			return 0;	
		}
		
		private int SNORxy(List<XYZ> pts)
		{
			double sections = Math.Floor(ALDData[HIndex].Length/3);
			double interval = ALDData[HIndex].Length/sections;
			double p=ALDData[HIndex].Chainage;
			double[] XY=new Double[2];
			for (int i = 0; i < sections; i++) {
				XY = SNORxy(p, 0);
				pts.Add(new XYZ(XY[0], XY[1],0));
				p+=interval;
			}
			return 0;	
		}
		
		private int SEGGxy(List<XYZ> pts)
		{
			double sections = Math.Floor(ALDData[HIndex].Length/3);
			double interval = ALDData[HIndex].Length/sections;
			double p=ALDData[HIndex].Chainage;
			double[] XY=new Double[2];
			for (int i = 0; i < sections; i++) {
				XY = SEGGxy(p, 0);
				pts.Add(new XYZ(XY[0], XY[1],0));
				p+=interval;
			}
			return 0;	
		}
		private int SREVxy(List<XYZ> pts)
		{
			double sections = Math.Floor(ALDData[HIndex].Length/3);
			double interval = ALDData[HIndex].Length/sections;
			double p=ALDData[HIndex].Chainage;
			double[] XY=new Double[2];
			for (int i = 0; i < sections; i++) {
				XY = SREVxy(p, 0);
				pts.Add(new XYZ(XY[0], XY[1],0));
				p+=interval;
			}
			return 0;	
		}
		
		

		public double[] Getxy(double p, double w) //p: Chainage, w:Offset
		{
			GetHIndex(p);
			
			double[] XY=new Double[2];
			
			switch (ALDData[HIndex].TSC.Substring(1,1)) {
				case "T":
					XY=Txy(p, w);
					break;
				case "C":
					XY=Cxy(p, w);
					break;
				case "S":
					switch (ALDData[HIndex-1].TSC.Substring(1,1) + ALDData[HIndex+1].TSC.Substring(1,1)) {
						case "TC":
							XY = SNORxy(p, w); //Normal direction
							break;
						case "CC":
							XY = SEGGxy(p, w); //EGG for Clothoid only.
							break;
						case "CT":
							XY = SREVxy(p, w); //Reverse direction
							break;
						default:
							
							break;
					}
					
					break;
				default:
					
					break;
			}			
			
			return XY;
		}
		
		public double Getz(double p) //p: Chainage
		{
			GetVIndex(p);			
			double Z=0.0;
			
			if(VALDData[VIndex].Grade != VALDData[VIndex+1].Grade)
			{
				Z=SYVL(p);
			}
			else
			{
				Z=TVL(p);
			}			
			
			return Z;
		}
		
		public double GetStartLocalChainage()
		{
			return ALDData[0].Chainage;
		}

		public double Geta(double p) //p: Chainage Get tangential Azimuth of the specified chainage p.
		{
			GetHIndex(p);
			
			double a = 0;
			
			switch (ALDData[HIndex].TSC.Substring(1,1)) {
				case "T":
					a =Ta();
					break;
				case "C":
					a =Ca(p);
					break;
				case "S":
					switch (ALDData[HIndex-1].TSC.Substring(1,1) + ALDData[HIndex+1].TSC.Substring(1,1)) {
						case "TC":
							a = SNORa(p); //Normal direction
							break;
						case "CC":
							a = SEGGa(p); //EGG for Clothoid only.
							break;
						case "CT":
							a = SREVa(p); //Reverse direction
							break;
						default:							
							break;
					}					
					break;
				default:					
					break;
			}
			return a;
		}
		
		public double[] Getas(double p) //p: Chainage. Get tangential Azimuth and slope % of the specified chainage p. XY[0]: Azimuth, XY[1]: Slope in %.
		{
			double[] XY = new double[2];
			XY[0] = Geta(p);
			XY[1] = Gets(p);
			return XY;
		}


		public double Gets(double p) //p: Chainage. Get slope % of the specified chainage p.
		{
			GetVIndex(p);
			
			double s=0;			
			
			if(VALDData[VIndex].Grade != VALDData[VIndex+1].Grade)
			{
				s=SYVLs(p);
			}
			else
			{
				s=TVLs();
			}			
			
			return s;
		}
		
		public double SYVLs(double p)
		{
			double x = p - VALDData[VIndex].Chainage;
			return (VALDData[VIndex].Grade + (VALDData[VIndex+2].Grade - VALDData[VIndex].Grade) / VALDData[VIndex+1].LVC * x);
		}

		public double TVLs()
		{
			return VALDData[VIndex].Grade;
		}
		
		private double[] SNORxy(double Psta, double w) //Normal
		{
			double XS = ALDData[HIndex].Easting;
			double YS = ALDData[HIndex].Northing;
			double Asta = ALDData[HIndex].Chainage;
			double R = ALDData[HIndex+1].Radius;
			double LS = ALDData[HIndex].Length;
			double angle = ALDData[HIndex].Azimuth;
			
			return Sxy(XS, YS, Asta, R, LS, angle, Psta, w);
			
		}
		
		private double SNORa(double Psta) //Normal
		{
			double Asta = ALDData[HIndex].Chainage;
			double R = ALDData[HIndex+1].Radius;
			double LS = ALDData[HIndex].Length;
			double angle = ALDData[HIndex].Azimuth;
			
			return Sa(Asta, Psta, R, LS, angle);
			
		}
		
		private double SREVa(double Psta) //Reverse
		{
			double Asta = 0;
			double p = ALDData[HIndex+1].Chainage - Psta;
			double R = -ALDData[HIndex-1].Radius;
			double LS = ALDData[HIndex].Length;
			double angle = ALDData[HIndex+1].Azimuth + Math.PI;
			
			return Normalize(Sa(Asta, p, R, LS, angle) + Math.PI);
			
		}
		
		private double Sa(double Asta, double Psta, double R, double LS, double angle)
		{
		    double L = Psta - Asta;
		    double SITA=0, x;
		    switch (ALDData[HIndex].CurveType)
		    {
			    case "SPIRAL":
				    double a = Math.Sqrt(Math.Abs(R) * LS) * Math.Sign(R);
				    SITA = (L / a) * (L / a) / 2 * Math.Sign(a);
					break;
			    case "HALFSINE":
				    SITA = 1 / (2 * R) * (L - LS / Math.PI * Math.Sin(L / LS * Math.PI));
					break;
			    case "PARABOLA":
						x = L - Math.Pow(L, 5) / (40 * (R * LS)* (R * LS));
						double XB = LS - Math.Pow(LS, 3) / (40 * R * R);
				    SITA = Math.Atan2(x * x, (2 * Math.Abs(R) * XB)) * Math.Sign(R);
					break;    
			    case "CUBICJPN":  //PARABOLICbole Japan
				    double BigX = BIGFXJPN(LS, R);
				    x = FXJPN(BigX, L, R);
				    SITA = Math.Atan2(x * x / (2 * R * BigX),1);
				    
				    
//				    double BigX = BIGFXJPN(LS, R);
//				    XY[0] = FXJPN(BigX, L, R);
//				    XY[1] = Math.Pow(XY[0], 3 )/ (6 * R * BigX);
//				    SITA = Math.Atan2(1, Math.Pow(XY[0], 2) / (2 * R * BigX));
				    
				    break;
			    case "CUBICECI":  //PARABOLICbole CECI
				    SITA = BIGFPCECI(L, Math.Abs(R)) * Math.Sign(R);
					break;
			
				default:
					break;
		
		    }
		    
		    return Normalize(angle + SITA);
		}


		private double[] SEGGxy(double Psta, double w) //EGG
		{
			//計算蛋形座標X
			//R1,R2 皆需考慮正負號
			double X1 = ALDData[HIndex].Easting;
			double Y1 = ALDData[HIndex].Northing;
			double Asta = ALDData[HIndex].Chainage;
			double R1 = ALDData[HIndex-1].Radius;
			double R2 = ALDData[HIndex+1].Radius;
			double LE = ALDData[HIndex].Length;
			double ALFA1 = ALDData[HIndex].Azimuth;
			double[] XYS = BEXY(X1, Y1, R1, R2, LE, ALFA1);
			double XS = XYS[0];
		    double YS = XYS[1];
		    double LS = OLS(R1, R2, LE);
		    double angle = BEA(R1, R2, LE, ALFA1);
		    if (Math.Abs(R1) >= Math.Abs(R2))
		    {
		        double R = R2;
		        double Psta1 = Psta - Asta + LS - LE;
		        	return Sxy(XS, YS, 0, R, LS, angle, Psta1, w);
		    }
		    else
		    {
		        double R = -R1;
		        double Psta1 = Asta + LS - Psta;
		    	return Sxy(XS, YS, 0, R, LS, angle, Psta1, -w);
		    }						
		}
		
		private double SEGGa(double P) //EGG
		{
			//計算蛋形座標X
			//R1,R2 皆需考慮正負號
			double Asta = ALDData[HIndex].Chainage;
			double R1 = ALDData[HIndex-1].Radius;
			double R2 = ALDData[HIndex+1].Radius;
			double LE = ALDData[HIndex].Length;
			double ALFA1 = ALDData[HIndex].Azimuth;
		    double LS = OLS(R1, R2, LE);
		    double angle = BEA(R1, R2, LE, ALFA1);
		    double Psta;
		    if (Math.Abs(R1) >= Math.Abs(R2))
		    {
		        Psta = P - Asta + LS - LE;
		        return Sa(0, Psta, R2, LS, angle);
		    }
		    else
		    {
		        Psta = Asta + LS - P;
		        return Sa(0, Psta, -R1, LS, angle - Math.PI);
		    }
		}
		
		private double[] SREVxy(double Psta, double w) //Reverse
		{
			double XS = ALDData[HIndex+1].Easting;
			double YS = ALDData[HIndex+1].Northing;
			double Asta = 0;
			double p = ALDData[HIndex+1].Chainage - Psta;
			double R = - ALDData[HIndex-1].Radius;
			double LS = ALDData[HIndex].Length;
			double angle = ALDData[HIndex+1].Azimuth + Math.PI;
			
			return Sxy(XS, YS, Asta, R, LS, angle, p, -w);
		}
		
		private double[] Sxy(double XS, double YS, double Asta, double R, double LS, double angle, double Psta, double w)
		{
			double[] XY=new Double[2];
			double SITA = 0;
			double L = Psta - Asta;
			
			switch (ALDData[HIndex].CurveType) {
				case "SPIRAL":
					double a = Math.Sqrt(Math.Abs(R) * LS) * Math.Sign(R);
					SITA = Math.Pow(L / a, 2) / 2 * Math.Sign(a);
					XY[0] = L * (1 -  Math.Pow(SITA, 2) / 10 +  Math.Pow(SITA, 4) / 216 -  Math.Pow(SITA, 6) / 9360);
					XY[1] = L * SITA * (1.0 / 3 -  Math.Pow(SITA, 2) / 42 +  Math.Pow(SITA, 4) / 1320);
					break;
				case "HALFSINE":
				    double b = 1 / (2 * R);
				    double la = Math.PI / LS;
				    double Ba = la * L;
				    XY[0] = (L) - Math.Pow(b, 2) * (2 * Math.Pow(Ba, 3) - 12 *  Math.Sin(Ba) + 12 * Ba *  Math.Cos(Ba) - 3 *  Math.Cos(Ba) *  Math.Sin(Ba) + 3 * Ba) / 12 / Math.Pow(la, 3);
				    XY[1] = b * (Math.Pow(L, 2) + 2 * ( Math.Cos(Ba) - 1) / Math.Pow(la, 2)) / 2 - Math.Pow(b, 3) * (3 * Math.Pow(Ba, 4) + 36 * Math.Pow(Ba, 2) *  Math.Cos(Ba) - 
				                                 60 *  Math.Cos(Ba) - 72 * Ba *  Math.Sin(Ba) - 18 * Ba *  Math.Cos(Ba) * Math.Sin(Ba) + 9 * Math.Pow(Ba, 2) - 9 * Math.Pow( Math.Cos(Ba), 2) -
				                                 4 * Math.Pow( Math.Cos(Ba), 3) + 73) / 72 / Math.Pow(la, 4);
				    SITA = 1 / (2 * R) * (L - LS / Math.PI * Math.Sin(L / LS * Math.PI));
					
					break;
				case "PARABOLA":
					XY[0] = L - Math.Pow(L, 5) / (40 * Math.Pow(R * LS, 2));
					double XB = LS - Math.Pow(LS, 3) / (40 * Math.Pow(R, 2));
					XY[1] = Math.Pow(XY[0], 3) / (6 * R * XB);
				    SITA = Math.Atan2(Math.Pow(XY[0], 2), (2 * Math.Abs(R) * XB)) * Math.Sign(R);
					break;
				    
				case "CUBICJPN":  //PARABOLICbole Japan
				    double BigX = BIGFXJPN(LS, R);
				    XY[0] = FXJPN(BigX, L, R);
				    XY[1] = Math.Pow(XY[0], 3 )/ (6 * R * BigX);
				    SITA = Math.Atan2(Math.Pow(XY[0], 2) / (2 * R * BigX),1);
					break;
				
				case "CUBICECI":  //PARABOLICbole CECI
				    double AbsR = Math.Abs(R);
				    double AA = BIGFACECI(LS, AbsR);
				    XY[0] = BIGFXCECI(L, AbsR);
				    XY[1] = Math.Pow(XY[0], 3 ) / (6 * AA) * Math.Sign(R);
				    SITA = BIGFPCECI(L, Math.Abs(R)) * Math.Sign(R);
					break;
				default:
					
					break;
			}
			
			double X1 = XY[0] - w * Math.Sin(SITA);
			double Y1 = XY[1] + w * Math.Cos(SITA);
			double R1 = rtopr(X1, Y1);
			double S1 = rtopa(X1, Y1);
			double[] XY2 = PtoRxy(R1, polar(S1 + angle));
			XY[0] = XS + XY2[0];
			XY[1] = YS + XY2[1];
			
			return XY;	
		}
		
		private double BIGFXJPN(double LS, double R)
		{
		    double a = 0.00001;
		    double b = LS;
		    double Fa, Fc, c;
	    	while (Math.Abs(a - b) > 0.000001)
	    	{
		        c = (a + b) / 2;
		        
		        if (a == 0)
		            Fa = 0;
		        else
		        	Fa = a - LS * (10 / (10 + Math.Pow(a /(2 * R), 2)));
		        
		        Fc = c - LS * (10 / (10 + Math.Pow(c/(2 * R), 2)));
		        
		        if (Fa * Fc > 0)
		            a = c;
		        else
		            b = c;
	        }
			return (a + b) / 2;
		}
		
		private double FXJPN(double BigX, double L, double R)
		{ 
		    double a = 0.00001;
		    double b = BigX;
		    double Fa, c, Fc;
		    while (Math.Abs(a - b) > 0.000001)
		    {
		        c = (a + b) / 2;
		        if (a == 0)
		            Fa = 0;
		        else
		        	Fa = a - L * (10 / (10 + Math.Pow(a*a/(2 * R * BigX), 2)));
		        
		        Fc = c - L * (10 / (10 + Math.Pow(c*c/(2 * R * BigX), 2)));
		        
              	if (Fa * Fc > 0)
		            a = c;
		        else
		            b = c;
	        }
			return (a + b) / 2;
		}
		
		private double BIGFACECI(double LS, double R)
		{ 
		    double a = 0;
		    double b = 1;
		    double Fa, p, Fc, c;
		    while ((Math.Abs(a - b)) > 0.0000000001)
		    {
		        c = (a + b) / 2;
		        
		        p = Math.Tan(a);
		        Fa = LS / (2 * R) - (p / Math.Pow(1 + Math.Pow(p, 2), 1.5)) * (1 + Math.Pow(p, 2) / 10 - Math.Pow(p, 4) / 72 + Math.Pow(p, 6) / 208);
		        
		        p = Math.Tan(c);
		        Fc = LS / (2 * R) - (p / Math.Pow(1 +  Math.Pow(p, 2), 1.5)) * (1 +  Math.Pow(p, 2) / 10 -  Math.Pow(p, 4) / 72 +  Math.Pow(p, 6) / 208);
		        
		        if (Fa * Fc > 0)
		            a = c;
		        else
		            b = c;
		    }
		    p = Math.Tan((a + b) / 2);
		    
		    double x = LS / (1 + Math.Pow(p, 2) / 10 - Math.Pow(p, 4) / 72 + Math.Pow(p, 6) / 208);
			return x * x / (2 * p);
		}
		
		private double BIGFXCECI(double LS, double R)
		{ 
		    double a = 0;
		    double b = 1;
		    double c, Fa, Fc, p;
		    while ((Math.Abs(a - b)) > 0.0000000001)
		    {
		        c = (a + b) / 2;
		        
		        p = Math.Tan(a);
		        Fa = LS / (2 * R) - (p / Math.Pow(1 + Math.Pow(p, 2), 1.5)) * (1 + Math.Pow(p, 2) / 10 - Math.Pow(p, 4) / 72 + Math.Pow(p, 6) / 208);
		        
		        p = Math.Tan(c);
		        Fc = LS / (2 * R) - (p / Math.Pow(1 + Math.Pow(p, 2), 1.5)) * (1 + Math.Pow(p, 2) / 10 - Math.Pow(p, 4) / 72 + Math.Pow(p, 6) / 208);
		        
		        if (Fa * Fc > 0)
		            a = c;
		        else
		            b = c;
		    }
		    p = Math.Tan((a + b) / 2);
	
			return LS / (1.0 + Math.Pow(p, 2) / 10.0 - Math.Pow(p, 4) / 72.0 + Math.Pow(p, 6) / 208.0);
		}
		
		private double BIGFPCECI(double LS, double R)
		{ 
		    double a = 0;
		    double b = 1;
		    double c, p, Fa, Fc;
		    while ((Math.Abs(a - b)) > 0.0000000001)
		    {
		        c = (a + b) / 2;
		        
		        p = Math.Tan(a);
		        Fa = LS / (2 * R) - (p / Math.Pow(1 + Math.Pow(p, 2), 1.5)) * (1 + Math.Pow(p, 2) / 10 - Math.Pow(p, 4) / 72 + Math.Pow(p, 6) / 208);
		        
		        p = Math.Tan(c);
		        Fc = LS / (2 * R) - (p / Math.Pow(1 + Math.Pow(p, 2), 1.5)) * (1 + Math.Pow(p, 2) / 10 - Math.Pow(p, 4) / 72 + Math.Pow(p, 6) / 208);
		        
		        if (Fa * Fc > 0)
		            a = c;
		        else
		            b = c;
		    }
			return (a + b) / 2;
		}
		
		private double[] BEXY(double X1, double Y1, double R1, double R2, double LE, double ALFA1)
		{
		    double LS = OLS(R1, R2, LE);
			double[] XY = new Double[2];
			double[] BXY = new Double[2];
			double OALFA;
			if (Math.Abs(R1) >= Math.Abs(R2))
			{
		        OALFA = ALFA1 - ((LS - LE) * (LS - LE) / (2 * LS * R2));
		        XY = Sxy(0, 0, 0, R2, LS, Math.PI*0.5, LS - LE, 0);
		        return Txy(X1, Y1, OALFA, 0, -XY[0], XY[1]);
			}
		    else
		    {
		        OALFA = ALFA1 + (LS * LS / (2 * LS * R1));
		        XY = Sxy(0, 0, 0, -R1, LS, Math.PI*0.5, LS, 0);
		        return Txy(X1, Y1, OALFA, 0, XY[0], -XY[1]);
		    }			
		}
		
		private double OLS(double R1, double R2, double LE)
		{
			if (Math.Abs(R1) >= Math.Abs(R2))
		        return  LE * Math.Abs(R1) / Math.Abs(Math.Abs(R1) - Math.Abs(R2));
		    else
		        return  LE * Math.Abs(R2) / Math.Abs(Math.Abs(R1) - Math.Abs(R2));
		}

		private double BEA(double R1, double R2, double LE, double ALFA1)
		{
		    double LS = OLS(R1, R2, LE);
		    if (Math.Abs(R1) >= Math.Abs(R2))
		        return ALFA1 - ((LS - LE) * (LS - LE) / (2 * LS * R2));
		    else
		        return ALFA1 + (LS * LS / (2 * LS * R1)) + Math.PI;
		}

		
		private double SYVL(double c4, double g1, double g2, double e2, double VCL, double p)
		{
			double x = p - c4;
    		return e2 - VCL * 
    			(g1 / 100) / 2 + (g1 / 100) * 
    			x - ((g1 / 100) - (g2 / 100)) * x*x / (2 * VCL);

		}
		
		private double SYVL(double p)
		{
			double x = p - VALDData[VIndex].Chainage;
    		return VALDData[VIndex+1].PviElevation - VALDData[VIndex+1].LVC * 
    			(VALDData[VIndex].Grade / 100) / 2 + (VALDData[VIndex].Grade / 100) * 
    			x - ((VALDData[VIndex].Grade / 100) - (VALDData[VIndex+2].Grade / 100)) * x*x / (2 * VALDData[VIndex+1].LVC);

		}
		
		private double TVL(double c1, double g1, double e1,double p)
		{
			return (p - c1) * g1 / 100 + e1;
		}
		
		private double TVL(double p)
		{
			return (p - VALDData[VIndex].Chainage) * VALDData[VIndex].Grade / 100 + VALDData[VIndex].Elevation;
		}
		
		private double[] Cxy(double p, double w)
		{
			double[] XY=new Double[2];
			double[] XY1=new Double[2];
			double ALFAP = ALDData[HIndex].Azimuth - Math.PI*0.5 * Math.Sign(ALDData[HIndex].Radius);
			XY = getCenter();
			
			double L = p - ALDData[HIndex].Chainage;
			double R1 = Math.Abs(ALDData[HIndex].Radius - w);
			double SITA1 = L / ALDData[HIndex].Radius;
			double ALFAPP = SITA1 + ALFAP;
			XY1 = PtoRxy(R1, polar(ALFAPP));
			XY[0] = XY1[0] + XY[0];
			XY[1] = XY1[1] + XY[1];

			return XY;
		}
		
		private double Ca(double p)
		{
		    double L = p - ALDData[HIndex].Chainage;
		    double SITA1 = L / ALDData[HIndex].Radius;
		    return Normalize(ALDData[HIndex].Azimuth + SITA1);
		}

		
		private double[] getCenter()
		{
			double[] XY=new Double[2];
			double ALFAP = ALDData[HIndex].Azimuth - Math.PI*0.5 * Math.Sign(ALDData[HIndex].Radius);
			XY=PtoRxy(- Math.Abs(ALDData[HIndex].Radius), polar(ALFAP));
			XY[0] = ALDData[HIndex].Easting + XY[0];
			XY[1] = ALDData[HIndex].Northing + XY[1];
			return XY;
		}
		
		private double[] Txy(double p, double w)
		{
			return Txy(ALDData[HIndex].Easting, ALDData[HIndex].Northing, ALDData[HIndex].Azimuth, ALDData[HIndex].Chainage, p, w);
		}

		private double[] Tpw(double x, double y)
		{
			return Tpw(ALDData[HIndex].Easting, ALDData[HIndex].Northing, ALDData[HIndex].Azimuth, ALDData[HIndex].Chainage, x, y);
		}

		private double[] Tpw(double xa, double ya, double angle, double Asta, double x, double y)
		{
			double[] PW = new double[2];
			
		    double R1 = rtopr(x - xa, y - ya);
		    double SITA = rtopa(x - xa, y - ya);
		    PW = PtoRxy(R1, Normalize(amz(SITA) - angle));
    		PW[0] = PW[0] + Asta;
			
			return PW;
		}
		
		
		private double Ta()
		{
			return ALDData[HIndex].Azimuth;
		}

		
		public double[] Txy(double x, double y, double amz, double Asta, double p, double w)
		{
			double[] XY=new Double[2];
			double L = p - Asta;
			double R = rtopr(L, w);
			double angle = polar(amz) + rtopa(L, -w);
			double[] X1 = PtoRxy(R, angle);
				
			XY[0] = x + X1[0];
			XY[1] = y + X1[1];
		
			return XY;
		}
		
		private double polar(double Azimuth)
		{
			return Normalize(Math.PI*0.5-Azimuth);
		}

		private double amz(double angle)
		{
			return polar(angle);
		}
		
		private double Normalize(double angle) //Make Angle value between 0~2pi
		{
			double temp = angle;
			while (temp < 0)
				temp+=Math.PI*2.0;
			while (temp > Math.PI*2.0)
				temp-=Math.PI*2.0;
			return temp;
		}

		
		private double[] PtoRxy(double R, double theda)
		{
			double[] XY=new Double[2];
			XY[0] = R * Math.Cos(theda);
			XY[1] = R * Math.Sin(theda);
			return XY;
		}
		
		private double rtopr(double x, double y)
		{
			return Math.Sqrt(x*x+y*y);
		}
		
		private double rtopa(double x, double y)
		{
			if (x == 0)
			{
				if (y > 0)
    				return Math.PI*0.5; 
    			else
    				return Math.PI*1.5;    
			}
			else
			{
				if (x > 0) 
        			return Math.Atan(y / x);
				else
	        		return Math.Atan(y / x) + Math.PI;
			}			
		}
		
		public int GetHIndex(double SearchKey)  //Apply Binary Search Algorithm, return -1 if failed
		{
            int left = 0 ; 
            int right = ALDData.Count()-1;
             if ( SearchKey == ALDData[ ALDData.Count()-1].Chainage)
            {
            	HIndex = ALDData.Count()-2;
                return ALDData.Count()-2;
            }
           while (left <= right)
            {
                int mid = (left + right) / 2;//取中間位子當基準
                if ((ALDData[mid].Chainage-SearchKey)<=0 && (ALDData[mid+1].Chainage-SearchKey)>0)
                {
                	HIndex = mid;
                    return mid;//找到的index值
                }
                else 
                {
                    if (ALDData[mid].Chainage < SearchKey)//在右邊的數列
                    {
                        left = mid + 1;
                    }
                    else//在左邊的數列
                    {
                        right = mid - 1;
                    }
                }

            }
            return -1;//找不到時
		}

		public int GetHIndex(double x, double y)  //Apply Binary Search Algorithm, return -1 if failed, x: Easting, y:Northing
		{										  // 後續進行髮夾彎測試及研究。
            int left = 0 ; 
            int right = ALDData.Count()-1;
            double[] PW1 = new double[2], PW2 = new double[2];
            PW2 = GetTpw(ALDData.Count()-1, x, y);
            if (Math.Abs(PW2[0])<= OffsetLimit && Math.Abs(PW2[1]) <= 0.000001)
            {
            	HIndex = ALDData.Count()-2;
                return ALDData.Count()-2;
            }
           while (left <= right)
            {
                int mid = (left + right) / 2;//取中間位子當基準
                PW1 = GetTpw(mid, x, y);
                PW2 = GetTpw(mid + 1, x, y);
                if (PW1[1]<=0 && PW2[1]>0 && Math.Abs(PW1[0])<= OffsetLimit && Math.Abs(PW2[0])<= OffsetLimit)
                {
                	HIndex = mid;
                    return mid;//找到的index值
                }
                else 
                {
                    if (PW1[1]<=0 && PW2[1]<0 /*&& Math.Abs(PW1[0])<= OffsetLimit * 2 && Math.Abs(PW2[0])<= OffsetLimit * 2 */)//在右邊的數列
                    {
                        left = mid + 1;
                    }
                    else//在左邊的數列
                    {
                        right = mid - 1;
                    }
                }

            }
            return -1;//找不到時
		}
		
		private double[] GetTpw(int i, double x, double y) //i: 第i個線形變化點。
		{
			return Tpw(ALDData[i].Easting, ALDData[i].Northing, ALDData[i].Azimuth + Math.PI*0.5, 0, x, y);
		}
		
		public int GetVIndex(double SearchKey)  //Apply Binary Search Algorithm, return -1 if failed
		{
            int left = 0 ; 
            int right = VALDData.Count()-1;
            if ( SearchKey == VALDData[ VALDData.Count()-1].Chainage)
            {
            	VIndex = VALDData.Count()-2;
                return VIndex;
            }
            while (left <= right)
            {
                int mid = (left + right) / 2;//取中間位子當基準
                if ((VALDData[mid].Chainage-SearchKey)<=0 && (VALDData[mid+1].Chainage-SearchKey)>0)
                {
                	if(VALDData[mid].Grade != VALDData[mid+1].Grade && (mid % 3) == 2)
                		mid--;
                	VIndex = mid;
                    return mid;//找到的index值
                }
                else 
                {
                    if (VALDData[mid].Chainage < SearchKey)//在右邊的數列
                    {
                        left = mid + 1;
                    }
                    else//在左邊的數列
                    {
                        right = mid - 1;
                    }
                }

            }
            return -1;//找不到時
		}
		
		
		public void ReadVALDData()
		{
			int size=Marshal.SizeOf(typeof(oldVerticalAlignment));
            FileStream fn = new FileStream(VALDPathName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fn);
            long ElementQuantity = fn.Length / size;
            VALDData = new VerticalAlignment[ElementQuantity];
            int i = 0;
            while (i < ElementQuantity)
            {
                byte[] buffer = br.ReadBytes(size);
                
				char[] Plat = new char[3];	//public string Plat;
		        char[] UpDown = new char[1];	//public string UpDown;
		        char[] Chaninage = new char[15];	
		        char[] Elevation = new char[10];	
		        char[] Grade = new char[10];	
		        char[] KValue = new char[15];	
		        char[] PviElevation = new char[10];	
		        char[] LVC = new char[10];	
		        char[] Mo = new char[10];
		        
	            Array.Copy(buffer, 0, Plat, 0, 3);
	            Array.Copy(buffer, 3, UpDown, 0, 1);
	            Array.Copy(buffer, 4, Chaninage, 0, 15);
	            Array.Copy(buffer, 19, Elevation, 0, 10);
	            Array.Copy(buffer, 29, Grade, 0, 10);
	            Array.Copy(buffer, 39, KValue, 0, 15);
	            Array.Copy(buffer, 54, PviElevation, 0, 10);
	            Array.Copy(buffer, 64, LVC, 0, 10);
	            Array.Copy(buffer, 74, Mo, 0, 10);

	            VALDData[i].Plat= new string(Plat);
	            VALDData[i].UpDown= new string(UpDown);
	            VALDData[i].Chainage= Convert.ToDouble(new string(Chaninage));
	            VALDData[i].Elevation= Convert.ToDouble(new string(Elevation));
	            VALDData[i].Grade= Convert.ToDouble(new string(Grade));
	            VALDData[i].KValue= Convert.ToDouble(new string(KValue));
	            string s=new string(PviElevation).Trim();
	            if ((new string(PviElevation)).Trim()!="")
	            {
		            VALDData[i].PviElevation= Convert.ToDouble(new string(PviElevation));
		            VALDData[i].LVC= Convert.ToDouble(new string(LVC));
		            VALDData[i].Mo= Convert.ToDouble(new string(Mo));
	            }
	            
	            i++;
            }


		}

		public int getSideOfWalkway(double ch1)
		{
			GetHIndex(ch1);
			if(ALDData[HIndex].Text1.Trim() =="") 	return 1;
			return int.Parse(ALDData[HIndex].Text1);
		}
		
		
		public double getTunnelOffset(double ch1, bool SingOn)
		{
			double TheTunnelOffset=0, TunnelOffsetValue=0, NextOffsetValue=0;
			GetHIndex(ch1);
            	switch( ALDData[HIndex].TSC.Substring(1,1)){
				case "T": 
		            TunnelOffsetValue = ALDData[HIndex].Real1;
		            TheTunnelOffset = TunnelOffsetValue;
			    	break;
           		case "S":
			    	if (ALDData[HIndex].TSC == "CS"){
			            TunnelOffsetValue = ALDData[HIndex].Real1;
			            NextOffsetValue = ALDData[HIndex+1].Real1;
			                TheTunnelOffset = NextOffsetValue +
			                	(TunnelOffsetValue - NextOffsetValue) / ALDData[HIndex].Length * (ALDData[HIndex+1].Chainage - ch1);
			    	}
			    	else
			    	{
	            		NextOffsetValue = ALDData[HIndex].Real1;
	            		TunnelOffsetValue = ALDData[HIndex+1].Real1;
			                TheTunnelOffset = NextOffsetValue +
			                    (TunnelOffsetValue - NextOffsetValue) / ALDData[HIndex].Length * (ch1 - ALDData[HIndex].Chainage);
			    	}
     				break;
            case "C":
	            TunnelOffsetValue = ALDData[HIndex].Real1;
	            TheTunnelOffset = TunnelOffsetValue;
    			break;
    		default:
    	    	break;

			}
    		return TheTunnelOffset;
		}

		
		public double getGaugeWidenning(double ch1, bool SingOn)
		{
			double TheGaugeWidenning=0, GaugeWidenningvalue=0;
			GetHIndex(ch1);
            	switch( ALDData[HIndex].TSC.Substring(1,1)){
				case "T": 
			        TheGaugeWidenning = 0;
			        if (HIndex+1 <= ALDData.Count()) {
			        	if ((ALDData[HIndex+1].TSC == "TC") && ((ALDData[HIndex+1].Chainage - ch1) <= 5)
			        	    && ((ALDData[HIndex+1].Chainage - ch1) > 0)){ //TC前5m開始軌距漸變
			                GaugeWidenningvalue = ALDData[HIndex+1].GaugeWidenning;
			               TheGaugeWidenning = 
			                 GaugeWidenningvalue / 5 * (5 - ALDData[HIndex+1].Chainage + ch1);
			        	}
			        }
		            if ((ALDData[HIndex].TSC == "CT") && ((ch1 - ALDData[HIndex].Chainage) <= 5) &&
			            ((ch1 - ALDData[HIndex].Chainage) > 0)) { //TC後5m內軌距漸變
		             	GaugeWidenningvalue = ALDData[HIndex - 1].GaugeWidenning;
		               	TheGaugeWidenning = GaugeWidenningvalue / 5 * (5 + ALDData[HIndex].Chainage - ch1);
		            }
	            	if (SingOn) TheGaugeWidenning = TheGaugeWidenning * LeftRightByAzimuth(ALDData[HIndex].Azimuth, ALDData[HIndex+1].Azimuth);
			    	break;
           		case "S":
			    	if (ALDData[HIndex].TSC == "CS"){
			            GaugeWidenningvalue = ALDData[HIndex - 1].GaugeWidenning;
		            	if (ALDData[HIndex+1].CurveType == "HALFSINE"){
			                TheGaugeWidenning = 
			                    GaugeWidenningvalue / 2 * (1 - Math.Cos((ALDData[HIndex+1].Chainage - ch1) / ALDData[HIndex].Length * Math.PI));
			            }
			            else{
			                TheGaugeWidenning = 
			                    GaugeWidenningvalue / ALDData[HIndex].Length * (ALDData[HIndex+1].Chainage - ch1);
			            }
			            if (SingOn) TheGaugeWidenning = TheGaugeWidenning * LeftRightByAzimuth(ALDData[HIndex].Azimuth, ALDData[HIndex+1].Azimuth);
			    	}
			    	else
			    	{
	            		GaugeWidenningvalue = ALDData[HIndex+1].GaugeWidenning;
	            		if (ALDData[HIndex+1].CurveType == "HALFSINE"){
			                TheGaugeWidenning = 
			                	GaugeWidenningvalue / 2 * (1 - Math.Cos((ch1 - ALDData[HIndex].Chainage) / ALDData[HIndex].Length * Math.PI));
	            		}
	            		else{
			                TheGaugeWidenning = 
			                    GaugeWidenningvalue / ALDData[HIndex].Length * (ch1 - ALDData[HIndex].Chainage);
	            		}
            			if (SingOn) TheGaugeWidenning = TheGaugeWidenning * LeftRightByAzimuth(ALDData[HIndex].Azimuth, ALDData[HIndex+1].Azimuth);
			    	}
     				break;
            case "C":
	            GaugeWidenningvalue = ALDData[HIndex].GaugeWidenning;
	            TheGaugeWidenning = GaugeWidenningvalue;
	            if (SingOn) TheGaugeWidenning = TheGaugeWidenning * LeftRightByAzimuth(ALDData[HIndex].Azimuth, ALDData[HIndex+1].Azimuth);
    			break;
    		default:
    	    	break;

			}
    		return TheGaugeWidenning;
		}

		public double getAppliedCant(double ch1, bool SingOn)
		{
			double TheCant=0, cantvalue=0;
			GetHIndex(ch1);
            	switch( ALDData[HIndex].TSC.Substring(1,1)){
				case "T": //todo: TC點前後或CT點後前的超高漸變方式依規劃手冊可以有三種情況，以下的程式碼尚未完成，後續如確有需要再研擬編寫。
			        TheCant = 0;
			        if (HIndex+1 <= ALDData.Count()) {
			        	if ((ALDData[HIndex+1].TSC == "TC") && ((ALDData[HIndex+1].Chainage - ch1) <= 25)
			        	    && ((ALDData[HIndex+1].Chainage - ch1) > 0)){ //TC前25m開始超高漸變
			                cantvalue = ALDData[HIndex+1].Cant;
			               TheCant = 
			                 cantvalue / 25 * (25 - ALDData[HIndex+1].Chainage + ch1);
			        	}
			        }
		            if ((ALDData[HIndex].TSC == "CT") && ((ch1 - ALDData[HIndex].Chainage) <= 25) &&
			            ((ch1 - ALDData[HIndex].Chainage) > 0)) { //TC後25m內超高漸變
		             	cantvalue = ALDData[HIndex - 1].Cant;
		               	TheCant = cantvalue / 25 * (25 + ALDData[HIndex].Chainage - ch1);
		            }
	            	if (SingOn) TheCant = TheCant * LeftRightByAzimuth(ALDData[HIndex].Azimuth, ALDData[HIndex+1].Azimuth);
			    	break;
           		case "S":
			    	if (ALDData[HIndex].TSC == "CS"){
			            cantvalue = ALDData[HIndex - 1].Cant;
		            	if (ALDData[HIndex+1].CurveType == "HALFSINE"){
			                TheCant = 
			                    cantvalue / 2 * (1 - Math.Cos((ALDData[HIndex+1].Chainage - ch1) / ALDData[HIndex].Length * Math.PI));
			            }
			            else{
			                TheCant = 
			                    cantvalue / ALDData[HIndex].Length * (ALDData[HIndex+1].Chainage - ch1);
			            }
			            if (SingOn) TheCant = TheCant * LeftRightByAzimuth(ALDData[HIndex].Azimuth, ALDData[HIndex+1].Azimuth);
			    	}
			    	else
			    	{
	            		cantvalue = ALDData[HIndex+1].Cant;
	            		if (ALDData[HIndex+1].CurveType == "HALFSINE"){
			                TheCant = 
			                	cantvalue / 2 * (1 - Math.Cos((ch1 - ALDData[HIndex].Chainage) / ALDData[HIndex].Length * Math.PI));
	            		}
	            		else{
			                TheCant = 
			                    cantvalue / ALDData[HIndex].Length * (ch1 - ALDData[HIndex].Chainage);
	            		}
            			if (SingOn) TheCant = TheCant * LeftRightByAzimuth(ALDData[HIndex].Azimuth, ALDData[HIndex+1].Azimuth);
			    	}
     				break;
            case "C":
	            cantvalue = ALDData[HIndex].Cant;
	            TheCant = cantvalue;
	            if (SingOn) TheCant = TheCant * LeftRightByAzimuth(ALDData[HIndex].Azimuth, ALDData[HIndex+1].Azimuth);
    			break;
    		default:
    	    	break;

			}
    		return TheCant;
		}

		
		public void ReadALDData()  //todo: ALD檔案的擴充性不佳，多年來的經驗得知xml格式是首選，未來配合LandXml的格式擴充，除了解決擴充性外，也可與商業軟體進行資料交換。
		{
            int size=Marshal.SizeOf(typeof(OldAlignmentData));
            FileStream fn = new FileStream(ALDPathName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fn);
            long ElementQuantity = fn.Length / size;
            ALDData = new AlignmentData[ElementQuantity];
            int i = 0;
            while (i < ElementQuantity)
            {

                byte[] buffer = br.ReadBytes(size);
                
				char[] Plat = new char[3];	//public string Plat;
		        char[] UpDown = new char[1];	//public string UpDown;
		        char[] TSC = new char[2];	//public string TSC;
	            char[] Easting = new char[16];
	            char[] Northing = new char[17];
		        char[] Chainage = new char[15];	//public double Chainage;
		        char[] ContinuousChainage = new char[15];	//public double ContinuousChainage;
		        char[] Azimuth = new char[13];	//public double Azimuth;
		        char[] Length = new char[15];	//public double Length;
		        char[] RadiusCurveType = new char[8];	//public double RadiusCurveType;
		        char[] CircularCurveNo = new char[9];	//public string CircularCurveNo;
		        //public double Cant;
		        //public double GaugeWidenning;
		        //public double Speedlimit;
		        char[] Text1 = new char[25];	//public string Text1;
		        char[] Text2 = new char[25];	//public string Text2;
		        //public double Real1;
		        //public double Real2;


	            //    ALDData[i] = (AlignmentData)Marshal.PtrToStructure(ptr, typeof(AlignmentData));
	            //string e = new string(ALDData[i].NE.Easting);
	            Array.Copy(buffer, 0, Plat, 0, 3);
	            Array.Copy(buffer, 3, UpDown, 0, 1);
	            Array.Copy(buffer, 4, TSC, 0, 2);
	            Array.Copy(buffer, 6, Easting, 0, 16);
	            Array.Copy(buffer, 22, Northing, 0, 17);
	            Array.Copy(buffer, 39, Chainage, 0, 15);
	            Array.Copy(buffer, 54, ContinuousChainage, 0, 15);
	            Array.Copy(buffer, 69, Azimuth, 0, 13);
	            Array.Copy(buffer, 82, Length, 0, 15);
	            Array.Copy(buffer, 97, RadiusCurveType, 0, 8);
	            Array.Copy(buffer, 105, CircularCurveNo, 0, 9);
	            Array.Copy(buffer, 138, Text1, 0, 25);
	            Array.Copy(buffer, 163, Text2, 0, 25);
	            
	            ALDData[i].Plat = new string(Plat);
	            ALDData[i].UpDown = new string(UpDown);
	            ALDData[i].TSC = new string(TSC);
	            ALDData[i].Easting= Convert.ToDouble(new string(Easting));
	            ALDData[i].Northing = Convert.ToDouble(new string(Northing));
	            ALDData[i].Chainage = Convert.ToDouble(new string(Chainage));
	            if ((new string(ContinuousChainage)).Trim() == "")
	            {
	            	ALDData[i].ContinuousChainage = 0;
	            }
	            else
	            {
	            	ALDData[i].ContinuousChainage = Convert.ToDouble(new string(ContinuousChainage));	            	
	            }
	            TrackAlignments.Azimuth Azi= new Azimuth(new string(Azimuth));
	            ALDData[i].Azimuth = Azi.ToRadians();
	            if (i < ElementQuantity-1)
	            {
		            ALDData[i].Length = Convert.ToDouble(new string(Length));
		            if (TSC[1] == 'C')
		            {
		            	ALDData[i].Radius = Convert.ToDouble(new string(RadiusCurveType));
		            	ALDData[i].CurveType = "ARC";
		            }
		            else
		            	ALDData[i].CurveType = new string(RadiusCurveType).Trim();
		            
		            ALDData[i].Cant = BitConverter.ToDouble(buffer.Skip(114).Take(8).ToArray(),0);
		            ALDData[i].GaugeWidenning = BitConverter.ToDouble(buffer.Skip(122).Take(8).ToArray(),0);
		            ALDData[i].Speedlimit = BitConverter.ToDouble(buffer.Skip(130).Take(8).ToArray(),0);
		            ALDData[i].Text1 = new string(Text1);
		            ALDData[i].Text2 = new string(Text2);
		            ALDData[i].Real1 = BitConverter.ToDouble(buffer.Skip(188).Take(8).ToArray(),0);
		            ALDData[i].Real2 = BitConverter.ToDouble(buffer.Skip(196).Take(8).ToArray(),0);
	            }
	            i++;
            }
            
           //ToDo: Assign the sign(Left/Right) of radius
           for (i = 0; i < ALDData.Count()-1; i++)
			{
				if (ALDData[i].TSC.Substring(1,1) == "C")
				{
					ALDData[i].Radius = ALDData[i].Radius * LeftRightByAzimuth(ALDData[i].Azimuth, ALDData[i+1].Azimuth);
				}
			}
		}
		
		public void ReadDataFromLandXml()
		{
            XmlTextReader reader = new XmlTextReader("D:\\Git\\XMLTest\\LandXMLTest\\jan96down.xml");
            reader.Namespaces = false;
            XPathDocument document = new XPathDocument(reader);
            XPathNavigator navigator = document.CreateNavigator();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(reader.NameTable);
            nsmgr.AddNamespace("ns", "http://www.landxml.org/schema/LandXML-1.1");
            //Read horizontal alignment
            XPathNodeIterator nodes = navigator.Select("//LandXML/Alignments/Alignment/CoordGeom/*", nsmgr);
            string dir = "";
            string preTSC = "T";
            string StartPoint = "";
            string [] XYZ;
            long ElementQuantity = nodes.Count;
            ALDData = new AlignmentData[ElementQuantity];
            for (int i = 0; i < nodes.Count; i++)
            {
             	nodes.MoveNext();
				if (nodes.Current.Name.Equals("Line")) dir = "dir"; else dir = "dirStart";
				switch (nodes.Current.Name) {
					case "Line":
						ALDData[i].TSC = preTSC + "T";
						preTSC = "T";
		            	ALDData[i].CurveType = "STRAIGHT";
							
						break;
					case "Spiral":
						ALDData[i].TSC = preTSC + "S";
						preTSC = "S";
						switch (nodes.Current.GetAttribute("spiType", nodes.Current.GetNamespace("ns"))) {
							case "clothoid":
							ALDData[i].CurveType = "SPIRAL";	
								break;
							default:
								
								break;
						}
						break;
					case "Curve":
						ALDData[i].TSC = preTSC + "C";
						preTSC = "C";
		            	ALDData[i].Radius = Convert.ToDouble(nodes.Current.GetAttribute("radius", nodes.Current.GetNamespace("ns")));
		            	ALDData[i].CurveType = "ARC";
						
						break;
					default:
						
						break;
				}
				
				StartPoint = nodes.Current.GetChildNodeValue("Start").Trim();
				XYZ = StartPoint.Split(new Char [] {' '});
				
				ALDData[i].Easting= Convert.ToDouble(XYZ[0]);
				ALDData[i].Northing = Convert.ToDouble(XYZ[1]);				
				ALDData[i].Chainage = Convert.ToDouble(nodes.Current.GetAttribute("staStart", nodes.Current.GetNamespace("ns")));
				ALDData[i].ContinuousChainage = Convert.ToDouble(nodes.Current.GetAttribute("ContinuousChainage", nodes.Current.GetNamespace("ns")));
				ALDData[i].Azimuth = Convert.ToDouble(nodes.Current.GetAttribute(dir, nodes.Current.GetNamespace("ns")));
	            ALDData[i].Length = Convert.ToDouble(nodes.Current.GetAttribute("length", nodes.Current.GetNamespace("ns")));

            }
            
            //Assign the sign(Left/Right) of radius
			for (int i = 0; i < ALDData.Count()-1; i++)
			{
				if (ALDData[i].TSC.Substring(1,1) == "C")
				{
					ALDData[i].Radius = ALDData[i].Radius * LeftRightByAzimuth(ALDData[i].Azimuth, ALDData[i+1].Azimuth);
				}
			}
           	//--------------------
            // Get the Vertical alignment data
            XPathNodeIterator PVInodes = navigator.Select("//LandXML/Alignments/Alignment/Profile/ProfAlign/*", nsmgr);
            ElementQuantity = PVInodes.Count;
            PVIData[] PVIDataSet = new PVIData[ElementQuantity];
            string PVI = "";
            for (int i = 0; i < PVInodes.Count; i++)
            {
                PVInodes.MoveNext();
                if (PVInodes.Current.Name == "ParaCurve")
                    PVIDataSet[i].Length =  Convert.ToDouble(PVInodes.Current.GetAttribute("length", nodes.Current.GetNamespace("ns")));
                PVI = PVInodes.Current.Value.Trim();
                XYZ = PVI.Split(new Char[] { ' ' });
                PVIDataSet[i].PVIName = PVInodes.Current.Name;
                PVIDataSet[i].Chainage =  Convert.ToDouble(XYZ[0]);
                PVIDataSet[i].Elevation =  Convert.ToDouble(XYZ[1]);
            }
            
            ElementQuantity = PVInodes.Count*3 - 4;
            VALDData = new VerticalAlignment[ElementQuantity];
            
        	int j = 0;
			VALDData[j].Chainage =  PVIDataSet[j].Chainage;
			VALDData[j].Elevation =  PVIDataSet[j].Elevation;
			VALDData[j].Grade =  (PVIDataSet[j+1].Elevation - PVIDataSet[j].Elevation)/(PVIDataSet[j+1].Chainage - PVIDataSet[j].Chainage) * 100.0;
        	j+=2;
            for (int i = 1; i < PVInodes.Count - 1; i++)
            {
            	double c1 = PVIDataSet[i-1].Chainage;
            	double c2 = PVIDataSet[i].Chainage;
            	double c3 = PVIDataSet[i+1].Chainage;
            	double e1 = PVIDataSet[i-1].Elevation;
            	double e2 = PVIDataSet[i].Elevation;
            	double e3 = PVIDataSet[i+1].Elevation;
            	double VCL = PVIDataSet[i].Length;
            	double g1 = (e2-e1)/(c2-c1)*100;
            	double g2 = (e3-e2)/(c3-c2)*100;
            	double R = VCL/(g2-g1);
            	
            	
            	double c4 = c2 - VCL / 2;
			    double c5 = c2 + VCL / 2;
			    double e4 = TVL(c1, g1, e1, c4);
			    double e5 = TVL(c2, g2, e2, c5);
			    double mve;
			    if (R == 0)
			        mve = e2;
			    else
			        mve = SYVL(c4, g1, g2, e2, VCL, c2);
			    double mo = mve - e2;

     			VALDData[j-1].Chainage = c4;
    			VALDData[j-1].Elevation = e4;
    			VALDData[j-1].Grade = g1;
    			VALDData[j-1].KValue = R;
    			
    			VALDData[j].Chainage =  c2;
    			VALDData[j].Elevation =  mve;
    			VALDData[j].Grade =  (g1+g2)/2;
    			VALDData[j].KValue = R;
    			VALDData[j].PviElevation = e2;
    			VALDData[j].LVC = VCL;
    			VALDData[j].Mo =  mo;
    			
    			VALDData[j+1].Chainage =  c5;
    			VALDData[j+1].Elevation =  e5;
    			VALDData[j+1].Grade =  g2;
    			VALDData[j-1].KValue = 0;
    			j+=3;
            }
            j -= 1;            
			VALDData[j].Chainage =  PVIDataSet[PVInodes.Count - 1].Chainage;
			VALDData[j].Elevation =  PVIDataSet[PVInodes.Count - 1].Elevation;
			VALDData[j].Grade =  (PVIDataSet[PVInodes.Count - 1].Elevation - PVIDataSet[PVInodes.Count - 1-1].Elevation)/
											(PVIDataSet[PVInodes.Count - 1].Chainage - PVIDataSet[PVInodes.Count - 1-1].Chainage) * 100.0;

            
		}

		private int LeftRightByAzimuth(double Ang1, double Ang2)
		{
			double UX = Math.Sin(Ang1);
			double UY = Math.Cos(Ang1);
			double VX = Math.Sin(Ang2);
			double VY = Math.Cos(Ang2);
			return  -Math.Sign(UX * VY - UY * VX);
		}
    } //class trackcenterline
    
    public static class XPathNavigatorExtensions
    {
        public static string GetChildNodeValue(this XPathNavigator navigator, string nodePath)
        {
            XPathNavigator nav = navigator.SelectSingleNode(nodePath);
            return nav == null ? string.Empty : nav.Value;
        }
    }
	
	public enum AngleUnit
	{
		Radians=0,
		Degrees,
	}
	
	public class Azimuth
	{
		public double Value=0; //in Radians
		public Azimuth(string Azi)
		{
			Value = Convert.ToDouble(Azi.Substring(0, 3))/ 1.0 + 
				Convert.ToDouble(Azi.Substring(4, 2)) / 60.0 + 
				Convert.ToDouble(Azi.Substring(7, 6)) / 3600.0;
			Value = Value*Math.PI/180.0;
		}
		
		public Azimuth(double Azi, AngleUnit Unit)
		{
			switch (Unit) {
				case AngleUnit.Radians:
					Value = Azi;
					break;
				case AngleUnit.Degrees:
					Value = Value*Math.PI/180.0;
					break;
				default:
					
					break;
			}
			
		}
		
		public double ToRadians()
		{
			return Value;
		}
		
		public double ToDegrees()
		{
			return Value/Math.PI*180.0;
		}
		
		public double ToPolar() //in Radians
		{
			if (Math.PI * 0.5 - Value > 0)
				return Math.PI * 0.5 - Value;
			else 
				return Math.PI * 2.5 - Value;
		}
	}
	
	
    internal unsafe struct Coordinates
    {
        public fixed byte Easting[16];
        public fixed byte Northing[17];
    }


    internal unsafe struct OldAlignmentData
    {
        public fixed byte Plat[3];
        public fixed byte UpDown[1];
        public fixed byte TSC[2];
        public fixed byte NE[33]; //Coordinates
        public fixed byte Chainage[15];
        public fixed byte ContinuousChainage[15];
        public fixed byte Azimuth[13];
        public fixed byte Length[15];
        public fixed byte RadiusCurveType[8];
        public fixed byte CircularCurveNo[9];
        public fixed byte Cant[8];
        public fixed byte GaugeWidenning[8];
        public fixed byte Speedlimit[8];
        public fixed byte Text1[25];
        public fixed byte Text2[25];
        public fixed byte Real1[8];
        public fixed byte Real2[8];
    }

    public struct AlignmentData
    {
        public string Plat;
        public string UpDown;
        public string TSC;
        public double Easting;
        public double Northing;
        public double Chainage;
        public double ContinuousChainage;
        public double Azimuth;   //in Radians
        public double Length;
        public double Radius;
        public string CurveType;
        public string CircularCurveNo;
        public double Cant;
        public double GaugeWidenning;
        public double Speedlimit;
        public string Text1;	//Text1 = Side of walkway, Right=1, Left=-1
        public string Text2;
        public double Real1; 	//Real1 = Offset of tunnel 'H'
        public double Real2;  
    }

    public struct VerticalAlignment
    {
        public string Plat;
        public string UpDown;
        public double Chainage;
        public double Elevation;
        public double Grade;   //in Radians
        public double KValue;
        public double PviElevation; //eli
        public double LVC;
        public double Mo;
    }

    public struct PVIData
    {
        public string PVIName;
        public double Chainage;
        public double Elevation;
        public double Length;
    }
    public unsafe struct oldVerticalAlignment
    {
    	public fixed byte Plat[3];
    	public fixed byte UpDown[1];
    	public fixed byte Chainage[15];
    	public fixed byte Elevation[10];
    	public fixed byte Grade[10];   //in Radians
    	public fixed byte KValue[15];
    	public fixed byte PviElevation[10]; //eli
    	public fixed byte LVC[10];
    	public fixed byte Mo[10];
    }

	

} //namespace
