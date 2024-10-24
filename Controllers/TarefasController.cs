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
            // Verifica se a tarefa é nula
            if (tarefa == null)
            {
                return BadRequest(new { Mensagem = "Tarefa não pode ser nula." });
            }

            // Se idVoluntario não for enviado, ele deve ser null
            if (tarefa.IdVoluntario == Guid.Empty) // Opcional, se você quiser tratar especificamente
            {
                tarefa.IdVoluntario = null;
            }

            // Chama o serviço para criar a tarefa
            var resultado = await _tarefaService.CriarTarefa(tarefa);

            // Verifica se a tarefa foi criada com sucesso
            if (resultado != null)
            {
                return Ok(new { Mensagem = "Tarefa registrada com sucesso!", Tarefa = resultado });
            }
            else
            {
                return BadRequest(new { Mensagem = "Não foi possível registrar a tarefa." });
            }
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
            try
            {
                var tarefaAtualizada = await _tarefaService.DefinirStatusTarefa(id, novoStatus);
                return Ok(tarefaAtualizada);
            }
            catch (NotFoundException ex) // Captura a exceção personalizada
            {
                return NotFound(new { Mensagem = ex.Message }); // Retorna a mensagem da exceção
            }
            catch (Exception ex) // Captura outras exceções
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro inesperado.", Detalhes = ex.Message });
            }
        }


    }
}
