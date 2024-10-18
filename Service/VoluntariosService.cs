using horta_facil_api.Data;
using horta_facil_api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace horta_facil_api.Services
{
    public class VoluntariosService
    {
        private readonly IMongoCollection<Voluntarios> _voluntarios;

        public VoluntariosService(MongoDbContext context)
        {
            _voluntarios = context.Voluntarios;
        }

        // Método para adicionar um novo voluntário
        public async Task<Voluntarios> AddVoluntarioAsync(Voluntarios voluntario)
        {
            await _voluntarios.InsertOneAsync(voluntario);
            return voluntario;
        }

        // Método para obter todos os voluntários
        public async Task<List<Voluntarios>> GetAllVoluntariosAsync()
        {
            return await _voluntarios.Find(v => true).ToListAsync();
        }

        // Método para obter um voluntário pelo ID
        public async Task<string> GetVoluntarioByIdAsync(Guid id)
        {
            var voluntario = await _voluntarios.Find(v => v.Id == id).FirstOrDefaultAsync();

            if (voluntario == null)
            {
                return "ID não encontrado"; // Retorna mensagem personalizada
            }

            return $"Voluntário encontrado: {voluntario.Nome}"; // Retorna o nome ou qualquer outro detalhe desejado
        }


        // Método para atualizar um voluntário
        public async Task<Voluntarios> UpdateVoluntarioAsync(Guid id, Voluntarios voluntario)
        {
            var voluntarioExistente = await _voluntarios.Find(v => v.Id == id).FirstOrDefaultAsync();

            if (voluntarioExistente == null)
            {
                // Log a mensagem antes de lançar a exceção
                Console.WriteLine($"Tentativa de atualização falhou: ID {id} não encontrado.");
                throw new Exception("ID não encontrado");
            }

            voluntario.Id = id;
            await _voluntarios.ReplaceOneAsync(v => v.Id == id, voluntario);
            return voluntario;
        }



        // Método para excluir um voluntário
        public async Task<bool> DeleteVoluntarioAsync(Guid id)
        {
            // Verifica se o voluntário existe
            var voluntarioExistente = await _voluntarios.Find(v => v.Id == id).FirstOrDefaultAsync();

            // Retorna falso se o ID não existir
            if (voluntarioExistente == null)
            {
                return false;
            }

            // Exclui o voluntário se ele existir
            await _voluntarios.DeleteOneAsync(v => v.Id == id);
            return true; // Retorna verdadeiro se a exclusão for bem-sucedida
        }


    }
}
