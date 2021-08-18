using System.Windows;

namespace PrismWPF.Control
{
    public static class VisibleExtension
    {
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(VisibleExtension), 
            new PropertyMetadata(false, OnIsVisibleChanged));

        public static readonly DependencyProperty IsBindProperty = DependencyProperty.RegisterAttached("IsBind", typeof(bool), typeof(VisibleExtension), 
            new PropertyMetadata(false, OnIsBindChanged));

        private static void OnIsVisibleChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            UIElement box = dp as UIElement;

            if (box == null)
            {
                return;
            }

            bool newValue = (bool)(e.NewValue);

            SetIsVisible(box, box.IsVisible);
        }

        private static void OnIsBindChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            UIElement box = dp as UIElement;

            if (box == null)
            {
                return;
            }

            bool wasBound = (bool)(e.OldValue);
            bool needToBind = (bool)(e.NewValue);

            if (wasBound)
            {
                box.IsVisibleChanged -= OnHandleIsVisibleChanged;
            }

            if (needToBind)
            {
                box.IsVisibleChanged += OnHandleIsVisibleChanged;

                SetIsVisible(box, box.IsVisible);
            }
        }

        private static void OnHandleIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UIElement box = sender as UIElement;

            SetIsVisible(box, box.IsVisible);
        }

        public static void SetIsBind(DependencyObject dp, bool value)
        {
            dp.SetValue(IsBindProperty, value);
        }

        public static bool GetIsBind(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsBindProperty);
        }

        public static void SetIsVisible(DependencyObject dp, bool value)
        {
            dp.SetValue(IsVisibleProperty, value);
        }

        public static bool GetIsVisible(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsVisibleProperty);
        }
    }
}
