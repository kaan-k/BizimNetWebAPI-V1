using Core.Configuration;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.Offer;
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
    public class Mongo_OfferDal:MongoRepository<Offer>, IOfferDal
    {
        private readonly IMongoDatabase _db;
        public Mongo_OfferDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
  : base(database, settings.Value.OffersCollectionName)
        {
            _db = database;
        }
        public List<Offer> GetAll(Expression<Func<Offer, bool>> filter = null)
        {
            if (filter == null)
                return _collection.Find(FilterDefinition<Offer>.Empty).ToList();

            return _collection.Find(filter).ToList();
        }
    }
}
