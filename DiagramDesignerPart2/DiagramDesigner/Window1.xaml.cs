
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace DiagramDesigner
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void OnClickNew(object sender, RoutedEventArgs args)
        {
            MyDesignerCanvas.MyCols = 2;
            MyDesignerCanvas.MyRows = 2;
            //MyDesignerCanvas.Children.Clear();
        }

        private void OnClickRotateLeft(object sender, RoutedEventArgs args)
        {
            Rotate(-90);
        }

        private void OnClickRotateRight(object sender, RoutedEventArgs args)
        {
            Rotate(90);
        }
        private void OnClickSave(object sender, RoutedEventArgs e)
        {
            DesignerItem item = new DesignerItem();
            item.Style = this.TryFindResource("DesignerItemStyle") as Style;
            item.Content = new Button() { IsHitTestVisible = false, Content = "China", Background = Brushes.Gray };
            item.Width = 100;
            item.Height = 100;
            item.SetValue(Canvas.LeftProperty, 10.0);
            item.SetValue(Canvas.TopProperty, 10.0);
            this.MyDesignerCanvas.Children.Add(item);
        }

        // sort of a hack, only values of 90 (right) or -90 (left) make sense
        // for demo purposes only
        private void Rotate(double angle)
        {
            foreach (DesignerItem item in MyDesignerCanvas.SelectedItems)
            {
                FrameworkElement element = item.Content as FrameworkElement;
                if (element != null)
                {
                    RotateTransform rotateTransform = element.LayoutTransform as RotateTransform;
                    if (rotateTransform == null)
                    {
                        rotateTransform = new RotateTransform();
                        element.LayoutTransform = rotateTransform;
                    }

                    rotateTransform.Angle = (rotateTransform.Angle + angle) % 360;
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - (item.Height - item.Width) / 2);
                    Canvas.SetTop(item, Canvas.GetTop(item) - (item.Width - item.Height) / 2);
                    double width = item.Width;
                    item.Width = item.Height;
                    item.Height = width;
                }
            }
        }

    }
}
