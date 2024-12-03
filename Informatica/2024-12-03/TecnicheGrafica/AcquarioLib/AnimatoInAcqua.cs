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
    public class AnimatoInAcqua : AnimatoSulFondo
    {
        public AnimatoInAcqua(Canvas canvas, Image image, int leftOffset = 0, int topOffset = 0)
            : base(canvas, image, leftOffset)
        {
            Thickness t = new Thickness(10 + Math.Abs(leftOffset), 10 + Math.Abs(topOffset), 0, 0);
            image.Margin = t;
        }

        public void Move(bool right, bool up)
        {
            trGroup.Children.Add(new TranslateTransform(right ? 1 : -1, up ? 1 : -1));
            // Image.RenderTransform = trGroup;
        }
    }
}
