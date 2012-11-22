using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAreas.Lib.Mongodb
{
    public interface IMongoDocument
    {
        ObjectId _id { get; set; }
    }
}
