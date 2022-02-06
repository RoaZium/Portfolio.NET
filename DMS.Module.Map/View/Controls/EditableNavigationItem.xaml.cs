using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DMS.Module.Map.View
{
    /// <summary>
    /// EditableNavigationItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditableNavigationItem : UserControl
    {
        public delegate void CaptionChangedHandler(object sender, EventArgs e);

        public event CaptionChangedHandler CaptionChanged;

        public static readonly DependencyProperty IsEditModeProperty = DependencyProperty.Register("IsEditMode", typeof(bool),
            typeof(EditableNavigationItem), new PropertyMetadata(false, OnIsEditModeChanged));

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string),
            typeof(EditableNavigationItem), new PropertyMetadata("", OnCaptionChanged));

        public bool IsEditMode
        {
            get => (bool)GetValue(IsEditModeProperty);
            set => SetValue(IsEditModeProperty, value);
        }

        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        public EditableNavigationItem()
        {
            InitializeComponent();
        }

        private void OnContainerMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsEditMode = false;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox s = sender as TextBox;

                s.SelectAll();
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            IsEditMode = false;
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox s = (sender as TextBox);
                switch (e.Key)
                {
                    case Key.Escape:
                        {
                            s.Text = Caption;
                            editMode = false;
                            IsEditMode = false;
                            e.Handled = true;
                            return;
                        }
                    case Key.Enter:
                        {
                            editMode = s.Text != Caption;
                            IsEditMode = false;
                            e.Handled = true;
                            return;
                        }
                }
            }
            catch { }
        }

        private bool editMode = false;

        private static void OnIsEditModeChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            EditableNavigationItem control = dp as EditableNavigationItem;

            if (control == null)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                control.editMode = control.IsEditMode;
            }
        }

        private static void OnCaptionChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            EditableNavigationItem control = dp as EditableNavigationItem;

            if (control == null)
            {
                return;
            }

            if (control.editMode)
            {
                control.editMode = false;
                if (control.CaptionChanged != null)
                {
                    control.CaptionChanged(control, new EventArgs());
                }
            }
        }
    }
}
