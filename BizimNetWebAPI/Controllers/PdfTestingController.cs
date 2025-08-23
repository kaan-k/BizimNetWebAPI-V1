using Business.Abstract;
using Business.Concrete.Constants;
using Entities.Concrete.Customer;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Duty;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfTestingController : ControllerBase
    {
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly IDutyService _dutyService;
        public PdfTestingController(IPdfGeneratorService pdfGeneratorService, IDutyService dutyService)
        {
            _pdfGeneratorService = pdfGeneratorService;
            _dutyService = dutyService;
        }
        [HttpPost("Add")]
        public IActionResult Add()
        {
            var result = _dutyService.GetTodaysDuties();

            return Ok(result);
        }
    }
}
