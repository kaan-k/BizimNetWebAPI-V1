using Core.Configuration;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Repositories;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Payments;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfBillingDal : EfEntityRepositoryBase<Billing, BizimNetContext>, IBillingDal
    {
      
    
    }
}
