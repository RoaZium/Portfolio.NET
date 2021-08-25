using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace PrismWPF.Common.Converter
{
    public class MultiMinConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = double.TryParse(values.Min().ToString(), out double minValue);

            if (!result)
            {
                return null;
            }

            double data = values.Min(element => (double)element);
            return values.Min(element => (double)element);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
