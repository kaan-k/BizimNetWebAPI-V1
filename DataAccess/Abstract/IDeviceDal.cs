using Core.DataAccess;
using Entities.Concrete.Devices;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    // 1. Inherit from IEntityRepository, not IMongoRepository
    public interface IDeviceDal : IEntityRepository<Device>
    {
        List<Device> GetAllDeviceDetails();
    }
}