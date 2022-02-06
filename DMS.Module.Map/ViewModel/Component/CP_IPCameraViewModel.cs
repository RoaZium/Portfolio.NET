using Coever.Lib.CoPlatform.Scenario.Core;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model.RestAPI;
using DMS.Module.Map.Model.Settings;
using DMS.Module.Map.Properties;
using DynamicMonitoring.View;
using Newtonsoft.Json;
using Prism.Commands;
using PrismWPF.Common.Broadcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_IPCameraViewModel : CP_CommonBaseViewModel
    {
        protected override void OnComponentData()
        {
        }

        public override void Load()
        {
            BroadcastReceiver.RegisterReceiver(ConstantManager.BROADCAST_IPCAMERA_HIDE, OnIPCameraChange);
        }

        public override void Unload()
        {
            IsChecked = false;
            BroadcastReceiver.UnregisterReceiver(ConstantManager.BROADCAST_IPCAMERA_HIDE, OnIPCameraChange);
        }

        #region Private variables
        private readonly double p_ControlLeftPos;
        private readonly double p_ControlTopPos;
        private bool p_IsSettedPopupPos;
        #endregion

        #region Properties

        private bool _IsVisible;
        public bool IsVisible
        {
            get => _IsVisible;
            set
            {
                _IsVisible = value;
                RaisePropertyChanged("IsVisible");

                OnIsVisibleChanged();
            }
        }

        private bool _IsChecked = true;
        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                _IsChecked = value;
                RaisePropertyChanged("IsChecked");

                OnIsCheckedChanged();
            }
        }

        private IPCamPopupWindowView _IPCamPopupWindow;
        public IPCamPopupWindowView IPCamPopupWindow
        {
            get
            {
                if (_IPCamPopupWindow == null)
                {
                    _IPCamPopupWindow = new IPCamPopupWindowView
                    {
                        DataContext = IPCamPopupViewModel
                    };

                    _IPCamPopupWindow.componentID = DmComponentM.RoutingCode;

                    _IPCamPopupWindow.Closed += OnPopupClosed;
                    _IPCamPopupWindow.IsVisibleChanged += OnPopupIsVisibleChanged;
                    p_IsSettedPopupPos = false;

                    Dictionary<string, IPCamPopupState> popupStates
                        = JsonConvert.DeserializeObject<Dictionary<string, IPCamPopupState>>(NoSaveSettings.Default.IPCamPopupStates);

                    if (popupStates != null && DmComponentM.RoutingCode != null)
                    {
                        if (popupStates.ContainsKey(DmComponentM.RoutingCode))
                        {
                            IPCamPopupState thisPopupState = popupStates[DmComponentM.RoutingCode];

                            if (thisPopupState.Width != null)
                            {
                                _IPCamPopupWindow.Width = thisPopupState.Width.Value;
                            }
                            if (thisPopupState.Height != null)
                            {
                                _IPCamPopupWindow.Height = thisPopupState.Height.Value;
                            }
                        }
                    }
                }
                else
                {
                    _IPCamPopupWindow.DataContext = IPCamPopupViewModel;
                }
                return _IPCamPopupWindow;
            }
        }

        private IPCamPopupWindowViewModel _IPCamPopupViewModel;
        public IPCamPopupWindowViewModel IPCamPopupViewModel
        {
            get
            {
                if (_IPCamPopupViewModel == null)
                {
                    if (DmComponentM != null && DmComponentM.PropertyM.TargetCode != null && uint.TryParse(DmComponentM.PropertyM.TargetCode, out uint camCode))
                    {
                        _IPCamPopupViewModel = new IPCamPopupWindowViewModel(camCode, DmComponentM.RoutingCode);
                    }
                    else
                    {
                        _IPCamPopupViewModel = null;
                    }
                }
                return _IPCamPopupViewModel;
            }
            set => _IPCamPopupViewModel = value;
        }

        #endregion

        #region Events
        public DelegateCommand<UIElement> IsVisibleChangedCommand => new DelegateCommand<UIElement>(IsVisibleChangedCommandExecute);

        private void IsVisibleChangedCommandExecute(UIElement sender)
        {
            if (sender.IsVisible)
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
            }
        }

        private void OnIsCheckedChanged()
        {
            if (IsChecked && IsVisible)
            {
                IPCamPopupWindow.Show();
                SetIPCamPopupWindowLocation();
            }
            else
            {
                IPCamPopupWindow.Hide();
            }
        }

        private void SetIPCamPopupWindowLocation()
        {
            var resultLinq = MapFrameViewModel.Instance.PMSLayoutVM.VMList.Where(p =>
            {
                return p.DmComponentM.RoutingType == ComponentType.DM701003.ToString();
            });

            if(resultLinq == null)
            {
                return;
            }

            int resultCount = resultLinq.Count(p =>
            {
                return (p as CP_IPCameraViewModel).IsChecked == true;
            });

            if(resultCount == 0)
            {
                return;
            }

            Point screenPosition = Application.Current.MainWindow.PointToScreen(new Point(0, 0));

            if (resultCount == 1)
            {
                IPCamPopupWindow.Left = screenPosition.X + (255 + ((IPCamPopupWindow.Width + 10) * 0));
                IPCamPopupWindow.Top = 30;
            }
            else if(resultCount == 2)
            {
                IPCamPopupWindow.Left = screenPosition.X + (255 + ((IPCamPopupWindow.Width + 10) * 1));
                IPCamPopupWindow.Top = 30;
            }
            else if (resultCount == 3)
            {
                IPCamPopupWindow.Left = screenPosition.X + (255 + ((IPCamPopupWindow.Width + 10) * 0));
                IPCamPopupWindow.Top = (IPCamPopupWindow.Height + 40) * 1;
            }
            else if (resultCount == 4)
            {
                IPCamPopupWindow.Left = screenPosition.X + (255 + ((IPCamPopupWindow.Width + 10) * 1));
                IPCamPopupWindow.Top = (IPCamPopupWindow.Height + 40) * 1;
            }
            else
            {
                IPCamPopupWindow.Left = Application.Current.MainWindow.Left;
                IPCamPopupWindow.Top = Application.Current.MainWindow.Top;
            }
        }

        private void OnIsVisibleChanged()
        {
            if (IsChecked && IsVisible)
            {
                IPCamPopupWindow.Show();
            }
            else
            {
                IPCamPopupWindow.Hide();
            }
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            _IPCamPopupWindow = null;
            IsChecked = false;
        }

        private void OnPopupIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;

            if (!newValue)
            {
                IPCamPopupWindow.Hide();
            }
            else
            {
                if (!p_IsSettedPopupPos)
                {
                    p_IsSettedPopupPos = true;

                    IPCamPopupWindow.Left = p_ControlLeftPos;
                    IPCamPopupWindow.Top = p_ControlTopPos;

                    Dictionary<string, IPCamPopupState> popupStates
                        = JsonConvert.DeserializeObject<Dictionary<string, IPCamPopupState>>(NoSaveSettings.Default.IPCamPopupStates);

                    if (DmComponentM?.RoutingCode != null && popupStates != null && popupStates.ContainsKey(DmComponentM.RoutingCode))
                    {
                        IPCamPopupState thisPopupState = popupStates[DmComponentM.RoutingCode];

                        if (thisPopupState.Left != null)
                        {
                            IPCamPopupWindow.Left = thisPopupState.Left.Value;
                        }
                        if (thisPopupState.Top != null)
                        {
                            IPCamPopupWindow.Top = thisPopupState.Top.Value;
                        }
                    }
                }
            }
        }

        private void OnIPCameraChange(string actionName, object sender, object e)
        {
            if (DmComponentM.RoutingCode != sender.ToString())
            {
                return;
            }

            IsChecked = false;
        }

        public async Task ExecuteAction(IPCamCompoActionModel action)
        {
            if (action == null)
            {
                return;
            }

            await Task.Factory.StartNew(() =>
            {
                int limitCount = 2000 / 100;
                for (int count = 0; count < limitCount && !IsVisible; count++)
                {
                    Thread.Sleep(100);
                }
            });

            if (IsVisible)
            {
                IsChecked = true;
                await IPCamPopupViewModel?.ExecuteAction(action.AreaName);
            }
        }
        #endregion
    }
}
