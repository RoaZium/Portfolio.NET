using DMS.Module.Map.Demo;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model;
using Newtonsoft.Json;
using PrismWPF.Common;
using PrismWPF.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_ParetoChartViewModel : CP_CommonBaseViewModel
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
                ChartSeriesList01 = chartSampleData.SetParetoChartData01();
                ChartSeriesList02 = chartSampleData.SetParetoChartData02();
            }
        }

        protected override void OnSubscribeAsyncMqtt(MqttMsgPublishEventArgs e)
        {
            try
            {
                base.OnSubscribeAsyncMqtt(e);

                var data = JsonConvert.DeserializeObject<List<DmChartDataModel>>(TopicMessage);

                if (data == null)
                {
                    return;
                }

                DmComponentM.ChartSeriesList = data.ToObservableCollection();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "ParetoChart MQTT:" + ex.ToString());
            }
        }

        public ObservableCollection<DmChartDataModel> _chartSeriesList01;
        public ObservableCollection<DmChartDataModel> ChartSeriesList01
        {
            get
            {
                if (_chartSeriesList01 == null)
                {
                    _chartSeriesList01 = new ObservableCollection<DmChartDataModel>();
                }
                return _chartSeriesList01;
            }
            set
            {
                _chartSeriesList01 = value;
                RaisePropertyChanged("ChartSeriesList01");
            }
        }

        public ObservableCollection<DmChartDataModel> _chartSeriesList02;
        public ObservableCollection<DmChartDataModel> ChartSeriesList02
        {
            get
            {
                if (_chartSeriesList02 == null)
                {
                    _chartSeriesList02 = new ObservableCollection<DmChartDataModel>();
                }
                return _chartSeriesList02;
            }
            set
            {
                _chartSeriesList02 = value;
                RaisePropertyChanged("ChartSeriesList02");
            }
        }
    }
}
