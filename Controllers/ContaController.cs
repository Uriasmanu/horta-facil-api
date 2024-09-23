using horta_facil_api.Models;
using horta_facil_api.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace horta_facil_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly string _chaveSecreta = "e3c46810-b96e-40d9-a9eb-9ffa7b373e5b";

        public ContaController(LoginService loginService)
        {
            _loginService = loginService;
        }

        // POST: api/Conta/authenticate
        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate([FromBody] LoginModel loginModel)
        {
            var user = await _loginService.AuthenticateUserAsync(loginModel.Username, loginModel.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Geração do token JWT
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_chaveSecreta));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "system_tasks",
                audience: "seus_usuarios",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new { token = jwtToken });
        }
    }
}
