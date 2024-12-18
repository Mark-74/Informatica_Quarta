
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AcquarioLib
{
    public class Inanimato
    {
        protected static List<Inanimato> instances = new List<Inanimato>();

        protected Canvas Canvas;
        protected TransformGroup trGroup;
        public Image Image { get; protected set; }
        protected DispatcherTimer Dispatcher { get; set; }

        public double positionX { get; protected set; } = 0;
        public double positionY { get; protected set; } = 0;

        public static Image ImageFromName(string name, int left = 10, int top = 10)
        {
            Image frame = new Image();
            BitmapImage tmp = new BitmapImage(new Uri($"immagini/{name}", UriKind.RelativeOrAbsolute));
            frame.Source = new TransformedBitmap(tmp, new ScaleTransform(170 / tmp.Width, 140 / tmp.Height));
            frame.Margin = new Thickness(left, top, 0, 0);

            return frame;
        }

        public void AddToScreen()
        {
            Canvas.Children.Add(Image);
        }

        public Inanimato(Canvas canvas, Image image, DispatcherTimer dispatcher)
        {
            if (image.Source == null) throw new ArgumentException("Image Source cannot be null.");
            if (image.Margin == null) throw new ArgumentException("Image Margin cannot be null.");

            Canvas = canvas;
            Image = image;
            Dispatcher = dispatcher;
            trGroup = new TransformGroup();
            Image.RenderTransform = trGroup;

            positionX = Image.Margin.Left;
            positionY = Image.Margin.Top;
            AddToScreen();
            instances.Add(this);
        }
    }

}
