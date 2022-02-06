using DMS.Module.Management.Managers;
using DMS.Module.Management.ViewModel;
using Prism.Events;
using System.Windows.Controls;

namespace DMS.Module.Management.View
{
    /// <summary>
    /// IPCamRecordInquiryView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IPCamRecordInquiryView : UserControl, IIPCamRecordingInquiryManager
    {
        private readonly IEventAggregator _EventAggregator = new EventAggregator();
        public IEventAggregator EventAggregator => _EventAggregator;

        public IPCamRecordInquiryView()
        {
            InitializeComponent();

            DataContext = new IPCamRecordInquiryViewModel() { EventAggregator = EventAggregator };
        }
    }
}
