using Core.Utilities.Results;
using Entities.Concrete.Devices; // ✅ Plural Namespace
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IDeviceService
    {
        IResult Add(Device request);

        // ✅ Changed string -> int
        IResult Delete(int id);

        IResult Update(Device request);

        IDataResult<List<Device>> GetByDeviceType(string deviceType);

        // ✅ Changed string -> int
        IDataResult<List<Device>> GetAllByCustomerId(int id);

        IDataResult<List<Device>> GetAllDetails();

        // ✅ Changed string -> int
        IDataResult<Device> GetById(int id);
    }
}