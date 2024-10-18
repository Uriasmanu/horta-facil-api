using horta_facil_api.Data;
using horta_facil_api.Models;
using horta_facil_api.Services;
using Microsoft.AspNetCore.Mvc;

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

        // POST api/voluntarios
        [HttpPost]
        public async Task<ActionResult<Voluntarios>> AddVoluntarioAsync([FromBody] Voluntarios voluntario)
        {
            var novoVoluntario = await _voluntariosService.AddVoluntarioAsync(voluntario);
            return CreatedAtAction(nameof(GetVoluntarioByIdAsync), new { id = novoVoluntario.Id }, novoVoluntario);
        }

        // GET api/voluntarios
        [HttpGet]
        public async Task<ActionResult<List<Voluntarios>>> GetAllVoluntariosAsync()
        {
            var voluntarios = await _voluntariosService.GetAllVoluntariosAsync();
            return Ok(voluntarios);
        }

        // GET api/voluntarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetVoluntarioByIdAsync(Guid id)
        {
            var resultado = await _voluntariosService.GetVoluntarioByIdAsync(id);
            if (resultado.Contains("não encontrado"))
            {
                return NotFound(resultado);
            }
            return Ok(resultado);
        }

        // PUT api/voluntarios/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Voluntarios>> UpdateVoluntarioAsync(Guid id, [FromBody] Voluntarios voluntario)
        {
            try
            {
                var voluntarioAtualizado = await _voluntariosService.UpdateVoluntarioAsync(id, voluntario);
                return Ok(voluntarioAtualizado);
            }
            catch (Exception)
            {
                return NotFound("ID não encontrado");
            }
        }

        // DELETE api/voluntarios/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVoluntarioAsync(Guid id)
        {
            var resultado = await _voluntariosService.DeleteVoluntarioAsync(id);
            if (!resultado)
            {
                return NotFound("ID não encontrado");
            }
            return NoContent();
        }
    }
}
