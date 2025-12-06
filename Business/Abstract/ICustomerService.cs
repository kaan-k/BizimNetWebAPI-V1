using Core.Enums;
using Core.Utilities.Results;
using Entities.Concrete.Customers; // ✅ Plural Namespace
using Entities.Concrete.Devices;   // ✅ Plural Namespace
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ICustomerService
    {
        IDataResult<Customer> Add(CustomerDto customer);
        IResult Update(Customer customer);

        // ✅ Changed string -> int
        IResult Delete(int id);

        IResult GetCustomerCountByStatus(CustomerStatus status);
        IDataResult<List<Customer>> GetAll();
        IDataResult<List<Customer>> GetAllFiltered(CustomerStatus status);
        IDataResult<List<Customer>> GetCustomersByField(string field);

        // ✅ Changed string -> int
        IDataResult<Customer> GetById(int id);
        IDataResult<List<Device>> GetAllDevicesByCustomerId(int id);
        IDataResult<List<Customer>> GetBranchesAsync(int parentId);

        // ✅ New Method: Since GetById(int) can't handle names anymore, add this:
        IDataResult<Customer> GetByCompanyName(string companyName);
    }
}