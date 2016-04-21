using System;
using System.Collections.Generic;
using System.Linq;
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

using System.IO;

namespace Cities
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> allCities = new List<string>();
        List<string> usedCities = new List<string>();

        char lastLetter;

        string badLetters = "ёьъ";

        public MainWindow()
        {
            InitializeComponent();
            allCities.AddRange(File.ReadAllText("Cities.txt").Split(' '));
        }

        private void SayCity_Button_Click(object sender, RoutedEventArgs e)
        {
            var userVariantNotLower = UserCity_TextBox.Text;
            var userVariant = UserCity_TextBox.Text.ToLower();

            var userFirstLetter = userVariant[0];

            if(userVariant.StartsWith("сдаюсь"))
            {
                Log_TextBox.Text += String.Format("Вы проиграли.") + Environment.NewLine;
                InitializeGame();
                Log_TextBox.Text += String.Format("Игра начата заново.") + Environment.NewLine + Environment.NewLine;
                ScrollLog();

                return;
            }

            if(usedCities.Exists(x => x.ToLower() == userVariant))
            {
                Log_TextBox.Text += String.Format("Вы: {0} - такой город уже назывался. Предложите другой вариант.", userVariantNotLower) + Environment.NewLine;
                ScrollLog();
                return;
            }

            var userCityIndex = allCities.FindIndex(x => x.ToLower() == userVariant);

            if (userCityIndex == -1)
            {
                Log_TextBox.Text += String.Format("Вы: {0} - такого города нет в базе. Предложите другой вариант.", userVariantNotLower) + Environment.NewLine;
                ScrollLog();
                return;
            }
            else if(userFirstLetter != lastLetter && lastLetter != '\0')
            {
                Log_TextBox.Text += String.Format("Вы: {0}. Необходимо назвать город на букву {1}, предложите другой вариант.", userVariantNotLower, lastLetter) + Environment.NewLine;
                ScrollLog();
                return;
            }
            else
            {
                Log_TextBox.Text += String.Format("Вы: {0}.", userVariantNotLower) + Environment.NewLine;
                //lastLetter = userVariant[userVariant.Length - 1];

                SetLastLetter(userVariant);

                usedCities.Add(allCities[userCityIndex]);

                allCities.RemoveAt(userCityIndex);

                ScrollLog();

                SayCity();
                return;
            }

            //if(allCities.Exists(x => x.ToLower()[0] == userFirstLetter))
            //{

            //}

            //lastLetter = userVariant[userVariant.Length - 1];
        }

        private void SayCity()
        {
            var indexOfFoudCity = allCities.FindIndex(x => x.ToLower()[0] == lastLetter);

            if (indexOfFoudCity == -1)
            {
                Log_TextBox.Text += String.Format("Компьютер: Сдаюсь. В моей базе больше нет городов на букву {0}.", lastLetter) + Environment.NewLine;
                Log_TextBox.Text += String.Format("Вы победили!!") + Environment.NewLine;
                InitializeGame();
                Log_TextBox.Text += String.Format("Игра начата заново.") + Environment.NewLine + Environment.NewLine;

                ScrollLog();
            }
            else
            {
                Log_TextBox.Text += String.Format("Компьютер: {0}.", allCities[indexOfFoudCity]) + Environment.NewLine + Environment.NewLine;
                ScrollLog();

                SetLastLetter(allCities[indexOfFoudCity]);

                usedCities.Add(allCities[indexOfFoudCity]);

                allCities.RemoveAt(indexOfFoudCity);
            }
        }

        void InitializeGame()
        {
            lastLetter = '\0';

            allCities.AddRange(usedCities);
            usedCities.Clear();
            //allCities.Clear();
            //File.ReadAllText("Cities.txt").Split(' '));
        }

        void ScrollLog()
        {
            UserCity_TextBox.Text = "";

            Log_TextBox.CaretIndex = Log_TextBox.Text.Length;
            Log_TextBox.ScrollToEnd();
        }

        void SetLastLetter(string city)
        {
            for(int i = city.Length - 1; i > 0; i--)
            {
                if(!badLetters.Contains(city[i]))
                {
                    lastLetter = city[i];
                    return;
                }
            }
        }
    }
}
