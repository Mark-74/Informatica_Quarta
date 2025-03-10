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

namespace progress_Bar_Async_3
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Funzione non bloccante che Modifica lo stato della ProgressBar2
        /// </summary>
        /// <param name="tempo">tempo in millisecondi che deve durare la progressBar</param>
        private async Task<int> StartProgressAsyncBar02(int tempo)
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

            return random.Next(0, 2) == 0 ? 0x1337 : 0x69420;
        }

        /// <summary>
        /// Funzione non bloccante che Modifica lo stato della ProgressBar1
        /// </summary>
        /// <param name="tempo">tempo in millisecondi che deve durare la progressBar</param>
        private async Task<int> StartProgressAsyncBar01(int tempo)
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

            return random.Next(0, 2) == 0 ? 0x1337 : 0x69420;
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
            Task<int> TaskProgressBar1 = StartProgressAsyncBar01(700);
            Task<int> TaskProgressBar2 = StartProgressAsyncBar02(500);

            await Task.Delay(2000);


            // aspettiamo la fine del task 2 (se non è ancora terminato) per raccogliere il valore restituito
            int cas2 = await TaskProgressBar2;
            txtSync02.Text = "Restituito " + cas2.ToString();

            await Task.Delay(3000); // simulate long operation

            // aspettiamo la fine del task 1 (se non è ancora terminato) per raccogliere il valore restituito
            int cas1 =  await TaskProgressBar1;
            txtSync01.Text = "Restituito " + cas1.ToString();

            btn_Start.IsEnabled = true;
        }
    }
}
