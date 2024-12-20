/*
 * 
 * Marco Balducci 4H 2024-11-22
 * Class for the wpf app
 * 
*/
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
        /// <summary>
        /// Window where the program is being run
        /// </summary>
        protected Window MainWindow { get; }

        /// <summary>
        /// Represents the current direction
        /// </summary>
        protected bool FacingRight { get; set; } = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas">The Main Canvas</param>
        /// <param name="image">The image to be shown</param>
        /// <param name="dispatcher">The dispatcher that manages the movements of the object</param>
        /// <param name="mainWindow">The window where the program in being run</param>
        /// <param name="movementX">defines the movement to be made along the X axis for each tick</param>
        /// <param name="movementY">defines the movement to be made along the Y axis for each tick</param>
        public AnimatoPilotato(Canvas canvas, Image image, DispatcherTimer dispatcher, Window mainWindow, int movementX = 5, int movementY = 5)
            : base(canvas, image, dispatcher, movementX, movementY)
        {
            MainWindow = mainWindow;
        }

        //functions used to check the movements
        protected bool CanIGoUpWards() => positionY - movementAmountY > 0;
        protected bool CanIGoDownWards() => Canvas.ActualHeight > positionY + Image.ActualHeight;
        protected bool CanIGoLeft() => positionX - movementAmountX > 0;
        protected bool CanIGoRight() => Canvas.RenderSize.Width > positionX + Image.ActualWidth;

        /// <summary>
        /// Manages the user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void InputManager(Object sender, KeyEventArgs e)
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
                    if (FacingRight) Flip();  //if direction changes, flip
                    FacingRight = false;
                    if (CanIGoLeft()) Canvas.SetLeft(Image, positionX -= movementAmountX);
                    break;
                case Key.Right:
                    if (!FacingRight) Flip(); //if direction changes, flip
                    FacingRight = true;
                    if (CanIGoRight()) Canvas.SetLeft(Image, positionX += movementAmountX);
                    break;
            }
        }

        /// <summary>
        /// Function to be called after creating the object, links the input manager to the window
        /// </summary>
        public override void Start()
        {
            //set new origin to flip correctly
            Image.RenderTransformOrigin = new Point(0.5, 0.5);

            MainWindow.KeyDown += InputManager;
        }
    }
}
