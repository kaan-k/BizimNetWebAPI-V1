using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public abstract class MongoBaseRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        protected MongoBaseRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }
        public virtual List<T> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }


    }
}
