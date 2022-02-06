
using DMS.Module.EnvironmentSetting.Infrastructure;
using DMS.Module.EnvironmentSetting.View;
using Prism.Commands;
using PrismWPF.Common.MVVM;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.EnvironmentSetting.ViewModel
{
    public class EnvSettingFrameViewModel : DMViewModelBase
    {
        public EnvSettingFrameViewModel()
        {
        }

        public override void Load()
        {
            BasicSettingCommandExecute();
        }

        public override void Unload()
        {
            EnvSettingV = null;
            BasicV = null;
        }

        public EnvSettingViewType envSettingViewType;

        private UserControl _EnvSettingV;
        public UserControl EnvSettingV
        {
            get
            {
                if (_EnvSettingV == null)
                {
                    _EnvSettingV = new UserControl();
                }
                return _EnvSettingV;
            }
            set
            {
                _EnvSettingV = value;
                RaisePropertyChanged();
            }
        }

        private EnvSetting_BasicView _BasicV;
        public EnvSetting_BasicView BasicV
        {
            get
            {
                if (_BasicV == null)
                {
                    _BasicV = new EnvSetting_BasicView();
                }
                return _BasicV;
            }
            set
            {
                _BasicV = value;
                RaisePropertyChanged("BasicV");
            }
        }


        public DelegateCommand BasicSettingCommand => new DelegateCommand(BasicSettingCommandExecute);

        private void BasicSettingCommandExecute()
        {
            EnvSettingV = BasicV;
            envSettingViewType = EnvSettingViewType.Basic;
        }

        public DelegateCommand<Window> OKCommand => new DelegateCommand<Window>(OKCommandExecute);

        private void OKCommandExecute(Window sender)
        {
            try
            {
                (BasicV.DataContext as EnvSetting_BasicViewModel).SaveSystemTitle();

                sender.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public DelegateCommand<Window> CloseCommand => new DelegateCommand<Window>(CloseCommandExecute);

        private void CloseCommandExecute(Window sender)
        {
            sender.Close();
        }
    }
}
