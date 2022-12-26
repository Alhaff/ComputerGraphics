using CooridnateGrid.DrawnObjects;
using CooridnateGrid.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CooridnateGrid
{
    public partial class MainWindow : Window
    {
        public DecartToConeTransfromation decartToCone { get; set; }

        public TransformationConnector Lab6Transformation { get; set; }
        public TransformationConnector PointTransformation { get; set; }
        private void Lab6_Selected(object sender, RoutedEventArgs e)
        {
            SetUpLab5();
            Linear2D.Visibility = Visibility.Visible;
            AffineTab.Visibility = Visibility.Visible;
            if (decartToCone == null)
            {
                decartToCone = new DecartToConeTransfromation(cone);
                BindDecart();
            }

            Lab6Transformation = new TransformationConnector(LinearTransformation,Affine, decartToCone,Linear3DTransformation);

            PointTransformation = new TransformationConnector(Affine, decartToCone, Linear3DTransformation);
            if (BzDrawer == null)
            {
                BzDrawer = new Bezier5CurvesDrawer();
                BzDrawer.MyColor = Colors.Blue;
                BzDrawer.ReadBinaryPointsFromFile(Pl, TempCanvas, defaultPath + "palm.dat");
                
                BzDrawer.TransformMe += Lab6Transformation;
                chosePointText = new TextBlock();
                chosePointText.Text = "Оберіть точку на екрані:";
                chosePointText.HorizontalAlignment = HorizontalAlignment.Center;
                controlButtonText = new TextBlock();
                controlButtonText.Text = "Натисніть P для паузи \nНатисність ЕSC для завершення малювання";
                controlButtonText.FontSize = 18;
                controlButtonText.HorizontalAlignment = HorizontalAlignment.Center;
                controlButtonText.Background = new System.Windows.Media.SolidColorBrush(Colors.White);
                if (Pl.Objects.Contains(Rotate.CenterPoint))
                {

                    Rotate.CenterPoint.TransformMe += PointTransformation;
                }
                BeforeAnimationStart();

            }else
            {
                BzDrawer.TransformMe -= LinearTransformation;
                BzDrawer.TransformMe += Lab6Transformation;
            }
            D1.Text = "dU =";
            D2.Text = "dV =";
            Rotate1.Text = "Центр обертання(u,v):";
            BzDrawer.MyColor = Colors.Blue;
            BzDrawer.RemoveAllCurvesPointsFromPlane(TempCanvas);
            BzDrawer.Thickness = 4;
            Pl.AddObject(BzDrawer);
        }
        private void BindDecart()
        {
            Bind(UStep, decartToCone, "EllipseDenominator");
            Bind(VStep, decartToCone, "OneStepOnZAxis");
        }

        private void Lab6_Unselected(object sender, RoutedEventArgs e)
        {
            if (Linear3D.Visibility != Visibility.Visible)
            {
                Lab5_Unselected(sender, e);
            }
            Pl.RemoveObject(BzDrawer);
            BzDrawer.Thickness = 1;
            D1.Text = "dX =";
            D2.Text = "dY =";
            Rotate1.Text = "Центр обертання(x,y):";

        }
    }
}
