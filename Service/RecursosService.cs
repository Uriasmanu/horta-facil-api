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

        public IEnumerable<Recursos> GetAll()
        {
            return _recursos.Find(recurso => true).ToList(); // Retorna todos os recursos
        }

        public Recursos GetById(Guid id)
        {
            var recurso = _recursos.Find(recurso => recurso.Id == id).FirstOrDefault(); // Retorna um recurso específico
            if (recurso == null)
            {
                throw new KeyNotFoundException($"Recurso com ID '{id}' não encontrado."); // Mensagem personalizada para 404
            }
            return recurso;
        }

        public Recursos Create(Recursos recurso)
        {
            recurso.Id = Guid.NewGuid(); // Garante que um novo ID é gerado
            _recursos.InsertOne(recurso); // Insere o recurso no MongoDB
            return recurso;
        }

        public Recursos Update(Guid id, Recursos recurso)
        {
            var existingRecurso = GetById(id);
            // Se já lançamos uma exceção em GetById, não precisamos mais verificar aqui.

            // Atualiza os campos do recurso
            existingRecurso.Nome = recurso.Nome;
            existingRecurso.TipoRecurso = recurso.TipoRecurso;
            existingRecurso.DataCriacao = DateTime.Now; // Atualiza a data de criação

            _recursos.ReplaceOne(r => r.Id == id, existingRecurso); // Atualiza o recurso no MongoDB
            return existingRecurso;
        }

        public bool Delete(Guid id)
        {
            var resultado = _recursos.DeleteOne(recurso => recurso.Id == id); // Remove o recurso do MongoDB
            if (resultado.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"Recurso com ID '{id}' não encontrado para exclusão."); // Mensagem personalizada para 404
            }
            return true; // Retorna true se o recurso foi deletado
        }
    }
}
