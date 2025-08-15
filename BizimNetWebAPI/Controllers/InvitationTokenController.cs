using Business.Abstract;
using Entities.Concrete.Customer;
using Entities.Concrete.InviteToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationTokenController : ControllerBase
    {
        private readonly IInviteTokenService _inviteTokenService;

        public InvitationTokenController(IInviteTokenService inviteTokenService)
        {
            _inviteTokenService = inviteTokenService;
        }
        [HttpPost("Add")]
        public IActionResult Add(InviteTokenCreateDto token)
        {
            var result = _inviteTokenService.Add(token);
            return Ok(result);
        }
    }
}
