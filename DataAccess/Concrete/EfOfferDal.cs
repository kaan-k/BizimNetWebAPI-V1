using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Offers; // ✅ Use Plural Namespace
using Microsoft.EntityFrameworkCore; // ✅ Needed for .Include()
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfOfferDal : EfEntityRepositoryBase<Offer, BizimNetContext>, IOfferDal
    {
        public List<Offer> GetAllOfferDetails()
        {
            using (var context = new BizimNetContext())
            {
                // ✅ SQL Way: Single Query with Joins
                return context.Offers
                    .Include(o => o.Customer)   // Join Customer Table
                    .Include(o => o.OfferItems) // Join OfferItems Table (List of products)
                    .ToList();
            }
        }

        public List<Offer> GetByStatus(string status)
        {
            using (var context = new BizimNetContext())
            {
                // ✅ SQL Way: Filter is done in the Database, not in Memory
                return context.Offers
                    .Include(o => o.Customer)
                    .Include(o => o.OfferItems)
                    .Where(o => o.Status == status) // Generates "WHERE Status = 'Pending'" SQL
                    .ToList();
            }
        }
    }
}