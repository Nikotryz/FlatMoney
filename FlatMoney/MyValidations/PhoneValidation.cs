using InputKit.Shared.Validations;
using System.Text.RegularExpressions;

namespace FlatMoney.MyValidations
{
    public class PhoneValidation : IValidation
    {
        public string Message { get; set; } = "Введите корректный телефон";

        private readonly Regex _phoneRegex = new Regex(@"^(\+)?[\d\s$$\-\(\)]+$");
        public bool Validate(object value)
        {
            if (value is null)
            {
                return true;
            }

            return _phoneRegex.IsMatch(value.ToString()!);
        }
    }
}
