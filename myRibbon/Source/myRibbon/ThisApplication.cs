/*
 * Created by SharpDevelop.
 * User: Jack
 * Date: 2018/6/3
 * Time: 下午 08:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;

namespace myRibbon
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("0FDCB981-38DD-4D72-9296-FDBA0CAB7217")]
	public partial class ThisApplication : IExternalApplication
	{
				
        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication a)
        {
	
            string ChecklistsNumber = "myRibbonButton";
            string path = Assembly.GetExecutingAssembly().Location; 
                                                              

            String exeConfigPath = Path.GetDirectoryName(path) + "\\myRibbon.dll";
            a.CreateRibbonTab(ChecklistsNumber);
            RibbonPanel PRLChecklistsPanel = a.CreateRibbonPanel(ChecklistsNumber, ChecklistsNumber);
            PushButtonData myPushButtonData = new PushButtonData(ChecklistsNumber, ChecklistsNumber, exeConfigPath, "myRibbon.Invoke");

         myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\011 myButtonImage paste into Addin.png"), UriKind.Absolute));

         
            RibbonItem myRibbonItem = PRLChecklistsPanel.AddItem(myPushButtonData);

            return Result.Succeeded;

        }				

		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}

		#region Revit Macros generated code
		private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
	}
			
	    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]	

       public class Invoke : IExternalCommand
    {
        //public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            //String exeConfigPath = Path.GetDirectoryName(path) + "..\\..\\..\\" + "myMacros\\2018\\Revit\\AppHookup\\_969_PRLChecklists\\AddIn\\_969_PRLChecklists.dll";
            String exeConfigPath = Path.GetDirectoryName(path) + "..\\..\\..\\" + "myModule\\AddIn\\myModule.dll";
            String exeConfigPath2 = Path.GetDirectoryName(path) + "..\\..\\..\\" + "myModule\\AddIn";

            string strCommandName = "ThisApplication";

            byte[] assemblyBytes = File.ReadAllBytes(exeConfigPath);

            Assembly objAssembly = Assembly.Load(assemblyBytes);
            IEnumerable<Type> myIEnumerableType = GetTypesSafely(objAssembly);
            foreach (Type objType in myIEnumerableType)
            {
                if (objType.IsClass)
                {
                    if (objType.Name.ToLower() == strCommandName.ToLower())
                    {
                        object ibaseObject = Activator.CreateInstance(objType);
                        object[] arguments = new object[] { commandData, exeConfigPath2, elements };
                        object result = null;

                        result = objType.InvokeMember("Execute", BindingFlags.Default | BindingFlags.InvokeMethod, null, ibaseObject, arguments);

                        break; 
                    }
                }
            }
            return Result.Succeeded;     

        }
        private static IEnumerable<Type> GetTypesSafely(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(x => x != null);
            }
        }
       }
	
}