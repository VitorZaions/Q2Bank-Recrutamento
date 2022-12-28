using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using Q2Bank.Data;
using Q2Bank.Models;
using Q2Bank.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Q2Bank.Controllers
{
    [ApiController]
    [Route("usuario")]

    public class UsuarioController : Controller
    {

        private readonly ILogger<UsuarioController> _logger;
        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        
        [HttpPost]
        [Route("registro")]
        [Description("Criar Usuario")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Post([FromServices] DataContext context, [FromBody] Usuario model)
        {
            Console.Write(model);

            if (ModelState.IsValid)
            {
                if(context.Usuario.Where(o => o.User == model.User).FirstOrDefault() != null)
                {
                    return BadRequest("Este usuário já existe.");
                }

                context.Usuario.Add(model);
                await context.SaveChangesAsync();

                var token = TokenService.GenerateToken(model);
                model.Senha = "";

                return new
                {
                    user = model,
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
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] Usuario User)
        {
            if (string.IsNullOrWhiteSpace(User.User))
                return BadRequest("Informe o login.");

            if (string.IsNullOrWhiteSpace(User.Senha))
                return BadRequest("Informe a senha.");

            var user = context.Usuario.Where(o => o.User == User.User && o.Senha == User.Senha).FirstOrDefault();

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
        public async Task<ActionResult<Usuario>> Get([FromServices] DataContext context)
        {
            Usuario _Usuario = context.Usuario.Where(o => o.Id == Convert.ToDecimal(User.Identity.Name)).FirstOrDefault();

            if (_Usuario == null)
            {
                return NotFound("Usuário não encontrado");
            }

            return _Usuario;
        }

        [HttpPut]
        [Route("atualizar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Atualizar Usuário")]
        public async Task<ActionResult<Usuario>> Put([FromServices] DataContext context, [FromBody] Usuario model)
        {
            if (ModelState.IsValid)
            {
                Usuario UserAtt = context.Usuario.Where(o => o.Id == Convert.ToDecimal(User.Identity.Name)).FirstOrDefault();

                if (UserAtt == null)
                    return NotFound("Usuário não encontrado.");

                UserAtt.Nome = model.Nome;
                UserAtt.Senha = model.Senha;

                context.ChangeTracker.Clear();
                context.Usuario.Update(UserAtt);
                await context.SaveChangesAsync();
                UserAtt.Senha = "";
                return UserAtt;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
