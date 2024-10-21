using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using horta_facil_api.Models;
using horta_facil_api.Services;

namespace horta_facil_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly TarefaService _tarefaService;

        public TarefasController(TarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarTarefa([FromBody] Tarefas tarefa)
        {
            if (tarefa == null)
                return BadRequest("Tarefa inválida.");

            var novaTarefa = await _tarefaService.CriarTarefa(tarefa);
            return CreatedAtAction(nameof(ObterTarefaPorId), new { id = novaTarefa.Id }, novaTarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarTarefa(Guid id, [FromBody] Tarefas tarefaAtualizada)
        {
            if (tarefaAtualizada == null || id != tarefaAtualizada.Id)
                return BadRequest("Dados da tarefa inválidos.");

            var resultado = await _tarefaService.AtualizarTarefa(id, tarefaAtualizada);
            if (resultado == null)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterTarefaPorId(Guid id)
        {
            var tarefa = await _tarefaService.ObterTarefaPorId(id);
            if (tarefa == null)
                return NotFound();

            return Ok(tarefa);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodasTarefas()
        {
            var tarefas = await _tarefaService.ObterTodasTarefas();
            return Ok(tarefas);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverTarefa(Guid id)
        {
            var removido = await _tarefaService.RemoverTarefa(id);
            if (!removido)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> DefinirStatusTarefa(Guid id, [FromBody] int novoStatus)
        {
            var tarefaAtualizada = await _tarefaService.DefinirStatusTarefa(id, novoStatus);
            if (tarefaAtualizada == null)
                return NotFound();

            return Ok(tarefaAtualizada);
        }
    }
}
