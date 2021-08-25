﻿namespace PrismWPF.Common.Drawing
{
    public static class ColorPalette_Pear36
    {
        public static string[] Colors = new string[]
        {
            "#5e315b",
            "#8c3f5d",
            "#ba6156",
            "#f2a65e",
            "#ffe478",
            "#cfff70",
            "#8fde5d",
            "#3ca370",
            "#3d6e70",
            "#323e4f",
            "#322947",
            "#473b78",
            "#4b5bab",
            "#4da6ff",
            "#66ffe3",
            "#ffffeb",
            "#c2c2d1",
            "#7e7e8f",
            "#606070",
            "#43434f",
            "#272736",
            "#3e2347",
            "#57294b",
            "#964253",
            "#e36956",
            "#ffb570",
            "#ff9166",
            "#eb564b",
            "#b0305c",
            "#73275c",
            "#422445",
            "#5a265e",
            "#80366b",
            "#bd4882",
            "#ff6b97",
            "#ffb5b5"
        };

        public static string GetSomeColor(object token)
        {
            if (token == null)
            {
                return "#00000000";
            }

            string s = token.ToString();

            int index = 0;
            for (int i = 0; i < s.Length; i++)
            {
                index = (s[i] + index) % Colors.Length;
            }

            return Colors[index];
        }
    }
}
