using Autofac.Core;
using AutoMapper;
using Business.Abstract;
using Business.Concrete.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Email;
using Entities.Concrete.Offer;
using Entities.Concrete.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ServicingManager(IServicingDal serviceDal, IMapper mapper, ICustomerService customer, ICustomerDal customerDal, IMailManager mailManager, IPdfGeneratorService pdfGeneratorService, IDocumentFileUploadService documentFileUploadService)
        {
            _serviceDal = serviceDal;
            _mailManager = mailManager;
            _customerService = customer;
            _mapper = mapper;
            _customerDal = customerDal;
            _pdfGeneratorService = pdfGeneratorService;
            _documentFileUploadService = documentFileUploadService;
        }
        public IDataResult<ServicingAddDto> Add(ServicingAddDto service)
        {
            var servicingToAdd = _mapper.Map<Servicing>(service);
            _serviceDal.Add(servicingToAdd);

            var pdfBytes = _pdfGeneratorService.GenerateServicingPdf(servicingToAdd);
            var filePath = PdfGeneratorHelper.CreateServicingPdfStructure(servicingToAdd);
            File.WriteAllBytes(filePath, pdfBytes);

            var documentFile = new DocumentFile
            {
                CreatedAt = DateTime.Now,
                DocumentName = servicingToAdd.Name,
                DocumentPath = filePath,
                DocumentFullName = $"{servicingToAdd.Name}.pdf",
                LastModifiedAt = DateTime.Now,
                DocumentType = "Servis",
                CustomerId = servicingToAdd.CustomerId
            };


            _documentFileUploadService.DocumentFileCreateServicing(documentFile);
            return new SuccessDataResult<ServicingAddDto>(service, "Servis başarıyla eklendi.");

        }

        public IDataResult<List<Servicing>> GetAll()
        {
            return new SuccessDataResult<List<Servicing>>(_serviceDal.GetAllServicingDetails());
        }

        public IDataResult<Servicing> GetById(string id)
        {
            return new SuccessDataResult<Servicing>(_serviceDal.Get(x => x.Id == id));
        }

        public IDataResult<Servicing> GetByTrackingId(string trackingId)
        {
            var service = _serviceDal.GetAll(x => x.TrackingId == trackingId).FirstOrDefault();
            if (service == null)
            {
                return new ErrorDataResult<Servicing>("Servis bulunamadı.");
            }
            return new SuccessDataResult<Servicing>(service,"Servis bulundu.");
        }

        public IResult MarkAsCompleted(string id)
        {
            var service = _serviceDal.Get(x=>x.Id == id);

            if (service == null)
            {
                return new ErrorResult("Servis bulunamadı.");
            }

            service.Status = "Completed";
            service.LastAction = "Servis tamamlandı.";
            service.LastActionDate = DateTime.Now;
            service.UpdatedAt = DateTime.Now;
            _serviceDal.Update(service);

            SendCompletionMail(service);

            return new SuccessResult("Servis başarıyla tamamlandı.");

        }
        

        public IResult MarkAsInProgress(string id)
        {
            var service = _serviceDal.Get(x => x.Id == id);

            if (service == null)
            {
                return new ErrorResult("Servis bulunamadı.");
            }

            service.Status = "IProgress";
            service.LastAction = "Servis tamir aşamasında";
            service.LastActionDate = DateTime.Now;
            service.UpdatedAt = DateTime.Now;
            _serviceDal.Update(service);

            return new SuccessResult("Servis başarıyla tamamlandı.");
        }

        public IResult SendCompletionMail(Servicing servicing)
        {
            var customer = _customerDal.Get(x => x.Id == servicing.CustomerId);

            var config = EmailHelper.config(customer.Email);
            

            //var customer = _customerService.GetById(request.CustomerId).Data;

            var content = new EMailContent
            {
                Subject = customer.Name + " Kurulum İsteği",
                Body = $@"
    <div style='font-family: Arial, sans-serif; color: #333;'>
        <h2 style='color: #2c3e50;'>Kurulum Talebi</h2>
        <p><strong>Müşteri Adresi:</strong> {customer.Address}</p>
        <p><strong>Telefon Numarası:</strong> {customer.PhoneNumber}</p>
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
            servicing.UpdatedAt = DateTime.Now; 
            _serviceDal.Update(servicing);
            return new SuccessDataResult<Servicing>(servicing, "Servis başarıyla güncellendi.");

        }
    }
}
