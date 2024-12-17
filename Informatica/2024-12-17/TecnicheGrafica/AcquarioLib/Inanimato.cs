
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace AcquarioLib
{
    public class Inanimato
    {
        protected Canvas Canvas;
        protected TransformGroup trGroup;
        public Image Image { get; protected set; }
        protected DispatcherTimer Dispatcher { get; set; }

        public void AddToScreen() => Canvas.Children.Add(Image);

        public Inanimato(Canvas canvas, Image image, DispatcherTimer dispatcher)
        {
            if (image.Source == null) throw new ArgumentException("Image Source cannot be null.");
            if (image.Margin == null) throw new ArgumentException("Image Margin cannot be null.");

            Canvas = canvas;
            Image = image;
            Dispatcher = dispatcher;
            trGroup = new TransformGroup();
            Image.RenderTransform = trGroup;
        }
    }

}
