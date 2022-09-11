﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CooridnateGrid.DrawnObjects
{
    public class Circle : IDrawnObject
    {
        private Vector2 _center;
        private double _r;
        private Func<Vector2, Vector2> _transformFunctions;
        private Color _myColor;

        public Color MyColor
        {
            get { return _myColor; }
            set { _myColor = value;
                OnPropertyChanged("MyColor");
            }
        }

        public Vector2 Center
        {
            get { return _center; }
            set { _center = value;
                OnPropertyChanged("Center");
            }
        }

        public double R
        {
            get { return _r; }
            set {
                _r = value; 
                OnPropertyChanged("R"); }
        }

        public Func<Vector2, Vector2> TransformFunctions
        {
            get => _transformFunctions;
            set
            {
                _transformFunctions = value;
                OnPropertyChanged("TransformFunctions");
            }
        }

        public Circle()
        {
            Center = new Vector2(0,0);
            R = 0;
            MyColor = Color.FromRgb(0, 255, 0);
            TransformFunctions = v => v;
        }

        public Circle(Vector2 startCoord, double r, Color color)
        {
            Center = startCoord;
            R = r;
            MyColor = color;
            TransformFunctions = v => v;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private IEnumerable<Vector2> GetCirclePoints()
        {
            for (double t = 0; t <= 2 * Math.PI; t += Math.PI / 24)
            {
                var x = R * Math.Cos(t) + Center.X;
                var y = R * Math.Sin(t) + Center.Y;
                yield return new Vector2((float)x, (float)y);
            }
            yield return new Vector2((float)(R * Math.Cos(0) + Center.X),
                                     (float)(R * Math.Sin(0) + Center.Y));
        }
        public IEnumerable<IEnumerable<Vector2>> GetContourPoints()
        {
            yield return GetCirclePoints();
        }
    }
}