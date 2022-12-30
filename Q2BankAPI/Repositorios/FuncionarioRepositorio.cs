using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q2Bank.Data;
using Q2Bank.Models;
using Q2Bank.Repositorios.Interfaces;

namespace Q2Bank.Repositorios
{
    public class FuncionarioRepositorio : IFuncionarioRepositorio
    {
        DataContext _context;
        public FuncionarioRepositorio([FromServices] DataContext context)
        {
            _context = context;
        }

        public async Task<Funcionario> AtualizarFuncionario(Funcionario model)
        {
            _context.ChangeTracker.Clear();
            _context.Funcionario.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Funcionario> Deletar(Funcionario model)
        {
            //Tudo certo, pode excluir
            _context.Funcionario.Remove(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Funcionario> GetFuncionarioPorId(int IDFuncionario)
        {
            Funcionario Func = _context.Funcionario.Where(o => o.Id == IDFuncionario).FirstOrDefault();
            return Func;
        }

        public async Task<List<Funcionario>> GetFuncionarios(int IDEmpresa)
        {
            List<Funcionario> Funcionarios = _context.Funcionario.Where(o => o.EmpresaID == IDEmpresa).ToList();
            return Funcionarios;
        }

        public async Task<Funcionario> Novo(Funcionario model)
        {
            _context.Funcionario.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> VerificarPermissaoEmpresa(int IDEmpresa, int IDUsuario)
        {
            var emp = _context.Empresa.Where(o => o.Id == IDEmpresa && o.UsuarioId == IDUsuario).FirstOrDefault();
            return emp == null ? false : true;
        }


    }
}
