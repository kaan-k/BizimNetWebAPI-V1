using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessUserController : ControllerBase
    {
        private readonly IBusinessUserService _businessUserInterface;

        public BusinessUserController(IBusinessUserService businessUserInterface)
        {
            _businessUserInterface = businessUserInterface;
        }


        [HttpPost("Add")]
        public IActionResult Add(BusinessUserDto businessUser)
        {
            var register = _businessUserInterface.Add(businessUser);
            var check = _businessUserInterface.CreateAccessToken(register.Data);
            if (!check.Success)
            {
                return BadRequest(check.Message);
            }
            return Ok(check);
        }
        [HttpPost("Login")]
        public IActionResult Login(BusinessUserLoginDto userForLoginDto)
        {
            var result = _businessUserInterface.UserLogin(userForLoginDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            var res = _businessUserInterface.CreateAccessToken(result.Data);
            if (res.Success)
            {
                HttpContext.Response.Headers.Add("Authorization", "Bearer " + res.Data.Token);
                return Ok(res);
            }
            return BadRequest(res.Message);
        }
        [HttpPost("Update")]
        public IActionResult Update(BusinessUser businessUser, string id)
        {
            var result = _businessUserInterface.Update(businessUser, id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _businessUserInterface.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("GetById")]
        public IActionResult Get(string id)
        {
            var result = _businessUserInterface.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _businessUserInterface.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

    }
}
