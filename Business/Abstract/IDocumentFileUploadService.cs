using Core.Utilities.Results;
using Entities.Concrete.DocumentFile; // ✅ Plural Namespace
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IDocumentFileUploadService
    {
        IDataResult<List<DocumentFile>> GetAll();
        IDataResult<List<DocumentFile>> GetDocumentDetails();

        // ✅ Changed string -> int
        IDataResult<DocumentFile> GetByDocument(int id);

        IDataResult<DocumentFile> GetByFileName(string filename);

        IResult DocumentFileAdd(IFormFile file, DocumentFile documentFile);
        IResult DocumentFileCreateServicing(DocumentFile documentFile);

        IResult DocumentFileUpdate(DocumentFile documentFile, IFormFile file);

        // ✅ Changed string -> int
        IResult DocumentFileDelete(int id);
    }
}