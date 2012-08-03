namespace Areas.Lib.Web
{
    public class MvcViewsHelpersConfig
    {
        public MvcViewsHelpersConfig()
        {
            EnableCacheInApplicationStore = true;
        }

        /// <summary>
        /// Search disk evertime for development machines and for production keep in Application Cache Store
        /// </summary>
        public bool EnableCacheInApplicationStore
        {
            get { return _enableCacheInApplicationStore;}
            set { _enableCacheInApplicationStore = value;}
        }
        private bool _enableCacheInApplicationStore = true; 


    }
}