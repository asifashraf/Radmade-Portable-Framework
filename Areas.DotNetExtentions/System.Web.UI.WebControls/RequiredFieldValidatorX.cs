using System.Web.UI.WebControls;
using System.Web.UI;


    public static class RequiredFieldValidatorX
    {
		#region Methods (1) 

		// Public Methods (1) 

        public static void Start(
           this RequiredFieldValidator r,
            string text, string error, Control controlToValidate
            )
        {           
                
                r.Text= text;
                r.ErrorMessage = error;
                r.ControlToValidate = controlToValidate.ID;
               
           
        }

		#endregion Methods 
    }

