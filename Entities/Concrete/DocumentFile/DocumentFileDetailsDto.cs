using Core.Entities.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete.DocumentFile
{
    public class DocumentFileDetailsDto:IDto
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PersonId { get; set; }
        public string? OfferId { get; set; }
        public string? CustomerId { get; set; }

        public string DepartmentId { get; set; } 
        public string DocumentName { get; set; }
        public string? DocumentPath { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastModifiedAt { get; set; }
        public string? DocumentType { get; set; }


    }
}
