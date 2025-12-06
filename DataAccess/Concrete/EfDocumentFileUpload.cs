using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.DocumentFile;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfDocumentFileUploadDal : EfEntityRepositoryBase<DocumentFile, BizimNetContext>, IDocumentFileUploadDal
    {
        public List<DocumentFile> GetDocumentDetails()
        {
            using (var context = new BizimNetContext())
            {
                return context.DocumentFiles
                    .Include(d => d.Customer)
                    .ToList();
            }
        }
    }
}