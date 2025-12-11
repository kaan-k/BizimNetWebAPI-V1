using Core.DataAccess;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IOrderDal: IEntityRepository<Order>
    {
    }
}
