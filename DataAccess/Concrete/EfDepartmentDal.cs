using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Departments; // ✅ Use the fixed Plural Namespace
using System.Collections.Generic;

namespace DataAccess.Concrete.EntityFramework
{
    // Inherit from the generic EF Repository
    public class EfDepartmentDal : EfEntityRepositoryBase<Department, BizimNetContext>, IDepartmentDal
    {
        public EfDepartmentDal(BizimNetContext context) : base(context)
        {
            // Any specific logic for EfOfferDal can go here, but usually it's left empty.
        }
    }
}