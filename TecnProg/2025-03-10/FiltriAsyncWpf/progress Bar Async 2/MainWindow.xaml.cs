/*
 * Marco Balducci 4H 10/03/2025 
 * Progress bar
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace progress_Bar_Async_2
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
        private async Task StartProgressAsyncBar02(int tempo)
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
        }

        /// <summary>
        /// Funzione non bloccante che Modifica lo stato della ProgressBar1
        /// </summary>
        /// <param name="tempo">tempo in millisecondi che deve durare la progressBar</param>
        private async Task StartProgressAsyncBar01(int tempo)
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
        }

        /// <summary>
        /// Handler del click del bottone. Fa partire le progressBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            btn_Start.IsEnabled = false;

            // definire un task lo fa anche partire
            Task TaskProgressBar1 = StartProgressAsyncBar01(700);

            await Task.Delay(800);

            Task TaskProgressBar2 = StartProgressAsyncBar02(500);

            

            // bloccante, aspettiamo la fine dei task
            await TaskProgressBar1; 
            await TaskProgressBar2;

            btn_Start.IsEnabled = true;
        }
    }
}
