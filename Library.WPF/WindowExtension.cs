
using System.Windows;

namespace PrismWPF.Control
{
    public static class WindowExtension
    {
        public static readonly DependencyProperty ShowSwitchProperty = DependencyProperty.RegisterAttached("ShowSwitch", typeof(bool), typeof(WindowExtension),
            new PropertyMetadata(false, OnShowSwitchChanged));

        public static readonly DependencyProperty ShowDialogSwitchProperty = DependencyProperty.RegisterAttached("ShowDialogSwitch", typeof(bool), typeof(WindowExtension),
            new PropertyMetadata(false, OnShowDialogSwitchChanged));

        public static readonly DependencyProperty CloseSwitchProperty = DependencyProperty.RegisterAttached("CloseSwitch", typeof(bool), typeof(WindowExtension),
            new PropertyMetadata(false, OnCloseSwitchChanged));

        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", typeof(bool), typeof(WindowExtension),
            new PropertyMetadata(false, OnDialogResultChanged));

        private static void OnShowSwitchChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            Window control = dp as Window;

            if (control == null)
            {
                return;
            }

            if ((bool)(e.NewValue))
            {
                control.Show();
                SetShowSwitch(dp, false);
            }
        }

        private static void OnShowDialogSwitchChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            Window control = dp as Window;

            if (control == null)
            {
                return;
            }

            if ((bool)(e.NewValue))
            {
                control.ShowDialog();
                SetShowDialogSwitch(dp, false);
            }
        }

        private static void OnCloseSwitchChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            Window control = dp as Window;

            if (control == null)
            {
                return;
            }

            if((bool)(e.NewValue))
            {
                control.Close();
                SetCloseSwitch(dp, false);
            }
        }

        private static void OnDialogResultChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            Window control = dp as Window;

            if (control == null
                || !System.Windows.Interop.ComponentDispatcher.IsThreadModal)
            {
                return;
            }

            control.DialogResult = (bool)e.NewValue;
        }

        public static void SetShowSwitch(DependencyObject dp, bool value)
        {
            dp.SetValue(ShowSwitchProperty, value);
        }

        public static void SetShowDialogSwitch(DependencyObject dp, bool value)
        {
            dp.SetValue(ShowDialogSwitchProperty, value);
        }

        public static void SetCloseSwitch(DependencyObject dp, bool value)
        {
            dp.SetValue(CloseSwitchProperty, value);
        }

        public static void SetDialogResult(DependencyObject dp, bool value)
        {
            dp.SetValue(DialogResultProperty, value);
        }

        public static bool GetShowSwitch(DependencyObject dp)
        {
            return (bool)dp.GetValue(ShowSwitchProperty);
        }

        public static bool GetShowDialogSwitch(DependencyObject dp)
        {
            return (bool)dp.GetValue(ShowDialogSwitchProperty);
        }

        public static bool GetCloseSwitch(DependencyObject dp)
        {
            return (bool)dp.GetValue(CloseSwitchProperty);
        }

        public static bool GetDialogResult(DependencyObject dp)
        {
            return (bool)dp.GetValue(DialogResultProperty);
        }
    }
}
