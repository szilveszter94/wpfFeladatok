using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace wpfFeladatok.Converters;

public class RgbToColorConverter : IMultiValueConverter
{
    public object Convert(object[]? values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length < 3 || values.Length > 4)
            return Brushes.Red;
        try
        {
            byte red = System.Convert.ToByte(values[0]);
            byte green = System.Convert.ToByte(values[1]);
            byte blue = System.Convert.ToByte(values[2]);
            
            if (red == 0 && green == 0 && blue == 0)
            {
                if (values[3] is Brush defaultBrush)
                {
                    return defaultBrush;
                }
                else
                {
                    return Brushes.Red;
                }
            }
            
            return new SolidColorBrush(Color.FromRgb(red, green, blue));
        }
        catch (Exception)
        {
            return Brushes.Red;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
