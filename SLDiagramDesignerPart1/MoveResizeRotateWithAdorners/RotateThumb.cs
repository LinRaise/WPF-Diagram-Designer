using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;

namespace DiagramDesigner
{
    public class RotateThumb : Control
    {
        private Point centerPoint;
        private Point startPoint;
        private double initialAngle;
        private Canvas designerCanvas;
        private ContentControl designerItem;
        private RotateTransform rotateTransform;

        private bool isDragging = false;

        public RotateThumb()
        {
            this.DefaultStyleKey = typeof(RotateThumb);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(RotateThumb_MouseLeftButtonDown);
            this.MouseMove += new MouseEventHandler(RotateThumb_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(RotateThumb_MouseLeftButtonUp);
        }

        void RotateThumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.isDragging = false;
        }

        void RotateThumb_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging)
            {
                if (this.designerItem != null && this.designerCanvas != null)
                {
                    Point endPoint = e.GetPosition(this.designerCanvas);
                    double angle = (Math.Atan2(endPoint.Y - this.centerPoint.Y, endPoint.X - this.centerPoint.X) - Math.Atan2(startPoint.Y - this.centerPoint.Y, startPoint.X - this.centerPoint.X)) * 180 / Math.PI;
                    RotateTransform rotateTransform = this.designerItem.RenderTransform as RotateTransform;

                    rotateTransform.Angle = this.initialAngle + Math.Round(angle, 0);
                    this.designerItem.InvalidateMeasure();
                }
            }
        }

        void RotateThumb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.CaptureMouse())
            {
                isDragging = true;
                this.designerItem = DataContext as ContentControl;

                if (this.designerItem != null)
                {
                    this.designerCanvas = VisualTreeHelper.GetParent(this.designerItem) as Canvas;

                    if (this.designerCanvas != null)
                    {
                        this.centerPoint = this.designerItem.TransformToVisual(this.designerCanvas).Transform(
                            new Point(this.designerItem.Width * this.designerItem.RenderTransformOrigin.X,
                                      this.designerItem.Height * this.designerItem.RenderTransformOrigin.Y));
                        startPoint = e.GetPosition(this.designerCanvas);

                        this.rotateTransform = this.designerItem.RenderTransform as RotateTransform;
                        if (this.rotateTransform == null)
                        {
                            this.designerItem.RenderTransform = new RotateTransform();
                            this.initialAngle = 0;
                        }
                        else
                        {
                            this.initialAngle = this.rotateTransform.Angle;
                        }
                    }
                }
            }
        }


    }
}
