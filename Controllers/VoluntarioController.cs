using Microsoft.AspNetCore.Mvc;
using horta_facil_api.Models;
using horta_facil_api.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace horta_facil_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoluntariosController : ControllerBase
    {
        private readonly VoluntariosService _voluntariosService;

        public VoluntariosController(VoluntariosService voluntariosService)
        {
            _voluntariosService = voluntariosService;
        }

        // GET: api/voluntarios
        [HttpGet]
        public async Task<ActionResult<List<Voluntarios>>> GetAll()
        {
            var voluntarios = await _voluntariosService.GetAllVoluntariosAsync();
            return Ok(voluntarios);
        }

        // GET: api/voluntarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Voluntarios>> GetById(Guid id)
        {
            var voluntario = await _voluntariosService.GetVoluntarioByIdAsync(id);
            if (voluntario == null)
            {
                return NotFound();
            }
            return Ok(voluntario);
        }

        // POST: api/voluntarios
        [HttpPost]
        public async Task<ActionResult<Voluntarios>> Create([FromBody] Voluntarios voluntario)
        {
            if (voluntario == null)
            {
                return BadRequest();
            }

            voluntario.Id = Guid.NewGuid(); // Gerar um novo ID
            await _voluntariosService.AddVoluntarioAsync(voluntario);
            return CreatedAtAction(nameof(GetById), new { id = voluntario.Id }, voluntario);
        }

        // PUT: api/voluntarios/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Voluntarios>> Update(Guid id, [FromBody] Voluntarios voluntario)
        {
            if (voluntario == null)
            {
                return BadRequest();
            }

            var existingVoluntario = await _voluntariosService.GetVoluntarioByIdAsync(id);
            if (existingVoluntario == null)
            {
                return NotFound();
            }

            await _voluntariosService.UpdateVoluntarioAsync(id, voluntario);
            return NoContent();
        }

        // DELETE: api/voluntarios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingVoluntario = await _voluntariosService.GetVoluntarioByIdAsync(id);
            if (existingVoluntario == null)
            {
                return NotFound();
            }

            await _voluntariosService.DeleteVoluntarioAsync(id);
            return NoContent();
        }
    }
}
