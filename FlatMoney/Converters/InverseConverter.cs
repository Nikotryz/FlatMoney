using System.Globalization;

namespace FlatMoney.Converters
{
    public class InverseConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int)
            {
                if ((int)value == 0) return true;
                if ((int)value == 1) return false;
            }

            if (value is bool)
            {
                return !(bool)value!;
            }

            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int)
            {
                if ((int)value == 0) return false;
                if ((int)value == 1) return true;
            }

            if (value is bool)
            {
                return (bool)value!;
            }

            return true;
        }
    }
}
