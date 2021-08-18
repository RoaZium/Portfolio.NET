using System.Windows;
using System.Windows.Controls;

namespace PrismWPF.Control
{
    public static class EditModeExtension
    {
        public static readonly DependencyProperty ContextMenuProperty = DependencyProperty.RegisterAttached("ContextMenu", typeof(ContextMenu), typeof(EditModeExtension));

        public static ContextMenu GetContextMenu(DependencyObject obj)
        {
            return (ContextMenu)obj.GetValue(ContextMenuProperty);
        }

        public static void SetContextMenu(DependencyObject obj, ContextMenu value)
        {
            obj.SetValue(ContextMenuProperty, value);
        }
    }
}
