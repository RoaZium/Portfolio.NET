using DynamicMonitoring.Common;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace DynamicMonitoring.ViewModel
{
    public class DisplayPanelComponentViewModel : DMViewModelBase
    {
        private DisplayPanelComponentModel _Model;
        public DisplayPanelComponentModel Model
        {
            get { return _Model; }
            set
            {
                _Model = value;
                RaisePropertyChanged("Model");
            }
        }

        public DisplayPanelComponentViewModel()
        {
        }

        public override void Load()
        {
            Init();

            _IsLoaded = true;
            StartDataPolling();
        }

        public override void Unload()
        {
            _IsLoaded = false;
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

        public void Init()
        {
            Title = null;
            Info = null;

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
                LoadProductionInfo();
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

        private async void LoadProductionInfo()
        {
            try
            {
                if (_isResetting)
                {
                    return;
                }

                DataTable dt = await DBHelper.GetProductionInfo(Model.TargetCode);

                Title = "";
                Info = "";

                if (_isResetting)
                {
                    return;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    #region title1
                    if (row.Table.Columns.Contains("title_1")
                        && row["title_1"] != DBNull.Value)
                    {
                        Title += row["title_1"].ToString();
                    }
                    #endregion

                    #region title2
                    if (row.Table.Columns.Contains("title_2")
                        && row["title_2"] != DBNull.Value)
                    {
                        Title += Environment.NewLine + row["title_2"].ToString();
                    }
                    #endregion

                    #region info1
                    if (row.Table.Columns.Contains("info_1")
                        && row["info_1"] != DBNull.Value)
                    {
                        Info += row["info_1"].ToString();
                    }
                    #endregion

                    #region info2
                    if (row.Table.Columns.Contains("info_2")
                        && row["info_2"] != DBNull.Value)
                    {
                        Info += Environment.NewLine + row["info_2"].ToString();
                    }
                    #endregion

                    #region info3
                    if (row.Table.Columns.Contains("info_3")
                        && row["info_3"] != DBNull.Value)
                    {
                        Info += Environment.NewLine + row["info_3"].ToString();
                    }
                    #endregion

                    #region info4
                    string info4 = null;
                    if (row.Table.Columns.Contains("info_4")
                        && row["info_4"] != DBNull.Value)
                    {
                        info4 += Environment.NewLine + row["info_4"].ToString();
                    }
                    #endregion
                }
            }
            catch { }
        }

        #region 리팩토링 대상

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

        private string _Info;
        public string Info
        {
            get { return _Info; }
            set
            {
                _Info = value;
                RaisePropertyChanged("Info");
            }
        }

        #endregion
    }
}
