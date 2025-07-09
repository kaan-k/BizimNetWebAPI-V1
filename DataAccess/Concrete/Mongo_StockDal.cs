using Core.Configuration;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Stock;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
