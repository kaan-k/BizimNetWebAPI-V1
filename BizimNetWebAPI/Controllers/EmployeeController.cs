using Business.Abstract;
using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("Add")]
        public IActionResult Add(EmployeeDto employee)
        {
            var result = _employeeService.Add(employee);
            return Ok(result);
        }
        [HttpPost("AddRange")]
        public IActionResult AddRange(List<EmployeeDto> employee)
        {
            var result = _employeeService.AddRange(employee);
            return Ok(result);
        }
        [HttpPost("Update")]
        public IActionResult Update(EmployeeDto employee, string employeeId)
        {
            var result = _employeeService.Update(employee, employeeId);
            return Ok(result);
        }
        [HttpPost("UpdateRange")]
        public IActionResult UpdateRange(List<Employee> employee)
        {
            var result = _employeeService.UpdateRange(employee);
            return Ok(result);
        }
        [HttpPost("AssignToDepartment")]
        public IActionResult AssignToDepartment(string employeeId, string departmentId)
        {
            var result = _employeeService.AssignToDepartment(employeeId, departmentId);
            return Ok(result);
        }
        [HttpPost("AssignRole")]
        public IActionResult AssignRole(string employeeId, string role)
        {
            var result = _employeeService.AssignRole(employeeId, role);
            return Ok(result);
        }
        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            var result = _employeeService.GetById(id);
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _employeeService.GetAll();
            return Ok(result);
        }
        [HttpGet("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _employeeService.Delete(id);
            return Ok(result);
        }
        [HttpGet("GetByDepartmentId")]
        public IActionResult GetByDepartmentId(string departmentId)
        {
            var result = _employeeService.GetByDepartmentId(departmentId);
            return Ok(result);
        }
        [HttpGet("GetByRole")]
        public IActionResult GetByRole(string role)
        {
            var result = _employeeService.GetByRole(role);
            return Ok(result);
        }
        [HttpGet("GetManagerByDepartment")]
        public IActionResult GetManagerByDepartment(string departmentId)
        {
            var result = _employeeService.GetManagerByDepartment(departmentId);
            return Ok(result);
        }
        [HttpGet("GetTotalEmployeeCount")]
        public IActionResult GetTotalEmployeeCount()
        {
            var result = _employeeService.GetTotalEmployeeCount();
            return Ok(result);
        }
        [HttpGet("GetCountByRole")]
        public IActionResult GetCountByRole(string role)
        {
            var result = _employeeService.GetCountByRole(role);
            return Ok(result);
        }
        [HttpGet("GetCountByDepartment")]
        public IActionResult GetCountByDepartment(string departmentId)
        {
            var result = _employeeService.GetCountByDepartment(departmentId);
            return Ok(result);
        }

    }
}
