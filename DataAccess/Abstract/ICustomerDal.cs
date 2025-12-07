using Core.DataAccess;
using Entities.Concrete.Customers;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    // 1. Inherit from IEntityRepository, not IMongoRepository
    public interface ICustomerDal : IEntityRepository<Customer>
    {
        List<Customer> GetAllDetails();


        bool HasAgreements(int customerId);
        bool HasOffers(int customerId);
        bool HasBranches(int customerId);
        bool HasDocuments(int customerId);

    }
}