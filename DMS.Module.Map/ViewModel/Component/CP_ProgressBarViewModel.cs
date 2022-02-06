using DMS.Module.Map.Demo;
using DMS.Module.Map.Infrastructure;
using PrismWPF.Common;
using System;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_ProgressBarViewModel : CP_CommonBaseViewModel
    {
        public override void Load()
        {
            base.Load();

            SetMqttChannel();
        }

        protected override void OnComponentData()
        {
            if (DmComponentM.PropertyM.CategoryCode == "0")
            {
                Demo_ChartData chartSampleData = new Demo_ChartData();
                DmComponentM.PropertyM.TextContent = chartSampleData.SetProgressBarData();
            }
        }

        protected override void OnSubscribeAsyncMqtt(MqttMsgPublishEventArgs e)
        {
            try
            {
                base.OnSubscribeAsyncMqtt(e);

                DmComponentM.PropertyM.TextContent = TopicMessage;
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "ProgressBar MQTT:" + ex.ToString());
            }
        }
    }
}
