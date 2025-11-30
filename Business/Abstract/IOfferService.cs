using Core.Enums;
using Core.Utilities.Results;
using Entities.Concrete.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOfferService
    {
        IResult Add(OfferDto offer);
        IResult Update(Offer offer);
        IResult Delete(string id);
        IDataResult<string> GenerateOfferReport(OfferDto offer);
        IDataResult<Offer> GetById(string id);
        IDataResult<List<Offer>> GetByStatus(string status);

        IDataResult<List<Offer>> GetAll();
        IDataResult<List<Offer>> GetAllDetails();

        IDataResult<List<Offer>> GetByCustomerId(string customerId);
        IDataResult<List<Offer>> GetByDateRange(DateTime start, DateTime end);
        IResult Approve(string offerId);

    }

}
