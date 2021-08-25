using System.Windows;
using System.Windows.Media;

namespace PrismWPF.Common.Infrastructure
{
    public static class ControlUtils
    {
        public static T FindAncestor<T>(this DependencyObject child) where T : class
        {
            DependencyObject parent = child.GetParent();

            while (parent != null)
            {
                if (typeof(T).IsInstanceOfType(parent))
                {
                    return parent as T;
                }

                parent = parent.GetParent();
            }

            return null;
        }

        public static DependencyObject GetParent(this DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
            {
                parent = (child as FrameworkElement)?.Parent;
            }

            return parent;
        }
    }
}
