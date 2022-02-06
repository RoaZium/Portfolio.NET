using DMS.Module.Management.Managers;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Management.View
{
    /// <summary>
    /// IPCamRecordPalyerView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IPCamRecordPalyerView : UserControl
    {
        public IPCamRecordPalyerView()
        {
            InitializeComponent();
        }

        public IPCamRecordPalyerView(object dummy)
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
                    (e.NewValue as DMViewModelBase).EventAggregator = this.FindAncestor<IIPCamRecordingInquiryManager>()?.EventAggregator;
                }
            }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (DataContext is DMViewModelBase)
            {
                (DataContext as DMViewModelBase).EventAggregator = this.FindAncestor<IIPCamRecordingInquiryManager>()?.EventAggregator;
            }
        }
    }
}
