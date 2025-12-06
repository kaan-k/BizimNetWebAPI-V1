using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Departments; // ✅ Use the fixed Plural Namespace
using System.Collections.Generic;

namespace DataAccess.Concrete.EntityFramework
{
    // Inherit from the generic EF Repository
    public class EfDepartmentDal : EfEntityRepositoryBase<Department, BizimNetContext>, IDepartmentDal
    {
        // No constructor needed; the base class handles the context creation internally.
    }
}