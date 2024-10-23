using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using horta_facil_api.Models;
using horta_facil_api.Service;

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

        [HttpGet]
        public ActionResult<IEnumerable<Recursos>> GetAll()
        {
            var recursos = _recursosService.GetAll();
            return Ok(recursos);
        }

        [HttpGet("{id}")]
        public ActionResult<Recursos> GetById(Guid id)
        {
            var recurso = _recursosService.GetById(id);
            if (recurso == null)
            {
                return NotFound(); // Retorna 404 se não encontrar
            }
            return Ok(recurso);
        }

        [HttpPost]
        public ActionResult<Recursos> Create([FromBody] Recursos recurso)
        {
            if (recurso == null)
            {
                return BadRequest(); // Retorna 400 se o corpo da requisição for nulo
            }

            var createdRecurso = _recursosService.Create(recurso);
            return CreatedAtAction(nameof(GetById), new { id = createdRecurso.Id }, createdRecurso); // Retorna 201 e o recurso criado
        }

        [HttpPut("{id}")]
        public ActionResult<Recursos> Update(Guid id, [FromBody] Recursos recurso)
        {
            if (recurso == null)
            {
                return BadRequest(); // Retorna 400 se o corpo da requisição for nulo
            }

            var updatedRecurso = _recursosService.Update(id, recurso);
            if (updatedRecurso == null)
            {
                return NotFound(); // Retorna 404 se o recurso não for encontrado
            }
            return Ok(updatedRecurso); // Retorna 200 com o recurso atualizado
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var result = _recursosService.Delete(id);
            if (!result)
            {
                return NotFound(); // Retorna 404 se o recurso não for encontrado
            }
            return NoContent(); // Retorna 204 sem conteúdo
        }
    }
}
