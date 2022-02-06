using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model.RestAPI;
using DMS.Module.Map.Properties;
using Newtonsoft.Json;
using Prism.Commands;
using PrismWPF.Common;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_AlarmHistoryViewModel : CP_CommonBaseViewModel
    {
        public override void Load()
        {
            base.Load();

            SetMqttChannel();
        }

        protected override void OnComponentData()
        {
        }

        protected override void OnSubscribeAsyncMqtt(MqttMsgPublishEventArgs e)
        {
            try
            {
                base.OnSubscribeAsyncMqtt(e);

                DmAlarmStatus data = new DmAlarmStatus();
                data.AlarmLevel = int.Parse(TopicMessage);
                data.AlarmDate = DateTime.Now;

                //1.이상이력만 보여주므로 0(정상)은 제외
                if (data.AlarmLevel == 0)
                {
                    return;
                }

                DmResourceSensorModel resultLinq = LinqManager.FilterFirstResource(TopicCode);

                //2.ResourceSensor Filter : ResourceSensor 명칭은 제공되지 않아 찾아야 한다.(추후 서버에서 작업필요)
                data.ResourceName = resultLinq == null ? string.Empty : resultLinq.ResourceName;

                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    foreach (DmComponentMst item in LinqManager.FilterWhereAlarmHistory())
                    {
                        item.DmAlarmStatusList.Insert(0, data);

                        if (item.DmAlarmStatusList.Count > ConstantManager.MAX_ABNORMALLIST_SIZE)
                        {
                            item.DmAlarmStatusList.Remove(item.DmAlarmStatusList.Last());
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "PieSeries MQTT:" + ex.ToString());
            }
        }

        #region Commands

        public DelegateCommand<MouseButtonEventArgs> AlarmDoubleClickCommand => new DelegateCommand<MouseButtonEventArgs>(AlarmDoubleClickCommandExecute);


        private void AlarmDoubleClickCommandExecute(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!MapFrameViewModel.Instance.IsAutoMonitoringMode)
            {
                object dataContext = (e.Source as FrameworkElement)?.DataContext;

                if (dataContext != null && dataContext is DmAlarmStatus)
                {
                    DmAlarmStatus alarm = dataContext as DmAlarmStatus;

                    DmComponentMst routingCode = LinqManager.FilterFirstRoutingCode(alarm.ResourceCode);

                    if (routingCode == null)
                    {
                        MessageBox.Show(Res.MsgNotExistLayout);
                        return;
                    }

                    DmComponentMst proutingCode = LinqManager.FilterFirstRoutingCode(routingCode.PRoutingCode);

                    if (proutingCode == null)
                    {
                        MessageBox.Show(Res.MsgNotExistLayout);
                        return;
                    }

                    MapFrameViewModel.Instance.SelectedComponentConfig(proutingCode);
                }
            }
        }

        #endregion

        #region Events

        protected override void DisposeUnmanaged()
        {
        }
        #endregion
    }
}
