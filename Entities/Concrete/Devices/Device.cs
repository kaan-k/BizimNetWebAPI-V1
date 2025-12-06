using Core.Entities.Abstract;
using Core.Enums;
using Entities.Concrete.Customers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Devices
{
    public class Device : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string DeviceType { get; set; }

        public string Name { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public string AnyDeskId { get; set; }
        public string? PublicIp { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
