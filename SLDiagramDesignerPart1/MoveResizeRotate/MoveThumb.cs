using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace DiagramDesigner
{
    [TemplatePart(Name = "thumb", Type = typeof(Thumb))]
    public class MoveThumb : Control
    {
        private Thumb thumb;

        public MoveThumb()
        {
            this.DefaultStyleKey = typeof(MoveThumb);
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ContentControl designerItem = DataContext as ContentControl;

            if (designerItem != null)
            {
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                RotateTransform rotateTransform = designerItem.RenderTransform as RotateTransform;
                if (rotateTransform != null)
                {
                    dragDelta = rotateTransform.Transform(dragDelta);
                }

                Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + dragDelta.X);
                Canvas.SetTop(designerItem, Canvas.GetTop(designerItem) + dragDelta.Y);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            thumb = this.GetTemplateChild("thumb") as Thumb;
            if (thumb != null)
            {
                thumb.DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
            }
        }
    }
}
