using DMS.Module.Management.Managers;
using DMS.Module.Management.ViewModel;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Management.View
{
    /// <summary>
    /// ManagementFrameView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ManagementFrameView : UserControl, IManagementFrameManager
    {
        private readonly IEventAggregator _EventAggregator = new EventAggregator();
        public IEventAggregator EventAggregator => _EventAggregator;

        //private bool _IsShowMenu = true;
        //public bool IsShowMenu
        //{
        //    get { return _IsShowMenu; }
        //    set
        //    {
        //        _IsShowMenu = value;

        //        if(_IsShowMenu == true)
        //        {
        //            xManageListV.Visibility = Visibility.Visible;
        //        }
        //        else
        //        {
        //            xManageListV.Visibility = Visibility.Collapsed;
        //        }
        //    }
        //}

        public ManagementFrameView()
        {
            InitializeComponent();

            DataContext = new ManagementFrameViewModel() { EventAggregator = EventAggregator };
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
