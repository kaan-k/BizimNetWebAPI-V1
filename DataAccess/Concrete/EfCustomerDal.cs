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
        public EfCustomerDal(BizimNetContext context) : base(context)
        {
            // Any specific logic for EfOfferDal can go here, but usually it's left empty.
        }
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



        public bool HasAgreements(int customerId)
        {
            using var context = new BizimNetContext();
            return context.Aggrements.Any(a => a.CustomerId == customerId);
        }

        public bool HasOffers(int customerId)
        {
            using var context = new BizimNetContext();
            return context.Offers.Any(o => o.CustomerId == customerId);
        }
        public bool HasDocuments(int customerId)
        {
            using var context = new BizimNetContext();
            return context.DocumentFiles.Any(o => o.CustomerId == customerId);
        }

        public bool HasBranches(int customerId)
        {
            throw new NotImplementedException();
            //Branch ilişkisi ekleyip geri dönülecek.
            //using var context = new BizimNetContext();
            //return context.Branches.Any(b => b.CustomerId == customerId);
        }

    }
}