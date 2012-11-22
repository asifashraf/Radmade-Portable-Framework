using System;
using System.Configuration;
using System.Reflection;
using System.Web;

namespace WebAreas.Lib.Config
{
    public static class SettingsHelper
    {
        /// <summary>
        /// Parses full object by naming convention
        /// </summary>
        /// <typeparam name="T">The type to construct</typeparam>
        /// <returns></returns>
        public static T GetObject<T>()
        {
            Type sourceType = typeof(T);

            if(HttpContext.Current.Application[sourceType.Name + "Config"] != null)
            {
                return HttpContext.Current.Application[sourceType.Name + "Config"].CastTo<T>();
            }

            PropertyInfo[] sourceProps = sourceType.GetProperties();//s prop
            ConstructorInfo construct = sourceType.GetConstructor(new Type[] { });//target constructor
            object instance = construct.Invoke(new object[] { });//t instance
            foreach (PropertyInfo p in sourceProps)
            {
                var settings = ConfigurationManager.AppSettings;
                var appSetting = string.Format("{0}.{1}", sourceType.Name, p.Name);
                if (settings[appSetting].IsNotNull())
                {
                    instance.SetPropertyValue(p.Name, ParseType(p.PropertyType, appSetting, settings[appSetting]));
                }
            }

            var result = instance.CastTo<T>();
            HttpContext.Current.Application[sourceType.Name + "Config"] = result;
            return result;
        }

        /// <summary>
        /// Get any simple app setting and parse it
        /// </summary>
        /// <typeparam name="T">The type to parse setting to</typeparam>
        /// <param name="key">The simple setting name</param>
        /// <param name="defaultValue">If value not present then default value to be used</param>
        /// <returns></returns>
        public static T GetValue<T>(this string key, T defaultValue)
        {
            return ConfigurationManager.AppSettings[key].IsNullOrEmpty() ?
            defaultValue : ParseType(typeof(T), key, ConfigurationManager.AppSettings[key]).CastTo<T>();
        }
        
        /// <summary>
        /// Get app settings based on machine name
        /// </summary>
        public static T GetByMachine<T>(this string settingName, T defaultValue)
        {
            /*return ConfigurationManager.AppSettings[key].IsNullOrEmpty() ?
            defaultValue : (T)Convert.ChangeType((object)ConfigurationManager.AppSettings[key], typeof(T));*/

            var machineName = System.Environment.MachineName;

            string settingFullName = string.Format("{0}[{1}]", settingName, machineName);

            var settings = ConfigurationManager.AppSettings;

            var type = typeof (T);

            if(settings[settingFullName] != null)
            {
                return ParseType(type, settingFullName, settings[settingFullName]).CastTo<T>();
            }

            string url = HttpContext.Current.Request.Url.ToString().ToLower();

            url = url.Substring(url.IndexOf("//") + 2);
            if(url.Contains("/"))
            {
                url = url.Substring(0, url.IndexOf("/"));
            }

            if (settings[url] != null)
            {
                return ParseType(type, settingFullName, settings[url]).CastTo<T>();
            }

            return defaultValue;
        }

        private static object ParseType(Type sourceType, string settingName, object value)
        {
            if (value.Text().StartsWith("connection="))
            {
                var connectionName = value.ToString().Substring(11).Trim();
                return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            }

            object result = null;
            if (sourceType.FullName.StartsWith("System.Collections.Generic.List")) //list parsing
            {
                if (sourceType.FullName.Contains("System.String"))
                {
                    result = value.ToString().SplitToStringList(",");
                }
                
                if (sourceType.FullName.Contains("System.Int32"))
                {
                    result = value.ToString().SplitToIntList(",");
                }
            }
            else
            {
                result = Convert.ChangeType(value, sourceType);
            }
            return result;
        }

    }
}
