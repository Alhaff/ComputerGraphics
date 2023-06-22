using CooridnateGrid.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CooridnateGrid
{
    public partial class MainWindow : Window
    {
        public ZigzagFractal Zigzag { get; set; }

        private void SetupZigzag()
        {

            if(Zigzag == null)
            {
                Zigzag = (ZigzagFractal)this.Resources["MyZigzag"];
                Zigzag.TransformMe += LinearTransformation;
                Zigzag.MyColor = Colors.Black;
            }

            Pl.AddObject(Zigzag);
        }
        private void Lab7_Selected(object sender, RoutedEventArgs e)
        {
            AffineTab.Visibility = Visibility.Visible;
            ProjectiveTab.Visibility = Visibility.Visible;
            Linear2D.Visibility = Visibility.Visible;
            Linear3D.Visibility = Visibility.Collapsed;
            CentralTab.Visibility = Visibility.Collapsed;
            SetupZigzag();
            Pl.RemoveObject(Axes);
        }

        private void Lab7_Unselected(object sender, RoutedEventArgs e)
        {

        }
        float accelerationIncrease = 0;
        private void IncreaseIterationButton_Click(object sender, RoutedEventArgs e)
        {
            if(Zigzag != null)
            {
                accelerationDecrease = 0;
                Zigzag.IterationCount +=1 +  (int)(accelerationIncrease * accelerationIncrease);
                accelerationIncrease += 0.1f;

            }
        }
        float accelerationDecrease = 0;
        private void ReduceIterationButton_Click(object sender, RoutedEventArgs e)
        {
            if (Zigzag != null)
            {
                accelerationIncrease = 0;
                Zigzag.IterationCount -= 1 +  (int)(accelerationDecrease * accelerationDecrease);
                accelerationDecrease += 0.1f;
            }

        }
    }
}
