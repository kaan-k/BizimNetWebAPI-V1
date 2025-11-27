using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Payment
{
    public class BillingDto:IDto
    {
        public string CustomerId { get; set; }
        public int Amount { get; set; }
        public int PaidAmount { get; set; }

        public DateTime BillingDate { get; set; }
        public string BillingMethod { get; set; }
        public string AggreementId { get; set; }
    }
}
