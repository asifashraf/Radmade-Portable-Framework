using System;
using System.Collections.Generic;
using System.Linq;

namespace Areas.Lib.AppEnvironment
{
    public class AppDomainHelper
    {
        /// <summary>
        /// Find all types by give attribute type
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type</typeparam>
        /// <returns>Returns all types where the specificied attribute was found</returns>
        public List<Type> FindTypesByAttrib<TAttribute>()
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                         from type in assembly.GetTypes()
                    where Attribute.IsDefined(typeof(TAttribute), type)
                         select type).ToList();

        }
    }
}

