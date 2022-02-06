using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.Mqtt.Core;
using DMS.Module.Map.Model.RestAPI;
using DMS.Module.Map.Network;
using DMS.Module.Map.View.Controls;
using DMS.Module.Map.ViewModel;
using DMS.Module.Map.ViewModel.Component;
using DMS.Module.Map.ViewModel.Control;
using Newtonsoft.Json;
using PrismWPF.Common;
using PrismWPF.Common.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using uPLibrary.Networking.M2Mqtt.Messages;
using EnvSettings = DMS.Module.EnvironmentSetting.Properties;

namespace DMS.Module.Map.Infrastructure
{
    public class MqttReceiveManager
    {
        public MqttReceiveManager()
        {
            SetMqttSubscribeInfo();

            MqttService.Instance.Client.MqttMsgPublishReceived += MqttMsgPublishReceived;
        }

        private CoeverMqttClient MqttClient;

        private List<string> mqttCodeList;

        private string[] topicList;

        public bool IsMqttMessage = false;

        public AbnormalAlarmPopupView alarmPopupV;

        public void SetMqttSubscribeInfo()
        {
            try
            {
                if (MqttClient != null)
                {
                    MqttClient = null;
                }

                string serverHost = EnvSettings.Settings.Default.ServerUrl?.Host;

                if (string.IsNullOrEmpty(serverHost))
                {
                    return;
                }

                int mqttPort = EnvSettings.Settings.Default.MqttPort;
                mqttPort = mqttPort == 0 ? 1883 : mqttPort;

                MqttClient = new CoeverMqttClient(serverHost, mqttPort, false, null, null, 0);
                MqttClient.MqttMsgPublishReceived += MqttMsgPublishReceived;

                //if (topicList != null)
                //{
                //    MqttClient?.Unsubscribe(topicList);
                //}

                RegisterMQTTtopic();

                MqttClient?.ConnectRandomId(this.GetType().Name);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        public void RegisterMQTTtopic()
        {
            try
            {
                if (topicList != null && topicList.Count() != 0)
                {
                    MqttClient?.Unsubscribe(topicList);
                }

                mqttCodeList = new List<string>();

                foreach (var item in LinqManager.FilterWhereAlarmCode())
                {
                    string[] data = item.PropertyM.AlarmCode.Split(',');

                    foreach (var item2 in data)
                    {
                        if (string.IsNullOrEmpty(item2))
                        {
                            continue;
                        }

                        mqttCodeList.Add($"/event/c/{nameof(MQTTTopic.alarm_state)}/" + item2);
                    }
                }

                topicList = mqttCodeList.ToArray();

                if (topicList != null && topicList.Count() != 0)
                {
                    var qos = new byte[topicList.Length];
                    MqttClient?.Subscribe(topicList, qos);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void UnRegisterMQTTtopic(string code)
        {
            foreach (var item in LinqManager.FilterWhereAlarmCode())
            {
                string[] data = item.PropertyM.AlarmCode.Split(',');

                foreach (var item2 in data)
                {
                    bool IsExist = mqttCodeList.Exists(p => p == ($"/event/c/{nameof(MQTTTopic.alarm_state)}/" + code));

                    if (!IsExist)
                    {
                        mqttCodeList.Remove(($"/event/c/{nameof(MQTTTopic.alarm_state)}/" + code));
                    }
                }
            }
        }

        private void SubscribeAsyncMqtt(MqttMsgPublishEventArgs e)
        {
            try
            {
                //MqttMsgPublishEventArgs localData;
                //bool cqResult = cq.TryDequeue(out localData);

                //if (cqResult == false)
                //{
                //    return;
                //}

                string[] topicParts = e.Topic.Split('/');
                string eventType = topicParts[2];
                string targetType = topicParts[3];
                string targetId = topicParts[4];

                string message = Encoding.UTF8.GetString(e.Message);

                switch (targetType)
                {
                    case nameof(MQTTTopic.sensor):
                        {
                            var data = JsonConvert.DeserializeObject<DmResourceSensorModel>(message);

                            SetResourceSensor(data);
                            break;
                        }
                    case nameof(MQTTTopic.alarm_state):
                        {
                            if (message != "2")
                            {
                                IsMqttMessage = false;
                                return;
                            }

                            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                alarmPopupV = new AbnormalAlarmPopupView();
                                var data = alarmPopupV.DataContext as AbnormalAlarmPopupViewModel;

                                DmResourceSensorModel resultLinq = LinqManager.FilterFirstResource(targetId);
                                data.Title = resultLinq == null ? string.Empty : resultLinq.ResourceName;
                                data.Code = targetId;

                                alarmPopupV.Owner = Application.Current.MainWindow;
                                alarmPopupV.ShowDialog();

                                //UserControl uc = MapFrameViewModel.Instance.Workspace.Where(p =>
                                //{
                                //    return (p.DataContext as PMSLayoutViewModel).IsActive == true;
                                //}).FirstOrDefault();

                                //var result = (uc.DataContext as PMSLayoutViewModel).DmComponentM;

                                //if (result.PropertyM.IsAlarmLayout)
                                //{
                                //    if (result.PropertyM.AlarmCode.Contains(targetId))
                                //    {
                                //        alarmPopupV = new AbnormalAlarmPopupView();
                                //        var data = alarmPopupV.DataContext as AbnormalAlarmPopupViewModel;

                                //        DmResourceSensorModel resultLinq = LinqManager.FilterFirstResource(targetId);
                                //        data.Title = resultLinq == null ? string.Empty : resultLinq.ResourceName;
                                //        data.Code = targetId;

                                //        alarmPopupV.Owner = Application.Current.MainWindow;
                                //        alarmPopupV.ShowDialog();
                                //    }

                                //    IsMqttMessage = false;
                                //}
                                //else
                                //{
                                //    alarmPopupV = new AbnormalAlarmPopupView();
                                //    var data = alarmPopupV.DataContext as AbnormalAlarmPopupViewModel;

                                //    DmResourceSensorModel resultLinq = LinqManager.FilterFirstResource(targetId);
                                //    data.Title = resultLinq == null ? string.Empty : resultLinq.ResourceName;
                                //    data.Code = targetId;

                                //    alarmPopupV.Owner = Application.Current.MainWindow;
                                //    alarmPopupV.ShowDialog();
                                //}
                            }));
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        private void SetResourceSensor(DmResourceSensorModel data)
        {
            var resultLinq = BaseSingletonManager.Instance.ResourceSensorList.FindIndex(p => p.ResourceCode == data.ResourceCode);

            if (resultLinq == -1)
            {
                return;
            }

            if (data.LimitHigh != null)
            {
                BaseSingletonManager.Instance.ResourceSensorList[resultLinq].LimitHigh = data.LimitHigh;
            }

            if (data.LimitLow != null)
            {
                BaseSingletonManager.Instance.ResourceSensorList[resultLinq].LimitLow = data.LimitLow;
            }

            if (data.MLimitHigh != null)
            {
                BaseSingletonManager.Instance.ResourceSensorList[resultLinq].MLimitHigh = data.MLimitHigh;
            }

            if (data.MLimitLow != null)
            {
                BaseSingletonManager.Instance.ResourceSensorList[resultLinq].MLimitLow = data.MLimitLow;
            }
        }

        /// <summary>
        /// 항목: 공정, 작업장, 설비, 단말기
        /// </summary>
        private void SetUpperAlarmStatus()
        {
            List<DmRoutingConfigurationAlarmStatusModel> resultAPI = CoPlatformWebClient.Instance.GetRoutingConfigutaionAlarmStatus(null);

            foreach (DmComponentMst item in LinqManager.FilterWhereUpperAlarmStatus())
            {
                var resultLinq = resultAPI.FirstOrDefault(p =>
                {
                    return p.RoutingCode == item.PropertyM.ItemCode;
                });

                if (resultLinq == null)
                {
                    continue;
                }

                item.PropertyM.AlarmState = (AlarmState)resultLinq.AlarmLevel;
                item.PropertyM.BlinkingLevel = resultLinq.BlinkingLevel;
            }
        }

        private async void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "MQTT Publish");

            if (IsMqttMessage == true)
            {
                return;
            }

            await Task.Run(() =>
            {
                IsMqttMessage = true;
                SubscribeAsyncMqtt(e);
            });
        }
    }
}
