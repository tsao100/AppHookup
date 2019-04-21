/*
 * 由SharpDevelop创建。
 * 用户： SY.design
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

namespace TrackTools.TrackAlignments
{
	/// <summary>
	/// Description of TrackAlignment.
	/// </summary>
	public class TrackCenterLine
	{
		int currentHIndex;
		int currentVIndex;
		string ALDPathName;
		string VALDPathName;
		AlignmentData[] ALDData;
		VerticalAlignment[] VALDData;
		public TrackCenterLine()
		{

			ALDPathName=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\DataTable\\"+
				Interaction.GetSetting("AlignmentTools","AppData","ALDName1","Y06UH.ALD")+"";
			
			VALDPathName=ALDPathName.Replace("H.ALD","V.ALD");
			
			
			ReadALDData();
			ReadVALDData();
		}
		
		public TrackCenterLine(string ALDNameNo)
		{

			ALDPathName=Interaction.GetSetting("AlignmentTools","AppData","Path","D:")+
				"\\AlignmentTools\\DataTable\\"+
				Interaction.GetSetting("AlignmentTools","AppData",ALDNameNo,"Y06UH.ALD")+"";
			
			VALDPathName=ALDPathName.Replace("H.ALD","V.ALD");
			
			
			ReadALDData();
			ReadVALDData();
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
			for(int i=0; i < ALDData.Count();i++){
				if (p>=ALDData[i].Chainage)
				{
					currentHIndex = i;
					currentVIndex = i;
					break;
				}
			}
			
			double[] XYZ=new Double[3];
			
			return XYZ;
		}
		
		
		public void ReadVALDData()
		{
			int size=Marshal.SizeOf(typeof(oldVerticalAlignment));
            FileStream fn = new FileStream(VALDPathName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fn);
            long ElementQuantity = fn.Length / size;
            VALDData = new VerticalAlignment[ElementQuantity];
            int i = 0;
            while ((fn.Position + size) < fn.Length)
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
		public void ReadALDData()
		{
            int size=Marshal.SizeOf(typeof(OldAlignmentData));
            FileStream fn = new FileStream(ALDPathName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fn);
            long ElementQuantity = fn.Length / size;
            ALDData = new AlignmentData[ElementQuantity];
            int i = 0;
            while ((fn.Position + size) < fn.Length)
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
	            ALDData[i].ContinuousChainage = Convert.ToDouble(new string(ContinuousChainage));
	            TrackAlignments.Azimuth Azi= new Azimuth(new string(Azimuth));
	            ALDData[i].Azimuth = Azi.ToRadians();
	            ALDData[i].Length = Convert.ToDouble(new string(Length));
	            if (TSC[1] == 'C')
	            {
	            	ALDData[i].Radius = Convert.ToDouble(new string(RadiusCurveType));
	            	ALDData[i].CurveType = "ARC";
	            }
	            else
	            	ALDData[i].CurveType = new string(RadiusCurveType);
	            
	            ALDData[i].Cant = BitConverter.ToDouble(buffer.Skip(114).Take(8).ToArray(),0);
	            ALDData[i].GaugeWidenning = BitConverter.ToDouble(buffer.Skip(122).Take(8).ToArray(),0);
	            ALDData[i].Speedlimit = BitConverter.ToDouble(buffer.Skip(130).Take(8).ToArray(),0);
	            ALDData[i].Text1 = new string(Text1);
	            ALDData[i].Text2 = new string(Text2);
	            ALDData[i].Real1 = BitConverter.ToDouble(buffer.Skip(138).Take(8).ToArray(),0);
	            ALDData[i].Real2 = BitConverter.ToDouble(buffer.Skip(163).Take(8).ToArray(),0);
	
	            i++;
            }
		}
		
		
		
		
	
	
//		[StructLayout(LayoutKind.Sequential)]
//	    public struct Data
//	    {
//	        [MarshalAs(UnmanagedType.I8)]
//	        public Int64 timestamp;
//	        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 600 * 16)]
//	        public byte[,] dataBlock1;
//	        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 600 * 16)]
//	        public byte[,] dataBlock2;
//	    }
	    
//	    public void ReadALDData()
//	    {
//	    	
//	        int size = Marshal.SizeOf(typeof(AlignmentData));
//			
//	        Stream iStream = File.OpenRead(ALDPathName);
//	        BinaryReader reader = new BinaryReader(iStream);
//			ALDData = new AlignmentData[iStream.Length/size];
//			int i=0;
//	        while ((iStream.Position + size) < iStream.Length)
//	        {
//	
//	            IntPtr ptr = Marshal.AllocHGlobal(size);
//	
//	            byte[] buffer = reader.ReadBytes(size);
//	
//	            Marshal.Copy(buffer, 0, ptr, size);
//	
//	            ALDData[i] = (AlignmentData)Marshal.PtrToStructure(ptr, typeof(AlignmentData));
//				i++;
//	        }
//	
//	    }//main()
    } //class trackcenterline
    
//	public struct Coordinates
//	{
//		[VBFixedString(16)]string Easting;
//		[VBFixedString(17)]string Northing;		
//	}
	
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
			if (Math.PI * 0.5 - Value < 0)
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
        public string Text1;
        public string Text2;
        public double Real1;
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
