using System.Text;
using System.IO;

    public static class FileStreamEx
    {
		#region Methods (1) 

		// Public Methods (1) 

        public static void AddText(this FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

		#endregion Methods 
    }

