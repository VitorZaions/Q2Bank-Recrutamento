using Microsoft.AspNetCore.Mvc;
using Q2Bank.Models;

namespace Q2Bank.Repositorios.Interfaces
{
    public interface IEmpresaRepositorio
    {
        Task<List<Empresa>> GetEmpresas(int IDUsuario); //Listar Empresas
        Task<Empresa> GetObterEmpresa(int IDEmpresa);
        Task<Empresa> Atualizar(Empresa model);
        Task<Empresa> Novo(Empresa model);
        Task<Empresa> Deletar(Empresa Empresa);

    }
}
