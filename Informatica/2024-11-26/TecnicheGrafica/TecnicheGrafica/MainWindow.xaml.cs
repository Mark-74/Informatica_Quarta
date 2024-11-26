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

        int x = 0;
        int y = 0;
        double scaleX = 1.0;
        double scaleY = 1.0;
        int degrees = 0;

        /// <summary>
        /// Setup of the dispatcher Timer, it is called only once when the application starts
        /// </summary>
        private void SetupTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
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
            Uri source = new Uri("immagini/foto_pesce_palla.png", UriKind.RelativeOrAbsolute);
            immagine = new Image();
            BitmapImage tmp = new BitmapImage(source);
            immagine.Source = new TransformedBitmap(tmp, new ScaleTransform(170 / tmp.Width, 140 / tmp.Height));
            immagine.Margin = new Thickness(10, 10, 0, 0);
            canvasAcquario.Children.Add(immagine);
        }
        public MainWindow()
        {
            InitializeComponent();
            SetupTimer();
            AggiungiOggetti();
        }

        private void btnTranslate_Click(object sender, RoutedEventArgs e)
        {
            TranslateTransform translateTransform = new TranslateTransform(--x, ++y);
            immagine.RenderTransform = translateTransform;
        }

        private void btnRotate_Click(object sender, RoutedEventArgs e)
        {
            RotateTransform rotateTransform = new RotateTransform(++degrees);
            immagine.RenderTransform = rotateTransform;
        }

        private void btnScale_Click(object sender, RoutedEventArgs e)
        {
            ScaleTransform scaleTransform = new ScaleTransform(scaleX += 0.1, scaleY += 0.1);
            immagine.RenderTransform = scaleTransform;

        }
    }
}