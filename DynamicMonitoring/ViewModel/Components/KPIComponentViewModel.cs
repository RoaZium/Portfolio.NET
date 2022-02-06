using DynamicMonitoring.Common;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Utils;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DynamicMonitoring.ViewModel
{
    public class KPIComponentViewModel : DMViewModelBase
    {
        private KPIComponentModel _Model;
        public KPIComponentModel Model
        {
            get { return _Model; }
            set
            {
                _Model = value;
                RaisePropertyChanged("Model");
            }
        }

        public KPIComponentViewModel()
        {
        }

        public override void Load()
        {
            DataList.Clear();
            _IsLoaded = true;
            StartDataPolling();
        }

        public override void Unload()
        {
            _IsLoaded = false;
        }

        /// <summary>
        /// Title
        /// </summary>
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                RaisePropertyChanged("Title");
            }
        }

        private Visibility _VisibilityLine;
        public Visibility VisibilityLine
        {
            get { return _VisibilityLine; }
            set
            {
                _VisibilityLine = value;
                RaisePropertyChanged("VisibilityLine");
            }
        }

        private Visibility _VisibilityBar;
        public Visibility VisibilityBar
        {
            get { return _VisibilityBar; }
            set
            {
                _VisibilityBar = value;
                RaisePropertyChanged("VisibilityBar");
            }
        }

        private ObservableCollection<SensorData> _dataList;
        public ObservableCollection<SensorData> DataList
        {
            get
            {
                if (_dataList == null)
                {
                    _dataList = new ObservableCollection<SensorData>();
                }
                return _dataList;
            }
        }

        private double _lastValue;
        public double LastValue
        {
            get { return _lastValue; }
            set
            {
                _lastValue = value;
                RaisePropertyChanged("LastValue");
            }
        }

        /// <summary>
        /// View가 로드 되었는지 여부
        /// </summary>
        private bool _IsLoaded;

        /// <summary>
        /// 데이터 폴링 쓰레드가 돌아가고 있는지 여부
        /// </summary>
        private bool _IsPollingTaskRunning = false;

        /// <summary>
        /// 데이터 폴링 메소드가 호출되는 주기
        /// </summary>
        private int _PollingTermMilis = 30000;

        /// <summary>
		/// 리셋중인지 여부
		/// </summary>
		private bool _isResetting = false;

        public async void Reset()
        {
            _isResetting = true;

            await Task.Run(() =>
            {
                while (_IsPollingTaskRunning)
                {
                }
            });

            DataList.Clear();
            _isResetting = false;
            if (_IsLoaded)
            {
                StartDataPolling();
            }
        }

        /// <summary>
        /// 주기적으로 데이터를 폴링하는 쓰레드를 시작한다.
        /// _IsLoaded가 false가 되거나 _isResetting이 true가 되면 쓰레드가 종료된다.
        /// </summary>
        private async void StartDataPolling()
        {
            if (_IsPollingTaskRunning)
            {
                return;
            }

            _IsPollingTaskRunning = true;

            while (_IsLoaded && !_isResetting)
            {
                LoadKPIData();
                await Task.Run(() =>
                {
                    long endTimeMilis = DateTimeUtils.GetCurrentTimeStampMilis() + _PollingTermMilis;
                    while (endTimeMilis > DateTimeUtils.GetCurrentTimeStampMilis() && _IsLoaded && !_isResetting)
                    {
                        Thread.Sleep(100);
                    }
                });
            }

            _IsPollingTaskRunning = false;
        }

        /// <summary>
		/// KPI 데이터를 가져온다
		/// </summary>
		private async void LoadKPIData()
        {
            try
            {
                DateTime lastDateTime = DataList == null || DataList.Count == 0 ?
                    DateTime.MinValue : DataList[DataList.Count - 1].DateTime;

                if (_isResetting)
                {
                    return;
                }

                List<DataTable> dts = await DBHelper.GetKPIData(Model.TargetCode);

                if (_isResetting)
                {
                    return;
                }

                DataTable titleTable = dts[0];
                DataTable sensorDataTable = dts[1];

                if (titleTable != null && titleTable.Rows.Count > 0)
                {
                    Title = titleTable.Rows[0].ToObject<TitleModel>().TITLE;
                }

                List<SensorData> dataList = new List<SensorData>();

                if (sensorDataTable != null && sensorDataTable.Rows.Count > 0)
                {
                    DataRow firstRow = sensorDataTable.Rows[0];

                    #region sensorCode
                    string sensorCode = null;
                    if (firstRow.Table.Columns.Contains("resource_code")
                        && firstRow["resource_code"] != DBNull.Value)
                    {
                        sensorCode = firstRow["resource_code"].ToString();
                    }
                    #endregion

                    #region sensorName
                    string sensorName = null;
                    if (firstRow.Table.Columns.Contains("resource_name")
                        && firstRow["resource_name"] != DBNull.Value)
                    {
                        sensorName = firstRow["resource_name"].ToString();
                    }
                    #endregion

                    #region measureUnit
                    string measureUnit = null;
                    if (firstRow.Table.Columns.Contains("resource_unit")
                        && firstRow["resource_unit"] != DBNull.Value)
                    {
                        measureUnit = firstRow["resource_unit"].ToString();
                    }
                    #endregion

                    for (int i = 1; i <= sensorDataTable.Rows.Count; i++)
                    {
                        if (_isResetting)
                        {
                            return;
                        }

                        DataRow row = sensorDataTable.Rows[sensorDataTable.Rows.Count - i];

                        #region dateTime
                        DateTime dateTime = DateTime.MinValue;
                        if (!row.Table.Columns.Contains("x")
                        || !DateTime.TryParse(row["x"].ToString(), out dateTime))
                        {
                            continue;
                        }
                        #endregion

                        if (lastDateTime >= dateTime)
                        {
                            break;
                        }

                        #region value
                        double value = 0;
                        if (!row.Table.Columns.Contains("y")
                        || !double.TryParse(row["y"].ToString(), out value))
                        {
                            continue;
                        }
                        #endregion

                        SensorData item = new SensorData()
                        {
                            SensorCode = sensorCode,
                            SensorName = sensorName,
                            DateTime = dateTime,
                            Value = value,
                            MeasureUnit = measureUnit
                        };

                        dataList.Add(item);
                    }
                    dataList.Reverse();
                }

                if (_isResetting)
                {
                    return;
                }

                foreach (var item in dataList)
                {
                    DataList.Add(item);
                }
                SetKPIUI();
            }
            catch { }
        }

        void OnDataListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                SensorData lastItem = e.NewItems[e.NewItems.Count - 1] as SensorData;

                LastValue = lastItem.Value;
            }
        }

        private void SetKPIUI()
        {
            if (Model.GraphType == "Linear")
            {
                VisibilityLine = Visibility.Visible;
                VisibilityBar = Visibility.Hidden;
            }
            else
            {
                VisibilityLine = Visibility.Hidden;
                VisibilityBar = Visibility.Visible;
            }
        }
    }
}
