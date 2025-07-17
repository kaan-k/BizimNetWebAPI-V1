using Core.Configuration;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.InstallationRequest;
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
    public class Mongo_InstallationRequestDal:MongoRepository<InstallationRequest>, IInstallationRequestDal
    {
        private readonly IMongoDatabase _db;
        public Mongo_InstallationRequestDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
          : base(database, settings.Value.InstallationRequestsCollectionName)
        {
            _db = database;
        }
        public List<InstallationRequest> GetAll(Expression<Func<InstallationRequest, bool>> filter = null)
        {
            if (filter == null)
                return _collection.Find(FilterDefinition<InstallationRequest>.Empty).ToList();

            return _collection.Find(filter).ToList();
        }
    }
}
