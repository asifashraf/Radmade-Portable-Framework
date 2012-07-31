using System.Collections.Generic;
using System.IO;

    public static class DriveInfoX
    {
		#region Properties (1) 

        public static DriveInfo DefaultInstance
        {
            get
            {
                return new DriveInfo("C:");
            }
        }

		#endregion Properties 

		#region Methods (3) 

		// Public Methods (3) 

        public static List<string> GetATOZLetters(this DriveInfo drive)
        {
            List<string> list = new List<string>();
            list.Add("C:");
            list.Add("D:");
            list.Add("E:");
            list.Add("F:");
            list.Add("G:");
            list.Add("H:");
            list.Add("I:");
            list.Add("J:");
            list.Add("K:");
            list.Add("L:");
            list.Add("M:");
            list.Add("N:");            
            list.Add("O:");
            list.Add("P:");
            list.Add("Q:");
            list.Add("R:");
            list.Add("S:");
            list.Add("T:");
            list.Add("U:");
            list.Add("V:");
            list.Add("W:");
            list.Add("X:");
            list.Add("Y:");
            list.Add("Z:");
            return list;
        }

        public static List<DriveInfo> GetDrives(this DriveInfo drive,
            DriveType type)
        {
            List<DriveInfo> list = new List<DriveInfo>();
            foreach (string s in DriveInfoX.DefaultInstance.GetATOZLetters())
            {                
                DriveInfo newDrive = new DriveInfo(s);
                DirectoryInfo di = newDrive.RootDirectory;
                if(drive.DriveType == type && di.Exists)
                {
                    list.Add(newDrive);
                }
            }
            return list;
        }

			/// <summary>
			/// Driver which contain Windows\System32 folders
			/// </summary>
			/// <param name="drive"></param>
			/// <returns></returns>
      public static List<DriveInfo> GetWindowsDrives(this DriveInfo drive)
      {
          List<string> driveLetters = DriveInfoX.DefaultInstance.GetATOZLetters();
          List<DriveInfo> dirs = new List<DriveInfo>();
          foreach (string letter in driveLetters)
          {
              DirectoryInfo d = new DirectoryInfo(
                  letter + @"\Windows\System32");
              if (d.Exists)
              {
                  dirs.Add(new DriveInfo(letter));
              }
          }
          return dirs;
      }

		#endregion Methods 
    }

