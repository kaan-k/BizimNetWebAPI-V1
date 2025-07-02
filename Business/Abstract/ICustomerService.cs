using Core.Enums;
using Core.Utilities.Results;
using Entities.Concrete.Customer;
using Entities.Concrete.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICustomerService
    {
        IDataResult<Customer> Add(CustomerDto customer);
        IResult Update(Customer customer);
        IResult Delete(string id);
        IResult GetCustomerCountByStatus(CustomerStatus status);
        IDataResult<List<Customer>> GetAll();
        IDataResult<List<Customer>> GetAllFiltered(CustomerStatus status);
        IDataResult<List<Customer>> GetCustomersByField(string field);
        IDataResult<Customer> GetById(string id);
        IDataResult<List<Device>> GetAllDevicesByCustomerId(string id);
    }
}
