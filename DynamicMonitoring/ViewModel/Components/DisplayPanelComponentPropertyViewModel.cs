using DynamicMonitoring.Common;
using DynamicMonitoring.Resources;
using System;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm;
using DynamicMonitoring.Model;
using System.Collections.ObjectModel;
using System.Data;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Utils;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Common.Broadcast;
using DynamicMonitoring.Common.Enums;

namespace DynamicMonitoring.ViewModel
{
    public class DisplayPanelComponentPropertyViewModel : DMViewModelBase
    {
        public List<string> _SelectionInclusionList = new List<string>();
        public List<string> _SelectionExclusionList = new List<string>();

        private DisplayPanelComponentModel _DisplayPanelCPModel;
        public DisplayPanelComponentModel DisplayPanelCPModel
        {
            get
            {
                if (_DisplayPanelCPModel == null)
                {
                    _DisplayPanelCPModel = new DisplayPanelComponentModel();
                }
                return _DisplayPanelCPModel;
            }
            set
            {
                _DisplayPanelCPModel = value;
                RaisePropertyChanged("DisplayPanelCPModel");
            }
        }

        public DisplayPanelComponentPropertyViewModel()
        {
        }

        public override void Load()
        {
            BroadcastReceiver.RegisterReceiver(Constants.BROADCAST_MAP_ALARMANDDNP, OnBroadcastSaveInfo);
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_ADD_MAPCOMPONENT, null, ComponentType.DPN);

            LoadProcessList();
        }

        public override void Unload()
        {
            BroadcastReceiver.UnregisterReceiver(Constants.BROADCAST_MAP_ALARMANDDNP, OnBroadcastSaveInfo);
        }

        private ObservableCollection<TargetSelectionModel> _processList;
        public ObservableCollection<TargetSelectionModel> ProcessList
        {
            get
            {
                if (_processList == null)
                {
                    _processList = new ObservableCollection<TargetSelectionModel>();
                }
                return _processList;
            }
        }

        private string _selectedProcessCode;
        public string SelectedProcessCode
        {
            get { return _selectedProcessCode; }
            set
            {
                _selectedProcessCode = value;
                RaisePropertyChanged("SelectedProcessCode");
            }
        }

        public async void LoadProcessList()
        {
            try
            {
                List<DataTable> dts = await DBHelper.GetProductionProcessSelection();

                if (dts != null)
                {
                    List<TargetSelectionModel> selectionList = dts[0].ToObjectList<TargetSelectionModel>();
                    List<TargetSelectionModel> exclusionSelectionList = dts[1].ToObjectList<TargetSelectionModel>();

                    exclusionSelectionList.RemoveAll(item =>
                    {
                        if (DisplayPanelCPModel.TargetCode == item.TargetCode)
                        {
                            return true;
                        }

                        return _SelectionInclusionList.Exists(code => code == item.TargetCode);
                    });

                    List<string> exclusionCodeList = new List<string>();

                    foreach (TargetSelectionModel exclusionSelection in exclusionSelectionList)
                    {
                        exclusionCodeList.Add(exclusionSelection.TargetCode);
                    }

                    foreach (string exclusionCode in _SelectionExclusionList)
                    {
                        exclusionCodeList.Add(exclusionCode);
                    }

                    selectionList.RemoveAll(item =>
                    {
                        if (DisplayPanelCPModel.TargetCode == item.TargetCode)
                        {
                            return false;
                        }

                        return exclusionCodeList.Exists(code => code == item.TargetCode);
                    });

                    ProcessList.AddRange(selectionList);

                    if (DisplayPanelCPModel.TargetCode != null
                        && ProcessList.FindIndex(item => item.TargetCode == DisplayPanelCPModel.TargetCode) > 0)
                    {
                        SelectedProcessCode = DisplayPanelCPModel.TargetCode;
                    }
                    else
                    {
                        SelectedProcessCode = ProcessList[0].TargetCode;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Res.MsgFailedLoadProcessList + " : " + e.Message);
            }
        }

        public bool ConfirmChange()
        {
            if (SelectedProcessCode == null)
            {
                MessageBox.Show(Res.MsgProcessDidnSelected);
                return false;
            }

            OnConfirmed();

            return true;
        }

        public Action Confirmed { get; set; }

        public void OnConfirmed()
        {
            if (Confirmed != null)
            {
                Confirmed();
            }
        }

        private void OnBroadcastSaveInfo(string actionName, object sender, object e)
        {
            _SelectionInclusionList = sender as List<string>;
            _SelectionExclusionList = e as List<string>;
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
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_ADD_MAPCOMPONENT, DisplayPanelCPModel, ComponentType.DPN);
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, null, null);
        }

        private void CancelCommandExecute()
        {
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, null, null);
        }
    }
}
