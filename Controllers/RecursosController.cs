using Microsoft.AspNetCore.Mvc;
using horta_facil_api.Models;
using horta_facil_api.Service;
using System;

namespace horta_facil_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecursosController : ControllerBase
    {
        private readonly RecursosService _recursosService;

        public RecursosController(RecursosService recursosService)
        {
            _recursosService = recursosService;
        }

        // GET: api/recursos
        [HttpGet]
        public ActionResult<IEnumerable<Recursos>> GetAll()
        {
            var recursos = _recursosService.GetAll();
            return Ok(recursos);
        }

        // GET: api/recursos/{id}
        [HttpGet("{id}")]
        public ActionResult<Recursos> GetById(Guid id)
        {
            try
            {
                var recurso = _recursosService.GetById(id);
                return Ok(recurso);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 com a mensagem da exceção
            }
        }

        // POST: api/recursos
        [HttpPost]
        public ActionResult<Recursos> Create([FromBody] Recursos recurso)
        {
            if (recurso == null)
            {
                return BadRequest("Recurso não pode ser nulo."); // Retorna 400 se o recurso for nulo
            }

            var createdRecurso = _recursosService.Create(recurso);
            return CreatedAtAction(nameof(GetById), new { id = createdRecurso.Id }, createdRecurso); // Retorna 201 com o recurso criado
        }

        // PUT: api/recursos/{id}
        [HttpPut("{id}")]
        public ActionResult<Recursos> Update(Guid id, [FromBody] Recursos recurso)
        {
            if (recurso == null)
            {
                return BadRequest("Recurso não pode ser nulo."); // Retorna 400 se o recurso for nulo
            }

            try
            {
                var updatedRecurso = _recursosService.Update(id, recurso);
                return Ok(updatedRecurso);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 com a mensagem da exceção
            }
        }

        // DELETE: api/recursos/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var result = _recursosService.Delete(id);
                if (!result)
                {
                    return NotFound($"Recurso com ID '{id}' não encontrado para exclusão."); // Mensagem personalizada
                }
                return NoContent(); // Retorna 204 sem conteúdo
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 com a mensagem da exceção
            }
        }
    }
}
