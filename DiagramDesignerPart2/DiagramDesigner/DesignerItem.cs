using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DiagramDesigner
{
    public class DesignerItem : ContentControl
    {
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
          DependencyProperty.Register("IsSelected", typeof(bool),
                                      typeof(DesignerItem),
                                      new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty MoveThumbTemplateProperty =
            DependencyProperty.RegisterAttached("MoveThumbTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        public static ControlTemplate GetMoveThumbTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(MoveThumbTemplateProperty);
        }

        public static void SetMoveThumbTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(MoveThumbTemplateProperty, value);
        }

        static DesignerItem()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignerItem), new FrameworkPropertyMetadata(typeof(DesignerItem)));
        }

        public DesignerItem()
        {
            this.Loaded += new RoutedEventHandler(this.DesignerItem_Loaded);
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;

            if (designer != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                {
                    this.IsSelected = !this.IsSelected;
                }
                else
                {
                    //if (!this.IsSelected)
                    //{
                    designer.DeselectAll();
                    this.IsSelected = true;
                    //}
                }
            }

            e.Handled = false;
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;
            if (designer != null)
            {
                double h = designer.ActualHeight / designer.MyRows;
                double w = designer.ActualWidth / designer.MyCols;
                double left = DesignerCanvas.GetLeft(this);
                double top = DesignerCanvas.GetTop(this);
                double newleft = System.Math.Floor(left / w) * w;
                double newtop = System.Math.Floor(top / h) * h;
                double newright = System.Math.Ceiling((left + this.ActualWidth) / w) * w;
                double newbottom = System.Math.Ceiling((top + this.ActualHeight) / h) * h;
                DesignerCanvas.SetLeft(this, newleft);
                DesignerCanvas.SetTop(this, newtop);
                this.Width = newright - newleft;
                this.Height = newbottom - newtop;
            }
        }
        private void DesignerItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Template != null)
            {
                ContentPresenter contentPresenter =
                    this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;

                MoveThumb thumb =
                    this.Template.FindName("PART_MoveThumb", this) as MoveThumb;

                MenuItem menu_delete = this.Template.FindName("PART_Delete", this) as MenuItem;
                if (menu_delete != null)
                {
                    //delete the DesignerItem
                    menu_delete.Click += new RoutedEventHandler((x, y) =>
                    {
                        DesignerCanvas designer = (DesignerCanvas)VisualTreeHelper.GetParent(this);
                        if (designer != null)
                        {
                            designer.Children.Remove(this);
                        }
                    });
                }

                if (contentPresenter != null && thumb != null)
                {
                    UIElement contentVisual =
                        VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;

                    if (contentVisual != null)
                    {
                        ControlTemplate template =
                            DesignerItem.GetMoveThumbTemplate(contentVisual) as ControlTemplate;

                        if (template != null)
                        {
                            thumb.Template = template;
                        }
                    }
                }
            }
        }
    }
}
