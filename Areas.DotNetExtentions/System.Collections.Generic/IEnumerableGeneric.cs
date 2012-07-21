using System.Collections.Generic;
using System.Linq;
using System.Text;

    public static class IEnumerableX
    {
        #region Methods (4)
        public static bool HasDuplication<T>(this IEnumerable<T> data,out object duplicatedValue)
        {
        if (null == data)
        {
            duplicatedValue = null;
            return false;
        }

        foreach (T o in data)
        {
            if (!"".IsNullOrEmpty(o))
            {
                IEnumerable<T> found = from t in data
                                       where "".MatchByString(o, t)
                                       select t;
                if (found.Count() > 1)
                {
                    duplicatedValue = found.First<T>();
                    return false;
                }
            }                
        }
        duplicatedValue = null;
        return false;
        }
        public static T One<T>(this IEnumerable<T> data)
        {
        	if (null == data)
        	{
        		return default(T);
        	}
        	try
        	{
        		return data.First();
        	}
        	catch
        	{
        		return default(T);
        	}
        }
        public static string ToCommaSeparatedString<T>(this IEnumerable<T> list)
        {
        StringBuilder sb = new StringBuilder();
        foreach (T t in list)
        {
            if (t.IsNotNullOrEmpty())
            {
                sb.Append(t.ts());
                sb.Append(",");
            }
        }
        return sb.ts().TrimEnd(',');
        }
        public static string ToDelimitedString<T>(this IEnumerable<T> list, string delimiter)
        {
        StringBuilder sb = new StringBuilder();
        bool first = true;
        foreach (T t in list)
        {
        if (t.IsNotNullOrEmpty())
        {
          if (first.Not()) sb.Append(delimiter);
          sb.Append(t.ts());
        }
        first = false;
        }
        return sb.ToString();
        }
        public static List<T> ToListSafely<T>(this IEnumerable<T> list)
        {
        		if (null == list)
        			return new List<T>();

        if (list.Count<T>() > 0)
            return list.ToList<T>();
        else
            return new List<T>();
        }
        public static string ToSingleLine<T>(this IEnumerable<T> data, string delimiter)
        {
        StringBuilder sb = new StringBuilder();

        foreach (var s in data)
        {
            sb.Append(string.Format("{0}{1}",
                s.text(), delimiter));
        }

        return sb.ToString()
            .TrimEnd(delimiter.ToCharArray());
        }
    	public static bool CountedZero<T>(this IEnumerable<T> data)
    	{
    		if (data.IsNull())
    			return true;
    		else
    			if (data.Count() == 0)
    				return true;
    		else
    			return false;
    	}
		public static bool CountedZeroOrNegative<T>(this IEnumerable<T> data)
		{
			if (data.IsNull())
				return true;
			else
				return data.Count() <= 0;
		}
    	public static bool CountedPositive<T>(this IEnumerable<T> data)
    	{
    		if (data.IsNull())
    			return false;
    		else
    			return data.Count() > 0;
    	}
    	public static bool CountedNegative<T>(this IEnumerable<T> data)
    	{
    		if (data.IsNull())
    			return false;
    		else
    			return data.Count() < 0;
    	}
        #endregion Methods
    }
