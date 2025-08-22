using Core.Configuration;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.Device;
using Entities.Concrete.DocumentFile;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class Mongo_DeviceDal: MongoRepository<Device>, IDeviceDal
    {
        public Mongo_DeviceDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
         : base(database, settings.Value.DeviceCollectionName)
        {
        }
        public List<Device> GetAllDeviceDetails()
        {
            var list = new List<Device>();
            var data = base.GetAll();
            var customerCollection = base._dataBase.GetCollection<Customer>("Customers");
            foreach (var item in data)
            {
                var filter = Builders<Customer>.Filter.Eq(k => k.Id, item.CustomerId);
                var customer = customerCollection.Find(filter).FirstOrDefault();
                list?.Add(new Device
                {
                    Id = item?.Id,
                    CustomerId= customer?.CompanyName,
                    Name = item?.Name,
                    AnyDeskId = item?.AnyDeskId,
                    CreatedAt = item.CreatedAt,
                    PublicIp = item?.PublicIp,
                    UpdatedAt= item?.UpdatedAt,
                    DeviceType = item?.DeviceType,
                });
             }
            return list;
        }
    }
}
