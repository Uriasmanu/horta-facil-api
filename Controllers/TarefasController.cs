using Microsoft.AspNetCore.Mvc;
using horta_facil_api.Service;
using horta_facil_api.Models;
using horta_facil_api.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace horta_facil_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly TarefasService _tarefasService;

        public TarefasController(TarefasService tarefasService)
        {
            _tarefasService = tarefasService;
        }

        // POST: api/tarefas
        [HttpPost]
        public async Task<ActionResult<Tarefas>> Create([FromBody] TarefasDTO tarefaDto)
        {
            if (tarefaDto == null)
            {
                return BadRequest("Tarefa inválida.");
            }

            try
            {
                var novaTarefa = await _tarefasService.CreateAsync(tarefaDto);
                return CreatedAtAction(nameof(GetById), new { id = novaTarefa.Id }, novaTarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Retorna uma mensagem de erro se o voluntário não for encontrado
            }
        }

        // GET: api/tarefas
        [HttpGet]
        public async Task<ActionResult<List<Tarefas>>> GetAll()
        {
            var tarefas = await _tarefasService.GetAllAsync();
            if (tarefas == null || tarefas.Count == 0)
            {
                return NotFound("Nenhuma tarefa encontrada."); // Retorna 404 se a lista estiver vazia
            }
            return Ok(tarefas);
        }

        // GET: api/tarefas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarefas>> GetById(Guid id)
        {
            var tarefa = await _tarefasService.GetByIdAsync(id);
            if (tarefa == null)
            {
                return NotFound($"Tarefa com ID {id} não encontrada."); // Retorna 404 se a tarefa não existir
            }
            return Ok(tarefa);
        }

        // PUT: api/tarefas/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] TarefasDTO tarefaDto)
        {
            if (tarefaDto == null)
            {
                return BadRequest("Tarefa inválida.");
            }

            try
            {
                var resultado = await _tarefasService.UpdateAsync(id, tarefaDto);
                if (!resultado)
                {
                    return NotFound($"Tarefa com ID {id} não encontrada."); // Retorna 404 se a tarefa não existir
                }

                return NoContent(); // Retorna 204 No Content em caso de sucesso
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Retorna uma mensagem de erro se o voluntário não for encontrado
            }
        }

        // DELETE: api/tarefas/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var resultado = await _tarefasService.DeleteAsync(id);
            if (!resultado)
            {
                return NotFound($"Tarefa com ID {id} não encontrada."); // Retorna 404 se a tarefa não existir
            }

            return NoContent(); // Retorna 204 No Content em caso de sucesso
        }
    }
}
