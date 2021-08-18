using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace PrismWPF.Control
{
    public class ResizeThumb : Thumb
    {
        private double angle;
        private Point transformOrigin;
        private FrameworkElement designerItem;

        public ResizeThumb()
        {
            DragStarted += new DragStartedEventHandler(this.ResizeThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        private double targetLeftOnDragStarted;
        private double targetTopOnDragStarted;

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as FrameworkElement;
            if (this.designerItem == null)
                return;

            targetLeftOnDragStarted = designerItem.Margin.Left;
            targetTopOnDragStarted = designerItem.Margin.Top;

            this.transformOrigin = this.designerItem.RenderTransformOrigin;
            RotateTransform rotateTransform = this.designerItem.RenderTransform as RotateTransform;

            if (rotateTransform != null)
            {
                this.angle = rotateTransform.Angle * Math.PI / 180.0;
            }
            else
            {
                this.angle = 0;
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem == null)
                return;

            double deltaVertical, deltaHorizontal;

            switch (VerticalAlignment)
            {
                case System.Windows.VerticalAlignment.Bottom:
                    {
                        deltaVertical = Math.Min(-e.VerticalChange, this.designerItem.Height - this.designerItem.MinHeight);

                        Thickness margin = this.designerItem.Margin;

                        targetLeftOnDragStarted = margin.Left - (deltaVertical * this.transformOrigin.Y * Math.Sin(-this.angle));
                        targetTopOnDragStarted = margin.Top + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle)));

                        margin.Left = targetLeftOnDragStarted < 0 ? 0 : targetLeftOnDragStarted;
                        margin.Top = targetTopOnDragStarted < 0 ? 0 : targetTopOnDragStarted;

                        this.designerItem.Margin = margin;

                        if(targetTopOnDragStarted < 0)
                        {
                            this.designerItem.Height -= deltaVertical - targetTopOnDragStarted;
                        } else
                        {
                            this.designerItem.Height -= deltaVertical;
                        }
                    }
                    break;
                case System.Windows.VerticalAlignment.Top:
                    {
                        deltaVertical = Math.Min(e.VerticalChange, this.designerItem.Height - this.designerItem.MinHeight);

                        Thickness margin = this.designerItem.Margin;

                        targetLeftOnDragStarted = margin.Left + deltaVertical * Math.Sin(-this.angle) - (this.transformOrigin.Y * deltaVertical * Math.Sin(-this.angle));
                        targetTopOnDragStarted = margin.Top + deltaVertical * Math.Cos(-this.angle) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle)));

                        margin.Left = targetLeftOnDragStarted < 0 ? 0 : targetLeftOnDragStarted;
                        margin.Top = targetTopOnDragStarted < 0 ? 0 : targetTopOnDragStarted;

                        this.designerItem.Margin = margin;

                        if (targetTopOnDragStarted < 0)
                        {
                            this.designerItem.Height -= deltaVertical - targetTopOnDragStarted;
                        }
                        else
                        {
                            this.designerItem.Height -= deltaVertical;
                        }
                    }
                    break;
                default:
                    break;
            }

            switch (HorizontalAlignment)
            {
                case System.Windows.HorizontalAlignment.Left:
                    {
                        deltaHorizontal = Math.Min(e.HorizontalChange, this.designerItem.Width - this.designerItem.MinWidth);

                        Thickness margin = this.designerItem.Margin;

                        targetLeftOnDragStarted = margin.Left + deltaHorizontal * Math.Cos(this.angle) + (this.transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angle)));
                        targetTopOnDragStarted = margin.Top + deltaHorizontal * Math.Sin(this.angle) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle);

                        margin.Left = targetLeftOnDragStarted < 0 ? 0 : targetLeftOnDragStarted;
                        margin.Top = targetTopOnDragStarted < 0 ? 0 : targetTopOnDragStarted;

                        this.designerItem.Margin = margin;

                        if (targetLeftOnDragStarted < 0)
                        {
                            this.designerItem.Width -= deltaHorizontal - targetLeftOnDragStarted;
                        }
                        else
                        {
                            this.designerItem.Width -= deltaHorizontal;
                        }
                    }
                    break;
                case System.Windows.HorizontalAlignment.Right:
                    {
                        deltaHorizontal = Math.Min(-e.HorizontalChange, this.designerItem.Width - this.designerItem.MinWidth);

                        Thickness margin = this.designerItem.Margin;

                        targetLeftOnDragStarted = margin.Left + (deltaHorizontal * this.transformOrigin.X * (1 - Math.Cos(this.angle)));
                        targetTopOnDragStarted = margin.Top = margin.Top - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle);

                        margin.Left = targetLeftOnDragStarted < 0 ? 0 : targetLeftOnDragStarted;
                        margin.Top = targetTopOnDragStarted < 0 ? 0 : targetTopOnDragStarted;

                        this.designerItem.Margin = margin;

                        if (targetLeftOnDragStarted < 0)
                        {
                            this.designerItem.Width -= deltaHorizontal - targetLeftOnDragStarted;
                        }
                        else
                        {
                            this.designerItem.Width -= deltaHorizontal;
                        }
                    }
                    break;
                default:
                    break;
            }

            e.Handled = true;
        }
    }
}
