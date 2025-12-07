using Core.DataAccess.EntityFramework;
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
        }

        // ---------------------------
        // ALL DUTIES (With Permission Logic)
        // ---------------------------
        public List<Duty> GetAllDutyDetails(int requesterId)
        {
            using var context = new BizimNetContext();

            var requester = context.BusinessUsers.FirstOrDefault(x => x.Id == requesterId);

            var query = context.Duties
                .Include(d => d.Customer)
                .Include(d => d.AssignedEmployee)   // 🔥 EMPLOYEE NAME BURADAN GELECEK
                .AsQueryable();

            // Admin -> tüm görevler
            if (requester != null && (requester.IsAuthorised || requester.FirstName == "Alev"))
            {
                return query.ToList();
            }

            // Normal kullanıcı -> sadece kendine atanmış görevler
            return query
                .Where(d => d.AssignedEmployeeId == requesterId)
                .ToList();
        }

        // ---------------------------
        // SPECIFIC EMPLOYEE DUTIES
        // ---------------------------
        public List<Duty> GetAllDutyDetailsPerEmployee(int employeeId)
        {
            using var context = new BizimNetContext();

            return context.Duties
                .Include(d => d.Customer)
                .Include(d => d.AssignedEmployee)
                .Where(d => d.AssignedEmployeeId == employeeId)
                .ToList();
        }

        // ---------------------------
        // BY STATUS
        // ---------------------------
        public List<Duty> GetAllDutyDetailsPerStatus(int employeeId, string status)
        {
            using var context = new BizimNetContext();

            return context.Duties
                .Include(d => d.Customer)
                .Include(d => d.AssignedEmployee)
                .Where(d => d.AssignedEmployeeId == employeeId && d.Status == status)
                .ToList();
        }
    }
}
