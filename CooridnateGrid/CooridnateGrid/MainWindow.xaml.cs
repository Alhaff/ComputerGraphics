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
        private CoordinateAxis UntransformAxes { get; set; }
        private MoveTransformation Move { get; set; }
        private Offset3DTransformation Offset { get; set; }
        private RotateTransformation Rotate { get; set; }
        private Rotate3DTransformations Rotate3D { get; set; }
        private TransformationConnector LinearTransformation { get; set; }
        private TransformationConnector Linear3DTransformation { get; set; }
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
            Offset = (Offset3DTransformation)this.Resources["Offset3D"];
            Rotate = (RotateTransformation)this.Resources["rotate"];
            Rotate3D = (Rotate3DTransformations)this.Resources["Rotate3D"];
            Affine = (AffineTransformation)this.Resources["affine"];
            Project = (ProjectiveTransformation)this.Resources["project"];
            centralProjectionTransformation = (CentralProjectionTransformation)this.Resources["CentralProjection"];
            AxisesRotation = (Rotate3DTransformations)this.Resources["RotateAxis"];
            LinearTransformation = new TransformationConnector(Move, Rotate);
            Transformation = new TransformationConnector(Affine, Project);
           // Project.RxPoint.MyColor = System.Windows.Media.Color.FromRgb(0, 220, 255);
           // Project.RyPoint.MyColor = System.Windows.Media.Color.FromRgb(0, 220, 255);
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
                
                if (Click1)
                {
                    Rotate3D.SetXAngLe += 0.1;
                    if (Rotate3D.AngleX >= 360)
                        Rotate3D.SetXAngLe = 0;
                }
                if (Click2)
                {
                    Rotate3D.SetYAngLe += 0.1;
                    if (Rotate3D.AngleY >= 360)
                        Rotate3D.SetYAngLe = 0;
                }
                if (Click3)
                {
                    Rotate3D.SetZAngLe += 0.1;
                    if (Rotate3D.AngleZ >= 360)
                        Rotate3D.SetZAngLe = 0;
                }
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
            Axes = new CoordinateAxis(200,200);
            Pl.Transform += Affine;
            Axes.MyColor = System.Windows.Media.Color.FromRgb(0,180, 0);
            Pl.AddObject(Axes);
            Rotate.CenterPoint = new PointOnPlane(Pl, new Vector3(0, 0, 1));
            Rotate.CenterPoint.IsPlaneTransfromMe = true;
            Rotate.CenterPoint.AddPointOnCanvas(TempCanvas);
            Project.R0Point = new PointOnPlane(Pl, new Vector3(0, 0, 1500));
            Project.R0Point.MyColor = Colors.Silver;
            Project.R0Point.IsPlaneTransfromMe = false;
            Project.RxPoint = new PointOnPlane(Pl, new Vector3(10, 0, 100));
            Project.RxPoint.MyColor = Colors.Silver;
            Project.RxPoint.IsPlaneTransfromMe = false;
            Project.RyPoint = new PointOnPlane(Pl, new Vector3(0, 100, 100));
            Project.RyPoint.MyColor = Colors.Silver;
            Project.RyPoint.IsPlaneTransfromMe = false;
            Pl.AddObject(Rotate.CenterPoint);
            AddLab1Obj();
        }
       
        private void ToDafaultValue_Click(object sender, RoutedEventArgs e)
        {
            if (MyDrawing != null)
            {
                MyDrawing.ReturnToDefaultSize();
            }
            if(MyEpicycloid != null)
            {
                MyEpicycloid.ReturnToDefaultSize();
            }
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
                Pl.RemoveObject(Project.R0Point, Project.RxPoint, Project.RyPoint);
                Project.R0Point.RemovePointFromCanvas(TempCanvas);
                Project.RxPoint.RemovePointFromCanvas(TempCanvas);
                Project.RyPoint.RemovePointFromCanvas(TempCanvas);
                //Pl.AddObject(Rotate.CenterPoint);
                //Rotate.CenterPoint.AddPointOnCanvas(TempCanvas);


            }
        }

        private void ApplyProjectionTransformation_Click(object sender, RoutedEventArgs e)
        {  if (!Pl.Transform.GetInvocationList().Contains(Transformation))
            {
                Pl.Transform -= Affine;
                Pl.Transform += Transformation;
                //Pl.RemoveObject(Rotate.CenterPoint);
                //Rotate.CenterPoint.RemovePointFromCanvas(TempCanvas);
                Pl.AddObject(Project.R0Point, Project.RxPoint, Project.RyPoint);
                Project.R0Point.AddPointOnCanvas(TempCanvas);
                Project.RxPoint.AddPointOnCanvas(TempCanvas);
                Project.RyPoint.AddPointOnCanvas(TempCanvas);
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
        private void TempCanvas_DragOver(object sender, DragEventArgs e)
        {
            var dropPosition = e.GetPosition(TempCanvas);

            if (e.Data.GetDataPresent(typeof(Ellipse)))
            {
                var point = (Ellipse)e.Data.GetData(typeof(Ellipse));
                {
                    Canvas.SetLeft(point, dropPosition.X - point.Width / 2);
                    Canvas.SetTop(point, dropPosition.Y - point.Height / 2);
                }
            }
        }

        private void TempCanvas_Drop(object sender, DragEventArgs e)
        {
            var dropPosition = e.GetPosition(TempCanvas);

            if (e.Data.GetDataPresent(typeof(Ellipse)))
            {
                var point = (Ellipse)e.Data.GetData(typeof(Ellipse));
                {
                    Canvas.SetLeft(point, dropPosition.X - point.Width / 2);
                    Canvas.SetTop(point, dropPosition.Y - point.Height / 2);
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ViewPort_MouseMove(object sender, MouseEventArgs e)
        {

        }
        bool Click1 = false;
        private void RotateOnXaxis_Click(object sender, RoutedEventArgs e)
        {
            Click1 = !Click1;
            
        }
        bool Click2 = false;
        private void RotateOnYaxis_Click(object sender, RoutedEventArgs e)
        {
            Click2 = !Click2;
        }
        bool Click3 = false;
        private void RotateOnZaxis_Click(object sender, RoutedEventArgs e)
        {
            Click3 = !Click3;
        }

      
    }
}
