using Core.DataAccess;
using Core.DataAccess.MongoDB;
using Entities.Concrete.Payments;
using Entities.Concrete.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IBillingDal: IEntityRepository<Billing>
    {
    }
}
