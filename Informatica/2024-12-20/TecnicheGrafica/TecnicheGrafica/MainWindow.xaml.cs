/*
 * 
 * Marco Balducci 4H 2024-11-22
 * Wpf App that resembles an acquarium with a controllable player and moving objects
 * 
*/

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
using Accessibility;

namespace TecnicheGrafica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Global variables
        DispatcherTimer FishDispatcher, PropsDispatcher;

        /// <summary>
        /// Setup of the dispatcher Timer, it is called only once when the application starts
        /// </summary>
        private void SetupTimer()
        {
            FishDispatcher = new DispatcherTimer();
            PropsDispatcher = new DispatcherTimer();
            FishDispatcher.Interval = TimeSpan.FromMilliseconds(40);
            PropsDispatcher.Interval = TimeSpan.FromMilliseconds(500);
            FishDispatcher.Start();
            PropsDispatcher.Start();

        }

        private void AggiungiOggetti()
        {
            //add some objects
            AnimatoInAcqua animato = new AnimatoInAcqua(canvasAcquario, Inanimato.ImageFromName("foto_pesce_palla.png"), FishDispatcher);
            animato.ChangeCenterOfRotation(100, 100);

            AnimatoSulPosto alga1 = new AnimatoSulPosto(canvasAcquario, Inanimato.ImageFromName("alga1.png"), PropsDispatcher);

            AnimatoPilotatoSilurante sub = new AnimatoPilotatoSilurante(canvasAcquario, Inanimato.ImageFromName("submarine.png"), FishDispatcher, this, Inanimato.ImageFromName("siluro.png"), bulletSpeed: 20);

            AnimatoSulFondo granchio = new AnimatoSulFondo(canvasAcquario, Inanimato.ImageFromName("crab.png"), FishDispatcher);

            AnimatoInAcqua carpa = new AnimatoInAcqua(canvasAcquario, Inanimato.ImageFromName("carp.png"), FishDispatcher, 20);

            AnimatoSulPosto alga2 = new AnimatoSulPosto(canvasAcquario, Inanimato.ImageFromName("alga2.png", 1125), PropsDispatcher);

            AnimatoInAcqua siluro = new AnimatoInAcqua(canvasAcquario, Inanimato.ImageFromName("pesceSiluro.png", left: 50, top: 100), FishDispatcher, 20, 0);

            //start each object's animation
            sub.Start();
            animato.Start();
            alga1.Start();
            granchio.Start();
            carpa.Start();
            alga2.Start();
            siluro.Start();
        }

        public MainWindow()
        {
            InitializeComponent();
            SetupTimer();
            AggiungiOggetti();
        }

    }
}