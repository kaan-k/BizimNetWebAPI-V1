using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.InstallationRequests; // ✅ Use the Plural Namespace
using Microsoft.EntityFrameworkCore; // ✅ Needed for .Include()
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfInstallationRequestDal : EfEntityRepositoryBase<InstallationRequest, BizimNetContext>, IInstallationRequestDal
    {
        public EfInstallationRequestDal(BizimNetContext context) : base(context)
        {
            // Any specific logic for EfOfferDal can go here, but usually it's left empty.
        }
        public List<InstallationRequest> GetAllInstallationRequestDetails()
        {
            using (var context = new BizimNetContext())
            {
                // ✅ SQL Way: Join related tables automatically
                var query = context.InstallationRequests
                    .Include(r => r.Customer); // Always includes Customer

                // ⚠️ IMPORTANT: 
                // Uncomment the lines below ONLY if you have added "virtual Offer Offer" 
                // and "virtual Employee AssignedEmployee" to your InstallationRequest entity.

                // .Include(r => r.Offer) 
                // .Include(r => r.AssignedEmployee) 

                return query.ToList();
            }
        }
    }
}