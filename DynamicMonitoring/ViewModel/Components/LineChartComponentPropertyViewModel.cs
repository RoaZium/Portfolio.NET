using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using System.Threading;
using System.Data;

using DynamicMonitoring.Model;
using DynamicMonitoring.Common;
using DynamicMonitoring.Resources;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Common.MVVM;
using Prism.Commands;
using DynamicMonitoring.Common.Broadcast;
using DynamicMonitoring.Common.Enums;

namespace DynamicMonitoring.ViewModel
{
    public class LineChartComponentPropertyViewModel : DMViewModelBase
    {
        private LineChartComponentModel _LineCPModel;
        public LineChartComponentModel LineCPModel
        {
            get
            {
                if (_LineCPModel == null)
                {
                    _LineCPModel = new LineChartComponentModel();
                }
                return _LineCPModel;
            }
            set
            {
                _LineCPModel = value;
                RaisePropertyChanged("LineCPModel");
            }
        }

        public override void Load()
        {
            LoadSensorList();
        }

        public override void Unload()
        {
        }

        public LineChartComponentModel _targetComponent;

        private ObservableCollection<ComboSelectionInfo> _sensorList;
        public ObservableCollection<ComboSelectionInfo> SensorList
        {
            get
            {
                if (_sensorList == null)
                {
                    _sensorList = new ObservableCollection<ComboSelectionInfo>();
                }
                return _sensorList;
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

        public LineChartComponentPropertyViewModel()
        {
        }

        private void LoadSensorList()
        {
            new Thread(() =>
            {
                try
                {
                    DataTable dt = DBHelper.GetSensorSelection();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Dictionary<string, ComboSelectionInfo> sensorSelectionList = new Dictionary<string, ComboSelectionInfo>();

                        foreach (DataRow row in dt.Rows)
                        {
                            string sensorCode = null;
                            string sensorName = null;
                            string title = null;

                            if (row.Table.Columns.Contains("resource_code")
                                && row["resource_code"] != DBNull.Value)
                            {
                                sensorCode = row["resource_code"].ToString();
                            }

                            if (sensorCode == null)
                                continue;

                            if (row.Table.Columns.Contains("resource_name")
                                && row["resource_name"] != DBNull.Value)
                            {
                                sensorName = row["resource_name"].ToString();
                            }

                            if (row.Table.Columns.Contains("title")
                                && row["title"] != DBNull.Value)
                            {
                                title = row["title"].ToString();
                            }

                            sensorSelectionList.Add(sensorCode, new ComboSelectionInfo(sensorCode, title, sensorName));
                        }

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (sensorSelectionList.Count > 0)
                            {
                                foreach (var item in sensorSelectionList)
                                {
                                    SensorList.Add(item.Value);
                                }

                                //if (_targetComponent.TargetCode != null && sensorSelectionList.ContainsKey(_targetComponent.TargetCode))
                                //{
                                //    SelectedSensorCode = _targetComponent.TargetCode;
                                //}
                                //else
                                //{
                                //    SelectedSensorCode = SensorList[0].Id.ToString();
                                //}
                            }
                            else
                            {
                                MessageBox.Show(Res.MsgFailedLoadSensorList);
                            }
                        });
                    }
                }
                catch { }
            }).Start();
        }

        public bool ConfirmChange()
        {
            if (SelectedSensorCode == null)
            {
                MessageBox.Show(Res.MsgSensorDidnSelected);
                return false;
            }

            _targetComponent.TargetCode = SelectedSensorCode;

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
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_ADD_MAPCOMPONENT, LineCPModel, ComponentType.LNC);
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, null, null);
        }

        private void CancelCommandExecute()
        {
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, null, null);
        }
    }
}
