using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model.RestAPI;
using DMS.Module.Map.Network;
using DMS.Module.Map.View;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_AutoModeViewModel : CP_CommonBaseViewModel
    {
        public override void Load()
        {
        }

        public override void Unload()
        {
            IsScenarioMode = false;
            BaseSingletonManager.Instance.ActionList = null;
        }

        protected override void OnComponentData()
        {
        }

        private ObservableCollection<UserControl> _Workspace;
        public ObservableCollection<UserControl> Workspace
        {
            get
            {
                if (_Workspace == null)
                {
                    _Workspace = new ObservableCollection<UserControl>();
                }
                return _Workspace;
            }
            set
            {
                _Workspace = value;
                RaisePropertyChanged("Workspace");
            }
        }

        private ObservableCollection<DmComponentMst> _layoutList;
        public ObservableCollection<DmComponentMst> LayoutList
        {
            get
            {
                if (_layoutList == null)
                {
                    _layoutList = new ObservableCollection<DmComponentMst>();
                }
                return _layoutList;
            }
            set
            {
                _layoutList = value;
                RaisePropertyChanged("LayoutList");
            }
        }

        private Visibility _lodingVisibility = Visibility.Collapsed;
        public Visibility LoadingVisibility
        {
            get => _lodingVisibility;
            set
            {
                _lodingVisibility = value;
                RaisePropertyChanged("LoadingVisibility");
            }
        }

        private bool _isScenarioMode;
        public bool IsScenarioMode
        {
            get => _isScenarioMode;
            set
            {
                _isScenarioMode = value;
                RaisePropertyChanged("IsScenarioMode");
            }
        }

        private void LoadScenarioList()
        {
            if(BaseSingletonManager.Instance.ActionList != null)
            {
                BaseSingletonManager.Instance.ActionList = null;
            }

            if(DmComponentM.PropertyM.ItemCode == null)
            {
                return;
            }

            var result = CoPlatformWebClient.Instance.GetScenarioInfo(int.Parse(DmComponentM.PropertyM.ItemCode));

            if (result == null)
            {
                return;
            }

            BaseSingletonManager.Instance.ActionList.AddRange(result.ScenarioActions.ConvertAll<ActionPackerModel>(item => new ActionPackerModel(result.ScenarioType, item)));

            foreach (ActionPackerModel actionPacker in BaseSingletonManager.Instance.ActionList)
            {
                DmComponentMst linqResult = LinqManager.FilterFirstRoutingCode(((LayoutActionModel)actionPacker.ActionModel).LayoutID);

                LayoutList.Add(linqResult);

                PMSLayoutView view = new PMSLayoutView();
                (view.DataContext as PMSLayoutViewModel).DmComponentM = linqResult;

                Workspace.Add(view);
            }

            StartAutoMode();
        }

        public void SelectedComponentConfig(DmComponentMst model)
        {
            if (model == null)
            {
                return;
            }

            UserControl uc = Workspace.FirstOrDefault(p => (p.DataContext as PMSLayoutViewModel).DmComponentM == model);

            if (uc == null)
            {
                return;
            }

            foreach (var item in LayoutList)
            {
                if(item == null)
                {
                    continue;
                }

                item.IsVisible = Visibility.Collapsed;
            }

            //foreach(var item in Workspace)
            //{
            //    (item.DataContext as PMSLayoutViewModel).DmComponentM.IsVisible = Visibility.Collapsed;
            //}

            (uc.DataContext as PMSLayoutViewModel).DmComponentM.IsVisible = Visibility.Visible;
        }

        private async void StartAutoMode()
        {
            await Task.Run(() =>
            {
                while (IsScenarioMode)
                {
                    foreach (ActionPackerModel actionPacker in BaseSingletonManager.Instance.ActionList)
                    {
                        DmComponentMst linqResult = LinqManager.FilterFirstRoutingCode(((LayoutActionModel)actionPacker.ActionModel).LayoutID);

                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            SelectedComponentConfig(linqResult);
                        }));

                        int limitCount = (actionPacker.ActionDelaySecond ?? 0) * 1000 / 100;
                        for (int count = 0; count < limitCount; count++)
                        {
                            if(!IsScenarioMode)
                            {
                                return;
                            }

                            Thread.Sleep(100);
                        }
                    }
                }
            });
        }

        public DelegateCommand PlayCommand => new DelegateCommand(PlayCommandExecute);

        private void PlayCommandExecute()
        {
            if (IsScenarioMode == true)
            {
                if (Workspace == null || Workspace.Count == 0)
                {
                    LoadScenarioList();
                }
                else
                {
                    StartAutoMode();
                }
            }
        }
    }
}
