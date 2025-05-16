using Core.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.InstallationRequest
{
    public class InstallationRequest:IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string OfferId { get; set; }
        public string CustomerId { get; set; }
        public string? AssignedEmployeeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }

        public bool IsAssigned { get; set; } = false; 
        public bool IsCompleted { get; set; }

        public string? InstallationNote { get; set; } 
    }
}
