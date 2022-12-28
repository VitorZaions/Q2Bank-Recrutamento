using System.ComponentModel.DataAnnotations;

namespace Q2Bank.Rules
{
    public class ValidaCargo : ValidationAttribute
    {
        string[] cargos = { "Programador", "Designer", "Administração", "Arquitetura", "Jornalismo", "Psicologia" };

        public override bool IsValid(object value)
        {
            try
            {  
                string Cargo = (string)value;
                return cargos.Contains(Cargo);
            }
            catch
            {
                return false;
            }
        }
    }
}
