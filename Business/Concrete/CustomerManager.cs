using AutoMapper;
using Business.Abstract;
using Castle.Core.Resource;
using Core.Enums;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Customer;
using Entities.Concrete.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;
        private readonly IDeviceDal _deviceDal;
        private readonly IMapper _mapper;

        public CustomerManager(ICustomerDal customerDal, IDeviceDal deviceDal, IMapper mapper)
        {
            _customerDal = customerDal;
            _deviceDal = deviceDal;
            _mapper = mapper;
        }
        public IDataResult<Customer> Add(CustomerDto customer)
        {
            var customerDto = _mapper.Map<Customer>(customer);
            _customerDal.Add(customerDto);
            return new SuccessDataResult<Customer>(customerDto,"Müşteri eklendi.");   
        }

        public IResult Delete(string id)
        {
            _customerDal.Delete(id);
            return new SuccessResult("Müşteri silindi.");
        }

        public IDataResult<List<Customer>> GetAll()
        {
            var customers = _customerDal.GetAll().ToList();

            return new SuccessDataResult<List<Customer>>(customers);
        }

        public IDataResult<List<Device>> GetAllDevicesByCustomerId(string id)
        {
            //var deviceIds = _customerDal.GetAll(x => x.Id == id).Select(x => x.DeviceIds);
            var devices = _deviceDal.GetAll(x => x.CustomerId == id).ToList();

            if (!devices.Any())
            {
                return new ErrorDataResult<List<Device>>(devices, "Müşteriye ait cihaz bulunamadı.");
            }
            return new SuccessDataResult<List<Device>>(devices, "Müşteriye ait tüm cihazlar getirildi.");
        }

        public IDataResult<List<Customer>> GetAllFiltered(CustomerStatus status)
        {
            //var query = _customerDal.GetAll().Where(x=>x.Status == status).ToList();
            var query = _customerDal.GetAll().Where(x => true).ToList();

            if (!query.Any())
            {
                return new ErrorDataResult<List<Customer>>(query, "Seçilen statüde müşteri bulunamadı.");
            }

            return new SuccessDataResult<List<Customer>>(query, "Seçilen statüde müşteri bulundu.");
        }

        public IDataResult<Customer> GetById(string id)
        {
            var customer = _customerDal.Get(x=> x.Id == id);

            return new SuccessDataResult<Customer>(customer);
        }

        public IResult GetCustomerCountByStatus(CustomerStatus status)
        {
            //var query = _customerDal.GetAll().Where(x => x.Status == status).ToList().Count;
            var query = _customerDal.GetAll().Where(x => true).ToList().Count;

            return new SuccessResult("Seçilen statüdeki müşteri sayısı: " + query.ToString());
        }

        public IDataResult<List<Customer>> GetCustomersByField(string field)
        {
            var customers = _customerDal.GetAll(x => x.CustomerField.ToLower() == field.ToLower());

            if (!customers.Any())
            {
                return new ErrorDataResult<List<Customer>>(field +" alanında hiçbir müşteri yok.");
            }

            return new SuccessDataResult<List<Customer>>(customers, field +" alanındaki tüm müşteriler getirildi.");

        }

       
        public IResult Update(Customer customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;    
            _customerDal.Update(customer);
            return new SuccessDataResult<Customer>(customer, "Müşteri güncellendi.");
        }
    }
}
