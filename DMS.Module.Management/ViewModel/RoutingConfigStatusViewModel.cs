using Coever.Lib.CoPlatform.Core.Network;
using DMS.Module.Management.Model.RestAPI;
using DMS.Module.Management.Network;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Data;

namespace DMS.Module.Management.ViewModel
{
    public class RoutingConfigStatusViewModel : DMViewModelBase
    {
        private object LockLoadSequence = new object();

        private static readonly int LOAD_DATA_TERM_MILIS = 60000;

        private readonly Timer _PollingDataTimer;

        private readonly object lockDataView = new object();

        public RoutingConfigStatusViewModel()
        {
            _RoutingConfigListView = new ListCollectionView(_RoutingConfigList);
            _PollingDataTimer = new Timer(LoadRoutingConfiguration);
        }

        public override void Load()
        {
            _IsLoaded = true;

            _PollingDataTimer.Change(0, LOAD_DATA_TERM_MILIS);
        }

        public override void Unload()
        {
            _IsLoaded = false;

            _PollingDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private readonly List<DmRoutingConfigurationStatusModel> _RoutingConfigList = new List<DmRoutingConfigurationStatusModel>();
        private readonly ListCollectionView _RoutingConfigListView;
        public ListCollectionView RoutingConfigListView => _RoutingConfigListView;

        /// <summary>
        /// View가 로드 되었는지 여부
        /// </summary>
        private bool _IsLoaded = false;

        private void LoadRoutingConfiguration(object state)
        {
            if (Monitor.TryEnter(LockLoadSequence))
            {
                try
                {
                    if (!_IsLoaded)
                    {
                        _PollingDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        return;
                    }

                    List<DmRoutingConfigurationStatusModel> resultAPI = WebRequests.GetRoutingConfigurationStatus(CoPlatformWebClient.Instance);

                    if(resultAPI == null)
                    {
                        return;
                    }

                    ObservableCollection<DmRoutingConfigurationStatusModel> response = resultAPI.ToObservableCollection();

                    if (response != null)
                    {
                        RoutingConfigListView.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            lock (lockDataView)
                            {
                                _RoutingConfigList.Clear();
                                _RoutingConfigList.AddRange(response);
                                RoutingConfigListView.Refresh();
                            }
                        }));
                    }
                }
                catch { }
                finally
                {
                    Monitor.Exit(LockLoadSequence);
                }
            }
        }
    }
}
