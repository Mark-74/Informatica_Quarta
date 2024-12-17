using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace AcquarioLib
{
    public class AnimatoSulPosto : Inanimato
    {
        public AnimatoSulPosto(Canvas canvas, Image image, DispatcherTimer dispatcher) 
            : base(canvas, image, dispatcher) { }

        protected (int X, int Y) CenterOfRotation { get; set; }
        public void ChangeCenterOfRotation(int X, int Y) => CenterOfRotation = (X, Y);

        public void flip()
        {
            //Image.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform flip = new ScaleTransform(-1, 1);
            trGroup.Children.Add(flip);
        }

        private void Animate(object sender, EventArgs a)
        {
            flip();
        }

        public void Start()
        {
            Dispatcher.Tick += new EventHandler(Animate);
        }
    }
}
