/*
 * 由SharpDevelop创建。
 * 用户： SY.design
 * 日期: 2019/3/8
 * 时间: 下午 05:43
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using Microsoft.Win32;
using System.Windows;

namespace TrackTools
{
	/// <summary>
	/// Description of Ini.
	/// </summary>
	public class Ini
	{
		static public string ReadRegistryKey(string RegKey, string Key)
		{
		     //讀取Registry Key位置
		     //Software\VB and VBA Program Settings\AlignmentTools\AppData
		      //RegistryKey RegK = Registry.LocalMachine.OpenSubKey(RegKey);     
		      RegistryKey RegK = Registry.CurrentUser.OpenSubKey(RegKey);
		      //讀取Registry Key String"test"裡面的值
		      string RegT = (string)RegK.GetValue(Key);
		      //Show Registry Key值，檢查讀取的值是否正確
		      MessageBox.Show(RegT);      
		      return RegT;
		}
	}
}
