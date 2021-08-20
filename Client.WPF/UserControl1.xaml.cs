using Client.WPF.ViewModel;
using System.Windows.Controls;

namespace Client.WPF
{
	/// <summary>
	/// UserControl1.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class UserControl1 : UserControl
	{
		private UserControlViewModel VM { get; set; }

		public UserControl1()
		{
			InitializeComponent();

			VM = new UserControlViewModel();
			DataContext = VM;
		}
	}
}
