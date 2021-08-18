using System.Windows;
using System.Windows.Controls;


namespace PrismWPF.Control
{
    public static class ButtonBaseExtension
    {
        public static readonly DependencyProperty ClickSwitchProperty = DependencyProperty.RegisterAttached("ClickSwitch", typeof(bool), typeof(ButtonBaseExtension),
            new PropertyMetadata(false, OnClickSwitchChanged));

        public static readonly DependencyProperty CommandSwitchProperty = DependencyProperty.RegisterAttached("CommandSwitch", typeof(bool), typeof(ButtonBaseExtension),
            new PropertyMetadata(false, OnCommandSwitchChanged));

        private static void OnClickSwitchChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            Button control = dp as Button;

            if (control == null)
            {
                return;
            }

            if ((bool)(e.NewValue))
            {
                control.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

                SetClickSwitch(dp, false);
            }
        }

        private static void OnCommandSwitchChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            Button control = dp as Button;

            if (control == null)
            {
                return;
            }

            if ((bool)(e.NewValue))
            {
                if (control.Command != null)
                {
                    control.Command.Execute(control);
                }

                SetClickSwitch(dp, false);
            }
        }

        public static void SetClickSwitch(DependencyObject dp, bool value)
        {
            dp.SetValue(ClickSwitchProperty, value);
        }

        public static bool GetClickSwitch(DependencyObject dp)
        {
            return (bool)dp.GetValue(ClickSwitchProperty);
        }

        public static void SetCommandSwitch(DependencyObject dp, bool value)
        {
            dp.SetValue(CommandSwitchProperty, value);
        }

        public static bool GetCommandSwitch(DependencyObject dp)
        {
            return (bool)dp.GetValue(CommandSwitchProperty);
        }
    }
}
