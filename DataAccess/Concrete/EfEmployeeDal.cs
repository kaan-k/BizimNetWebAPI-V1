using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Employees; // ✅ Use the fixed Namespace
using System.Collections.Generic;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfEmployeeDal : EfEntityRepositoryBase<Employee, BizimNetContext>, IEmployeeDal
    {
        public EfEmployeeDal(BizimNetContext context) : base(context)
        {
            // Any specific logic for EfOfferDal can go here, but usually it's left empty.
        }
    }
}