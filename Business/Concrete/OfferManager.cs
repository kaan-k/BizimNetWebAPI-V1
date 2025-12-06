using AutoMapper;
using Business.Abstract;
using Business.Concrete.Constants; // Ensure PdfGeneratorHelper is here
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Offers; // ✅ Plural
using System;
using System.Collections.Generic;
using System.IO; // Needed for File.WriteAllBytes
using System.Linq;

namespace Business.Concrete
{
    public class OfferManager : IOfferService
    {
        private readonly IOfferDal _offerDal;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly IMapper _mapper;
        private readonly IInstallationRequestService _installationRequestService;
        private readonly ICustomerService _customerService;
        private readonly IDocumentFileUploadService _documentFileUploadService;

        public OfferManager(
            IOfferDal offerDal,
            IMapper mapper,
            IInstallationRequestService installationRequestService,
            IPdfGeneratorService pdfGeneratorService,
            ICustomerService customerService,
            IDocumentFileUploadService documentFileUploadService)
        {
            _offerDal = offerDal;
            _mapper = mapper;
            _installationRequestService = installationRequestService;
            _pdfGeneratorService = pdfGeneratorService;
            _customerService = customerService;
            _documentFileUploadService = documentFileUploadService;
        }

        public IResult Add(OfferDto offerDto)
        {
            if (offerDto == null)
            {
                return new ErrorResult("Teklif boş olamaz.");
            }

            var mappedOffer = _mapper.Map<Offer>(offerDto);

            // Set initial dates
            mappedOffer.CreatedAt = DateTime.UtcNow; // ✅ Use UtcNow

            _offerDal.Add(mappedOffer);
            return new SuccessResult("Teklif başarıyla eklendi.");
        }

        public IResult Approve(int offerId)
        {
            var offer = _offerDal.Get(x => x.Id == offerId);
            if (offer == null)
            {
                return new ErrorResult("Teklif bulunamadı.");
            }

            // 2. Change Status
            offer.Status = "Approved";
            offer.UpdatedAt = DateTime.UtcNow; // ✅ Use UtcNow

            _offerDal.Update(offer);

            return new SuccessResult("Teklif onaylandı.");
        }

        public IResult Delete(int id)
        {
            _offerDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<string> GenerateOfferReport(OfferDto offer)
        {
            // 1. Get Customer Name (CustomerId is now int)
            var customerResult = _customerService.GetById(offer.CustomerId);
            if (customerResult.Data == null) return new ErrorDataResult<string>("Müşteri bulunamadı");

            var companyName = customerResult.Data.CompanyName;

            // 2. Generate the PDF Bytes
            var pdfBytes = _pdfGeneratorService.GenerateOfferPdf(offer);

            // 3. Save to Disk
            var filePath = PdfGeneratorHelper.CreateOfferPdfStructure(offer);
            File.WriteAllBytes(filePath, pdfBytes);

            // 4. Save to DB (DocumentFiles)
            var documentFile = new DocumentFile
            {
                CreatedAt = DateTime.UtcNow,
                DocumentName = $"{companyName}-{DateTime.UtcNow:yyyyMMdd}",
                DocumentPath = filePath,
                DocumentFullName = $"{companyName}.pdf",
                LastModifiedAt = DateTime.UtcNow,
                DocumentType = "Teklif Raporu",
                CustomerId = offer.CustomerId // ✅ Assign int ID directly
            };

            _documentFileUploadService.DocumentFileCreateServicing(documentFile);

            // 5. Convert to Base64 and Return Data
            string pdfBase64 = Convert.ToBase64String(pdfBytes);

            return new SuccessDataResult<string>(pdfBase64, "Teklif raporu başarıyla oluşturuldu.");
        }

        public IDataResult<List<Offer>> GetAll()
        {
            var offers = _offerDal.GetAll();
            return new SuccessDataResult<List<Offer>>(offers);
        }

        public IDataResult<List<Offer>> GetAllDetails()
        {
            return new SuccessDataResult<List<Offer>>(_offerDal.GetAllOfferDetails());
        }

        public IDataResult<List<Offer>> GetByCustomerId(int customerId)
        {
            var offers = _offerDal.GetAll(x => x.CustomerId == customerId);
            return new SuccessDataResult<List<Offer>>(offers);
        }

        public IDataResult<List<Offer>> GetByDateRange(DateTime start, DateTime end)
        {
            // Simple SQL date range query
            var offers = _offerDal.GetAll(x => x.CreatedAt >= start && x.CreatedAt <= end);
            return new SuccessDataResult<List<Offer>>(offers);
        }

        public IDataResult<Offer> GetById(int id)
        {
            var offer = _offerDal.Get(x => x.Id == id);
            return new SuccessDataResult<Offer>(offer);
        }

        public IDataResult<List<Offer>> GetByStatus(string status)
        {
            var offers = _offerDal.GetByStatus(status);
            return new SuccessDataResult<List<Offer>>(offers);
        }

        public IResult Update(Offer offer)
        {
            if (offer == null)
            {
                return new ErrorResult("Teklif boş olamaz.");
            }

            offer.UpdatedAt = DateTime.UtcNow; // ✅ Use UtcNow
            _offerDal.Update(offer);
            return new SuccessResult();
        }
    }
}