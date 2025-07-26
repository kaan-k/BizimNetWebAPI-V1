using Core.Configuration;
using Core.DataAccess.MongoDB;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
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
          : base(database, settings.Value.DocumentFileCollectionName)
        {
        }

        public List<DocumentFileDetailsDto> GetDocumentDetails()
        {
            var list = new List<DocumentFileDetailsDto>();
            var document = base.GetAll();
            foreach (var item in document)
            {
                var department = base._dataBase.GetCollection<Department>("Departments")?.Find(k => k.Id == item.DepartmentId)?.FirstOrDefault();
                var offer = base._dataBase.GetCollection<Offer>("Offers")?.Find(k => k.Id == item.OfferId)?.FirstOrDefault();
                list?.Add(new DocumentFileDetailsDto
                {
                    Id = item?.Id, 
                    DocumentName = item?.DocumentName,
                    DocumentPath = item?.DocumentPath,
                    DepartmentId = department.Name,
                    OfferId=offer.OfferTitle,
                    CreatedAt=item.CreatedAt, 
                    LastModifiedAt = item.LastModifiedAt
                });
            }
            return list;
        }
    }
}
