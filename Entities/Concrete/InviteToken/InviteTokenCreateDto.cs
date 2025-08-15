using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.InviteToken
{
    public class InviteTokenCreateDto:IDto
    {
        public string Email { get; set; }
        public DateTime Expiration { get; set; }
    }
}
