using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q2Bank.Data;
using Q2Bank.Models;
using Q2Bank.Repositorios.Interfaces;

namespace Q2Bank.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        DataContext _context;
        public UsuarioRepositorio([FromServices] DataContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Atualizar(Usuario model)
        {
            _context.ChangeTracker.Clear();
            _context.Usuario.Update(model);
            await _context.SaveChangesAsync();

            model.Senha = "";

            return model;
        }

        public async Task<Usuario> Login(Usuario model)
        {
            var user = _context.Usuario.Where(o => o.User == model.User && o.Senha == model.Senha).FirstOrDefault();
            return user;
        }

        public async Task<Usuario> Novo(Usuario model)
        {
            _context.Usuario.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Usuario> Obter(int IDUsuario)
        {
            Usuario _Usuario = _context.Usuario.Where(o => o.Id == IDUsuario).FirstOrDefault();
            return _Usuario;
        }

        public async Task<Usuario> VerificarUsuario(string IDUsuario)
        {
            Usuario _Usuario = _context.Usuario.Where(o => o.User == IDUsuario).FirstOrDefault();
            return _Usuario;
        }


    }
}
