using Microsoft.EntityFrameworkCore;
using Q2Bank.Models;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Q2Bank.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {          
        }

        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<Funcionario> Funcionario { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
