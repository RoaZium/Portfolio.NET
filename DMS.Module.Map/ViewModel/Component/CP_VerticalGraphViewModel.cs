using DMS.Module.Map.Demo;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model;
using PrismWPF.Common;
using System;
using System.Windows;
using System.Windows.Threading;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_VerticalGraphViewModel : CP_CommonBaseViewModel
    {
        public override void Load()
        {
            base.Load();

            SetMqttChannel();

            DmComponentM.ResourceM = LinqManager.FilterFirstResourceSensor(DmComponentM.PropertyM.ItemCode);
        }

        public override void Unload()
        {
            base.Unload();
        }

        protected override void OnComponentData()
        {
            if (DmComponentM.PropertyM.CategoryCode == "0")
            {
                Demo_ChartData chartSampleData = new Demo_ChartData();
                DmComponentM.ChartSeriesList = chartSampleData.SetBarChartData();
            }
            else
            {
                MapFrameViewModel.Instance.LoadResourceSensorData(DmComponentM);
            }
        }

        protected override void OnSubscribeAsyncMqtt(MqttMsgPublishEventArgs e)
        {
            try
            {
                base.OnSubscribeAsyncMqtt(e);

                DmChartDataModel dmChartDataModel = new DmChartDataModel
                {
                    ArgumentDataMember = DateTime.Now.ToString("HH:mm:ss"),
                    ValueDataMember = TopicMessage
                };

                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (DmComponentM.ChartSeriesList.Count > ConstantManager.MAX_LIST_SIZE)
                    {
                        DmComponentM.ChartSeriesList.RemoveAt(0);
                    }

                    DmComponentM.ChartSeriesList.Add(dmChartDataModel);
                }));

                DmComponentM.PropertyM.TextContent = TopicMessage;
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "VerticalGraph MQTT:" + ex.ToString());
            }
        }

        readonly DispatcherTimer timer = new DispatcherTimer();

        private DateTime _maxDateTime;
        public DateTime MaxDateTime
        {
            get => _maxDateTime;
            set
            {
                _maxDateTime = value;
                RaisePropertyChanged("MaxDateTime");
            }
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            DmComponentM.ChartSeriesList.Add(new DmChartDataModel()
            {
                ArgumentDataMember = DateTime.Now.ToString("HH:mm:ss"),
            });

            MaxDateTime = DateTime.Now;

            if (DmComponentM.ChartSeriesList.Count == ConstantManager.MAX_LIST_SIZE)
            {
                DmComponentM.ChartSeriesList.RemoveAt(0);
            }
        }
    }
}
