using AutoMapper;
using Business.Abstract;
using Business.Concrete.Constants;
using Core.Entities.Concrete;
using Core.Enums;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.InstallationRequest;
using Entities.Concrete.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OfferManager : IOfferService
    {
        private readonly IOfferDal _offerDal;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly IMapper _mapper;
        private readonly IInstallationRequestService _installationRequestService;
        public OfferManager(IOfferDal offerDal, IMapper mapper, IInstallationRequestService installationRequestService, IPdfGeneratorService dfGeneratorService)
        {
            _offerDal = offerDal;
            _mapper = mapper;
            _installationRequestService = installationRequestService;
            _pdfGeneratorService = dfGeneratorService;

        }
        public IResult Add(OfferDto offer)
        {
            if(offer== null)
            {
                return new ErrorResult();
            }
            var mappedOffer = _mapper.Map<Offer>(offer);
            _offerDal.Add(mappedOffer);
            return new SuccessResult();
        }

        public IResult Approve(string offerId)
        {
            var offer = _offerDal.Get(x=> x.Id == offerId);
            if(offer== null)
            {
                return new ErrorResult();
            }
            if(offer.Status == OfferStatus.Pending)
            {
                offer.Status = OfferStatus.Approved;
                var pdfBytes = _pdfGeneratorService.GenerateOfferPdf(offer);
                var filePath = PdfGeneratorHelper.CreateOfferPdfStructure(offer);
                File.WriteAllBytes(filePath, pdfBytes);
                _offerDal.Update(offer);
            }

            var instRequest = new InstallationRequestDto
            {
                CreatedAt = DateTime.Now,
                OfferId = offerId,
                CustomerId = offer.CustomerId,
                IsAssigned = false,
                InstallationNote = "",
                IsCompleted = false,
            };
            _installationRequestService.Add(instRequest);

            return new SuccessResult();
        }

        public IResult Delete(string id)
        {
            _offerDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<List<Offer>> GetAll()
        {
            var offers = _offerDal.GetAll();

            return new SuccessDataResult<List<Offer>>(offers);
        }

        public IDataResult<List<Offer>> GetByCustomerId(string customerId)
        {
            var offer = _offerDal.GetAll(X=> X.CustomerId == customerId);

            return new SuccessDataResult<List<Offer>>(offer);

        }

        public IDataResult<List<Offer>> GetByDateRange(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Offer>> GetByEmployeeId(string employeeId)
        {
            var offer = _offerDal.GetAll(X => X.EmployeeId == employeeId);

            return new SuccessDataResult<List<Offer>>(offer);
        }

        public IDataResult<Offer> GetById(string id)
        {
            var offer = _offerDal.Get(x => x.Id == id);
            return new SuccessDataResult<Offer>(offer);
        }

        public IDataResult<List<Offer>> GetByStatus(OfferStatus status)
        {
            var offer = _offerDal.GetAll(x => x.Status == status);
            return new SuccessDataResult<List<Offer>>(offer);
        }

        public IDataResult<int> GetOfferCountByStatus(OfferStatus status)
        {
            var offer = _offerDal.GetAll(x => x.Status == status);

            return new SuccessDataResult<int>(offer.Count, "Teklif sayısı getirildi.");
        }

        public IResult Reject(string offerId, string reason)
        {
            var offer = _offerDal.Get(x => x.Id == offerId);
            if (offer == null)
            {
                return new ErrorResult();
            }
            if(offer.Status == OfferStatus.Pending)
            {
                offer.Status = OfferStatus.Rejected;
                offer.RejectionReason = reason;
                _offerDal.Update(offer);
            }
            return new SuccessResult();
        }

        public IResult Update(Offer offer)
        {
            var existingOffer = _offerDal.Get(x=>x.Id == offer.Id);
            if (existingOffer == null)
            {
                return new ErrorResult();

            }
            //existingOffer.Id = offer.Id;
            existingOffer.Status = offer.Status;
            existingOffer.OfferDetails = offer.OfferDetails;
            existingOffer.OfferTitle = offer.OfferTitle;
            existingOffer.CustomerId = offer.CustomerId;
            existingOffer.EmployeeId = offer.EmployeeId;
            existingOffer.TotalAmount = offer.TotalAmount;
            existingOffer.UpdatedAt = DateTime.Now;

            _offerDal.Update(existingOffer);

            return new SuccessResult();
        }

        public IDataResult<List<Offer>> WorkerCalculateEscelation()
        {
            var results = _offerDal.GetAll(x => x.Status == OfferStatus.Approved);
            var today = DateTime.Now;

            var escalatedOffers = new List<Offer>();

            foreach (var offer in results)
            {
                var daysPassed = (today - offer.CreatedAt).TotalDays;

                if (daysPassed >= 3)
                {
                    escalatedOffers.Add(offer);
                }
            }

            return new SuccessDataResult<List<Offer>>(escalatedOffers);
        }
    }
}
