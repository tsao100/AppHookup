/*
 * Created by SharpDevelop.
 * User: Joshua.Lumley
 * Date: 8/09/2017
 * Time: 11:45 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace myModule
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class Form1 : System.Windows.Forms.Form
	{
	    ButtonEE7Parameter myEE7Parameter;
        ExternalEvent myEE7ActionParameter;	
	public Document doc { get; set; }
		
            linePatternsWeightsFalse mylinePatternsWeightsFalse;
            ExternalEvent myMakeLinePatterns;
            
           linePatternsWeightsTrue mylinePatternsWeightsTrue;
            ExternalEvent myMakeLineWeights;
	public Form1()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			myEE7Parameter = new ButtonEE7Parameter();
            myEE7ActionParameter = ExternalEvent.Create(myEE7Parameter);
			mylinePatternsWeightsFalse = new linePatternsWeightsFalse();
			myMakeLinePatterns = ExternalEvent.Create(mylinePatternsWeightsFalse);
			
			mylinePatternsWeightsTrue = new linePatternsWeightsTrue();
			myMakeLineWeights = ExternalEvent.Create(mylinePatternsWeightsTrue);
			//
		}
		

		
		void Button1Click(object sender, EventArgs e)
		{
    		myMakeLinePatterns.Raise();			
		}
		
		void Button2Click(object sender, EventArgs e)
		{
    		myMakeLineWeights.Raise();			
		}
		
		void Button3Click(object sender, EventArgs e)
		{
   			//throw new InvalidOperationException(); 	

	   		try
            {
   				//throw new InvalidOperationException();  
//		      	using (Transaction t = new Transaction(doc, "Set a parameters"))
//		              {
//		                  t.Start();
//		                  doc.ProjectInformation.GetParameters("Project Name")[0].Set("Space Elevator");  //this needs to change in two places
//		                   t.Commit();
//		               }
		      	
		      	
				myEE7ActionParameter.Raise();  				
            }

            #region catch and finally
            catch (Exception ex)
            {
                TaskDialog.Show("Catch", "Failed due to:" + Environment.NewLine + ex.Message);
            }
            finally
            {
          
            }
            #endregion			
		}
	}
	
	
public class linePatternsWeightsTrue : IExternalEventHandler
    {

        public void Execute(UIApplication a)
        {
        	
        	UIDocument uidoc =  a.ActiveUIDocument;
        	Document doc = uidoc.Document;
        	
	Transaction transaction = new Transaction( doc);
	transaction.Start( "Draw Line Patterns or Weights" );	
        	
        	DrawLines myThis = new DrawLines();
        	
        	myThis._99_DrawLinePatterns(true, false, uidoc);
    		transaction.Commit();
        	return;
        	
        }
        
       public string GetName()
        {
            return "External Event Example";
        }
	    
	    
	}
	    
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("23CF5F71-5468-438D-97C7-554F4F782936")]	
	
	    public class linePatternsWeightsFalse : IExternalEventHandler
    {
    	
        public void Execute(UIApplication a)
        {
        	
        	UIDocument uidoc =  a.ActiveUIDocument;
        	Document doc = uidoc.Document;
        	
	Transaction transaction = new Transaction( doc);
	transaction.Start( "Draw Line Patterns or Weights" );	
        	
        	DrawLines myThis = new DrawLines();
      	
        	myThis._99_DrawLinePatterns(false, false, uidoc);

    		transaction.Commit();
        	return;
        }
        
        
   public string GetName()
        {
            return "External Event Example";
        }
	    
	    
	}
}
