using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Q2Bank.Data;
using Q2Bank.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Q2Bank.Controllers
{
    [ApiController]
    [Route("funcionario")]

    public class FuncionarioController : Controller
    {

        private readonly ILogger<FuncionarioController> _logger;
        public FuncionarioController(ILogger<FuncionarioController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [Route("listar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Obter funcionario por empresa")]
        public async Task<ActionResult<List<Funcionario>>> GetFuncionarios([FromServices] DataContext context, int IDEmpresa)
        {
            //Verifica se tem permissão na empresa
            Empresa Emp = context.Empresa.Where(o => o.Id == IDEmpresa && o.UsuarioId == Convert.ToDecimal(User.Identity.Name)).FirstOrDefault();

            if (Emp == null)
                return Unauthorized("Sem permissão para obter os funcionários da empresa.");

            List<Funcionario> Funcionarios = context.Funcionario.Where(o => o.EmpresaID == IDEmpresa).ToList();
            
            if (Funcionarios.Count <= 0)
            {
                return NotFound("Nenhum funcionário foi encontrado para a empresa informada.");
            }

            return Funcionarios;

        }

        [HttpGet]
        [Route("obter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Obter funcionário por id")]
        public async Task<ActionResult<Funcionario>> Get([FromServices] DataContext context, int IDFuncionario)
        {
            Funcionario Func = context.Funcionario.Where(o => o.Id == IDFuncionario).FirstOrDefault();
            Empresa Emp = context.Empresa.Where(o => o.Id == Func.EmpresaID && o.UsuarioId == Convert.ToDecimal(User.Identity.Name)).FirstOrDefault();

            if (Func == null)
            {
                return NotFound("Funcionário não encontrado");
            }

            if (Emp == null)
            {
                return Unauthorized("Sem autorização para editar este funcionário");
            }

            return Func;
        }


        [HttpPut]
        [Route("atualizar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Atualizar Funcionário")]
        public async Task<ActionResult<Funcionario>> Put([FromServices] DataContext context, [FromBody] Funcionario model)
        {
            if (ModelState.IsValid)
            {
                Funcionario Funcion = context.Funcionario.Where(o => o.Id == model.Id).FirstOrDefault();

                if (Funcion == null)
                    return NotFound("Funcionário não encontrado.");

                //Verifica se tem permissão na empresa
                Empresa Emp = context.Empresa.Where(o => o.Id == model.EmpresaID && o.UsuarioId == Convert.ToDecimal(User.Identity.Name)).FirstOrDefault();

                if (Emp == null)
                    return Unauthorized("Sem permissão para criar o funcionário.");

                context.ChangeTracker.Clear();
                context.Funcionario.Update(model);
                await context.SaveChangesAsync();
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
        [Description("Deletar Funcionário")]
        public async Task<ActionResult<Funcionario>> Delete([FromServices] DataContext context, [FromBody] int IDFuncionario)
        {
            if (ModelState.IsValid)
            {

                Funcionario Funcion = context.Funcionario.Where(o => o.Id == IDFuncionario).FirstOrDefault();
                if (Funcion == null)
                    return NotFound("Funcionário não encontrado.");

                //Verifica se tem permissão na empresa
                Empresa Emp = context.Empresa.Where(o => o.Id == Funcion.EmpresaID && o.UsuarioId == Convert.ToDecimal(User.Identity.Name)).FirstOrDefault();

                if (Emp == null)
                    return Unauthorized("Sem permissão para remover o funcionário.");
                
                //Tudo certo, pode excluir
                context.Funcionario.Remove(Funcion);
                await context.SaveChangesAsync();

                return Funcion;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("novo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Criar Funcionário")]
        public async Task<ActionResult<Funcionario>> Post([FromServices] DataContext context, [FromBody] Funcionario model)
        {
            if (ModelState.IsValid)
            {
                //Verifica se tem permissão na empresa
                Empresa Emp = context.Empresa.Where(o => o.Id == model.EmpresaID && o.UsuarioId == Convert.ToDecimal(User.Identity.Name)).FirstOrDefault();

                if (Emp == null)
                    return Unauthorized("Sem permissão para criar o funcionário.");

                context.Funcionario.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
