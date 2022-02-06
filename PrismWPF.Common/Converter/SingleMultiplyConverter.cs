using System;
using System.Globalization;
using System.Windows.Data;

namespace PrismWPF.Common.Converter
{
    public class SingleMultiplyConverter : IValueConverter
    {
        public double ValueToMultiply { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = 0;

            if (value != null)
            {
                double.TryParse(value.ToString(), out val);
            }

            return val * ValueToMultiply;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
