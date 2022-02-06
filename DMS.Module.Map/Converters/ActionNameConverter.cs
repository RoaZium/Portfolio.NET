using Coever.Lib.CoPlatform.Scenario.Core;
using DMS.Module.Map.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DMS.Module.Map.Converters
{
    public class ActionNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is IActionModel)
                {
                    var iJob = value as IActionModel;
                    switch (iJob.ActionType)
                    {
                        case ActionTypes.alarm_layout:
                        {
                            return Res.StrActionNameAlarmLayout;
                        }
                        case ActionTypes.ipcam_compo:
                        {
                            return Res.StrActionNameIPCamComponent;
                        }
                        case ActionTypes.layout:
                        {
                            return Res.StrActionNameLayout;
                        }
                        default:
                        {
                            return Res.StrInvalidActon;
                        }
                    }
                }
            }
            catch { }

            return Res.StrInvalidActon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
