using Coever.Lib.CoPlatform.Core.Network;
using DMS.Module.Management.Infrastructure;
using DMS.Module.Management.Model;
using DMS.Module.Management.Model.RestAPI;
using DMS.Module.Management.Network;
using Prism.Commands;
using PrismWPF.Common.Broadcast;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace DMS.Module.Management.ViewModel
{
    public class AlarmManagementViewModel : DMViewModelBase
    {
        public AlarmManagementViewModel()
        {
            var resultAPI = CoPlatformWebClient.Instance.GetComponentMst();

            if (resultAPI == null)
            {
                return;
            }

            IEnumerable<DmComponentMst> resultLinq = resultAPI.Where(p =>
            {
                return
                p.RoutingType == "DM601001" ||
                p.RoutingType == "DM601002" ||
                p.RoutingType == "DM601003" ||
                p.RoutingType == "DM601004" ||
                p.RoutingType == "DM601005" ||
                p.RoutingType == "Screen";
            });

            _layoutList = resultLinq.ToList();
            _layoutListView = new ListCollectionView(_layoutList);

            _TargetList = WebRequests.GetBaseInfo(CoPlatformWebClient.Instance, "DM601005");
            _TargetListView = new ListCollectionView(_TargetList);
        }

        public override void Load()
        {
        }

        public override void Unload()
        {
        }

        private List<DmComponentMst> _layoutList;
        private ListCollectionView _layoutListView;
        public ListCollectionView LayoutListView => _layoutListView;

        private List<DmBaseInfoModel> _TargetList;
        private ListCollectionView _TargetListView;
        public ListCollectionView TargetListView => _TargetListView;

        private DmComponentMst _selectedLayout;
        public DmComponentMst SelectedLayout
        {
            get => _selectedLayout;
            set
            {
                _selectedLayout = value;

                if(_selectedLayout != null)
                {
                    LoadChangeSensor();
                }

                RaisePropertyChanged("SelectedLayout");
            }
        }

        private AlarmScenarioListViewModel _ScenarioListVM = new AlarmScenarioListViewModel();
        public AlarmScenarioListViewModel ScenarioListVM
        {
            get => _ScenarioListVM;
            set
            {
                _ScenarioListVM = value;
                RaisePropertyChanged("ScenarioListVM");
            }
        }

        private Visibility _visibilitySensorList;
        public Visibility VisibilitySensorList
        {
            get => _visibilitySensorList;
            set
            {
                _visibilitySensorList = value;
                RaisePropertyChanged("VisibilitySensorList");
            }
        }

        public DelegateCommand TargetCheckChangedCommand => new DelegateCommand(TargetCheckChangedCommandExecute);
        public DelegateCommand SaveCommand => new DelegateCommand(SaveCommandExecute);

        private void TargetCheckChangedCommandExecute()
        {
            //IsCheckedAll = !_TargetList.Exists(checkableTarget => !checkableTarget.IsChecked);
        }

        private void SaveCommandExecute()
        {
            try
            {
                var resultLinq = _TargetList.Where(target => target.IsChecked);

                string code = string.Empty;

                foreach (var item in resultLinq)
                {
                    code = code + item.BaseInfoCode + ",";
                }

                if (SelectedLayout == null)
                {
                    return;
                }

                SelectedLayout.PropertyM.AlarmCode = code;

                BroadcastReceiver.SendBroadcast(ConstantManager.BROADCAST_ALARMCODE, SelectedLayout.RoutingCode, SelectedLayout.PropertyM.AlarmCode);

                VisibilitySensorList = Visibility.Hidden;
            }
            catch(Exception ex)
            {
                
            }
        }

        private void LoadChangeSensor()
        {
            string codeList = null;
            string[] codes = null;

            //1. 전체 센서 해제
            foreach (var item in _TargetList)
            {
                item.IsChecked = false;
            }

            TargetListView.Filter = null;

            //2. 이미 등록된 센서 목록 필터
            foreach (var item in _layoutList)
            {
                if (item.PropertyM.AlarmCode == null)
                {
                    continue;
                }

                if(item == SelectedLayout)
                {
                    continue;
                }

                codeList = codeList + item.PropertyM.AlarmCode;
            }

            //3-1. 자기 자신 등록된 센서 없으면 끝
            if (SelectedLayout.PropertyM.AlarmCode != null)
            {
                codes = SelectedLayout.PropertyM.AlarmCode.Split(',');
            }

            if (codeList != null)
            {
                TargetListView.Filter = new Predicate<object>((selection) =>
                {
                    return !codeList.Contains((selection as DmBaseInfoModel).BaseInfoCode);
                //return (selection as DmBaseInfoModel).IsChecked == false ||
                //SelectedLayout.PropertyM.AlarmCode.Contains((selection as DmBaseInfoModel).BaseInfoCode);
                });
            }

            if(codes == null)
            {
                return;
            }

            //3-2. 자기 자신 등록된 센서 체크
            foreach (var item in codes)
            {
                var data = _TargetList.FirstOrDefault(p => p.BaseInfoCode == item);

                if(data == null)
                {
                    continue;
                }

                data.IsChecked = true;
            }
        }
    }
}
