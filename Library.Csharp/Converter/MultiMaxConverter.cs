using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace PrismWPF.Common.Converter
{
    public class MultiMaxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Max(element => (double)element);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
