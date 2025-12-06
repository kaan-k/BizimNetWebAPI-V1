using Core.Utilities.Results;
using Entities.Concrete.Offers; // ✅ Plural Namespace
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IOfferService
    {
        IResult Add(OfferDto offer);
        IResult Update(Offer offer);

        // ✅ Changed string -> int
        IResult Delete(int id);

        IDataResult<string> GenerateOfferReport(OfferDto offer);

        // ✅ Changed string -> int
        IDataResult<Offer> GetById(int id);

        IDataResult<List<Offer>> GetByStatus(string status);

        IDataResult<List<Offer>> GetAll();
        IDataResult<List<Offer>> GetAllDetails();

        // ✅ Changed string -> int
        IDataResult<List<Offer>> GetByCustomerId(int customerId);

        IDataResult<List<Offer>> GetByDateRange(DateTime start, DateTime end);

        // ✅ Changed string -> int
        IResult Approve(int offerId);
    }
}