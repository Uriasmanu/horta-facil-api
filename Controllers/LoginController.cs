using horta_facil_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace horta_facil_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        // POST: api/Login/register
        [HttpPost("register")]
        public async Task<ActionResult<LoginDTO>> Register(RegisterDTO registerDTO)
        {
            if (await _loginService.UserExistsAsync(registerDTO.Username))
            {
                return Conflict("Username already exists");
            }

            // Convertendo RegisterDTO para Login
            var login = new Login
            {
                Username = registerDTO.Username,
                Password = registerDTO.Password
            };

            var createdUser = await _loginService.CreateUserAsync(login);
            var createdUserDTO = new LoginDTO
            {
                Id = createdUser.Id,
                Username = createdUser.Username
            };

            return CreatedAtAction(nameof(GetLogin), new { id = createdUserDTO.Id }, createdUserDTO);
        }


        // GET: api/Login/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoginDTO>> GetLogin(Guid id)
        {
            var loginDTO = await _loginService.GetUserByIdAsync(id);

            if (loginDTO == null)
            {
                return NotFound();
            }

            return Ok(loginDTO);
        }

        // GET: api/Login
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginDTO>>> GetAllLogins()
        {
            var loginsDTO = await _loginService.GetAllUsersAsync();
            return Ok(loginsDTO);
        }
    }
}
