using System.Windows;
using System.Windows.Controls;

namespace PrismWPF.Control
{
   public static class PasswordExtension
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordExtension),
            new PropertyMetadata(string.Empty, OnPasswordChanged));

        public static readonly DependencyProperty IsBindProperty = DependencyProperty.RegisterAttached("IsBind", typeof(bool), typeof(PasswordExtension),
            new PropertyMetadata(false, OnIsBindChanged));

        private static readonly DependencyProperty UpdatingPasswordProperty = DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordExtension),
            new PropertyMetadata(false));

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = d as PasswordBox;

            // only handle this event when the property is attached to a PasswordBox
            // and when the BindPassword attached property has been set to true
            if (d == null || !GetIsBind(d))
            {
                return;
            }

            // avoid recursive updating by ignoring the box's changed event
            box.PasswordChanged -= HandlePasswordChanged;

            string newPassword = (string)e.NewValue;

            if (!GetUpdatingPassword(box))
            {
                box.Password = newPassword;
            }

            box.PasswordChanged += HandlePasswordChanged;
        }

        private static void OnIsBindChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event

            PasswordBox box = dp as PasswordBox;

            if (box == null)
            {
                return;
            }

            bool wasBound = (bool)(e.OldValue);
            bool needToBind = (bool)(e.NewValue);

            if (wasBound)
            {
                box.PasswordChanged -= HandlePasswordChanged;
            }

            if (needToBind)
            {
                box.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            // set a flag to indicate that we're updating the password
            SetUpdatingPassword(box, true);
            // push the new password into the BoundPassword property
            SetPassword(box, box.Password);
            SetUpdatingPassword(box, false);
        }

        public static void SetIsBind(DependencyObject dp, bool value)
        {
            dp.SetValue(IsBindProperty, value);
        }

        public static bool GetIsBind(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsBindProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPasswordProperty);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPasswordProperty, value);
        }
    }
}
