using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model.RestAPI;
using Prism.Commands;
using PrismWPF.Common;
using PrismWPF.Common.Converter;
using PrismWPF.Common.Infrastructure;
using System;
using System.Linq;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_AlarmStateViewModel : CP_CommonBaseViewModel
    {
        public override void Load()
        {
            base.Load();

            SetMqttChannel();
        }

        private CP_CommonBaseViewModel _detailInfoVM;
        public CP_CommonBaseViewModel DetailInfoVM
        {
            get
            { 
                if(_detailInfoVM == null)
                {
                    _detailInfoVM = new CP_DetailInformationViewModel();
                }
                return _detailInfoVM;
            }
            set
            {
                _detailInfoVM = value;
                RaisePropertyChanged("DetailInfoVM");
            }
        }

        protected override void OnComponentData() { }

        protected override void OnSubscribeAsyncMqtt(MqttMsgPublishEventArgs e)
        {
            try
            {
                base.OnSubscribeAsyncMqtt(e);

                DmComponentM.PropertyM.AlarmState = (AlarmState)int.Parse(TopicMessage);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "AlarmState MQTT:" + ex.ToString());
            }
        }

        public DelegateCommand OpenedCommand => new DelegateCommand(OpenedCommandExecute);

        private void OpenedCommandExecute()
        {
            try
            {
                DetailInfoVM.DmComponentM = BaseSingletonManager.Instance.ComponentBaseM.FirstOrDefault(p => p.RoutingType == ComponentType.DetailInfo.ToString()).CopyObject<DmComponentMst>();

                DetailInfoVM.DmComponentM.PropertyM.ItemCode = DmComponentM.PropertyM.ItemCode;
                DetailInfoVM.DmComponentM.PropertyM.ItemType = DmComponentM.PropertyM.ItemType;

                DetailInfoVM.Load();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }
    }
}
