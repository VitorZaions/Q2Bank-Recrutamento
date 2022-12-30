using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Q2Bank.Rules;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Q2Bank.Models
{
    public class Empresa
    {
        //Key = Auto Incremento
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        [StringLength(200,MinimumLength = 1,ErrorMessage = "O campo nome deve conter no máximo 200 caracteres.")]
        [ValidaStringPadrao(ErrorMessage = "Não use os caracteres '&' e '?' no nome.")]
        public string Nome { get; set; }

        //Endereço
        [Required(ErrorMessage = "O campo UF é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo UF deve conter dois caracteres.")]
        public string UF { get; set; }

        [Required(ErrorMessage = "O campo CEP é obrigatório.")]
        [StringLength(8, MinimumLength = 8,ErrorMessage = "O CEP deve conter 8 dígitos.")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "O campo logradouro é obrigatório.")]
        [StringLength(60, ErrorMessage = "O logradouro deve conter no máximo 60 caracteres.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O campo localidade é obrigatório.")]
        [StringLength(60, ErrorMessage = "A localidade deve conter no máximo 60 caracteres.")]
        public string Localidade { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [StringLength(60, ErrorMessage = "O bairro deve conter no máximo 60 caracteres.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O campo número é obrigatório.")]
        [StringLength(6, ErrorMessage = "O número deve conter no máximo 6 caracteres.")]
        public string Numero { get; set; }

        [StringLength(50, ErrorMessage = "O complemento deve conter no máximo 50 caracteres.")]
        public string Complemento { get; set; }

        // Contato
        [Required(ErrorMessage = "O campo telefone é obrigatório.")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "O telefone deve conter entre 10 e 11 dígitos, incluindo o DDD.")]
        public string Telefone { get; set; }


        [Required(ErrorMessage = "O campo usuário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Usuário inválido.")]
        public int UsuarioId { get; set; }

        [ValidateNever]
        public Usuario Usuario { get; set; }


    }
}
