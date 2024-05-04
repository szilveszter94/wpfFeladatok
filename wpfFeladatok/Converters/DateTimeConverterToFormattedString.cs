using System.Globalization;
using System.Windows.Data;

namespace wpfFeladatok.Converters;


public class DateTimeConverterToFormattedString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
