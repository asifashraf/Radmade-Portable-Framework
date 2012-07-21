using System;
using System.Reflection;

    public static class TypeX
    {
		#region Methods (4) 

		// Public Methods (4) 

        public static object CallStaticMethod(
            this Type type, 
            string methodName, 
            bool exceptionOnNull, 
            bool returnEmptyStringOnNull,
            object[] parameters)
        {            
            MethodInfo m = type.GetMethod(methodName);
            if (m != null)
            {
                return m.Invoke(type, parameters);
            }
            else
            {
                if (exceptionOnNull)
                {
                    throw new Exception(
                        String.Format("{0}, method not found in type {1}",
                        methodName, type.FullName)
                        );
                }
                if (returnEmptyStringOnNull)
                {
                    return String.Empty;
                }
                return null;
            }
        }

        public static T CallStaticMethod<T>(this Type type, 
            string methodName,
            bool exceptionOnNullMethod,
            bool returnEmptyStringOnNullMethod,
            object[] parameters)
        {
            object o = type.CallStaticMethod(methodName,
                exceptionOnNullMethod,
                returnEmptyStringOnNullMethod,
                parameters);
            return o.CastTo<T>();
        }

        public static Attribute GetAttribute<tAttribute>(this Type attributeType)
        {
            //Get instance of the attribute.
            Attribute MyAttribute = (Attribute)(Attribute.GetCustomAttribute(attributeType, typeof(tAttribute)));

            if (null == MyAttribute)
            {
                throw new Exception(String.Format("Attribute {0} could not be found on type{0}", typeof(tAttribute).Name, attributeType.Name));
            }
            else
            {
                return MyAttribute;
            }
        }

        public static object GetStaticMethodValue(this Type type, string methodName)
        {
            MethodInfo m = type.GetMethod(methodName);
            return m.Invoke(type, new object[]{});
        }

        public static object DefaultValueByType(this Type any)
        {
            if (any == typeof(String)) return String.Empty;
            if (any == typeof( bool)) return false;
            if (any == typeof(short) || any == typeof(int) || any == typeof(long) || any == typeof(decimal) || any == typeof(double) || any == typeof(float) || any == typeof(byte) || any == typeof(Single)
                || any == typeof(uint) || any == typeof(UInt16) || any == typeof(UInt64)) return 0;
            if (any == typeof(Guid)) return Guid.NewGuid();
            if (any == typeof(DateTime)) return DateTime.Now;
            if (any == typeof(char)) return null;
            throw new Exception("unhandled case");
        }

        public static object Construct(this Type type)
        {
            ConstructorInfo ci = type.GetConstructor(new Type[] { });
            return ci.Invoke(new object[] { });
        }

        #endregion Methods 
    }
