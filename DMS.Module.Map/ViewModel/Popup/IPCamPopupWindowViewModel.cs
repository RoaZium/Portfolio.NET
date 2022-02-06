using AxisVideoTransformLib;
using Coever.Lib.Axis.Core;
using Coever.Lib.CoPlatform.Core.Models;
using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.IPCam.Core.Model;
using Coever.Lib.WPF.Core.Tasks;
using DMS.Module.Map.Model.Settings;
using DMS.Module.Map.Network;
using DMS.Module.Map.Properties;
using Newtonsoft.Json;
using Prism.Commands;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Map.ViewModel
{
    public class IPCamPopupWindowViewModel : DMViewModelBase
    {
        #region Private variables
        private readonly uint _IPCamCode;

        public string _CompoID;

        private readonly AsyncTask<string, DmIpcamFull> _loadTask;
        #endregion

        #region Properties
        /// <summary>
		/// Title
		/// </summary>
        private string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                RaisePropertyChanged("Title");
            }
        }

        /// <summary>
		/// IPCamInfo
		/// </summary>
        private DmIpcamFull _IPCamInfo;
        public DmIpcamFull IPCamInfo
        {
            get => _IPCamInfo;
            set
            {
                DmIpcamFull origin = _IPCamInfo;

                _IPCamInfo = value;
                RaisePropertyChanged("IPCamInfo");

                Title = _IPCamInfo?.Mst?.Name;
                if (_IPCamInfo?.Mst == null)
                {
                    IPCamViewModel = null;
                }
                else if (origin?.Mst == null
                  || origin.Mst.Address != _IPCamInfo.Mst.Address
                  || origin.Mst.Account != _IPCamInfo.Mst.Account
                  || origin.Mst.Pwd != _IPCamInfo.Mst.Pwd
                  || origin.Mst.Type != _IPCamInfo.Mst.Type)
                {
                    switch (_IPCamInfo.Mst.Type)
                    {
                        case nameof(CameraTypes.PANO_360):
                            {
                                Enum.TryParse<TiltOrientation>(_IPCamInfo.Mst.Orientation, out TiltOrientation orientation);
                                IPCamViewModel
                                    = new IPCam_PANO_360_ViewModel(
                                        _IPCamInfo.Mst.Address, _IPCamInfo.Mst.Account,
                                        _IPCamInfo.Mst.Pwd,
                                        orientation);
                                break;
                            }
                        case nameof(CameraTypes.FLAT):
                            {
                                IPCamViewModel
                                    = new IPCam_FLAT_ViewModel(
                                        _IPCamInfo.Mst.Address, _IPCamInfo.Mst.Account,
                                        _IPCamInfo.Mst.Pwd);
                                break;
                            }
                        case nameof(CameraTypes.PTZ):
                            {
                                IPCamViewModel
                                    = new IPCam_PTZ_ViewModel(
                                        _IPCamInfo.Mst.Address, _IPCamInfo.Mst.Account,
                                        _IPCamInfo.Mst.Pwd);
                                break;
                            }
                        default:
                            {
                                IPCamViewModel = null;
                                break;
                            }
                    }
                }
                else if (_IPCamInfo?.Mst?.Type == nameof(CameraTypes.PANO_360)
                  && origin?.Mst?.Orientation != _IPCamInfo?.Mst?.Orientation)
                {
                    if (IPCamViewModel is IPCam_PANO_360_ViewModel)
                    {
                        Enum.TryParse<TiltOrientation>(_IPCamInfo.Mst.Orientation, out TiltOrientation orientation);
                        (IPCamViewModel as IPCam_PANO_360_ViewModel).Orientation
                            = orientation;
                    }
                }
            }
        }

        /// <summary>
		/// IPCamViewModel
		/// </summary>
        private IPCamViewModelBase _IPCamViewModel;
        public IPCamViewModelBase IPCamViewModel
        {
            get => _IPCamViewModel;
            set
            {
                _IPCamViewModel = value;
                RaisePropertyChanged("IPCamViewModel");
            }
        }
        #endregion

        #region Construct
        public IPCamPopupWindowViewModel(uint ipcamCode, string compoID)
        {
            _IPCamCode = ipcamCode;
            _CompoID = compoID;

            _loadTask = new AsyncTask<string, DmIpcamFull>(
                null,
                (task, inputs) =>
                {
                    DmIpcamFull result = CoPlatformWebClient.Instance.GetIPCamFull(_IPCamCode);

                    return task.Status == AsyncTaskStatus.RUNNING ? result : null;
                },
                (task, result, inputs) =>
                {
                    IPCamInfo = result;
                });
        }
        #endregion

        #region Methods
        public async Task ExecuteAction(string areaName)
        {
            if (areaName == null)
            {
                return;
            }

            if (IPCamInfo?.Areas == null || IPCamInfo.Areas.Count == 0)
            {
                await Task.Factory.StartNew(() =>
                {
                    while (_loadTask.Status == AsyncTaskStatus.RUNNING)
                    {
                        Thread.Sleep(100);
                    }
                });
            }

            if (IPCamInfo == null)
            {
                return;
            }

            DmIpcamArea area = IPCamInfo?.Areas?.Find(item => item != null && item.Name == areaName);

            if (area != null)
            {
                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        int limitCount = 5000 / 100;
                        for (int count = 0; count < limitCount && !IPCamViewModel.IsConnected; count++)
                        {
                            Thread.Sleep(100);
                        }
                        IIPCamAreaModel iArea = null;
                        switch (IPCamInfo.Mst.Type)
                        {
                            case nameof(CameraTypes.FLAT):
                                {
                                    iArea = JsonConvert.DeserializeObject<IPCam_FLAT_AreaModel>(area.AreaInfJs);
                                    break;
                                }
                            case nameof(CameraTypes.PANO_360):
                                {
                                    iArea = JsonConvert.DeserializeObject<IPCam_PAN_360_AreaModel>(area.AreaInfJs);
                                    break;
                                }
                            case nameof(CameraTypes.PTZ):
                                {
                                    iArea = JsonConvert.DeserializeObject<IPCam_PTZ_AreaModel>(area.AreaInfJs);
                                    break;
                                }
                        }
                        IPCamViewModel?.SelectArea(iArea);
                    }
                    catch { }
                });
            }
        }
        #endregion

        #region Events, Commands
        public override void Load()
        {

        }

        public override void Unload()
        {
        }

        public DelegateCommand<UIElement> IsVisibleChangedCommand => new DelegateCommand<UIElement>(IsVisibleChangedCommandExecute);

        private void IsVisibleChangedCommandExecute(UIElement sender)
        {
            if (sender.IsVisible)
            {
                _loadTask.execute();
            }
            else
            {
                _loadTask.cancel();
            }
        }

        public DelegateCommand<SelectionChangedEventArgs> AreaSelectionChangedCommand => new DelegateCommand<SelectionChangedEventArgs>(AreaSelectionChangedCommandExecute);

        private void AreaSelectionChangedCommandExecute(SelectionChangedEventArgs e)
        {
            ComboBox source = e.Source as ComboBox;
            try
            {
                DmIpcamArea selectedArea = source.SelectedItem as DmIpcamArea;

                IIPCamAreaModel area = null;
                switch (_IPCamInfo.Mst.Type)
                {
                    case nameof(CameraTypes.PANO_360):
                        {
                            area = JsonConvert.DeserializeObject<IPCam_PAN_360_AreaModel>(selectedArea.AreaInfJs);
                            break;
                        }
                    case nameof(CameraTypes.FLAT):
                        {
                            area = JsonConvert.DeserializeObject<IPCam_FLAT_AreaModel>(selectedArea.AreaInfJs);
                            break;
                        }
                    case nameof(CameraTypes.PTZ):
                        {
                            area = JsonConvert.DeserializeObject<IPCam_PTZ_AreaModel>(selectedArea.AreaInfJs);
                            break;
                        }
                }

                if (area != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        IPCamViewModel.SelectArea(area);
                    });
                }
            }
            catch { }
            source.SelectedItem = null;
        }

        public DelegateCommand<SizeChangedEventArgs> WindowSizeChangedCommand => new DelegateCommand<SizeChangedEventArgs>(WindowSizeChangedCommandExecute);

        private void WindowSizeChangedCommandExecute(SizeChangedEventArgs e)
        {
            if (_CompoID != null)
            {
                Dictionary<string, IPCamPopupState> popupStates
                    = JsonConvert.DeserializeObject<Dictionary<string, IPCamPopupState>>(NoSaveSettings.Default.IPCamPopupStates);

                if (popupStates == null)
                {
                    popupStates = new Dictionary<string, IPCamPopupState>();
                }

                IPCamPopupState thisPopupState = null;

                if (popupStates.ContainsKey(_CompoID))
                {
                    thisPopupState = popupStates[_CompoID];
                }
                else
                {
                    thisPopupState = new IPCamPopupState()
                    {
                        Code = _CompoID,
                    };
                }

                thisPopupState.Width = e.NewSize.Width;
                thisPopupState.Height = e.NewSize.Height;

                popupStates[_CompoID] = thisPopupState;

                NoSaveSettings.Default.IPCamPopupStates = JsonConvert.SerializeObject(popupStates);
            }
        }

        public DelegateCommand<Window> WindowLocationChangedCommand => new DelegateCommand<Window>(WindowLocationChangedCommandExecute);

        private void WindowLocationChangedCommandExecute(Window window)
        {
            if (_CompoID != null)
            {
                Dictionary<string, IPCamPopupState> popupStates
                    = JsonConvert.DeserializeObject<Dictionary<string, IPCamPopupState>>(NoSaveSettings.Default.IPCamPopupStates);

                if (popupStates == null)
                {
                    popupStates = new Dictionary<string, IPCamPopupState>();
                }

                IPCamPopupState thisPopupState = null;

                if (popupStates.ContainsKey(_CompoID))
                {
                    thisPopupState = popupStates[_CompoID];
                }
                else
                {
                    thisPopupState = new IPCamPopupState()
                    {
                        Code = _CompoID,
                    };
                }

                thisPopupState.Left = window.Left;
                thisPopupState.Top = window.Top;

                popupStates[_CompoID] = thisPopupState;

                NoSaveSettings.Default.IPCamPopupStates = JsonConvert.SerializeObject(popupStates);
            }
        }
        #endregion
    }
}
