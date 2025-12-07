using Core.Utilities.Results;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Offers; // Needed for DTOs if they are in this namespace
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IAggrementService 
    {
        IDataResult<Aggrement> Add(AggrementDto agreementDto);
        IResult Update(Aggrement agreement);
        IResult Delete(int id);
        IDataResult<AggrementDto> GetById(int id);
        IDataResult<List<Aggrement>> GetAll();

        IResult CreateAgreementFromOffer(int offerId);


        IDataResult<Aggrement> ReceiveBill(int agreementId, decimal amount);
        IResult RegisterPayment(int agreementId, decimal amount);
        IResult CancelPayment(int agreementId, decimal amount);
    }
}