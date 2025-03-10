/*
 * Marco Balducci 4H 2025/03/03 
 * Wpf app che applica filtri a immagini
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

namespace FiltriAsyncWpf
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Global variables
        private static string _fileName;

        public MainWindow()
        {
            InitializeComponent();
            AddCheckerToFilterInput();
            Title = "Marco Balducci 4H";
        }

        #region Input Check
        /// <summary>
        /// This function is called at the start of the program to add input checks to all the input boxes
        /// </summary>
        private void AddCheckerToFilterInput()
        {
            //cicle over each input box
            foreach (TextBox item in grid_filter.Children)
            {
                //add event to each input box
                item.TextChanged += (object sender, TextChangedEventArgs e) =>
                {
                    //check input
                    if (item.Text.Length > 0)
                    {
                        //if user inserts - the check awaits
                        if (item.Text.Length == 1 && item.Text[0] == '-')
                            return;

                        //if the parse fails tell the user
                        double value;
                        if (!double.TryParse(item.Text, out value))
                            MessageBox.Show($"The value '{item.Text}' is not a number.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                };
            }
        }

        /// <summary>
        /// Checks if there are any non number inputs
        /// </summary>
        /// <returns>true if the check is successfull, false otherwise</returns>
        private bool CheckFilterTexts()
        {
            bool result = true;
            //cicle over each input box
            foreach (TextBox item in grid_filter.Children)
            {
                //try to parse each input, if one parse fails tell the user
                double value;
                result = double.TryParse(item.Text, out value);
                if (!result)
                {
                    //tell the user the coordiates of the failed parse
                    MessageBox.Show($"The filter box at position [{item.Name[7] + 1}, {item.Name[8] + 1}] is not a number, Please change it.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                }
            }

            //if even one result fails, result is false
            return result;
        }

        #endregion

        #region Calculate Convolution
        /// <summary>
        /// Transforms the filter into a double 3x3 matrix
        /// </summary>
        /// <returns>the parsed matrix</returns>
        /// <exception cref="ArgumentException">thrown if the check fails</exception>
        private double[,] GetFilter()
        {
            //check input boxes
            if (!CheckFilterTexts()) throw new ArgumentException("The number check for the filter has failed.");

            //parse the input boxes into a 3x3 matrix
            double[,] filter = new double[,]
            {
                { double.Parse(filter_00.Text), double.Parse(filter_01.Text), double.Parse(filter_02.Text) },
                { double.Parse(filter_10.Text), double.Parse(filter_11.Text), double.Parse(filter_12.Text) },
                { double.Parse(filter_20.Text), double.Parse(filter_21.Text), double.Parse(filter_22.Text) },
            };

            return filter;
        }

        /// <summary>
        /// Applies the convolution to the given image
        /// </summary>
        /// <param name="img">the image to work on</param>
        /// <param name="filter">the double 3x3 matrix that acts as the filter</param>
        /// <returns></returns>
        private System.Drawing.Bitmap Convolution(System.Drawing.Bitmap img, double[,] filter)
        {
            //create new empty bitmap with same dimensions as original
            System.Drawing.Bitmap imgRisultato = new System.Drawing.Bitmap(img.Width, img.Height);

            //cicle over each pixel
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    //calculate convolution for each pixel
                    imgRisultato.SetPixel(i, j, CalculateSingleConvolution(img, i, j, filter));
                }
            }

            return imgRisultato;
        }

        /// <summary>
        /// Calculates the convolution for a single pixel
        /// </summary>
        /// <param name="img">the image to work on</param>
        /// <param name="x">the x coordinate of the pixel</param>
        /// <param name="y">the y coordinate of the pixel</param>
        /// <param name="filter">the double 3x3 matrix</param>
        /// <returns></returns>
        private System.Drawing.Color CalculateSingleConvolution(System.Drawing.Bitmap img, int x, int y, double[,] filter)
        {
            //pixel variables
            int R = 0, G = 0, B = 0;
            int offset = 1; // for 3x3 matrix

            for (int i = -offset; i <= offset; i++)
            {
                for (int j = -offset; j <= offset; j++)
                {
                    //calculate current coordinates
                    int px = x + i;
                    int py = y + j;

                    //Check bounds
                    if (px >= 0 && px < img.Width && py >= 0 && py < img.Height)
                    {
                        //get RGB color from pixel and calculate weight
                        System.Drawing.Color pixel = img.GetPixel(px, py);
                        int weight = (int)filter[i + offset, j + offset];

                        //Accumulate the weighted sum for R, G, and B channels
                        R += pixel.R * weight;
                        G += pixel.G * weight;
                        B += pixel.B * weight;
                    }
                }
            }

            //Clamp the values to [0, 255] range
            R = Math.Min(Math.Max(R, 0), 255);
            G = Math.Min(Math.Max(G, 0), 255);
            B = Math.Min(Math.Max(B, 0), 255);

            //return the new calculated color
            return System.Drawing.Color.FromArgb(R, G, B);
        }

        /// <summary>
        /// Saves the image after the convolution
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private ImageSource BitmapToBitmapSource(Bitmap source)
        {
            using (MemoryStream memory = new MemoryStream()) // Create a new memory stream
            {
                source.Save(memory, System.Drawing.Imaging.ImageFormat.Png); // Save the source image as a PNG format into the memory stream
                memory.Position = 0; // Reset the position of the memory stream to the beginning
                BitmapImage bitmapImage = new BitmapImage(); // Create a new BitmapImage
                bitmapImage.BeginInit(); // Begin the initialization of the BitmapImage
                bitmapImage.StreamSource = memory; // Set the stream source of the BitmapImage to the memory stream
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Cache the image into memory to allow for better performance and offline usage
                bitmapImage.EndInit(); // End the initialization of the BitmapImage
                return bitmapImage; // Return the created BitmapImage
            }

        }

        /// <summary>
        /// Transforms the image with the current settings
        /// </summary>
        private async void TransformImage()
        {
            try
            {
                double[,] matConvol = GetFilter(); //get the filter
                System.Drawing.Bitmap imgOriginal = new System.Drawing.Bitmap(_fileName); //cast image to Bitmap
                img_current_image.Source = BitmapToBitmapSource(Convolution(imgOriginal, matConvol)); //set the result as the source of the image
            } 
            catch (Exception ex)
            {
                //if any error occurs, make the user know but don't crash the program
                MessageBox.Show(ex.Message, "An error has occured", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        #endregion

        #region button events

        /// <summary>
        /// event handler for the btn_apply button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            TransformImage();
        }

        /// <summary>
        /// event handler for the btn_load button, lets the user upload an image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            //create new dialog window
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            //set file extensions filter
            dlg.DefaultExt = ".png";
            dlg.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp; *.tiff)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff;";

            //get result of the action
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                //get the image and set it as source of the image displayer in the xaml
                _fileName = dlg.FileName;
                BitmapImage bitMap = new BitmapImage(new Uri(_fileName));
                lbl_fileName.Content = _fileName;
                img_current_image.Source = bitMap;
            }
        }
        #endregion
    }
}
