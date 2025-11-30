using AutoMapper;
using Business.Abstract;
using Business.Concrete.Constants;
using Core.Entities.Concrete;
using Core.Enums;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Duty;
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
        private readonly ICustomerService _customerService;
        private readonly IDocumentFileUploadService _documentFileUploadService;

        public OfferManager(IOfferDal offerDal, IMapper mapper, IInstallationRequestService installationRequestService, IPdfGeneratorService dfGeneratorService, ICustomerService customerService, IDocumentFileUploadService documentFileUploadService)
        {
            _offerDal = offerDal;
            _mapper = mapper;
            _installationRequestService = installationRequestService;
            _pdfGeneratorService = dfGeneratorService;
            _customerService = customerService;
            _documentFileUploadService = documentFileUploadService;
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
            var offer = _offerDal.Get(x => x.Id == offerId);
            if (offer == null)
            {
                return new ErrorResult("Teklif bulunamadı.");
            }

            // 2. Change Status
            offer.Status = "Approved";
            offer.UpdatedAt = DateTime.Now;
            _offerDal.Update(offer);

            return new SuccessResult("Teklif onaylandı.");

        }

        public IResult Delete(string id)
        {
            _offerDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<string> GenerateOfferReport(OfferDto offer)
        {
            var customer = offer.CustomerId;
            var name = _customerService.GetById(customer);

            // 1. Generate the PDF Bytes
            var pdfBytes = _pdfGeneratorService.GenerateOfferPdf(offer);

            // 2. Keep your existing logic (Save to Disk & DB)
            var filePath = PdfGeneratorHelper.CreateOfferPdfStructure(offer);
            File.WriteAllBytes(filePath, pdfBytes);

            var documentFile = new DocumentFile
            {
                CreatedAt = DateTime.Now,
                DocumentName = name.Data.CompanyName + "-" + DateTime.Today.ToString(),
                DocumentPath = filePath,
                DocumentFullName = $"{name.Data.CompanyName}.pdf",
                LastModifiedAt = DateTime.Now,
                DocumentType = "Teklif Raporu",
            };
            _documentFileUploadService.DocumentFileCreateServicing(documentFile);

            // 3. Convert to Base64 and Return Data
            // This allows the Angular frontend to display/download it immediately
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

        public IDataResult<List<Offer>> GetByCustomerId(string customerId)
        {
            var offer = _offerDal.GetAll(X=> X.CustomerId == customerId);

            return new SuccessDataResult<List<Offer>>(offer);

        }

        public IDataResult<List<Offer>> GetByDateRange(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        

        public IDataResult<Offer> GetById(string id)
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
                return new ErrorResult();

            }
            offer.UpdatedAt = DateTime.Now; 
            _offerDal.Update(offer); 
            return new SuccessResult();
        }

        
    }
}
