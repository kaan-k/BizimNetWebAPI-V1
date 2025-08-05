using Core.Configuration;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete;
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

        public List<BusinessUserDetailsDto> GetAll()
        {
            var list = new List<BusinessUserDetailsDto>();
            var data = base.GetAll();
            foreach (var item in data)
            {
                list.Add(new BusinessUserDetailsDto
                {
                    Id = item.Id,
                    CompanyAddress = item.CompanyAddress,
                    CompanyName = item.CompanyName,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email
                });
            }
            return list;

        }
    }
}
