using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Castle.Core.Internal;
using Core.Utilities.FileHelper;
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
        private readonly IFileHelper _fileHelper;

        public DocumentFileUploadController(IDocumentFileUploadService documentFileUploadService, IMapper mapper, IDocumentFileUploadDal documentFileUploadDal, IFileHelper fileHelper)
        {
            _documentFileService = documentFileUploadService;
            _mapper = mapper;
            _documentFileUploadDal = documentFileUploadDal;
            _fileHelper = fileHelper;
        }

        [HttpPost("Add")]
        public IActionResult DocumentFileAdd([FromForm] DocumentFileAddRequest request)
        {
            var map = _mapper.Map<DocumentFile>(request);

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
            var result = _documentFileService.GetDocumentDetails();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetByDocument")]
        public IActionResult GetByDocument(string id)
        {
            var result = _documentFileService.GetByDocument(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        //allowanon kaldirmayi unutma!!

        [HttpGet("DownloadDocument/{id}")]
        [AllowAnonymous]
        public IActionResult DownloadDocument(string id)
        {
            
            var document = _documentFileService.GetByDocument(id);
            if (document == null)
            {
                return NotFound();
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
            var map = _mapper.Map<DocumentFile>(request);

            // Eski veriyi veritabanından çek
            var oldDocumentResult = _documentFileService.GetByDocument(map.Id);
            if (oldDocumentResult.Success && oldDocumentResult.Data != null)
            {
                var oldDocument = oldDocumentResult.Data;
                // DocumentName değişmişse eski dosyayı sil
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "DocumentFile",
                        oldDocument.DocumentName, oldDocument.DocumentPath);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                    var path = Path.Combine(PathConstant.DocumentFile + map.DocumentName + Path.DirectorySeparatorChar);
                    map.DocumentPath = _fileHelper.Upload(request.File, path);
                }
            } 

            var result = _documentFileService.DocumentFileUpdate(map, request.File);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }



    }
}
