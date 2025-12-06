using AutoMapper;
using Business.Abstract;
using Business.Concrete.Constants; // Ensure PdfGeneratorHelper is here
using Core.Utilities.Context;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Duties; // ✅ Plural Namespace
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO; // Needed for File.WriteAllBytes
using System.Linq;

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

        public DutyManager(
            IDutyDal dutyDal,
            IMapper mapper,
            IUserContext userContext,
            IPdfGeneratorService pdfGeneratorService,
            IDocumentFileUploadService documentFileUploadService,
            ICustomerService customerService)
        {
            _dutyDal = dutyDal;
            _mapper = mapper;
            _user = userContext;
            _pdfGeneratorService = pdfGeneratorService;
            _documentFileUploadService = documentFileUploadService;
            _customerService = customerService;
        }

        private int GetCurrentUserId()
        {
            // Helper to parse the UserID from the context
            return int.TryParse(_user.UserId, out int userId) ? userId : 0;
        }

        public IDataResult<Duty> Add(DutyDto dutyDto)
        {
            var dutyEntity = _mapper.Map<Duty>(dutyDto);

            dutyEntity.CreatedBy = GetCurrentUserId();
            dutyEntity.CreatedAt = DateTime.UtcNow; // ✅ Use UtcNow

            _dutyDal.Add(dutyEntity);
            return new SuccessDataResult<Duty>(dutyEntity, "Görev başarıyla oluşturuldu.");
        }

        public IDataResult<Duty> AddCompleted(DutyDto request)
        {
            var dutyDto = _mapper.Map<Duty>(request);
            int currentUserId = GetCurrentUserId();

            dutyDto.CreatedBy = currentUserId;
            dutyDto.CreatedAt = DateTime.UtcNow;
            dutyDto.CompletedAt = DateTime.UtcNow;
            dutyDto.CompletedBy = currentUserId;
            dutyDto.Status = "Tamamlandı";

            _dutyDal.Add(dutyDto);
            return new SuccessDataResult<Duty>(dutyDto, "Görev başarıyla oluşturuldu.");
        }

        public IResult Delete(int id)
        {
            _dutyDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<List<Duty>> GetAll()
        {
            var duties = _dutyDal.GetAll();
            return new SuccessDataResult<List<Duty>>(duties);
        }

        public IDataResult<List<Duty>> GetAllByCustomerId(int customerId)
        {
            var duties = _dutyDal.GetAll(x => x.CustomerId == customerId);
            if (duties == null) return new ErrorDataResult<List<Duty>>();

            return new SuccessDataResult<List<Duty>>(duties);
        }

        public IDataResult<List<Duty>> GetAllByCustomerIdReport(int customerId)
        {
            var customerResult = _customerService.GetById(customerId);
            //if (customerResult.Data == null) return new ErrorResult<List<Duty>>(customerResult,"Müşteri bulunamadı");

            var duties = _dutyDal.GetAll(x => x.CustomerId == customerId);
            if (duties == null || !duties.Any()) return new ErrorDataResult<List<Duty>>();

            // PDF Generation Logic
            var pdfBytes = _pdfGeneratorService.GenerateDutiesByCustomerPdf(duties, DateTime.UtcNow);
            var filePath = PdfGeneratorHelper.CreateDailyDutiesReportPdfStructure();

            File.WriteAllBytes(filePath, pdfBytes);

            var documentFile = new DocumentFile
            {
                CreatedAt = DateTime.UtcNow,
                DocumentName = $"{customerResult.Data.CompanyName}-{DateTime.UtcNow:yyyyMMdd}",
                DocumentPath = filePath,
                DocumentFullName = $"{customerResult.Data.CompanyName}.pdf",
                LastModifiedAt = DateTime.UtcNow,
                DocumentType = "Servis Raporu",
            };

            _documentFileUploadService.DocumentFileCreateServicing(documentFile);

            return new SuccessDataResult<List<Duty>>(duties);
        }

        public IDataResult<List<Duty>> GetAllByEmployeeId(int employeeId)
        {
            // Uses the custom DAL method with .Include()
            return new SuccessDataResult<List<Duty>>(_dutyDal.GetAllDutyDetailsPerEmployee(employeeId));
        }

        public IDataResult<List<Duty>> GetAllByStatus(string status)
        {
            // Uses the custom DAL method with .Include()
            return new SuccessDataResult<List<Duty>>(_dutyDal.GetAllDutyDetailsPerStatus(GetCurrentUserId(), status));
        }

        public IDataResult<List<Duty>> GetAllDetails(int userId)
        {
            return new SuccessDataResult<List<Duty>>(_dutyDal.GetAllDutyDetails(userId));
        }

        public IDataResult<Duty> GetById(int id)
        {
            var duty = _dutyDal.Get(x => x.Id == id);
            return new SuccessDataResult<Duty>(duty);
        }

        public IDataResult<List<Duty>> GetTodaysDuties()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var duties = _dutyDal.GetAll(x => x.CompletedAt >= today && x.CompletedAt < tomorrow);

            var pdfBytes = _pdfGeneratorService.GenerateDailyDutiesPdf(duties, DateTime.UtcNow);
            var filePath = PdfGeneratorHelper.CreateDailyDutiesReportPdfStructure();

            File.WriteAllBytes(filePath, pdfBytes);

            var documentFile = new DocumentFile
            {
                CreatedAt = DateTime.UtcNow,
                DocumentName = DateTime.UtcNow.Date.ToString("yyyyMMdd"),
                DocumentPath = filePath,
                DocumentFullName = $"{DateTime.UtcNow.Date:yyyyMMdd}.pdf",
                LastModifiedAt = DateTime.UtcNow,
                DocumentType = "Günlük Rapor",
            };
            _documentFileUploadService.DocumentFileCreateServicing(documentFile);

            return new SuccessDataResult<List<Duty>>(duties);
        }

        public IDataResult<Duty> MarkAsCompleted(int id)
        {
            var duty = _dutyDal.Get(x => x.Id == id);
            if (duty == null) return new ErrorDataResult<Duty>("Görev bulunamadı");

            var timeNow = DateTime.UtcNow;

            if (duty.CompletedBy != null)
            {
                return new ErrorDataResult<Duty>(duty, "Bu görev zaten tamamlanmış.");
            }

            // Logic: If Deadline < Now, it means we finished LATE.
            // If Deadline > Now, we finished EARLY (Before Deadline).
            // Your logic: if(duty.Deadline < timeNow) -> True.
            // Wait, if Deadline (Yesterday) < Now (Today), then we are LATE. 
            // So CompletedBeforeDeadline should be FALSE.

            if (duty.Deadline.HasValue && duty.Deadline < timeNow)
            {
                // Deadline passed, we are late.
                duty.CompletedBeforeDeadline = false;
            }
            else
            {
                // Deadline is in future, we are early.
                duty.CompletedBeforeDeadline = true;
            }

            duty.Status = "Tamamlandı";
            duty.UpdatedAt = DateTime.UtcNow;
            duty.CompletedAt = DateTime.UtcNow;
            duty.CompletedBy = GetCurrentUserId();

            _dutyDal.Update(duty);

            return new SuccessDataResult<Duty>(duty, "Görev başarıyla tamamlandı!");
        }

        public IDataResult<List<Duty>> ReplaceCustomerId(int customerId, int customerIdToReplace)
        {
            var duties = _dutyDal.GetAll(x => x.CustomerId == customerId);
            foreach (Duty duty in duties)
            {
                duty.CustomerId = customerIdToReplace;
                _dutyDal.Update(duty);
            }

            return new SuccessDataResult<List<Duty>>(duties, "Customer IDs successfully replaced.");
        }

        public IResult Update(Duty request)
        {
            request.UpdatedAt = DateTime.UtcNow; // Ensure updated time is set
            _dutyDal.Update(request);
            return new SuccessResult();
        }

        public IDataResult<Duty> UpdateStatusById(int id, string newStatus)
        {
            var dutyToUpdate = _dutyDal.Get(x => x.Id == id);
            if (dutyToUpdate == null) return new ErrorDataResult<Duty>("Görev bulunamadı");

            dutyToUpdate.Status = newStatus;
            dutyToUpdate.UpdatedAt = DateTime.UtcNow;

            _dutyDal.Update(dutyToUpdate);
            return new SuccessDataResult<Duty>(dutyToUpdate);
        }
    }
}