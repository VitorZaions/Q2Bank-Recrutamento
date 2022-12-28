using Microsoft.AspNetCore.Mvc;
using Q2Bank.Models;

namespace Q2Bank.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario> Novo(Usuario model);

        Task<Usuario> Login(Usuario model);

        Task<Usuario> Obter(int IDUsuario);

        Task<Usuario> Atualizar(Usuario model);

        Task<Usuario> VerificarUsuario(string IDUsuario);


    }
}
