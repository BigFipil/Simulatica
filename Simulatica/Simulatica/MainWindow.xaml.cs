using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simulatica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool collapsed;
        public bool IsMenuCollapsed 
        {
            get
            {
                return collapsed;
            }
            set
            {
                if(collapsed != value)
                {
                    collapsed = value;
                    OnToggle();
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            IsMenuCollapsed = true;
        }

        private void BorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }


        private void ToggleNavMenu(object sender, RoutedEventArgs e)
        {
            IsMenuCollapsed = !IsMenuCollapsed;

            //if (!IsMenuCollapsed)
            //{
            //    var myDoubleAnimation = new ThicknessAnimation();
            //    myDoubleAnimation.From = new Thickness(0, 10, 0, 4);
            //    myDoubleAnimation.To = new Thickness(200, 10, 0, 4); // TODO: binding 200 
            //    myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            //    Storyboard s = new Storyboard();
            //    s.Children.Add(myDoubleAnimation);
            //    Storyboard.SetTargetProperty(More, new PropertyPath(Button.MarginProperty));
            //    //s.Begin();
            //    //Storyboard.SetTarget(myDoubleAnimation, new DependencyObject(More.Margin));
            //    //MainGrid.ColumnDefinitions[0].Width = new GridLength(210);
            //}
            //else
            //{
            //    var myDoubleAnimation = new DoubleAnimation();
            //    myDoubleAnimation.From = 210;
            //    myDoubleAnimation.To = 60;
            //    myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            //    //Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(More.Margin.Left));
            //    //MainGrid.ColumnDefinitions[0].Width = new GridLength(60);
            //}
        }

        public event PropertyChangedEventHandler propChange;
        private void OnToggle([CallerMemberName] string propertyName = null)
        {
            propChange?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void ResizeMainWindowWidth(object sender, MouseButtonEventArgs e)
        {
            Width = (double) e.GetPosition(this).X + 5;
        }
        private void ResizeMainWindowHeight(object sender, MouseButtonEventArgs e)
        {
            Height = (double)e.GetPosition(this).Y + 5;
        }
    }
}
