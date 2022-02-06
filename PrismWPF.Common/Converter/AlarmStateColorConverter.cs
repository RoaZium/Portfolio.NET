using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PrismWPF.Common.Converter
{
    public class AlarmStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AlarmState state = AlarmState.Unknown;
            if (value is AlarmState || value is int)
            {
                state = (AlarmState)value;
            }
            else if (value != null)
            {
                bool result = Enum.TryParse(value.ToString(), out state);

                if(result == false)
                {
                    state = AlarmState.Unknown;
                }
            }

            return GetAlarmStateColor(state);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static object GetAlarmStateColor(AlarmState state)
        {
            switch (state)
            {
                case AlarmState.Normal:
                    {
                        return ColorTranslator.ToHtml(System.Drawing.Color.LimeGreen);
                    }
                case AlarmState.Abnormal:
                    {
                        return ColorTranslator.ToHtml(System.Drawing.Color.Yellow);
                    }
                case AlarmState.Danger:
                    {
                        return ColorTranslator.ToHtml(System.Drawing.Color.Red);
                    }
                case AlarmState.Stop:
                    {
                        return ColorTranslator.ToHtml(System.Drawing.Color.Gray);
                    }
                case AlarmState.None:
                    {
                        return ColorTranslator.ToHtml(System.Drawing.Color.Transparent);
                    }
                case AlarmState.Unknown:
                default:
                    {
                        return ColorTranslator.ToHtml(System.Drawing.Color.Transparent);
                    }
            }
        }
    }

    /// <summary>
    ///  알람상태
    /// </summary>
    public enum AlarmState
    {
        Normal = 0,   // 정상
        Abnormal = 1, // 비정상
        Danger = 2,   // 위험
        Stop = 900,   // 멈춤
        None = -1,     // 없음
        Unknown = 999   // 알수 없음
    }
}
