using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Services; // ✅ Use the Plural Namespace we fixed
using Microsoft.EntityFrameworkCore; // ✅ Necessary for .Include()
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfServicingDal : EfEntityRepositoryBase<Servicing, BizimNetContext>, IServicingDal
    {
        public EfServicingDal(BizimNetContext context) : base(context)
        {
            // Any specific logic for EfOfferDal can go here, but usually it's left empty.
        }
        public List<Servicing> GetAllServicingDetails()
        {
            using (var context = new BizimNetContext())
            {
                // ✅ SQL Way: No "foreach" loop needed.
                // .Include() performs a LEFT JOIN automatically to fetch Customer data.
                return context.Services // Make sure your DbSet in Context is named 'Servicings' or 'Services'
                    .Include(s => s.Customer)
                    .ToList();
            }
        }
    }
}