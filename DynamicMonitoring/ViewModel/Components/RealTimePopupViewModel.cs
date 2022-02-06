using DynamicMonitoring.Common;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model;
using DynamicMonitoring.Utils;
using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DynamicMonitoring.ViewModel
{
    public class RealTimePopupViewModel : DMViewModelBase
    {
        #region Properties
        private string _SensorCode;
        public string SensorCode
        {
            get { return _SensorCode; }
            set
            {
                _SensorCode = value;
                RaisePropertyChanged("SensorCode");
            }
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

        private SensorLimit _MLimit;
        public SensorLimit MLimit
        {
            get { return _MLimit; }
            set
            {
                _MLimit = value;
                RaisePropertyChanged("MLimit");
            }
        }

        private SensorLimit _Limit;
        public SensorLimit Limit
        {
            get { return _Limit; }
            set
            {
                _Limit = value;
                RaisePropertyChanged("Limit");
            }
        }

        private DateTime _MaxXRange;
        public DateTime MaxXRange
        {
            get { return _MaxXRange; }
            set
            {
                _MaxXRange = value;
                MinXRange = value.AddHours(-1);
                RaisePropertyChanged("MaxXRange");
            }
        }

        private DateTime _MinXRange;
        public DateTime MinXRange
        {
            get { return _MinXRange; }
            set
            {
                _MinXRange = value;
                RaisePropertyChanged("MinXRange");
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

        private string _RealTimeContent;
        public string RealTimeContent
        {
            get { return _RealTimeContent; }
            set
            {
                _RealTimeContent = value;
                RaisePropertyChanged("RealTimeContent");
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
        #endregion

        #region Construct
        public RealTimePopupViewModel(string sensorCode)
        {
            SensorCode = sensorCode;

            Init();
        }

        #endregion

        #region Methods
        public void Init()
        {
            DataList.Clear();
            MLimit = null;
            Limit = null;
            MaxXRange = DateTime.Now;

            RaisePropertyChanged(string.Empty);
        }

        public async void Reset()
        {
            _isResetting = true;

            await Task.Run(() =>
            {
                while (_IsPollingTaskRunning)
                {
                }
            });

            Init();
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
                LoadSensorData();
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
        /// 실시간 센서 데이터를 가져온다
        /// </summary>
        private async void LoadSensorData()
        {
            try
            {
                DateTime lastDateTime = DataList == null || DataList.Count == 0 ?
                    DateTime.MinValue : DataList[DataList.Count - 1].DateTime;

                if (_isResetting || SensorCode == null)
                {
                    return;
                }

                List<DataTable> dts = await DBHelper.GetSensorRealtimeData(SensorCode);

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

                    SensorLimit mLimit = null;
                    if (firstRow.Table.Columns.Contains("m_limit_low") && firstRow.Table.Columns.Contains("m_limit_high"))
                    {
                        if (firstRow["m_limit_low"] != DBNull.Value
                            || firstRow["m_limit_high"] != DBNull.Value)
                        {
                            #region mLow
                            double mLow = 0;
                            if (firstRow.Table.Columns.Contains("m_limit_low"))
                            {
                                double.TryParse(firstRow["m_limit_low"].ToString(), out mLow);
                            }
                            #endregion

                            #region mHigh
                            double mHigh = mLow;
                            if (firstRow.Table.Columns.Contains("m_limit_high"))
                            {
                                double.TryParse(firstRow["m_limit_high"].ToString(), out mHigh);
                            }
                            #endregion

                            mLimit = new SensorLimit()
                            {
                                SensorCode = sensorCode,
                                Low = mLow,
                                High = mHigh,
                            };
                        }
                    }

                    SensorLimit limit = null;
                    if (firstRow.Table.Columns.Contains("limit_low") && firstRow.Table.Columns.Contains("limit_high"))
                    {
                        if (firstRow["limit_low"] != DBNull.Value
                            || firstRow["limit_high"] != DBNull.Value)
                        {
                            #region low
                            double low = 0;
                            if (firstRow.Table.Columns.Contains("limit_low"))
                            {
                                double.TryParse(firstRow["limit_low"].ToString(), out low);
                            }
                            #endregion

                            #region high
                            double high = 0;
                            if (firstRow.Table.Columns.Contains("limit_high"))
                            {
                                double.TryParse(firstRow["limit_high"].ToString(), out high);
                            }
                            #endregion

                            limit = new SensorLimit()
                            {
                                SensorCode = sensorCode,
                                Low = low,
                                High = high,
                            };
                        }
                    }

                    MLimit = mLimit;
                    Limit = limit;

                    for (int i = 1; i <= sensorDataTable.Rows.Count; i++)
                    {
                        if (_isResetting)
                        {
                            return;
                        }

                        DataRow row = sensorDataTable.Rows[sensorDataTable.Rows.Count - i];

                        #region dateTime
                        DateTime dateTime = DateTime.MinValue;
                        if (!row.Table.Columns.Contains("x_axis")
                        || !DateTime.TryParse(row["x_axis"].ToString(), out dateTime))
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
                        if (!row.Table.Columns.Contains("y_value")
                        || !double.TryParse(row["y_value"].ToString(), out value))
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

                if (DataList.Count != 0 && DataList.Count == dataList.Count)
                {
                    MaxXRange = dataList[dataList.Count - 1].DateTime;
                }
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
        #endregion

        #region Commands
        public DelegateCommand<UIElement> LoadedCommand
        {
            get { return new DelegateCommand<UIElement>(LoadedCommandExecute); }
        }

        public DelegateCommand UnloadedCommand
        {
            get { return new DelegateCommand(UnloadedCommandExecute); }
        }

        private void LoadedCommandExecute(UIElement sender)
        {
            if (sender.IsVisible)
            {
                _IsLoaded = true;
                StartDataPolling();
            }
        }

        private void UnloadedCommandExecute()
        {
            _IsLoaded = false;
        }
        #endregion
    }
}
