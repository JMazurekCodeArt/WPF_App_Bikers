using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Interface
{
    public partial class Window2 : Window
    {
        private List<Button> ButtonsList;
        private Button Button1;
        private Button Button2; // Dodaj deklarację przycisku Button2

        public Window2()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            ButtonsList = new List<Button>();

            Button1 = new Button();
            Button1.Content = "Button1";
            Button1.Click += DeleteButton_Click;
            stackPanel.Children.Add(Button1);
            ButtonsList.Add(Button1);

            Button2 = new Button(); // Inicjalizuj przycisk Button2
            Button2.Content = "Button2"; // Ustaw zawartość przycisku
            Button2.Click += DeleteButton_Click; // Dodaj obsługę zdarzenia Click
            stackPanel.Children.Add(Button2); // Dodaj przycisk do stack panelu
            ButtonsList.Add(Button2); // Dodaj przycisk do listy

            LoadButtonsFromInfoFile();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadButtonsFromInfoFile();
        }

        private void Bikers2_OnClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow drugieOkno = new MainWindow();
            drugieOkno.Show();

            Window.GetWindow(this).Close();
        }

        private void LoadButtonsFromInfoFile()
        {
            string infoFilePath = @"..\..\..\trasy\info.txt";
            if (File.Exists(infoFilePath))
            {
                stackPanel.Children.Clear();

                string[] lines = File.ReadAllLines(infoFilePath);
                int rowIndex = 1;

                foreach (string line in lines)
                {
                    string buttonName = line;

                    Button button = new Button();
                    button.Content = buttonName;
                    button.Tag = rowIndex;
                    button.Click += DeleteButton_Click;

                    stackPanel.Children.Add(button);

                    ButtonsList.Add(button);

                    rowIndex++;
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            Button button = (Button)sender;

            int rowNumber = (int)button.Tag;

            if (rowNumber > 3)
            {
                string infoFilePath = @"..\..\..\trasy\info.txt";
                if (File.Exists(infoFilePath))
                {
                    string[] lines = File.ReadAllLines(infoFilePath);
                    if (rowNumber <= lines.Length)
                    {
                        lines = lines.Where((line, index) => index != rowNumber - 1).ToArray();
                        File.WriteAllLines(infoFilePath, lines);

                        ButtonsList.Remove(button);
                        stackPanel.Children.Remove(button);

                        UpdateButtonTags();
                    }
                }


            }
        }
        private void UpdateButtonTags()
        {
            int rowIndex = 1;
            foreach (Button button in ButtonsList)
            {
                button.Tag = rowIndex;
                rowIndex++;
            }
        }
    }
}
