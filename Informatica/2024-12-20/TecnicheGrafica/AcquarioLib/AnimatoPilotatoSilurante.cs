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
using Microsoft.Windows.Themes;

namespace AcquarioLib
{
    public class AnimatoPilotatoSilurante : AnimatoPilotato
    {
        /// <summary>
        /// Bullet image
        /// </summary>
        private Image Bullet { get; }

        //The speed of the bullets
        private int BulletSpeed { get; set; }

        /// <summary>
        /// List of launched bullets
        /// </summary>
        private List<(Image, bool)> Bullets { get; } = new List<(Image, bool)>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas">The Main Canvas</param>
        /// <param name="image">The image to be shown</param>
        /// <param name="dispatcher">The dispatcher that manages the movements of the object</param>
        /// <param name="mainWindow">The window where the program in being run</param>
        /// <param name="bullet">The image that represents the bullet shot by the player</param>
        /// <param name="movementX">defines the movement to be made along the X axis for each tick</param>
        /// <param name="movementY">defines the movement to be made along the Y axis for each tick</param>
        /// <param name="bulletSpeed">the speed of the bullet</param>
        public AnimatoPilotatoSilurante(Canvas canvas, Image image, DispatcherTimer dispatcher, Window mainWindow, Image bullet, int movementX = 5, int movementY = 5, int bulletSpeed = 5)
            : base(canvas, image, dispatcher, mainWindow, movementX, movementY)
        {
            Bullet = bullet;
            BulletSpeed = bulletSpeed;
        }

        /// <summary>
        /// variables for reload counter
        /// </summary>
        private int cooldown = 15;
        private int cooldownCounter = 15;

        /// <summary>
        /// Static method that returns a point in the middle of the image
        /// </summary>
        /// <param name="img">the image to retrieve the center of</param>
        /// <returns></returns>
        private Point ImageToPoint(Image img) => new Point(Canvas.GetLeft(img) + img.ActualWidth/2, Canvas.ActualHeight - Canvas.GetTop(img) + img.ActualHeight/2);
        /// <summary>
        /// Overload of ImageToPoint, retrieves the center using the X and Y parameters instead of the position retrieved from the Canvas
        /// </summary>
        /// <param name="img">the image to retrieve the center of</param>
        /// <param name="X">The X axis position</param>
        /// <param name="Y">The Y axis Position</param>
        /// <param name="Y">The Y axis Position</param>
        /// <returns></returns>
        private static Point ImageToPoint(Image img, double X, double Y) => new Point(X + img.ActualWidth / 2, Y + img.ActualHeight / 2);

        /// <summary>
        /// Checks if 2 points are close enough to be considered a hit
        /// </summary>
        /// <param name="Fish">The point representing the Fish</param>
        /// <param name="bullet">The point representig the bullet</param>
        /// <returns>true if the collision is confirmed, false otherwise</returns>
        private static bool IsCollision(Point Fish, Point bullet)
        {
            Debug.Write(Math.Abs(Fish.X - bullet.X) <= 100 && Math.Abs(Fish.Y - bullet.Y) <= 100);

            //if the bullets are 100 pixels close both vertically and horizzontally then the hit is confirmed
            return Math.Abs(Fish.X - bullet.X) <= 100 && Math.Abs(Fish.Y - bullet.Y) <= 100;
        }

