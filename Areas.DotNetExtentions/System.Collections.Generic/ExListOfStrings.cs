using System;
using System.Collections.Generic;
using System.Linq;

public static class ExListOfStrings
    {
		#region Methods (12) 

		// Public Methods (12) 

        public static List<int> CastToIntList(this IEnumerable<string> baseList)
        {
            List<int> listTarget = new List<int>();
            foreach (object o in baseList)
            {
                listTarget.Add(Convert.ToInt32(o));
            }
            return listTarget;
        }

				public static bool HasMembers(this IEnumerable<string> list)
				{
					return list.Count<string>() > 0;
				}

        public static bool HaveMembers(this IEnumerable<string> list)
        {
            return list.Count<string>() > 0;
        }

        public static bool IsEmpty(this IEnumerable<string> list)
        {
            return list.Count<string>() == 0;
        }

        public static void PrefixAdd(this List<string> list, string prefix)
        {
            for (int i = 0; i < list.Count; i++)
                list[i] = string.Format("{0}{1}", prefix, list[i]);
        }

        public static void PrefixRemove(this List<string> list, string prefix)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].ToLower().StartsWith(prefix))
                    list[i] = list[i].Substring(prefix.Length);
        }

        public static void PrefixReplace(this List<string> list, string prefix, string newPrefix)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].ToLower().StartsWith(prefix))
                    list[i] = string.Format("{0}{1}", newPrefix, list[i].Substring(prefix.Length));
        }

        public static void SuffixAdd(this List<string> list, string suffix)
        {
            for (int i = 0; i < list.Count; i++)
                list[i] = string.Format("{0}{1}", list[i], suffix);
        }

        public static void SuffixRemove(this List<string> list, string suffix)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].ToLower().EndsWith(suffix))
                    list[i] = list[i].Substring(0, list[i].LastIndexOf(suffix));
        }

        public static void SuffixReplace(this List<string> list, string suffix, string newSuffix)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].ToLower().EndsWith(suffix))
                    list[i] = string.Format("{0}{1}", list[i].Substring(0, list[i].LastIndexOf(suffix)), newSuffix);
        }

        public static List<string> ToListSafely(IEnumerable<string> list)
        {
            return list.ToListSafely<string>();
        }

        public static void ToLower(this List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
                try
                {
                    list[i] = list[i].ToLower();
                }
                catch { }
        }

		#endregion Methods 
    }

