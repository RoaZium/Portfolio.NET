using DMS.Module.Management.Managers;
using DMS.Module.Management.ViewModel;
using Prism.Events;
using System.Windows.Controls;

namespace DMS.Module.Management.View
{
    /// <summary>
    /// AlarmScenarioManagementView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AlarmScenarioManagementView : UserControl, IAlarmScenarioManagementManager
    {
        private readonly IEventAggregator _EventAggregator = new EventAggregator();
        public IEventAggregator EventAggregator => _EventAggregator;

        public AlarmScenarioManagementView()
        {
            InitializeComponent();

            DataContext = new AlarmScenarioManagementViewModel() { EventAggregator = EventAggregator };
        }
    }
}
