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
    public class AnimatoSulFondo : AnimatoSulPosto
    {
        protected double positionX = 0;
        protected bool right;
        protected int movementAmountX;

        public AnimatoSulFondo(Canvas canvas, Image image, DispatcherTimer dispatcher, int movementX = 10)
            : base(canvas, image, dispatcher) 
        {
            movementAmountX = movementX;
        }


        public void Rotate(double degrees = 1.0)
        {
            trGroup.Children.Add(new RotateTransform(degrees, CenterOfRotation.X, CenterOfRotation.Y));
        }

        protected bool CheckWidthBounds()
        {
            if (right)
            {
                if (Canvas.RenderSize.Width < positionX + Image.ActualWidth) flip();
                return Canvas.RenderSize.Width > positionX + Image.ActualWidth;
            }
            else
            {
                if (positionX < 0) flip();
                return positionX < 0;
            }  
        }

        public virtual void Move(object sender, EventArgs a)
        {
            right = CheckWidthBounds();
            positionX += right ? movementAmountX : -movementAmountX;
            Canvas.SetLeft(Image, positionX);
        }

        public virtual void start()
        {
            //position image to the bottom
            Image.Margin = new Thickness(Image.Margin.Left, Canvas.Height - Image.RenderSize.Height, Image.Margin.Right, 0);

            //set new origin to flip correctly
            Image.RenderTransformOrigin = new Point(0.5, 0.5);

            //set new origin to flip correctly
            Dispatcher.Tick += new EventHandler(Move);
        }
    }
}
