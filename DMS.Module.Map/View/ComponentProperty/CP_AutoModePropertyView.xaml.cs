using Coever.Lib.CoPlatform.Core.Network;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Network;
using DMS.Module.Map.ViewModel;
using PrismWPF.Common;
using PrismWPF.Common.Infrastructure;
using System;
using System.Linq;
using System.Windows.Controls;

namespace DMS.Module.Map.View.ComponentProperty
{
    /// <summary>
    /// CP_AutoModePropertyView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CP_AutoModePropertyView : UserControl
    {
        public CP_AutoModePropertyView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var result = CoPlatformWebClient.Instance.GetScenarioList("PMS");

                if (result == null)
                {
                    return;
                }

                BaseSingletonManager.Instance.ScenarioList = result.ToObservableCollection();

                var data = result.FirstOrDefault(p => p.ScenarioCode.ToString() == MapFrameViewModel.Instance.ComponentMst.PropertyM.ItemCode);

                BaseSingletonManager.Instance.SelectedScenario = data;
            }
            catch(Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        private void ComboBox_Scenario_DropDownOpened(object sender, System.EventArgs e)
        {
            try
            {
                var result = CoPlatformWebClient.Instance.GetScenarioList("PMS");

                if (result == null)
                {
                    return;
                }

                BaseSingletonManager.Instance.ScenarioList = result.ToObservableCollection();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        private void ComboBox_Scenario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = ((Coever.Lib.CoPlatform.Scenario.Core.ScenarioModel)((object[])e.AddedItems)[0]).ScenarioCode;

            MapFrameViewModel.Instance.ComponentMst.PropertyM.ItemCode = data.ToString();

            //var resultLinq = MapFrameViewModel.Instance.PMSLayoutVM.ComponentList.FirstOrDefault(p =>
            //{
            //    return (p.DataContext as CP_CommonBaseViewModel).DmComponentM.RoutingCode == MapFrameViewModel.Instance.ComponentMst.RoutingCode;
            //});

            //(resultLinq.DataContext as CP_AutoModeViewModel).IsScenarioMode = false;
            //(resultLinq.DataContext as CP_AutoModeViewModel).Workspace = null;
        }
    }
}
