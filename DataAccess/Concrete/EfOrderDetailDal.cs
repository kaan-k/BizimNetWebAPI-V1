using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfOrderDetailDal : EfEntityRepositoryBase<OrderDetail, BizimNetContext>, IOrderDetailDal
    {
        public EfOrderDetailDal(BizimNetContext context) : base(context)
        {
        }

    }
}
