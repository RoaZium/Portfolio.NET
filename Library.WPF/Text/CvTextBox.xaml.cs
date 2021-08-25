using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrismWPF.Control.Text
{
    /// <summary>
    /// CvTextBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CvTextBox : TextBox
    {
        public static readonly DependencyProperty TextTypeProperty =
                DependencyProperty.Register(
                        "TextType",
                        typeof(TextType),
                        typeof(CvTextBox),
                        new PropertyMetadata(
                                TextType.All));

        public TextType TextType
        {
            get { return (TextType)GetValue(TextTypeProperty); }
            set { SetValue(TextTypeProperty, value); }
        }

        public CvTextBox()
        {
            InitializeComponent();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if ((TextType & TextType.Number) == TextType.Number)
            {
                if (e.Key != Key.Space)
                {
                    return;
                }
            }
            if ((TextType & TextType.SignedNumber) == TextType.SignedNumber)
            {
                if (e.Key != Key.Space)
                {
                    return;
                }
            }
            if ((TextType & TextType.Decimal) == TextType.Decimal)
            {
                if (e.Key != Key.Space)
                {
                    return;
                }
            }
            if ((TextType & TextType.SignedDecimal) == TextType.SignedDecimal)
            {
                if (e.Key != Key.Space)
                {
                    return;
                }
            }
            if ((TextType & TextType.DateTime) == TextType.DateTime)
            {
                return;
            }
            if ((TextType & TextType.Date) == TextType.Date)
            {
                if (e.Key != Key.Space)
                {
                    return;
                }
            }
            if ((TextType & TextType.Time) == TextType.Time)
            {
                if (e.Key != Key.Space)
                {
                    return;
                }
            }
            if ((TextType & TextType.Email) == TextType.Email)
            {
                return;
            }
            if ((TextType & TextType.URL) == TextType.URL)
            {
                return;
            }
            if ((TextType & TextType.Phone) == TextType.Phone)
            {
                if (e.Key != Key.Space)
                {
                    return;
                }
            }

            e.Handled = true;
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            if (TextType == TextType.None)
            {
                e.Handled = true;
                return;
            }

            if (TextType == TextType.All)
            {
                return;
            }

            if (TextType == TextType.PositiveInteger)
            {
                if (Regex.IsMatch(e.Text, "^[.0-9]+$"))
                {
                    return;
                }
            }

            if ((TextType & TextType.SignedNumber) == TextType.SignedNumber)
            {
                if (Regex.IsMatch(e.Text, "^[-+0-9]+$"))
                {
                    return;
                }
            }

            if ((TextType & TextType.Decimal) == TextType.Decimal)
            {
                if (Regex.IsMatch(e.Text, "^[.0-9]+$"))
                {
                    return;
                }
            }

            if ((TextType & TextType.SignedDecimal) == TextType.SignedDecimal)
            {
                if (Regex.IsMatch(e.Text, "^[-+.0-9]+$"))
                {
                    return;
                }
            }

            if ((TextType & TextType.DateTime) == TextType.DateTime)
            {
                if (Regex.IsMatch(e.Text, "^[-:\\s0-9]+$"))
                {
                    return;
                }
            }

            if ((TextType & TextType.Date) == TextType.Date)
            {
                if (Regex.IsMatch(e.Text, "^[-0-9]+$"))
                {
                    return;
                }
            }

            if ((TextType & TextType.Time) == TextType.Time)
            {
                if (Regex.IsMatch(e.Text, "^[:0-9]+$"))
                {
                    return;
                }
            }

            if ((TextType & TextType.Email) == TextType.Email)
            {
                if (Regex.IsMatch(e.Text, @"^[^""\\;:()]+$"))
                {
                    return;
                }
            }

            if ((TextType & TextType.URL) == TextType.URL)
            {
                if (Regex.IsMatch(e.Text, @"^[^""\\]+$"))
                {
                    return;
                }
            }

            if ((TextType & TextType.Phone) == TextType.Phone)
            {
                if (Regex.IsMatch(e.Text, "^[-+0-9]+$"))
                {
                    return;
                }
            }

            // 공정.센서 0이 아닌 1부터 시작 하도록
            if ((TextType & TextType.Number) == TextType.Number)
            {
                if (Regex.IsMatch(e.Text, "^[0-9]+$"))
                {
                    return;
                }
            }

            if ((TextType & TextType.NullNumber) == TextType.NullNumber)
            {
                if (Regex.IsMatch(e.Text, "^[0-9]+$") || (string.IsNullOrEmpty(e.Text)))
                {
                    return;
                }
            }

            if ((TextType & TextType.PositiveInteger) == TextType.PositiveInteger)
            {
                if (Regex.IsMatch(e.Text, "^[.0-9]+$"))
                {
                    return;
                }
            }

            e.Handled = true;
        }

        private string _OldText = null;

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            TextChange change = e.Changes.ElementAt(0);
            string text = this.Text;

            if (text == _OldText)
            {
                return;
            }

            _OldText = text;

            if (e.UndoAction == UndoAction.Clear)
            {
                return;
            }

            string addedText = text.Substring(change.Offset, change.AddedLength);
            int caretIndex = this.CaretIndex;

            if (TextType == TextType.Number)
            {
                if (Regex.IsMatch(text, "[0-9]+"))
                {
                    foreach (Match matched in Regex.Matches(text.Substring(0, caretIndex), "[^0-9]+"))
                    {
                        caretIndex -= matched.Value.Length;
                    }
                    text = Regex.Replace(text, "[^0-9]+", "");

                    caretIndex -= Regex.Match(text.Substring(0, caretIndex), "^0+").Value.Length;
                    text = Regex.Replace(text, "^0+", "");

                    if (text == string.Empty)
                    {
                        text = "0";
                        caretIndex = 1;
                    }

                    if (this.Text != text)
                    {
                        this.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            this.Text = text;
                            this.CaretIndex = caretIndex;
                        }));

                        return;
                    }
                }
                else
                {
                    this.Text = "0";
                    this.CaretIndex = 1;

                    return;
                }
            }
            else if (TextType == TextType.SignedNumber)
            {
                if (Regex.IsMatch(text, "[-+0-9]+"))
                {
                    foreach (Match matched in Regex.Matches(text.Substring(0, caretIndex), "[^-+0-9]+"))
                    {
                        caretIndex -= matched.Value.Length;
                    }
                    text = Regex.Replace(text, "[^-+0-9]+", "");

                    Regex signRegex = new Regex("[-+]");
                    Match matchedSign = signRegex.Match(addedText);
                    if (!matchedSign.Success)
                    {
                        matchedSign = signRegex.Match(text);
                    }
                    if (matchedSign.Success)
                    {
                        caretIndex -= signRegex.Matches(text.Substring(0, caretIndex)).Count
                            - (matchedSign.Value == "-" ? 1 : 0);
                        text = (matchedSign.Value == "+" ? "" : "-") + signRegex.Replace(text, "");
                    }

                    Match m = Regex.Match(text, "(?<=^-?)0+(?=[0-9])");
                    if (m.Index + m.Length > caretIndex)
                    {
                        caretIndex = m.Index;
                    }
                    else
                    {
                        caretIndex -= m.Length;
                    }

                    text = Regex.Replace(text, "(?<=^-?)0+(?=[0-9])", "");

                    if (text == string.Empty)
                    {
                        text = "0";
                        caretIndex = 1;
                    }

                    if (this.Text != text)
                    {
                        this.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            this.Text = text;
                            this.CaretIndex = caretIndex;
                        }));

                        return;
                    }
                }
                else
                {
                    this.Text = "0";
                    this.CaretIndex = 1;

                    return;
                }
            }
            else if (TextType == TextType.Decimal)
            {
                if (Regex.IsMatch(text, "[.0-9]+"))
                {
                    foreach (Match matched in Regex.Matches(text.Substring(0, caretIndex), "[^.0-9]+"))
                    {
                        caretIndex -= matched.Value.Length;
                    }
                    text = Regex.Replace(text, "[^.0-9]+", "");

                    Regex pointRegex = new Regex("[.]");
                    if (pointRegex.IsMatch(addedText))
                    {
                        int pointIndex = change.Offset + addedText.IndexOf(".");

                        pointIndex -= pointRegex.Matches(text.Substring(0, pointIndex)).Count;
                        caretIndex -= pointRegex.Matches(text.Substring(0, caretIndex)).Count - (pointIndex == 0 ? 2 : 1);

                        text = (pointIndex == 0 ? "0" : "") + pointRegex.Replace(text, "").Insert(pointIndex, ".");
                    }
                    else if (pointRegex.IsMatch(text))
                    {
                        int pointIndex = text.IndexOf(".");

                        caretIndex -= pointRegex.Matches(text.Substring(0, caretIndex)).Count
                            - (pointIndex == 0 ? 2
                            : (pointIndex < caretIndex ? 1 : 0));

                        text = (pointIndex == 0 ? "0" : "") + pointRegex.Replace(text, "").Insert(pointIndex, ".");
                    }

                    Match m = Regex.Match(text, "^0+(?=([0-9]+[.])|([0-9]))");
                    if (m.Index + m.Length > caretIndex)
                    {
                        caretIndex = m.Index;
                    }
                    else
                    {
                        caretIndex -= m.Length;
                    }

                    text = Regex.Replace(text, "^0+(?=([0-9]+[.])|([0-9]))", "");

                    if (text == string.Empty)
                    {
                        text = "0";
                        caretIndex = 1;
                    }

                    if (this.Text != text)
                    {
                        this.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            this.Text = text;
                            this.CaretIndex = caretIndex;
                        }));

                        return;
                    }
                }
                else
                {
                    this.Text = "0";
                    this.CaretIndex = 1;

                    return;
                }
            }
            else if (TextType == TextType.SignedDecimal)
            {
                if (Regex.IsMatch(text, "[-.+0-9]+"))
                {
                    foreach (Match matched in Regex.Matches(text.Substring(0, caretIndex), "[^-.+0-9]+"))
                    {
                        caretIndex -= matched.Value.Length;
                    }
                    text = Regex.Replace(text, "[^-.+0-9]+", "");

                    Regex signRegex = new Regex("[-+]");
                    Match matchedSign = signRegex.Match(addedText);
                    if (!matchedSign.Success)
                    {
                        matchedSign = signRegex.Match(text);
                    }
                    if (matchedSign.Success)
                    {
                        caretIndex -= signRegex.Matches(text.Substring(0, caretIndex)).Count
                            - (matchedSign.Value == "-" ? 1 : 0);
                        text = (matchedSign.Value == "+" ? "" : "-") + signRegex.Replace(text, "");
                    }

                    Regex pointRegex = new Regex("[.]");
                    if (pointRegex.IsMatch(addedText))
                    {
                        int pointIndex = change.Offset + addedText.IndexOf(".");

                        pointIndex -= pointRegex.Matches(text.Substring(0, pointIndex)).Count;
                        caretIndex -= pointRegex.Matches(text.Substring(0, caretIndex)).Count - (pointIndex == 0 ? 2 : 1);

                        text = (pointIndex == 0 ? "0" : "") + pointRegex.Replace(text, "").Insert(pointIndex, ".");
                    }
                    else if (pointRegex.IsMatch(text))
                    {
                        int pointIndex = text.IndexOf(".");

                        caretIndex -= pointRegex.Matches(text.Substring(0, caretIndex)).Count
                            - (pointIndex == 0 ? 2
                            : (pointIndex < caretIndex ? 1 : 0));

                        text = (pointIndex == 0 ? "0" : "") + pointRegex.Replace(text, "").Insert(pointIndex, ".");
                    }

                    Match m = Regex.Match(text, "(?<=^-?)0+(?=([0-9]+[.])|([0-9]))");
                    if (m.Index + m.Length > caretIndex)
                    {
                        caretIndex = m.Index;
                    }
                    else
                    {
                        caretIndex -= m.Length;
                    }

                    text = Regex.Replace(text, "(?<=^-?)0+(?=([0-9]+[.])|([0-9]))", "");

                    if (text == string.Empty)
                    {
                        text = "0";
                        caretIndex = 1;
                    }

                    if (this.Text != text)
                    {
                        this.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            this.Text = text;
                            this.CaretIndex = caretIndex;
                        }));

                        return;
                    }
                }
                else
                {
                    this.Text = "0";
                    this.CaretIndex = 1;

                    return;
                }
            }
            else if (TextType == TextType.Email)
            {
                Regex atSignRegex = new Regex("@");
                if (atSignRegex.IsMatch(addedText))
                {
                    int atSignIndex = change.Offset + addedText.IndexOf("@");

                    atSignIndex -= atSignRegex.Matches(text.Substring(0, atSignIndex)).Count;
                    caretIndex -= atSignRegex.Matches(text.Substring(0, caretIndex)).Count - 1;

                    text = atSignRegex.Replace(text, "").Insert(atSignIndex, "@");
                }
                else if (atSignRegex.IsMatch(text))
                {
                    int atSignIndex = text.IndexOf("@");

                    caretIndex -= atSignRegex.Matches(text.Substring(0, caretIndex)).Count - (atSignIndex < caretIndex ? 1 : 0);

                    text = atSignRegex.Replace(text, "").Insert(atSignIndex, "@");
                }

                if (this.Text != text)
                {
                    this.Text = text;
                    this.CaretIndex = caretIndex;

                    return;
                }
            }
            else if (TextType == TextType.PositiveInteger)
            {
                if (text == "0")
                {
                    this.Text = "1";
                    return;
                }

                if (Regex.IsMatch(text, "[.0-9]+"))
                {
                    foreach (Match matched in Regex.Matches(text.Substring(0, caretIndex), "[^.0-9]+"))
                    {
                        caretIndex -= matched.Value.Length;
                    }
                    text = Regex.Replace(text, "[^.0-9]+", "");

                    Regex pointRegex = new Regex("[.]");
                    if (pointRegex.IsMatch(addedText))
                    {
                        int pointIndex = change.Offset + addedText.IndexOf(".");

                        pointIndex -= pointRegex.Matches(text.Substring(0, pointIndex)).Count;
                        caretIndex -= pointRegex.Matches(text.Substring(0, caretIndex)).Count - (pointIndex == 0 ? 2 : 1);

                        text = (pointIndex == 0 ? "0" : "") + pointRegex.Replace(text, "").Insert(pointIndex, ".");
                    }
                    else if (pointRegex.IsMatch(text))
                    {
                        int pointIndex = text.IndexOf(".");

                        caretIndex -= pointRegex.Matches(text.Substring(0, caretIndex)).Count
                            - (pointIndex == 0 ? 2
                            : (pointIndex < caretIndex ? 1 : 0));

                        text = (pointIndex == 0 ? "0" : "") + pointRegex.Replace(text, "").Insert(pointIndex, ".");
                    }

                    Match m = Regex.Match(text, "^0+(?=([0-9]+[.])|([0-9]))");
                    if (m.Index + m.Length > caretIndex)
                    {
                        caretIndex = m.Index;
                    }
                    else
                    {
                        caretIndex -= m.Length;
                    }

                    text = Regex.Replace(text, "^0+(?=([0-9]+[.])|([0-9]))", "");

                    if (text == string.Empty)
                    {
                        text = "0";
                        caretIndex = 1;
                    }

                    if (this.Text != text)
                    {
                        this.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            this.Text = text;
                            this.CaretIndex = caretIndex;
                        }));

                        return;
                    }
                }
                else
                {
                    this.Text = "0";
                    this.CaretIndex = 1;

                    return;
                }
            }
        }
    }
}
