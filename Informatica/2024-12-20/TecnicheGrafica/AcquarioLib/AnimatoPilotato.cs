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
using System.Windows.Input;
using System.Diagnostics;

namespace AcquarioLib
{
    public class AnimatoPilotato : AnimatoInAcqua
    {
        protected Window MainWindow { get; }
        protected bool FacingRight { get; set; } = true;
        public AnimatoPilotato(Canvas canvas, Image image, DispatcherTimer dispatcher, Window mainWindow, int movementX = 5, int movementY = 5)
            : base(canvas, image, dispatcher, movementX, movementY)
        {
            MainWindow = mainWindow;
        }

        protected bool CanIGoUpWards() => positionY - movementAmountY > 0;
        protected bool CanIGoDownWards() => Canvas.ActualHeight > positionY + Image.ActualHeight;
        protected bool CanIGoLeft() => positionX - movementAmountX > 0;
        protected bool CanIGoRight() => Canvas.RenderSize.Width > positionX + Image.ActualWidth;

        protected virtual void GestoreComandi(Object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if(CanIGoUpWards()) Canvas.SetTop(Image, positionY -= movementAmountY);
                    break;
                case Key.Down:
                    if(CanIGoDownWards()) Canvas.SetTop(Image, positionY += movementAmountY);
                    break;
                case Key.Left:
                    if (FacingRight) flip();
                    FacingRight = false;
                    if (CanIGoLeft()) Canvas.SetLeft(Image, positionX -= movementAmountX);
                    break;
                case Key.Right:
                    if (!FacingRight) flip();
                    FacingRight = true;
                    if (CanIGoRight()) Canvas.SetLeft(Image, positionX += movementAmountX);
                    break;
            }
        }


        public override void Start()
        {
            //set new origin to flip correctly
            Image.RenderTransformOrigin = new Point(0.5, 0.5);

            MainWindow.KeyDown += GestoreComandi;
        }
    }
}
