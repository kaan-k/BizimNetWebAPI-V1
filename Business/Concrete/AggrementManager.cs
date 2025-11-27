using AutoMapper;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AggrementManager : IAggrementService
    {
        private readonly IMapper _mapper;
        private readonly IAggrementDal _aggrementDal;
        public AggrementManager(IMapper mapper, IAggrementDal aggrementDal) {
            _mapper = mapper;
            _aggrementDal = aggrementDal;
        }
        public IDataResult<Aggrement> Add(AggrementDto aggrement)
        {
            var aggrementToAdd = _mapper.Map<Aggrement>(aggrement);
            _aggrementDal.Add(aggrementToAdd);
            return new SuccessDataResult<Aggrement>(aggrementToAdd);

        }

        public IResult Delete(string id)
        {
            _aggrementDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<Aggrement> RecieveBill(string aggrementId, int amount)
        {
            var aggrement = _aggrementDal.Get(a => a.Id == aggrementId);
            if (aggrement == null)
            {
                return new ErrorDataResult<Aggrement>("Sözleşme bulunamadı.");
            }
            _aggrementDal.Update(aggrement);
            return new SuccessDataResult<Aggrement>(aggrement, "Ödeme alındı ve sözleşme güncellendi.");
        }

        public IDataResult<List<Aggrement>> GetAll()
        {
            var aggrements = _aggrementDal.GetAllAggrementDetails();
            return new SuccessDataResult<List<Aggrement>>(aggrements);
        }

        public IDataResult<Aggrement> GetById(string id)
        {
            var aggrement = _aggrementDal.Get(x => x.Id == id);
            return new SuccessDataResult<Aggrement>(aggrement);
        }

        public IResult Update(Aggrement aggrement, string id)
        {
            throw new NotImplementedException();
        }

        public IResult RegisterPayment(string agreementId, string billingId, int amount)
        {
            // 1. Get the Agreement
            var agreement = _aggrementDal.Get(a => a.Id == agreementId);
            if (agreement == null)
            {
                return new ErrorResult("Sözleşme bulunamadı.");
            }

            // 2. Initialize list if it's null (MongoDB specific safety)
            if (agreement.billings == null)
            {
                agreement.billings = new List<string>();
            }

            // 3. Add the Billing ID
            agreement.billings.Add(billingId);

            // 4. Update the Paid Amount
            agreement.PaidAmount += amount;

            // 5. Check if fully paid (Optional logic)
            // if (agreement.PaidAmount >= agreement.AgreedAmount) ...

            // 6. Update Database
            _aggrementDal.Update(agreement);

            return new SuccessResult();
        }

        public IResult CancelPayment(string agreementId, string billingId, int amount)
        {
            var agreement = _aggrementDal.Get(a => a.Id == agreementId);
            if (agreement == null)
            {
                return new ErrorResult("İlgili sözleşme bulunamadı.");
            }

            // 1. Remove the Billing ID from the list
            if (agreement.billings != null)
            {
                agreement.billings.Remove(billingId);
            }

            // 2. Subtract the amount from PaidAmount (Reverse the math)
            agreement.PaidAmount -= amount;

            // Safety check: Prevent negative numbers if data was out of sync
            if (agreement.PaidAmount < 0)
            {
                agreement.PaidAmount = 0;
            }

            // 3. Update Database
            _aggrementDal.Update(agreement);

            return new SuccessResult();
        }
    }
}
