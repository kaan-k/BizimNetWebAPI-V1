using Core.DataAccess;
using Entities.Concrete.Customers;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    // 1. Inherit from IEntityRepository, not IMongoRepository
    public interface ICustomerDal : IEntityRepository<Customer>
    {
        List<Customer> GetAllDetails();
    }
}