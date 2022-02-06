using DevExpress.Xpf.Docking;
using DynamicMonitoring.Common;
using DynamicMonitoring.Common.Broadcast;
using DynamicMonitoring.Common.Enums;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model;
using DynamicMonitoring.Resources;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DynamicMonitoring.ViewModel
{
    public class MapToolBarViewModel : DMViewModelBase
    {
        public MapToolBarViewModel()
        {
            LocalizationManager.Instance.CultureChanged += OnCultureChanged;
        }

        public override void Load()
        {
            BroadcastReceiver.RegisterReceiver(Constants.MAP1, OnToolbarVisibility);
            BroadcastReceiver.RegisterReceiver(Constants.MAP5, OnEditModeChanged);
        }

        public override void Unload()
        {
            BroadcastReceiver.UnregisterReceiver(Constants.MAP1, OnToolbarVisibility);
            BroadcastReceiver.UnregisterReceiver(Constants.MAP5, OnEditModeChanged);
        }

        public override string DisplayName { get { return "T"; } }

        public string MapName
        {
            get
            {
                if (IsEditMode == true)
                    return Res.StrMapModification;
                else
                    return Res.StrMapObservation;
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                _isEditMode = value;
                BroadcastReceiver.SendBroadcast(Constants.MAP2, IsEditMode, null);
                RaisePropertyChanged("IsEditMode");
                RaisePropertyChanged("MapName");
            }
        }

        public void OnCultureChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged("MapName");
        }

        private void OnToolbarVisibility(string actionName, object sender, object e)
        {
            IsEditMode = (bool)e;
        }

        private void OnEditModeChanged(string actionName, object sender, object e)
        {
            IsEditMode = (bool)e;

            BroadcastReceiver.SendBroadcast(Constants.MAP2, IsEditMode, null);
        }

        public DelegateCommand<Nullable<ComponentType>> AddComponentCommand
        {
            get { return new DelegateCommand<Nullable<ComponentType>>(AddComponentCommandExecute); }
        }

        /// <summary>
        /// MapLayout Save Command
        /// </summary>
        public DelegateCommand SaveComponentCommand
        {
            get { return new DelegateCommand(SaveComponentCommandExecute); }
        }

        /// <summary>
        /// MapEdit Command
        /// </summary>
        public DelegateCommand MapEditCommand
        {
            get { return new DelegateCommand(MapEditCommandExecute); }
        }

        private void AddComponentCommandExecute(ComponentType? obj)
        {
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_ADD_MAPCOMPONENTPROPERTY, obj, null);
        }

        private void SaveComponentCommandExecute()
        {
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_SAVE_MAPLAYOUT, null, null);
        }

        private void MapEditCommandExecute()
        {
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_EDIT_MAP, IsEditMode, null);
        }
    }
}
