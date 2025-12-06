using AutoMapper;
using Business.Abstract;
using Business.Concrete.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Offers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;

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
        private readonly BizimNetContext _context;

        public OfferManager(
            IOfferDal offerDal,
            IMapper mapper,
            IInstallationRequestService installationRequestService,
            IPdfGeneratorService pdfGeneratorService,
            ICustomerService customerService,
            IDocumentFileUploadService documentFileUploadService, BizimNetContext context)
        {
            _offerDal = offerDal;
            _mapper = mapper;
            _installationRequestService = installationRequestService;
            _pdfGeneratorService = pdfGeneratorService;
            _customerService = customerService;
            _documentFileUploadService = documentFileUploadService;
            _context = context;
        }

        public IDataResult<OfferDto> GetById(int id)
        {
            var entity = _offerDal.GetByIdWithDetails(id);

            if (entity == null)
                return new ErrorDataResult<OfferDto>("Teklif bulunamadı.");

            var dto = _mapper.Map<OfferDto>(entity);

            return new SuccessDataResult<OfferDto>(dto);
        }


        public IResult Add(OfferDto offerDto)
        {
            if (offerDto == null)
                return new ErrorResult("Teklif verisi boş olamaz.");

            // DTO → Entity
            var offer = _mapper.Map<Offer>(offerDto);

            offer.CreatedAt = DateTime.UtcNow;

            if (offerDto.ExpirationDate.HasValue)
                offer.ExpirationDate = DateTime.SpecifyKind(offerDto.ExpirationDate.Value, DateTimeKind.Utc);

            // OFFER SAVE
            _offerDal.Add(offer);
            _context.SaveChanges();               // 🔥 ID burada oluşur

            // OFFER ITEMS SAVE
            if (offerDto.Items != null)
            {
                foreach (var itemDto in offerDto.Items)
                {
                    var item = _mapper.Map<OfferItem>(itemDto);
                    item.OfferId = offer.Id;

                    _context.Add(item);
                }
            }

            _context.SaveChanges();

            return new SuccessResult("Teklif başarıyla oluşturuldu.");
        }


        public IResult Approve(int offerId)
        {
            var offer = _offerDal.Get(x => x.Id == offerId);
            if (offer == null)
                return new ErrorResult("Teklif bulunamadı.");

            if (offer.Status == "Approved")
                return new ErrorResult("Teklif zaten onaylanmış.");

            offer.Status = "Approved";
            offer.UpdatedAt = DateTime.UtcNow;

            _offerDal.Update(offer);

            return new SuccessResult("Teklif onaylandı.");
        }

        public IResult Delete(int id)
        {
            _offerDal.Delete(id);
            return new SuccessResult("Teklif silindi.");
        }

        public IDataResult<string> GenerateOfferReport(int offerId)
        {
            // 1️⃣ Offer entity'sini full include ile çek
            var offer = _offerDal.GetByIdWithDetails(offerId);

            if (offer == null)
                return new ErrorDataResult<string>("Teklif bulunamadı.");

            // 2️⃣ Customer bilgisini entity’den al
            var customerName = offer.Customer?.CompanyName ?? "Müşteri";

            // 3️⃣ PDF üretimi → entity ile yapılır
            var pdfBytes = _pdfGeneratorService.GenerateOfferPdf(offer.Id);

            // 4️⃣ Fiziksel dosya oluşturma
            var filePath = PdfGeneratorHelper.CreateOfferPdfStructure(_mapper.Map<OfferDto>(offer));
            File.WriteAllBytes(filePath, pdfBytes);

            // 5️⃣ DocumentFile kaydı
            var documentFile = new DocumentFile
            {
                CreatedAt = DateTime.UtcNow,
                DocumentName = $"{customerName}-{DateTime.UtcNow:yyyyMMdd}",
                DocumentPath = filePath,
                DocumentFullName = $"{customerName}.pdf",
                LastModifiedAt = DateTime.UtcNow,
                DocumentType = "Teklif Raporu",
                CustomerId = offer.CustomerId
            };

            _documentFileUploadService.DocumentFileCreateServicing(documentFile);

            // 6️⃣ Base64 döndürme
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
            var offers = _offerDal.GetAll(x => x.CreatedAt >= start && x.CreatedAt <= end);
            return new SuccessDataResult<List<Offer>>(offers);
        }



        public IDataResult<List<Offer>> GetByStatus(string status)
        {
            var offers = _offerDal.GetByStatus(status);
            return new SuccessDataResult<List<Offer>>(offers);
        }

        public IResult Update(Offer offer)
        {
            if (offer == null)
                return new ErrorResult("Teklif boş olamaz.");

            offer.UpdatedAt = DateTime.UtcNow;
            _offerDal.Update(offer);
            return new SuccessResult("Teklif güncellendi.");
        }
    }
}
