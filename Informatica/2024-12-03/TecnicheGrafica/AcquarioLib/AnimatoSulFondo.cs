using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace AcquarioLib
{
    public class AnimatoSulFondo : Inanimato
    {
        public AnimatoSulFondo(Canvas canvas, Image image, int leftOffset = 0)
            : base(canvas, image)
        {
            Thickness t = new Thickness(10 + Math.Abs(leftOffset), 10, 0, 0);
            image.Margin = t;
            image.VerticalAlignment = VerticalAlignment.Bottom;
        }

        protected (int X, int Y) CenterOfRotation { get; set; }

        public void ChangeCenterOfRotation(int X, int Y) => CenterOfRotation = (X, Y);

        public void Rotate(double degrees = 1.0)
        {
            trGroup.Children.Add(new RotateTransform(degrees, CenterOfRotation.X, CenterOfRotation.Y));
        }

        public void Move(bool right)
        {
            trGroup.Children.Add(new TranslateTransform(right ? 1 : -1, 0));
            // Image.RenderTransform = trGroup;
        }
    }
}
