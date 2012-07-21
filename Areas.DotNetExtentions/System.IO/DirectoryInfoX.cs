using System;
using System.Collections.Generic;
using System.IO;

    public static class DirectoryInfoX
    {
		#region Properties (1) 

        public static DirectoryInfo DefaultInstance
        {
            get
            {
                return new DirectoryInfo("C:");
            }
        }

		#endregion Properties 

		#region Methods (7) 

		// Public Methods (7) 

			/// <summary>
			/// Create directory if not there
			/// </summary>
			/// <param name="instance"></param>
        public static void CreateIfNotExists(this DirectoryInfo instance)
        {
            if (!instance.Exists)
            {
                instance.Create();
            }
        }

			/// <summary>
			/// with web slash in the end
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
				public static string EnsureLocalDriveSlashInTheEnd(
							this string path
							)
				{
					return "".EnsureLocalDriveSlashInTheEnd(path);
				}

			/// <summary>
			/// with local slash in the end
			/// </summary>
			/// <param name="s"></param>
			/// <param name="path"></param>
			/// <returns></returns>
        public static string EnsureLocalDriveSlashInTheEnd(
            this string s,
            string path
            )
        {
            if (path.EndsWith(@"\"))
                return path;
            else
                return string.Format(@"{0}\", path);

        }

				public static string EnsureWebSlashInTheEnd(
						 this string s
						 )
				{
					return "".EnsureWebSlashInTheEnd(s);

				}

        public static string EnsureWebSlashInTheEnd(
            this string s,
            string path
            )
        {
            if (path.EndsWith(@"/"))
                return path;
            else
                return string.Format(@"{0}/", path);
                
        }

        public static List<DirectoryInfo> GetDriveRoots(
            this DirectoryInfo instance)
        {
            List<DriveInfo> fixedDisks = DriveInfoX.DefaultInstance.GetDrives(DriveType.Fixed);
            List<DirectoryInfo> roots = new List<DirectoryInfo>();
            foreach (DriveInfo drive in fixedDisks)
            {
                roots.Add(drive.RootDirectory);
            }
            return roots; 
        }

				/// <summary>
				/// returns Direcotry Info for any special folder
				/// </summary>
				/// <param name="specialFolder"></param>
				/// <returns></returns>
				public static DirectoryInfo GetSpecialFolder(this DirectoryInfo dir, 
					Environment.SpecialFolder specialFolder)
				{
					return new DirectoryInfo(Environment.GetFolderPath(specialFolder));
				}

		#endregion Methods 
    }

