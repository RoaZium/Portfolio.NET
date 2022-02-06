using DMS.Module.Map.Infrastructure;
using PrismWPF.Common;
using PrismWPF.Common.Converter;
using System;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_GifViewModel : CP_CommonBaseViewModel
    {
        public override void Load()
        {
            base.Load();
            SetMqttChannel();

            if(DmComponentM.PropertyM.AlarmState == AlarmState.Danger)
            {
                DmComponentM.PropertyM.GifM.AnimationSpeedRatio = 0.000000000001;
            }
            else
            {
                DmComponentM.PropertyM.GifM.AnimationSpeedRatio = 1;
            }
        }

        protected override void OnComponentData()
        {
            DmComponentM.PropertyM.GifM.FilePath = ConstantManager.DIRECTORY_GIFs + "\\" + DmComponentM.PropertyM.GifM.Name;
        }

        protected override void OnSubscribeAsyncMqtt(MqttMsgPublishEventArgs e)
        {
            try
            {
                base.OnSubscribeAsyncMqtt(e);

                if(TopicMessage == "2")
                {
                    DmComponentM.PropertyM.GifM.AnimationSpeedRatio = 0.000000000001;
                }
                else
                {
                    DmComponentM.PropertyM.GifM.AnimationSpeedRatio = 1;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "DataBox MQTT:" + ex.ToString());
            }
        }
    }
}
