using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Q2Bank.Data;
using Q2Bank.Models;
using System.ComponentModel;

namespace Q2Bank.Controllers
{
    [ApiController]
    [Route("empresa")]

    public class EmpresaController : Controller
    {
        private readonly ILogger<EmpresaController> _logger;

        public EmpresaController(ILogger<EmpresaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Obter empresas por usuário")]
        public async Task<ActionResult<List<Empresa>>> Get([FromServices] DataContext context)
        {
            List<Empresa> Empresas = context.Empresa.Where(o => o.UsuarioId == Convert.ToInt32(User.Identity.Name)).ToList();

            if (Empresas.Count <= 0)
            {
                return NotFound("Nenhuma empresa foi encontrada.");
            }
            else
            {
                return Empresas;
            }
        }

        [HttpGet]
        [Route("obter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Obter empresa por id")]
        public async Task<ActionResult<Empresa>> Get([FromServices] DataContext context, int IDEmpresa)
        {
            Empresa Empresa = context.Empresa.Where(o => o.Id == IDEmpresa).FirstOrDefault();


            if (Empresa == null)
            {
                return NotFound("Empresa não encontrada..");
            }
            
            if (Empresa.UsuarioId != Convert.ToInt32(User.Identity.Name))
            {
                return Unauthorized("Você não está autorizada a obter dados desta empresa.");
            }                        
            return Empresa;            
        }


        [HttpPut]
        [Route("atualizar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Atualizar Empresa")]
        public async Task<ActionResult<Empresa>> Put([FromServices] DataContext context, [FromBody] Empresa model)
        {
            if (ModelState.IsValid)
            { 
                Empresa Empresa = context.Empresa.Where(o => o.Id == model.Id).FirstOrDefault();

                if (Empresa == null)
                    return NotFound("Empresa não encontrada.");

                if (Empresa.UsuarioId != Convert.ToInt32(User.Identity.Name))
                {
                    return Unauthorized("Você não tem autorização para editar esta empresa.");
                }

                context.ChangeTracker.Clear();

                context.Empresa.Update(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("novo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Criar Empresa")]
        public async Task<ActionResult<Empresa>> Post([FromServices] DataContext context, [FromBody] Empresa model)
        {
            if (ModelState.IsValid)
            {
                //Valida o criador

                if (model.UsuarioId != Convert.ToInt32(User.Identity.Name))
                {
                    return Unauthorized("Você não tem autorização para criar a empresa nesta conta.");
                }

                //model.Usuario = context.Usuario.Where(o => o.Id == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();

                context.Empresa.Add(model);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch(Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                }
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("deletar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Deletar Empresa")]
        public async Task<ActionResult<Empresa>> Delete([FromServices] DataContext context, [FromBody] int IDEmpresa)
        {
            if (ModelState.IsValid)
            {
                Empresa Empresa = context.Empresa.Where(o => o.Id == IDEmpresa).FirstOrDefault();

                if (Empresa == null)
                    return NotFound("Empresa não encontrada.");

                if (Empresa.UsuarioId != Convert.ToInt32(User.Identity.Name))
                {
                    return Unauthorized("Você não tem autorização para deletar esta empresa.");
                }

                var update = context.Empresa.Remove(Empresa);
                var result = await context.SaveChangesAsync();

                return Empresa;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /* public IActionResult Index()
         {
             return View();
         }*/
    }
}
