using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Aggrements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggrementController : ControllerBase
    {
        private readonly IAggrementService _aggrementService;

        public AggrementController(IAggrementService aggrementService)
        {
            _aggrementService = aggrementService;
        }

        [HttpPost("Add")]
        public IActionResult Add(AggrementDto aggrementDto)
        {
            var result = _aggrementService.Add(aggrementDto);
            return Ok(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(Aggrement aggrement, string id)
        {
            var result = _aggrementService.Update(aggrement, id);
            return Ok(result);
        }
        [HttpPost("CreateFromOffer")]
        public IActionResult CreateFromOffer(string offerId)
        {
            var result = _aggrementService.CreateAgreementFromOffer(offerId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _aggrementService.Delete(id);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            var result = _aggrementService.GetById(id);
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _aggrementService.GetAll();
            return Ok(result);
        }

        [HttpPost("RecieveBill")]
        public IActionResult RecieveBill(string aggrementId, int amount)
        {
            var result = _aggrementService.RecieveBill(aggrementId, amount);
            return Ok(result);
        }
    }
}