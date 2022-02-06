
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DynamicMonitoring.Common.Broadcast;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Common;
using System.Windows.Media;
using DynamicMonitoring.Resources;
using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using DynamicMonitoring.View;
using DynamicMonitoring.Common.Enums;
using DynamicMonitoring.Model.Component;

namespace DynamicMonitoring.ViewModel
{
    public class ComponentPropertyFrameViewModel : DMViewModelBase
    {
        /// <summary>
        /// TargetComponet View
        /// </summary>
        private UserControl _TargetComponents;
        public UserControl TargetComponents
        {
            get
            {
                if (_TargetComponents == null)
                {
                    _TargetComponents = new UserControl();
                }
                return _TargetComponents;
            }
            set
            {
                _TargetComponents = value;
                RaisePropertyChanged("TargetComponents");
            }
        }

        public ComponentPropertyFrameViewModel()
        {
        }

        public override void Load()
        {
            LocalizationManager.Instance.CultureChanged += OnCultureChanged;

            BroadcastReceiver.RegisterReceiver(Constants.BROADCAST_ADD_MAPCOMPONENTPROPERTY, OnAddMapComponentProperty);
            BroadcastReceiver.RegisterReceiver(Constants.BROADCAST_MODIFY_MAPCOMPONENTPROPERTY, OnModifyMapComponentProperty);
            BroadcastReceiver.RegisterReceiver(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, OnRemoveMapComponentProperty);
        }

        public override void Unload()
        {
            TargetComponents = null;

            BroadcastReceiver.UnregisterReceiver(Constants.BROADCAST_ADD_MAPCOMPONENTPROPERTY, OnAddMapComponentProperty);
            BroadcastReceiver.UnregisterReceiver(Constants.BROADCAST_MODIFY_MAPCOMPONENTPROPERTY, OnModifyMapComponentProperty);
            BroadcastReceiver.UnregisterReceiver(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, OnRemoveMapComponentProperty);
        }

        private void OnAddMapComponentProperty(string actionName, object sender, object e)
        {
            var type = sender as Nullable<ComponentType>;
            switch (type)
            {
                case ComponentType.ALM:
                    {
                        TargetComponents = new AlarmComponentPropertyView();
                        break;
                    }
                case ComponentType.IMG:
                    {
                        TargetComponents = new ImageComponentPropertyView();
                        break;
                    }
                case ComponentType.KPI:
                    {
                        TargetComponents = new KPIComponentPropertyView();
                        break;
                    }
                case ComponentType.LNC:
                    {
                        TargetComponents = new LineChartComponentPropertyView();
                        break;
                    }
                case ComponentType.DPN:
                    {
                        TargetComponents = new DisplayPanelComponentPropertyView();
                        break;
                    }
            }
        }

        private void OnModifyMapComponentProperty(string actionName, object sender, object e)
        {
            var type = e as Nullable<ComponentType>;
            switch (type)
            {
                case ComponentType.ALM:
                    {
                        TargetComponents = new AlarmComponentPropertyView();
                        (TargetComponents.DataContext as AlarmComponentPropertyViewModel).AlarmModel = sender as AlarmComponentModel;
                        break;
                    }
                case ComponentType.IMG:
                    {
                        TargetComponents = new ImageComponentPropertyView();
                        (TargetComponents.DataContext as ImageComponentPropertyViewModel).ImageCPModel = sender as ImageComponentModel;
                        break;
                    }
                case ComponentType.KPI:
                    {
                        TargetComponents = new KPIComponentPropertyView();
                        (TargetComponents.DataContext as KPIComponentPropertyViewModel).KPICPModel = sender as KPIComponentModel;
                        break;
                    }
                case ComponentType.LNC:
                    {
                        TargetComponents = new LineChartComponentPropertyView();
                        (TargetComponents.DataContext as LineChartComponentPropertyViewModel).LineCPModel = sender as LineChartComponentModel;
                        break;
                    }
                case ComponentType.DPN:
                    {
                        TargetComponents = new DisplayPanelComponentPropertyView();
                        (TargetComponents.DataContext as DisplayPanelComponentPropertyViewModel).DisplayPanelCPModel = sender as DisplayPanelComponentModel;
                        break;
                    }
            }
        }

        private void OnRemoveMapComponentProperty(string actionName, object sender, object e)
        {
            TargetComponents = null;
        }

        public DelegateCommand OKBtnCommand
        {
            get { return new DelegateCommand(OKBtnCommandExecute); }
        }

        public DelegateCommand CancelBtnCommand
        {
            get { return new DelegateCommand(CancelBtnCommandExecute); }
        }

        private void OKBtnCommandExecute()
        {
        }

        private void CancelBtnCommandExecute()
        {
        }




        #region 리팩토링 대상

        public ImageSource Glyph { get { return ObjectConverter.ToImageSource(Res.ImgAbnormal); } set { } }

        public string Name { get { return Res.StrComponentProperties; } set { } }

        #endregion
    }
}
