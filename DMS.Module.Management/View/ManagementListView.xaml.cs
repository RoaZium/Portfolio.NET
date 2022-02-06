using DMS.Module.Management.Managers;
using DMS.Module.Management.ViewModel;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System.Windows;
using System.Windows.Controls;


namespace DMS.Module.Management.View
{
    /// <summary>
    /// ManagementListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ManagementListView : UserControl
    {
        public ManagementListView()
        {
            InitializeComponent();

            DataContext = new ManagementListViewModel();
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (DataContext is DMViewModelBase)
            {
                (DataContext as DMViewModelBase).EventAggregator = this.FindAncestor<IManagementFrameManager>()?.EventAggregator;
            }
        }
    }
}
