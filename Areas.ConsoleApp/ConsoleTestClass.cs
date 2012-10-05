using System;

namespace Areas.ConsoleApp
{
    using System.Configuration;

    public abstract class ConsoleTestClass
     {
         /// <summary>
         /// The entry point of the testing
         /// </summary>
         public abstract void InitTest();

         protected virtual  string ConnectionString
         {
             get
             {
                 return ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
             }
         }

         protected void Write(object textObject)
         {
             Console.Write(textObject);
         }

         protected void Write(string format, params object[] parameters)
         {
             Console.Write(format, parameters);
         }

        /// <summary>
        /// Equal to WriteLine
        /// </summary>
        /// <param name="textObject">Object to print</param>
         protected void Print(object textObject)
         {
             this.AddCurrentTime();
             Console.WriteLine(textObject);
         }

        /// <summary>
        /// Equal to Writeline
        /// </summary>
        /// <param name="format">A string containing chained parameters such as a string passed in string.Format</param>
        /// <param name="parameters">Given parameters for the chained parameters used in format parameter</param>
         protected void Print(string format, params object[] parameters)
         {
             this.AddCurrentTime();
             Console.WriteLine(format, parameters);
         }

         /// <summary>
         /// Start an operation
         /// </summary>
         /// <param name="textObject">Object to print</param>
         protected void Start(object textObject)
         {
             Console.WriteLine("============================");
             Console.WriteLine(string.Format(">>({0})>>{1} (Start)", CurrentTime, textObject));
         }

         /// <summary>
         /// End an operation
         /// </summary>
         /// <param name="textObject">Object to print</param>
         protected void End(object textObject, DateTime? startTime = null)
         {
             if(startTime.IsNull())
             {
                 Console.WriteLine(string.Format(">>({0})>>{1} (End)", CurrentTime, textObject));    
             }
             else
             {
                 var span = DateTime.Now - startTime.Value;
                 Console.WriteLine(string.Format(">>({0})>>{1} (End) Time consumed: {2} seconds", CurrentTime, textObject, span.TotalSeconds));
             }
             
             Console.WriteLine("---------------------------------------------------");
         }

        protected void Step(Action method, string message)
        {
            var startTime = DateTime.Now;
            this.Start(message);
            method();
            this.End(message, startTime);
        }

         public void Read()
         {
             Print("Waiting for user input");
             Console.Read();
         }

        private string CurrentTime
        {
            get
            {
                return this.FormatTime(DateTime.Now);
            }
        }

        private string FormatTime(DateTime toFormat)
        {
            return string.Format("{0}:{1}:{2}:{3}",toFormat.Hour, toFormat.Minute, toFormat.Second, toFormat.Millisecond);
        }

        private void AddCurrentTime()
        {
            Write(string.Format(" ({0})", CurrentTime));
        }
     }
}
