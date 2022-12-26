using CooridnateGrid.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CooridnateGrid
{
    public partial class MainWindow : Window
    {
       // private Bezier5Fragment Bz { get; set; }
        private string defaultPath = "../../../Resources/";
        private Bezier5CurvesDrawer BzDrawer { get; set; }
        private Bezier5CurvesDrawer Pikachu { get; set; }
        private List<List<Vector3>> DiffCurve { get; set; }
        TextBlock chosePointText;
        TextBlock controlButtonText;
        private TimeSpan CurveAnimationPreviousTick { get; set; }
        public float CurveAnimationElapsedMillisecondsSinceLastTick
        {
            get
            {
                return (float)(Timer.Elapsed - CurveAnimationPreviousTick).TotalMilliseconds;
            }
        }
        void KeyEsc()
        {
            TempCanvas.MouseDown -= BezierDrawer;
            BzDrawer.ConectLastAndFirstPoint();
            BzDrawer.ApplyTransfromationToInerPoint();
            TempCanvas.Children.Remove(controlButtonText);
        }

        void KeyP()
        {
            TempCanvas.MouseDown -= BezierDrawer;
            BzDrawer.ApplyTransfromationToInerPoint();
            TempCanvas.Children.Remove(controlButtonText);
        }
        private void Lab3_Selected(object sender, RoutedEventArgs e)
        {
            AffineTab.Visibility = Visibility.Visible;
            ProjectiveTab.Visibility = Visibility.Visible;
            Linear2D.Visibility = Visibility.Visible;
            Linear3D.Visibility = Visibility.Collapsed;
            CentralTab.Visibility = Visibility.Collapsed;
            if (BzDrawer == null)
            {
                BzDrawer = new Bezier5CurvesDrawer();
                BzDrawer.MyColor = Colors.Blue;
                BzDrawer.ReadBinaryPointsFromFile(Pl, TempCanvas, defaultPath + "palm.dat");
                BzDrawer.TransformMe += LinearTransformation;
                chosePointText = new TextBlock();
                chosePointText.Text = "Оберіть точку на екрані:";
                chosePointText.HorizontalAlignment = HorizontalAlignment.Center;
                controlButtonText = new TextBlock();
                controlButtonText.Text = "Натисніть P для паузи \nНатисність ЕSC для завершення малювання";
                controlButtonText.FontSize = 18;
                controlButtonText.HorizontalAlignment = HorizontalAlignment.Center;
                controlButtonText.Background = new System.Windows.Media.SolidColorBrush(Colors.White);
                BeforeAnimationStart();


            }
            else
            {
                BzDrawer.TransformMe += LinearTransformation;
                if(Lab6Transformation != null)
                BzDrawer.TransformMe -= Lab6Transformation;
            }
            BzDrawer.MyColor = Colors.Blue;
            if(!Pl.Transform.GetInvocationList().Contains(Affine))
                Pl.Transform += Affine;
            if (BaseProjection != null)
                Pl.Transform -= BaseProjection;
           
            Pl.AddObject(BzDrawer);
            Pl.RemoveObject(Axes);
            keysAndActions.Add(System.Windows.Input.Key.Escape, KeyEsc);
            keysAndActions.Add(System.Windows.Input.Key.P, KeyP);


        }
        private void LoadImage(Vector3 point)
        {
            var p = Pl.ToPlaneCoord(point);
            ImageFromFile background = new ImageFromFile(new Uri("../../../Resources/pikachu.png", UriKind.Relative), p);
            Pl.AddObjectOnFirstPlace(background);
        }
        private void Lab3_Unselected(object sender, RoutedEventArgs e)
        {
            //Pl.RemoveObject(Bz);
            BzDrawer.RemoveAllCurvesPointsFromPlane(TempCanvas);
            BzDrawer.RemoveAllHexagonPointsFromPlane(TempCanvas);
            Pl.RemoveObject(BzDrawer);
            keysAndActions.Remove(System.Windows.Input.Key.Escape);
            keysAndActions.Remove(System.Windows.Input.Key.P);
            // Pl.RemoveFirstObject();

        }
        private void LoadBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            TempCanvas.MouseDown += LoadBackgroundButton_MouseDown;
            TempCanvas.MouseMove += LoadBackgroundButton_MouseMove;
            TempCanvas.Children.Add(chosePointText);
        }

        private void DeleteBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            Pl.RemoveFirstObject();
        }

        private void LoadBackgroundButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(TempCanvas);
            LoadImage(new Vector3((float)pos.X, (float)pos.Y, 1));
            TempCanvas.MouseDown -= LoadBackgroundButton_MouseDown;
            TempCanvas.MouseMove -= LoadBackgroundButton_MouseMove;
            TempCanvas.Children.Remove(chosePointText);
        }
       
        private void LoadBackgroundButton_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(TempCanvas);
            Canvas.SetLeft(chosePointText, pos.X + 15);
            Canvas.SetTop(chosePointText, pos.Y -15);
           
        }

        private void BezierDrawer(object sender, MouseButtonEventArgs e)
        {
            BzDrawer.Bezier5Curve_MouseDown(Pl, TempCanvas, sender, e);
        }
        private void StartDrawCurveButton_Click(object sender, RoutedEventArgs e)
        {
            TempCanvas.MouseDown += BezierDrawer;
            TempCanvas.Children.Add(controlButtonText);
            Canvas.SetLeft(controlButtonText, 0);
            Canvas.SetTop(controlButtonText, 0);
        }

        private void ShowCarcasePoint_Checked(object sender, RoutedEventArgs e)
        {
            BzDrawer.AddAllHexagonPointsOnPlane(TempCanvas);
        }

        private void ShowCarcasePoint_Unchecked(object sender, RoutedEventArgs e)
        {
            BzDrawer.RemoveAllHexagonPointsFromPlane(TempCanvas);
        }

        private void ShowPoint_Checked(object sender, RoutedEventArgs e)
        {
            if(BzDrawer != null)
            BzDrawer.AddAllCurvesPointsOnPlane(TempCanvas);
        }

        private void ShowPoint_Unchecked(object sender, RoutedEventArgs e)
        {
            BzDrawer.RemoveAllCurvesPointsFromPlane(TempCanvas);
        }

        private void WriteToFileButton_Click(object sender, RoutedEventArgs e)
        {
            if(fileName.Text != null)
            {
                string path = defaultPath + fileName.Text + ".dat";
                BzDrawer.WriteBinaryPointsToFile(path);
            }
        }

        private void ReadFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (fileName.Text != null)
            {
                string path = defaultPath + fileName.Text + ".dat";
                BzDrawer.ReadBinaryPointsFromFile(Pl, TempCanvas,path);
            }
            BzDrawer.ApplyTransfromationToInerPoint();
            BeforeAnimationStart();
        }

        private void DeleteCurrentCurveButton_Click(object sender, RoutedEventArgs e)
        {
            BzDrawer.ClearCurrentCurve(TempCanvas);
        }

       
        private IEnumerable<Vector3> GetDiffVector(Bezier5Curve start, Bezier5Curve end)
        {
            Func<Vector3, Vector3, Vector3> Diff = (v1, v2) => (v2 - v1) / (int)AnimationSlider.Maximum;
            for (int i = 0; i < 2; i++)
            {
                yield return Diff(start.StartEndPoints[i].Center, end.StartEndPoints[i].Center);
            }
            for(int i =0; i < 4; i++)
            {
                yield return Diff(start.MiddlePoints[i].Center, end.MiddlePoints[i].Center);
            }
        }
        private void BeforeAnimationStart()
        {
            if (Pikachu == null)
            {
                Pikachu = new Bezier5CurvesDrawer();
                Pikachu.ReadBinaryPointsFromFile(Pl, TempCanvas, defaultPath + "pikachu.dat");
                Pikachu.RemoveAllCurvesPointsFromPlane(TempCanvas);
                
            }
            if(DiffCurve == null)
            {
                DiffCurve = new List<List<Vector3>>();
            }
            if(BzDrawer.FragmentCount == Pikachu.FragmentCount)
            {
                DiffCurve.Clear();
                for(int i =0; i < BzDrawer.FragmentCount; i++)
                {
                    var curve1 = BzDrawer.Curves[i];
                    var curve2 = Pikachu.Curves[i];
                    DiffCurve.Add(GetDiffVector(curve1, curve2).ToList());
                }
            }
        }

        private void AddDiff(Bezier5Curve curve, List<Vector3> diff)
        {
            curve.StartEndPoints[0].Center += diff[0];
            //curve.StartEndPoints[1].Center += diff[1];
            for (int i = 0; i < 4; i++)
            {
                curve.MiddlePoints[i].Center += diff[i+2];
            }
            
        }
        private void SubDiff(Bezier5Curve curve, List<Vector3> diff)
        {
            curve.StartEndPoints[0].Center -= diff[0];
            //curve.StartEndPoints[1].Center -= diff[1];
            for (int i = 0; i < 4; i++)
            {
                curve.MiddlePoints[i].Center -= diff[i + 2];
            }
        }
        private void AnimationTick(Action<Bezier5Curve, List<Vector3>> f)
        {
            if(DiffCurve.Count == BzDrawer.FragmentCount)
            {
                for(int i = 0; i < BzDrawer.FragmentCount;i++)
                {
                    f(BzDrawer.Curves[i], DiffCurve[i]);
                }
            }
        }
        int AnimationCounter = 1;
        bool IDirection = true;
        private void CurveAnimation(object? sender, EventArgs e)
        {
            if (CurveAnimationElapsedMillisecondsSinceLastTick >= 25)
            {
                if (IDirection)
                {
                    AnimationSlider.Value++;
                }
                else
                {
                    AnimationSlider.Value--;
                }
                if (AnimationSlider.Value == AnimationSlider.Maximum)
                {
                    CompositionTarget.Rendering -= CurveAnimation;
                    IDirection = false;
                   
                }
                if (AnimationSlider.Value == AnimationSlider.Minimum)
                {
                    CompositionTarget.Rendering -= CurveAnimation;
                    IDirection = true;
                    
                }
                CurveAnimationPreviousTick = Timer.Elapsed;
            }
        }
        bool IRunAnimation = true;
        private void StartAnimation_Click(object sender, RoutedEventArgs e)
        {
            if (Pikachu == null || BzDrawer.FragmentCount != Pikachu.FragmentCount)
            {
                BeforeAnimationStart();
            }
            if (IRunAnimation)
            {
                CompositionTarget.Rendering += CurveAnimation;
                IRunAnimation = false;
                StartAnimation.Background = new System.Windows.Media.SolidColorBrush(Colors.Purple);
                StartAnimation.Foreground = new System.Windows.Media.SolidColorBrush(Colors.White);
            }
            else
            {
                CompositionTarget.Rendering += CurveAnimation;
                IRunAnimation = true;
                StartAnimation.Background = new System.Windows.Media.SolidColorBrush(Colors.LightGray);
                StartAnimation.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            }
        }
        private int PrevValue = 1;
        private void AnimationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           
            int Diff = (int)AnimationSlider.Value - PrevValue;
            PrevValue = (int)AnimationSlider.Value;
            for (int i = 0; i < Math.Abs(Diff); i++)
            {
                if(Diff > 0)
                {
                    AnimationTick(AddDiff);
                }
                else if(Diff < 0)
                {
                    AnimationTick(SubDiff);
                }
            }
        }
    }
}
