using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Common;
using DynamicMonitoring.Resources;
using System;
using System.Windows;
using DevExpress.Mvvm;
using Microsoft.Win32;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Common.Broadcast;
using DynamicMonitoring.Common.Enums;

namespace DynamicMonitoring.ViewModel
{
    public class ImageComponentPropertyViewModel : DMViewModelBase
    {
        private ImageComponentModel _ImageCPModel;
        public ImageComponentModel ImageCPModel
        {
            get
            {
                if (_ImageCPModel == null)
                {
                    _ImageCPModel = new ImageComponentModel();
                }
                return _ImageCPModel;
            }
            set
            {
                _ImageCPModel = value;
                RaisePropertyChanged("ImageCPModel");
            }
        }

        public override void Load()
        {
            Uri = ImageCPModel.ImagePath;
        }

        public override void Unload()
        {
        }

        private string _uri;
        public string Uri
        {
            get { return _uri; }
            set
            {
                _uri = value;
                RaisePropertyChanged("Uri");
            }
        }

        public ImageComponentPropertyViewModel()
        {
        }

        public bool ConfirmChange()
        {
            if (Uri == null)
            {
                MessageBox.Show(Res.MsgIncorrectInput);
                return false;
            }

            OnConfirmed();

            return true;
        }

        public Action Confirmed { get; set; }

        public void OnConfirmed()
        {
            if (Confirmed != null)
            {
                Confirmed();
            }
        }

        public DelegateCommand SelectCommand
        {
            get { return new DelegateCommand(SelectCommandExecute); }
        }

        private void SelectCommandExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Select a picture";
            openFileDialog.Filter = "Image files (*.png;*.jpeg,*bmp)|*.png;*.jpeg;*bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                ImageCPModel.ImagePath = openFileDialog.FileName;
                //Uri = openFileDialog.FileName;
            }
        }

        public DelegateCommand OKCommand
        {
            get { return new DelegateCommand(OKCommandExecute); }
        }

        public DelegateCommand CancelCommand
        {
            get { return new DelegateCommand(CancelCommandExecute); }
        }

        private void OKCommandExecute()
        {
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_ADD_MAPCOMPONENT, ImageCPModel, ComponentType.IMG);
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, null, null);
        }

        private void CancelCommandExecute()
        {
            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_REMOVE_MAPCOMPONENTPROPERTY, null, null);
        }
    }
}
