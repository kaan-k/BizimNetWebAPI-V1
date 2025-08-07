using Core.Configuration;
using Core.DataAccess.MongoDB;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.Department;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Offer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.DataBases.MongoDB
{
    public class MongoDB_DocumentFileUpload : MongoRepository<DocumentFile>, IDocumentFileUploadDal
    {
        public MongoDB_DocumentFileUpload(IMongoDatabase database, IOptions<MongoDbSettings> settings)
          : base(database, settings.Value.DocumentsCollectionName)
        {
        }


        public List<DocumentFile> GetDocumentDetails()
        {
            var list =  new List<DocumentFile>();
            var document = base.GetAll();
            var customerCollection = base._dataBase.GetCollection<Customer>("Customers");

            foreach (var item in document)
            {
                var filter = Builders<Customer>.Filter.Eq(k => k.Id, item.CustomerId);
                var customer = customerCollection.Find(filter).FirstOrDefault();

                list?.Add(new DocumentFile
                {
                    DocumentType = item?.DocumentType,
                    CustomerId = customer?.CompanyName,
                    DocumentName = item?.DocumentName,
                    DocumentPath = item?.DocumentPath,
                    DocumentFullName = item?.DocumentFullName,
                    CreatedAt = item?.CreatedAt,
                    DepartmentId = item?.DepartmentId,
                    downloderIds = item?.downloderIds,
                    Id = item?.Id,
                    OfferId =item?.OfferId,
                    LastModifiedAt = item?.LastModifiedAt,
                    PersonId = item?.PersonId

                });
            }
            return list;

        }
    }
}
