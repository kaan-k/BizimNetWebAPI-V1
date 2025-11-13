using Core.Entities.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Duty
{
    public class Duty : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CustomerId { get; set; }
        public string Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }


        public DateTime? BeginsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public string? CompletedBy{ get; set; }
        public string AssignedEmployeeId { get; set; }
        public bool CompletedBeforeDeadline { get; set; }

        public string? SignatureBase64 { get; set; }

    }
}
