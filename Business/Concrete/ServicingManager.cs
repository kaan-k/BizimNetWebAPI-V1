using AutoMapper;
using Business.Abstract;
using Business.Concrete.Constants; // Ensure PdfGeneratorHelper/EmailHelper is here
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Email;
using Entities.Concrete.Services; // ✅ Plural
using System;
using System.Collections.Generic;
using System.IO; // Required for File.WriteAllBytes
using System.Linq;

namespace Business.Concrete
{
    public class ServicingManager : IServicingService
    {
        private readonly IServicingDal _serviceDal;
        private readonly IMailManager _mailManager;
        private readonly IDocumentFileUploadService _documentFileUploadService;
        private readonly ICustomerDal _customerDal;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public ServicingManager(
            IServicingDal serviceDal,
            IMapper mapper,
            ICustomerService customer,
            ICustomerDal customerDal,
            IMailManager mailManager,
            IPdfGeneratorService pdfGeneratorService,
            IDocumentFileUploadService documentFileUploadService)
        {
            _serviceDal = serviceDal;
            _mailManager = mailManager;
            _customerService = customer;
            _mapper = mapper;
            _customerDal = customerDal;
            _pdfGeneratorService = pdfGeneratorService;
            _documentFileUploadService = documentFileUploadService;
        }

        public IDataResult<ServicingAddDto> Add(ServicingAddDto serviceDto)
        {
            var servicingToAdd = _mapper.Map<Servicing>(serviceDto);

            // Set Initial Dates
            servicingToAdd.CreatedAt = DateTime.UtcNow; // ✅ Use UtcNow

            _serviceDal.Add(servicingToAdd);

            // Generate PDF logic
            var pdfBytes = _pdfGeneratorService.GenerateServicingPdf(servicingToAdd);
            var filePath = PdfGeneratorHelper.CreateServicingPdfStructure(servicingToAdd);

            File.WriteAllBytes(filePath, pdfBytes);

            var documentFile = new DocumentFile
            {
                CreatedAt = DateTime.UtcNow,
                DocumentName = servicingToAdd.Name,
                DocumentPath = filePath,
                DocumentFullName = $"{servicingToAdd.Name}.pdf",
                LastModifiedAt = DateTime.UtcNow,
                DocumentType = "Servis",
                CustomerId = servicingToAdd.CustomerId, // Now it's an int, perfect
                // ServiceId = servicingToAdd.Id // Ideally link it back to the service
            };

            _documentFileUploadService.DocumentFileCreateServicing(documentFile);

            return new SuccessDataResult<ServicingAddDto>(serviceDto, "Servis başarıyla eklendi.");
        }

        public IDataResult<List<Servicing>> GetAll()
        {
            // Uses the custom .Include() method
            return new SuccessDataResult<List<Servicing>>(_serviceDal.GetAllServicingDetails());
        }

        public IDataResult<Servicing> GetById(int id)
        {
            return new SuccessDataResult<Servicing>(_serviceDal.Get(x => x.Id == id));
        }

        public IDataResult<Servicing> GetByTrackingId(string trackingId)
        {
            // GetAll(filter) is better than GetAll().FirstOrDefault() for SQL performance
            var service = _serviceDal.Get(x => x.TrackingId == trackingId);

            if (service == null)
            {
                return new ErrorDataResult<Servicing>("Servis bulunamadı.");
            }
            return new SuccessDataResult<Servicing>(service, "Servis bulundu.");
        }

        public IResult MarkAsCompleted(int id)
        {
            var service = _serviceDal.Get(x => x.Id == id);

            if (service == null)
            {
                return new ErrorResult("Servis bulunamadı.");
            }

            service.Status = "Completed";
            service.LastAction = "Servis tamamlandı.";
            service.LastActionDate = DateTime.UtcNow; // ✅ Use UtcNow
            service.UpdatedAt = DateTime.UtcNow;

            _serviceDal.Update(service);

            // Send Mail (Wrap in try-catch to prevent crashing if mail fails)
            try
            {
                SendCompletionMail(service);
            }
            catch
            {
                // Log error but don't fail the transaction
            }

            return new SuccessResult("Servis başarıyla tamamlandı.");
        }

        public IResult MarkAsInProgress(int id)
        {
            var service = _serviceDal.Get(x => x.Id == id);

            if (service == null)
            {
                return new ErrorResult("Servis bulunamadı.");
            }

            service.Status = "InProgress"; // Fixed typo "IProgress" -> "InProgress"
            service.LastAction = "Servis tamir aşamasında";
            service.LastActionDate = DateTime.UtcNow;
            service.UpdatedAt = DateTime.UtcNow;

            _serviceDal.Update(service);

            return new SuccessResult("Servis tamir aşamasına alındı.");
        }

        public IResult SendCompletionMail(Servicing servicing)
        {
            // Ensure CustomerId is valid int
            var customer = _customerDal.Get(x => x.Id == servicing.CustomerId);
            if (customer == null) return new ErrorResult("Müşteri bulunamadı");

            var config = EmailHelper.config(customer.Email);

            var content = new EMailContent
            {
                Subject = customer.Name + " Servis Tamamlandı",
                Body = $@"
    <div style='font-family: Arial, sans-serif; color: #333;'>
        <h2 style='color: #2c3e50;'>Servis Tamamlandı</h2>
        <p><strong>Cihaz:</strong> {servicing.Name}</p>
        <p><strong>Takip Kodu:</strong> {servicing.TrackingId}</p>
        <hr style='margin-top: 20px;'>
        <p style='font-size: 12px; color: #999;'>Bu e-posta otomatik olarak oluşturulmuştur.</p>
    </div>",
                IsBodyHtml = true
            };

            _mailManager.SendMail(config, content);
            return new SuccessResult("Mail başarıyla gönderildi.");
        }

        public IDataResult<Servicing> Update(Servicing servicing)
        {
            servicing.UpdatedAt = DateTime.UtcNow;
            _serviceDal.Update(servicing);
            return new SuccessDataResult<Servicing>(servicing, "Servis başarıyla güncellendi.");
        }
    }
}