using Coever.Lib.Mqtt.Core;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model.RestAPI;
using Prism.Commands;
using PrismWPF.Common;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using uPLibrary.Networking.M2Mqtt.Messages;
using EnvSettings = DMS.Module.EnvironmentSetting.Properties;

namespace DMS.Module.Map.ViewModel.Component
{
    public abstract class CP_CommonBaseViewModel : DMViewModelBase
    {
        public override void Load()
        {
            OnComponentData();
        }

        public override void Unload()
        {
        }

        private DmComponentMst _dmComponentM;
        public DmComponentMst DmComponentM
        {
            get
            {
                if (_dmComponentM == null)
                {
                    _dmComponentM = new DmComponentMst();
                }
                return _dmComponentM;
            }
            set
            {
                _dmComponentM = value;
                RaisePropertyChanged("DmComponentM");
            }
        }

        private CoeverMqttClient MqttClient;

        private string[] topicList = null;

        public string TopicMessage;
        public string TopicName;
        public string TopicCode;

        /// <summary>
        /// MQTT 채널 설정
        /// </summary>
        /// <param name="topic"></param>
        public void SetMqttChannel()
        {
            try
            {
                //1.Host 및 Port 정보 유효성 체크
                string brokerHostName = EnvSettings.Settings.Default.ServerUrl?.Host;
                int brokerPort = EnvSettings.Settings.Default.MqttPort == 0 ? 1883 : EnvSettings.Settings.Default.MqttPort;

                if (string.IsNullOrEmpty(brokerHostName))
                {
                    LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "Not Exist brokerHost Info");
                    return;
                }

                MqttClient = new CoeverMqttClient(brokerHostName, brokerPort, false, null, null, 0);
                MqttClient.MqttMsgPublishReceived += MqttMsgPublishReceived;

                SetMqttSubscribe();
            }
            catch(Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        /// <summary>
        /// MQTT 구독 설정
        /// </summary>
        /// <param name="topic"></param>
        public void SetMqttSubscribe()
        {
            try
            {
                if(topicList != null)
                {
                    MqttClient?.Unsubscribe(topicList);
                }

                switch (DmComponentM.RoutingType)
                {
                    case nameof(ComponentType.DM601001):
                        {
                            break;
                        }
                    case nameof(ComponentType.DM601005):
                        {
                            topicList = new string[] { ConstantManager.MQTTTopic_AlarmState + DmComponentM.BaseInfoCode };
                            break;
                        }
                    case nameof(ComponentType.AlarmHistory):
                        {
                            topicList = new string[] { ConstantManager.MQTTTopic_AlarmState + "#" };
                            break;
                        }
                    case nameof(ComponentType.DM801003):
                        {
                            topicList = new string[]
                            {
                                ConstantManager.MQTTTopic_SensorData + DmComponentM.BaseInfoCode,
                                ConstantManager.MQTTTopic_AlarmState + DmComponentM.BaseInfoCode
                            };

                            break;
                        }
                    case nameof(ComponentType.BarSeries):
                    case nameof(ComponentType.LineSeries):
                    case nameof(ComponentType.NestedDonutSeries):
                    case nameof(ComponentType.ParetoChart):
                    case nameof(ComponentType.PieChart):
                    case nameof(ComponentType.ScatterChart):
                    case nameof(ComponentType.XbarChart):
                        {
                            topicList = new string[] { ConstantManager.MQTTTopic_Chart + DmComponentM.BaseInfoCode };
                            break;
                        }
                    case nameof(ComponentType.DM701001):
                        {
                            topicList = new string[] { ConstantManager.MQTTTopic_Image + DmComponentM.BaseInfoCode };
                            break;
                        }
                    case nameof(ComponentType.DataGrid):
                        {
                            topicList = new string[] { ConstantManager.MQTTTopic_DataGrid + DmComponentM.BaseInfoCode };
                            break;
                        }
                    case nameof(ComponentType.AlarmState):
                    case nameof(ComponentType.Animation):
                        {
                            topicList = new string[] { ConstantManager.MQTTTopic_AlarmState + DmComponentM.BaseInfoCode };
                            break;
                        }
                    case nameof(ComponentType.Gif):
                        {
                            topicList = new string[] { ConstantManager.MQTTTopic_AlarmState + DmComponentM.BaseInfoCode };
                            break;
                        }
                    default:
                        {
                            topicList = new string[] { ConstantManager.MQTTTopic_SensorData + DmComponentM.BaseInfoCode };
                            break;
                        }
                }

                byte[] qos = new byte[topicList.Length];
                MqttClient?.Subscribe(topicList, qos);

                if(MqttClient == null || MqttClient.ClientId == null)
                {
                    MqttClient?.ConnectRandomId(this.GetType().Name);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        private async void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            await Task.Run(() =>
            {
                OnSubscribeAsyncMqtt(e);
            });
        }

        public DelegateCommand ComponentPreviewMouseLeftButtonUpCommand => new DelegateCommand(ComponentPreviewMouseLeftButtonUpCommandExecute);
        public DelegateCommand ComponentPreviewMouseLeftButtonDownCommand => new DelegateCommand(ComponentPreviewMouseLeftButtonDownCommandExecute);

        public DelegateCommand<MouseButtonEventArgs> ComponentMouseLeftButtonDownCommand => new DelegateCommand<MouseButtonEventArgs>(ComponentMouseLeftButtonDownCommandExecute);

        private void ComponentPreviewMouseLeftButtonUpCommandExecute()
        {
            try
            {
                if (MapFrameViewModel.Instance.IsMapEdit == false)
                {
                    return;
                }

                MapFrameViewModel.Instance.ComponentMst = DmComponentM;
                MapFrameViewModel.Instance.ComponentMst.ResourceM = DmComponentM.ResourceM;

                //설정된 타입, 항목 셋팅
                MapFrameViewModel.Instance.SelectedCategoryData();

                bool result = DataUtils.CompareJsonData(DmComponentM, MapFrameViewModel.Instance.PMSLayoutVM.OriginComponentData);

                if (result == false)
                {
                    MapFrameViewModel.Instance.PMSLayoutVM.OriginComponentData.Action = ActionType.U;
                    DmComponentM.Action = ActionType.U;
                    MapFrameViewModel.Instance.PMSLayoutVM.RevertBackList.Add(MapFrameViewModel.Instance.PMSLayoutVM.OriginComponentData.CopyObject());
                    MapFrameViewModel.Instance.PMSLayoutVM.OriginComponentData = DmComponentM.CopyObject();
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "[Component Property]: " + ex.ToString());
            }

            (MapFrameViewModel.Instance.PMSLayoutVM as PMSLayoutViewModel).UpdateComponentmst(DmComponentM);
        }

        private void ComponentPreviewMouseLeftButtonDownCommandExecute()
        {
            try
            {
                if (MapFrameViewModel.Instance.IsMapEdit == false)
                {
                    return;
                }

                //foreach (var item in MapFrameViewModel.Instance.PMSLayoutVM.ComponentList)
                //{
                //    (item.DataContext as CP_CommonBaseViewModel).DmComponentM.IsSelectMode = Visibility.Collapsed;
                //}

                foreach (var item in MapFrameViewModel.Instance.PMSLayoutVM.VMList)
                {
                    item.DmComponentM.IsSelectMode = Visibility.Collapsed;
                }

                DmComponentM.IsSelectMode = Visibility.Visible;

                MapFrameViewModel.Instance.PMSLayoutVM.OriginComponentData = DmComponentM.CopyObject();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "[Component Property]: " + ex.ToString());
            }
        }

        private void ComponentMouseLeftButtonDownCommandExecute(MouseButtonEventArgs e)
        {
            if (e.ClickCount <= 1)
            {
                return;
            }

            if (MapFrameViewModel.Instance.IsAutoMonitoringMode == true)
            {
                return;
            }

            DmComponentMst data = (MapFrameViewModel.Instance.AssetListV.DataContext as Asset_ListViewModel).LayoutList.FirstOrDefault(p => p.RoutingCode == DmComponentM.RoutingCode);

            if (data == null)
            {
                return;
            }

            //공정구성 항목만 레이아웃 생성(공정/작업장/설비/단말기/센서/툴)
            switch ((ComponentType)Enum.Parse(typeof(ComponentType), data.RoutingType))
            {
                case ComponentType.DM601001:
                case ComponentType.DM601002:
                case ComponentType.DM601003:
                case ComponentType.DM601004:
                case ComponentType.DM601005:
                case ComponentType.DM701000:
                    {
                        MapFrameViewModel.Instance.SelectedComponentConfig(data);
                        break;
                    }
            }
        }

        protected abstract void OnComponentData();

        protected virtual void OnSubscribeAsyncMqtt(MqttMsgPublishEventArgs e)
        {
            try
            {
                string[] topicParts = e.Topic.Split('/');
                //string eventType = topicParts[2];
                string targetType = topicParts[3];
                string targetId = topicParts[4];

                TopicName = targetType;
                TopicCode = targetId;
                TopicMessage = Encoding.UTF8.GetString(e.Message);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }
    }
}
