using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAreas.Lib.Mongodb
{
    public abstract class MongoDocument : IMongoDocument
    {
        public ObjectId _id { get; set; }
    }
}
