using Core.Entities.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Offer
{
    public class OfferDetailsDto:IDto
    {
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public string OfferTitle { get; set; }
        public string OfferDetails { get; set; }
        public string? RejectionReason { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
