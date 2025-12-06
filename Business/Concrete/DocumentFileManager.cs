using Business.Abstract;
using Business.BusinessRule; // Ensure this namespace exists in your project
using Business.Constants;

// using Business.Constants; // Ensure PathConstant exists
using Core.Utilities.Business;
using Core.Utilities.FileHelper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.DocumentFile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Business.Concrete
{
    public class DocumentFileManager : IDocumentFileUploadService
    {
        private readonly IDocumentFileUploadDal _documentFileDal;
        private readonly IFileHelper _fileHelper;

        public DocumentFileManager(IDocumentFileUploadDal documentFileUploadDal, IFileHelper fileHelper)
        {
            _documentFileDal = documentFileUploadDal;
            _fileHelper = fileHelper;
        }

        public IResult DocumentFileAdd(IFormFile file, DocumentFile documentFile)
        {
            // Assuming BusinessRules and PdfRules are defined elsewhere in your project
            var rule = BusinessRules.Run(PdfRules.IsPdfExtensionRight(file.FileName));

            if (rule == null)
            {
                // Ensure PathConstant is available or replace with hardcoded string for testing
                var newPath = Path.Combine(PathConstant.DocumentFile + documentFile.DocumentName + Path.DirectorySeparatorChar);

                documentFile.DocumentPath = _fileHelper.Upload(file, newPath);
                documentFile.CreatedAt = DateTime.UtcNow; // ✅ Set Creation Date

                _documentFileDal.Add(documentFile);
                return new SuccessResult("Başarılı");
            }
            return new ErrorResult("Yanlış dosya türü");
        }

        public IResult DocumentFileCreateServicing(DocumentFile documentFile)
        {
            documentFile.CreatedAt = DateTime.UtcNow;
            _documentFileDal.Add(documentFile);
            return new SuccessResult("Başarılı");
        }

        public IResult DocumentFileDelete(int id)
        {
            var document = _documentFileDal.Get(x => x.Id == id);
            if (document == null) return new ErrorResult("Dosya bulunamadı.");

            // Delete physical file
            _fileHelper.Delete(PathConstant.DocumentFile + document.DocumentName + Path.DirectorySeparatorChar + document.DocumentPath);

            // Delete DB record
            _documentFileDal.Delete(id);
            return new SuccessResult("Dosya silindi.");
        }

        public IResult DocumentFileUpdate(DocumentFile documentFile, IFormFile file)
        {
            documentFile.LastModifiedAt = DateTime.UtcNow; // ✅ Use UtcNow
            _documentFileDal.Update(documentFile);
            return new SuccessResult();
        }

        public IDataResult<List<DocumentFile>> GetAll()
        {
            return new SuccessDataResult<List<DocumentFile>>(_documentFileDal.GetAll(), "Başarıyla getirildi.");
        }

        public IDataResult<DocumentFile> GetByDocument(int id)
        {
            return new SuccessDataResult<DocumentFile>(_documentFileDal.Get(x => x.Id == id), "Başarıyla getirildi.");
        }

        public IDataResult<DocumentFile> GetByFileName(string filename)
        {
            // Note: This logic pulls all records into memory (.ToList/AsEnumerable) 
            // because 'Split' and 'Last' are hard to translate to SQL.
            // For large databases, consider storing the "CleanFileName" in a separate column.

            var document = _documentFileDal.GetAll()
                .FirstOrDefault(x => x.DocumentPath != null && x.DocumentPath.ToLower().Split('_').Last() == filename.ToLower());

            if (document == null)
            {
                return new ErrorDataResult<DocumentFile>("Document not found.");
            }

            return new SuccessDataResult<DocumentFile>(document, "Başarılı.");
        }

        public IDataResult<List<DocumentFile>> GetDocumentDetails()
        {
            var docx = _documentFileDal.GetDocumentDetails();
            return new SuccessDataResult<List<DocumentFile>>(docx);
        }
    }
}