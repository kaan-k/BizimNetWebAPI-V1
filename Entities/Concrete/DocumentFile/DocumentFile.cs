using Core.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.DocumentFile
{
    public class DocumentFile : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? OfferId { get; set; }
        public string PersonId { get; set; }
        public string DepartmentId { get; set; }
        public List<string> downloderIds { get; set; } = new List<string>();
        public string DocumentName { get; set; }
        public string? DocumentPath { get; set; }
        public string? DocumentFullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastModifiedAt { get;set; }
    }

}
