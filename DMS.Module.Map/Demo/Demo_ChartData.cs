using DMS.Module.Map.Model;
using DMS.Module.Map.Properties;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace DMS.Module.Map.Demo
{
    public class Demo_ChartData
    {
        public ObservableCollection<DmChartDataModel> _chartSeriesList;
        public ObservableCollection<DmChartDataModel> ChartSeriesList
        {
            get
            {
                if (_chartSeriesList == null)
                {
                    _chartSeriesList = new ObservableCollection<DmChartDataModel>();
                }
                return _chartSeriesList;
            }
            set => _chartSeriesList = value;
        }

        private readonly Random _random = new Random();

        /// <summary>
        /// 데이터 그리드
        /// </summary>
        /// <returns></returns>
        public DataTable SetDataGridData()
        {
            DataTable table = new DataTable();

            DataColumn dataColumn = new DataColumn();
            DataColumn column;
            DataRow row;

            column = new DataColumn();
            column.ColumnName = "이상이력";
            table.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "시간";
            table.Columns.Add(column);

            //column = new DataColumn();
            //column.ColumnName = "수량";
            //column.DataType = System.Type.GetType("System.Int32");
            //table.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "내용";
            table.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "담당자";
            table.Columns.Add(column);

            Random rnd = new Random();

            int val = rnd.Next(1, 100);


            for (int i = 0; i <= 10; i++)
            {
                row = table.NewRow();
                row["이상이력"] = rnd.Next(0, 3);
                row["시간"] = DateTime.Now;
                //row["수량"] = rnd.Next(1, 10000);
                row["내용"] = "[SENSOR_NO] 압력 데이터 이상 발생_" + i;
                row["담당자"] = "홍길동";
                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// 상세 정보 컴포넌트
        /// </summary>
        /// <returns></returns>
        public DataTable SetDetailInfoData()
        {
            DataTable table = new DataTable();

            DataColumn dataColumn = new DataColumn();
            DataColumn column;

            column = new DataColumn();
            column.ColumnName = "Column01";
            table.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "Column02";
            table.Columns.Add(column);

            DataRow row1;
            row1 = table.NewRow();
            row1["Column01"] = "설비명";
            row1["Column02"] = "생산 설비";
            table.Rows.Add(row1);

            DataRow row2;
            row2 = table.NewRow();
            row2["Column01"] = "모델No";
            row2["Column02"] = "BD4-0193856";
            table.Rows.Add(row2);

            DataRow row3;
            row3 = table.NewRow();
            row3["Column01"] = "설치일";
            row3["Column02"] = "2020년 09월 21일";
            table.Rows.Add(row3);

            DataRow row4;
            row4 = table.NewRow();
            row4["Column01"] = "규격";
            row4["Column02"] = "-----------";
            table.Rows.Add(row4);

            return table;
        }

        /// <summary>
        /// 선형
        /// </summary>
        public ObservableCollection<DmChartDataModel> SetLineChartData()
        {
            if (ChartSeriesList != null)
            {
                ChartSeriesList = null;
            }

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(1).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(2).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(3).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(4).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(5).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(6).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(7).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(8).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(9).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });

            return ChartSeriesList;
        }

        /// <summary>
        /// 막대형
        /// </summary>
        public ObservableCollection<DmChartDataModel> SetBarChartData()
        {
            if (ChartSeriesList != null)
            {
                ChartSeriesList = null;
            }

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(1).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(2).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(3).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(4).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(5).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(6).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(7).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(8).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = DateTime.Now.AddMinutes(9).ToShortTimeString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "" });

            return ChartSeriesList;
        }

        /// <summary>
        /// 원형
        /// </summary>
        public ObservableCollection<DmChartDataModel> SetPieChartData()
        {
            if (ChartSeriesList != null)
            {
                ChartSeriesList = null;
            }

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-5", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-4", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-3", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-2", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFFE939A" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-1", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFFFFFFF" });

            return ChartSeriesList;
        }

        /// <summary>
        /// 산포도
        /// </summary>
        public ObservableCollection<DmChartDataModel> SetScatterChartData()
        {
            if (ChartSeriesList != null)
            {
                ChartSeriesList = null;
            }

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "1".ToString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "2".ToString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "3".ToString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "4".ToString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "5".ToString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "6".ToString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "7".ToString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "8".ToString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "9".ToString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "10".ToString(), ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });

            return ChartSeriesList;
        }

        /// <summary>
        /// Xbar
        /// </summary>
        public ObservableCollection<DmChartDataModel> SetXbarChartData()
        {
            if (ChartSeriesList != null)
            {
                ChartSeriesList = null;
            }

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = "Data", ArgumentDataMember = "1", ValueDataMember = _random.Next(12, 97).ToString(), ColorDataMember = "Red" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = "Data", ArgumentDataMember = "2", ValueDataMember = _random.Next(12, 97).ToString(), ColorDataMember = "Red" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = "Data", ArgumentDataMember = "3", ValueDataMember = _random.Next(12, 97).ToString(), ColorDataMember = "Red" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = "Data", ArgumentDataMember = "4", ValueDataMember = _random.Next(12, 97).ToString(), ColorDataMember = "Red" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = "Data", ArgumentDataMember = "5", ValueDataMember = _random.Next(12, 97).ToString(), ColorDataMember = "Red" });

            return ChartSeriesList;
        }

        /// <summary>
        /// Pareto
        /// </summary>
        public ObservableCollection<DmChartDataModel> SetParetoChartData01()
        {
            if (ChartSeriesList != null)
            {
                ChartSeriesList = null;
            }

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-5", ValueDataMember = "40", ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-4", ValueDataMember = "45", ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-3", ValueDataMember = "70", ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-2", ValueDataMember = "80", ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-1", ValueDataMember = "90", ColorDataMember = "#FFF5E98F" });

            return ChartSeriesList;
        }

        /// <summary>
        /// Pareto
        /// </summary>
        public ObservableCollection<DmChartDataModel> SetParetoChartData02()
        {
            if (ChartSeriesList != null)
            {
                ChartSeriesList = null;
            }

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-5", ValueDataMember = "100", ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-4", ValueDataMember = "85", ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-3", ValueDataMember = "75", ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-2", ValueDataMember = "70", ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-1", ValueDataMember = "65", ColorDataMember = "#FFF5E98F" });

            return ChartSeriesList;
        }

        /// <summary>
        /// 복합 선형
        /// </summary>
        public ObservableCollection<DmChartDataModel> SetLineSeriesChartData()
        {
            if (ChartSeriesList != null)
            {
                ChartSeriesList = null;
            }

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-5", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-4", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-3", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-2", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-1", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-5", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-4", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-3", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-2", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-1", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-5", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-4", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-3", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-2", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-1", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });

            return ChartSeriesList;
        }

        /// <summary>
        /// 복합 막대형
        /// </summary>
        public ObservableCollection<DmChartDataModel> SetBarSeriesChartData()
        {
            if (ChartSeriesList != null)
            {
                ChartSeriesList = null;
            }

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-5", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-4", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-3", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-2", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-1", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-5", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-4", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-3", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-2", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-1", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-5", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-4", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-3", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-2", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-1", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });

            return ChartSeriesList;
        }

        /// <summary>
        /// 중첩 도넛
        /// </summary>
        public ObservableCollection<DmChartDataModel> SetNestedDonutChartData()
        {
            if (ChartSeriesList != null)
            {
                ChartSeriesList = null;
            }

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-5", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-4", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-3", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-2", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FFF5E98F" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrTemperature, ArgumentDataMember = "D-1", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "Transparent" });

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-5", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-4", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-3", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-2", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF8ED4F7" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrHumidity, ArgumentDataMember = "D-1", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "Transparent" });

            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-5", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-4", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-3", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-2", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "#FF78EC90" });
            ChartSeriesList.Add(new DmChartDataModel { SeriesDataMember = Res.StrDust, ArgumentDataMember = "D-1", ValueDataMember = _random.Next(0, 100).ToString(), ColorDataMember = "Transparent" });

            return ChartSeriesList;
        }

        /// <summary>
        /// 진행 표시 줄
        /// </summary>
        public string SetProgressBarData()
        {
            string data = _random.Next(1, 90).ToString();
            return data;
        }
    }
}
