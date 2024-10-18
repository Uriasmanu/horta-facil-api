using horta_facil_api.Data;
using horta_facil_api.Models;
using horta_facil_api.DTOs;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace horta_facil_api.Service
{
    public class TarefasService
    {
        private readonly IMongoCollection<Tarefas> _tarefas;
        private readonly IMongoCollection<Voluntarios> _voluntarios; // Coleção de voluntários

        public TarefasService(MongoDbContext context)
        {
            _tarefas = context.Tarefas;
            _voluntarios = context.Voluntarios; // Inicializa a coleção de voluntários
        }

        // Verifica se o voluntário existe
        private async Task<bool> VoluntarioExiste(Guid voluntarioId)
        {
            var voluntario = await _voluntarios.Find(v => v.Id == voluntarioId).FirstOrDefaultAsync();
            return voluntario != null;
        }

        // Create: Adiciona uma nova tarefa com o status padrão como "Pendente"
        public async Task<Tarefas> CreateAsync(TarefasDTO tarefaDto)
        {
            // Verifica se o voluntário existe
            if (!await VoluntarioExiste(tarefaDto.Voluntario.Id))
            {
                throw new Exception("Voluntário não encontrado.");
            }

            // Mapeia o DTO para o modelo Tarefas
            var novaTarefa = new Tarefas
            {
                Id = Guid.NewGuid(), // Gera um novo Id para a tarefa
                Nome = tarefaDto.Nome,
                Descricao = tarefaDto.Descricao,
                Status = StatusTarefa.Pendente, // Define o status padrão como "Pendente"
                Data = DateTime.UtcNow, // Adiciona a data atual
                Voluntario = new Voluntarios
                {
                    Id = tarefaDto.Voluntario.Id,
                    Nome = tarefaDto.Voluntario.Nome
                }
            };

            await _tarefas.InsertOneAsync(novaTarefa);
            return novaTarefa;
        }

        // Read: Obtém uma tarefa por Id
        public async Task<Tarefas> GetByIdAsync(Guid id)
        {
            return await _tarefas.Find(tarefa => tarefa.Id == id).FirstOrDefaultAsync();
        }

        // Read: Obtém todas as tarefas
        public async Task<List<Tarefas>> GetAllAsync()
        {
            return await _tarefas.Find(tarefa => true).ToListAsync();
        }

        // Update: Atualiza uma tarefa por Id
        public async Task<bool> UpdateAsync(Guid id, TarefasDTO tarefaDto)
        {
            // Verifica se o voluntário existe
            if (!await VoluntarioExiste(tarefaDto.Voluntario.Id))
            {
                throw new Exception("Voluntário não encontrado.");
            }

            var tarefaExistente = await GetByIdAsync(id);
            if (tarefaExistente == null)
            {
                return false; // Tarefa não encontrada
            }

            // Mapeia o DTO para a tarefa existente
            tarefaExistente.Nome = tarefaDto.Nome;
            tarefaExistente.Descricao = tarefaDto.Descricao;
            tarefaExistente.Voluntario = new Voluntarios
            {
                Id = tarefaDto.Voluntario.Id,
                Nome = tarefaDto.Voluntario.Nome
            };

            var resultado = await _tarefas.ReplaceOneAsync(tarefa => tarefa.Id == id, tarefaExistente);
            return resultado.ModifiedCount > 0;
        }

        // Delete: Remove uma tarefa por Id
        public async Task<bool> DeleteAsync(Guid id)
        {
            var resultado = await _tarefas.DeleteOneAsync(tarefa => tarefa.Id == id);
            return resultado.DeletedCount > 0;
        }
    }
}
