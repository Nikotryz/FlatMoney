using InputKit.Shared.Validations;

namespace FlatMoney.MyValidations
{
    public class NonNegativeValidation : IValidation
    {
        public string Message { get; set; } = "Число не должно быть отрицательным";

        public bool Validate(object value)
        {
            if (value.ToString().Contains('-'))
            {
                return false;
            }
            return true;
        }
    }
}
