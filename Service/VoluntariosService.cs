using horta_facil_api.Data;
using horta_facil_api.DTOs;
using horta_facil_api.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace horta_facil_api.Services
{
    public class VoluntariosService
    {
        private readonly IMongoCollection<Voluntarios> _voluntarios;
        private readonly IMongoCollection<Tarefas> _tarefas;

        public VoluntariosService(MongoDbContext context)
        {
            _voluntarios = context.Voluntarios;
            _tarefas = context.Tarefas;
        }

        // Método para buscar todos os voluntários
        public async Task<List<VoluntarioDTO>> GetVoluntariosAsync()
        {
            var voluntarios = await _voluntarios.Find(v => true).ToListAsync();
            return voluntarios.Select(v => new VoluntarioDTO
            {
                Id = v.Id,
                Nome = v.Nome
            }).ToList();
        }

        // Método para buscar voluntário por ID
        public async Task<VoluntarioDTO?> GetVoluntarioByIdAsync(Guid id)
        {
            var voluntario = await _voluntarios.Find(v => v.Id == id).FirstOrDefaultAsync();
            if (voluntario == null) return null;

            return new VoluntarioDTO
            {
                Id = voluntario.Id,
                Nome = voluntario.Nome
            };
        }

        // Método para adicionar um novo voluntário
        public async Task<VoluntarioDTO> AddVoluntarioAsync(VoluntarioDTO voluntarioDto)
        {
            var voluntario = new Voluntarios
            {
                Id = Guid.NewGuid(),
                Nome = voluntarioDto.Nome,
                Idade = 0 // Defina conforme o necessário ou adicione ao DTO
            };

            await _voluntarios.InsertOneAsync(voluntario);

            return new VoluntarioDTO
            {
                Id = voluntario.Id,
                Nome = voluntario.Nome
            };
        }

        // Método para atualizar um voluntário existente
        public async Task<bool> UpdateVoluntarioAsync(Guid id, VoluntarioDTO voluntarioDto)
        {
            var voluntario = await _voluntarios.Find(v => v.Id == id).FirstOrDefaultAsync();
            if (voluntario == null) return false;

            voluntario.Nome = voluntarioDto.Nome;

            var updateResult = await _voluntarios.ReplaceOneAsync(v => v.Id == id, voluntario);

            return updateResult.ModifiedCount > 0;
        }

        // Método para remover um voluntário por ID
        public async Task<bool> RemoveVoluntarioAsync(Guid id)
        {
            var deleteResult = await _voluntarios.DeleteOneAsync(v => v.Id == id);
            return deleteResult.DeletedCount > 0;
        }

        // Método para associar uma tarefa a um voluntário
        public async Task<bool> AssignTarefaToVoluntarioAsync(Guid voluntarioId, Guid tarefaId)
        {
            var voluntario = await _voluntarios.Find(v => v.Id == voluntarioId).FirstOrDefaultAsync();
            if (voluntario == null) return false;

            var tarefa = await _tarefas.Find(t => t.Id == tarefaId).FirstOrDefaultAsync();
            if (tarefa == null) return false;

            voluntario.Tarefa = tarefa;

            var updateResult = await _voluntarios.ReplaceOneAsync(v => v.Id == voluntarioId, voluntario);
            return updateResult.ModifiedCount > 0;
        }
    }
}
