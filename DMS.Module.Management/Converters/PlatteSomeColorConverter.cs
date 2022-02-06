using PrismWPF.Common.Drawing;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DMS.Module.Management.Converters
{
    public class PlatteSomeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ColorPalette_Pear36.GetSomeColor(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
