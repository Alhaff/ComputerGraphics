using CooridnateGrid.CoordinatePlane;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.DrawnObjects
{
    public class ImageFromFile : DrawnObject, IDrawingSelf
    {
        string path;
        Vector3 UpLeftCornerPoint = new Vector3(0, 0, 1);
        public ImageFromFile(Uri uri)
        {
            path = uri.ToString();
        }

        public ImageFromFile(Uri uri, Vector3 upLeftCornerPoint)
        {
            this.path = uri.ToString();
            UpLeftCornerPoint = upLeftCornerPoint;
        }

        public void Draw(MyPlane pl, Graphics g)
        {
            var image = System.Drawing.Image.FromFile(path);
            var planeCoord = pl.ToBitmapCoord(UpLeftCornerPoint);
            using (var wrapMode = new ImageAttributes())
            {
                var destRect = new Rectangle((int)planeCoord.X, (int)planeCoord.Y, (int)(image.Width / (pl.ScaleCoef / (pl.StepInPixels /20))), (int)(image.Height / (pl.ScaleCoef / (pl.StepInPixels / 20))));
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                g.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
            
        }

        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            throw new NotImplementedException();
        }
    }
}
