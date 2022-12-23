using CooridnateGrid.CoordinatePlane;
using CooridnateGrid.ExtensionsClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CooridnateGrid.DrawnObjects
{
    public class EpicycloidaTangentPoint : PointOnPlane
    {
        private Epicycloid _targetEpicycloid;
        private double _angle = 0;
        public EpicycloidaTangentPoint(MyPlane pl, Epicycloid target) : base(pl, target.GetEcicycloidPoint(0))
        {
            _targetEpicycloid = target;
            PointCenter.Width = 0.3 * pl.StepInPixels;
            PointCenter.Height = 0.3 * pl.StepInPixels;
            MyColor = Colors.Red;
            PointCenter.Fill = new SolidColorBrush(Colors.DarkRed);
            IsPlaneTransfromMe = true;
            TransformMe += target.TransformMe;
        }
        
        public double TangentAngle 
        {
            get { return (Math.Round(_angle / (Math.PI / 180),1)); }
            set
            {
                _angle = value * (Math.PI / 180);
                Center = _targetEpicycloid.GetEcicycloidPoint(_angle);
                OnPropertyChanged("TangentAngle");
            }
        }
        public override void _pointCenter_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
        public double GetTangentValue(double angle, double precision = 1E-4)
        {
            Vector3 point = _targetEpicycloid.GetEcicycloidPoint(angle + precision);
            Vector3 tangent = point - Center;
            return tangent.Angle();
        }
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            throw new NotImplementedException();
        }
        public override void Draw(MyPlane pl, Graphics g)
        {
            Center = _targetEpicycloid.GetEcicycloidPoint(_angle);
            base.Draw(pl, g);
            var tangent =  GetTangentValue(_angle);
            var LineStart =pl.ToBitmapCoord(TransformMe(Line.GetLineEndPoint(Center, 4, tangent)));
            var LineEnd = pl.ToBitmapCoord(TransformMe(Line.GetLineEndPoint(Center, 4, tangent + Math.PI)));
            var NormalLineStart = pl.ToBitmapCoord(TransformMe(Center));
            var NormalLineEnd = pl.ToBitmapCoord(TransformMe(Line.GetLineEndPoint(Center, 4, tangent + -Math.PI/2)));
            pl.DrawLine(g, Tuple.Create(LineStart, LineEnd),Thickness, MyColor);
            pl.DrawLine(g, Tuple.Create(NormalLineStart, NormalLineEnd),Thickness, MyColor);
        }        
    }
}
