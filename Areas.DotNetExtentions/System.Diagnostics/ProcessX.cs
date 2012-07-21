using System.Diagnostics;

    public static class ProcessX
    {
		#region Methods (3) 

		// Public Methods (3) 

			public static void FindAndKillMe(this Process proc, string processName)
			{
				//here we're going to get a list of all running processes on
				//the computer
				foreach (Process clsProcess in Process.GetProcesses())
				{
					//now we're going to see if any of the running processes
					//match the currently running processes by using the StartsWith Method,
					//this prevents us from incluing the .EXE for the process we're looking for.
					//. Be sure to not
					//add the .exe to the name you provide, i.e: NOTEPAD,
					//not NOTEPAD.EXE or false is always returned even if
					//notepad is running
					if (clsProcess.ProcessName.ToLower().Trim() == processName.ToLower().Trim())
					{
						//since we found the proccess we now need to use the
						//Kill Method to kill the process. Remember, if you have
						//the process running more than once, say IE open 4
						//times the loop thr way it is now will close all 4,
						//if you want it to just close the first one it finds
						//then add a return; after the Kill
						try
						{
							clsProcess.Kill();
						}
						catch { }
					}
				}				
			}

        public static string RunProcess(
            this Process Instance,
            string fileFullPath,
            string commandParameters)
        {
            //Create process
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //strCommand is path and file name of command to run
            pProcess.StartInfo.FileName = fileFullPath;

            //strCommandParameters are parameters to pass to program
            pProcess.StartInfo.Arguments = commandParameters;

            pProcess.StartInfo.UseShellExecute = false;

            //Set output of program to be written to process output stream
            pProcess.StartInfo.RedirectStandardOutput = true;

            //Start the process
            pProcess.Start();

            //Get program output
            string strOutput = pProcess.StandardOutput.ReadToEnd();

            //Wait for process to finish
            pProcess.WaitForExit();

            return strOutput; 

        }

        public static string RunProcess(
            this Process instance,
            string workingDirectory,
            string fileFullName, 
            string arguments)
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.WorkingDirectory = workingDirectory;
            proc.StartInfo.FileName = fileFullName;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            proc.Close();
            return output;
        }

		#endregion Methods 
    }

