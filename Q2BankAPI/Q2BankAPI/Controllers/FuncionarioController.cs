using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Q2Bank.Data;
using Q2Bank.Models;
using Q2Bank.Repositorios;
using Q2Bank.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Q2Bank.Controllers
{
    [ApiController]
    [Route("funcionario")]

    public class FuncionarioController : Controller
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly ILogger<FuncionarioController> _logger;


        public FuncionarioController(ILogger<FuncionarioController> logger, IFuncionarioRepositorio funcionarioRepositorio)
        {
            _logger = logger;
            _funcionarioRepositorio = funcionarioRepositorio;
        }


        [HttpGet]
        [Route("listar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Obter funcionario por empresa")]
        public async Task<ActionResult<List<Funcionario>>> GetFuncionarios(int IDEmpresa)
        {
            //Verifica se tem permissão na empresa
            bool PermEmpresa = await _funcionarioRepositorio.VerificarPermissaoEmpresa(IDEmpresa, Convert.ToInt32(User.Identity.Name));

            if (PermEmpresa == false)
                return Unauthorized("Sem permissão para obter os funcionários da empresa.");

            var Funcionarios = await _funcionarioRepositorio.GetFuncionarios(IDEmpresa);
            
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
        public async Task<ActionResult<Funcionario>> GetFuncionarioPorId(int IDFuncionario)
        {
            Funcionario Func = await _funcionarioRepositorio.GetFuncionarioPorId(IDFuncionario);
          
            bool PemEmpresa = await _funcionarioRepositorio.VerificarPermissaoEmpresa(Func.EmpresaID, Convert.ToInt32(User.Identity.Name));

            if (Func == null)
                return NotFound("Funcionário não encontrado");

            if (PemEmpresa == false)
                return Unauthorized("Sem autorização para editar este funcionário");

            return Func;
        }


        [HttpPut]
        [Route("atualizar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Description("Atualizar Funcionário")]
        public async Task<ActionResult<Funcionario>> AtualizarFuncionario([FromServices] DataContext _context, [FromBody] Funcionario model)
        {
            if (ModelState.IsValid)
            {
                Funcionario Funcion = _context.Funcionario.Where(o => o.Id == model.Id).FirstOrDefault();

                if (Funcion == null)
                    return NotFound("Funcionário não encontrado.");

                //Verifica se tem permissão na empresa

                bool PermEmpresa = await _funcionarioRepositorio.VerificarPermissaoEmpresa(model.EmpresaID, Convert.ToInt32(User.Identity.Name));

                if (PermEmpresa == false)
                    return Unauthorized("Sem permissão para criar o funcionário.");

                return await _funcionarioRepositorio.AtualizarFuncionario(model);

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
        public async Task<ActionResult<Funcionario>> Deletar([FromBody] int IDFuncionario)
        {
            if (ModelState.IsValid)
            {

                Funcionario Funcion = await _funcionarioRepositorio.GetFuncionarioPorId(IDFuncionario);
                if (Funcion == null)
                    return NotFound("Funcionário não encontrado.");

                //Verifica se tem permissão na empresa
                bool PermEmpresa = await _funcionarioRepositorio.VerificarPermissaoEmpresa(Funcion.EmpresaID, Convert.ToInt32(User.Identity.Name));

                if (PermEmpresa == false)
                    return Unauthorized("Sem permissão para remover o funcionário.");

                return await _funcionarioRepositorio.Deletar(Funcion);
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
        public async Task<ActionResult<Funcionario>> Novo([FromServices] DataContext _context, [FromBody] Funcionario model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                    return BadRequest("Não é possível informar o ID.");

                //Verifica se tem permissão na empresa

                bool PermEmpresa = await _funcionarioRepositorio.VerificarPermissaoEmpresa(model.EmpresaID, Convert.ToInt32(User.Identity.Name));

                if (PermEmpresa == false)
                    return Unauthorized("Sem permissão para criar o funcionário.");

                return await _funcionarioRepositorio.Novo(model);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
