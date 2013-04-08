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

        public void Create<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }
            
            db.SaveChanges();
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
        
        public void Delete<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Remove(entity);
            }
            db.SaveChanges();
        }
        
        public void Dispose()
        {
            this.db.Dispose();
        }
        
        public void CreateWith<T>(IEnumerable<object> entities) where T : class
        {
            if (entities.IsNull())
            {
                return;
            }
            
            var list = new List<T>();

            foreach (var item in entities)
            {
                var t = typeof(T).Construct() as T;

                item.CopyMatchProperties(t);

                list.Add(t);
            }

            Create<T>(list);
        }

        public T CreateWith<T>(object entity) where T : class
        {
            var t = typeof(T).Construct() as T;

            entity.CopyMatchProperties(t);

            return Create<T>(t);
        }        

        public void UpdateWith<T>(T entity) where T : class
        {
            var t = typeof(T).Construct() as T;

            entity.CopyMatchProperties(t);

            Update<T>(t);
        }
    }
}
