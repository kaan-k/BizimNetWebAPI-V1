using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Aggrements;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class AgreementManager : IAggrementService
    {
        private readonly IMapper _mapper;
        private readonly IAggrementDal _agreementDal;
        private readonly IOfferDal _offerDal;
        private readonly BizimNetContext _context;

        public AgreementManager(IMapper mapper, IAggrementDal agreementDal, IOfferDal offerDal, BizimNetContext context)
        {
            _mapper = mapper;
            _agreementDal = agreementDal;
            _offerDal = offerDal;
            _context = context;
        }

        public IDataResult<Aggrement> Add(AggrementDto agreementDto)
        {
            var agreement = _mapper.Map<Aggrement>(agreementDto);

            _agreementDal.Add(agreement); // SaveChanges içeride

            return new SuccessDataResult<Aggrement>(agreement);
        }

        public IResult Delete(int id)
        {
            // 1. Sözleşmeyi bul
            var agreement = _agreementDal.Get(a => a.Id == id);
            if (agreement == null)
                return new ErrorResult("Sözleşme bulunamadı");

            // 2. Bu sözleşmeye bağlı Billing var mı?
            bool hasBilling = _context.Billings.Any(b => b.AgreementId == id);

            if (hasBilling)
                return new ErrorResult("Bu sözleşmeye bağlı ödemeler mevcut, silinemez!");

            // 3. Sil
            _agreementDal.Delete(agreement);

            return new SuccessResult("Sözleşme başarıyla silindi.");
        }


        public IResult Update(Aggrement agreement)
        {
            _agreementDal.Update(agreement);
            return new SuccessResult();
        }

        public IDataResult<AggrementDto> GetById(int id)
        {
            var agreement = _agreementDal.GetAllAgreementDetails(id);

            if (agreement == null)
                return new ErrorDataResult<AggrementDto>("Agreement not found");

            var dto = _mapper.Map<AggrementDto>(agreement);

            return new SuccessDataResult<AggrementDto>(dto);
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
                return new ErrorDataResult<Aggrement>("Sözleşme bulunamadı.");

            agreement.PaidAmount ??= 0;
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
            var offer = _offerDal.Get(o => o.Id == offerId);
            if (offer == null)
                return new ErrorResult("Teklif bulunamadı.");

            if (offer.Status != "Approved")
                return new ErrorResult("Sadece onaylanmış teklifler sözleşmeye dönüştürülebilir.");

            var existing = _agreementDal.Get(a => a.OfferId == offerId);
            if (existing != null)
                return new ErrorResult("Bu teklif için zaten bir sözleşme var.");

            var agreement = new Aggrement
            {
                OfferId = offerId,
                CustomerId = offer.CustomerId,
                AgreementTitle = offer.OfferTitle,
                AgreementType = "Sales",
                AgreedAmount = offer.TotalAmount,
                PaidAmount = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddYears(1)
            };

            offer.Status = "Agreement";

            _offerDal.Update(offer);     // SaveChanges içeride (OfferDAL)
            _agreementDal.Add(agreement); // SaveChanges içeride (AggrementDAL)

            return new SuccessResult("Teklif başarıyla sözleşmeye dönüştürüldü.");
        }

    }
}