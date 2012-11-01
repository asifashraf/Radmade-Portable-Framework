namespace Areas.Lib.ConnectionResolver
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Configuration;

    public class ResolverSection : ConfigurationSection
    {
        [ConfigurationProperty("dbServers")]
        public DbServerCollection DbServers
        {
            get
            {
                return base["dbServers"] as DbServerCollection;
            }
        }

        /// <summary>
        /// Get resolved connectionstring 
        /// </summary>
        public string Connectionstring
        {
            get
            {
                string defaultConnectionstring = string.Empty;
                foreach (DbServer dbServer in this.DbServers)
                {
                    string url = HttpContext.Current.Request.Url.ToString().ToLower();
                    if (dbServer.Name.ToLower() == System.Environment.MachineName.ToLower())
                    {
                        return dbServer.ConnectionString;
                    }
                    else if (url.Substring(url.IndexOf("//") + 2).StartsWith(dbServer.Name))
                    {
                        return dbServer.ConnectionString;
                    }
                    if (dbServer.Name.ToLower() == "default")
                    {
                        defaultConnectionstring = dbServer.ConnectionString;
                    }
                } // foreach

                if (!string.IsNullOrEmpty(defaultConnectionstring))
                {
                    return defaultConnectionstring;
                }
                else
                {
                    throw new Exception("Connectionstring could not be resolved for machine: " + System.Environment.MachineName);
                }
            }
        }

        /// <summary>
        /// Comma seperated machine names to use as development machines
        /// </summary>
        [ConfigurationProperty("devMachines", IsRequired=false, DefaultValue="")]
        public String DevMachines
        {
            get
            {
                if(base["devMachines"] != null)
                {
                    //return base["devMachines"] as string;
                    return Convert.ToString(base["devMachines"]);
                }
                else
                {
                    return "";
                }                
            }
        }

        /// <summary>
        /// List of machines names
        /// </summary>
        public string[] ListDevMachines
        {
            get
            {
                return this.DevMachines.Split(new char[]{','});
            }
        }

        /// <summary>
        /// Is developer machine
        /// </summary>
        public static bool IsDevMachine
        {
            get
            {
                foreach (var machine in CurrentConfigurationSection.ListDevMachines)
                {
                    if(System.Environment.MachineName.ToLower() == machine.ToLower())
                        return true;
                }
                return false;
            }
        }

        public static ResolverSection CurrentConfigurationSection
        {
            get
            {
                var conn = WebConfigurationManager.GetSection("LocalConnectionResolver") as ResolverSection;
                return conn;
            }
        }
    }
}
