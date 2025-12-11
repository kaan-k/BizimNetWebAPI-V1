using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfSectionDal: EfEntityRepositoryBase<Section, BizimNetContext>, ISectionDal
    {
        public EfSectionDal(BizimNetContext context) : base(context)
        {
        }
    }
}
