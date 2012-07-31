using System;

public static class ExDateTime
    {
		#region Methods (2) 

		// Public Methods (2) 

        public static string ToSlashForamt_dd_mm_yyyy(this DateTime o)
        {
            return string.Format("{0}/{1}/{2}",                
                o.Day.ts().ToTwoDigits(),
                o.Month.ts().ToTwoDigits(),
                o.Year.ts());
        }

        public static string ToSlashForamt_mm_dd_yyyy(this DateTime o)
        {
            return string.Format("{0}/{1}/{2}", 
                o.Month.ts().ToTwoDigits(), 
                o.Day.ts().ToTwoDigits(), 
                o.Year.ts());
        }//2010-04-18 13:03:44.957

        public static string ToSqlQueryFormat(this DateTime dt)
        {
            return string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6}", dt.Year, dt.Month.ToTwoDigits(), dt.Day.ToTwoDigits(),
                dt.Hour.ToTwoDigits(), dt.Minute.ToTwoDigits(), dt.Second.ToTwoDigits(), dt.Millisecond);
        }

		#endregion Methods 
    }

