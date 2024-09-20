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

        // Registrar usuario
        public async Task<bool> RegistrarLogin(Login novoLogin)
        {
            var loginExistente = await _logins.Find(x => x.Email == novoLogin.Email).FirstOrDefaultAsync();
            if (loginExistente != null)
            {
                return false;
            }

            novoLogin.Senha = SenhaHasher.HashSenha(novoLogin.Senha);

            await _logins.InsertOneAsync(novoLogin);
            return true;
        }

        // Buscar todos os usuarios
        public async Task<List<Login>> BuscarTodosLogins()
        {
            return await _logins.Find(new BsonDocument()).ToListAsync();
        }

        // Buscar usuario por id
        public async Task<Login> BuscarUsuarioPorId(Guid id)
        {
            return await _logins.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        // Deletar usuario por id
        public async Task<bool> ExcluirUsuarioPorId(Guid id)
        {
            var resultado = await _logins.DeleteOneAsync(x => x.Id == id);
            return resultado.DeletedCount > 0;
        }
    }
}
