using Core.DataAccess.MongoDB;
using Core.Entities.Concrete;
using Entities.Concrete.Duty;
using Entities.Concrete.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IDutyDal: IMongoRepository<Duty>
    {
        public List<Duty> GetAllDutyDetails();

    }
}
