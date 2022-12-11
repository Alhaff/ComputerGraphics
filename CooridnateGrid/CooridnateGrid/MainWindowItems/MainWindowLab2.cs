using CooridnateGrid.DrawnObjects;
using CooridnateGrid.ExtensionsClasses;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CooridnateGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        private bool IsSwitchScaleAnimationDirection = false;
        private bool IsScaleAnimationPressed = false;
        private bool IsRotateAnimationPressed = false;
        private bool IsTangentAnimationPressed = false;
        private bool IsSwitchTangentAnimationDirection = false;
        private double _rotateAnimationTimeInMS = 5;
        private double _tangentAnimationTimeInMS = 5;
        private double _scaleAnimationTimeInMS = 5;
        private double _rotateAnimationAcceleration = 1;
        private double _scaleAnimationAcceleration = 1;
        private double _tangentAnimationAcceleration = 1;
        #endregion

        #region Propreties
        public Epicycloid MyEpicycloid { get; set; } = null;
        EpicycloidaTangentPoint MyEpicycloidaTangentPoint { get; set; } = null;
        #region Timers
        private TimeSpan RotateAnimationPreviousTick { get; set; }
        public float RotateAnimationElapsedMillisecondsSinceLastTick
        {
            get { 
                return (float)(Timer.Elapsed - RotateAnimationPreviousTick).TotalMilliseconds;
            }
        }

        private TimeSpan ScaleAnimationPreviousTick { get; set; }
        public float ScaleAnimationElapsedMillisecondsSinceLastTick
        {
            get
            {
                return (float)(Timer.Elapsed - ScaleAnimationPreviousTick).TotalMilliseconds;
            }
        }

        private TimeSpan TangentAnimationPreviousTick { get; set; }
        public float TangentAnimationElapsedMillisecondsSinceLastTick
        {
            get
            {
                return (float)(Timer.Elapsed - TangentAnimationPreviousTick).TotalMilliseconds;
            }
        }
        #endregion
        #region AnimationsSpeed
        public double RotateAnimationAcceleration
        {
            get { return _rotateAnimationAcceleration; }
            set
            {
                if (value > 0)
                {
                    _rotateAnimationAcceleration = value;
                }
            }
        }

        public double ScaleAnimationAcceleration
        {
            get { return _scaleAnimationAcceleration; }
            set {
                if (value > 0)
                {
                    _scaleAnimationAcceleration = value;
                }
            }
        }

        public double TangentAnimationAcceleration
        {
            get { return _tangentAnimationAcceleration; }
            set {
                if (value > 0)
                {
                    _tangentAnimationAcceleration = value;
                }
            }
        }

        public double RotateAnimationSpeed { get => _rotateAnimationTimeInMS / _rotateAnimationAcceleration; }
        public double ScaleAnimationSpeed { get => _scaleAnimationTimeInMS / _scaleAnimationAcceleration; }
        public double TangentAnimationSpeed { get => _tangentAnimationTimeInMS / _tangentAnimationAcceleration; }
        #endregion
        #endregion
        private void Bind(TextBox item,object bindObj, string propretyName, IValueConverter valueConvertor = null)
        {
            Binding bind = new Binding();
            bind.Source = bindObj;
            bind.Path = new PropertyPath(propretyName);
            if(valueConvertor != null) bind.Converter = valueConvertor;
            bind.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            item.SetBinding(TextBox.TextProperty, bind);
        }
        
        private void Lab2_Selected(object sender, RoutedEventArgs e)
        {
            if (Pl != null)
            {
                if (MyEpicycloid == null)
                {
                    MyEpicycloid = (Epicycloid)this.Resources["epicycloid"];
                    MyEpicycloid.TransformMe += LinearTransformation;
                    Bind(ScaleSpeed,this, "ScaleAnimationAcceleration");
                    Bind(RotateSpeed, this, "RotateAnimationAcceleration");
                    Bind(TangentSpeed, this, "TangentAnimationAcceleration");
                }
              
                RotateAnimationPreviousTick = Timer.Elapsed;
                Pl.AddObject(MyEpicycloid);
            }
        }

        private void Lab2_Unselected(object sender, RoutedEventArgs e)
        {
            if (Pl != null)
            {
                Pl.RemoveObject(MyEpicycloid);
            }
        }

      
        private void Lab2Tangent_Selected(object sender, RoutedEventArgs e)
        {
            if (Pl != null)
            {
                if (MyEpicycloidaTangentPoint == null)
                {
                    MyEpicycloidaTangentPoint = new EpicycloidaTangentPoint(Pl, MyEpicycloid);
                    Bind(tangentPoint, MyEpicycloidaTangentPoint, "Center", (IValueConverter)this.Resources["VectorConverter"]);
                    Bind(tangentAngle, MyEpicycloidaTangentPoint, "TangentAngle");
                }
                Pl.AddObject(MyEpicycloidaTangentPoint);
                MyEpicycloidaTangentPoint.AddPointOnCanvas(TempCanvas);
            }
        }

        private void Lab2Tangent_Unselected(object sender, RoutedEventArgs e)
        {
            if (Pl != null)
            {
                MyEpicycloidaTangentPoint.RemovePointFromCanvas(TempCanvas);
                Pl.RemoveObject(MyEpicycloidaTangentPoint);
                if(IsTangentAnimationPressed)
                {
                    this.CancelTangentAnimationButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
        }
        private void ReduceAngleButton_Click(object sender, RoutedEventArgs e)
        {
            MyEpicycloidaTangentPoint.TangentAngle -= 0.5;
        }

        private void IncreaseAngleButton_Click(object sender, RoutedEventArgs e)
        {
            MyEpicycloidaTangentPoint.TangentAngle += 0.5;
        }
       
        private void ScaleAnimatationForEpicycloid(object? sender, EventArgs e)
        {
            if (ScaleAnimationElapsedMillisecondsSinceLastTick >= ScaleAnimationSpeed)
            {
                if (!IsSwitchScaleAnimationDirection)
                {
                    MyEpicycloid.ScaleCoef += 0.01;
                    if (MyEpicycloid.ScaleCoef >= 2d)
                    {
                        IsSwitchScaleAnimationDirection = true;
                    }
                }
                else
            if (IsSwitchScaleAnimationDirection)
                {
                    MyEpicycloid.ScaleCoef -= 0.01;
                    if (Math.Round(MyEpicycloid.ScaleCoef, 1) <= 0.5)
                    {
                        IsSwitchScaleAnimationDirection = false;
                    }
                }
            }
            ScaleAnimationPreviousTick = Timer.Elapsed;
        }

        private void RotateAnimatationForEpicycloid(object? sender, EventArgs e)
        {
            if(RotateAnimationElapsedMillisecondsSinceLastTick >= RotateAnimationSpeed)
            {
                Rotate.Angle = (Rotate.Angle + 1) % 360;
                RotateAnimationPreviousTick = Timer.Elapsed;
            }
        }

        private void TangentAnimatation(object? sender, EventArgs e)
        {
            if (TangentAnimationElapsedMillisecondsSinceLastTick >= TangentAnimationSpeed)
            {
                this.IncreaseAngleButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                TangentAnimationPreviousTick = Timer.Elapsed;
            }
        }

        private void CancelScaleAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsScaleAnimationPressed)
            {
                CompositionTarget.Rendering -= ScaleAnimatationForEpicycloid;
                IsScaleAnimationPressed = false;
            }
        }

        private void ApplyScaleAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsScaleAnimationPressed)
            {
                CompositionTarget.Rendering += ScaleAnimatationForEpicycloid;
                IsScaleAnimationPressed = true;
            }
        }

        private void CancelRotateAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsRotateAnimationPressed)
            {
                CompositionTarget.Rendering -= RotateAnimatationForEpicycloid;
                IsRotateAnimationPressed = false;
            }
        }

        private void ApplyRotateAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRotateAnimationPressed)
            {
                CompositionTarget.Rendering += RotateAnimatationForEpicycloid;
                IsRotateAnimationPressed = true;
            }
        }

        private void CancelTangentAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsTangentAnimationPressed)
            {
                CompositionTarget.Rendering -= TangentAnimatation;
                IsTangentAnimationPressed = false;
            }
        }

        private void ApplyTangentAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if(!IsTangentAnimationPressed)
            {
                CompositionTarget.Rendering += TangentAnimatation;
                IsTangentAnimationPressed = true;
            }
        }
    }
}