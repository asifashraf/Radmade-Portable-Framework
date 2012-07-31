using System.Threading;

    public static class ThreadX
    {
		#region Methods (1) 

		// Public Methods (1) 

        public static void CallParameterizedThread(object state, ParameterizedThreadStart method)
        {
            Thread thread = new Thread(method);
            thread.Start(state);
        }

		#endregion Methods 
    }

