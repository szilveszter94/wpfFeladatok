using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace wpfFeladatok.Converters;

public class SingleColorValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null || !(value is int) || !(parameter is string))
            return Binding.DoNothing;

        byte colorValue = System.Convert.ToByte(value);
        string color = (string)parameter;

        switch (color.ToLower())
        {
            case "red":
                return new SolidColorBrush(Color.FromRgb(colorValue, 0, 0));
            case "green":
                return new SolidColorBrush(Color.FromRgb(0, colorValue, 0));
            case "blue":
                return new SolidColorBrush(Color.FromRgb(0, 0, colorValue));
            default:
                return Binding.DoNothing;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
    
}