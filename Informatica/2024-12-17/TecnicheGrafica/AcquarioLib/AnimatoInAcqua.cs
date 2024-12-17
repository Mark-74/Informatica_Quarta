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
        protected double positionY = 0;
        protected bool up;
        protected int movementAmountY;

        public AnimatoInAcqua(Canvas canvas, Image image, DispatcherTimer dispatcher, int movementX = 5, int movementY = 5)
            : base(canvas, image, dispatcher, movementX)
        {
            movementAmountY = movementY;
        }

        protected bool CheckHeightBounds()
        {
            if (up)
                return Canvas.RenderSize.Height > positionY + Image.ActualHeight;
            else
                return positionY < 0;
        }

        public override void Move(object sender, EventArgs a)
        {
            right = CheckWidthBounds();
            up = CheckHeightBounds();
            positionX += right ? movementAmountX : -movementAmountX;
            positionY += up ? movementAmountY : -movementAmountY;
            Canvas.SetLeft(Image, positionX);
            Canvas.SetBottom(Image, positionY);
        }

        public override void start()
        {

            //set new origin to flip correctly
            Image.RenderTransformOrigin = new Point(0.5, 0.5);

            //set new origin to flip correctly
            Dispatcher.Tick += new EventHandler(Move);
        }
    }
}
