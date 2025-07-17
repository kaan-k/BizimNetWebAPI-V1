using Core.Configuration;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.Offer;
using Entities.Concrete.Service;
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
    public class Mongo_ServicingDal : MongoRepository<Servicing>, IServicingDal
    {
        private readonly IMongoDatabase _db;
        public Mongo_ServicingDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
  : base(database, settings.Value.ServicesCollectionName)
        {
            _db = database;
        }
        public List<Servicing> GetAll(Expression<Func<Servicing, bool>> filter = null)
        {
            if (filter == null)
                return _collection.Find(FilterDefinition<Servicing>.Empty).ToList();

            return _collection.Find(filter).ToList();
        }
    }
}
