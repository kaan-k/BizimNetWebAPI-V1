using Business.Abstract;
using Entities.Concrete.Services; // ✅ Plural Namespace
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

        [HttpPost("Update")]
        public IActionResult Update([FromBody] Servicing serviceDto)
        {
            var result = _servicingService.Update(serviceDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ TrackingId remains string (e.g. "ARY-123")
        [HttpGet("GetByTrackingId")]
        public IActionResult GetByTrackingId(string trackingId)
        {
            var result = _servicingService.GetByTrackingId(trackingId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id) // ✅ Changed string -> int
        {
            var result = _servicingService.GetById(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _servicingService.GetAll();
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("MarkAsCompleted")]
        public IActionResult MarkAsCompleted(int id) // ✅ Changed string -> int
        {
            var result = _servicingService.MarkAsCompleted(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("MarkAsInProgress")]
        public IActionResult MarkAsInProgress(int id) // ✅ Changed string -> int
        {
            var result = _servicingService.MarkAsInProgress(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}