using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using Q2Bank.Data;
using Q2Bank.Models;
using Q2Bank.Repositorios;
using Q2Bank.Repositorios.Interfaces;
using Q2Bank.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Q2Bank.Controllers
{
    [ApiController]
    [Route("usuario")]

    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ILogger<UsuarioController> _logger;
        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepositorio usuarioRepositorio)
        {
            _logger = logger;
            _usuarioRepositorio = usuarioRepositorio;
        }

        
        [HttpPost]
        [Route("registro")]
        [Description("Criar Usuario")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Novo([FromBody] Usuario model)
        {
            Console.Write(model);

            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                    return BadRequest("Não é possível informar o ID.");

                if (_usuarioRepositorio.VerificarUsuario(model.User) != null)
                    return BadRequest("Este usuário já existe.");

                var usuario = await _usuarioRepositorio.Novo(model);

                var token = TokenService.GenerateToken(usuario);
                usuario.Senha = "";

                return new
                {
                    user = usuario,
                    token = token
                };
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("login")]
        [Description("Obter usuário")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Login([FromBody] Usuario User)
        {
            if (string.IsNullOrWhiteSpace(User.User))
                return BadRequest("Informe o login.");

            if (string.IsNullOrWhiteSpace(User.Senha))
                return BadRequest("Informe a senha.");

            var user = await _usuarioRepositorio.Login(User);

            if(user == null)
                return NotFound("Login incorreto.");

            var token = TokenService.GenerateToken(user);
            user.Senha = "";

            return new 
            { 
                user = user,
                token = token
            };
                
        }

        [HttpGet]
        [Route("obter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Obter usuário por id")]
        public async Task<ActionResult<Usuario>> Obter()
        {
            Usuario _Usuario =  await _usuarioRepositorio.Obter(Convert.ToInt32(User.Identity.Name));
            return _Usuario == null ? NotFound("Usuário não encontrado") : _Usuario;
        }

        [HttpPut]
        [Route("atualizar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Atualizar Usuário")]
        public async Task<ActionResult<Usuario>> Atualizar([FromBody] Usuario model)
        {
            if (ModelState.IsValid)
            {
                Usuario UserAtt = await _usuarioRepositorio.Obter(Convert.ToInt32(User.Identity.Name));

                if (UserAtt == null)
                    return NotFound("Usuário não encontrado.");

                UserAtt.Nome = model.Nome;
                UserAtt.Senha = model.Senha;
                
                return await _usuarioRepositorio.Atualizar(UserAtt);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
