/*
 * Marco Balducci 4H 2025-02-24
 * Applicazione 
*/
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FiltriAsyncWpfAPP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _fileName;

        public MainWindow()
        {
            InitializeComponent();
            AddCheckerToFilterInput();
        }

        private void AddCheckerToFilterInput()
        {
            foreach (TextBox item in grid_filter.Children)
            {
                item.TextChanged += (object sender, TextChangedEventArgs e) =>
                {
                    if(item.Text.Length > 0)
                    {
                        if (item.Text.Length == 1 && item.Text[0] == '-')
                            return;

                        double value;
                        if (!double.TryParse(item.Text, out value))
                            MessageBox.Show($"The value '{item.Text}' is not a number.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                };
            }
        }

        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp; *.tiff; *.webp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff;*.webp";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                _fileName = dlg.FileName;
                BitmapImage bitMap = new BitmapImage(new Uri(_fileName));
                lbl_fileName.Content = _fileName;
                img_current_image.Source = bitMap;
            }
        }

        private bool CheckFilterTexts()
        {
            bool result = true;
            foreach (TextBox item in grid_filter.Children)
            {
                double value;
                result = double.TryParse(item.Text, out value);
                if (!result)
                    MessageBox.Show($"The filter box at position [{item.Name[7]+1}, {item.Name[8]+1}] is not a number, Please change it.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return result;
        }

        private int[,] GetFilter()
        {
            if (!CheckFilterTexts()) throw new ArgumentException("The number check for the filter has failed.");

            int[,] filter = new int[,]
            {
                { int.Parse(filter_00.Text), int.Parse(filter_01.Text), int.Parse(filter_02.Text) },
                { int.Parse(filter_10.Text), int.Parse(filter_11.Text), int.Parse(filter_12.Text) },
                { int.Parse(filter_20.Text), int.Parse(filter_21.Text), int.Parse(filter_22.Text) },
            };

            return filter;
        }

        private BitmapImage Convolution(BitmapImage img, int[,] filter)
        {
            //byte[,] resultMatrix = new byte[]
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    
                }
            }

            return new BitmapImage();
        }

        private async void TransformImage()
        {
            int[,] matConvol = GetFilter();
            BitmapImage imgOriginal = new BitmapImage(new Uri(_fileName));
            img_current_image.Source = Convolution(imgOriginal, matConvol);
        }

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            TransformImage();
        }
    }
}