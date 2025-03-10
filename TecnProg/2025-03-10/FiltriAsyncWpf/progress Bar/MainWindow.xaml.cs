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

namespace progress_Bar
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

        private void StartProgressBar02(int tempo)
        {
            txtSync02.Text = "Start ProgressBar";
            prgBar02.Minimum = 0;
            prgBar02.Maximum = tempo;
            for (int i = 0; i <= tempo; i++)
            {
                prgBar02.Value = i;
                Thread.Sleep(1);
            }
            txtSync02.Text = "End ProgressBarAsync";
        }

        private void StartProgressBar01(int tempo)
        {
            txtSync01.Text = "Start ProgressBar";
            prgBar01.Minimum = 0;
            prgBar01.Maximum = tempo;
            for (int i = 0; i <= tempo; i++)
            {
                prgBar01.Value = i;
                Thread.Sleep(1);
            }
            txtSync01.Text = "End ProgressBarAsync";
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            StartProgressBar01(1000);
            StartProgressBar02(1000);
        }
    }
}
