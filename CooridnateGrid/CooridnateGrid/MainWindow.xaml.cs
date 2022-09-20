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
using System.Drawing;
using System.Diagnostics;

namespace CooridnateGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Propreties
        private int BitmapWidth { get; set; }
        private int BitmapHeight { get; set; }
        private WriteableBitmap WR { get; set; }
        public MyPlane Pl { get; set; }
        private CoordinateAxis Axes { get; set; }
        private Lab1Drawing MyDrawing { get; set; }

        private MoveTransformation Move { get; set; }
        private RotateTransformation Rotate { get; set; }
        private TransformationConnector LinearTransformation { get; set; }
        private TransformationConnector Transformation { get; set; }
        private AffineTransformation Athene { get; set; }
        private ProjectiveTransformation Project { get; set; }

        public Stopwatch Timer { get; set; } = new Stopwatch();

        private TimeSpan PreviousTick { get; set; }

        public float ElapsedMillisecondsSinceLastTick
        {
            get
            {
                return (float)(Timer.Elapsed - PreviousTick).TotalMilliseconds;
            }
        }

        private bool Reverse { get; set; } = false;

        bool IsPressed { get; set; } = false;
        #endregion

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
            ViewPort.Source = WR;
            Timer.Start();
            PreviousTick = Timer.Elapsed;
            Draw();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }
        private void Draw()
        {
            var wr = WR;
            var w = wr.PixelWidth;
            var h = wr.PixelHeight;
            var stride = wr.BackBufferStride;
            var pixelPtr = wr.BackBuffer;
            var bm2 = new Bitmap(w, h, stride, CoordinatePlane.MyPlane.ConvertPixelFormat(wr.Format), pixelPtr);
            wr.Lock();
            using (var g = Graphics.FromImage(bm2))
            {
                Pl.Draw(g);
            }
            wr.AddDirtyRect(new Int32Rect(0, 0, BitmapWidth, BitmapHeight));
            wr.Unlock();
           
            if (IsPressed)
            {
                AutoRotate();
            }
            
        }

        private  void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            if (ElapsedMillisecondsSinceLastTick >= 20)
            {
                Draw();
                PreviousTick = Timer.Elapsed;
            }
        }
   

        private void AutoRotate()
        {
             if (Pl.Transform.GetInvocationList().Contains(Transformation))
            {
              
                if (!Reverse)
                {
                    Project.Rx = Project.Rx + new Vector3(10, 0, 0);
                }
                else
                {
                    Project.Rx = Project.Rx - new Vector3(10, 0, 0);
                }
                if (Project.Rx.X >= 1000)
                {
                    Reverse = true;
                }
                else if (Project.Rx.X <= -1000)
                {
                    Reverse = false;

                }
            }
        }
        private void CreatePlane()
        {
            Pl = new MyPlane(BitmapWidth, BitmapHeight, 20);
            WR = BitmapFactory.New(BitmapWidth, BitmapHeight);
            Binding bind = new Binding();
            bind.Source = Pl;
            bind.Path = new PropertyPath("StepInPixels");
            StepInPixel.SetBinding(TextBox.TextProperty, bind);
            Axes = new CoordinateAxis(Pl);
            MyDrawing = ((Lab1Drawing)this.Resources["mainObj"]);
           // var temp = new CoordinateAxis(BitmapWidth / Pl.StepInPixels, BitmapHeight / Pl.StepInPixels);
            MyDrawing.TransformMe += LinearTransformation;
            // temp.TransformMe += Athene;
            //temp.MyColor = Color.FromRgb(255, 0, 0);
            //Pl.Transform += Transformation;
            Pl.Transform += Athene;
            Pl.AddObject(Axes);
            Pl.AddObject(MyDrawing);
            //Pl.AddObject(temp);
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
