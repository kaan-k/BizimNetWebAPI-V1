using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.Concrete.Duties;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfDutyDal : EfEntityRepositoryBase<Duty, BizimNetContext>, IDutyDal
    {
        public EfDutyDal(BizimNetContext context) : base(context)
        {
            // Any specific logic for EfOfferDal can go here, but usually it's left empty.
        }
        public List<Duty> GetAllDutyDetails(int requesterId)
        {
            using (var context = new BizimNetContext())
            {
                // 1. Check who is making the request
                var requester = context.BusinessUsers.Find(requesterId);

                // 2. Prepare the base query with Joins
                var query = context.Duties
                    .Include(d => d.Customer) // Loads Customer data
                                              //.Include(d => d.AssignedUser) // Uncomment if you add this Nav Property later
                    .AsQueryable();

                // 3. Apply the Permission Logic (Replicating your Mongo Logic)
                if (requester.IsAuthorised || requester.FirstName == "Alev")
                {
                    // Admin/Alev sees everything
                    return query.ToList();
                }
                else
                {
                    // Regular users only see duties assigned to them
                    return query.Where(d => d.AssignedEmployeeId == requesterId).ToList();
                }
            }
        }

        public List<Duty> GetAllDutyDetailsPerEmployee(int employeeId)
        {
            using (var context = new BizimNetContext())
            {
                return context.Duties
                    .Include(d => d.Customer)
                    .Where(d => d.AssignedEmployeeId == employeeId)
                    .ToList();
            }
        }

        public List<Duty> GetAllDutyDetailsPerStatus(int employeeId, string status)
        {
            using (var context = new BizimNetContext())
            {
                // Filters by both Status AND Employee (implied by your previous logic)
                return context.Duties
                    .Include(d => d.Customer)
                    .Where(d => d.AssignedEmployeeId == employeeId && d.Status == status)
                    .ToList();
            }
        }
    }
}