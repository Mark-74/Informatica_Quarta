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
        public AnimatoPilotato(Canvas canvas, Image image, DispatcherTimer dispatcher, Window mainWindow, int movementX = 5, int movementY = 5)
            : base(canvas, image, dispatcher, movementX, movementY)
        {
            MainWindow = mainWindow;
        }

        private bool CanIGoUpWards() => positionY - movementAmountY > 0;
        private bool CanIGoDownWards() => Canvas.ActualHeight > positionY + Image.ActualHeight;
        private bool CanIGoLeft() => positionX - movementAmountX > 0;
        private bool CanIGoRight() => Canvas.RenderSize.Width > positionX + Image.ActualWidth;

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
                    if(CanIGoLeft()) Canvas.SetLeft(Image, positionX -= movementAmountX);
                    break;
                case Key.Right:
                    if(CanIGoRight()) Canvas.SetLeft(Image, positionX += movementAmountX);
                    break;
            }
        }


        public override void start()
        {
            //set new origin to flip correctly
            Image.RenderTransformOrigin = new Point(0.5, 0.5);

            MainWindow.KeyDown += GestoreComandi;
        }
    }
}
