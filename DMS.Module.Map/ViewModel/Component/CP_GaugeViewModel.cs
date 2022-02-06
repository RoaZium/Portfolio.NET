using DMS.Module.Map.Infrastructure;
using PrismWPF.Common;
using System;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_GaugeViewModel : CP_CommonBaseViewModel
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

                DmComponentM.PropertyM.TotalValueM.Value = TopicMessage;
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "Gauge MQTT:" + ex.ToString());
            }
        }
    }
}
