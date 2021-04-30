using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simulatica.User_Controls
{
    /// <summary>
    /// Logika interakcji dla klasy ExitBar.xaml
    /// </summary>
    public partial class ExitBar : UserControl
    {
        public ExitBar()
        {
            InitializeComponent();
        }

        void MinimalizeAction(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);

            window.WindowState = WindowState.Minimized;
        }

        void MaximalizeAction(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);

            if (window.WindowState != WindowState.Maximized)
            {
                window.WindowState = WindowState.Maximized;

                window.BorderThickness = new Thickness(5);
            }

            else
            {
                window.WindowState = WindowState.Normal;
                window.BorderThickness = new Thickness(5);
            }
        }

        void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
