using Core.Configuration;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace DataAccess.Concrete
{
    public class Mongo_BusinessUserDal : MongoRepository<BusinessUser>, IBusinessUserDal
    {
        public Mongo_BusinessUserDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
            : base(database, settings.Value.BusinessUserCollectionName)
        {
        }

        public List<BusinessUser> GetAll(Expression<Func<BusinessUser, bool>> filter = null)
        {
            if (filter == null)
                return _collection.Find(FilterDefinition<BusinessUser>.Empty).ToList();

            return _collection.Find(filter).ToList();
        }
    }
}
