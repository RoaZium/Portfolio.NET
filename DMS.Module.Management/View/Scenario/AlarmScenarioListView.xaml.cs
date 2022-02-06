using DMS.Module.Management.Managers;
using DMS.Module.Management.ViewModel;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Management.View
{
    /// <summary>
    /// AlarmScenarioListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AlarmScenarioListView : UserControl
    {
        public AlarmScenarioListView()
        {
            InitializeComponent();

            DataContext = new AlarmScenarioListViewModel();
        }

        public AlarmScenarioListView(object dummy)
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == "DataContext")
            {
                if (e.OldValue is DMViewModelBase)
                {
                    (e.OldValue as DMViewModelBase).EventAggregator = null;
                }

                if (e.NewValue is DMViewModelBase)
                {
                    (e.NewValue as DMViewModelBase).EventAggregator = this.FindAncestor<IAlarmScenarioManagementManager>()?.EventAggregator;
                }
            }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (DataContext is DMViewModelBase)
            {
                (DataContext as DMViewModelBase).EventAggregator = this.FindAncestor<IAlarmScenarioManagementManager>()?.EventAggregator;
            }
        }
    }
}
