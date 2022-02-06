using DMS.Module.Management.Managers;
using DMS.Module.Management.ViewModel;
using Prism.Events;
using System.Windows.Controls;

namespace DMS.Module.Management.View
{
    /// <summary>
    /// ScenarioManagementView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ScenarioManagementView : UserControl, IScenarioManagementManager
    {
        private readonly IEventAggregator _EventAggregator = new EventAggregator();
        public IEventAggregator EventAggregator => _EventAggregator;

        public ScenarioManagementView()
        {
            InitializeComponent();

            DataContext = new ScenarioManagementViewModel() { EventAggregator = EventAggregator };
        }
    }
}
