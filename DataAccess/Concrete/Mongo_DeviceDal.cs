using Core.Configuration;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.Device;
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
    public class Mongo_DeviceDal: MongoRepository<Device>, IDeviceDal
    {
        public Mongo_DeviceDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
         : base(database, settings.Value.DeviceCollectionName)
        {
        }
        public List<Device> GetAll(Expression<Func<Device, bool>> filter = null)
        {
            if (filter == null)
                return _collection.Find(FilterDefinition<Device>.Empty).ToList();

            return _collection.Find(filter).ToList();
        }
    }
}
