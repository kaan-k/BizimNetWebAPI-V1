using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BillingManager : IBillingService
    {
        private readonly IMapper _mapper;
        private readonly IBillingDal _billingDal;
        private readonly IAggrementService _aggrementService;
        public BillingManager(IMapper mapper, IBillingDal billingDal, IAggrementService aggrementService)
        {
            _mapper = mapper;
            _billingDal = billingDal;
            _aggrementService = aggrementService;
        }
        public IDataResult<Billing> Add(BillingDto billingDto)
        {
            var billingEntity = _mapper.Map<Billing>(billingDto);

            billingEntity.BillingDate = DateTime.Now;

            _billingDal.Add(billingEntity);
            var updateResult = _aggrementService.RegisterPayment(
                billingEntity.Id,
                billingEntity.Amount
            );

            if (!updateResult.Success)
            {
                return new SuccessDataResult<Billing>(billingEntity, "Ödeme alındı fakat sözleşme güncellenemedi.");
            }

            return new SuccessDataResult<Billing>(billingEntity, "Ödeme bilgisi eklendi ve sözleşme güncellendi.");
        }



        public IResult Delete(int id)
        {
            // 1. Get the billing record BEFORE deleting it
            // We need to know the Amount and AgreementId to reverse the transaction
            var billing = _billingDal.Get(b => b.Id == id);

            if (billing == null)
            {
                return new ErrorResult("Silinecek ödeme kaydı bulunamadı.");
            }

            // 2. Call Agreement Service to revert the balance
            var result = _aggrementService.CancelPayment(billing.AgreementId, billing.Amount);

            if (!result.Success)
            {
                // If we can't update the agreement, we shouldn't delete the bill 
                // to prevent data inconsistency.
                return new ErrorResult("Sözleşme güncellenemediği için silme işlemi durduruldu.");
            }

            // 3. If Agreement updated successfully, delete the billing record
            _billingDal.Delete(id); // Assuming your DAL Delete takes an ID, or pass the object 'billing'

            return new SuccessResult("Ödeme bilgisi silindi ve sözleşme bakiyesi güncellendi.");
        }

        public IDataResult<List<Billing>> GetAll()
        {
            var billings = _billingDal.GetAll().ToList();
            return new SuccessDataResult<List<Billing>>(billings);
        }

        public IDataResult<Billing> GetById(int id)
        {
            var billing = _billingDal.Get(b => b.Id == id);
            return new SuccessDataResult<Billing>(billing);
        }

        public IDataResult<Billing> RecievePay(int billId,int amount)
        {
            var bill = _billingDal.Get(b => b.Id == billId);
            if (bill == null)
            {
                return new ErrorDataResult<Billing>("Fatura bulunamadı.");
            }
            if (bill.Amount != amount)
            {
                return new ErrorDataResult<Billing>("Ödeme tutarı fatura tutarı ile eşleşmiyor.");
            }

            bill.PaidAmount += amount;
            bill.PaymentDate = DateTime.Now;
            _billingDal.Update(bill);

            //_aggrementService.RecieveBill(bill.AggreementId, amount);
            return new SuccessDataResult<Billing>(bill, "Ödeme alındı.");

        }

        public IResult Update(Billing billing)
        {
            _billingDal.Update(billing);
            return new SuccessResult("Ödeme bilgisi güncellendi.");
        }
    }
}
