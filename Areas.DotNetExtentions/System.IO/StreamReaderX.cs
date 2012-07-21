using System.Collections.Generic;
using System.IO;

    public static class StreamReaderX
    {
		#region Methods (1) 

		// Public Methods (1) 

        public static bool ReadLines(this StreamReader rd, int linesAtOnce, out List<string> lines)
        {
            lines = null;
            lines = new List<string>();
                        
            for (int b = 1; b <= linesAtOnce; b++)
            {
                if (!rd.EndOfStream)
                {
                    lines.Add(rd.ReadLine());
                }
            }
            if (lines.Count > 0)
            {
                return true;
            }
            else
            {

            } return !rd.EndOfStream;
        }

		#endregion Methods 
    }

