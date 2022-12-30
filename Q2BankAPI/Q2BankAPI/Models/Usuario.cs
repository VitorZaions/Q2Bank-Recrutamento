using Microsoft.EntityFrameworkCore;
using Q2Bank.Rules;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace Q2Bank.Models
{
    [Index(nameof(User), IsUnique = true)]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]      
        [StringLength(50,ErrorMessage = "O usuário deve conter no máximo 50 caracteres.")]             
        public string User { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "A senha deve conter no máximo 50 caracteres.")]
        public string Senha { get; set; }

        [Required]
        [ValidaStringPadrao(ErrorMessage = "Não use os caracteres '&' e '?' no nome.")]
        [StringLength(100, ErrorMessage = "O nome deve conter no máximo 100 caracteres.")]
        public string Nome { get; set; }

    }
}
