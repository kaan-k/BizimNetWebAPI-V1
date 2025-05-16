using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Castle.Core.Internal;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.DocumentFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace BizimNetWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DocumentFileUploadController : ControllerBase
    {
        private readonly IDocumentFileUploadService _documentFileService;
        private readonly IDocumentFileUploadDal _documentFileUploadDal;
        private readonly IMapper _mapper;

        public DocumentFileUploadController(IDocumentFileUploadService documentFileUploadService, IMapper mapper, IDocumentFileUploadDal documentFileUploadDal)
        {
            _documentFileService = documentFileUploadService;
            _mapper = mapper;   
            _documentFileUploadDal = documentFileUploadDal;
        }

        [HttpPost("Add")]
        public IActionResult DocumentFileAdd([FromForm] DocumentFileAddRequest request)
        {
            var map = new DocumentFile
            {
                PersonId = request.PersonId,
                DepartmentId = request.DepartmentId,
                DocumentName = request.DocumentName,
                DocumentPath = request.DocumentPath,
                DocumentFullName = request.DocumentFullName
            };

            var fileNameResult = _documentFileService.GetByFileName(request.File.FileName);

            Core.Utilities.Results.IResult result;
            if (fileNameResult.Success)
            {
                result = _documentFileService.DocumentFileUpdate(map, request.File);
            }
            else
            {
                result = _documentFileService.DocumentFileAdd(request.File, map);
            }

            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _documentFileService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        //[HttpGet("GetAllDocumentDetails")]
        //public IActionResult GetDocumentDetails()
        //{
        //    var result = _documentFileService.GetDocumentDetails();
        //    if (!result.Success)
        //    {
        //        return BadRequest(result.Message);
        //    }
        //    return Ok(result);
        //}
        //allowanon kaldirmayi unutma!!
  
        [HttpGet("DownloadDocument/{documentId}")]
        [AllowAnonymous]
        public IActionResult DownloadDocument(string documentId, [FromQuery]string employeeId)
        {
            // Belgeyi belge kimliği (documentId) kullanarak alın
            var document = _documentFileService.GetByDocument(documentId);
            if (document == null)
            {
                return NotFound(); // Belge bulunamazsa 404 Not Found döndürün
            }

            if(employeeId != null)
            {
                

                if (!document.Data.downloderIds.Contains(employeeId))
                {
                    document.Data.downloderIds.Add(employeeId);
                    _documentFileUploadDal.Update(document.Data);
                }
            }

            // Belgeyi tarayıcıya indir
            var newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "DocumentFile" + Path.DirectorySeparatorChar + 
                document.Data.DocumentName + Path.DirectorySeparatorChar + document.Data.DocumentPath);
            var fileBytes = System.IO.File.ReadAllBytes(newPath);
            return File(fileBytes, "application/pdf", document.Data.DocumentName);
        }


        [HttpGet("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _documentFileService.DocumentFileDelete(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("Update")]
        public IActionResult Update([FromForm] DocumentFileUpdateRequest request)
        {
            var map = new DocumentFile
            {
                PersonId = request.PersonId,
                DepartmentId = request.DepartmentId,
                DocumentName = request.DocumentName,
                DocumentPath = request.DocumentPath,
                DocumentFullName = request.DocumentFullName,
                LastModifiedAt = DateTime.Now
            };

            var fileNameResult = _documentFileService.GetByFileName(request.File.FileName);

            if (fileNameResult.Success)
            {
                var result = _documentFileService.DocumentFileUpdate(map, request.File);
                return result.Success ? Ok(result) : BadRequest(result.Message);
            }

            return BadRequest("Veri bulunamadı");
        }



    }
}
