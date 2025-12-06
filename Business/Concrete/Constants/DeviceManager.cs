using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Devices; // ✅ Plural Namespace
using System;
using System.Collections.Generic;

namespace Business.Concrete // ✅ Fixed Namespace (was .Constants)
{
    public class DeviceManager : IDeviceService
    {
        private readonly IMapper _mapper;
        private readonly IDeviceDal _deviceDal;

        public DeviceManager(IDeviceDal deviceDal, IMapper mapper)
        {
            _deviceDal = deviceDal;
            _mapper = mapper;
        }

        public IResult Add(Device request)
        {
            var mappedRequest = _mapper.Map<Device>(request);

            // Optional: Set creation date if not handled in DTO/Mapper
            mappedRequest.CreatedAt = DateTime.UtcNow;

            _deviceDal.Add(mappedRequest);
            return new SuccessResult("Cihaz eklendi.");
        }

        public IResult Delete(int id)
        {
            // Optional: Check if exists
            var device = _deviceDal.Get(x => x.Id == id);
            if (device == null) return new ErrorResult("Cihaz bulunamadı.");

            _deviceDal.Delete(id);
            return new SuccessResult("Cihaz silindi.");
        }

        public IDataResult<List<Device>> GetAllByCustomerId(int id)
        {
            var devices = _deviceDal.GetAll(x => x.CustomerId == id);
            return new SuccessDataResult<List<Device>>(devices);
        }

        public IDataResult<List<Device>> GetAllDetails()
        {
            // Uses the .Include() method we wrote in EfDeviceDal
            return new SuccessDataResult<List<Device>>(_deviceDal.GetAllDeviceDetails());
        }

        public IDataResult<List<Device>> GetByDeviceType(string deviceType)
        {
            var devices = _deviceDal.GetAll(x => x.DeviceType == deviceType);
            return new SuccessDataResult<List<Device>>(devices);
        }

        public IDataResult<Device> GetById(int id)
        {
            var device = _deviceDal.Get(x => x.Id == id);
            if (device == null) return new ErrorDataResult<Device>("Cihaz bulunamadı.");

            return new SuccessDataResult<Device>(device);
        }

        public IResult Update(Device request)
        {
            // 1. Fetch existing record to ensure it exists
            var existingDevice = _deviceDal.Get(x => x.Id == request.Id);

            if (existingDevice == null)
            {
                return new ErrorResult("Güncellenecek cihaz bulunamadı.");
            }

            // 2. Update properties
            existingDevice.Name = request.Name;
            existingDevice.DeviceType = request.DeviceType;
            existingDevice.CustomerId = request.CustomerId;
            existingDevice.AnyDeskId = request.AnyDeskId;
            existingDevice.PublicIp = request.PublicIp;
            existingDevice.UpdatedAt = DateTime.UtcNow; // ✅ Use UtcNow

            // 3. Save
            _deviceDal.Update(existingDevice);

            return new SuccessResult("Cihaz güncellendi.");
        }
    }
}