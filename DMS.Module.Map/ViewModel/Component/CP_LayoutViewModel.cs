using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model.RestAPI;
using DMS.Module.Map.View.Component;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_LayoutViewModel : CP_CommonBaseViewModel
    {
        public override void Load()
        {
            LoadComponentList();
        }

        public override void Unload()
        {
        }

        protected override void OnComponentData()
        {
        }

        private ObservableCollection<CP_CommonBaseViewModel> _vMList;
        public ObservableCollection<CP_CommonBaseViewModel> VMList
        {
            get
            {
                if (_vMList == null)
                {
                    _vMList = new ObservableCollection<CP_CommonBaseViewModel>();
                }
                return _vMList;
            }
            set
            {
                _vMList = value;
                RaisePropertyChanged("VMList");
            }
        }

        public async void LoadComponentList()
        {
            await Task.Run(() =>
            {
                if(DmComponentM.BaseInfoCode == null)
                {
                    return;
                }

                VMList = null;

                var resultLinq = LinqManager.FilterWhereLayout(DmComponentM.BaseInfoCode);

                if (resultLinq == null)
                {
                    return;
                }

                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    foreach (var item in resultLinq)
                    {
                        var data = BaseSingletonManager.Instance.ConvertDMSMasterDataViewModel((ComponentType)Enum.Parse(typeof(ComponentType), item.RoutingType));

                        if (data != null)
                        {
                            data.DmComponentM = item;
                            data.Load();
                            VMList.Add(data);
                        }
                    }
                }));
            });
        }

        private DmResourceSensorModel SetResourceSensor(DmComponentMst dmComponentMst)
        {
            DmResourceSensorModel dmResourceSensorModel = null;

            if (dmComponentMst.RoutingType == ComponentType.DM801001.ToString()
                || dmComponentMst.RoutingType == ComponentType.DM801002.ToString()
                || dmComponentMst.RoutingType == ComponentType.LineSeries.ToString()
                || dmComponentMst.RoutingType == ComponentType.BarSeries.ToString()
                || dmComponentMst.RoutingType == ComponentType.Gauge.ToString()
                || dmComponentMst.RoutingType == ComponentType.Indicator.ToString()
                || dmComponentMst.RoutingType == ComponentType.NestedDonutSeries.ToString()
                || dmComponentMst.RoutingType == ComponentType.ParetoChart.ToString()
                || dmComponentMst.RoutingType == ComponentType.PieChart.ToString()
                || dmComponentMst.RoutingType == ComponentType.ScatterChart.ToString()
                || dmComponentMst.RoutingType == ComponentType.XbarChart.ToString())
            {
                dmResourceSensorModel = LinqManager.FilterFirstResourceSensor(dmComponentMst.PropertyM.ItemCode);
            }

            return dmResourceSensorModel;
        }

        public DelegateCommand LayoutMoveCommand => new DelegateCommand(LayoutMoveCommandExecute);

        private void LayoutMoveCommandExecute()
        {
            DmComponentMst resultLinq = LinqManager.FilterFirstRoutingCode(DmComponentM.PropertyM.ItemCode);

            if(resultLinq == null)
            {
                return;
            }

            MapFrameViewModel.Instance.SelectedComponentConfig(resultLinq);
        }
    }
}
