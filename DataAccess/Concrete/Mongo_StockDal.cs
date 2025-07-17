using Core.Configuration;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Service;
using Entities.Concrete.Stock;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class Mongo_StockDal:MongoRepository<Stock>, IStockDal
    {
        private readonly IMongoDatabase _db;
        public Mongo_StockDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
  : base(database, settings.Value.StockCollectionName)
        {
            _db = database;
        }
        public List<Stock> GetAll(Expression<Func<Stock, bool>> filter = null)
        {
            if (filter == null)
                return _collection.Find(FilterDefinition<Stock>.Empty).ToList();

            return _collection.Find(filter).ToList();
        }
    }
}
