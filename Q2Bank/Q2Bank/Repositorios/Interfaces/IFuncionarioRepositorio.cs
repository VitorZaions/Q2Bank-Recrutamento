using Microsoft.AspNetCore.Mvc;
using Q2Bank.Models;
using System.Threading.Tasks;

namespace Q2Bank.Repositorios.Interfaces
{
    public interface IFuncionarioRepositorio
    {
        Task<List<Funcionario>> GetFuncionarios(int IDEmpresa);
        Task<Funcionario> GetFuncionarioPorId(int IDFuncionario);
        Task<Funcionario> AtualizarFuncionario(Funcionario model);
        Task<Funcionario> Deletar(Funcionario model);
        Task<Funcionario> Novo(Funcionario model);
        Task<bool> VerificarPermissaoEmpresa(int IDEmpresa, int IDUsuario);

    }
}
