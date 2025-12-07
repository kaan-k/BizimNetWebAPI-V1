using AutoMapper;
using Business.Abstract;
using Business.Concrete.Constants;
using Core.Utilities.Context;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Migrations;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Duties;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
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
            return int.TryParse(_user.UserId, out int userId) ? userId : 0;
        }

        // Ortak helper: Entity -> DTO (CustomerName doldurur)
        private DutyDto MapDutyToDto(Duty duty)
        {
            if (duty == null) return null;

            var dto = _mapper.Map<DutyDto>(duty);

            // CustomerName
            if (duty.Customer != null)
            {
                dto.CustomerName = duty.Customer.CompanyName;
            }
            else
            {
                var customerResult = _customerService.GetById(duty.CustomerId);
                if (customerResult.Success && customerResult.Data != null)
                    dto.CustomerName = customerResult.Data.CompanyName;
            }

            // AssignedEmployeeName şimdilik boş; BusinessUser servisini
            // buraya inject edersen buradan doldurursun.
            // dto.AssignedEmployeeName = ...;

            return dto;
        }

        private List<DutyDto> MapDutyListToDtoList(List<Duty> duties)
        {
            return duties?.Select(MapDutyToDto).ToList() ?? new List<DutyDto>();
        }
        private static DateTime? ToUtc(DateTime? value)
        {
            if (value == null) return null;
            if (value.Value.Kind == DateTimeKind.Utc) return value;
            return DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
        }

        public IDataResult<DutyDto> Add(DutyDto dutyDto)
        {
            var dutyEntity = _mapper.Map<Entities.Concrete.Duties.Duty>(dutyDto);

            dutyEntity.CreatedBy = GetCurrentUserId();
            dutyEntity.CreatedAt = DateTime.UtcNow;

            // Status boşsa Pending olarak setle
            if (string.IsNullOrWhiteSpace(dutyEntity.Status))
                dutyEntity.Status = "Pending";
            dutyEntity.CreatedAt = DateTime.UtcNow;

            dutyEntity.Deadline = ToUtc(dutyEntity.Deadline);
            _dutyDal.Add(dutyEntity);

            var resultDto = MapDutyToDto(dutyEntity);
            return new SuccessDataResult<DutyDto>(resultDto, "Görev başarıyla oluşturuldu.");
        }

        public IDataResult<DutyDto> AddCompleted(DutyDto request)
        {
            var dutyEntity = _mapper.Map<Entities.Concrete.Duties.Duty>(request);
            int currentUserId = GetCurrentUserId();
            var now = DateTime.UtcNow;

            dutyEntity.CreatedBy = currentUserId;
            dutyEntity.CreatedAt = now;
            dutyEntity.CompletedAt = now;
            dutyEntity.CompletedBy = currentUserId;
            dutyEntity.BeginsAt = DateTime.UtcNow;
            dutyEntity.Status = "Completed";

            if (dutyEntity.Deadline.HasValue)
            {
                // Deadline >= now => zamanında bitmiş (before deadline)
                dutyEntity.CompletedBeforeDeadline = dutyEntity.Deadline.Value >= now;
            }

            _dutyDal.Add(dutyEntity);

            var dto = MapDutyToDto(dutyEntity);
            return new SuccessDataResult<DutyDto>(dto, "Görev tamamlanmış olarak oluşturuldu.");
        }

        public IResult Delete(int id)
        {
            _dutyDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<List<DutyDto>> GetAll()
        {
            var duties = _dutyDal.GetAll();
            var dtoList = MapDutyListToDtoList(duties);
            return new SuccessDataResult<List<DutyDto>>(dtoList);
        }

        public IDataResult<List<DutyDto>> GetAllByCustomerId(int customerId)
        {
            var duties = _dutyDal.GetAll(x => x.CustomerId == customerId);
            if (duties == null || !duties.Any())
                return new ErrorDataResult<List<DutyDto>>("Kayıt bulunamadı.");

            var dtoList = MapDutyListToDtoList(duties);
            return new SuccessDataResult<List<DutyDto>>(dtoList);
        }

        public IDataResult<List<DutyDto>> GetAllByCustomerIdReport(int customerId)
        {
            var customerResult = _customerService.GetById(customerId);
            var duties = _dutyDal.GetAll(x => x.CustomerId == customerId);

            if (customerResult.Data == null || duties == null || !duties.Any())
                return new ErrorDataResult<List<DutyDto>>("Müşteri veya görev bulunamadı.");

            // PDF
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

            var dtoList = MapDutyListToDtoList(duties);
            return new SuccessDataResult<List<DutyDto>>(dtoList);
        }

        public IDataResult<List<DutyDto>> GetAllByEmployeeId(int employeeId)
        {
            var duties = _dutyDal.GetAllDutyDetailsPerEmployee(employeeId);
            var dtoList = MapDutyListToDtoList(duties);
            return new SuccessDataResult<List<DutyDto>>(dtoList);
        }

        public IDataResult<List<DutyDto>> GetAllByStatus(string status)
        {
            var duties = _dutyDal.GetAllDutyDetailsPerStatus(GetCurrentUserId(), status);
            var dtoList = MapDutyListToDtoList(duties);
            return new SuccessDataResult<List<DutyDto>>(dtoList);
        }

        public IDataResult<List<DutyDto>> GetAllDetails(int userId)
        {
            var duties = _dutyDal.GetAllDutyDetails(userId);
            var dtoList = MapDutyListToDtoList(duties);
            return new SuccessDataResult<List<DutyDto>>(dtoList);
        }

        public IDataResult<DutyDto> GetById(int id)
        {
            var duty = _dutyDal.Get(x => x.Id == id);
            if (duty == null)
                return new ErrorDataResult<DutyDto>("Görev bulunamadı");

            var dto = MapDutyToDto(duty);
            return new SuccessDataResult<DutyDto>(dto);
        }

        public IDataResult<List<DutyDto>> GetTodaysDuties()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var duties = _dutyDal.GetAll(x => x.CompletedAt >= today && x.CompletedAt < tomorrow);

            // PDF için entity listesi
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

            var dtoList = MapDutyListToDtoList(duties);
            return new SuccessDataResult<List<DutyDto>>(dtoList);
        }

        public IDataResult<DutyDto> MarkAsCompleted(int id)
        {
            var duty = _dutyDal.Get(x => x.Id == id);
            if (duty == null)
                return new ErrorDataResult<DutyDto>("Görev bulunamadı");

            var now = DateTime.UtcNow;

            if (duty.CompletedBy != null)
                return new ErrorDataResult<DutyDto>("Bu görev zaten tamamlanmış.");

            if (duty.Deadline.HasValue)
            {
                duty.CompletedBeforeDeadline = duty.Deadline.Value >= now;
            }

            duty.Status = "Completed";
            duty.UpdatedAt = now;
            duty.CompletedAt = now;
            duty.CompletedBy = GetCurrentUserId();

            _dutyDal.Update(duty);

            var dto = MapDutyToDto(duty);
            return new SuccessDataResult<DutyDto>(dto, "Görev başarıyla tamamlandı!");
        }

        public IDataResult<List<DutyDto>> ReplaceCustomerId(int customerId, int customerIdToReplace)
        {
            var duties = _dutyDal.GetAll(x => x.CustomerId == customerId);

            foreach (var duty in duties)
            {
                duty.CustomerId = customerIdToReplace;
                _dutyDal.Update(duty);
            }

            var dtoList = MapDutyListToDtoList(duties);
            return new SuccessDataResult<List<DutyDto>>(dtoList, "Customer IDs successfully replaced.");
        }

        public IResult Update(DutyDto request)
        {
            var duty = _dutyDal.Get(x => x.Id == request.Id);
            if (duty == null)
                return new ErrorResult("Görev bulunamadı");

            // DTO -> Entity update
            _mapper.Map(request, duty);
            duty.UpdatedAt = DateTime.UtcNow;

            _dutyDal.Update(duty);
            return new SuccessResult();
        }

        public IDataResult<DutyDto> UpdateStatusById(int id, string newStatus)
        {
            var dutyToUpdate = _dutyDal.Get(x => x.Id == id);
            if (dutyToUpdate == null)
                return new ErrorDataResult<DutyDto>("Görev bulunamadı");

            dutyToUpdate.Status = newStatus;
            dutyToUpdate.UpdatedAt = DateTime.UtcNow;

            _dutyDal.Update(dutyToUpdate);

            var dto = MapDutyToDto(dutyToUpdate);
            return new SuccessDataResult<DutyDto>(dto);
        }
    }
}
