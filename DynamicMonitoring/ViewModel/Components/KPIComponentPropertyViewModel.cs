using DynamicMonitoring.Common;
using DynamicMonitoring.Common.Broadcast;
using DynamicMonitoring.Common.Enums;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Resources;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace DynamicMonitoring.ViewModel
{
    public class KPIComponentPropertyViewModel : DMViewModelBase
    {
        private KPIComponentModel _KPICPModel;
        public KPIComponentModel KPICPModel
        {
            get
            {
                if (_KPICPModel == null)
                {
                    _KPICPModel = new KPIComponentModel();
                }
                return _KPICPModel;
            }
            set
            {
                _KPICPModel = value;
                RaisePropertyChanged("KPICPModel");
            }
        }

        public KPIComponentPropertyViewModel()
        {
        }

        public override void Load()
        {
            SelectedKPIList = KPICPModel.TargetCode;

            if (KPICPModel.GraphType != null)
            {
                GraphType = KPICPModel.GraphType;
            }
            else
            {
                GraphType = "Linear";
            }

            SetKPIList();

            if (KPICPModel.TargetCode == null)
                SelectedKPIList = KPIList[0].Id.ToString();
        }

        public override void Unload()
        {
        }

        private ObservableCollection<ComboSelectionInfo> _KPIList;
        public ObservableCollection<ComboSelectionInfo> KPIList
        {
            get
            {
                if (_KPIList == null)
                {
                    _KPIList = new ObservableCollection<ComboSelectionInfo>();
                }
                return _KPIList;
            }
        }

        private string _SelectedKPIList;
        public string SelectedKPIList
        {
            get { return _SelectedKPIList; }
            set
            {
                _SelectedKPIList = value;
                KPICPModel.TargetCode = _SelectedKPIList;
                RaisePropertyChanged("SelectedKPIList");
            }
        }

        private string _GraphType = "Linear";
        public string GraphType
        {
            get { return _GraphType; }
            set
            {
                _GraphType = value;
                KPICPModel.GraphType = _GraphType;
                RaisePropertyChanged("GraphType");
            }
        }

        private bool _IsCheckedLine = true;
        public bool IsCheckedLine
        {
            get { return _IsCheckedLine; }
            set
            {
                _IsCheckedLine = value;

                if (_IsCheckedLine == true)
                {
                    GraphType = "Linear";
                }
                else
                {
                    GraphType = "Bar";
                }

                RaisePropertyChanged("IsCheckedLine");
            }
        }

        private bool _IsCheckedBar;
        public bool IsCheckedBar
        {
            get { return _IsCheckedBar; }
            set
            {
                _IsCheckedBar = value;

                RaisePropertyChanged("IsCheckedBar");
            }
        }

        private void SetKPIList()
        {
            DataTable dt = DBHelper.GetKPISelection();

            foreach (DataRow row in dt.Rows)
            {
                string kpicode = string.Empty;
                string kpiname = string.Empty;
                string title = string.Empty;

                kpicode = row["kpi_code"].ToString();
                kpiname = row["kpi_name"].ToString();
                title = row["title"].ToString();

                KPIList.Add(new ComboSelectionInfo(kpicode, kpiname, title));
            }

            if (GraphType == "Linear")
                IsCheckedLine = true;
            else
                IsCheckedBar = true;
        }

        public DelegateCommand OKCommand
        {
            get { return new DelegateCommand(OKCommandExecute); }
        }

        public DelegateCommand CancelCommand
        {
            get { return new DelegateCommand(CancelCommandExecute); }
        }

        private void OKCommandExecute()
        {
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_ADD_MAPCOMPONENT, KPICPModel, ComponentType.KPI);
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, null, null);
        }

        private void CancelCommandExecute()
        {
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, null, null);
        }
    }
}
