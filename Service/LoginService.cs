using horta_facil_api.Data;
using horta_facil_api.DTOs;
using horta_facil_api.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace horta_facil_api.Service
{
    public class LoginService 
    {
        private readonly IMongoCollection<Login> _logins;
        public LoginService(MongoDbContext context)
        {
            _logins = context.Logins;
        }

        public async Task<Login> AuthenticateUserAsync(string email, string senha)
        {
            var login = await _logins.Find(x => x.Email == email).FirstOrDefaultAsync();

            if (login == null || !SenhaHasher.VerifyPassword(login.Senha, senha))
            {
                return null;  // Se o usuário não for encontrado ou a senha estiver incorreta
            }

            return login; // Retorna o objeto login, que tem as informações do usuário
        }


        // Buscar todos os usuarios
        public async Task<List<LoginDTO>> BuscarTodosLogins()
        {
            // Busca todos os logins do banco de dados
            var logins = await _logins.Find(new BsonDocument()).ToListAsync();

            // Mapeia cada Login para um LoginDTO
            var loginDTOs = logins.Select(login => new LoginDTO
            {
                Id = login.Id,
                UserName = login.Username,
                email = login.Email,

                
            }).ToList();

            return loginDTOs;
        }

        // Buscar usuario por id
        public async Task<LoginDTO> BuscarUsuarioPorId(Guid id)
        {
            var login = await _logins.Find(x => x.Id == id).FirstOrDefaultAsync();
            
            if(login == null)
            {
                return null;
            }

            return new LoginDTO
            {
                Id = login.Id,
                UserName = login.Username,
                email = login.Email,
            };

        }

        // Deletar usuario por id
        public async Task<bool> ExcluirUsuarioPorId(Guid id)
        {
            var resultado = await _logins.DeleteOneAsync(x => x.Id == id);
            return resultado.DeletedCount > 0;
        }
    }
}
