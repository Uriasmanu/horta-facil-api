using horta_facil_api.Data;
using horta_facil_api.Models;
using System;

namespace horta_facil_api.Service
{
    private readonly AppDbContext _context;

    public LoginService (AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _context.Logins.AnyAsync(l => l.Username == username);
    }

    public async Task<Login> CreateUserAsync(Login login)
    {
        login.Password = SenhaHasher.HashSenha(login.Password); // Hashing da senha
        _context.Logins.Add(login);
        await _context.SaveChangesAsync();
        return login;
    }


    public async Task<Login?> AuthenticateUserAsync(string username, string password)
    {
        var user = await _context.Logins.FirstOrDefaultAsync(l => l.Username == username);

        if (user != null && SenhaHasher.VerifyPassword(user.Password, password))
        {
            return user;
        }

        return null;
    }

    public async Task<LoginDTO?> GetUserByIdAsync(Guid id)
    {
        var login = await _context.Logins.FindAsync(id);

        if (login == null) return null;

        return new LoginDTO
        {
            Id = login.Id,
            Username = login.Username
        };
    }

    public async Task<IEnumerable<LoginDTO>> GetAllUsersAsync()
    {
        return await _context.Logins
            .Select(login => new LoginDTO
            {
                Id = login.Id,
                Username = login.Username
            })
            .ToListAsync();
    }
}
