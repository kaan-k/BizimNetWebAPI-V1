using Core.DataAccess;
using Entities.Concrete.Duties;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IDutyDal : IEntityRepository<Duty>
    {
        // ✅ Changed string IDs to int
        List<Duty> GetAllDutyDetails(int requesterId);
        List<Duty> GetAllDutyDetailsPerEmployee(int employeeId);
        List<Duty> GetAllDutyDetailsPerStatus(int employeeId, string status);
    }
}