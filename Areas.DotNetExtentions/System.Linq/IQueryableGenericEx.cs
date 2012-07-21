using System.Collections.Generic;
using System.Linq;

public static class IQueryableGenericEx
    {
		#region Methods (1) 

		// Public Methods (1) 

        public static List<T> ToListSafe<T>(this IQueryable<T> iq)
        {
            return iq.Any() ? iq.ToList<T>() : new List<T>();
        }

    #endregion Methods 
    }

