using Microsoft.AspNetCore.Mvc;
using horta_facil_api.Models;
using horta_facil_api.Service;
using System;
using System.Collections.Generic;

namespace horta_facil_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursosController : ControllerBase
    {
        private readonly RecursosService _recursosService;

        public RecursosController(RecursosService recursosService)
        {
            _recursosService = recursosService;
        }

        // GET: api/recursos
        [HttpGet]
        public ActionResult<IEnumerable<RecursosDTO>> GetAll()
        {
            var recursos = _recursosService.GetAll();
            return Ok(recursos); // Retorna todos os recursos como DTOs
        }

        // GET: api/recursos/{id}
        [HttpGet("{id}")]
        public ActionResult<RecursosDTO> GetById(Guid id)
        {
            try
            {
                var recurso = _recursosService.GetById(id);
                return Ok(recurso); // Retorna o recurso específico como DTO
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 se não encontrado
            }
        }

        // POST: api/recursos
        [HttpPost]
        public ActionResult<Recursos> Create(RecursosDTO recursoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna 400 se o modelo for inválido
            }

            var novoRecurso = _recursosService.Create(recursoDTO);
            return CreatedAtAction(nameof(GetById), new { id = novoRecurso.Id }, novoRecurso); // Retorna 201 Created
        }

        // PUT: api/recursos/{id}
        [HttpPut("{id}")]
        public ActionResult<Recursos> Update(Guid id, RecursosDTO recursoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna 400 se o modelo for inválido
            }

            try
            {
                var recursoAtualizado = _recursosService.Update(id, recursoDTO);
                return Ok(recursoAtualizado); // Retorna 200 OK com o recurso atualizado
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 se não encontrado
            }
        }

        // DELETE: api/recursos/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _recursosService.Delete(id);
                return NoContent(); // Retorna 204 No Content se a exclusão foi bem-sucedida
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 se não encontrado
            }
        }
    }
}
