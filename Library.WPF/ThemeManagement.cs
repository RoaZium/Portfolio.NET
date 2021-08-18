using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrismWPF.Control
{
    public class ThemeManagement
    {
        private const string blackURI = @"/PrismWPF.Control;Component/Theme/Black/ColorDictionary.xaml";
        private const string blueURI = @"/PrismWPF.Control;Component/Theme/Blue/ColorDictionary.xaml";
        private const string whiteURI = @"/PrismWPF.Control;Component/Theme/White/ColorDictionary.xaml";

        private const string prismWPFStyle = "PrismWPFStyleDictionary";

        public ThemeManagement()
        {
        }

        public void SetThemeType(ThemeType themetype)
        {
            var prismWPFStyleDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(p => p.Source.ToString().Contains(prismWPFStyle));

            if(prismWPFStyleDictionary == null)
            {
                return;
            }

            prismWPFStyleDictionary.MergedDictionaries[0].MergedDictionaries.Clear();

            if (themetype == ThemeType.Black)
            {
                prismWPFStyleDictionary.MergedDictionaries[0].MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(blackURI, UriKind.RelativeOrAbsolute)
                });
            }
            else if (themetype == ThemeType.Blue)
            {
                prismWPFStyleDictionary.MergedDictionaries[0].MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(blueURI, UriKind.RelativeOrAbsolute)
                });
            }
            else
            {
                prismWPFStyleDictionary.MergedDictionaries[0].MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(whiteURI, UriKind.RelativeOrAbsolute)
                });
            }
        }
    }

    public enum ThemeType
    {
        Black,
        Blue,
        White
    }
}
