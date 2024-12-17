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
using Microsoft.Windows.Themes;

namespace AcquarioLib
{
    public class AnimatoPilotatoSilurante : AnimatoPilotato
    {
        private Image Bullet { get; }
        private List<(Image, bool)> Bullets { get; } = new List<(Image, bool)>();
        public AnimatoPilotatoSilurante(Canvas canvas, Image image, DispatcherTimer dispatcher, Window mainWindow, Image bullet, int movementX = 5, int movementY = 5)
            : base(canvas, image, dispatcher, mainWindow, movementX, movementY)
        {
            Bullet = bullet;
        }

        protected override void GestoreComandi(object sender, KeyEventArgs e)
        {
            base.GestoreComandi(sender, e);
            switch (e.Key)
            {
                case Key.Space:
                    Shoot();
                    break;
            }
        }

        public void UpdateSiluri(Object sender, EventArgs e)
        {
            for(int i = 0; i < Bullets.Count; i++)
            {
                (Image img, bool isFacingRight) item = Bullets[i];
                if (item.isFacingRight)
                    Canvas.SetLeft(item.img, Canvas.GetLeft(item.img) + 5);
                else
                    Canvas.SetLeft(item.img, Canvas.GetLeft(item.img) - 5);

                Debug.WriteLine($"{Canvas.GetLeft(item.img)}, {Canvas.ActualWidth}");

                if (Canvas.GetLeft(item.img) > Canvas.ActualWidth || Canvas.GetLeft(item.img) < - item.img.ActualWidth)
                {
                    Canvas.Children.Remove(item.img);
                    Bullets.Remove(item);
                }
            }
        }

        private void Shoot()
        {
            Image bullet = new Image();
            bullet.Source = Bullet.Source;
            bullet.Width = Bullet.Width;
            bullet.Height = Bullet.Height;
            if (!FacingRight)
            {
                bullet.RenderTransformOrigin = new Point(0.5, 0.5);
                bullet.RenderTransform = new ScaleTransform(-1, 1);
            }
            bullet.Margin = new Thickness(FacingRight ? positionX + Image.ActualWidth : positionX - Image.ActualWidth, positionY + 5, 0, 0);
            bullet.Name = "bullet";
            Canvas.Children.Add(bullet);
            Bullets.Add((bullet, FacingRight));

            //idk it just works
            foreach (var Child in Canvas.Children)
            {
                Image img = Child as Image;
                if (img is not null)
                {
                    if (img.Name == "bullet" && double.IsNaN(Canvas.GetLeft(img)))
                    {
                        Canvas.SetLeft(img, positionX);
                    }
                }
            }
        }

        public override void start()
        {
            base.start();
            Dispatcher.Tick += new EventHandler(UpdateSiluri);
        }
    }
}
