using Core.Utilities.Results;
using Entities.Concrete.DocumentFile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDocumentFileUploadService
    {
        IDataResult<List<DocumentFile>> GetAll();
        IDataResult<List<DocumentFile>> GetDocumentDetails();

        IDataResult<DocumentFile> GetByDocument(string id);
        IDataResult<DocumentFile> GetByFileName(string filename);

        IResult DocumentFileAdd(IFormFile file, DocumentFile documentFile);
        IResult DocumentFileCreateServicing(DocumentFile documentFile);

        IResult DocumentFileUpdate(DocumentFile documentFile, IFormFile file);
        IResult DocumentFileDelete(string id);
    }
}
