namespace Areas.Lib.LinqToSql
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;

    public class ServiceHolder<TDataContextType> : IDisposable where TDataContextType : DataContext 
    {
        private List<AbstractService<TDataContextType>> _services = new List<AbstractService<TDataContextType>>();
        private TDataContextType _sharedDataContext;
        /*public User User { get; set; }*/

        public TService Service<TService>()
            where TService : AbstractService<TDataContextType>, new()
        {
            var existingService = this._services.FirstOrDefault(s => s.GetType() == typeof(TService));
            if (existingService != null)
                return (TService)existingService;

            existingService = new TService();

            if (this._services.Count == 0)
            {
                // our very first service, we will use it's data context as the shared one!!!
                this._sharedDataContext = existingService.DataContext;
            }
            else
            {
                existingService.DataContext = this._sharedDataContext;	// assign it to reuse the shared data context
            }

            this._services.Add(existingService);
            return (TService)existingService;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._sharedDataContext != null)
                    this._sharedDataContext.Dispose();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