        /// <summary>
        /// Checks if there is a hit for each frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckHits(Object sender, EventArgs e)
        {
            //iterate over each bullet
            for (int i = 0; i < Bullets.Count; i++)
            {
                //Get center of the bullet
                (Image img, bool isFacingRight) bullet = Bullets[i];
                Point bulletPoint = ImageToPoint(bullet.img);
                Debug.WriteLine($"bullet: {bulletPoint}");

                //Iterate over each instance of Inanimato
                for (int j = 0; j < instances.Count; j++)
                {
                    //Get center of Inanimato
                    Inanimato inanimato = instances[j];
                    Point fishPoint = ImageToPoint(inanimato.Image, inanimato.positionX, Canvas.ActualHeight - inanimato.positionY);
                    Debug.WriteLine($"fish: {fishPoint}");

                    //if the current Inanimato is the player, skip
                    if (inanimato is AnimatoPilotatoSilurante) continue;

                    //if there is a collision, remove bullet and fish
                    if (IsCollision(fishPoint, bulletPoint))
                    {
                        Canvas.Children.Remove(inanimato.Image);
                        instances.Remove(inanimato);
                        Canvas.Children.Remove(bullet.img);
                        Bullets.Remove(bullet);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the space key to shoot to The regular InputManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void InputManager(object sender, KeyEventArgs e)
        {
            base.InputManager(sender, e);
            switch (e.Key)
            {
                case Key.Space:
                    Shoot();
                    break;
            }
        }

        /// <summary>
        /// Function that, added to the dispatcher, updates each torpedo's position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateBullets(Object sender, EventArgs e)
        {
            //update reload time
            cooldownCounter++;

            //iterate over each bullet
            for(int i = 0; i < Bullets.Count; i++)
            {
                //Move bullet to the right if it is facing right, otherwise to the left
                (Image img, bool isFacingRight) item = Bullets[i];
                if (item.isFacingRight)
                    Canvas.SetLeft(item.img, Canvas.GetLeft(item.img) + BulletSpeed);
                else
                    Canvas.SetLeft(item.img, Canvas.GetLeft(item.img) - BulletSpeed);

                Debug.WriteLine($"{Canvas.GetLeft(item.img)}, {Canvas.ActualWidth}");

                //If bullet is out of the canvas remove it
                if (Canvas.GetLeft(item.img) > Canvas.ActualWidth || Canvas.GetLeft(item.img) < - item.img.ActualWidth)
                {
                    Canvas.Children.Remove(item.img);
                    Bullets.Remove(item);
                }
            }
        }

        /// <summary>
        /// Adds a bullet to the canvas and its refresh to the Dispatcher
        /// </summary>
        private void Shoot()
        {
            //check reload time
            if(cooldownCounter < cooldown) return;
            else cooldownCounter = 0;

            //Copy bullet image
            Image bullet = new Image();
            bullet.Source = Bullet.Source;
            bullet.Width = Bullet.Width;
            bullet.Height = Bullet.Height;

            //flip the image if the submarine is not facing right at the time of shooting
            if (!FacingRight)
            {
                bullet.RenderTransformOrigin = new Point(0.5, 0.5);
                bullet.RenderTransform = new ScaleTransform(-1, 1);
            }

            //add name to bullet and add bullet to the Canvas
            bullet.Name = "bullet";
            Canvas.Children.Add(bullet);
            Bullets.Add((bullet, FacingRight));

            //Update the position of the newly added bullet
            foreach (var Child in Canvas.Children)
            {
                //get bullet image and check if it is an image
                Image img = Child as Image;
                if (img is not null)
                {
                    //if image is not bullet or already has a position skip
                    if (img.Name == "bullet" && double.IsNaN(Canvas.GetLeft(img)))
                    {
                        //Set top position and left position considering the direction (FacingRight)
                        Canvas.SetTop(img, Canvas.GetTop(Image) + Image.ActualHeight / 4);
                        Canvas.SetLeft(img, FacingRight ? Canvas.GetLeft(Image) + Image.ActualWidth : Canvas.GetLeft(Image) - Image.ActualWidth);
                    }
                }
            }
        }

        /// <summary>
        /// Function to be called after creating the object, links the input manager to the window
        /// </summary>
        public override void Start()
        {
            base.Start();
            //update submarine name
            Image.Name = "player";

            //add events to Dispatcher
            Dispatcher.Tick += new EventHandler(UpdateBullets);
            Dispatcher.Tick += new EventHandler(CheckHits);
        }
    }
}
