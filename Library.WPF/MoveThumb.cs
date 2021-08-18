using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace PrismWPF.Control
{
    public class MoveThumb : Thumb
    {
        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register(
                "TargetElement",
                typeof(FrameworkElement),
                typeof(MoveThumb),
                new FrameworkPropertyMetadata(null));

        public FrameworkElement TargetElement
        {
            get { return (FrameworkElement)GetValue(TargetElementProperty); }
            set { SetValue(TargetElementProperty, value); }
        }

        public MoveThumb()
        {
            DragStarted += new DragStartedEventHandler(MoveThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(MoveThumb_DragDelta);
        }

        private double targetLeftOnDragStarted;
        private double targetTopOnDragStarted;

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            FrameworkElement moveTarget = TargetElement;
            if(moveTarget == null)
                return;

            targetLeftOnDragStarted = moveTarget.Margin.Left;
            targetTopOnDragStarted = moveTarget.Margin.Top;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement moveTarget = TargetElement;
            if(moveTarget == null)
                return;

            Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

            // 이미 회전되어 있는 항목을 이동시키기 위한 작업
            RotateTransform rotateTransform = moveTarget.RenderTransform as RotateTransform;
            if(rotateTransform != null)
            {
                dragDelta = rotateTransform.Transform(dragDelta);
            }

            Thickness margin = moveTarget.Margin;

            targetLeftOnDragStarted = margin.Left + dragDelta.X;
            targetTopOnDragStarted = margin.Top + dragDelta.Y;

            margin.Left = targetLeftOnDragStarted < 0 ? 0 : targetLeftOnDragStarted;
            margin.Top = targetTopOnDragStarted < 0 ? 0 : targetTopOnDragStarted;

            moveTarget.Margin = margin;
        }
    }
}
