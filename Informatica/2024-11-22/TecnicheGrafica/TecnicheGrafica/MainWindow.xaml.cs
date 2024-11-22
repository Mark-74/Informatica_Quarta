//Marco Balducci 4H 2024-11-22

using System;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TecnicheGrafica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Global variables
        DispatcherTimer dispatcherTimer;

        Image immagine;

        /// <summary>
        /// Setup of the dispatcher Timer, it is called only once when the application starts
        /// </summary>
        private void SetupTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(300);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick); //method to be called periodically
            dispatcherTimer.Start();
        }
        int i = 0;
        Random random = new Random();
        
        /// <summary>
        /// Function called by the dispatcherTimer every Tick, it updates all the animations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs a)
        {
            ++i;
            txtContatore.Text = i.ToString();
        }

        private void AggiungiOggetti()
        {
            Uri source = new Uri("immagini/foto_pesce palla.jpg", UriKind.RelativeOrAbsolute);
            immagine = new Image();
            immagine.Source = new BitmapImage(source);
            immagine.Margin = new Thickness(50, 50, 100, 100);
            canvasAcquario.Children.Add(immagine);
        }
        public MainWindow()
        {
            InitializeComponent();
            SetupTimer();
            AggiungiOggetti();
        }
    }
}