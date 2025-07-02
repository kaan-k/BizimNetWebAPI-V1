using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete.Service;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicingController : ControllerBase
    {
        private readonly IServicingService _servicingService;

        public ServicingController(IServicingService servicingService)
        {
            _servicingService = servicingService;
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] ServicingAddDto serviceDto)
        {
            var result = _servicingService.Add(serviceDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("GetByTrackingId")]
        public IActionResult GetByTrackingId(string trackingId)
        {
            var result = _servicingService.GetByTrackingId(trackingId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("MarkAsCompleted")]
        public IActionResult MarkAsCompleted(string id)
        {
            var result = _servicingService.MarkAsCompleted(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpGet("MarkAsInProgress")]
        public IActionResult MarkAsInProgress(string id)
        {
            var result = _servicingService.MarkAsInProgress(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
