using System;
using System.Collections.Generic;
using System.Collections;

    public static class ObjectArray
    {
        public static List<NameAndValue> ToNameAndValueList(this object[] nameValuePairs)
        {
            List<NameAndValue> list = new List<NameAndValue>();
            for (int i = 0; i < nameValuePairs.Length; i += 2)
            {
                try
                {
                    list.Add(new NameAndValue(nameValuePairs[i].ts(), nameValuePairs[i + 1]));
                }
                catch
                {
                    try
                    {
                        object[] nvp = ((IEnumerable)nameValuePairs).ToList<object>().ToArray();
                        for (int i2 = 0; i2 < nvp.Length; i2 += 2)
                        {
                            try
                            {
                                list.Add(new NameAndValue(nameValuePairs[i2].ts(), nameValuePairs[i2 + 1]));
                            }
                            catch
                            {
                                list.Add(new NameAndValue(nameValuePairs[i].ts(), nameValuePairs[i + 1]));
                            }
                        }
                    }
                    catch 
                    {
                        throw new Exception("The name value parameters list is not in correct format");
                    }
                }
            }
            return list;
        }
    }

    public class NameAndValue
    {
        public string Name {get;set;}
        public object Value {get;set;}
        public NameAndValue()
        {
            this.Name = string.Empty;
            this.Value = null;
        }
        public NameAndValue(string name)
        {
            this.Name = name;
            this.Value = null;
        }
        public NameAndValue(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
