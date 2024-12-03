
using System.Windows.Controls;
using System.Windows.Media;

namespace AcquarioLib
{
    public class Inanimato
    {
        protected Canvas Canvas;
        protected TransformGroup trGroup;
        public Image Image { get; protected set; }

        public void AddToScreen() => Canvas.Children.Add(Image);

        public Inanimato(Canvas canvas, Image image)
        {
            if (image.Source == null) throw new ArgumentException("Image Source cannot be null.");
            if (image.Margin == null) throw new ArgumentException("Image Margin cannot be null.");

            Canvas = canvas;
            Image = image;
            trGroup = new TransformGroup();
            Image.RenderTransform = trGroup;
        }
    }

}
