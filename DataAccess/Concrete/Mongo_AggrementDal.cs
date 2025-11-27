using Core.Configuration;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Customer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class Mongo_AggrementDal: MongoRepository<Aggrement>, IAggrementDal
    {
        public Mongo_AggrementDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
            : base(database, settings.Value.AggrementsCollectionName)
        {
        }

        public List<Aggrement> GetAllAggrementDetails()
        {
            var list = new List<Aggrement>();
            // 1. Get all raw agreements
            var data = base.GetAll();

            // 2. Get connection to Customers collection
            var customerCollection = base._dataBase.GetCollection<Customer>("Customers");

            foreach (var item in data)
            {
                // 3. Find the Customer corresponding to the ID in the agreement
                // Added Trim() safety just like your reference code
                var filter = Builders<Customer>.Filter.Eq(k => k.Id, item.CustomerId != null ? item.CustomerId.Trim() : "");
                var customer = customerCollection.Find(filter).FirstOrDefault();

                // 4. Create the new object for the View, swapping ID for Name
                list.Add(new Aggrement
                {
                    Id = item.Id,
                    AggrementTitle = item.AggrementTitle,

                    // --- THE MAGIC SWITCH ---
                    // Instead of showing "65b2...", we show "Coca Cola"
                    CustomerId = customer?.CompanyName ?? "Bilinmeyen Müşteri",
                    // ------------------------

                    AggrementType = item.AggrementType,
                    AgreedAmount = item.AgreedAmount,
                    PaidAmount = item.PaidAmount,
                    billings = item.billings,
                    ExpirationDate = item.ExpirationDate,
                    isActive = item.isActive

                    // If your Agreement entity inherits from a base that has CreatedAt, add it here:
                    // CreatedAt = item.CreatedAt
                });
            }

            return list;
        }
    }
}
