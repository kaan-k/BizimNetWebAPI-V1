using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Email;
using Entities.Concrete.InstallationRequests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class InstallationRequestManager : IInstallationRequestService
    {
        private readonly IInstallationRequestDal _installationRequestDal;
        private readonly IMailManager _mailManager;
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public InstallationRequestManager(
            IInstallationRequestDal installationRequestDal,
            IMapper mapper,
            IEmployeeService employeeService,
            IMailManager mailManager,
            ICustomerService customerService)
        {
            _customerService = customerService;
            _installationRequestDal = installationRequestDal;
            _mapper = mapper;
            _employeeService = employeeService;
            _mailManager = mailManager;
        }

        public IResult Add(InstallationRequestDto request)
        {
            var mappedRequest = _mapper.Map<InstallationRequest>(request);

            // Allow SQL to generate ID
            mappedRequest.CreatedAt = DateTime.UtcNow;

            _installationRequestDal.Add(mappedRequest);

            if (request.AssignedEmployeeId != null)
            {
                SendInstallationMail(request);
            }

            return new SuccessResult();
        }

        public IResult AssignEmployee(int requestId, int employeeId)
        {
            var request = _installationRequestDal.Get(x => x.Id == requestId);
            if (request == null)
            {
                return new ErrorResult("Kurulum isteği bulunamadı.");
            }
            if (request.IsAssigned)
            {
                return new ErrorResult("Kurulum isteğine atanmış bir çalışan bulunmakta.");
            }

            request.IsAssigned = true;
            request.AssignedEmployeeId = employeeId;
            request.LastUpdatedAt = DateTime.UtcNow;

            _installationRequestDal.Update(request);

            return new SuccessResult("Çalışan başarıyla atandı.");
        }

        public IResult Delete(int id)
        {
            _installationRequestDal.Delete(id);
            return new SuccessResult("Kurulum isteği başarıyla silindi.");
        }

        public IDataResult<List<InstallationRequest>> GetAll()
        {
            var requests = _installationRequestDal.GetAll();
            return new SuccessDataResult<List<InstallationRequest>>(requests);
        }

        public IDataResult<List<InstallationRequest>> GetAllInstallationRequestDetails()
        {
            return new SuccessDataResult<List<InstallationRequest>>(_installationRequestDal.GetAllInstallationRequestDetails());
        }

        public IDataResult<List<InstallationRequest>> GetAssigned()
        {
            var requests = _installationRequestDal.GetAll(x => x.IsAssigned == true);
            return new SuccessDataResult<List<InstallationRequest>>(requests);
        }

        public IDataResult<List<InstallationRequest>> GetByCustomerId(int customerId)
        {
            var requests = _installationRequestDal.GetAll(x => x.CustomerId == customerId);
            return new SuccessDataResult<List<InstallationRequest>>(requests);
        }

        public IDataResult<InstallationRequest> GetById(int id)
        {
            var request = _installationRequestDal.Get(x => x.Id == id);
            return new SuccessDataResult<InstallationRequest>(request);
        }

        public IDataResult<List<InstallationRequest>> GetByOfferId(int offerId)
        {
            var requests = _installationRequestDal.GetAll(x => x.OfferId == offerId);
            return new SuccessDataResult<List<InstallationRequest>>(requests);
        }

        public IDataResult<List<InstallationRequest>> GetUnassigned()
        {
            var requests = _installationRequestDal.GetAll(x => x.IsAssigned == false);
            return new SuccessDataResult<List<InstallationRequest>>(requests);
        }

        public IResult MarkAsCompleted(int requestId)
        {
            var request = _installationRequestDal.Get(x => x.Id == requestId);
            if (request == null) return new ErrorResult("Kurulum isteği bulunamadı.");

            request.IsCompleted = true;
            request.LastUpdatedAt = DateTime.UtcNow;

            _installationRequestDal.Update(request);

            return new SuccessResult("Kurulum isteği tamamlandı.");
        }

        public IResult SendAssignmentMail()
        {
            return new SuccessResult("Assignment mail logic not implemented yet.");
        }

        public IResult SendInstallationMail(InstallationRequestDto request)
        {
            if (request.AssignedEmployeeId == null) return new ErrorResult("Çalışan atanmamış.");

            // Convert nullable int? to int
            int employeeId = request.AssignedEmployeeId.Value;

            var mailResult = _employeeService.ReturnEmployeeEmail(employeeId);
            if (!mailResult.Success) return new ErrorResult("Çalışan emaili bulunamadı.");

            var mail = mailResult.Message; // Assuming Message holds the email address string

            var config = new EmailConfiguration
            {
                SmtpServer = "smtp.gmail.com",
                Port = 587,
                From = "kaannkale@gmail.com",
                Username = "kaannkale@gmail.com",
                Password = "pkho hrxk adwx oxkf ", // ⚠️ Move this to appsettings.json!
                To = new List<string> { mail }
            };

            var customerResult = _customerService.GetById(request.CustomerId);
            if (customerResult.Data == null) return new ErrorResult("Müşteri bulunamadı");

            var customer = customerResult.Data;

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

        public IResult Update(InstallationRequest request)
        {
            var existingRequest = _installationRequestDal.Get(x => x.Id == request.Id);

            if (existingRequest == null) return new ErrorResult("Kurulum isteği bulunamadı.");

            existingRequest.OfferId = request.OfferId;
            existingRequest.IsAssigned = request.IsAssigned;
            existingRequest.InstallationNote = request.InstallationNote;
            // existingRequest.CreatedAt = request.CreatedAt; // Usually we don't change creation date
            existingRequest.CustomerId = request.CustomerId;
            existingRequest.AssignedEmployeeId = request.AssignedEmployeeId;
            existingRequest.IsCompleted = request.IsCompleted;
            existingRequest.LastUpdatedAt = DateTime.UtcNow;

            _installationRequestDal.Update(existingRequest);
            return new SuccessResult("Kurulum isteği başarıyla güncellendi.");
        }

        public IResult UpdateNote(int requestId, string note)
        {
            var existingRequest = _installationRequestDal.Get(x => x.Id == requestId);
            if (existingRequest == null) return new ErrorResult("Kurulum isteği bulunamadı.");

            existingRequest.InstallationNote = note;
            existingRequest.LastUpdatedAt = DateTime.UtcNow;

            _installationRequestDal.Update(existingRequest);
            return new SuccessResult("Not güncellendi.");
        }

        public IDataResult<List<InstallationRequest>> WorkerCalculateEscalation()
        {
            // Logic: Get unfinished requests older than 3 days
            var results = _installationRequestDal.GetAll(x => x.IsCompleted == false);
            var today = DateTime.UtcNow.Date;
            var escalatedInstallments = new List<InstallationRequest>();

            foreach (var installment in results)
            {
                var daysPassed = (today - installment.CreatedAt).TotalDays;

                if (daysPassed >= 3)
                {
                    escalatedInstallments.Add(installment);
                }
            }
            return new SuccessDataResult<List<InstallationRequest>>(escalatedInstallments);
        }
    }
}