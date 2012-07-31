using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

    public static class ConfigurationSectionEx
    {        
        public static T LoadConfigSection<T>(
            this ConfigurationSection configSection, 
            string defaultName,
            SectionDestination destination)
        {
            T _section = default(T);

            //Try to get a reference to the default <netTiersService> section
            if (!String.IsNullOrEmpty(defaultName))
            {         
                switch(destination)
                {
                    case SectionDestination.Website:
                    case SectionDestination.Machine:
                    _section = (T)WebConfigurationManager.GetSection(defaultName);
                    break;
                    
                    case SectionDestination.Executable:
                    _section = (T)ConfigurationManager.GetSection(defaultName);
                    break;
                }
            }
            
            if (_section == null)
            {
                Configuration c = null;
                switch (destination)
                {
                    case SectionDestination.Website:
                        c = WebConfigurationManager.OpenWebConfiguration("~");
                        break;
                    case SectionDestination.Machine:
                        c = WebConfigurationManager.OpenMachineConfiguration();
                    break;
                    case SectionDestination.Executable:
                        c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        break;
                }
                    

                // lastly, try to find the specific NetTiersServiceSection for this assembly
                foreach (ConfigurationSection temp in c.Sections)
                {
                    if (typeof(T) == temp.GetType())
                    {
                        return (T)(object)temp;
                    }
                }
            }

            if (_section != null)
                return _section;
            else
                throw new Exception(string.Format("section {0} could not be loaded", configSection.ToString()));
        }

        public static T LoadConfigSection<T>(
            this ConfigurationSection configSection,
            string defaultName)
        {
            T _section = default(T);
            
            if (!String.IsNullOrEmpty(defaultName))
            {            
                _section = (T)ConfigurationManager.GetSection(defaultName); 
            }

            if (_section == null)
            {
                Configuration c = (null == HttpContext.Current) ?
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) :
                WebConfigurationManager.OpenWebConfiguration("~");

                // lastly, try to find the specific NetTiersServiceSection for this assembly
                foreach (ConfigurationSection temp in c.Sections)
                {
                    if (typeof(T) == temp.GetType())
                    {
                        return (T)(object)temp;
                    }
                }
                
                c = WebConfigurationManager.OpenMachineConfiguration();
                // lastly, try to find the specific NetTiersServiceSection for this assembly
                foreach (ConfigurationSection temp in c.Sections)
                {
                    if (typeof(T) == temp.GetType())
                    {
                        return (T)(object)temp;
                    }
                }                   
            }

            if (_section != null)
                return _section;
            else
                throw new Exception(string.Format("section {0} could not be loaded", configSection.ToString()));
        }
    }

    public enum SectionDestination
    {
        Website,
        Machine,
        Executable
    }
