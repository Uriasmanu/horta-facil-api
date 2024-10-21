using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using horta_facil_api.Data;
using horta_facil_api.Models;
using MongoDB.Driver;

namespace horta_facil_api.Services
{
    public class TarefaService
    {
        private readonly IMongoCollection<Tarefas> _tarefas;

        public TarefaService(MongoDbContext context)
        {
            _tarefas = context.Tarefas;
        }

        public async Task<Tarefas> CriarTarefa(Tarefas tarefa)
        {
            await _tarefas.InsertOneAsync(tarefa); // Insere a nova tarefa no MongoDB
            return tarefa; // Retorna a tarefa criada
        }

        public async Task<Tarefas> AtualizarTarefa(Guid id, Tarefas tarefaAtualizada)
        {
            var resultado = await _tarefas.ReplaceOneAsync(t => t.Id == id, tarefaAtualizada);
            return resultado.IsAcknowledged ? tarefaAtualizada : null; // Retorna a tarefa atualizada se bem-sucedido
        }

        public async Task<Tarefas> ObterTarefaPorId(Guid id)
        {
            var tarefa = await _tarefas.Find(t => t.Id == id).FirstOrDefaultAsync();
            return tarefa; // Retorna a tarefa encontrada
        }

        public async Task<List<Tarefas>> ObterTodasTarefas()
        {
            return await _tarefas.Find(t => true).ToListAsync(); // Retorna todas as tarefas
        }

        public async Task<bool> RemoverTarefa(Guid id)
        {
            var resultado = await _tarefas.DeleteOneAsync(t => t.Id == id);
            return resultado.DeletedCount > 0; // Retorna true se a tarefa foi removida
        }

        public async Task<Tarefas> DefinirStatusTarefa(Guid id, int novoStatus)
        {
            var tarefa = await ObterTarefaPorId(id); // Recupera a tarefa existente
            if (tarefa != null)
            {
                tarefa.DefinirStatus(novoStatus); // Atualiza o status
                await AtualizarTarefa(id, tarefa); // Salva a tarefa atualizada
            }
            return tarefa; // Retorna a tarefa com o status atualizado
        }
    }
}