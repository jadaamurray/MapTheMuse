using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Data;
using MapTheMuseApi.Models;
using MapTheMuseApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace MapTheMuseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationsController : ControllerBase
    {
        private readonly IDestinationService _destinationService;

        public DestinationsController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DestinationListDto>>> GetAll()
        => Ok(await _destinationService.GetAllDestinationsAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<DestinationDetailDto>> GetById(int id)
        {
            var dto = await _destinationService.GetDestinationByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DestinationDetailDto>> Create([FromBody] DestinationCreateUpdateDto dto)
        {
            var created = await _destinationService.CreateDestinationAsync(dto);
            return CreatedAtAction(nameof(GetById),
                new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] DestinationCreateUpdateDto dto)
        {
            if (!await _destinationService.UpdateDestinationAsync(id, dto))
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _destinationService.DeleteDestinationAsync(id))
                return NotFound();
            return NoContent();
        }
    }
}
