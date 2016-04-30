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
            Control designerItem = this.DataContext as Control;

            if (designerItem != null)
            {
                double left = Canvas.GetLeft(designerItem);
                double top = Canvas.GetTop(designerItem);

                Canvas.SetLeft(designerItem, left + e.HorizontalChange);
                Canvas.SetTop(designerItem, top + e.VerticalChange);
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
