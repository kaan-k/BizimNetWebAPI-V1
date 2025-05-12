using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DataAccess.MongoDB
{
    public interface IMongoDB_Context<T>
    {
        MongoClient client { get; set; }
        IMongoDatabase database { get; set; }
        IMongoCollection<T> collection { get; set; }

        IMongoCollection<T> GetMongoDBCollection();
    }
}
