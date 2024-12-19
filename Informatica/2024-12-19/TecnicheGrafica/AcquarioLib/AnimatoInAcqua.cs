using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;

namespace AcquarioLib
{
    public class AnimatoInAcqua : AnimatoSulFondo
    {
        //variables for movement along the Y axis
        protected bool down;
        protected int movementAmountY;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas">The Main Canvas</param>
        /// <param name="image">The image to be shown</param>
        /// <param name="dispatcher">The dispatcher that manages the movements of the object</param>
        /// <param name="movementX">defines the movement to be made along the X axis for each tick</param>
        /// <param name="movementY">defines the movement to be made along the Y axis for each tick</param>
        public AnimatoInAcqua(Canvas canvas, Image image, DispatcherTimer dispatcher, int movementX = 5, int movementY = 5)
            : base(canvas, image, dispatcher, movementX)
        {
            movementAmountY = movementY;
        }

        /// <summary>
        /// Checks the Height for the Move function
        /// </summary>
        /// <returns></returns>
        protected bool CheckHeightBounds()
        {
            if (down) //if we are moving down, return true if we can keep moving down, else false
                return Canvas.RenderSize.Height > positionY + Image.ActualHeight;
            else    //if we are moving up, return true if we can keep moving up, else false
                return positionY < 0;
        }

        /// <summary>
        /// Moves the image along the Y and X axis for each Dispatcher Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        public override void Move(object sender, EventArgs a)
        {
            //check width and height
            isMovingRight = CheckWidthBounds();
            down = CheckHeightBounds();

            //update position variables
            positionX += isMovingRight ? movementAmountX : -movementAmountX;
            positionY += down ? movementAmountY : -movementAmountY;

            //update position on the screen
            Canvas.SetLeft(Image, positionX);
            Canvas.SetTop(Image, positionY);
        }

        /// <summary>
        /// Function to be called after creating the object, starts the animation
        /// </summary>
        public override void Start()
        {
            //set new origin to flip correctly
            Image.RenderTransformOrigin = new Point(0.5, 0.5);

            //set new origin to flip correctly
            Dispatcher.Tick += new EventHandler(Move);
        }
    }
}
