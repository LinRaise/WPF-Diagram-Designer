using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace DiagramDesigner
{
    public class DesignerCanvas : Canvas
    {
        #region MyRows MyCols
        public int MyRows
        {
            get { return (int)GetValue(MyRowsProperty); }
            set { SetValue(MyRowsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyRows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyRowsProperty =
            DependencyProperty.Register("MyRows", typeof(int), typeof(DesignerCanvas), new PropertyMetadata(0, new PropertyChangedCallback(MyRowsProperty_Changed)));

        private static void MyRowsProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignerCanvas designerCanvas = (DesignerCanvas)d;
            int myRows = (int)e.NewValue;
            if (myRows > 0 && designerCanvas.MyCols > 0
                && designerCanvas.ActualHeight > 0 && designerCanvas.ActualWidth > 0)
            {
                designerCanvas.BrushGrid();
            }
        }

        public int MyCols
        {
            get { return (int)GetValue(MyColsProperty); }
            set { SetValue(MyColsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyCols.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyColsProperty =
            DependencyProperty.Register("MyCols", typeof(int), typeof(DesignerCanvas), new PropertyMetadata(0, new PropertyChangedCallback(MyColsProperty_Changed)));

        private static void MyColsProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignerCanvas designerCanvas = (DesignerCanvas)d;
            int myCols = (int)e.NewValue;
            if (myCols > 0 && designerCanvas.MyRows > 0
                && designerCanvas.ActualHeight > 0 && designerCanvas.ActualWidth > 0)
            {
                designerCanvas.BrushGrid();
            }
        }
        #endregion

        private void BrushGrid()
        {
            double unitHeiht = this.ActualHeight / MyRows;
            double unitWidth = this.ActualWidth / MyCols;
            DrawingBrush m_gridBrush = new DrawingBrush(new GeometryDrawing(
                new SolidColorBrush(Colors.White),
                     new Pen(new SolidColorBrush(Colors.Gray), 1.0),
                         new RectangleGeometry(new Rect(0, 0, unitWidth, unitHeiht))));
            m_gridBrush.Stretch = Stretch.None;
            m_gridBrush.TileMode = TileMode.Tile;
            m_gridBrush.Viewport = new Rect(0, 0.0, unitWidth, unitHeiht);
            m_gridBrush.ViewportUnits = BrushMappingMode.Absolute;
            this.Background = m_gridBrush;
        }

        private Point? dragStartPoint = null;

        public IEnumerable<DesignerItem> SelectedItems
        {
            get
            {
                var selectedItems = from item in this.Children.OfType<DesignerItem>()
                                    where item.IsSelected == true
                                    select item;

                return selectedItems;
            }
        }

        public void DeselectAll()
        {
            foreach (DesignerItem item in this.SelectedItems)
            {
                item.IsSelected = false;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            BrushGrid();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this)
            {
                this.dragStartPoint = new Point?(e.GetPosition(this));
                this.DeselectAll();
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                this.dragStartPoint = null;
            }

            if (this.dragStartPoint.HasValue)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    RubberbandAdorner adorner = new RubberbandAdorner(this, this.dragStartPoint);
                    if (adorner != null)
                    {
                        adornerLayer.Add(adorner);
                    }
                }

                e.Handled = true;
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            string xamlString = e.Data.GetData("DESIGNER_ITEM") as string;
            if (!String.IsNullOrEmpty(xamlString))
            {
                DesignerItem newItem = null;
                FrameworkElement content = XamlReader.Load(XmlReader.Create(new StringReader(xamlString))) as FrameworkElement;

                if (content != null)
                {
                    newItem = new DesignerItem();
                    newItem.Style = this.TryFindResource("DesignerItemStyle") as Style;
                    newItem.Content = content;

                    Point position = e.GetPosition(this);
                    //if (content.MinHeight != 0 && content.MinWidth != 0)
                    //{
                    //    newItem.Width = content.MinWidth * 2; ;
                    //    newItem.Height = content.MinHeight * 2;
                    //}
                    //else
                    //{
                    //    newItem.Width = 65;
                    //    newItem.Height = 65;
                    //}
                    double unitwidth = this.ActualWidth / MyCols;
                    double unitheight = this.ActualHeight / MyRows;
                    newItem.Width = unitwidth;
                    newItem.Height = unitheight;
                    double left = Math.Floor(position.X / unitwidth) * unitwidth;
                    double top = Math.Floor(position.Y / unitheight) * unitheight;
                    DesignerCanvas.SetLeft(newItem, Math.Max(0, left/*position.X - newItem.Width / 2*/));
                    DesignerCanvas.SetTop(newItem, Math.Max(0, top/*position.Y - newItem.Height / 2*/));
                    this.Children.Add(newItem);

                    this.DeselectAll();
                    newItem.IsSelected = true;
                }

                e.Handled = true;
            }
        }

        //protected override Size MeasureOverride(Size constraint)
        //{
        //    Size size = new Size();
        //    foreach (UIElement element in Children)
        //    {
        //        double left = Canvas.GetLeft(element);
        //        double top = Canvas.GetTop(element);
        //        left = double.IsNaN(left) ? 0 : left;
        //        top = double.IsNaN(top) ? 0 : top;

        //        element.Measure(constraint);

        //        Size desiredSize = element.DesiredSize;
        //        if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
        //        {
        //            size.Width = Math.Max(size.Width, left + desiredSize.Width);
        //            size.Height = Math.Max(size.Height, top + desiredSize.Height);
        //        }
        //    }

        //    // add some extra margin
        //    size.Width += 10;
        //    size.Height += 10;
        //    return size;
        //}
    }
}
