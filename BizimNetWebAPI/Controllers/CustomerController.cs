using Business.Abstract;
using Core.Enums;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("Add")]
        public IActionResult Add(CustomerDto customer)
        {
            var result  = _customerService.Add(customer);
            return Ok(result);
        }
        [HttpPost("Update")]
        public IActionResult Update(Customer customer)
        {
            var result = _customerService.Update(customer);
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _customerService.GetAll();
            return Ok(result);
        }
        
        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            var result = _customerService.GetById(id);
            return Ok(result);
        }
        [HttpGet("GetAllFiltered")]
        public IActionResult GetAllFiltered(CustomerStatus status)
        {
            var result = _customerService.GetAllFiltered(status);
            return Ok(result);
        }

        [HttpGet("Deleteeee")]
        public IActionResult Delete(string id)
        {
            var result = _customerService.Delete(id);
            return Ok(result);
        }
        [HttpGet("GetCustomerCountByStatus")]
        public IActionResult GetCustomerCountByStatus(CustomerStatus status)
        {
            var result = _customerService.GetCustomerCountByStatus(status);
            return Ok(result);
        }

    }
}
