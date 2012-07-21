using System;

public static class ExceptionX
    {
		#region Properties (1) 

        public static Exception Instance
        {
            get
            {
                return new Exception();
            }
        }

		#endregion Properties 
#region Methods (1) 

		// Public Methods (1) 

        public static void Throw(this Exception ex, string message, params string[] parameters)
        {
            throw new Exception(string.Format(message, parameters));
        }

		#endregion Methods 
		
    }
