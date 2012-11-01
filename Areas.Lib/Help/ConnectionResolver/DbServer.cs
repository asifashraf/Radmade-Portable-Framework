namespace Areas.Lib.ConnectionResolver
{
    using System.Configuration;

    public class DbServer : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired=true)]
        public string Name
        {
            get
            {
                return base["name"].ToString();
            }
        }

        [ConfigurationProperty("connectionString", IsRequired=true)]
        public string ConnectionString
        {
            get
            {
                return base["connectionString"].ToString();
            }
        }
    }
}
