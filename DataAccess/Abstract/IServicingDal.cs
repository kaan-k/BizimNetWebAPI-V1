using Core.DataAccess;
using Entities.Concrete.Services;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IServicingDal : IEntityRepository<Servicing>
    {

        List<Servicing> GetAllServicingDetails();
    }
}