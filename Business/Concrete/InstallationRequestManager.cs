using AutoMapper;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Email;
using Entities.Concrete.InstallationRequest;
using Entities.Concrete.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class InstallationRequestManager : IInstallationRequestService
    {
        private readonly IInstallationRequestDal _installationRequestDal;
        private readonly IMailManager _mailManager;
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public InstallationRequestManager(IInstallationRequestDal installationRequestDal, IMapper mapper, IEmployeeService employeeService, IMailManager mailManager, ICustomerService customerService)
        {
            _customerService = customerService;
            _installationRequestDal = installationRequestDal;
            _mapper = mapper;
            _employeeService = employeeService;
            _mailManager = mailManager;
        }


        public IResult Add(InstallationRequestDto request)
        {
            //if(request.AssignedEmployeeId == null)
            //{
            //    var avaliableEmployees = _employeeService.GetByRole("Kurulum Danışmanı");
            //    var random = new Random();
            //    var selectedEmployee = avaliableEmployees.Data[random.Next(avaliableEmployees.Data.Count)];

            //    request.AssignedEmployeeId = selectedEmployee.Id;
            //}
            //request.IsAssigned = true;
            var mappedRequest = _mapper.Map<InstallationRequest>(request);
            _installationRequestDal.Add(mappedRequest);

            if(request.AssignedEmployeeId != null)
            {
                SendInstallationMail(request);
            }


            return new SuccessResult();
        }

        public IResult AssignEmployee(string requestId, string employeeId)
        {
            var request = _installationRequestDal.Get(x=>x.Id == requestId);
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
            request.LastUpdatedAt = DateTime.Now;
            _installationRequestDal.Update(request);

            return new SuccessResult("Çalışan başarıyla atandı.");
        }

        public IResult Delete(string id)
        {
            _installationRequestDal.Delete(id);
            return new SuccessResult("Kurulum isteği başarıyla silindi.");
        }

        public IDataResult<List<InstallationRequest>> GetAll()
        {
            var installationRequests = _installationRequestDal.GetAll();

            return new SuccessDataResult<List<InstallationRequest>>(installationRequests);
        }

        public IDataResult<List<InstallationRequest>> GetAssigned()
        {
            var installationRequests = _installationRequestDal.GetAll(x=>x.IsAssigned == true);

            return new SuccessDataResult<List<InstallationRequest>>(installationRequests);
        }

        public IDataResult<List<InstallationRequest>> GetByCustomerId(string customerId)
        {
            var installationRequests = _installationRequestDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataResult<List<InstallationRequest>>(installationRequests);
        }

        public IDataResult<InstallationRequest> GetById(string id)
        {
            var installationRequests = _installationRequestDal.Get(x => x.Id == id);

            return new SuccessDataResult<InstallationRequest>(installationRequests);
        }

        public IDataResult<List<InstallationRequest>> GetByOfferId(string offerId)
        {
            var installationRequests = _installationRequestDal.GetAll(x => x.OfferId == offerId);

            return new SuccessDataResult<List<InstallationRequest>>(installationRequests);
        }

        public IDataResult<List<InstallationRequest>> GetUnassigned()
        {
            var installationRequests = _installationRequestDal.GetAll(x => x.IsAssigned == false);

            return new SuccessDataResult<List<InstallationRequest>>(installationRequests);
        }

        public IResult MarkAsCompleted(string requestId)
        {
            var installationRequest = _installationRequestDal.Get(x=>x.Id == requestId);
            if (installationRequest == null)
            {
                return new ErrorResult("Kurulum isteği bulunamadı.");
            }
            installationRequest.IsCompleted = true;
            _installationRequestDal.Update(installationRequest);

            return new SuccessResult("Kurulum isteği tamamlandı.");
        }

        public IResult SendInstallationMail(InstallationRequestDto request)
        {
            var mailResult = _employeeService.ReturnEmployeeEmail(request.AssignedEmployeeId);
            var mail = mailResult.Message;
            var config = new EmailConfiguration
            {
                SmtpServer = "smtp.gmail.com",
                Port = 587,
                From = "kaannkale@gmail.com",
                Username = "kaannkale@gmail.com",
                Password = "pkho hrxk adwx oxkf ",
                To = new List<string> { mail }
            };

            var customer = _customerService.GetById(request.CustomerId).Data;

            var content = new EMailContent
            {
                Subject = customer.Name + " Kurulum İsteği",
                Body = $"<h1>Test</h1><p>Müşteri adresi: {customer.Address}</p> <br> <p>Müşteri telefon numarası: {customer.PhoneNumber}</p>",
                IsBodyHtml = true
            };


            _mailManager.SendMail(config, content);
            return new SuccessResult("Mail başarıyla gönderildi.");
        }

        public IResult SendAssignmentMail()
        {


            return new SuccessResult("a");
        }

        public IResult Update(InstallationRequest request)
        {
            var existingRequest = _installationRequestDal.Get(x=> x.Id == request.Id);

            if (existingRequest == null)
            {
                return new ErrorResult("Kurulum isteği bulunamadı.");
            }
            //_mapper.Map(request, existingRequest);
            existingRequest.OfferId = request.OfferId;
            existingRequest.IsAssigned = request.IsAssigned;
            existingRequest.InstallationNote = request.InstallationNote;
            existingRequest.CreatedAt = request.CreatedAt;
            existingRequest.CustomerId = request.CustomerId;
            existingRequest.IsAssigned = request.IsAssigned;
            existingRequest.AssignedEmployeeId = request.AssignedEmployeeId;
            existingRequest.IsCompleted = request.IsCompleted;
            existingRequest.LastUpdatedAt = DateTime.Now;

            _installationRequestDal.Update(existingRequest);
            return new SuccessResult("Kurulum isteği başarıyla güncellendi.");

        }

        public IResult UpdateNote(string requestId, string note)
        {
            var existingRequest = _installationRequestDal.Get(x => x.Id == requestId);

            if (existingRequest == null)
            {
                return new ErrorResult("Kurulum isteği bulunamadı.");
            }

            existingRequest.InstallationNote = note;
            existingRequest.LastUpdatedAt = DateTime.Now;
            _installationRequestDal.Update(existingRequest);
            return new SuccessResult("Kurulum isteği başarıyla güncellendi.");
        }

        public IDataResult<List<InstallationRequest>> WorkerCalculateEscalation()
        {
            var results = _installationRequestDal.GetAll(x=>x.IsCompleted == false);
            var today = DateTime.Now.Date;
            var escalatedInstallments = new List<InstallationRequest>();

            foreach (var installment in results)
            {
                var daysPassed = (today - installment.CreatedAt).TotalDays;

                if (daysPassed >= 3)
                {
                    
                    //
                    escalatedInstallments.Add(installment);
                }
            }
            return new SuccessDataResult<List<InstallationRequest>>(escalatedInstallments);


        }
    }
}
