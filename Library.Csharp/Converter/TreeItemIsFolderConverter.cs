using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrismWPF.Common.Converter
{
    public class TreeItemIsFolderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType().Name == "TreeFolderInfo")
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
