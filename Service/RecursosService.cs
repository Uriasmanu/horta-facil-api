using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using horta_facil_api.Models;
using horta_facil_api.Data;

namespace horta_facil_api.Service
{
    public class RecursosService
    {
        private readonly IMongoCollection<Recursos> _recursos;

        public RecursosService(MongoDbContext context)
        {
            _recursos = context.Recursos;
        }

        public async Task<List<Recursos>> GetAll()
        {

            return await _recursos.Find(t => true).ToListAsync();
        }

        public RecursosDTO GetById(Guid id)
        {
            var recurso = _recursos.Find(r => r.Id == id).FirstOrDefault();
            if (recurso == null)
            {
                throw new KeyNotFoundException($"Recurso com ID '{id}' não encontrado.");
            }
            return new RecursosDTO
            {
                Nome = recurso.Nome,
                TipoRecurso = recurso.TipoRecurso
            };
        }

        public Recursos Create(RecursosDTO recursoDTO)
        {
            var recurso = new Recursos
            {
                Id = Guid.NewGuid(),
                Nome = recursoDTO.Nome,
                TipoRecurso = recursoDTO.TipoRecurso,

            };

            _recursos.InsertOne(recurso);
            return recurso;
        }


        public RecursosDTO Update(Guid id, RecursosDTO recursoDTO)
        {
            var existingRecurso = _recursos.Find(r => r.Id == id).FirstOrDefault();
            if (existingRecurso == null)
            {
                throw new KeyNotFoundException($"Recurso com ID '{id}' não encontrado.");
            }

            // Atualiza os campos do recurso
            existingRecurso.Nome = recursoDTO.Nome;
            existingRecurso.TipoRecurso = recursoDTO.TipoRecurso;

            _recursos.ReplaceOne(r => r.Id == id, existingRecurso);

            // Retorna o DTO atualizado
            return new RecursosDTO
            {
                Nome = existingRecurso.Nome,
                TipoRecurso = existingRecurso.TipoRecurso
            };
        }

        public bool Delete(Guid id)
        {
            var resultado = _recursos.DeleteOne(r => r.Id == id);
            if (resultado.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"Recurso com ID '{id}' não encontrado para exclusão.");
            }
            return true;
        }
    }
}
