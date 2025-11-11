using Core.Configuration;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.Duty;
using Entities.Concrete.Offer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class Mongo_DutyDal:MongoRepository<Duty>, IDutyDal
    {
        public Mongo_DutyDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
         : base(database, settings.Value.DutiesCollectionName)
        {
        }
        public List<Duty> GetAllDutyDetails(string requesterId)
        {
            var list = new List<Duty>();
            var data = base.GetAll();
            var customerCollection = base._dataBase.GetCollection<Customer>("Customers");
            var businessUsersCollection = base._dataBase.GetCollection<BusinessUser>("BusinessUsers");

            foreach (var item in data)
            {
                var filter = Builders<Customer>.Filter.Eq(k => k.Id, item.CustomerId);
                var customer = customerCollection.Find(filter).FirstOrDefault();

                var businessUserCreatedFilter = Builders<BusinessUser>.Filter.Eq(k => k.Id, item.CreatedBy);
                var businessUserCompletedFilter = Builders<BusinessUser>.Filter.Eq(k => k.Id, item.CompletedBy);
                var businessUserFilter = Builders<BusinessUser>.Filter.Eq(k => k.Id, requesterId);
                var businessUserAssignedAt = Builders<BusinessUser>.Filter.Eq(k => k.Id, item.AssignedEmployeeId);


                var createdBy = businessUsersCollection.Find(businessUserCreatedFilter).FirstOrDefault();
                var completedBy = businessUsersCollection.Find(businessUserCompletedFilter).FirstOrDefault();
                var assignedAt = businessUsersCollection.Find(businessUserAssignedAt).FirstOrDefault();
                var businessUser = businessUsersCollection.Find(businessUserFilter).FirstOrDefault();

                if(item.AssignedEmployeeId == requesterId)
                {
                    list?.Add(new Duty
                    {
                        Id = item?.Id,
                        CustomerId = customer?.CompanyName,
                        Status = item?.Status,
                        CreatedAt = item?.CreatedAt,
                        UpdatedAt = item?.UpdatedAt,
                        CompletedAt = item?.CompletedAt,
                        Deadline = item?.Deadline,
                        Name = item?.Name,
                        Description = item?.Description,
                        AssignedEmployeeId = assignedAt?.FirstName,
                        Priority = item?.Priority,
                        CreatedBy = createdBy?.FirstName,
                        CompletedBy = completedBy?.FirstName,
                        BeginsAt = item.BeginsAt,
                        EndsAt = item.EndsAt

                    });
                }
                else if (businessUser.isAuthorised)
                {
                    list?.Add(new Duty
                    {
                        Id = item?.Id,
                        CustomerId = customer?.CompanyName,
                        Status = item?.Status,
                        CreatedAt = item?.CreatedAt,
                        UpdatedAt = item?.UpdatedAt,
                        CompletedAt = item?.CompletedAt,
                        Deadline = item?.Deadline,
                        Name = item?.Name,
                        Description = item?.Description,
                        AssignedEmployeeId = assignedAt?.FirstName,
                        Priority = item?.Priority,
                        CreatedBy = createdBy?.FirstName,
                        CompletedBy = completedBy?.FirstName,
                        BeginsAt = item.BeginsAt,
                        EndsAt = item.EndsAt

                    });
                }
                
            }
            return list;
        }



        public List<Duty> GetAllDutyDetailsPerEmployee(string employeeId)
        {
            var list = new List<Duty>();
            var data = base.GetAll();
            var customerCollection = base._dataBase.GetCollection<Customer>("Customers");
            var businessUsersCollection = base._dataBase.GetCollection<BusinessUser>("BusinessUsers");

            foreach (var item in data)
            {
                var filter = Builders<Customer>.Filter.Eq(k => k.Id, item.CustomerId);
                var customer = customerCollection.Find(filter).FirstOrDefault();

                var businessUserCreatedFilter = Builders<BusinessUser>.Filter.Eq(k => k.Id, item.CreatedBy);
                var businessUserCompletedFilter = Builders<BusinessUser>.Filter.Eq(k => k.Id, item.CompletedBy);
                var businessUserFilter = Builders<BusinessUser>.Filter.Eq(k => k.Id, employeeId);
                var businessUserAssignedAt = Builders<BusinessUser>.Filter.Eq(k => k.Id, item.AssignedEmployeeId);


                var createdBy = businessUsersCollection.Find(businessUserCreatedFilter).FirstOrDefault();
                var completedBy = businessUsersCollection.Find(businessUserCompletedFilter).FirstOrDefault();
                var assignedAt = businessUsersCollection.Find(businessUserAssignedAt).FirstOrDefault();
                var businessUser = businessUsersCollection.Find(businessUserFilter).FirstOrDefault();

                if (item.AssignedEmployeeId == employeeId)
                {
                    list?.Add(new Duty
                    {
                        Id = item?.Id,
                        CustomerId = customer?.CompanyName,
                        Status = item?.Status,
                        CreatedAt = item?.CreatedAt,
                        UpdatedAt = item?.UpdatedAt,
                        CompletedAt = item?.CompletedAt,
                        Deadline = item?.Deadline,
                        Name = item?.Name,
                        Description = item?.Description,
                        AssignedEmployeeId = assignedAt?.FirstName,
                        Priority = item?.Priority,
                        CreatedBy = createdBy?.FirstName,
                        CompletedBy = completedBy?.FirstName,
                        BeginsAt = item.BeginsAt,
                        EndsAt = item.EndsAt

                    });
                }
                //else if (businessUser.isAuthorised)
                //{
                //    list?.Add(new Duty
                //    {
                //        Id = item?.Id,
                //        CustomerId = customer?.CompanyName,
                //        Status = item?.Status,
                //        CreatedAt = item?.CreatedAt,
                //        UpdatedAt = item?.UpdatedAt,
                //        CompletedAt = item?.CompletedAt,
                //        Deadline = item?.Deadline,
                //        Name = item?.Name,
                //        Description = item?.Description,
                //        AssignedEmployeeId = assignedAt?.FirstName,
                //        Priority = item?.Priority,
                //        CreatedBy = createdBy?.FirstName,
                //        CompletedBy = completedBy?.FirstName,
                //        BeginsAt = item.BeginsAt,
                //        EndsAt = item.EndsAt

                //    });
                //}

            }
            return list;
        }


        public List<Duty> GetAllDutyDetailsPerStatus(string employeeId,string status)
        {
            var list = new List<Duty>();
            var data = base.GetAll();
            var customerCollection = base._dataBase.GetCollection<Customer>("Customers");
            var businessUsersCollection = base._dataBase.GetCollection<BusinessUser>("BusinessUsers");

            foreach (var item in data)
            {
                var filter = Builders<Customer>.Filter.Eq(k => k.Id, item.CustomerId);
                var customer = customerCollection.Find(filter).FirstOrDefault();

                var businessUserCreatedFilter = Builders<BusinessUser>.Filter.Eq(k => k.Id, item.CreatedBy);
                var businessUserCompletedFilter = Builders<BusinessUser>.Filter.Eq(k => k.Id, item.CompletedBy);
                var businessUserFilter = Builders<BusinessUser>.Filter.Eq(k => k.Id, employeeId);
                var businessUserAssignedAt = Builders<BusinessUser>.Filter.Eq(k => k.Id, item.AssignedEmployeeId);


                var createdBy = businessUsersCollection.Find(businessUserCreatedFilter).FirstOrDefault();
                var completedBy = businessUsersCollection.Find(businessUserCompletedFilter).FirstOrDefault();
                var assignedAt = businessUsersCollection.Find(businessUserAssignedAt).FirstOrDefault();
                var businessUser = businessUsersCollection.Find(businessUserFilter).FirstOrDefault();

                if (item.Status == status)
                {
                    list?.Add(new Duty
                    {
                        Id = item?.Id,
                        CustomerId = customer?.CompanyName,
                        Status = item?.Status,
                        CreatedAt = item?.CreatedAt,
                        UpdatedAt = item?.UpdatedAt,
                        CompletedAt = item?.CompletedAt,
                        Deadline = item?.Deadline,
                        Name = item?.Name,
                        Description = item?.Description,
                        AssignedEmployeeId = assignedAt?.FirstName,
                        Priority = item?.Priority,
                        CreatedBy = createdBy?.FirstName,
                        CompletedBy = completedBy?.FirstName,
                        BeginsAt = item.BeginsAt,
                        EndsAt = item.EndsAt

                    });
                }
                //else if (businessUser.isAuthorised)
                //{
                //    list?.Add(new Duty
                //    {
                //        Id = item?.Id,
                //        CustomerId = customer?.CompanyName,
                //        Status = item?.Status,
                //        CreatedAt = item?.CreatedAt,
                //        UpdatedAt = item?.UpdatedAt,
                //        CompletedAt = item?.CompletedAt,
                //        Deadline = item?.Deadline,
                //        Name = item?.Name,
                //        Description = item?.Description,
                //        AssignedEmployeeId = assignedAt?.FirstName,
                //        Priority = item?.Priority,
                //        CreatedBy = createdBy?.FirstName,
                //        CompletedBy = completedBy?.FirstName,
                //        BeginsAt = item.BeginsAt,
                //        EndsAt = item.EndsAt

                //    });
                //}

            }
            return list;
        }


    }
}
