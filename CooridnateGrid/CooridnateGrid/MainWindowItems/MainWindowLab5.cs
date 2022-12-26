using CooridnateGrid.ConverterClasses;
using CooridnateGrid.DrawnObjects;
using CooridnateGrid.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CooridnateGrid
{
    public partial class MainWindow : Window
    {
        private Axes3d axes3D { get; set; }

        private Axes3d RotationAxis { get; set; }
        private Cube cube { get; set; }
        private Cone cone { get; set; }
        private HipParabol hip { get; set; }
        private TransformationConnector BaseProjection { get; set; }
        private Rotate3DTransformations AxisesRotation { get; set; } 
        private CentralProjectionTransformation centralProjectionTransformation { get; set; } 
        private void SetUpLab5()
        {
            AffineTab.Visibility = Visibility.Collapsed;
            ProjectiveTab.Visibility = Visibility.Collapsed;
            Linear2D.Visibility = Visibility.Collapsed;
            Linear3D.Visibility = Visibility.Visible;
            CentralTab.Visibility = Visibility.Visible;
            ConeTab.Visibility = Visibility.Visible;
            if (axes3D == null)
            {
                AxisesRotation.AngleX = 85;
                AxisesRotation.AngleY = -30;
                AxisesRotation.AngleZ = 180;
                BaseProjection = new TransformationConnector(AxisesRotation, centralProjectionTransformation);
                axes3D = new Axes3d();


            }
            if (cone == null)
            {
                Linear3DTransformation = new TransformationConnector(Offset, Rotate3D);
                cone = new Cone();
                cone.MyColor = Colors.Black;
                cone.TransformMe += Linear3DTransformation;
                RotationAxis = new Axes3d(Colors.Purple, Colors.Purple, Colors.Purple);
                Rotate3D.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == "Center")
                    {
                        RotationAxis.Center = Rotate3D.Center;
                    }
                };
                ConeBinding();
            }

          

            Pl.Transform -= Affine;
            Pl.Transform += BaseProjection;
            if (Pl.Objects.Contains(Axes)) Pl.RemoveObject(Axes);
            if (!Pl.Objects.Contains(RotationAxis)) Pl.AddObject(RotationAxis);
            if (!Pl.Objects.Contains(axes3D)) Pl.AddObject(axes3D);
            //if (!Pl.Objects.Contains(hip)) Pl.AddObject(hip);
            if (!Pl.Objects.Contains(cone)) Pl.AddObject(cone);
            
        }
        private void Lab5_Selected(object sender, RoutedEventArgs e)
        {

            SetUpLab5();

        }
        private void Lab5_Unselected(object sender, RoutedEventArgs e)
        {
            if (!(Linear2D.Visibility == Visibility.Visible && Linear3D.Visibility == Visibility.Visible))
            {
                Pl.RemoveObject(axes3D);
                Pl.RemoveObject(cone);
                Pl.AddObject(Rotate.CenterPoint);
                Rotate.CenterPoint.AddPointOnCanvas(TempCanvas);
            }
        }

        private void ConeBinding()
        {
            Bind(ConeA, cone, "A");
            Bind(ConeB, cone, "B");
            Bind(ConeC, cone, "C");
            Bind(ConeHeight, cone, "Height");
            Bind(ConeCenter, cone, "Center", new Vector3ToStringConvertor());
        }

        private void RotateToDefault_Click(object sender, RoutedEventArgs e)
        {
            if(AxisesRotation != null)
            {
                AxisesRotation.AngleX = 85;
                AxisesRotation.AngleY = -30;
                AxisesRotation.AngleZ = 180;
            }
        }
    }
}
