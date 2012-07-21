using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


   public static class FormX
    {
		#region Methods (2) 

		// Public Methods (2) 

       public static void IncludeFileInputAttribute(this HtmlForm instance)
       {
           instance.Attributes.Add("enctype", "multipart/form-data");
       }

       public static void ResetControls(this HtmlForm form, params Control[] controls)
       {
           foreach (Control c in controls)
           {
               if (c.GetType() == typeof(TextBox))
               {
                   ((TextBox)c).Text = string.Empty;
               }

               if (c.GetType() == typeof(DropDownList))
               {
                   ((DropDownList)c).SelectedIndex = 0;
               }
               if (c.GetType() == typeof(CheckBox))
               {
                   ((CheckBox)c).Checked = false;
               }
           }
           
       }

		#endregion Methods 
    }

