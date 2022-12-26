using CooridnateGrid.CoordinatePlane;
using CooridnateGrid.ExtensionsClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CooridnateGrid.DrawnObjects
{
    public class Bezier5CurvesDrawer : DrawnObject, IDrawingSelf
    {
        private List<Bezier5Curve> bezier5Curves = new List<Bezier5Curve>();
        private LinkedList<PointOnPlane> tmp = new LinkedList<PointOnPlane>();
        private bool Show { get; set; } = true;
        List<Bezier5Curve> SelectedCurves = new List<Bezier5Curve>();
        public bool IDrawCurvePoint { get; set; } = true;
        public bool IDrawHexagonPoint { get; set; } = false;
        private PointOnPlane StartPoint {get; set;}
        public List<Bezier5Curve> Curves { get => bezier5Curves; }
        public int FragmentCount { get => bezier5Curves.Count; }
        public System.Windows.Media.Color HexagonLineColor { get; set; } = Colors.Black;

        #region TangentThings
        private (double,double) GetTangentValueInConnectedPointOfSelectedFragment(double precision = 1E-4)
        {
            var p1 = SelectedCurves[0].Bezier5CurvePoint(1);
            var p1NearestPoint = SelectedCurves[0].Bezier5CurvePoint(1 - precision);
            var angleP1 = (p1NearestPoint - p1).Angle();
            var p2 = SelectedCurves[1].Bezier5CurvePoint(0);
            var p2NearestPoint = SelectedCurves[1].Bezier5CurvePoint(0 + precision);
            var angleP2 = (p2NearestPoint - p2).Angle();
            angleP1 +=  angleP1 < 0? Math.PI : 0;
            angleP2 += angleP2 < 0?  Math.PI : 0;
            return (angleP1, angleP2);
        }
        private bool IBreakPoint()
        {
            if (SelectedCurves[0].StartEndPoints[1] == SelectedCurves[1].StartEndPoints[0])
            {
                var tangents = GetTangentValueInConnectedPointOfSelectedFragment();
                var diff = Math.Abs(tangents.Item1) - Math.Abs(tangents.Item2);
                return !(diff >= -1E-2 && diff <= 1E-2);
            }
            return true;
        }
        private void BrushTangentPoint(Canvas canvas)
        {
            Action<PointOnPlane,Canvas> UpdatePoint = (p,c) =>
            {
               p.RemovePointFromCanvas(c);
               p.AddPointOnCanvas(c);
            };
            if (IBreakPoint())
            {
                SelectedCurves[0].StartEndPoints[1].MyColor = Colors.Red;
            }
            else
            {
                SelectedCurves[0].StartEndPoints[1].MyColor = Colors.Green;
            }
            UpdatePoint(SelectedCurves[0].StartEndPoints[1], canvas);
        }
        private IEnumerable<Vector3> GetTangentPoint(Vector3 point,double angle)
        {
           
            var p1Start = Line.GetLineEndPoint(point, 2, angle);
            var p1End = Line.GetLineEndPoint(point, 2, Math.PI + angle);
            return new List<Vector3>() { p1Start, p1End };
        }

        #endregion

        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
           foreach (var curve in bezier5Curves)
            {
                var tempColor = MyColor;
                yield return curve.GetBezier5FragmentCountour();
                if(IDrawHexagonPoint || SelectedCurves.Contains(curve))
                {
                   
                    MyColor = HexagonLineColor;
                    yield return curve.GetHexagonCountour();
                   
                }
                if(SelectedCurves.Count ==2)
                {
                    var tangents = GetTangentValueInConnectedPointOfSelectedFragment();
                    MyColor = Colors.Green;
                    yield return GetTangentPoint(SelectedCurves[0].StartEndPoints[1].Center, tangents.Item1);
                    MyColor = Colors.Red;
                    yield return GetTangentPoint(SelectedCurves[1].StartEndPoints[0].Center, tangents.Item2);
                }
                MyColor = tempColor;
            }
        }
        #region AddPointsOnCanvas
        private void AddHexagonPointsOnPlane(Canvas canvas, Bezier5Curve curve)
        {
            foreach (var point in curve.MiddlePoints)
            {
                point.AddPointOnCanvas(canvas);
            }
        }

        public void AddAllHexagonPointsOnPlane(Canvas canvas)
        {
            if (!IDrawHexagonPoint)
            {
                foreach (var curve in bezier5Curves)
                {
                    AddHexagonPointsOnPlane(canvas, curve);
                }
                IDrawHexagonPoint = true;
            }
        }

        private void RemoveHexagonPointsFromPlane(Canvas canvas, Bezier5Curve curve)
        {
            foreach (var point in curve.MiddlePoints)
            {
                point.RemovePointFromCanvas(canvas);
            }
        }

        public void RemoveAllHexagonPointsFromPlane(Canvas canvas)
        {
            if (IDrawHexagonPoint)
            {
                foreach (var curve in bezier5Curves)
                {
                    RemoveHexagonPointsFromPlane(canvas, curve);
                }
                IDrawHexagonPoint = false;
            }
        }

        private void AddCurvePointsOnPlane(Canvas canvas, Bezier5Curve curve)
        {
            foreach (var point in curve.StartEndPoints)
            {
                point.AddPointOnCanvas(canvas);
            }
        }

        public void AddAllCurvesPointsOnPlane(Canvas canvas)
        {
            if (!IDrawCurvePoint)
            {
                foreach (var curve in bezier5Curves)
                {
                    AddCurvePointsOnPlane(canvas, curve);
                }
                IDrawCurvePoint = true;
            }
        }

        private void RemoveCurvePointsFromPlane(Canvas canvas, Bezier5Curve curve)
        {
            foreach (var point in curve.StartEndPoints)
            {
                point.RemovePointFromCanvas(canvas);
            }
        }

        public void RemoveAllCurvesPointsFromPlane(Canvas canvas)
        {
            if (IDrawCurvePoint)
            {
                foreach (var curve in bezier5Curves)
                {
                    RemoveCurvePointsFromPlane(canvas, curve);
                }
                IDrawCurvePoint = false;
            }
        }
        #endregion

        #region MouseEventsWithPoint
        public void Bezier5Curve_MouseDown(MyPlane plane, Canvas canvas, object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IDrawCurvePoint = true;
                var pos = e.GetPosition(canvas);
                var point = plane.ToPlaneCoord(new Vector3((float)pos.X, (float)pos.Y, 1));
                var planePoint = new PointOnPlane(plane, point);
                planePoint.MyColor = MyColor;
                planePoint.AddPointOnCanvas(canvas);
                planePoint.PointCenter.MouseDown += (s, e) =>  BezierCurvePoint_MouseDown(canvas, s,e);
                tmp.AddLast(planePoint);
                if (tmp.Count == 1)
                {
                    StartPoint = tmp.First.Value;
                }
                if (tmp.Count == 2)
                {
                    var start = tmp.First.Value;
                    var end = tmp.Last.Value;
                    bezier5Curves.Add(new Bezier5Curve(
                        new List<PointOnPlane>()
                        {
                        start,
                        end
                        })
                        );
                    tmp.RemoveFirst();
                }
            }
        }

        private List<Bezier5Curve> FindCurvesWithPoint(Ellipse point)
        {
           
            var tmpList = new List<Bezier5Curve>();
            for (int i = 0; i < bezier5Curves.Count; i++)
            {
                
                var curve = bezier5Curves[i];
                foreach (var p1 in curve.StartEndPoints)
                {
                    if (p1.PointCenter == point)
                    {
                        if (!tmpList.Contains(curve))
                            tmpList.Add(curve);
                        if (tmpList.Count == 2)
                        {
                            return tmpList;
                        }
                    }

                }
            }
            return tmpList;
        }
      
        private void BezierCurvePoint_MouseDown(Canvas canvas, object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var point = sender as Ellipse;

                var curves =  FindCurvesWithPoint(point);

               

                foreach (var curve in curves)
                {
                    if (!SelectedCurves.Contains(curve))
                    {
                        point.MouseMove -= curves[0].StartEndPoints[1]._pointCenter_MouseMove;
                        point.MouseMove += TangentPointMove;
                        curves.First().MiddlePoints.Last().PointCenter.MouseMove -= curves.First().MiddlePoints.Last()._pointCenter_MouseMove;
                        curves.First().MiddlePoints.Last().PointCenter.MouseMove += NeighborhoodsTangentPoint_MouseMove;
                        curves.Last().MiddlePoints.First().PointCenter.MouseMove -= curves.Last().MiddlePoints.First()._pointCenter_MouseMove;
                        curves.Last().MiddlePoints.First().PointCenter.MouseMove += NeighborhoodsTangentPoint_MouseMove;
                        AddHexagonPointsOnPlane(canvas, curve);
                        SelectedCurves.Add(curve);
                    }
                    else
                    {
                        point.MouseMove -= TangentPointMove;
                        point.MouseMove += curves[0].StartEndPoints[1]._pointCenter_MouseMove;
                        curves.First().MiddlePoints.Last().PointCenter.MouseMove += curves.First().MiddlePoints.Last()._pointCenter_MouseMove;
                        curves.First().MiddlePoints.Last().PointCenter.MouseMove -= NeighborhoodsTangentPoint_MouseMove;
                        curves.Last().MiddlePoints.First().PointCenter.MouseMove += curves.Last().MiddlePoints.First()._pointCenter_MouseMove;
                        curves.Last().MiddlePoints.First().PointCenter.MouseMove -= NeighborhoodsTangentPoint_MouseMove;
                        RemoveHexagonPointsFromPlane(canvas, curve);
                        SelectedCurves.Remove(curve);

                    }
                }
                if(SelectedCurves.Count ==2) BrushTangentPoint(canvas);
            }
        }

        public void TangentPointMove(object sender, MouseEventArgs e)
        {
            TangentPoint_MouseMove(SelectedCurves[0].StartEndPoints[1],
                                Tuple.Create(SelectedCurves[0].MiddlePoints[3], SelectedCurves[1].MiddlePoints[0]), sender, e);
        }
        private  void TangentPoint_MouseMove(PointOnPlane point , Tuple<PointOnPlane,PointOnPlane> neighborhoods, object sender, MouseEventArgs e)
        {
            var start = point.Center;
            point._pointCenter_MouseMove(sender, e);
            var end = point.Center;
            var offset = end - start;
            if (point.MyColor == Colors.Green)
            {
                neighborhoods.Item1.Center += offset;
                neighborhoods.Item2.Center += offset;
            }
        }
        
        private void NeighborhoodsTangentPoint_MouseMove(object sender, MouseEventArgs e)
        {
            var ellipse = sender as Ellipse;
            PointOnPlane selectedPoint = SelectedCurves.SelectMany(curve => curve.MiddlePoints
                                                                                  .Select(p => p)
                                                                                  .Where(p => p.PointCenter == ellipse)
                                                      ).FirstOrDefault();
            if (selectedPoint != null)
            {
                var tangentPoint = SelectedCurves.First().StartEndPoints.Last().Center;
                var start = (selectedPoint.Center - tangentPoint).Angle();
                selectedPoint._pointCenter_MouseMove(sender, e);
                var end = (selectedPoint.Center -tangentPoint).Angle();
                var offset = end - start;
                if (SelectedCurves.First().StartEndPoints.Last().MyColor == Colors.Green)
                {
                    if (selectedPoint == SelectedCurves.First().MiddlePoints.Last())
                    {
                        var tmp = (SelectedCurves.Last().MiddlePoints.First().Center - tangentPoint).Rotate(offset);
                        var res = (tmp + tangentPoint);
                        SelectedCurves.Last().MiddlePoints.First().Center = new Vector3(res.X,res.Y,1);


                    }
                    else if (selectedPoint == SelectedCurves.Last().MiddlePoints.First())
                    {
                        var tmp = (SelectedCurves.First().MiddlePoints.Last().Center - tangentPoint).Rotate(offset);
                        var res = (tmp + tangentPoint);
                        SelectedCurves.First().MiddlePoints.Last().Center = new Vector3(res.X, res.Y, 1);
                    }
                }
            }
        }
        #endregion

        public void ConectLastAndFirstPoint()
        {
            if (bezier5Curves.Count > 0 && StartPoint!= null)
            {
                var start = bezier5Curves[bezier5Curves.Count - 1].StartEndPoints[1];
                var end = StartPoint;
                bezier5Curves.Add(new Bezier5Curve(
                       new List<PointOnPlane>()
                       {
                        start,
                        end
                       })
                       );
                tmp.Clear();
            }
        }
        public void ApplyTransfromationToInerPoint()
        {
            foreach(var curve in bezier5Curves)
            {
                foreach(var p in curve.StartEndPoints)
                {
                    p.TransformMe += this.TransformMe;
                }

                foreach(var p in curve.MiddlePoints)
                {
                    p.TransformMe += this.TransformMe;
                }
            }
        }


        #region FileWorks
        public void WriteBinaryPointsToFile(string filePath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
               foreach(var curve in bezier5Curves)
                {
                    foreach(var startEnd in curve.StartEndPoints)
                    {
                        writer.Write(startEnd.Center.X);
                        writer.Write(startEnd.Center.Y);
                        writer.Write(startEnd.Center.Z);
                    }

                    foreach (var middle in curve.MiddlePoints)
                    {
                        writer.Write(middle.Center.X);
                        writer.Write(middle.Center.Y);
                        writer.Write(middle.Center.Z);
                    }
                }
               writer.Close();
            }

        }
        private IEnumerable<Vector3> GetVectorsFromFile(BinaryReader reader, int vectorCount)
        {
            for(int i = 0; i < vectorCount; i++)
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                float z = reader.ReadSingle();
                yield return new Vector3(x, y, z);
            }
        }

        private void SetupReadedCurves()
        {
            var start = bezier5Curves[0];
                for (int i = 1; i < bezier5Curves.Count; i++)
                {
                    bezier5Curves[i].StartEndPoints[0] = start.StartEndPoints[1];
                    bezier5Curves[i].StartEndPoints[0].PointCenter.MouseDown += (s, e) => BezierCurvePoint_MouseDown(canvas, s, e);
                    start = bezier5Curves[i];
                    if(i == bezier5Curves.Count -1)
                    {
                        if (start.StartEndPoints[1].Center == bezier5Curves[0].StartEndPoints[0].Center)
                        {
                            start.StartEndPoints[1] = bezier5Curves[0].StartEndPoints[0];
                            start.StartEndPoints[1].PointCenter.MouseDown += (s, e) => BezierCurvePoint_MouseDown(canvas, s, e);
                        }
                    }  
                }
                
        }

        public void ReadBinaryPointsFromFile(MyPlane pl, Canvas canvas, string filePath)
        {
            this.ClearCurrentCurve(canvas);
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open), Encoding.ASCII))
            {
                var res = reader.PeekChar();
                try
                {
                    while ( res > -1)
                {
                   
                        List<Vector3> StartEnd = GetVectorsFromFile(reader, 2).ToList();
                        List<Vector3> Middle = GetVectorsFromFile(reader, 4).ToList();
                        var color = MyColor;
                        bezier5Curves.Add(
                            new Bezier5Curve(
                                new List<PointOnPlane>()
                                {
                                new PointOnPlane(pl, StartEnd[0],color),
                                new PointOnPlane(pl, StartEnd[1],color)
                                },
                                new List<PointOnPlane>()
                                {
                                new PointOnPlane(pl, Middle[0],color),
                                new PointOnPlane(pl, Middle[1],color),
                                new PointOnPlane(pl, Middle[2],color),
                                new PointOnPlane(pl, Middle[3],color)
                                })
                            );
                    } 
                }
                catch { }
                SetupReadedCurves();
                reader.Close();
            }
            this.AddAllCurvesPointsOnPlane(canvas);
        }
        #endregion
        public void Draw(MyPlane pl, Graphics g)
        {
            pl.DrawObj(this, g);
            foreach( var curve in bezier5Curves)
            {
                if (IDrawCurvePoint || SelectedCurves.Contains(curve))
                {
                    foreach (var point in curve.StartEndPoints)
                    {
                        point.Draw(pl, g);
                    }
                }
                if (IDrawHexagonPoint || SelectedCurves.Contains(curve))
                {
                    foreach (var point in curve.MiddlePoints)
                    {
                        point.Draw(pl, g);
                    }
                }
            }
        }

        public void ClearCurrentCurve(Canvas canvas)
        {
            this.RemoveAllCurvesPointsFromPlane(canvas);
            this.RemoveAllHexagonPointsFromPlane(canvas);
            foreach(var curve in SelectedCurves)
            {
                RemoveCurvePointsFromPlane(canvas, curve);
                RemoveHexagonPointsFromPlane(canvas, curve);
            }
            SelectedCurves.Clear();
            bezier5Curves.Clear();
            tmp.Clear();
        }

    }
}
