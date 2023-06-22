using CooridnateGrid.DrawnObjects;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace CooridnateGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Lab1Drawing MyDrawing { get; set; } = null;

       
        private Circle Circle { get; set; } = null;
        private void AddLab1Obj()
        {
            AffineTab.Visibility = Visibility.Visible;
            ProjectiveTab.Visibility = Visibility.Visible;
            Linear2D.Visibility = Visibility.Visible;
            Linear3D.Visibility = Visibility.Collapsed;
            CentralTab.Visibility = Visibility.Collapsed;
            ConeTab.Visibility = Visibility.Collapsed;
            if (Pl != null)
            {

                if(MyDrawing == null)
                {
                    MyDrawing = ((Lab1Drawing)this.Resources["mainObj"]);
                    
                }
                if (!Pl.Objects.Contains(Axes))
                    Pl.AddObject(Axes);
                Pl.AddObject(MyDrawing);
                MyDrawing.TransformMe += LinearTransformation;
                if (!Pl.Transform.GetInvocationList().Contains(Affine))
                    Pl.Transform += Affine;
                if (BaseProjection != null)
                    Pl.Transform -= BaseProjection;
            }
        }

        private void RemoveLab1Obj()
        {
            if (Pl != null)
            {
                MyDrawing.TransformMe -= LinearTransformation;
                Pl.RemoveObject(MyDrawing);
               
            }
        }

        private void Lab1_Selected(object sender, RoutedEventArgs e)
        {
           
            AddLab1Obj();
        }

        private void Lab1_Unselected(object sender, RoutedEventArgs e)
        {
            RemoveLab1Obj();
           
        }
    }
}