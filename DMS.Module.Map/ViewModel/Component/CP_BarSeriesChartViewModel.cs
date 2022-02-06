using DMS.Module.Map.Demo;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model;
using Newtonsoft.Json;
using PrismWPF.Common;
using PrismWPF.Common.Infrastructure;
using System;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_BarSeriesChartViewModel : CP_CommonBaseViewModel
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
                DmComponentM.ChartSeriesList = chartSampleData.SetBarSeriesChartData();
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
            catch(Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "BarSeriesChart MQTT:" +  ex.ToString());
            }
        }
    }
}
