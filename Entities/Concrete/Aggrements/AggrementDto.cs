using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Aggrements
{
    public class AggrementDto:IDto
    {
        public string CustomerId { get; set; }
        public string AggrementTitle { get; set; }

        public string AggrementType { get; set; }

        public int AgreedAmount { get; set; }
        public int PaidAmount { get; set; }
        public List<string> billings { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool isActive { get; set; }
    }
}
