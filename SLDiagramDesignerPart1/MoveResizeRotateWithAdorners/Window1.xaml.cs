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

namespace DiagramDesigner
{
    public partial class Window1 : UserControl
    {
        ResizeRotateChrome resize = new ResizeRotateChrome();
        public Window1()
        {
            InitializeComponent();
            this.main.Children.Add(resize);
            this.path.MouseLeftButtonDown += new MouseButtonEventHandler(path_MouseLeftButtonDown);
            this.ellipse.MouseLeftButtonDown += new MouseButtonEventHandler(ellipse_MouseLeftButtonDown);
        }

        void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            resize.Decorator(this.ellipse);
        }

        void path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            resize.Decorator(this.path);
        }
    }
}
