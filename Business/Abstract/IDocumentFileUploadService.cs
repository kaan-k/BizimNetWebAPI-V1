using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.DocumentFiles;
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
        IDataResult<DocumentFile> GetByDocument(string id);
        IDataResult<DocumentFile> GetByFileName(string filename);

        IResult DocumentFileAdd(IFormFile file, DocumentFile documentFile);
        IResult DocumentFileUpdate(DocumentFile documentFile, IFormFile file);
        IResult DocumentFileDelete(string id);

       //IDataResult<List<DocumentFileDetailsDto>> GetDocumentDetails();
    }
}
