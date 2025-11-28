using Core.Configuration;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.Device;
using Entities.Concrete.Offer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Offer> GetAllOfferDetails()
        {

            //throw new NotImplementedException();

            var list = new List<Offer>();
            var data = base.GetAll();
            var customerCollection = base._dataBase.GetCollection<Customer>("Customers");
            var employeeCollection = base._dataBase.GetCollection<Employee>("Employees");

            foreach (var item in data)
            {
                var filter = Builders<Customer>.Filter.Eq(k => k.Id, item.CustomerId);
                var customer = customerCollection.Find(filter).FirstOrDefault();

                var Empfilter = Builders<Employee>.Filter.Eq(k => k.Id, item.CustomerId);
                var employee = employeeCollection.Find(Empfilter).FirstOrDefault();



                list?.Add(new Offer
                {
                    Id = item?.Id,
                    CustomerId = customer?.CompanyName,
                    OfferTitle = item?.OfferTitle,
                    CreatedAt = item?.CreatedAt,
                    TotalAmount = item.TotalAmount,
                    UpdatedAt = item?.UpdatedAt,
                    items = item.OfferDetails,
                });
            }
            return list;
        }
    }
}
