using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfTableDal: EfEntityRepositoryBase<Table, BizimNetContext>, ITableDal
    {
        public EfTableDal(BizimNetContext context) : base(context)
        {
        }
    }
}
