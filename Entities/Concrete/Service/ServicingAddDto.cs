using Core.Entities.Abstract;
using Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Service
{
    public class ServicingAddDto:IDto
    {

        public string Name { get; set; }
        //public string TrackingId { get; set; } = $"ARY{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        public List<string> DeviceIds { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public DateTime? LastActionDate { get; set; }
        public string? LastAction { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
