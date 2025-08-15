using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.InviteToken
{
    public class InviteTokenValidationDto:IDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime Expiration { get; set; }
    }
}
