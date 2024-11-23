using InputKit.Shared.Validations;

namespace FlatMoney.Validations
{
    public class RequiredValidation : IValidation
    {
        public string Message { get; set; } = "Поле обязательно для ввода";

        public bool Validate(object value)
        {
            if (value != null && (string)value != string.Empty) return true;
            return false;
        }
    }
}
