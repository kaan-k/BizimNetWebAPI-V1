using AutoMapper;
using Business.Abstract;
using Core.Enums;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete.Constants
{
    public class DeviceManager : IDeviceService
    {

        private readonly IMapper _mapper;
        private readonly IDeviceDal _deviceDal;
        public DeviceManager(IDeviceDal deviceDal, IMapper mapper) {
        
            _deviceDal = deviceDal;
            _mapper = mapper;
        }

        public IResult Add(DeviceDto request)
        {
            var mappedRequest = _mapper.Map<Device>(request);
            _deviceDal.Add(mappedRequest);
            return new SuccessResult();

        }

        public IResult Delete(string id)
        {
            _deviceDal.Delete(id);
            return new SuccessResult();

        }

        public IDataResult<List<Device>> GetByDeviceType(DeviceType deviceType)
        {
            var devices = _deviceDal.GetAll(x=> x.DeviceType == deviceType);

            return new SuccessDataResult<List<Device>>(devices);
        }

        public IResult Update(DeviceDto request)
        {
            throw new NotImplementedException();
        }
    }
}
