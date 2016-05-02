using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DiagramDesigner
{
    public class MoveThumb : Thumb
    {
        private DesignerItem designerItem;
        private DesignerCanvas designerCanvas;

        public MoveThumb()
        {
            DragStarted += new DragStartedEventHandler(this.MoveThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as DesignerItem;

            if (this.designerItem != null)
            {
                this.designerCanvas = VisualTreeHelper.GetParent(this.designerItem) as DesignerCanvas;
            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null && this.designerCanvas != null && this.designerItem.IsSelected)
            {
                double minLeft = double.MaxValue;
                double minTop = double.MaxValue;

                foreach (DesignerItem item in this.designerCanvas.SelectedItems)
                {
                    minLeft = Math.Min(Canvas.GetLeft(item), minLeft);
                    minTop = Math.Min(Canvas.GetTop(item), minTop);
                }

                double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                foreach (DesignerItem item in this.designerCanvas.SelectedItems)
                {
                    double left = Canvas.GetLeft(item) + deltaHorizontal;
                    double top = Canvas.GetTop(item) + deltaVertical;
                    if (left + item.ActualWidth < this.designerCanvas.ActualWidth)
                        Canvas.SetLeft(item, left);
                    if (top + item.ActualHeight < this.designerCanvas.ActualHeight)
                        Canvas.SetTop(item, top);
                }

                //this.designerCanvas.InvalidateMeasure();
                e.Handled = true;
            }
        }
    }
}
