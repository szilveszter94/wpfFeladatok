using System.Globalization;
using System.Windows.Data;

namespace wpfFeladatok.Converters;

public class BooleanOrNullConverterToLoginStatus : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return "";
        }
        if (value is bool boolValue)
        {
            return boolValue ? "Login successful." : "Login failed!";
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }
            return stringValue == "Login successful.";
        }
        return value;
    }
}