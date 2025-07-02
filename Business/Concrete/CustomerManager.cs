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

        public CustomerManager(ICustomerDal customerDal, IDeviceDal deviceDal)
        {
            _customerDal = customerDal;
            _deviceDal = deviceDal;
        }
        public IDataResult<Customer> Add(CustomerDto customer)
        {
            var customerDto = new Customer
            {
                Name = customer.Name,
                CompanyName = customer.CompanyName,
                CustomerField = customer.CustomerField,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                City = customer.City,
                Country = customer.Country,
                Status = customer.Status,
                LastActionDate = customer.LastActionDate,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt,
            };

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
            var query = _customerDal.GetAll().Where(x=>x.Status == status).ToList();

            if(!query.Any())
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
            var query = _customerDal.GetAll().Where(x => x.Status == status).ToList().Count;

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
            var customerToUpdate = _customerDal.Get(x=> x.Id == customer.Id);

            customerToUpdate.UpdatedAt = DateTime.UtcNow;    
            _customerDal.Update(customer);
            _customerDal.Update(customerToUpdate);
            return new SuccessDataResult<Customer>(customer, "Müşteri güncellendi.");
        }
    }
}
