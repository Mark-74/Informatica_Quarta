/*
 * Marco Balducci 4H 10/03/2025 
 * Progress bar
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace progress_bar_Async
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Funzione non bloccante che Modifica lo stato della ProgressBar2
        /// </summary>
        /// <param name="tempo">tempo in millisecondi che deve durare la progressBar</param>
        private async void StartProgressAsyncBar02(int tempo)
        {
            // Setup
            txtSync02.Text = "Start ProgressBar";
            prgBar02.Minimum = 0;
            prgBar02.Maximum = tempo;

            // Update progressBar
            for (int i = 0; i <= tempo; i++)
            {
                prgBar02.Value = i;
                await Task.Delay(1);
            }
            txtSync02.Text = "End ProgressBarAsync";

            // Controllo che entrambe le progressBar abbiano terminato, in tal caso sblocco il bottone
            if (txtSync01.Text == "End ProgressBarAsync")
                btn_Start.IsEnabled = true;
        }

        /// <summary>
        /// Funzione non bloccante che Modifica lo stato della ProgressBar1
        /// </summary>
        /// <param name="tempo">tempo in millisecondi che deve durare la progressBar</param>
        private async void StartProgressAsyncBar01(int tempo)
        {
            // Setup
            txtSync01.Text = "Start ProgressBar";
            prgBar01.Minimum = 0;
            prgBar01.Maximum = tempo;

            // Update progressBar
            for (int i = 0; i <= tempo; i++)
            {
                prgBar01.Value = i;
                await Task.Delay(1);
            }
            txtSync01.Text = "End ProgressBarAsync";

            // Controllo che entrambe le progressBar abbiano terminato, in tal caso sblocco il bottone
            if(txtSync02.Text == "End ProgressBarAsync")
                btn_Start.IsEnabled = true;
        }

        /// <summary>
        /// Handler del click del bottone. Fa partire le progressBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            btn_Start.IsEnabled = false;

            StartProgressAsyncBar01(700); // Start progressBar1 -> non bloccante
            await Task.Delay(800);        // Await 800          ->     bloccante
            StartProgressAsyncBar02(500); // Start progressBar2 -> non bloccante
        }
    }
}
