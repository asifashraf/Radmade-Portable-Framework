using System.Collections.Generic;
using System.Xml.Linq;


    public static class ExIEnumerableXAttribute
    {
		#region Methods (3) 

		// Public Methods (3) 

        public static IEnumerable<XAttribute> Change(this IEnumerable<XAttribute> attribs, string attributeName, string newValue)
        {
            try
            {
                List<XAttribute> list = attribs.ToListSafely<XAttribute>();
                XAttribute a = list.GetByName(attributeName);
                a.Value = newValue;
                return list;
            }
            catch
            {
                return new List<XAttribute>();
            }
        }

        public static XAttribute GetByName(this IEnumerable<XAttribute> attribs, string attributeName)
        {
            foreach (XAttribute attrib in attribs)
            {
                if (attrib.Name.LocalName.ToLower() == attributeName.ToLower())
                    return attrib;
            }            
                return null;
        }

        public static IEnumerable<XAttribute> Remove(this IEnumerable<XAttribute> attribs, string attributeName)
        {
            try
            {
                List<XAttribute> list = attribs.ToListSafely<XAttribute>();
                XAttribute a = list.GetByName(attributeName);
                list.Remove(a);
                return list;
            }
            catch
            {
                return new List<XAttribute>();
            }
        }

		#endregion Methods 
    }

