using System.Windows.Controls;

namespace DMS.Module.Management.View
{
    /// <summary>
    /// RecordAlarmItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RecordAlarmItem : UserControl
    {
        public RecordAlarmItem()
        {
            InitializeComponent();
        }

        private void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            c_popup.IsOpen = true;
        }

        private void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            c_popup.IsOpen = false;
        }
    }
}
