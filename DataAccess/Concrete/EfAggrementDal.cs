using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Aggrements;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    // 1. Inherit correctly: <Entity, Context>
    public class EfAggrementDal : EfEntityRepositoryBase<Aggrement, BizimNetContext>, IAggrementDal
    {
        

        public List<Aggrement> GetAllAgreementDetails()
        {
            using (var context = new BizimNetContext())
            {
                return context.Aggrements
                    .Include(a => a.CustomerId)
                    .ToList();
            }
        }
    }
}