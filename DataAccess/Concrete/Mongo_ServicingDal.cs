using Core.Configuration;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.Device;
using Entities.Concrete.Offer;
using Entities.Concrete.Service;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Servicing> GetAllServicingDetails()
        {
            var list = new List<Servicing>();
            var data = base.GetAll();
            var customerCollection = base._dataBase.GetCollection<Customer>("Customers");
            foreach (var item in data)
            {
                var filter = Builders<Customer>.Filter.Eq(k => k.Id, item.CustomerId);
                var customer = customerCollection.Find(filter).FirstOrDefault();
                list?.Add(new Servicing
                {
                    Id = item?.Id,
                    CustomerId = customer?.Name,
                    Name = item?.Name,
                    CreatedAt=item.CreatedAt,
                    DeviceIds=item?.DeviceIds,
                    LastAction=item?.LastAction,
                    LastActionDate=item?.LastActionDate,
                    Status=item?.Status,
                    TrackingId=item?.TrackingId,
                    UpdatedAt= item?.UpdatedAt
                });
            }
            return list;
        }
    }
}
