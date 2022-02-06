using System;
using System.Windows.Data;

namespace PrismWPF.Common.Converter
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class SingleInverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value.GetType() != typeof(bool))
            {
                throw new InvalidOperationException("The value must be a boolean");
            }

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
