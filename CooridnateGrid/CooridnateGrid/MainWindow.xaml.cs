using CooridnateGrid.CoordinatePlane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CooridnateGrid.DrawnObjects;
using System.Numerics;

namespace CooridnateGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int BitmapWidth { get; set; }
        private int BitmapHeight { get; set; }
        public CartesianPlane Pl { get; set; }

        private CoordinateAxis Axes { get; set; }

        private Circle Circle { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ViewPort_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapWidth = (int)this.ViewPortContainer.ActualWidth;
            BitmapHeight = (int)this.ViewPortContainer.ActualHeight;
            CreatePlane();
            ViewPort.Source = Pl.WrBitmap;
            Pl.Draw();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            Pl.Draw();
        }

        private void CreatePlane()
        {
            Pl = new CartesianPlane(BitmapWidth, BitmapHeight, 20);
            Axes = new CoordinateAxis(BitmapWidth / Pl.StepInPixels, BitmapHeight / Pl.StepInPixels);
            Circle = ((Circle)this.Resources["mainCircle"]);
            Circle.MyColor = Color.FromRgb(255, 0, 0);
            Pl.AddObject(Axes);
            Pl.AddObject(Circle);
        }
    }
}
