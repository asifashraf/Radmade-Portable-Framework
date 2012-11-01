using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class objectEx
  {
	#region Methods

    public static T CastTo<T>(this object o)
	{
		return (T)o;
	}

	public static T CloneTo<T>(this object toClone)
		{
			Type sourceType = toClone.GetType();
      Type targetType = typeof(T);
      PropertyInfo[] sourceProps = sourceType.GetProperties();//s prop
      PropertyInfo[] targetProps = targetType.GetProperties();//t prop
      ConstructorInfo construct = targetType.GetConstructor(new Type[] { });//target constructor
      object instance = construct.Invoke(new object[] { });//t instance
      foreach (PropertyInfo pi in sourceProps)
      {
          IEnumerable<PropertyInfo> q = from p in targetProps
                                 where p.Name == pi.Name
                                 select p;
          if (q.Count() > 0)
          {
              PropertyInfo p = q.First<PropertyInfo>();
              try
              {
                  p.SetValue(instance, pi.GetValue(toClone, null), null);
              }
              catch { }
          }
      }
      return instance.CastTo<T>();
		}

	public static void CopyMatchProperties(this object sourceObject,	object targetInstance,
        params string[] exceptions)
		{
            List<string> exceptionslower = exceptions.ToListSafely<string>();
            exceptionslower.ToLower();
			Type sourceType = sourceObject.GetType();
			Type targetType = targetInstance.GetType();
			PropertyInfo[] sourceProps = sourceType.GetProperties();//s prop
			PropertyInfo[] targetProps = targetType.GetProperties();//t prop
			ConstructorInfo construct = targetType.GetConstructor(new Type[] { });//target constructor
			foreach (PropertyInfo pi in sourceProps)
			{
                var exceptional = from i in exceptionslower
                                  where i == pi.Name.ToLower()
                                  select i;
                if (exceptional.Any())
                {
                    continue;
                }
				var q = from p in targetProps
																			where p.Name == pi.Name
																			select p;
				if (q.Any())
				{
					var p = q.First<PropertyInfo>();
					try
					{
						p.SetValue(targetInstance, pi.GetValue(sourceObject, null), null);
					}
					catch { }
				}
			}
		}
    
    public static object GetPropertyValue(this object o, string propertyName)
    {
        Type t = o.GetType();
        PropertyInfo pi = t.GetProperty(propertyName);
        if (null != pi)
        {
            return pi.GetValue(o, new object[] { });
        }
        else
        {
            FieldInfo fi = t.GetField(propertyName);
            if (null != fi)
            {
                return fi.GetValue(o);
            }
            else
            {
                throw new Exception("No property or field with name " + propertyName + " could not be found");
            }
        }
    }
    
    public static void SetPropertyValue(this object o, string propertyName, object value)
    {
        Type t = o.GetType();
        PropertyInfo pi = t.GetProperty(propertyName);
        if (null != pi)
        {
            pi.SetValue(o, value, new object[] { });
        }
        else
        {
            FieldInfo fi = t.GetField(propertyName);
            if (null != fi)
            {
                fi.SetValue(o, value);
            }
            else
            {
                throw new Exception("No property or field with name " + propertyName + " could not be found");
            }
        }
    }
	
    public static bool IsConvertableToDate(this object o)
    {
        if (o.IsNotNullOrEmpty())
        {
            bool done = false;
            try
            {
                DateTime dt = Convert.ToDateTime(o);
                if (dt.IsNotNull())
                {
                    done = true;
                }
            }
            catch { }
            return done;
        }
        return false;
    }

    public static bool IsNotNull(this object s)
    {
        return null != s;
    }

    public static bool IsNotNullOrEmpty(this object s)
    {
        return !s.IsNullOrEmpty();
    }

    public static bool IsNull(this object s)
    {
        return null == s;
    }

    public static bool IsNullOrEmpty(this object s)
    {
        return "".IsNullOrEmpty(s);
    }

    public static bool IsNullOrEmptyOrDbNull(this object objectToCheck)
    {
      return (objectToCheck.IsNullOrEmpty() || objectToCheck == DBNull.Value);
    }
    
    public static bool IsNotNullOrEmptyOrDbNull(this object objectToCheck)
    {
      return !(objectToCheck.IsNullOrEmpty() || objectToCheck == DBNull.Value);
    }

    public static bool IsNullOrEmpty(this object o, object toCheck)
    {
        if (null == toCheck)
            return true;
        if (String.IsNullOrEmpty(toCheck.ToString().Trim()))
            return true;

        return false;
    }

    public static bool MatchByString(this object o, object objectToMatch)
    {
        return "".MatchByString(o, objectToMatch);
    }

    public static bool MatchByString(this object o, object object1, object object2)
    {
        if ("".IsNullOrEmpty(object1) && "".IsNullOrEmpty(object2))
            return true;

        if ("".IsNullOrEmpty(object1) || "".IsNullOrEmpty(object2))
            return false;

        return object1.ToString().Trim().ToLower() == object2.ToString().Trim().ToLower();
    }

    public static string Text(this object o)
    {
        if (null == o)
        {
            return string.Empty;
        }
        return o.ToString();
    }

    public static string ToTwoDecimalPlaces(this object o)
    {
        if (o.IsNotNullOrEmpty())
        {
            if (o.ts().Contains(".") 
                && o.ts().CountOccurance(o.ts(), '.') == 1)
            {
                string beforeDecimal = o.ts().Substring(0,
                    o.ts().IndexOf("."));
                if (beforeDecimal.Length == 0)
                {
                    beforeDecimal = "0";
                }

                string afterDecimal = o.ts().Substring(
                    o.ts().IndexOf(".") + 1);
                if(afterDecimal.Length < 2)
                {
                    afterDecimal = afterDecimal.ToTwoDigitsAfterDecimal();
                }
                else
                {
                    afterDecimal = afterDecimal.Substring(0, 2);
                }

                return string.Format("{0}.{1}", 
                    beforeDecimal,
                    afterDecimal);
            }
        }
        return o.Text();
    }

    public static string ToThreeDecimalPlaces(this object o)
    {
        if (o.IsNotNullOrEmpty())
        {
            if (o.ts().Contains(".")
                && o.ts().CountOccurance(o.ts(), '.') == 1)
            {
                string beforeDecimal = o.ts().Substring(0,
                    o.ts().IndexOf("."));
                if (beforeDecimal.Length == 0)
                {
                    beforeDecimal = "0";
                }

                string afterDecimal = o.ts().Substring(
                    o.ts().IndexOf(".") + 1);
                if (afterDecimal.Length < 3)
                {
                    afterDecimal = afterDecimal.ToThreeDigitsAfterDecimal();
                }
                else
                {
                    afterDecimal = afterDecimal.Substring(0, 3);
                }

                return string.Format("{0}.{1}",
                    beforeDecimal,
                    afterDecimal);
            }
        }
        return o.Text();
    }

    public static string ToTwoDigits(this object o)
    {
        if (o.IsNullOrEmpty())
            return "00";
        if (o.ts().Length == 1)
            return string.Format("0{0}", o.ts());
        return o.ts();
    }
    
    public static string ToTwoDigitsAfterDecimal(this object o)
    {
        if (o.IsNullOrEmpty())
            return "00";
        if (o.ts().Length == 1)
            return string.Format("{0}0", o.ts());
        return o.ts();
    }
    
    public static string ToThreeDigits(this object o)
    {
        if (o.IsNullOrEmpty())
            return "000";
        if (o.ts().Length == 1)
            return string.Format("0{0}", o.ts());
        if (o.ts().Length == 2)
            return string.Format("00{0}", o.ts());
        return o.ts();
    }
    
    public static string ToThreeDigitsAfterDecimal(this object o)
    {
        if (o.IsNullOrEmpty())
            return "000";
        if (o.ts().Length == 1)
            return string.Format("{0}00", o.ts());
        if (o.ts().Length == 2)
            return string.Format("{0}0", o.ts());
        return o.ts();
    }
    
    public static string ts(this object o)
    {
        return o.ToString();
    }
        
    #endregion Methods 
  }