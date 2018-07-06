/*
 * Created by SharpDevelop.
 * User: Jack
 * Date: 2018/6/3
 * Time: 下午 08:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace myModule
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("1B6928FB-91FE-4FDD-8490-9D3941B60791")]
    public partial class ThisApplication : IExternalCommand
	{
				
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            myMacro(commandData.Application.ActiveUIDocument);

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
		public void myMacro(UIDocument uidoc)
		{
			TaskDialog mainDialog = new TaskDialog("Fun Secrets of Revit Coding!");
            
            
//            mainDialog.MainInstruction = "Secret Code in Revit API !";
//            mainDialog.MainContent = "Do you want to be an awesome, all powerful, all knowing Revit API coder?";
//            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1,"Yes, I do - show me the way!");
//            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "No, I'm in a comfortable vegetative state.");
            
//
//           mainDialog.MainInstruction = "You just iterated!";            
//            mainDialog.MainContent = "Did you go build-to-test in under 2 seconds?";
//            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1,"Yes, that was under two seconds!");
//            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "No, that was more than two seconds.");


            mainDialog.MainInstruction = "Lets do something";            
            mainDialog.MainContent = "Do you want to draw line patterns or line weights?";
            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1,"Line Patterns");
            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Line Weights");

            mainDialog.CommonButtons = TaskDialogCommonButtons.Close;
            mainDialog.DefaultButton = TaskDialogResult.Close;


            Form1 myForm1 = new Form1();
            myForm1.doc = uidoc.Document;
            myForm1.Show();
            
            return;
       
            
            TaskDialogResult tResult = mainDialog.Show();
                

            bool Yes = true;

            if (TaskDialogResult.CommandLink1 == tResult)
            {
                Yes = true;

            }else if (TaskDialogResult.CommandLink2 == tResult)
            {
                Yes = false;
            }
            else
            {
                return;
            }
            
//            if(Yes)TaskDialog.Show("TaskDialogue", "You are cool.");
//            if(!Yes)TaskDialog.Show("TaskDialogue", "Ignorance is bliss.");
//            if(Yes)TaskDialog.Show("TaskDialogue", "Well done.");
//            if(!Yes)TaskDialog.Show("TaskDialogue", "Practice makes perfect.");
            
        Transaction transaction = new Transaction( uidoc.Document);
     	transaction.Start( "Draw Line Patterns or Weights" );	
        	
        	DrawLines myThis = new DrawLines();
        	
         if(Yes)	myThis._99_DrawLinePatterns(true, false, uidoc);
         if(!Yes)	myThis._99_DrawLinePatterns(false, false, uidoc);
        	
  	  transaction.Commit();    

		//use relative paths there is no need to figure out a directly structure, you can leave development modules exactly where you revit made them (there is no need to move them with relative paths)
		//there is no need to restart revit to install an addin, ther is no need to find a special directly for addin as opposed to macros (they are the same thing)
		//use relative path in the addin and there is no need to ever move it
		//use InvokeMember to run rapid iteration, because everytime you want to use the btton you have to restart revit which is annoying expective if you have a massive project open, because previously that is a secrete becaues usually a restart of revit is required if a assembly changed
		//any change to the revit db requires transactions
		
		//then i am going to sequence you through the normal range or error messages which revit throws up, and how to deal with them
		
		
		//this isn't project specfic to but always for rapid iteration /* */
		//IExternalCommand modess less commands need to be external evetns

		//put crashes inside try catch statements to avoid crashes, i will demonstartate that now
         //use addin file
        //keep actions commit stateents
		}
	}
}