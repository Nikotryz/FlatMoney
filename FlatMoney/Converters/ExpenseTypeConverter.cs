using FlatMoney.Models;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FlatMoney.Converters 
{
    public class ExpenseTypeConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            List<string> result = [];
            foreach (var item in (ObservableCollection<ExpenseTypeModel>)value!)
            {
                result.Add(item.Name);
            }
            return result;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            ObservableCollection<ExpenseTypeModel> result = [];
            return result;
        }
    }
}
