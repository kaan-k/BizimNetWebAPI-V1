using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfOrderDal : EfEntityRepositoryBase<Order, BizimNetContext>, IOrderDal
    {
        public EfOrderDal(BizimNetContext context) : base(context)
        {
        }
    
    }
}
