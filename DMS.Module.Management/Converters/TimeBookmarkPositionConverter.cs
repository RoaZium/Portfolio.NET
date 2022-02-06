using System;
using System.Globalization;
using System.Windows.Data;

namespace DMS.Module.Management.Converters
{
    public class TimeBookmarkPositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double leftPosition = 0;

            try
            {
                long timePosition = ((DateTime)values[0]).Ticks;
                long startTime = ((DateTime)values[1]).Ticks;
                long endTime = ((DateTime)values[2]).Ticks;
                double width = (double)values[3] - 10;

                leftPosition = (timePosition - startTime) / (double)(endTime - startTime) * width;
            }
            catch { }
            return leftPosition;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
