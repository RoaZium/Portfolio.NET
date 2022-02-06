/* 작성자: HJW
 * 작성일자: 2018-05-09
 * 작성내용: bool -> Visibility(Collapsed, Visible)으로 변환
 */

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrismWPF.Common.Converter
{
    public class BooleanToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.Parse(parameter.ToString()) == 1)
            {
                if (true.Equals(value) == false)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            else
            {
                if (true.Equals(value) == false)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
