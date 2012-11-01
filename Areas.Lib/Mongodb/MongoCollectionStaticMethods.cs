using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Areas.Lib.Mongodb
{
    public static class MongoCollectionStaticMethods
    {
        public static T Where<T>(this MongoCollection<T> collection, string id)
        {
            var query = Query.EQ("_id", id);
            return collection.FindOneAs<T>(query);
        }

        public static MongoCursor<T> Where<T>(this MongoCollection<T> collection, IMongoQuery query)
        {            
            return collection.FindAs<T>(query);
        }
    }
}
