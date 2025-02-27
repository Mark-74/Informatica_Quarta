﻿//Marco Balducci 4H 2024-11-22

using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Runtime.CompilerServices;
using AcquarioLib;

namespace TecnicheGrafica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Global variables
        DispatcherTimer dispatcherTimer;

        AnimatoSulFondo anim;

        double scaleX = 1.0;
        double scaleY = 1.0;
        double degrees = 0.0;

        /// <summary>
        /// Setup of the dispatcher Timer, it is called only once when the application starts
        /// </summary>
        private void SetupTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispatcherTimer.Start();
            
        }
        int i = 0;
        Random random = new Random();

        private void AggiungiOggetti()
        {
            Image immagine = new Image();
            Uri source = new Uri("immagini/foto_pesce_palla.png", UriKind.RelativeOrAbsolute);
            immagine = new Image();
            BitmapImage tmp = new BitmapImage(source);
            immagine.Source = new TransformedBitmap(tmp, new ScaleTransform(170 / tmp.Width, 140 / tmp.Height));
            immagine.Margin = new Thickness(10, 10, 0, 0);
            
            anim = new AnimatoInAcqua(canvasAcquario, immagine, dispatcherTimer);
            double x = canvasAcquario.ActualHeight;
            anim.ChangeCenterOfRotation(100, 100);
            anim.AddToScreen();

            Image frame1 = new Image();
            frame1.Source = new TransformedBitmap(new BitmapImage(new Uri("immagini/alga1.png", UriKind.RelativeOrAbsolute)), new ScaleTransform());
            frame1.Margin = new Thickness(10, 10, 0, 0);
            AnimatoSulPosto alga = new AnimatoSulPosto(canvasAcquario, frame1, dispatcherTimer);
            alga.ChangeCenterOfRotation(20, 20);
            alga.AddToScreen();
            anim.start();
            alga.Start();
        }
        public MainWindow()
        {
            InitializeComponent();
            SetupTimer();
            AggiungiOggetti();
        }

    }
}