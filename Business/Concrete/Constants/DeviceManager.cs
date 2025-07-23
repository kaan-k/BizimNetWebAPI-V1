using AutoMapper;
using Business.Abstract;
using Core.Enums;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Device;
using Entities.Concrete.Offer;
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

        public IDataResult<List<Device>> GetAllDetails()
        {
            return new SuccessDataResult<List<Device  >>(_deviceDal.GetAllDeviceDetails());
        }

        public IDataResult<List<Device>> GetByDeviceType(string deviceType)
        {
            var devices = _deviceDal.GetAll(x=> x.DeviceType == deviceType);

            return new SuccessDataResult<List<Device>>(devices);
        }
        public IDataResult<List<Device>> GetAllByCustomerId(string id)
        {
            var devices = _deviceDal.GetAll(x => x.CustomerId == id);

            return new SuccessDataResult<List<Device>>(devices);
        }

        public IDataResult<Device> GetById(string id)
        {
            return new SuccessDataResult<Device>(_deviceDal.Get(x => x.Id == id));
        }

        public IResult Update(Device request)
        {
            request.UpdatedAt = DateTime.Now;
            _deviceDal.Update(request);
            return new SuccessResult();
        }

        public IDataResult<string> GetNameById(string id)
        {
            var name = _deviceDal.Get(x => x.Id == id);
            if(name != null)
            {
                return new SuccessDataResult<string>(name.Name);
            }

            return new ErrorDataResult<string>();

        }
    }
}
