using Core.DataAccess;
using Entities.Concrete.Offers;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IOfferDal : IEntityRepository<Offer>
    {
        List<Offer> GetAllOfferDetails();
        List<Offer> GetByStatus(string status);
        Offer GetByIdWithDetails(int id);
    }
}
