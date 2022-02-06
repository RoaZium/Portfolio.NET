using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Common;
using DynamicMonitoring.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DevExpress.Mvvm;
using DynamicMonitoring.Model;
using System.Collections.ObjectModel;
using System.Data;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Utils;
using DynamicMonitoring.Common.Broadcast;
using DynamicMonitoring.Common.Enums;

namespace DynamicMonitoring.ViewModel
{
    public class AlarmComponentPropertyViewModel : DMViewModelBase
    {
        public Dictionary<string, List<string>> _SelectionInclusionList = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> _SelectionExclusionList = new Dictionary<string, List<string>>();

        /// <summary>
        /// AlarmComponent Common Model
        /// </summary>
        private AlarmComponentModel _AlarmModel;
        public AlarmComponentModel AlarmModel
        {
            get
            {
                if (_AlarmModel == null)
                {
                    _AlarmModel = new AlarmComponentModel();
                }
                return _AlarmModel;
            }
            set
            {
                _AlarmModel = value;
                RaisePropertyChanged("AlarmModel");
            }
        }

        public AlarmComponentPropertyViewModel()
        {
        }

        public override void Load()
        {
            BroadcastReceiver.RegisterReceiver(Constants.BROADCAST_MAP_ALARMANDDNP, OnBroadcastSaveInfo);
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_ADD_MAPCOMPONENT, null, ComponentType.ALM);

            if (!string.IsNullOrEmpty(AlarmModel.TargetType))
            {
                SelectedAlarmType = AlarmModel.TargetType;
            }

            LoadAlarmSelectionList();
        }

        public override void Unload()
        {
            BroadcastReceiver.UnregisterReceiver(Constants.BROADCAST_MAP_ALARMANDDNP, OnBroadcastSaveInfo);
        }

        private ObservableCollection<ComboSelectionInfo> _alarmTypeList;
        public ObservableCollection<ComboSelectionInfo> AlarmTypeList
        {
            get
            {
                if (_alarmTypeList == null)
                {
                    _alarmTypeList = new ObservableCollection<ComboSelectionInfo>();

                    _alarmTypeList.Add(new ComboSelectionInfo("TG001", "공정", "TG001"));
                    _alarmTypeList.Add(new ComboSelectionInfo("TG002", "센서", "TG002"));
                }
                return _alarmTypeList;
            }
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

        private ObservableCollection<TargetSelectionModel> _sensorList;
        public ObservableCollection<TargetSelectionModel> SensorList
        {
            get
            {
                if (_sensorList == null)
                {
                    _sensorList = new ObservableCollection<TargetSelectionModel>();
                }
                return _sensorList;
            }
        }

        private string _selectedAlarmType = "TG001";
        public string SelectedAlarmType
        {
            get { return _selectedAlarmType; }
            set
            {
                _selectedAlarmType = value;
                RaisePropertyChanged("SelectedAlarmType");
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

        private string _selectedSensorCode;
        public string SelectedSensorCode
        {
            get { return _selectedSensorCode; }
            set
            {
                _selectedSensorCode = value;
                RaisePropertyChanged("SelectedSensorCode");
            }
        }

        public async void LoadAlarmSelectionList()
        {
            try
            {
                List<DataTable> dts = await DBHelper.GetAlarmTargetSelection();

                if (dts != null)
                {
                    List<TargetSelectionModel> selectionList = dts[0].ToObjectList<TargetSelectionModel>();
                    List<TargetSelectionModel> exclusionSelectionList = dts[1].ToObjectList<TargetSelectionModel>();

                    exclusionSelectionList.RemoveAll(item =>
                    {
                        if (_SelectionInclusionList.ContainsKey(item.TargetType))
                        {
                            return _SelectionInclusionList[item.TargetType].Exists(code => code == item.TargetCode);
                        }
                        else if (AlarmModel.TargetType == item.TargetType && AlarmModel.TargetCode == item.TargetCode)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });

                    Dictionary<string, List<string>> exclusionCodeList = new Dictionary<string, List<string>>();

                    foreach (TargetSelectionModel exclusionSelection in exclusionSelectionList)
                    {
                        if (!exclusionCodeList.ContainsKey(exclusionSelection.TargetType))
                        {
                            exclusionCodeList[exclusionSelection.TargetType] = new List<string>();
                        }

                        exclusionCodeList[exclusionSelection.TargetType].Add(exclusionSelection.TargetCode);
                    }

                    foreach (KeyValuePair<string, List<string>> list in _SelectionExclusionList)
                    {
                        if (!exclusionCodeList.ContainsKey(list.Key))
                        {
                            exclusionCodeList[list.Key] = new List<string>();
                        }

                        exclusionCodeList[list.Key].AddRange(list.Value);
                    }

                    selectionList.RemoveAll(item =>
                    {
                        if (AlarmModel.TargetType == item.TargetType && AlarmModel.TargetCode == item.TargetCode)
                        {
                            return false;
                        }
                        else if (exclusionCodeList.ContainsKey(item.TargetType))
                        {
                            return exclusionCodeList[item.TargetType].Exists(code => code == item.TargetCode);
                        }
                        else
                        {
                            return false;
                        }
                    });

                    var processSelectionList = selectionList.Where(item => item.TargetType == "TG001");
                    var sensorSelectionList = selectionList.Where(item => item.TargetType == "TG002");

                    ProcessList.AddRange(processSelectionList);
                    SensorList.AddRange(sensorSelectionList);

                    if (AlarmModel.TargetType != null && AlarmModel.TargetType == "TG001" && AlarmModel.TargetCode != null && ProcessList.FindIndex(item => item.TargetCode == AlarmModel.TargetCode) > 0)
                    {
                        SelectedProcessCode = AlarmModel.TargetCode;
                    }
                    else
                    {
                        if (ProcessList.Count > 0)
                        {
                            SelectedProcessCode = ProcessList[0].TargetCode;
                        }
                    }

                    if (AlarmModel.TargetType != null && AlarmModel.TargetType == "TG002" && AlarmModel.TargetCode != null
                        && SensorList.FindIndex(item => item.TargetCode == AlarmModel.TargetCode) > 0)
                    {
                        SelectedSensorCode = AlarmModel.TargetCode;
                    }
                    else
                    {
                        if (SensorList.Count > 0)
                        {
                            SelectedSensorCode = SensorList[0].TargetCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
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
            _SelectionInclusionList = sender as Dictionary<string, List<string>>;
            _SelectionExclusionList = e as Dictionary<string, List<string>>;
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
            if (SelectedAlarmType == null || (SelectedAlarmType == "TG001" && SelectedProcessCode == null) || (SelectedAlarmType == "TG002" && SelectedSensorCode == null))
            {
                MessageBox.Show(Res.MsgIncorrectInput);
            }

            AlarmModel.X = AlarmModel.X;
            AlarmModel.Y = AlarmModel.Y;
            AlarmModel.Width = AlarmModel.Width;
            AlarmModel.Height = AlarmModel.Height;
            AlarmModel.TargetType = SelectedAlarmType;

            if (SelectedAlarmType == "TG001")
                AlarmModel.TargetCode = SelectedProcessCode;
            else
                AlarmModel.TargetCode = SelectedSensorCode;

            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_ADD_MAPCOMPONENT, AlarmModel, ComponentType.ALM);
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, null, null);
        }

        private void CancelCommandExecute()
        {
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, null, null);
        }
    }
}
