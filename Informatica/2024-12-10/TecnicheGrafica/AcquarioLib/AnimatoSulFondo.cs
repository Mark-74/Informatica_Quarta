using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System.Xaml;

namespace AcquarioLib
{
    public class AnimatoSulFondo : Inanimato
    {
        public AnimatoSulFondo(Canvas canvas, Image image, DispatcherTimer dispatcher, int leftOffset = 0)
            : base(canvas, image, dispatcher) {}

        protected (int X, int Y) CenterOfRotation { get; set; }
        private bool right = true; 
        private int amount = 10;

        public void ChangeCenterOfRotation(int X, int Y) => CenterOfRotation = (X, Y);

        public void Rotate(double degrees = 1.0)
        {
            trGroup.Children.Add(new RotateTransform(degrees, CenterOfRotation.X, CenterOfRotation.Y));
        }

        public void Move(object sender, EventArgs a)
        {
            trGroup.Children.Add(new TranslateTransform(right ? amount : -amount, 0));
            // Image.RenderTransform = trGroup;
        }

        public void flip()
        {
            ScaleTransform flip = new ScaleTransform(-1,1);
            trGroup.Children.Add(flip);
        }

        public virtual void start()
        {
            //position image to the bottom
            Image.Margin = new Thickness(Image.Margin.Left, Canvas.Height - Image.RenderSize.Height, Image.Margin.Right, 0);
            Image.RenderTransformOrigin = new Point(0.5, 0.5);

            //add animation to the dispatcher
            Dispatcher.Tick += new EventHandler(Move);
        }
    }
}
