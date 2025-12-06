using System;

namespace Entities.DTOs.BillingDtos
{
    public class BillingDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AgreementId { get; set; }

        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }

        public DateTime BillingDate { get; set; }
        public DateTime DueDate { get; set; }

        public DateTime? PaymentDate { get; set; }
        public string BillingMethod { get; set; }
    }

}
