using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Customers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCustomerDal : EfEntityRepositoryBase<Customer, BizimNetContext>, ICustomerDal
    {
        public List<Customer> GetAllDetails()
        {
            using (var context = new BizimNetContext())
            {
                // ✅ Eager Loading:
                // This fetches the Customer AND their "Parent Customer" (HQ) in one query.
                // If you want to load Devices too, add: .Include(c => c.Devices)

                return context.Customers // Ensure DbSet is named 'Customers' in Context
                    .Include(c => c.ParentCustomer)
                    .ToList();
            }
        }
    }
}