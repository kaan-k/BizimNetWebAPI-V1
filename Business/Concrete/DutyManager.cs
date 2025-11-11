using AutoMapper;
using Business.Abstract;
using Business.Concrete.Constants;
using Core.Utilities.Context;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Duty;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class DutyManager : IDutyService
    {
        private readonly IMapper _mapper;
        private readonly IDutyDal _dutyDal;
        private readonly IUserContext _user;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly IDocumentFileUploadService _documentFileUploadService;
        private readonly ICustomerService _customerService;
        public DutyManager(IDutyDal dutyDal, IMapper mapper, IUserContext userContext, IPdfGeneratorService pdfGeneratorService, IDocumentFileUploadService documentFileUploadService, ICustomerService customerService)
        {
            _dutyDal = dutyDal;
            _mapper = mapper;
            _user = userContext;
            _pdfGeneratorService = pdfGeneratorService;
            _documentFileUploadService = documentFileUploadService;
            _customerService = customerService;
        }
        public IDataResult<Duty> Add(DutyDto duty)
        {
            var dutyDto = _mapper.Map<Duty>(duty);
            dutyDto.CreatedBy = _user.UserId;
            dutyDto.CreatedAt = DateTime.UtcNow;
            _dutyDal.Add(dutyDto);
            return new SuccessDataResult<Duty>(dutyDto,"Görev başarıyla oluşturuldu.");
        }

        public IDataResult<Duty> AddCompleted(DutyDto request)
        {
            var dutyDto = _mapper.Map<Duty>(request);
            dutyDto.CreatedBy = _user.UserId;
            dutyDto.CreatedAt = DateTime.UtcNow;
            dutyDto.CompletedAt = DateTime.UtcNow;
            dutyDto.CompletedBy = _user.UserId;
            dutyDto.Status = "Tamamlandı";


            _dutyDal.Add(dutyDto);
            return new SuccessDataResult<Duty>(dutyDto, "Görev başarıyla oluşturuldu.");
        }

        public IResult Delete(string id)
        {
            _dutyDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<List<Duty>> GetAll()
        {
            var duties = _dutyDal.GetAll();

            return new SuccessDataResult<List<Duty>>(duties);
        }

        public IDataResult<List<Duty>> GetAllByCustomerId(string customerId)
        {
            var duties = _dutyDal.GetAll(x => x.CustomerId == customerId);

            if(duties == null)
            {
                return new ErrorDataResult<List<Duty>>();
            }

            return new SuccessDataResult<List<Duty>>(duties);

        }
        public IDataResult<List<Duty>> GetAllByEmployeeId(string employeeId)
        {
            //var duties = _dutyDal.GetAll(x => x.AssignedEmployeeId == employeeId);
            return new SuccessDataResult<List<Duty>>(_dutyDal.GetAllDutyDetailsPerEmployee(employeeId));


            //if (duties == null)
            //{
            //    return new ErrorDataResult<List<Duty>>();
            //}

            //return new SuccessDataResult<List<Duty>>(duties);

        }

        public IDataResult<List<Duty>> GetAllByStatus(string status)
        {
            var duties = _dutyDal.GetAllDutyDetailsPerStatus(_user.UserId,status);


            if (duties == null)
            {
                return new ErrorDataResult<List<Duty>>();
            }

            return new SuccessDataResult<List<Duty>>(duties);
        }

        public IDataResult<List<Duty>> GetAllDetails(string userId)
        {

            return new SuccessDataResult<List<Duty>>(_dutyDal.GetAllDutyDetails(userId)); 
        }

        public IDataResult<Duty> GetById(string id)
        {
            var dutyToGet = _dutyDal.Get(x=> x.Id == id);

            return new SuccessDataResult<Duty>(dutyToGet);
        }

        public IDataResult<List<Duty>> GetTodaysDuties()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var duties = _dutyDal.GetAll(x =>
                x.CompletedAt >= today && x.CompletedAt < tomorrow);

            var pdfBytes = _pdfGeneratorService.GenerateDailyDutiesPdf(duties, DateTime.Today);
            var filePath = PdfGeneratorHelper.CreateDailyDutiesReportPdfStructure();
            File.WriteAllBytes(filePath, pdfBytes);

            var documentFile = new DocumentFile
            {
                CreatedAt = DateTime.Now,
                DocumentName = DateTime.Today.ToString(),
                DocumentPath = filePath,
                DocumentFullName = $"{DateTime.Today.ToString()}.pdf",
                LastModifiedAt = DateTime.Now,
                DocumentType = "Günlük Rapor",
            };
            _documentFileUploadService.DocumentFileCreateServicing(documentFile);


            return new SuccessDataResult<List<Duty>>(duties);
        }
        public IDataResult<List<Duty>> GetAllByCustomerIdReport(string customerId)
        {
            var name = _customerService.GetById(customerId);
            var duties = _dutyDal.GetAll(x => x.CustomerId == customerId);

            if (duties == null)
            {
                return new ErrorDataResult<List<Duty>>();
            }

            var pdfBytes = _pdfGeneratorService.GenerateDutiesByCustomerPdf(duties, DateTime.Today);
            var filePath = PdfGeneratorHelper.CreateDailyDutiesReportPdfStructure();
            File.WriteAllBytes(filePath, pdfBytes);

            var documentFile = new DocumentFile
            {
                CreatedAt = DateTime.Now,
                DocumentName = name.Data.CompanyName+"-"+DateTime.Today.ToString(),
                DocumentPath = filePath,
                DocumentFullName = $"{name.Data.CompanyName}.pdf",
                LastModifiedAt = DateTime.Now,
                DocumentType = "Servis Raporu",
            };
            _documentFileUploadService.DocumentFileCreateServicing(documentFile);


            return new SuccessDataResult<List<Duty>>(duties);

        }

        public IDataResult<Duty> MarkAsCompleted(string id)
        {
            var duty = _dutyDal.Get(x => x.Id == id);
            var timeNow = DateTime.Now;
            if(duty.CompletedBy != null)
            {
                return new ErrorDataResult<Duty>(duty, "Bu görev zaten tamamlanmış.");
            }

            if(duty.Deadline <  timeNow)
            {
                duty.CompletedBeforeDeadline = true;
            }
            else
            {
                duty.CompletedBeforeDeadline = false;
            }

                duty.Status = "Tamamlandı";
            duty.UpdatedAt = DateTime.Now;
            duty.CompletedAt = DateTime.Now;
            duty.CompletedBy = _user.UserId;
            _dutyDal.Update(duty);

            return new SuccessDataResult<Duty>(duty,"Görev başarıyla tamamlandı!");

        }

        public IResult Update(Duty request)
        {
            _dutyDal.Update(request);

            return new SuccessResult();
        }

        public IDataResult<Duty> UpdateStatusById(string id, string newStatus)
        {
            var dutyToUpdate = _dutyDal.Get(x=> x.Id == id);
            dutyToUpdate.Status = newStatus;
            _dutyDal.Update(dutyToUpdate);
            return new SuccessDataResult<Duty>(dutyToUpdate);
        }
    }
}
