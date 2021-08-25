using System;
using System.Globalization;
using System.Windows.Data;

namespace PrismWPF.Common.Converter
{
    public class SingleIsEqureConverter : IValueConverter
    {
        public object CheckValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && CheckValue != null)
            {
                if (value.GetType() == CheckValue.GetType())
                {
                    return value.Equals(CheckValue);
                }
                else
                {
                    return value.ToString() == CheckValue.ToString();
                }
            }
            return value == CheckValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
