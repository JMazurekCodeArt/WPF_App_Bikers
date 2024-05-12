using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Interface
{
    public partial class Window1 : Window
    {

        public Window1()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadButtonsFromInfoFile();
        }


        private void Bikers1_OnClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow drugieOkno = new MainWindow();
            drugieOkno.Show();

            Window.GetWindow(this).Close();
        }

        private void LoadButtonsFromInfoFile()
        {
            string infoFilePath = @"..\..\..\trasy\info.txt";
            string[] lines = File.ReadAllLines(infoFilePath);
            foreach (string line in lines)
            {
                string[] words = line.Split(' ');

                // Pobierz nazwę przycisku z drugiego słowa linii pliku
                string buttonName = words[1];

                // Pobierz ścieżkę do obrazka z pierwszego słowa linii pliku
                string buttonImagePath = $@"..\..\trasy\{words[0]}";

                // Pobierz dystans z trzeciego słowa linii pliku
                string buttonDistance = words[2] + " km";

                // Pobierz czas z czwartego słowa linii pliku
                string buttonTime = words[3] + " h";

                // Pobierz poziom trudności z piątego słowa linii pliku
                string buttonDifficulty = words[4];

                // Stwórz nowy przycisk
                Button button = new Button();
                button.Content = buttonName;

                // Dodaj obsługę zdarzenia Click
                button.Click += (s, e) =>
                {
                    // Wyświetl odpowiednie obrazy, dystans, czas i poziom trudności
                    image1.Source = new BitmapImage(new Uri(buttonImagePath, UriKind.Relative));
                    dis.Content = buttonDistance;
                    time.Content = buttonTime;
                    dif.Content = buttonDifficulty;
                };

                // Dodaj przycisk do stack panelu
                stackPanel.Children.Add(button);
            }
        }






    }
}
