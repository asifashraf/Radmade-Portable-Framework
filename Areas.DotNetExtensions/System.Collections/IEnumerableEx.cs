using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq.Expressions;
using System.Linq;

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

        public static List<T> CustomSort<T, TPropertyType>
    (this IEnumerable<T> collection, string propertyName, string sortOrder)
        {
            List<T> sortedlist = null;

            ParameterExpression pe = Expression.Parameter(typeof(T), "p");
            Expression<Func<T, TPropertyType>> expr = Expression.Lambda<Func<T, TPropertyType>>(Expression.Property(pe, propertyName), pe);

            if (!string.IsNullOrEmpty(sortOrder) && sortOrder == "desc")
                sortedlist = collection.OrderByDescending<T, TPropertyType>(expr.Compile()).ToList();
            else
                sortedlist = collection.OrderBy<T, TPropertyType>(expr.Compile()).ToList();

            return sortedlist;
        }

        public static IEnumerable<T> CustomSort<T>(this IEnumerable<T> source, string propertyName, bool descending)
        {
            if (null == source) throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(propertyName)) return source;

            var p = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(p, propertyName);
            var keySelector = Expression.Lambda(prop, p);
            // p => p.Property

            var pSource = Expression.Parameter(typeof(IEnumerable<T>), "source");
            var orderBy = Expression.Call(typeof(Enumerable),
                (descending ? "OrderByDescending" : "OrderBy"),
                new[] { typeof(T), prop.Type },
                pSource, keySelector);

            var sorter = Expression.Lambda<Func<IEnumerable<T>, IEnumerable<T>>>(orderBy, pSource);
            // source => source.OrderBy[Descending](p => p.Property)

            return sorter.Compile()(source);
        }
    }
