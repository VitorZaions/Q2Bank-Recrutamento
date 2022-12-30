using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Q2Bank.Data;
using Q2Bank.Models;
using Q2Bank.Repositorios;
using Q2Bank.Repositorios.Interfaces;
using System.ComponentModel;

namespace Q2Bank.Controllers
{
    [ApiController]
    [Route("empresa")]

    public class EmpresaController : Controller
    {

        //private DataContext _context;
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly ILogger<FuncionarioController> _logger;
        public EmpresaController(ILogger<FuncionarioController> logger,IEmpresaRepositorio empresaRepositorio)
        {
            _logger = logger;
            _empresaRepositorio = empresaRepositorio;
        }


        
        [HttpGet]
        [Route("listar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Obter empresas por usuário")]
        public async Task<ActionResult<List<Empresa>>> GetEmpresas()
        {
            var Empresas = await _empresaRepositorio.GetEmpresas(Convert.ToInt32(User.Identity.Name));
            return (Empresas.Count <= 0) ? NotFound("Nenhuma empresa foi encontrada.") : Empresas;            
        }

        [HttpGet]
        [Route("obter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Obter empresa por id")]
        public async Task<ActionResult<Empresa>> GetObterEmpresa(int IDEmpresa)
        {
            Empresa Empresa = await _empresaRepositorio.GetObterEmpresa(IDEmpresa);

            if (Empresa == null)
                return NotFound("Empresa não encontrada..");    
            
            if (Empresa.UsuarioId != Convert.ToInt32(User.Identity.Name))
                return Unauthorized("Você não está autorizada a obter dados desta empresa.");
                      
            return Empresa;            
        }


        [HttpPut]
        [Route("atualizar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Atualizar Empresa")]
        public async Task<ActionResult<Empresa>> Atualizar([FromBody] Empresa model)
        {
            if (ModelState.IsValid)
            { 
                Empresa Empresa = await _empresaRepositorio.GetObterEmpresa(model.Id);

                if (Empresa == null)
                    return NotFound("Empresa não encontrada.");

                if (Empresa.UsuarioId != Convert.ToInt32(User.Identity.Name))
                    return Unauthorized("Você não tem autorização para editar esta empresa.");

                return await _empresaRepositorio.Atualizar(model);
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
        public async Task<ActionResult<Empresa>> Novo([FromBody] Empresa model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                    return BadRequest("Não é possível informar o ID.");

                if (model.UsuarioId != Convert.ToInt32(User.Identity.Name))
                    return Unauthorized("Você não tem autorização para criar a empresa nesta conta.");

                return await _empresaRepositorio.Novo(model);
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
        public async Task<ActionResult<Empresa>> Deletar([FromServices] DataContext context, [FromBody] int IDEmpresa)
        {
            if (ModelState.IsValid)
            {
                Empresa Empresa = await _empresaRepositorio.GetObterEmpresa(IDEmpresa);

                if (Empresa == null)
                    return NotFound("Empresa não encontrada.");

                if (Empresa.UsuarioId != Convert.ToInt32(User.Identity.Name))
                    return Unauthorized("Você não tem autorização para deletar esta empresa.");

                return await _empresaRepositorio.Deletar(Empresa);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
