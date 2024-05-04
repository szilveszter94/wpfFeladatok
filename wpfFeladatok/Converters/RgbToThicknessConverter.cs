using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace wpfFeladatok.Converters;

public class RgbToThicknessConverter : IMultiValueConverter
{
    public object Convert(object[]? values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length < 3 || values.Length > 4)
            return new Thickness(1);
        try
        {
            byte red = System.Convert.ToByte(values[0]);
            byte green = System.Convert.ToByte(values[1]);
            byte blue = System.Convert.ToByte(values[2]);
            
            if (red > 0 || green > 0 || blue > 0)
            {
                return new Thickness(3);
            }


            return new Thickness(1);
        }
        catch (Exception)
        {
            return new Thickness(1);
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}