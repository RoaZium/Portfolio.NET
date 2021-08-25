using System;
using System.Globalization;
using System.Windows.Data;

namespace PrismWPF.Common.Converter
{
    public class SingleRangeConverter : IValueConverter
    {
        public double StartValue { get; set; }

        public double EndValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;

            return ((EndValue - StartValue) * val) + StartValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
