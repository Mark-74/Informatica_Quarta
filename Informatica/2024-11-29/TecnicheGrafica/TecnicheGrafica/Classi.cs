//Marco Balducci 4H 2024-11-29 Questo file verrà spostato in un progetto condiviso, contiene le classi per animare le immagini

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TecnicheGrafica
{
    class Inanimato
    {
        protected Canvas Canvas;
        public Image Image { get; protected set; }

        public void AddToScreen() => Canvas.Children.Add(Image);

        public Inanimato(Canvas canvas, Image image)
        {
            if (image.Source == null) throw new ArgumentException("Image Source cannot be null.");
            if (image.Margin == null) throw new ArgumentException("Image Margin cannot be null.");

            Canvas = canvas;
            Image = image;
        }
    }

    class AnimatoSulPosto : Inanimato
    {
        protected double degrees = 0.0;
        public AnimatoSulPosto(Canvas canvas, Image image)
            : base(canvas, image)
        { }

        public void Rotate()
        {
            RotateTransform r = new RotateTransform(++degrees);
            Image.RenderTransform = r;
        }
    }

    class AnimatoSulFondo : AnimatoSulPosto
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }
        public AnimatoSulFondo(Canvas canvas, Image image, int leftOffset = 0)
            : base(canvas, image)
        {
            X = Math.Abs(leftOffset);
            Y = 0;
            Thickness t = new Thickness(10 + X, 10 + Y, 0, 0);
            image.Margin = t;
        }

        public void Move(bool right)
        {
            TranslateTransform translateTransform = new TranslateTransform(right ? 1 : -1, 0);
            Image.RenderTransform = translateTransform;
        }
    }

    class AnimatoInAcqua : AnimatoSulFondo
    {
        public AnimatoInAcqua(Canvas canvas, Image image, int leftOffset = 0, int topOffset = 0)
            : base(canvas, image, leftOffset)
        {
            Y = Math.Abs(topOffset);
            Thickness t = new Thickness(10 + X, 10 +Y, 0, 0);
            image.Margin = t;
        }

        public void Move(bool right, bool up)
        {
            TranslateTransform translateTransform = new TranslateTransform(X += (right ? 1 : -1), Y += (up ? 1 : -1));
            Image.RenderTransform = translateTransform;
        }
    }

    /*
    class AnimatoPilotato : AnimatoInAcqua
    {

    }

    class AnimatoPilotatoSilurante : AnimatoPilotato
    {

    }
    */
}
