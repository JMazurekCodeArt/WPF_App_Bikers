using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Interface
{
    
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
        }
        private void Bikers3_OnClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow drugieOkno = new MainWindow();
            drugieOkno.Show();

            Window.GetWindow(this).Close();
        }

        private bool isDrawing = false;
        private Line currentLine;

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDrawing = true;

                currentLine = new Line
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 4,
                    X1 = e.GetPosition(canvas).X,
                    Y1 = e.GetPosition(canvas).Y,
                    X2 = e.GetPosition(canvas).X,
                    Y2 = e.GetPosition(canvas).Y
                };

                canvas.Children.Add(currentLine);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                currentLine.X2 =  e.GetPosition(canvas).X;
                currentLine.Y2 =  e.GetPosition(canvas).Y;
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
        }

        private void ClearCanvasOnClick(object sender, MouseButtonEventArgs e)
        {
            canvas.Children.Clear();
        }

        private void SaveToFile_OnClick(object sender, MouseButtonEventArgs e)
        {
            // Uzyskanie informacji o katalogu i nazwy pliku
            DirectoryInfo dirInfo = new DirectoryInfo(@"..\..\..\trasy");
            FileInfo[] files = dirInfo.GetFiles();
            int fileNumber = files.Length;
            string fileName = "canvas_" + fileNumber.ToString() + ".png";
            string filePath = System.IO.Path.Combine(dirInfo.FullName, fileName);

            // Ustawienie marginesów Canvas na 0
            canvas.Margin = new Thickness(0, 0, 0, 0);

            // Uzyskanie PresentationSource dla okna
            PresentationSource source = PresentationSource.FromVisual(this);

            // Obliczenie wartości DPI dla PresentationSource
            double dpiX = 96.0, dpiY = 96.0;
            if (source != null)
            {
                dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            }

            // Utworzenie renderera bitmapy, który będzie renderować płótno Canvas
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, dpiX, dpiY, PixelFormats.Pbgra32);
            renderBitmap.Render(canvas);

            // Kodowanie bitmapy do pliku PNG
            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            Validate(name.Text, dis.Text, time.Text, dif.Text);
            // Zapisywanie pliku
            using (Stream fileStream = File.Create(filePath))
            {
                pngEncoder.Save(fileStream);
            }

            string info = System.IO.Path.Combine(dirInfo.FullName, "info.txt");
            using (StreamWriter writer = new StreamWriter(info, true))
            {
                string line = $"{fileName} {name.Text} {dis.Text} {time.Text} {dif.Text}";
                writer.WriteLine(line);
            }

        }

        private bool Validate(string name, string dis, string time, string dif)
        {
            // Sprawdź, czy nazwa zawiera spacje
            if (name.Contains(" "))
            {
                MessageBox.Show("Nazwa nie może zawierać spacji.");
                return false;
            }

            // Sprawdź, czy nazwa nie jest pusta
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Nazwa nie może być pusta.");
                return false;
            }


            // Sprawdź, czy wartość pola dis jest pusta
            if (string.IsNullOrEmpty(dis))
            {
                MessageBox.Show("Pole 'dis' nie może być puste.");
                return false;
            }

            // Sprawdź, czy wartość pola dis ma format 0.0
            if (!Regex.IsMatch(dis, @"^[0-9\.]+$"))
            {
                MessageBox.Show("Pole 'dis' musi być liczbą w formacie 0.0.");
                return false;
            }


            // Sprawdź, czy wartość pola time jest pusta
            if (string.IsNullOrEmpty(time))
            {
                MessageBox.Show("Pole 'time' nie może być puste.");
                return false;
            }

            // Sprawdź, czy wartość pola time ma format 00.00
            if (!DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeValue) || timeValue.TimeOfDay == TimeSpan.Zero)
            {
                MessageBox.Show("Pole 'time' musi być w formacie hh:mm.");
                return false;
            }

            // Sprawdź, czy wartość pola dif jest pusta
            if (string.IsNullOrEmpty(dif))
            {
                MessageBox.Show("Pole 'dif' nie może być puste.");
                return false;
            }

            return true;
        }


    }

}
