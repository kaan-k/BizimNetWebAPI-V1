using Core.Utilities.Results;
using Entities.Concrete.Offers;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IOfferService
    {
        IResult Add(OfferDto offerDto);
        IResult Approve(int offerId);
        IResult Delete(int id);
        IResult Update(Offer offer);

        IDataResult<string> GenerateOfferReport(int offerId);

        IDataResult<List<Offer>> GetAll();
        IDataResult<List<Offer>> GetAllDetails();
        IDataResult<List<Offer>> GetByCustomerId(int customerId);
        IDataResult<List<Offer>> GetByStatus(string status);
        IDataResult<List<Offer>> GetByDateRange(DateTime start, DateTime end);

        IDataResult<OfferDto> GetById(int id);

    }
}
