/*
 * Marco Balducci 4H 31/03/2025
 * App wpf che applica filtri ad immagini in modo asincrono
*/
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FiltriWPF3
{
    public partial class MainWindow : Window
    {
        static string _imagePath = ""; // Percorso dell'immagine caricata

        public MainWindow()
        {
            InitializeComponent();
            btnApplyFilter.IsEnabled = false; // Disabilita il pulsante di applicazione del filtro all'avvio
        }

        /// <summary>
        /// Gestisce il click del pulsante "Applica Filtro" eseguendo l'elaborazione asincrona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            await ApplyFilterAsync();
        }

        /// <summary>
        /// Converte una stringa in intero, ritornando 0 in caso di formato non valido
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int ParseInput(string input)
        {
            return int.TryParse(input, out int number) ? number : 0;
        }

        /// <summary>
        /// Apre una finestra di dialogo per caricare un'immagine e la visualizza nell'interfaccia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "Image files (*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff;*.webp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff;*.webp"
            };

            if (dialog.ShowDialog() == true)
            {
                _imagePath = dialog.FileName;
                imgDisplay.Source = new BitmapImage(new Uri(_imagePath));
                lblImageName.Content = _imagePath;
                btnApplyFilter.IsEnabled = true; // Abilita il pulsante dopo il caricamento
            }
            else
            {
                MessageBox.Show("Nessuna immagine selezionata. Selezionare un'immagine valida", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Applica una convoluzione all'immagine utilizzando un filtro specificato, suddividendo il lavoro in task paralleli
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private async Task<Bitmap> ApplyConvolution(Bitmap sourceImage, int[,] filter)
        {
            // Ottiene dimensioni dell'immagine e prepara il risultato in formato 32bpp ARGB  
            int width = sourceImage.Width;
            int height = sourceImage.Height;
            Bitmap resultBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Crea una copia dell'immagine sorgente per evitare modifiche durante il lock  
            Bitmap sourceBitmap = new Bitmap(sourceImage);

            // Blocca i dati dell'immagine sorgente in modalità lettura  
            var sourceData = sourceBitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                sourceBitmap.PixelFormat);

            // Blocca i dati dell'immagine risultante in modalità scrittura  
            var resultData = resultBitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                resultBitmap.PixelFormat);

            try
            {
                // Calcola il numero totale di byte da copiare e prepara gli array di byte  
                int totalBytes = Math.Abs(sourceData.Stride) * height;
                byte[] sourceBytes = new byte[totalBytes];
                byte[] resultBytes = new byte[totalBytes];

                // Copia i dati dell'immagine sorgente nell'array di byte  
                Marshal.Copy(sourceData.Scan0, sourceBytes, 0, totalBytes);

                // Calcola i punti medi per dividere l'immagine in 4 quadranti  
                int midWidth = width / 2;
                int midHeight = height / 2;

                // Configurazione della progressBar per il monitoraggio del progresso  
                progressBar.Minimum = 0;
                progressBar.Maximum = width;
                progressBar.Value = 0;

                // Array per tracciare l'avanzamento delle colonne (aggiornamento sincronizzato)  
                int[] columnCounts = new int[width];
                object syncLock = new object();

                // Creazione di 4 task per elaborare simultaneamente 4 sezioni dell'immagine  
                Task[] tasks = new Task[4];
                tasks[0] = Task.Run(() => ProcessImageSection(sourceBytes, resultBytes, 0, midWidth, 0, midHeight, sourceData.Stride, filter, width, height, columnCounts, syncLock));
                tasks[1] = Task.Run(() => ProcessImageSection(sourceBytes, resultBytes, midWidth, width, 0, midHeight, sourceData.Stride, filter, width, height, columnCounts, syncLock));
                tasks[2] = Task.Run(() => ProcessImageSection(sourceBytes, resultBytes, 0, midWidth, midHeight, height, sourceData.Stride, filter, width, height, columnCounts, syncLock));
                tasks[3] = Task.Run(() => ProcessImageSection(sourceBytes, resultBytes, midWidth, width, midHeight, height, sourceData.Stride, filter, width, height, columnCounts, syncLock));

                // Attende il completamento di tutti i task paralleli  
                await Task.WhenAll(tasks);

                // Copia i byte risultanti nell'immagine di destinazione  
                Marshal.Copy(resultBytes, 0, resultData.Scan0, totalBytes);
            }
            finally
            {
                // Sblocca i dati delle immagini per evitare memory leak  
                sourceBitmap.UnlockBits(sourceData);
                resultBitmap.UnlockBits(resultData);
            }

            return resultBitmap; // Restituisce l'immagine elaborata  
        }

        /// <summary>
        /// Elabora un quadrante dell'immagine applicando il filtro e aggiornando la progressBar in modo thread-safe
        /// </summary>
        /// <param name="sourceBytes"></param>
        /// <param name="resultBytes"></param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <param name="startY"></param>
        /// <param name="endY"></param>
        /// <param name="stride"></param>
        /// <param name="filterMatrix"></param>
        /// <param name="imgWidth"></param>
        /// <param name="imgHeight"></param>
        /// <param name="columnCounts"></param>
        /// <param name="_lock"></param>
        private void ProcessImageSection(
             byte[] sourceBytes, byte[] resultBytes, // Array di byte per immagine sorgente e risultante
             int startX, int endX, int startY, int endY, // Coordinate del quadrante da elaborare
             int stride, // Stride dell'immagine (larghezza in byte di una riga)
             int[,] filterMatrix, // Matrice filtro per la convoluzione
             int imgWidth, int imgHeight, // Dimensioni dell'immagine
             int[] columnCounts, object _lock) // Strutture per il tracking del progresso e sincronizzazione
        {
            int filterRadius = 1;
            int bytesPerPixel = 4;

            // Itera ogni pixel nel quadrante definito da startX, endX, startY, endY
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    int red = 0, green = 0, blue = 0; // RGB per i canali colore

                    // Cicla attraverso i pixel del filtro (es. 3x3) centrato su (x,y)
                    for (int ky = -filterRadius; ky <= filterRadius; ky++)
                    {
                        for (int kx = -filterRadius; kx <= filterRadius; kx++)
                        {
                            int pixelX = x + kx; // Coordinata X del pixel del filtro
                            int pixelY = y + ky; // Coordinata Y del pixel del filtro

                            // Verifica che il pixel del filtro sia all'interno dei bordi dell'immagine
                            if (pixelX >= 0 && pixelX < imgWidth && pixelY >= 0 && pixelY < imgHeight)
                            {
                                // Calcola la posizione nell'array dei byte del pixel sorgente
                                int sourceIndex = (pixelY * stride) + (pixelX * bytesPerPixel);

                                // Estrae i componenti BGR
                                byte b = sourceBytes[sourceIndex];
                                byte g = sourceBytes[sourceIndex + 1];
                                byte r = sourceBytes[sourceIndex + 2];

                                // Recupera il valore del filtro e aggiorna gli accumulatori
                                int kernelValue = filterMatrix[ky + filterRadius, kx + filterRadius];
                                red += r * kernelValue;
                                green += g * kernelValue;
                                blue += b * kernelValue;
                            }
                        }
                    }

                    // Clamping dei valori dei canali nell'intervallo [0, 255]
                    red = red < 0 ? 0 : (red > 255 ? 255 : red);
                    green = green < 0 ? 0 : (green > 255 ? 255 : green);
                    blue = blue < 0 ? 0 : (blue > 255 ? 255 : blue);

                    // Calcola la posizione nell'array dei byte del pixel risultante
                    int resultIndex = (y * stride) + (x * bytesPerPixel);
                    resultBytes[resultIndex] = (byte)blue;
                    resultBytes[resultIndex + 1] = (byte)green;
                    resultBytes[resultIndex + 2] = (byte)red;
                    resultBytes[resultIndex + 3] = 255;
                }

                // Aggiorna la progressBar in modo thread-safe
                lock (_lock)
                {
                    columnCounts[x]++;
                    if (columnCounts[x] == 2)
                    {
                        // Usa il dispatcher di wpf per evitare errori di asincronismo
                        Application.Current.Dispatcher.Invoke(() => progressBar.Value++);
                    }
                }
            }
        }

        /// <summary>
        /// Gestisce l'applicazione asincrona del filtro dopo aver letto i valori del kernel dall'interfaccia
        /// </summary>
        /// <returns></returns>
        private async Task ApplyFilterAsync()
        {
            int[,] filterMatrix = new int[3, 3];

            // Legge i valori del filtro dalle TextBox dell'interfaccia
            filterMatrix[0, 0] = ParseInput(txt00.Text);
            filterMatrix[0, 1] = ParseInput(txt01.Text);
            filterMatrix[0, 2] = ParseInput(txt02.Text);
            filterMatrix[1, 0] = ParseInput(txt10.Text);
            filterMatrix[1, 1] = ParseInput(txt11.Text);
            filterMatrix[1, 2] = ParseInput(txt12.Text);
            filterMatrix[2, 0] = ParseInput(txt20.Text);
            filterMatrix[2, 1] = ParseInput(txt21.Text);
            filterMatrix[2, 2] = ParseInput(txt22.Text);

            try
            {
                Bitmap originalImage = new Bitmap(_imagePath);
                Bitmap filteredImage = await ApplyConvolution(originalImage, filterMatrix);
                imgDisplay.Source = BitmapToBitmapSource(filteredImage); // Aggiorna l'immagine visualizzata
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Filter application failed: {ex.Message}", "Processing Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Converte un oggetto Bitmap di System.Drawing in un BitmapSource compatibile con WPF
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>        
        private ImageSource BitmapToBitmapSource(Bitmap source)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                source.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}