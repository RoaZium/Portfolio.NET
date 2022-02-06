using DynamicMonitoring.Common;
using DynamicMonitoring.Common.Model;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Utils;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DynamicMonitoring.ViewModel
{
    public class AlarmComponentPopupViewModel : DMViewModelBase
    {
        /// <summary>
        /// 팝업에 뿌리기 위해 DB에서 불러온 데이터
        /// </summary>
        private DisplayPanelModel _DPModel;
        public DisplayPanelModel DPModel
        {
            get { return _DPModel; }
            set
            {
                _DPModel = value;
                RaisePropertyChanged("DPModel");
            }
        }

        private string _TargetType;
        public string TargetType
        {
            get { return _TargetType; }
            set
            {
                _TargetType = value;
                RaisePropertyChanged("TargetType");
            }
        }

        private string _TargetCode;
        public string TargetCode
        {
            get { return _TargetCode; }
            set
            {
                _TargetCode = value;
                RaisePropertyChanged("TargetCode");
            }
        }

        public AlarmComponentPopupViewModel()
        {
            Init();
        }

        public override void Load()
        {
            LoadAssetInfo();
        }

        public override void Unload()
        {
        }

        #region Methods
        public void Init()
        {
            DPModel = null;

            RaisePropertyChanged(string.Empty);
        }

        public void Reset()
        {
            Init();

            LoadAssetInfo();
        }

        private async void LoadAssetInfo()
        {
            if (TargetType == null || TargetCode == null)
            {
                return;
            }
            try
            {
                DataRow dr = await DBHelper.GetAssetInfo(TargetType, _TargetCode);

                if (dr != null)
                {
                    DPModel = dr.ToObject<DisplayPanelModel>();
                }
            }
            catch { }
        }
        #endregion
    }
}
