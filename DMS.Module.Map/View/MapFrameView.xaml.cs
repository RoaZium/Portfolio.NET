using DMS.Module.Map.Managers;
using DMS.Module.Map.ViewModel;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DMS.Module.Map.View
{
    /// <summary>
    /// MapFrameView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MapFrameView : UserControl, IMonitoringManager
    {
        private readonly IEventAggregator _EventAggregator = new EventAggregator();
        public IEventAggregator EventAggregator => _EventAggregator;

        public MapFrameViewModel VM { get; private set; }

        public MapFrameView()
        {
            InitializeComponent();

            MapFrameViewModel.Instance.EventAggregator = _EventAggregator;

            VM = MapFrameViewModel.Instance;
            DataContext = VM;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM == null)
            {
                return;
            }

            VM.Load();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (VM == null)
            {
                return;
            }

            VM.Unload();
        }

        #region Animation

        private void btnLeftMenuHide_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void btnLeftMenuShow_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbShowLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        //private void btnTopMenuHide_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowHideMenu("sbHideTopMenu", btnTopMenuHide, btnTopMenuShow, pnlTopMenu);
        //}

        //private void btnTopMenuShow_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowHideMenu("sbShowTopMenu", btnTopMenuHide, btnTopMenuShow, pnlTopMenu);
        //}

        private void btnRightMenuHide_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbHideRightMenu", btnRightMenuHide, btnRightMenuShow, pnlRightMenu);
        }

        private void btnRightMenuShow_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbShowRightMenu", btnRightMenuHide, btnRightMenuShow, pnlRightMenu);
        }

        private void btnBottomMenuHide_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbHideBottomMenu", btnBottomMenuHide, btnBottomMenuShow, pnlBottomMenu);
        }

        private void btnBottomMenuShow_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbShowBottomMenu", btnBottomMenuHide, btnBottomMenuShow, pnlBottomMenu);
        }

        private void ShowHideMenu(string Storyboard, Button btnHide, Button btnShow, StackPanel pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);

            if (Storyboard.Contains("Show"))
            {
                btnHide.Visibility = Visibility.Visible;
                btnShow.Visibility = Visibility.Hidden;
            }
            else if (Storyboard.Contains("Hide"))
            {
                btnHide.Visibility = Visibility.Hidden;
                btnShow.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}
