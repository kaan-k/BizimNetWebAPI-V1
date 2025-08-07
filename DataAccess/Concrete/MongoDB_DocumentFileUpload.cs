using Core.Configuration;
using Core.DataAccess.MongoDB;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.DocumentFile;
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

        //public List<DocumentFileDetailsDto> GetDocumentDetails()
        //{
        //    var list = new List<DocumentFileDetailsDto>();
        //    var document = base.GetAll();
        //    foreach (var item in document)
        //    {
        //        var department = base._dataBase.GetCollection<Department>("Departments")?.Find(k => k.Id == item.DepartmentId)?.FirstOrDefault()?.DepartmentName;
        //        var personName = "";
        //        var person = base._dataBase.GetCollection<User>("Users")?.Find(k => k.Id == item.PersonId)?.FirstOrDefault();
        //        personName = person?.FirstName + " " + person?.LastName;
        //        if (string.IsNullOrWhiteSpace(personName))
        //        {
        //            var employee = base._dataBase.GetCollection<Employee>("Employees")?.Find(k => k.Id == item.PersonId)?.FirstOrDefault();
        //            personName = employee?.Name + " " + employee?.Surname;
        //        }
        //        list?.Add(new DocumentFileDetailsDto
        //        {
        //            Id = item?.Id,
        //            PersonName = person?.FirstName + " " + person?.LastName,
        //            DocumentName = item?.DocumentName,
        //            DocumentPath = item?.DocumentPath,
        //            DepartmentId = department
        //        });
        //    }
        //    return list;
        //}
    }
}
