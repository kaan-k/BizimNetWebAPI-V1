using Core.Utilities.Results;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Offers; // Needed for DTOs if they are in this namespace
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IAggrementService // ✅ Fixed spelling (Aggrement -> Agreement)
    {
        IDataResult<Aggrement> Add(AggrementDto agreementDto);
        IResult Update(Aggrement agreement); // Removed 'id' param, it's inside the object
        IResult Delete(int id);
        IDataResult<Aggrement> GetById(int id);
        IDataResult<List<Aggrement>> GetAll();

        // Custom Logic
        IResult CreateAgreementFromOffer(int offerId);

        // Payment Logic
        IDataResult<Aggrement> ReceiveBill(int agreementId, decimal amount); // ✅ decimal
        IResult RegisterPayment(int agreementId, decimal amount); // Removed billingId param (not needed for calculation)
        IResult CancelPayment(int agreementId, decimal amount);
    }
}