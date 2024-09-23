﻿using horta_facil_api.Models;
using horta_facil_api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace horta_facil_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly string _chaveSecreta = "e3c46810-b96e-40d9-a9eb-9ffa7b373e5b";

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Registrar([FromBody] Login novoLogin)
        {
            var registroBemSucedido = await _loginService.RegistrarLogin(novoLogin);

            if (!registroBemSucedido)
            {
                return BadRequest("Email já registrado");
            }

            return Ok("Usuario registrado com sucesso");
        }

        [HttpGet("usuarios")]
        public async Task<ActionResult> MostrarUsuarios()
        {
            var usuarios = await _loginService.BuscarTodosLogins();
            return Ok(usuarios);
        }

        [HttpGet("usuario/{id}")]
        public async Task<ActionResult> UsuarioPorId(string id)
        {
            var usuario = await _loginService.BuscarUsuarioPorId(System.Guid.Parse(id));

            if (usuario == null) 
            { 
                return NotFound("Usuario não encontrado");
            }

            return Ok(usuario);
        }

        [HttpDelete("usuario/{id}")]
        public async Task<ActionResult> DeletarUSuario(string id)
        {
            var usuarioDeletado = await _loginService.ExcluirUsuarioPorId(System.Guid.Parse(id));

            if(!usuarioDeletado)
            {
                return NotFound("Usuario não encontrado");
            }

            return Ok("Usuario excluido com sucesso");
        }
    }
    
}
