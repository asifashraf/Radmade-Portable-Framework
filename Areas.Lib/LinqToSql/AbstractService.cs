namespace Areas.Lib.LinqToSql
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;

    using Areas.Lib.ConnectionResolver;

    using Microsoft.AppFabricCAT.Samples.Azure.TransientFaultHandling;
    using Microsoft.AppFabricCAT.Samples.Azure.TransientFaultHandling.SqlAzure;
    using Microsoft.AppFabricCAT.Samples.Azure.TransientFaultHandling.Configuration;

    /// <summary>
    /// All Services inherit from this service.
    /// All Services are disposable and should be disposed after usage.
    /// </summary>
    public abstract class AbstractService<TDataContextType> : IDisposable
        where TDataContextType : DataContext
    {
        private TDataContextType _dataContext;
        private bool _ownsDataContext = false;

        public void SetDataContext(TDataContextType dataContext)
        {
            if (this._dataContext != null && this._dataContext != dataContext)
            {
                if (this._ownsDataContext)
                {
                    this._dataContext.Dispose();
                }
            }
            this._dataContext = dataContext;
        }

        protected void Retry(Action action)
        {
            RetryPolicyConfigurationSettings retryPolicySettings = ApplicationConfiguration.Current.GetConfigurationSection<RetryPolicyConfigurationSettings>(RetryPolicyConfigurationSettings.SectionName);
            RetryPolicyInfo retryPolicyInfo = retryPolicySettings.Policies.Get("FixedIntervalDefault");
            RetryPolicy sqlAzureRetryPolicy = retryPolicyInfo.CreatePolicy<SqlAzureTransientErrorDetectionStrategy>();

            sqlAzureRetryPolicy.ExecuteAction(() =>
            {
                action.Invoke();
            });
        }

        protected TResult Retry<TResult>(Func<TResult> function)
        {
            TResult result = default(TResult);

            RetryPolicyConfigurationSettings retryPolicySettings = ApplicationConfiguration.Current.GetConfigurationSection<RetryPolicyConfigurationSettings>(RetryPolicyConfigurationSettings.SectionName);
            RetryPolicyInfo retryPolicyInfo = retryPolicySettings.Policies.Get("FixedIntervalDefault");
            RetryPolicy sqlAzureRetryPolicy = retryPolicyInfo.CreatePolicy<SqlAzureTransientErrorDetectionStrategy>();

            sqlAzureRetryPolicy.ExecuteAction(() =>
            {
                result = function.Invoke();
            });

            return result;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TDataContextType DataContext
        {
            get
            {
                if (this._dataContext == null)
                {
                    this._ownsDataContext = true;
                    this._dataContext = typeof(TDataContextType).Construct(typeof(String), ResolverSection.CurrentConfigurationSection.Connectionstring) as TDataContextType;
                    this.OnDataContextCreated();
                }
                return this._dataContext;
            }
            set
            {
                this.SetDataContext(value);
                this._ownsDataContext = false;
            }
        }

        protected virtual void OnDataContextCreated()
        {

        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (this._dataContext != null && this._ownsDataContext)
                    {
                        this._dataContext.Dispose();
                        this._dataContext = null;
                    }
                }
            }
            this._disposed = true;
        }

        public IQueryable<T> All<T>() where T : class
        {
            return this.Retry(() => this.DataContext.GetTable<T>());
        }


        public T Get<T>(Func<T,bool> predicate) where T : class
        {
            return this.Retry(() => this.DataContext.GetTable<T>().Where(predicate).One());
        }
        public T Create<T>(T entity) where T : class
        {
            return this.Retry(() =>
                {
                    this.DataContext.GetTable<T>().InsertOnSubmit(entity);
                    this.DataContext.SubmitChanges();
                    return entity;
                });
        }

        public IEnumerable<T> Create<T>(IEnumerable<T> entities) where T : class
        {
            return this.Retry(() =>
            {
                foreach (var entity in entities)
                {
                    this.DataContext.GetTable<T>().InsertOnSubmit(entity);
                }

                this.DataContext.SubmitChanges();
                
                return entities;
            });
        }

        public void Delete<T>(T entity) where T : class
        {
            this.Retry(() =>
            {
                this.DataContext.GetTable<T>().DeleteOnSubmit(entity);
                this.DataContext.SubmitChanges();
            });
        }

        public void Delete<T>(IEnumerable<T> entities) where T : class
        {
            this.Retry(() =>
            {
                this.DataContext.GetTable<T>().DeleteAllOnSubmit(entities);

                this.DataContext.SubmitChanges();
            });
        }

        public void SaveChanges()
        {
            this.Retry(() => this.DataContext.SubmitChanges());
        }
    }
}
