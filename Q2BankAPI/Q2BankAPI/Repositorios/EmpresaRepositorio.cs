using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q2Bank.Data;
using Q2Bank.Models;
using Q2Bank.Repositorios.Interfaces;

namespace Q2Bank.Repositorios
{
    public class EmpresaRepositorio : IEmpresaRepositorio
    {
        DataContext _context;
        public EmpresaRepositorio([FromServices] DataContext context)
        {
            _context = context;
        }

        public async Task<Empresa> Atualizar(Empresa model)
        {
            _context.ChangeTracker.Clear();
            _context.Empresa.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Empresa> Deletar(Empresa Empresa)
        {
            _context.Empresa.Remove(Empresa);
            await _context.SaveChangesAsync();
            return Empresa;
        }

        public async Task<List<Empresa>> GetEmpresas(int IDUsuario)
        {
            List<Empresa> Empresas = _context.Empresa.Where(o => o.UsuarioId == IDUsuario).ToList();
            return Empresas;
        }

        public async Task<Empresa> GetObterEmpresa(int IDEmpresa)
        {
            var Empresa = _context.Empresa.Where(o => o.Id == IDEmpresa).FirstOrDefault();
            return Empresa;
        }

        public async Task<Empresa> Novo(Empresa model)
        {
            var Emp = _context.Empresa.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }
    }
}
