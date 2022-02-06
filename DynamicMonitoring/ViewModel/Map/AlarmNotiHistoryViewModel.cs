using DevExpress.Xpf.Docking;
using DynamicMonitoring.Common;
using DynamicMonitoring.Common.Enums;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model;
using DynamicMonitoring.Resources;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DynamicMonitoring.ViewModel
{
    public class AlarmNotiHistoryViewModel : DMViewModelBase, IMVVMDockingProperties
    {
        private bool IsLoaded = false;
        private bool IsThreadRunning = false;
        private int LoadDataTermMilis = 30000;

        string IMVVMDockingProperties.TargetName
        {
            get { return "RightHost"; }
            set { }
        }

        public ImageSource Glyph
        {
            get { return ObjectConverter.ToImageSource(Res.ImgAbnormal); }
        }

        private string _Name;
        public string Name
        {
            get { return "fefwef"; }
            set
            {
                _Name = value;
                RaisePropertyChanged("Name");
            }
        }

        private ObservableCollection<AlarmNotiInfoModel> _NotiList;
        public ObservableCollection<AlarmNotiInfoModel> NotiList
        {
            get
            {
                if (_NotiList == null)
                {
                    _NotiList = new ObservableCollection<AlarmNotiInfoModel>();
                }
                return _NotiList;
            }
            set
            {
                _NotiList = value;
                RaisePropertyChanged("NotiList");
            }
        }

        public AlarmNotiHistoryViewModel()
        {
            LocalizationManager.Instance.CultureChanged += OnCultureChanged;
        }

        public override void Load()
        {
            if (!IsLoaded)
            {
                LoadThreadStart();
                IsLoaded = true;
            }
        }

        public override void Unload()
        {
            IsLoaded = false;
        }

        #region Methods

        public void LoadThreadStart()
		{ 
			new Thread(() =>
            {
                if (!IsThreadRunning)
                {
                    IsThreadRunning = true;
                    while (IsLoaded)
                    {
                        loadNoti();
                        for (int i = 0; i < (LoadDataTermMilis / 100) && IsLoaded; i++)
                        {
                            Thread.Sleep(100);
                        }
                    }
                    IsThreadRunning = false;
                }
            }
                ).Start();
        }

        private void loadNoti()
        {
            try
            {
                DateTime lastDateTime = NotiList == null || NotiList.Count == 0 ? DateTime.MinValue : NotiList[0].AlarmDateTime;
                DataTable dt = DBHelper.GetRecentAlramList100();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        AlarmNotiInfoModel item = DataRowToAlarmNotiInfo(row);

                        if (item == null || item.AlarmDateTime <= lastDateTime)
                        {
                            continue;
                        }

                        Application.Current.Dispatcher.Invoke(() => NotiList.Add(item));

                        if (NotiList.Count > 500)
                        {
                            NotiList.RemoveAt(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private AlarmNotiInfoModel DataRowToAlarmNotiInfo(DataRow row)
        {
            DateTime alarmDateTime = DateTime.MinValue;
            AlarmState alarmState = AlarmState.None;
            string processName = null;
            string sensorCode = null;
            string sensorName = null;
            double sensorValue = 0;
            string sensorMeasureUnit = null;

            if (!row.Table.Columns.Contains("alarm_date") || !DateTime.TryParse(row["alarm_date"].ToString(), out alarmDateTime))
            {
                return null;
            }

            if (row.Table.Columns.Contains("process_name") && row["process_name"] != DBNull.Value)
            {
                processName = row["process_name"].ToString();
            }

            if (row.Table.Columns.Contains("resource_code") && row["resource_code"] != DBNull.Value)
            {
                sensorCode = row["resource_code"].ToString();
            }

            if (row.Table.Columns.Contains("resource_name") && row["resource_name"] != DBNull.Value)
            {
                sensorName = row["resource_name"].ToString();
            }

            if (row.Table.Columns.Contains("collection_value"))
            {
                double.TryParse(row["collection_value"].ToString(), out sensorValue);
            }

            if (row.Table.Columns.Contains("resource_unit") && row["resource_unit"] != DBNull.Value)
            {
                sensorMeasureUnit = row["resource_unit"].ToString();
            }

            if (row.Table.Columns.Contains("alarm_level") && row["alarm_level"] != DBNull.Value && !Enum.TryParse(row["alarm_level"].ToString(), out alarmState))
            {
                alarmState = AlarmState.Unknown;
            }

            return new AlarmNotiInfoModel(alarmDateTime, processName, sensorCode, sensorName, sensorValue, sensorMeasureUnit, alarmState);
        }

        #endregion
    }
}
