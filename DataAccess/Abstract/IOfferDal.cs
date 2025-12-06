using Core.DataAccess; // ✅ Where IEntityRepository is located
using Entities.Concrete.Offers; // ✅ Use the Plural Namespace
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IOfferDal : IEntityRepository<Offer>
    {
        List<Offer> GetAllOfferDetails();
        List<Offer> GetByStatus(string status);
    }
}