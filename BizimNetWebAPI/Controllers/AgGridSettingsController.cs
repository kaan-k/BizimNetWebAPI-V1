using Business.Abstract;
using Entities.Concrete.Customer;
using Entities.Concrete.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgGridSettingsController : ControllerBase
    {
        private readonly IAgGridSettingsService _agGridSettingsService;
        public AgGridSettingsController(IAgGridSettingsService agGridSettingsService ) {
            _agGridSettingsService = agGridSettingsService;
        }


        [HttpPost("Add")]
        public IActionResult Add(AgGridSettingsDto agGridSetting)
        {
            var result = _agGridSettingsService.Add(agGridSetting);
            return Ok(result);
        }
    }
}
