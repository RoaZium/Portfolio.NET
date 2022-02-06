using DMS.Module.Map.Infrastructure;
using PrismWPF.Common;
using PrismWPF.Common.Converter;
using System;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_DataBoxViewModel : CP_CommonBaseViewModel
    {
        public override void Load()
        {
            base.Load();

            SetMqttChannel();
        }

        protected override void OnComponentData() { }

        protected override void OnSubscribeAsyncMqtt(MqttMsgPublishEventArgs e)
        {
            try
            {
                base.OnSubscribeAsyncMqtt(e);

                if (TopicName == nameof(MQTTTopic.alarm_state))
                {
                    DmComponentM.PropertyM.AlarmState = (AlarmState)int.Parse(TopicMessage);
                }
                else
                {
                    DmComponentM.PropertyM.TextContent = TopicMessage;
                }
            }
            catch(Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "DataBox MQTT:" + ex.ToString());
            }
        }
    }
}
