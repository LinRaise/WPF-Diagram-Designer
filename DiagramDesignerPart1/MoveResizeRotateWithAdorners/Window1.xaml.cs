using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DiagramDesigner
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs args)
        {
            CheckBox selectionCheckBox = sender as CheckBox;
            if (selectionCheckBox != null && selectionCheckBox.IsChecked == true)
            {
                foreach (Control child in DesignerCanvas.Children)
                {
                    Selector.SetIsSelected(child, true);
                }
            }
            else
            {
                foreach (Control child in DesignerCanvas.Children)
                {
                    Selector.SetIsSelected(child, false);
                }
            }
        }
    }
}
