using Core.Entities.Abstract;
using Entities.Concrete.Payments;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Aggrements
{
    
    public class Aggrement : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int OfferId { get; set; }
        public string AgreementTitle { get; set; }

        public int CustomerId { get; set; }
        public string? AgreementType { get; set; }
        public decimal AgreedAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        // Inside Agreement.cs
        public virtual ICollection<Billing>? Billings { get; set; }
        public DateTime? CreatedAt { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public bool? IsActive { get; set; }

    }
}
