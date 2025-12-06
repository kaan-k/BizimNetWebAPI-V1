using Core.DataAccess;
using Core.DataAccess.MongoDB;
using Core.Entities.Concrete;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Offers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IAggrementDal : IEntityRepository<Aggrement>
    {
        Aggrement GetAllAgreementDetails(int id);
    }

}
