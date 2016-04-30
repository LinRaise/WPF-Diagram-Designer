using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Data;

namespace DiagramDesigner
{
    public class ResizeRotateChrome : Control
    {
        public ResizeRotateChrome()
        {
            this.DefaultStyleKey = typeof(ResizeRotateChrome);
        }

        public void Decorator(FrameworkElement element)
        {
            if (element != null)
            {
                this.DataContext = element;

                this.SetBinding(RenderTransformProperty, new System.Windows.Data.Binding("RenderTransform"));
                this.SetBinding(WidthProperty, new System.Windows.Data.Binding("Width") );
                this.SetBinding(HeightProperty, new System.Windows.Data.Binding("Height"));
                this.SetBinding(Canvas.LeftProperty, new System.Windows.Data.Binding("(Canvas.Left)"));
                this.SetBinding(Canvas.TopProperty, new System.Windows.Data.Binding("(Canvas.Top)"));
            }
        }

        public void Hide()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
