using Business.Abstract;
using Entities.Concrete.Aggrements;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgreementController : ControllerBase
    {
        private readonly IAggrementService _agreementService;

        public AgreementController(IAggrementService agreementService)
        {
            _agreementService = agreementService;
        }

        [HttpPost("Add")]
        public IActionResult Add(AggrementDto agreementDto)
        {
            var result = _agreementService.Add(agreementDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(Aggrement agreement)
        {
            var result = _agreementService.Update(agreement);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("CreateFromOffer")]
        public IActionResult CreateFromOffer(int offerId)
        {
            var result = _agreementService.CreateAgreementFromOffer(offerId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _agreementService.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _agreementService.GetById(id);

            return result.Success ? Ok(result) : BadRequest(result.Message);
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _agreementService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("ReceiveBill")]
        public IActionResult ReceiveBill(int agreementId, decimal amount)
        {
            var result = _agreementService.ReceiveBill(agreementId, amount);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}