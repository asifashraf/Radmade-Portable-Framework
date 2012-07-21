using System;
using RadApi.Exts.DotNetExts.Support.Enums;

public static class Exbool
    {
		#region Methods (4) 

		// Public Methods (4) 

				public static bool From01(this string value)
				{
					if (value == "1")
						return true;
					else
						return false;
				}

				public static string To01(this bool value)
				{
					return value ? "1" : "0";
				}

				public static bool Not(this bool value)
				{
					return !value;
				}

				public static string ToYesNo(this bool boo)
				{
					 return (new Boolean()).ToYesNo(boo, TextNotation.Pascal);
				}

        public static string ToYesNo(this bool boo, bool value, TextNotation notation)
        {
            switch (notation)
            {
                case TextNotation.Camal:
                case TextNotation.Lower:
                case TextNotation.UnderScoreLower:                
                    return value ? "yes" : "no";
                case TextNotation.Pascal:
                case TextNotation.UnderscorePascal:
                    return value ? "Yes" : "No";
                case TextNotation.Upper:
                case TextNotation.UnderScoreUpper:
                    return value ? "YES" : "NO";
                
            }
            throw new Exception("Notation not handled");
        }

		#endregion Methods 
    }