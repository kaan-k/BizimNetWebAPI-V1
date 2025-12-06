using AutoMapper;
using Business.Abstract;
using Core.Enums;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Customers;
using Entities.Concrete.Devices;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var customerEntity = _mapper.Map<Customer>(customer);
            // SQL generates the ID automatically, so no need to set it manually.
            _customerDal.Add(customerEntity);
            return new SuccessDataResult<Customer>(customerEntity, "Müşteri eklendi.");
        }

        public IResult Delete(int id)
        {
            // EF Core Delete usually requires fetching the entity first or using a stub.
            // Using the overload Delete(int id) we created in the generic repo:
            _customerDal.Delete(id);
            return new SuccessResult("Müşteri silindi.");
        }

        public IDataResult<List<Customer>> GetAll()
        {
            // Use the custom method GetAllDetails() if you want to include ParentCustomer
            var customers = _customerDal.GetAllDetails();
            return new SuccessDataResult<List<Customer>>(customers);
        }

        public IDataResult<List<Device>> GetAllDevicesByCustomerId(int id)
        {
            // ✅ Efficient SQL Query
            var devices = _deviceDal.GetAll(x => x.CustomerId == id);

            if (!devices.Any())
            {
                return new ErrorDataResult<List<Device>>(devices, "Müşteriye ait cihaz bulunamadı.");
            }
            return new SuccessDataResult<List<Device>>(devices, "Müşteriye ait tüm cihazlar getirildi.");
        }

        public IDataResult<List<Customer>> GetAllFiltered(CustomerStatus status)
        {
            // Assuming 'Status' is stored as a string in DB but Enum in C#.
            // Adjust .ToString() if needed.
            var query = _customerDal.GetAll(x => x.Status == status.ToString());

            if (!query.Any())
            {
                return new ErrorDataResult<List<Customer>>(query, "Seçilen statüde müşteri bulunamadı.");
            }
            return new SuccessDataResult<List<Customer>>(query, "Seçilen statüde müşteri bulundu.");
        }

        public IDataResult<List<Customer>> GetBranchesAsync(int parentId)
        {
            var branches = _customerDal.GetAll(x => x.ParentCustomerId == parentId);
            return new SuccessDataResult<List<Customer>>(branches, "Şubeler bulundu.");
        }

        public IDataResult<CustomerDto> GetById(int id)
        {
            var customer = _customerDal.Get(x => x.Id == id);
            if (customer == null)
                return new ErrorDataResult<CustomerDto>("Customer not found");

            var dto = _mapper.Map<CustomerDto>(customer);
            return new SuccessDataResult<CustomerDto>(dto);
        }


        // ✅ New method for searching by name (replaces the ObjectId logic)
        public IDataResult<Customer> GetByCompanyName(string companyName)
        {
            var customer = _customerDal.Get(x => x.CompanyName == companyName);
            if (customer == null) return new ErrorDataResult<Customer>("Müşteri bulunamadı");

            return new SuccessDataResult<Customer>(customer);
        }

        public IResult GetCustomerCountByStatus(CustomerStatus status)
        {
            var count = _customerDal.GetAll(x => x.Status == status.ToString()).Count;
            return new SuccessResult("Seçilen statüdeki müşteri sayısı: " + count);
        }

        public IDataResult<List<Customer>> GetCustomersByField(string field)
        {
            var customers = _customerDal.GetAll(x => x.CustomerField.ToLower() == field.ToLower());

            if (!customers.Any())
            {
                return new ErrorDataResult<List<Customer>>(field + " alanında hiçbir müşteri yok.");
            }
            return new SuccessDataResult<List<Customer>>(customers, field + " alanındaki tüm müşteriler getirildi.");
        }

        public IResult Update(Customer customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;
            _customerDal.Update(customer);
            return new SuccessDataResult<Customer>(customer, "Müşteri güncellendi.");
        }
    }
}