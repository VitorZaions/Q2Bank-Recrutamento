using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Q2Bank.Rules;
using System.ComponentModel.DataAnnotations;

namespace Q2Bank.Models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }

        [ValidaStringPadrao(ErrorMessage = "Não use os caracteres '&' e '?' no nome.")]
        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome deve conter no máximo 200 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo salário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O salário deve ser maior que zero.")]
        public decimal Salario { get; set; }

        [ValidaCargo(ErrorMessage = "O cargo informado não existe.")]
        [Required(ErrorMessage = "O campo cargo é obrigatório.")]
        [StringLength(200, ErrorMessage = "O cargo deve conter no máximo 200 caracteres.")]
        public string Cargo { get; set; }

        [Required(ErrorMessage = "O campo empresa é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Empresa inválida.")]
        public int EmpresaID { get; set; }

        [ValidateNever]
        public Empresa Empresa { get; set; }

    }
}
