using System.Diagnostics;
using System.IO;

    public static class ProcessStartInfoX
    {
		#region Methods (1) 

		// Public Methods (1) 

        /// <summary>
        /// Runs an executable and returns the output string
        /// </summary>
        /// <param name="instance">The Type bearing the extension method</param>
        /// <param name="filePath">Executable path</param>
        /// <returns>string ourput</returns>
        public static string RunProcess(this ProcessStartInfo instance,
        string filePath)
        {
            ProcessStartInfo psi = new ProcessStartInfo(filePath);
            psi.RedirectStandardOutput = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            Process proc = System.Diagnostics.Process.Start(psi);
            StreamReader myOutput = proc.StandardOutput;
            proc.WaitForExit();
            string output = string.Empty;
            if (proc.HasExited)
            {
                output = myOutput.ReadToEnd();
            }
            return output;
        }

		#endregion Methods 
           }
