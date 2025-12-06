using Core.Entities.Abstract;
using System;

namespace Entities.Concrete.Payments // ✅ Changed to PLURAL
{
    public class BillingDto : IDto
    {
        public int Id { get; set; } // Added Id

        public int CustomerId { get; set; } // ✅ int

        public decimal Amount { get; set; } // ✅ decimal
        public decimal PaidAmount { get; set; } // ✅ decimal

        public DateTime BillingDate { get; set; }
        public string BillingMethod { get; set; }

        public int AgreementId { get; set; } // ✅ Spelling fixed & int
    }
}