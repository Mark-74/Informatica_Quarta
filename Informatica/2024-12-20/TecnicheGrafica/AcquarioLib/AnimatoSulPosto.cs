﻿/*
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

namespace AcquarioLib
{
    public class AnimatoSulPosto : Inanimato
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas">The Main Canvas</param>
        /// <param name="image">The image to be shown</param>
        /// <param name="dispatcher">The dispatcher that manages the movements of the object</param>
        public AnimatoSulPosto(Canvas canvas, Image image, DispatcherTimer dispatcher) 
            : base(canvas, image, dispatcher) { }

        /// <summary>
        /// Puts the image on the bottom keeping its margin from the left
        /// </summary>
        public void MoveToBottom()
        {
            //Image.Margin = new Thickness(Image.Margin.Left, Canvas.Height - Image.RenderSize.Height, Image.Margin.Right, 0);
            Canvas.SetTop(Image, Canvas.ActualHeight - Image.ActualHeight - 10);
            positionY = Canvas.ActualHeight - Image.ActualHeight;
        }

        /// <summary>
        /// flips the image
        /// </summary>
        public void Flip()
        {
            //Image.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform flip = new ScaleTransform(-1, 1);
            trGroup.Children.Add(flip);
        }

        /// <summary>
        /// Moves the image to the bottom and flips it, to be used in dispatcher event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private void Animate(object sender, EventArgs a)
        {
            MoveToBottom();
            Flip();
        }

        /// <summary>
        /// Function to be called after creating the object, starts the animation
        /// </summary>
        public override void Start()
        {
            //set new origin to flip correctly
            Image.RenderTransformOrigin = new Point(0.5, 0.5);

            Dispatcher.Tick += new EventHandler(Animate);
        }
    }
}
