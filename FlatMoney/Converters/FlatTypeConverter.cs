using System.Globalization;

namespace FlatMoney.Converters
{
    public class FlatTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Своя") return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false) return "Своя";
            return "Арендная";
        }
    }
}
