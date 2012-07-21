using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumX
    {
		#region Methods (4) 

		// Public Methods (4) 

        public static List<EnumMember> GetAllEnumMembers<T>(this Enum en)
        {
            List<EnumMember> res = new List<EnumMember>();
            StringEnum se = new StringEnum(en.GetType());
            Array arr = se.GetStringValues();
            foreach (var v in arr)
            {
                res.Add(new EnumMember
                {
                    Name = Enum.GetName(typeof(T),("".GetEnum<T>(v.ToString()))),
                     StringValue = v.ToString()
                });
            }
            return res;
        }
        public static string[] GetNames<T>(this Enum en)
        {
            var all = en.GetAllEnumMembers<T>();
            return (from a in all
                    select a.Name).ToArray<string>();
        }
        public static string[] GetValues<T>(this Enum en)
        {
            var all = en.GetAllEnumMembers<T>();
            return (from a in all
                    select a.StringValue).ToArray<string>();
        }
        public static string GetStringValue(this Enum e)
        {
            return StringEnum.GetStringValue(e);
        }
        public static string Name(this Enum e)
        {
            return e.ts();
        }
        public static string Value(this Enum e)
        {
            return e.GetStringValue();
        }        
        #endregion Methods 
    }

    public class EnumMember
    {
		#region Properties (2) 

        public string Name
        {
            get;
            set;
        }

        public string StringValue 
        {
            get; set;
        }

		#endregion Properties 
    }

