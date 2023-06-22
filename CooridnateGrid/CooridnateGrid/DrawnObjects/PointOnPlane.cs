using CooridnateGrid.CoordinatePlane;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CooridnateGrid.ExtensionsClasses;
using CooridnateGrid;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace CooridnateGrid.DrawnObjects
{
    public class PointOnPlane : DrawnObject, IDrawingSelf
    {
        #region Variables
            private Vector3 _center;
            public readonly MyPlane plane;
        #endregion

        #region Propreties
        public Vector3 Center
        {
            get { return _center; }
            set 
            { 
                _center = value;
                OnPropertyChanged("Center");
            }
        }

        public System.Windows.Shapes.Ellipse PointCenter { get; set; }
          private bool DragIsOver { get; set; } = true; 
       // public static System.Windows.Shapes.Ellipse ChosenPoint { get; private set; }
        #endregion

        #region Constructors
        public PointOnPlane(MyPlane pl, float x, float y, float z = 1f)
        {
            plane = pl;
            MyColor = System.Windows.Media.Color.FromRgb(220, 0, 0);
            _center = new Vector3(x, y, z);
            PointCenter = new System.Windows.Shapes.Ellipse();
            PointCenter.Height = 0.4 * pl.StepInPixels;
            PointCenter.Width = 0.4 * pl.StepInPixels;
            PointCenter.Fill = new SolidColorBrush(MyColor);
            PointCenter.MouseMove += _pointCenter_MouseMove;
            IsPlaneTransfromMe = true;
        }

        public virtual void _pointCenter_MouseMove(object sender, MouseEventArgs e)
        {
            Func<int, byte> opositeColor = (par) => (byte)Math.Abs(255 - par); 
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragIsOver = false;
                var chosenPoint = sender as System.Windows.Shapes.Ellipse;
                var savedColor = chosenPoint.Fill;
                chosenPoint.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(MyColor.A,
                                                                      opositeColor(MyColor.R),
                                                                      opositeColor(MyColor.G),
                                                                      opositeColor(MyColor.B)
                                                                      ));
                DragDrop.DoDragDrop(PointCenter, PointCenter, DragDropEffects.Move);
                chosenPoint.Fill = savedColor;
                Center = plane.ToPlaneCoord(new Vector3((float)(Canvas.GetLeft(chosenPoint) + chosenPoint.Width/2), 
                                                        (float)(Canvas.GetTop(chosenPoint) + chosenPoint.Height/2), Center.Z));
                DragIsOver = true;
            }
        }

        public PointOnPlane(MyPlane pl, Vector3 vector) : this(pl, vector.X, vector.Y, vector.Z)
        {
           
        }
        public PointOnPlane(MyPlane pl, Vector3 vector,System.Windows.Media.Color color) : this(pl,vector)
        {
            MyColor = color;
        }
        #endregion

        #region Methods
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {

            throw new NotImplementedException();
        }

        public void AddPointOnCanvas(Canvas canvas)
        {
            if (!canvas.Children.Contains(PointCenter))
            {
                canvas.Children.Add(PointCenter);
                var coord = IsPlaneTransfromMe ? plane.ToBitmapCoord(Center)
                    : plane.ToBitmapCoordWithoutTransform(Center);
                PointCenter.Fill = new SolidColorBrush(MyColor);
                Canvas.SetLeft(PointCenter, coord.X - PointCenter.Width / 2);
                Canvas.SetTop(PointCenter, coord.Y - PointCenter.Height / 2);
            }
        }
        public void RemovePointFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(PointCenter);
        }

        public virtual void Draw(MyPlane pl, Graphics g)
        {
            if (DragIsOver)
            {
                var coord = IsPlaneTransfromMe ? plane.ToBitmapCoord(TransformMe(Center)) 
                    : plane.ToBitmapCoordWithoutTransform(TransformMe(Center));
                Canvas.SetLeft(PointCenter, coord.X - PointCenter.Width / 2);
                Canvas.SetTop(PointCenter, coord.Y - PointCenter.Height / 2);
            }
        }
        #endregion
    }
}
