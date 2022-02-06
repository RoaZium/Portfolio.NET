using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model;
using Newtonsoft.Json;
using PrismWPF.Common;
using PrismWPF.Common.Converter;
using System;
using System.Collections.Generic;
using System.Windows;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_ImageViewModel : CP_CommonBaseViewModel
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

                var data = JsonConvert.DeserializeObject<List<DmChartDataModel>>(TopicMessage);

                if (data == null)
                {
                    return;
                }

                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    DmComponentM.PropertyM.ImageSource = ObjectConverter.StringToImageSource(TopicMessage);
                }));
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "ImageView MQTT:" + ex.ToString());
            }
        }
    }
}
