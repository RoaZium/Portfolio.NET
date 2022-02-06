using DMS.Module.Map.Managers;
using DMS.Module.Map.ViewModel;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Map.View
{
    /// <summary>
    /// ScenarioView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ScenarioView : UserControl
    {
        public ScenarioView()
        {
            InitializeComponent();

            DataContext = new ScenarioViewModel();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ScenarioViewModel)?.Load();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ScenarioViewModel)?.Unload();
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
                    Prism.Events.IEventAggregator eventAggregator = this.FindAncestor<IMonitoringManager>()?.EventAggregator;

                    if (eventAggregator != null)
                    {
                        (e.NewValue as DMViewModelBase).EventAggregator = eventAggregator;
                    }
                }
            }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (DataContext is DMViewModelBase)
            {
                Prism.Events.IEventAggregator eventAggregator = this.FindAncestor<IMonitoringManager>()?.EventAggregator;

                if (eventAggregator != null)
                {
                    (DataContext as DMViewModelBase).EventAggregator = eventAggregator;
                }
            }
        }
    }
}
