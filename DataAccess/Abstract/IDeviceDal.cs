using Core.DataAccess.MongoDB;
using Entities.Concrete.Device;
using Entities.Concrete.DocumentFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IDeviceDal: IMongoRepository<Device>
    {

    }
}
