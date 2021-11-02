using System;
using System.Linq;
using System.Windows;

namespace Library.WPF.Managers
{
	public class ThemeManager
	{
		public ThemeManager()
		{

		}

        public void SetThemeType(ThemeType themetype)
        {
            var styleDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(p => p.Source.ToString().Contains(ConstantManager.styleDictionary));

            if (styleDictionary == null)
            {
                return;
            }

            styleDictionary.MergedDictionaries[0].MergedDictionaries.Clear();

            if (themetype == ThemeType.Black)
            {
                styleDictionary.MergedDictionaries[0].MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(ConstantManager.blackURI, UriKind.RelativeOrAbsolute)
                });
            }
            else if (themetype == ThemeType.Blue)
            {
                styleDictionary.MergedDictionaries[0].MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(ConstantManager.blueURI, UriKind.RelativeOrAbsolute)
                });
            }
            else
            {
                styleDictionary.MergedDictionaries[0].MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(ConstantManager.whiteURI, UriKind.RelativeOrAbsolute)
                });
            }
        }
    }
}
