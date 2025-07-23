using Core.Enums;
using Core.Utilities.Results;
using Entities.Concrete.Department;
using Entities.Concrete.Device;
using Entities.Concrete.InstallationRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDeviceService
    {
        IResult Add(DeviceDto request);
        IResult Delete(string id);
        IResult Update(Device request);


        IDataResult <List<Device>>GetByDeviceType(string deviceType);
        IDataResult<List<Device>> GetAllByCustomerId(string id);
        IDataResult<List<Device>> GetAllDetails();
        IDataResult<Device> GetById(string id);
        IDataResult<string> GetNameById(string id);


    }
}
