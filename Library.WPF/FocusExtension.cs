using System.Windows;


namespace PrismWPF.Control
{
    public static class FocusExtension
    {
        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusExtension),
            new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

        public static readonly DependencyProperty IsBindProperty = DependencyProperty.RegisterAttached("IsBind", typeof(bool), typeof(FocusExtension), 
            new PropertyMetadata(false, OnIsBindChanged));

        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static void SetIsBind(DependencyObject dp, bool value)
        {
            dp.SetValue(IsBindProperty, value);
        }

        public static bool GetIsBind(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsBindProperty);
        }

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uie = (FrameworkElement)d;

            SyncFocusProperty(uie, (bool)e.NewValue);
        }

        private static void OnIsBindChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement ui = dp as FrameworkElement;

            if (ui == null)
            {
                return;
            }

            bool wasBound = (bool)(e.OldValue);
            bool needToBind = (bool)(e.NewValue);

            if (wasBound)
            {
                ui.GotFocus -= OnHandleIsFocusChanged;
                ui.LostFocus -= OnHandleIsFocusChanged;
                ui.Loaded -= OnHandleIsUiLoaded;
            }

            if (needToBind)
            {
                ui.GotFocus += OnHandleIsFocusChanged;
                ui.LostFocus += OnHandleIsFocusChanged;
                if (!ui.IsLoaded)
                {
                    ui.Loaded += OnHandleIsUiLoaded;
                }
            }
        }

        private static void OnHandleIsFocusChanged(object sender, RoutedEventArgs e)
        {
            FrameworkElement ui = sender as FrameworkElement;

            SetIsFocused(ui, ui.IsFocused);
        }

        private static void OnHandleIsUiLoaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement ui = sender as FrameworkElement;

            ui.Loaded -= OnHandleIsUiLoaded;

            SyncFocusProperty(ui, GetIsFocused(ui));
        }

        private static void SyncFocusProperty(UIElement ui, bool isFocused)
        {
            if (isFocused)
            {
                if (!ui.IsFocused)
                {
                    ui.Focus();
                }
            }
        }
    }
}
