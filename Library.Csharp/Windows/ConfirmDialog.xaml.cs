using PrismWPF.Common.Converter;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PrismWPF.Common.Windows
{
    /// <summary>
    /// ConfirmDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ConfirmDialog : Window
    {
        internal Func<bool> Verified { get; set; }

        internal Action VerificationFailed { get; set; }

        internal string Text
        {
            get => MessageTextBox.Text;
            set => MessageTextBox.Text = value;
        }

        private string _verificationText;
        internal string VerificationText
        {
            get => _verificationText;
            set
            {
                _verificationText = value;
                if (value == null)
                {
                    VerificationTextBox.Visibility = Visibility.Collapsed;
                }
                else
                {
                    VerificationTextBox.Visibility = Visibility.Visible;
                    VerificationTextBox.Focus();
                }
            }
        }

        internal ConfirmDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OnOkBtnClick(object sender, RoutedEventArgs e)
        {
            if (VerificationText != null)
            {
                string InputtedText = VerificationTextBox.Text;

                if (InputtedText == VerificationText)
                {
                    OnVerified();
                }
                else
                {
                    OnVerificationFailed();
                }
            }
            else
            {
                OnVerified();
            }
        }

        private void OnVerified()
        {
            if (Verified == null || Verified())
            {
                DialogResult = true;
                Close();
            }
        }

        private void OnVerificationFailed()
        {
            VerificationFailed?.Invoke();
        }

        private void OnCancelBtnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    {
                        e.Handled = true;
                        CancelBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        return;
                    }
                case Key.Enter:
                    {
                        e.Handled = true;
                        OkBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        return;
                    }
            }
        }
    }

    public class ConfirmMessageBox
    {
        public static bool Show(string text, Func<bool> onVerified, Action onVerificationFailed,
            string verificationText, string title, string iconUri)
        {
            return Show(null, text, onVerified, onVerificationFailed, verificationText, title, iconUri);
        }

        public static bool Show(string text, Func<bool> onVerified, Action onVerificationFailed,
            string verificationText, string title, Uri iconUri)
        {
            return Show(null, text, onVerified, onVerificationFailed, verificationText, title, iconUri);
        }

        public static bool Show(string text, Func<bool> onVerified, Action onVerificationFailed = null,
            string verificationText = null, string title = null, ImageSource icon = null)
        {
            return Show(null, text, onVerified, onVerificationFailed, verificationText, title, icon);
        }

        public static bool Show(Window owner, string text, Func<bool> onVerified, Action onVerificationFailed,
            string verificationText, string title, string iconUri)
        {
            return Show(owner, text, onVerified, onVerificationFailed, verificationText, title, ObjectConverter.ToImageSource(iconUri));
        }

        public static bool Show(Window owner, string text, Func<bool> onVerified, Action onVerificationFailed,
            string verificationText, string title, Uri iconUri)
        {
            return Show(owner, text, onVerified, onVerificationFailed, verificationText, title, ObjectConverter.ToImageSource(iconUri));
        }

        public static bool Show(Window owner, string text, Func<bool> onVerified, Action onVerificationFailed = null,
            string verificationText = null, string title = null, ImageSource icon = null)
        {
            ConfirmDialog dlg = new ConfirmDialog()
            {
                Text = text,
                Verified = onVerified,
                VerificationFailed = onVerificationFailed,
                VerificationText = verificationText,
            };

            if (owner != null)
            {
                dlg.Owner = owner;
            }

            if (title != null)
            {
                dlg.Title = title;
            }

            if (icon != null)
            {
                dlg.Icon = icon;
            }

            return dlg.ShowDialog().Value;
        }
    }
}
