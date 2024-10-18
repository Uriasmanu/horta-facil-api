using horta_facil_api.DTOs;
using horta_facil_api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace horta_facil_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoluntariosController : ControllerBase
    {
        private readonly VoluntariosService _voluntariosService;

        public VoluntariosController(VoluntariosService voluntariosService)
        {
            _voluntariosService = voluntariosService;
        }

        // GET: api/voluntarios
        [HttpGet]
        public async Task<ActionResult<List<VoluntarioDTO>>> GetVoluntarios()
        {
            var voluntarios = await _voluntariosService.GetVoluntariosAsync();
            return Ok(voluntarios);
        }

        // GET: api/voluntarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<VoluntarioDTO>> GetVoluntarioById(Guid id)
        {
            var voluntario = await _voluntariosService.GetVoluntarioByIdAsync(id);
            if (voluntario == null)
                return NotFound("Voluntário não encontrado.");

            return Ok(voluntario);
        }

        // POST: api/voluntarios
        [HttpPost]
        public async Task<ActionResult<VoluntarioDTO>> AddVoluntario([FromBody] VoluntarioDTO voluntarioDto)
        {
            if (voluntarioDto == null)
                return BadRequest("Dados inválidos.");

            var novoVoluntario = await _voluntariosService.AddVoluntarioAsync(voluntarioDto);
            return CreatedAtAction(nameof(GetVoluntarioById), new { id = novoVoluntario.Id }, novoVoluntario);
        }

        // PUT: api/voluntarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoluntario(Guid id, [FromBody] VoluntarioDTO voluntarioDto)
        {
            if (voluntarioDto == null || id == Guid.Empty)
                return BadRequest("Dados inválidos.");

            var updateResult = await _voluntariosService.UpdateVoluntarioAsync(id, voluntarioDto);
            if (!updateResult)
                return NotFound("Voluntário não encontrado.");

            return NoContent(); // Status 204
        }

        // DELETE: api/voluntarios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveVoluntario(Guid id)
        {
            var deleteResult = await _voluntariosService.RemoveVoluntarioAsync(id);
            if (!deleteResult)
                return NotFound("Voluntário não encontrado.");

            return NoContent();
        }

        // POST: api/voluntarios/{voluntarioId}/tarefa/{tarefaId}
        [HttpPost("{voluntarioId}/tarefa/{tarefaId}")]
        public async Task<IActionResult> AssignTarefa(Guid voluntarioId, Guid tarefaId)
        {
            var result = await _voluntariosService.AssignTarefaToVoluntarioAsync(voluntarioId, tarefaId);
            if (!result)
                return NotFound("Voluntário ou Tarefa não encontrados.");

            return NoContent();
        }
    }
}
