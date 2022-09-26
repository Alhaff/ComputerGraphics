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
        #region Variables
            private Dictionary<Key, Action> keysAndActions = new Dictionary<Key, Action>();
            private int cameraMoveStepInPixels = 25; 
            private double cameraScaleCoef = 0.5;
        #endregion
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
        private AffineTransformation Affine { get; set; }
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
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Move = (MoveTransformation)this.Resources["move"];
            Rotate = (RotateTransformation)this.Resources["rotate"];
            Affine = (AffineTransformation)this.Resources["affine"];
            Project = (ProjectiveTransformation)this.Resources["project"];
            LinearTransformation = new TransformationConnector(Rotate, Move);
            Transformation = new TransformationConnector(Affine, Project);
            Project.R0 = new Vector3(0, 0, 1500);
            Project.Rx = new Vector3(-1000, 0, 1);
            Project.Ry = new Vector3(0, 1000, 1);
            keysAndActions.Add(System.Windows.Input.Key.A, KeyA);
            keysAndActions.Add(System.Windows.Input.Key.D, KeyD);
            keysAndActions.Add(System.Windows.Input.Key.W, KeyW);
            keysAndActions.Add(System.Windows.Input.Key.S, KeyS);
        }
        void KeyA()
        {
            Pl.Dx += cameraMoveStepInPixels;
        }
        void KeyD()
        {
            Pl.Dx -= cameraMoveStepInPixels;
        }
        void KeyW()
        {
            Pl.Dy += cameraMoveStepInPixels;
        }
        void KeyS()
        {
            Pl.Dy -= cameraMoveStepInPixels;
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
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {

            if (ElapsedMillisecondsSinceLastTick >= 20)
            { 
                Draw();
                PreviousTick = Timer.Elapsed;
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
            var temp = new CoordinateAxis(100,100);
            temp.IsPlaneTransfromMe = false;
            MyDrawing.TransformMe += LinearTransformation;
            //temp.TransformMe += Athene;
            //temp.MyColor = System.Windows.Media.Color.FromRgb(100, 0, 0);
            //Pl.Transform += Transformation;
            Pl.Transform += Affine;
            Axes.MyColor = System.Windows.Media.Color.FromRgb(0,180, 0);
            //Axes.TransformMe += Athene;
            //MyDrawing.TransformMe += Athene;
            Pl.AddObject(temp);
            Pl.AddObject(Axes);
            Pl.AddObject(MyDrawing);
           
        }

        private void ToDafaultValue_Click(object sender, RoutedEventArgs e)
        {
            MyDrawing.ReturnToDefaultSize();
            Pl.ScaleCoef = 1;
            Pl.Dx = 0;
            Pl.Dy = 0;
        }

        private void CancelProjectionTransformation_Click(object sender, RoutedEventArgs e)
        {
            if (Pl.Transform.GetInvocationList().Contains(Transformation))
            { 
                Pl.Transform -= Transformation;
                Pl.Transform += Affine;
                //Pl.BitmapWidth -= 50;
                //Pl.BitmapHeight -= 50;
            }
        }

        private void ApplyProjectionTransformation_Click(object sender, RoutedEventArgs e)
        {  if (!Pl.Transform.GetInvocationList().Contains(Transformation))
            {
                Pl.Transform -= Affine;
                Pl.Transform += Transformation;
            }
        }

        private void ViewPort_KeyDown(object sender, KeyEventArgs e)
        {
           foreach(var key in keysAndActions.Keys)
            {
                if(e.Key.Equals(key))
                {
                    keysAndActions[key].Invoke();
                }
            }
        }

        private void ViewPort_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(e.Delta < 0)
            {
                Pl.ScaleCoef += cameraScaleCoef;
            }
            else
            {
                Pl.ScaleCoef -= cameraScaleCoef;
            }
        }

        private void ViewPort_MouseEnter(object sender, MouseEventArgs e)
        {
            Keyboard.Focus(ViewPort);
        }
    }
}
