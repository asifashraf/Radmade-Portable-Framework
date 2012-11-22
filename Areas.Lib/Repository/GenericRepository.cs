namespace WebAreas.Lib.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;


    public class GenericRepository<TContext> : IDataRepository, IDisposable
        where TContext : DbContext
    {
        protected TContext db;
        //CRUD
        public T Create<T>(T entity) where T : class
        {
            var newEntry = db.Set<T>().Add(entity);
            db.SaveChanges();
            return newEntry;
        }

        public IQueryable<T> Read<T>() where T : class
        {
            return db.Set<T>().AsQueryable<T>();
        }

        public void Update<T>(T entity) where T : class
        {
            var entry = db.Entry(entity);
            db.Set<T>().Attach(entity);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            db.Set<T>().Remove(entity);
            db.SaveChanges();
        }
        
        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
