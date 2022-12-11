using CooridnateGrid.DrawnObjects;
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
            if (Pl != null)
            {
                if(MyDrawing == null)
                {
                    MyDrawing = ((Lab1Drawing)this.Resources["mainObj"]);
                    
                }
                Pl.AddObject(MyDrawing);
                MyDrawing.TransformMe += LinearTransformation;
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