using System.Globalization;
using System.Windows.Data;

namespace wpfFeladatok.Converters;


public class BooleanConverterToOpenClosedText : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Close popup" : "Open popup";
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            return stringValue == "Toggle is Open";
        }
        return value;
    }
}
