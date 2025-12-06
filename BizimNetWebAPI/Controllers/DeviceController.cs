using Business.Abstract;
using Entities.Concrete.Devices; // ✅ Plural Namespace
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpPost("Add")]
        public IActionResult Add(Device request)
        {
            var result = _deviceService.Add(request);
            return Ok(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(Device request)
        {
            var result = _deviceService.Update(request);
            return Ok(result);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(int id) // ✅ Changed string -> int
        {
            var result = _deviceService.Delete(id);
            return Ok(result);
        }

        [HttpGet("GetByDeviceType")]
        public IActionResult GetByDeviceType(string deviceType)
        {
            var result = _deviceService.GetByDeviceType(deviceType);
            return Ok(result);
        }

        [HttpGet("GetAllByCustomerId")]
        public IActionResult GetAllByCustomerId(int id) // ✅ Changed string -> int
        {
            var result = _deviceService.GetAllByCustomerId(id);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id) // ✅ Changed string -> int
        {
            var result = _deviceService.GetById(id);
            return Ok(result);
        }

        [HttpGet("GetAllDetails")]
        public IActionResult GetAllDetails()
        {
            var result = _deviceService.GetAllDetails();
            return Ok(result);
        }
    }
}