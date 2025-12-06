using Core.DataAccess;
using Entities.Concrete.DocumentFile;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IDocumentFileUploadDal : IEntityRepository<DocumentFile>
    {
        List<DocumentFile> GetDocumentDetails();
    }
}