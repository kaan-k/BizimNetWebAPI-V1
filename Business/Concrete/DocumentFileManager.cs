using Business.Abstract;
using Business.BusinessRule;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.FileHelper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.DocumentFile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class DocumentFileManager : IDocumentFileUploadService
    {
        private IDocumentFileUploadDal _documentFileDal;
        private readonly IFileHelper _fileHelper;

        public DocumentFileManager(IDocumentFileUploadDal documentFileUploadDal, IFileHelper fileHelper)
        {
            _documentFileDal = documentFileUploadDal;
            _fileHelper = fileHelper;
        }
        public IResult DocumentFileAdd(IFormFile file, DocumentFile documentFile)
        {
            var rule = BusinessRules.Run(PdfRules.IsPdfExtensionRight(file.FileName));
            if (rule == null)
            {
                var newPath = Path.Combine(PathConstant.DocumentFile + documentFile.DocumentName + Path.DirectorySeparatorChar);
                documentFile.DocumentPath = _fileHelper.Upload(file, newPath);
                _documentFileDal.Add(documentFile);
                return new SuccessResult("Başarılı");
            }
            return new ErrorResult("Yanlış dosya türü");
        }

        public IResult DocumentFileDelete(string id)
        {
            var document = _documentFileDal.Get(x => x.Id == id);

            _fileHelper.Delete(PathConstant.DocumentFile + document.DocumentName + Path.DirectorySeparatorChar + document.DocumentPath);

            _documentFileDal.Delete(document.Id);
            return new SuccessResult("Messages.DeletionSuccessful");
        }

        public IResult DocumentFileUpdate(DocumentFile documentFile, IFormFile file)
        {
            documentFile.DocumentPath = _fileHelper.Update(file, PathConstant.DocumentFile + documentFile.DocumentName + Path.DirectorySeparatorChar + documentFile.DocumentPath, PathConstant.DocumentFile + documentFile.DocumentName);
            documentFile.LastModifiedAt = DateTime.Now;
            _documentFileDal.Update(documentFile);
            return new SuccessResult();
        }

        public IDataResult<List<DocumentFile>> GetAll()
        {
            var documents = _documentFileDal.GetAll();
            if(documents != null)
            {
                return new SuccessDataResult<List<DocumentFile>>(documents, "Başarıyla getirildi.");

            }
            return new ErrorDataResult<List< DocumentFile >> ("");
        }

        public IDataResult<DocumentFile> GetByDocument(string id)
        {
            return new SuccessDataResult<DocumentFile>(_documentFileDal.Get(x => x.Id == id), "Başarıyla getirildi.");
        }

        public IDataResult<DocumentFile> GetByFileName(string filename)
        {
            string normalizedFilename = Path.GetFileNameWithoutExtension(filename);
            //var document = _documentFileDal.Get(x => x.DocumentName.ToLower() == normalizedFilename.ToLower());

            var document = _documentFileDal.GetAll().AsEnumerable().FirstOrDefault(x => x.DocumentPath.ToLower().Split('_').Last() == filename.ToLower());



            if (document == null)
            {
                return new ErrorDataResult<DocumentFile>("Document not found.");
            }

            return new SuccessDataResult<DocumentFile>(document, "Messages.Successful");

        }
    }
}

