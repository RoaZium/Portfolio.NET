using System.Windows;


namespace PrismWPF.Control
{
    public static class UpdateSourceExtension
    {
        public static readonly DependencyProperty UpdateSourceSwitchProperty = DependencyProperty.RegisterAttached("UpdateSourceSwitch", typeof(bool), typeof(UpdateSourceExtension),
            new PropertyMetadata(false, OnUpdateSourceSwitchChanged));

        public static readonly DependencyProperty SourcePropertyTypeProperty = DependencyProperty.RegisterAttached(
            "SourcePropertyType", typeof(DependencyProperty), typeof(UpdateSourceExtension));

        private static void OnUpdateSourceSwitchChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement control = dp as FrameworkElement;

            if (control == null)
            {
                return;
            }

            if ((bool)(e.NewValue))
            {
                control.GetBindingExpression(GetSourcePropertyType(dp)).UpdateSource();

                SetUpdateSourceSwitch(dp, false);
            }
        }

        public static void SetUpdateSourceSwitch(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdateSourceSwitchProperty, value);
        }

        public static bool GetUpdateSourceSwitch(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdateSourceSwitchProperty);
        }

        public static void SetSourcePropertyType(DependencyObject dp, DependencyProperty value)
        {
            dp.SetValue(SourcePropertyTypeProperty, value);
        }

        public static DependencyProperty GetSourcePropertyType(DependencyObject dp)
        {
            return (DependencyProperty)dp.GetValue(SourcePropertyTypeProperty);
        }
    }
}
