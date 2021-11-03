using Library.WPF.Managers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Portfolio.WPF
{
	/// <summary>
	/// App.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
            SetBaseTheme();
		}

		private void SetBaseTheme()
		{
            ThemeType themeType = ThemeType.Black;

            //switch (Settings.Default.Theme)
            //{
            //    case "Black":
            //        {
            //            themeType = ThemeType.Black;
            //            break;
            //        }
            //    case "Blue":
            //        {
            //            themeType = ThemeType.Blue;
            //            break;
            //        }
            //    case "White":
            //        {
            //            themeType = ThemeType.White;
            //            break;
            //        }
            //    default:
            //        {
            //            themeType = ThemeType.Black;
            //            Settings.Default.Theme = themeType.ToString();
            //            break;
            //        }
            //}

            ThemeManager themeManager = new ThemeManager();
            themeManager.SetThemeType(themeType);
        }
	}
}
