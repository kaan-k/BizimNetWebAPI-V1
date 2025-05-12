using Core.Configuration;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataAccess.Concrete
{
    public class Mongo_BusinessUserDal : MongoRepository<BusinessUser>, IBusinessUserDal
    {
        public Mongo_BusinessUserDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
            : base(database, settings.Value.BusinessUserCollectionName)
        {
        }

        public List<BusinessUser> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
