namespace Areas.Lib.ConnectionResolver
{
    using System.Configuration;

    public class DbServerCollection : ConfigurationElementCollection
    {
        public DbServer this[int index]
        {
            get
            {
                return base.BaseGet(index) as DbServer;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DbServer();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DbServer)element).Name;
        } 
    }
}
