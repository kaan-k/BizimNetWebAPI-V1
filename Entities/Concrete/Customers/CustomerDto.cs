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
        public int? ParentCustomerId { get; set; }
        [ForeignKey("ParentCustomerId")]
        public virtual Customer? ParentCustomer { get; set; }

        public bool? IsHeadquarters { get; set; }
        public string? BranchName { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public virtual ICollection<Device>? Devices { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? TaxId { get; set; }
        public string? CustomerField { get; set; }
        public string? Status { get; set; }

        public DateTime? LastActionDate { get; set; }
        public string? LastAction { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<Aggrement>? Agreements { get; set; }
    }
}
