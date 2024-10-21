using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using horta_facil_api.Models;
using horta_facil_api.Services;
using static horta_facil_api.Services.TarefaService;

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
        public async Task<IActionResult> AtualizarTarefa(Guid id, [FromBody] TarefaAtualizacaoDTO tarefaAtualizada)
        {
            if (tarefaAtualizada == null)
                return BadRequest("Dados da tarefa inválidos.");

            // Mapeia TarefaAtualizacaoDTO para Tarefas
            var tarefa = new Tarefas
            {
                Id = id,
                Nome = tarefaAtualizada.Nome,
                Descricao = tarefaAtualizada.Descricao,
                // Mapeie outras propriedades conforme necessário
            };

            try
            {
                await _tarefaService.AtualizarTarefa(tarefa);
                return NoContent(); // Retorna 204 No Content se a atualização for bem-sucedida
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 Not Found se a tarefa não for encontrada
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> ObterTarefaPorId(Guid id)
        {
            try
            {
                var tarefa = await _tarefaService.ObterTarefaPorId(id);
                return Ok(tarefa); // Retorna a tarefa encontrada com status 200
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message }); // Retorno 404 com a mensagem de erro
            }
            catch (Exception ex)
            {
                // Loga a exceção (opcional)
                return StatusCode(500, new { mensagem = "Erro interno do servidor.", detalhes = ex.Message }); // Retorno 500
            }
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
            try
            {
                var tarefaRemovida = await _tarefaService.RemoverTarefa(id);
                if (!tarefaRemovida)
                {
                    return NotFound($"A tarefa com o ID '{id}' não foi encontrada."); // Resposta 404 se a tarefa não existir
                }

                return NoContent(); // Retorna 204 No Content se a tarefa for removida com sucesso
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 com a mensagem da exceção personalizada
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover a tarefa: {ex.Message}"); // Retorna 500 em caso de erro
            }
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
