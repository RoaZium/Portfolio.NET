using System;
using System.Globalization;
using System.Windows.Data;

namespace PrismWPF.Common.Converter
{
    public class AlarmStateStrConverter : IValueConverter
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

                if (result == false)
                {
                    state = AlarmState.Unknown;
                }
            }

            return GetAlarmStateStr(state);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static string GetAlarmStateStr(AlarmState state)
        {
            switch (state)
            {
                case AlarmState.Normal:
                    {
                        return "정 상";
                    }
                case AlarmState.Abnormal:
                    {
                        return "이 상";
                    }
                case AlarmState.Danger:
                    {
                        return "위 험";
                    }
                case AlarmState.Stop:
                    {
                        return "정 지";
                    }
                case AlarmState.None:
                    {
                        return "";
                    }
                case AlarmState.Unknown:
                default:
                    {
                        return "Unknown";
                    }
            }
        }
    }
}
