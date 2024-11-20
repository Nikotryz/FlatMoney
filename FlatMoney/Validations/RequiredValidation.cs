using InputKit.Shared.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
