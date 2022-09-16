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
using CooridnateGrid.ExtensionsClasses;
using CooridnateGrid.ConverterClasses;
using System.Numerics;
using System.Threading;
using CooridnateGrid.Transformation;

namespace CooridnateGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int BitmapWidth { get; set; }
        private int BitmapHeight { get; set; }
        public MyPlane Pl { get; set; }
        private CoordinateAxis Axes { get; set; }
        private Lab1Drawing MyDrawing { get; set; }

        private MoveTransformation Move { get; set; }
        private RotateTransformation Rotate { get; set; }
        private TransformationConnector LinearTransformation { get; set; }
        private TransformationConnector Transformation { get; set; }
        private AffineTransformation Athene { get; set; }
        private ProjectiveTransformation Project { get; set; }
        private bool reverse = false;
        bool IsPressed = false;
        public MainWindow()
        {
            InitializeComponent();
            Move = (MoveTransformation)this.Resources["move"];
            Rotate = (RotateTransformation)this.Resources["rotate"];
            Athene = (AffineTransformation)this.Resources["athene"];
            Project = (ProjectiveTransformation)this.Resources["project"];
            LinearTransformation = new TransformationConnector(Rotate, Move);
            Transformation = new TransformationConnector(Athene, Project);
            Project.R0 = new Vector3(0, 0, 1500);
            Project.Rx = new Vector3(-1000, 0, 1);
            Project.Ry = new Vector3(0, 1000, 1);
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
            if (IsPressed)
            {
                AutoRotate();
            }
           
        }

        private void AutoRotate()
        {
             if (Pl.Transform.GetInvocationList().Contains(Transformation))
            {
                Thread.Sleep(100);
                if (!reverse)
                {
                    Project.Rx = Project.Rx + new Vector3(10, 0, 0);
                }
                else
                {
                    Project.Rx = Project.Rx - new Vector3(10, 0, 0);
                }
                if (Project.Rx.X >= 1000)
                {
                    reverse = true;
                }
                else if (Project.Rx.X <= -1000)
                {
                    reverse = false;

                }
            }
        }
        private void CreatePlane()
        {
            Pl = new MyPlane(BitmapWidth, BitmapHeight, 20);
            Axes = new CoordinateAxis(BitmapWidth / Pl.StepInPixels, BitmapHeight / Pl.StepInPixels);
            MyDrawing = ((Lab1Drawing)this.Resources["mainObj"]);
            MyDrawing.TransformMe += LinearTransformation;
            Pl.Transform += Athene;
            //Pl.Transform += Transformation;
            Pl.AddObject(Axes);
            Pl.AddObject(MyDrawing);
        }

        private void ToDafaultValue_Click(object sender, RoutedEventArgs e)
        {
            MyDrawing.ReturnToDefaultSize();
        }

        private void CancelProjectionTransformation_Click(object sender, RoutedEventArgs e)
        {
            if (Pl.Transform.GetInvocationList().Contains(Transformation))
            {
                Pl.Transform -= Transformation;
                Pl.Transform += Athene;
               
            }
        }

        private void ApplyProjectionTransformation_Click(object sender, RoutedEventArgs e)
        {  if (!Pl.Transform.GetInvocationList().Contains(Transformation))
            {
                Pl.Transform -= Athene;
                Pl.Transform += Transformation;
            }
        }

       
        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            IsPressed = !IsPressed;
        }
    }
}
