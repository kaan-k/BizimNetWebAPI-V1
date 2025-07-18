using Core.Entities.Abstract;
using Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Device
{
    public class DeviceDto : IDto
    {
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public string DeviceType { get; set; }
        public string AnyDeskId { get; set; }
        public string? PublicIp { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
