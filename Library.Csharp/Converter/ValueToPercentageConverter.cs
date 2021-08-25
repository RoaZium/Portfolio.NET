using System;
using System.Globalization;
using System.Windows.Data;

namespace PrismWPF.Common.Converter
{
    public class ValueToPercentageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //Lable 데이터
                if (values.Length == 2)
                {
                    if(values[0] == null || values[1] == null)
                    {
                        return null;
                    }

                    double value1 = 0;
                    double value2 = 0;

                    double.TryParse(values[0].ToString(), out value1);
                    double.TryParse(values[1].ToString(), out value2);

                    var data3 = (value1 / value2) * 100;
                    return data3;
                }
                else if (values.Length == 3) // 게이지 컴포넌트
                {
                    if (values == null || values[0] == null || values[1] == null || values[2] == null)
                    {
                        return new object();
                    }

                    double TotalRangeValue = 0;
                    double CurrentValue = 0;
                    double TotalValue = 0;

                    bool valid1 = double.TryParse(values[0].ToString(), out TotalRangeValue);
                    bool valid2 = double.TryParse(values[1].ToString(), out CurrentValue);
                    bool valid3 = double.TryParse(values[2].ToString(), out TotalValue);

                    if (valid1 == false || valid2 == false || valid3 == false)
                    {
                        return new object();
                    }

                    double percentValue = (CurrentValue / TotalValue) * 100;

                    double totalProgress = (TotalRangeValue * percentValue) / 100;

                    if (totalProgress > TotalRangeValue)
                    {
                        totalProgress = TotalRangeValue;
                    }

                    return totalProgress;
                }
            }
            catch (Exception e)
            {

            }

            return new object();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
