using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Aggrements;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class AgreementManager : IAggrementService
    {
        private readonly IMapper _mapper;
        private readonly IAggrementDal _agreementDal; // ✅ Make sure DAL interface is renamed too
        private readonly IOfferDal _offerDal;

        public AgreementManager(IMapper mapper, IAggrementDal agreementDal, IOfferDal offerDal)
        {
            _mapper = mapper;
            _agreementDal = agreementDal;
            _offerDal = offerDal;
        }

        public IDataResult<Aggrement> Add(AggrementDto agreementDto)
        {
            var agreement = _mapper.Map<Aggrement>(agreementDto);

            // ⚠️ SQL Difference: Do NOT set agreement.Id here.
            // The database will generate the int ID automatically.

            _agreementDal.Add(agreement);

            // After .Add(), 'agreement.Id' will be populated with the new Integer ID
            return new SuccessDataResult<   Aggrement>(agreement);
        }

        public IResult Delete(int id)
        {
            // You might need to check if it exists first, depending on your DAL implementation
            var agreement = _agreementDal.Get(a => a.Id == id);
            if (agreement == null) return new ErrorResult("Sözleşme bulunamadı");

            _agreementDal.Delete(agreement); // EF Core usually deletes by Entity, not just ID
            return new SuccessResult();
        }

        public IResult Update(Aggrement agreement)
        {
            _agreementDal.Update(agreement);
            return new SuccessResult();
        }

        public IDataResult<Aggrement> GetById(int id)
        {
            var agreement = _agreementDal.Get(x => x.Id == id);
            return new SuccessDataResult<Aggrement>(agreement);
        }

        public IDataResult<List<Aggrement>> GetAll()
        {
            // Assuming GetAllAgreementDetails includes related entities like Customer
            var agreements = _agreementDal.GetAll(null);
            return new SuccessDataResult<List<Aggrement>>(agreements);
        }

        public IDataResult<Aggrement> ReceiveBill(int agreementId, decimal amount)
        {
            var agreement = _agreementDal.Get(a => a.Id == agreementId);
            if (agreement == null)
            {
                return new ErrorDataResult<Aggrement>("Sözleşme bulunamadı.");
            }

            // Logic: Update the calculated amount
            agreement.PaidAmount += amount;

            _agreementDal.Update(agreement);
            return new SuccessDataResult<Aggrement>(agreement, "Ödeme alındı ve sözleşme güncellendi.");
        }

        public IResult RegisterPayment(int agreementId, decimal amount)
        {
            // 1. Get the Agreement
            var agreement = _agreementDal.Get(a => a.Id == agreementId);
            if (agreement == null) return new ErrorResult("Sözleşme bulunamadı.");

            // 2. Update the Paid Amount
            agreement.PaidAmount += amount;

            // ⚠️ SQL Difference: 
            // We DO NOT add billingId to a list here. 
            // The Billing Entity itself holds the 'AgreementId'. 
            // The relationship is already established when the Billing was created.

            // 3. Update Database
            _agreementDal.Update(agreement);

            return new SuccessResult();
        }

        public IResult CancelPayment(int agreementId, decimal amount)
        {
            var agreement = _agreementDal.Get(a => a.Id == agreementId);
            if (agreement == null) return new ErrorResult("İlgili sözleşme bulunamadı.");

            // 1. Reverse the math
            agreement.PaidAmount -= amount;

            // Safety check
            if (agreement.PaidAmount < 0) agreement.PaidAmount = 0;

            // ⚠️ SQL Difference: No need to remove billingId from a list.

            _agreementDal.Update(agreement);

            return new SuccessResult();
        }

        public IResult CreateAgreementFromOffer(int offerId)
        {
            // 1. Fetch the Approved Offer
            var offer = _offerDal.Get(o => o.Id == offerId);
            if (offer == null) return new ErrorResult("Teklif bulunamadı.");

            if (offer.Status != "Approved") return new ErrorResult("Sadece onaylanmış teklifler sözleşmeye dönüştürülebilir.");

            // 2. Check if agreement already exists (By OfferId)
            var existing = _agreementDal.Get(a => a.OfferId == offerId);
            if (existing != null) return new ErrorResult("Bu teklif için zaten bir sözleşme var.");

            // 3. Map Offer Data to Agreement
            var agreement = new Aggrement
            {
                // Id = ... ❌ REMOVED. Let SQL handle auto-increment.

                OfferId = offerId,
                CustomerId = offer.CustomerId,
                AgreementTitle = offer.OfferTitle, // Fixed spelling
                AgreementType = "Sales",
                AgreedAmount = offer.TotalAmount, // decimal
                PaidAmount = 0,

                // billings = ... ❌ REMOVED. EF Core manages this via Navigation Property.

                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddYears(1)
            };

            // 4. Update Offer Status
            offer.Status = "Agreement";
            _offerDal.Update(offer);

            // 5. Save Agreement
            _agreementDal.Add(agreement);

            return new SuccessResult("Teklif başarıyla sözleşmeye dönüştürüldü.");
        }
    }
}