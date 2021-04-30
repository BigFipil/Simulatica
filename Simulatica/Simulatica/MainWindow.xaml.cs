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
        public bool IsMenuCollapsed { get; set; }

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

            var target = NavBar;
            var Logotype = Logo;

            ThicknessAnimation marginAnimation = new ThicknessAnimation();
            marginAnimation.Duration = TimeSpan.FromSeconds(0.3);
            marginAnimation.FillBehavior = FillBehavior.HoldEnd;
            marginAnimation.AccelerationRatio = 0.8;

            DoubleAnimation widthAnimation = new DoubleAnimation();
            widthAnimation.Duration = TimeSpan.FromSeconds(0.3);
            widthAnimation.FillBehavior = FillBehavior.HoldEnd;
            widthAnimation.AccelerationRatio = 0.8;

            if (!IsMenuCollapsed)
            {
                marginAnimation.From = new Thickness(2);
                marginAnimation.To = new Thickness(54, 4, 54, 4); // TODO: binding 200

                widthAnimation.From = 60;
                widthAnimation.To = 240;
            }
            else
            {
                marginAnimation.From = new Thickness(54, 4, 54, 4);
                marginAnimation.To = new Thickness(2);

                widthAnimation.From = 240;
                widthAnimation.To = 60;
            }


            target.BeginAnimation(WidthProperty, widthAnimation);
            Logotype.BeginAnimation(MarginProperty, marginAnimation);
            //t2.BeginAnimation(ScaleTransform.ScaleYProperty, anim2);
            //t2.BeginAnimation(TranslateTransform.XProperty, anim2);
            //t2.BeginAnimation(ScaleTransform.ScaleYProperty, anim2);
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
