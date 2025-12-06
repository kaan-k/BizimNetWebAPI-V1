using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete; // Where BusinessUser is located
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfBusinessUserDal : EfEntityRepositoryBase<BusinessUser, BizimNetContext>, IBusinessUserDal
    {
        public List<BusinessUser> GetAllBusinessUserDetails()
        {
            using (var context = new BizimNetContext())
            {
                // ✅ LINQ Projection: Generates optimized SQL "SELECT Id, Name, Email..."
                var result = from user in context.BusinessUsers // Ensure DbSet is named 'BusinessUsers'
                             select new BusinessUser
                             {
                                 Id = user.Id,
                                 CompanyAddress = user.CompanyAddress,
                                 CompanyName = user.CompanyName,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Email = user.Email
                             };

                return result.ToList();
            }
        }
    }
}