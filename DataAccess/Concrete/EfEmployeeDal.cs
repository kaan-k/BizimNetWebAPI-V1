using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Employees; // ✅ Use the fixed Namespace
using System.Collections.Generic;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfEmployeeDal : EfEntityRepositoryBase<Employee, BizimNetContext>, IEmployeeDal
    {
        // No constructor needed. Base class handles the Context.
    }
}