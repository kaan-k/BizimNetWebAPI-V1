using Core.Entities.Abstract;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Customers
{
    public class CustomerDto:IDto
    {
        public string Name { get; set; }          // zorunlu
        public string CompanyName { get; set; }   // opsiyonel
        public int? ParentCustomerId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string TaxId { get; set; }
        public string City { get; set; }
        public string CustomerField { get; set; }
        public string Status { get; set; }
    }
}
