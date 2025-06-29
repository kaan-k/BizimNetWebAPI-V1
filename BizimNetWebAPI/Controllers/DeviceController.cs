using Business.Abstract;
using Core.Enums;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Device;
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
        public IActionResult Add(DeviceDto request)
        {
            var result = _deviceService.Add(request);
            return Ok(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(DeviceDto request)
        {
            var result = _deviceService.Update(request);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _deviceService.Delete(id);
            return Ok(result);
        }

        [HttpGet("GetByDeviceType")]
        public IActionResult GetByDeviceType(DeviceType deviceType)
        {
            var result = _deviceService.GetByDeviceType(deviceType);
            return Ok(result);
        }
    }
}
