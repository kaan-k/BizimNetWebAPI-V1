using Core.Configuration;
using Core.DataAccess.MongoDB;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
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
          : base(database, settings.Value.CustomerCollectionName)
        {
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
