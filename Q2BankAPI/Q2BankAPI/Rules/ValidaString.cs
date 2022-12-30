using System.ComponentModel.DataAnnotations;

namespace Q2Bank.Rules
{
    public class ValidaStringPadrao : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                string Valor = (string)value;

                if(Valor == null)
                {
                    return true;
                }

                bool result = Valor.Contains("?") || Valor.Contains("&");
                return !result;
            }
            catch
            {
                return false;
            }
        }
    }
}
