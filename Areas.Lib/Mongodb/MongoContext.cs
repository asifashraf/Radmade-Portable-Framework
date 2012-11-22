using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
namespace WebAreas.Lib.Mongodb
{
    //help docs http://www.mongodb.org/display/DOCS/CSharp+Driver+Quickstart#CSharpDriverQuickstart-Introduction

    public class MongoContext
    {
        public string ConnectionString { get; set; }

        public MongoServer Server { get; set; }
        public MongoDatabase Database { get; set; }

        public MongoContext(string database, string connectionString = "")
        {
            this.ConnectionString = connectionString.IsNullOrEmpty() ? "mongodb://localhost/?safe=true" : connectionString;
            this.Server = MongoServer.Create(this.ConnectionString);
            this.Database = this.Server.GetDatabase(database);
        }
        
        //Create
        public SafeModeResult CreateDocument<T>(T document)
        {
            return GetCollection<T>().Insert<T>(document);
        }

        //Read
        public IQueryable<T> ReadAll<T>()
        {
            return GetCollection<T>().AsQueryable<T>();
        }

        public T ReadOneById<T>(string id)
        {
            var query = Query.EQ("_id", id);
            return GetCollection<T>().FindOneAs<T>(query);
        }

        public MongoCursor<T> ReadByQuery<T>(IMongoQuery query)
        {
            return GetCollection<T>().FindAs<T>(query);
        }

        //Update
        public SafeModeResult UpdateDocument<T>(T document)
        {
            return GetCollection<T>().Save<T>(document);
        }

        public SafeModeResult UpdateByQuery<T>(UpdateBuilder update, IMongoQuery query)
        {
            return GetCollection<T>().Update(query, update);
        }
        
        //Delete
        public SafeModeResult DeleteDocument<T>(IMongoDocument document)
        {
            return GetCollection<T>().Remove(Query.EQ("_id", document._id));
        }

        public SafeModeResult DeleteById<T>(long id)
        {
            return GetCollection<T>().Remove(Query.EQ("_id", id.ToString()));
        }

        public SafeModeResult DeleteByQuery<T>(IMongoQuery query)
        {
            return GetCollection<T>().Remove(query);
        }

        public SafeModeResult DeleteAll<T>()
        {
            return GetCollection<T>().RemoveAll();
        }

        //private members
        private string getCollectionName<T>()
        {
            return typeof(T).Name;
        }

        private MongoCollection<T> GetCollection<T>()
        {
            return this.Database.GetCollection<T>(typeof(T).Name);
        }

    }
}
