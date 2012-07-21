using System.Collections.Generic;
using System.Collections;

    public static class IEnumerableEx
    {
		#region Methods (1) 

		// Public Methods (1) 

        public static List<T> ToList<T>(this IEnumerable e)
        {
            List<T> list = new List<T>();
            if (e.IsNull()) return list;
            IEnumerator r = e.GetEnumerator();            
            while (r.MoveNext())
            {
                list.Add((T)r.Current);
            }
            return list;
        }       
		#endregion Methods 
    }
