using Core.Entities.Abstract;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Customers;  // ✅ Import Customer
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete.Payments // ✅ Changed to PLURAL
{
    public class Billing : IEntity
    {
        [Key]
        public int Id { get; set; } // ✅ int for SQL

        // ✅ Customer Relationship
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        // ✅ Money must be decimal
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }

        public DateTime BillingDate { get; set; }
        public DateTime DueDate { get; set; }

        // ✅ Changed to DateTime? (Nullable) because a bill might not be paid yet
        public DateTime? PaymentDate { get; set; }

        public string BillingMethod { get; set; }

        // ✅ Fixed Spelling (Aggreement -> Agreement) & Type (string -> int)
        public int AgreementId { get; set; }

        [ForeignKey("AgreementId")]
        public virtual Aggrement Agreement { get; set; } // Navigation Property
    }
}