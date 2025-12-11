using Business.Abstract;
using Entities.Concrete.Tables;
using Entities.DTOs; // Ensure this namespace exists for TableAddDto
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpPost("Add")]
        public IActionResult Add(TableAddDto tableAddDto)
        {
            var result = _tableService.Add(tableAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(Table table)
        {
            var result = _tableService.Update(table);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _tableService.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _tableService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("MassAdd")]
        public IActionResult MassAdd(TableMassAddDto dto)
        {
            var result = _tableService.MassAdd(dto);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _tableService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // --- Bonus: Crucial for your POS Floor Plan tabs ---
        [HttpGet("GetBySection")]
        public IActionResult GetBySection(int sectionId)
        {
            var result = _tableService.GetBySection(sectionId); // Now returns List<TableDetailDto>
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}