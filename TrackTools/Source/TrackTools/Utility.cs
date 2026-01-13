/*
 * 由SharpDevelop创建。
 * 用户： Jack
 * 日期: 2019/8/13
 * 时间: 下午 05:44
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Linq;

namespace TrackTools
{
	/// <summary>
	/// Description of Utility.
	/// </summary>
	public class Utility
	{
		public Utility()
		{
		}
		
		/// <summary>
		/// Retrieve a database element 
		/// of the given type and name.
		/// </summary>
		public static Element FindElementByName(
		Document doc,
		Type targetType,
		string targetName )
		{
		return new FilteredElementCollector( doc )
		  .OfClass( targetType )
		  .FirstOrDefault<Element>(
		    e => e.Name.Equals( targetName ) );
		}
		
	    public static IEnumerable<FamilyInstance>
		    GetFamilyInstancesByFamilyAndType(
		      Document doc,
		      string familyName,
		      string typeName )
		{
			return new FilteredElementCollector( doc )
			  .OfClass( typeof( FamilyInstance ) )
			  .Cast<FamilyInstance>()
			  .Where( x => x.Symbol.Family.Name.Equals( familyName ) ) // family
			  .Where( x => x.Name.Equals( typeName ) ); // family type               
		}
	    
	    public static IEnumerable<FamilyInstance>
		    GetFamilyInstancesByFamilyAndType(
		      Document doc,
		      string typeName )
		{
			return new FilteredElementCollector( doc )
			  .OfClass( typeof( FamilyInstance ) )
			  .Cast<FamilyInstance>()
			  .Where( x => x.Name.Equals( typeName ) ); // family type               
		}
	    
	    public static IEnumerable<FamilySymbol>
		    GetFamilySymbolByType(
		      Document doc,
		      string typeName )
		{
			return new FilteredElementCollector( doc )
			  .OfClass( typeof( FamilySymbol ) )
			  .Cast<FamilySymbol>()
			  .Where( x => x.Name.Equals( typeName ) ); // family type               
		}
	    
	    public static IEnumerable<Form>
		    GetForm(
		      Document doc)
		{
			return new FilteredElementCollector( doc )
			  .OfClass( typeof( Form) )
			  .Cast<Form>()
				; // Reference
		}
	    
	    public static IEnumerable<ReferencePoint>
		    GetReferencePoint(
		      Document doc,
		      int i )
		{
			return new FilteredElementCollector( doc )
				.OfCategory(BuiltInCategory.OST_AdaptivePoints)
			  .OfClass( typeof( ReferencePoint ) )
			  .Cast<ReferencePoint>()
				.Where( x => x.GetParameters("數目").First().AsInteger() == i ); // ReferencePoint
		}

	    public static int
		    GetReferencePointQuantity(
		      Document doc)
		{
			return new FilteredElementCollector( doc )
				.OfCategory(BuiltInCategory.OST_AdaptivePoints)
			  .OfClass( typeof( ReferencePoint ) )
			  .Cast<ReferencePoint>()
				.Count(); // ReferencePoint
		}
	    
	}
}
