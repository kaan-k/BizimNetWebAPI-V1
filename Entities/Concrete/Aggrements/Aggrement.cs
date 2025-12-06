using Core.Entities.Abstract;
using Entities.Concrete.Customers;
using Entities.Concrete.Offers;
using Entities.Concrete.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities.Concrete.Aggrements
{
    public class Aggrement : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int? OfferId { get; set; }
        public string AgreementTitle { get; set; }
        public string? CustomerName { get; set; }

        public int CustomerId { get; set; }
        public string? AgreementType { get; set; }
        public decimal AgreedAmount { get; set; }
        public decimal? PaidAmount { get; set; }

        [JsonIgnore]
        public virtual Customer Customer { get; set; }
        [JsonIgnore]

        public virtual Entities.Concrete.Offers.Offer? Offer { get; set; }
        [JsonIgnore]

        public virtual ICollection<Billing>? Billings { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpirationDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
