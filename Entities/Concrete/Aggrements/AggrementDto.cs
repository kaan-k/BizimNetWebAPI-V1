using Core.Entities.Abstract;
using Entities.Concrete.Offers;
using Entities.Concrete.Payments;
using Entities.DTOs.BillingDtos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Aggrements
{
    public class AggrementDto:IDto
    {

            public int Id { get; set; }

            public int CustomerId { get; set; }
            public string? CustomerName { get; set; }

            public int? OfferId { get; set; }
            public OfferDto? Offer { get; set; }

            public List<BillingDto> Billings { get; set; }

            public string AgreementTitle { get; set; }
            public decimal AgreedAmount { get; set; }
            public decimal? PaidAmount { get; set; }

            public DateTime CreatedAt { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public bool? IsActive { get; set; }

    }
}
