using Core.DataAccess;
using Core.DataAccess.MongoDB;
using Entities.Concrete.DocumentFile;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IDocumentFileUploadDal : IMongoRepository<DocumentFile>
    {
        public List<DocumentFile> GetDocumentDetails();

    }
}
