using horta_facil_api.Data;
using horta_facil_api.Models;
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

        // Registrar novo usuário
        public async Task<Login> RegistrarUsuarioAsync(LoginModel loginModel)
        {
            // Verificar se o e-mail já está em uso
            var usuarioExistente = await _logins.Find(x => x.Email == loginModel.Email).FirstOrDefaultAsync();
            if (usuarioExistente != null)
            {
                return null;  // E-mail já cadastrado
            }

            // Hash da senha antes de salvar
            var senhaHash = SenhaHasher.HashSenha(loginModel.Password);

            // Criar novo objeto Login
            var novoLogin = new Login
            {
                Id = Guid.NewGuid(),
                Username = loginModel.Email.Split('@')[0],  // Usar o prefixo do e-mail como username
                Email = loginModel.Email,
                Senha = senhaHash,
            };

            // Inserir novo usuário no banco
            await _logins.InsertOneAsync(novoLogin);

            // Retornar o objeto Login do usuário registrado
            return novoLogin;
        }

        // Buscar todos os usuarios
        public async Task<List<Login>> BuscarTodosLogins()
        {
            var logins = await _logins.Find(new BsonDocument()).ToListAsync();
            return logins;
        }

        // Buscar usuario por id
        public async Task<Login> BuscarUsuarioPorId(Guid id)
        {
            var login = await _logins.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (login == null)
            {
                return null;
            }

            return login;
        }

        // Deletar usuario por id
        public async Task<bool> ExcluirUsuarioPorId(Guid id)
        {
            var resultado = await _logins.DeleteOneAsync(x => x.Id == id);
            return resultado.DeletedCount > 0;
        }
    }
}
