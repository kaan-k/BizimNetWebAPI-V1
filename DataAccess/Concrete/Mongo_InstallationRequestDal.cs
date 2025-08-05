using Core.Configuration;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.InstallationRequest;
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
    public class Mongo_InstallationRequestDal:MongoRepository<InstallationRequest>, IInstallationRequestDal
    {
        private readonly IMongoDatabase _db;
        public Mongo_InstallationRequestDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
          : base(database, settings.Value.InstallationRequestsCollectionName)
        {
            _db = database;
        }

        public List<InstallationRequest> GetAllInstallationRequestDetails()
        {
            var list = new List<InstallationRequest>();
            var data = base.GetAll();
            var customerCollection = base._dataBase.GetCollection<Customer>("Customers");
            var offerCollection = base._dataBase.GetCollection<Offer>("Offers");
            var employeeCollection = base._dataBase.GetCollection<Employee>("Employees");


            foreach (var item in data)
            {
                var filter = Builders<Customer>.Filter.Eq(k => k.Id, item.CustomerId);
                var customer = customerCollection.Find(filter).FirstOrDefault();

                var offerfilter = Builders<Offer>.Filter.Eq(k => k.Id, item.OfferId);
                var offer = offerCollection.Find(offerfilter).FirstOrDefault();


                var Empfilter = Builders<Employee>.Filter.Eq(k => k.Id, item.CustomerId);
                var employee = employeeCollection.Find(Empfilter).FirstOrDefault();
                list?.Add(new InstallationRequest
                {
                    Id = item?.Id,
                    CustomerId = customer?.Name,
                    CreatedAt = item?.CreatedAt ?? DateTime.UtcNow,
                    AssignedEmployeeId = employee?.Id,
                    FinishedAt = item?.CreatedAt ?? DateTime.UtcNow,
                    InstallationNote = item?.InstallationNote,
                    IsAssigned = (bool)(item?.IsAssigned),
                    IsCompleted = (bool)(item?.IsCompleted),
                    LastUpdatedAt = item?.CreatedAt ?? DateTime.UtcNow,
                    OfferId = offer?.OfferTitle,
                });
            }
            return list;
        }
    }
}
