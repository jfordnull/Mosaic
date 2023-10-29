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

namespace Mosaic
{
    public partial class CreateGameView : Page
    {
        public delegate void NewGameEventHandler(object sender, NewGameEventArgs e);

        public event NewGameEventHandler NewGameCreated;

        public CreateGameView()
        {
            InitializeComponent();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            if((int.TryParse(BoardSizeText.Text, out int n) && n >= 3 && n <= 32) &&
                (SimpleButton.IsChecked == true || GeneralButton.IsChecked == true))
            {
                HideErrorText();
                GameState.gameActive = true;
                bool AIBool = IsAIButton.IsChecked ?? false;
                bool generalGame = GeneralButton.IsChecked ?? false;
                NewGameCreated(this, new NewGameEventArgs(n, Player1NameText.Text, Player2NameText.Text, AIBool, generalGame));
            }
            else
            {
                DisplayErrorText();
            }
        }

        private void BoardInput_HandleInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

        private void BoardInput_HandleSpaces(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) { e.Handled = true; }
        }

        private void DisplayErrorText()
        {
            BoardSizeError.Visibility = Visibility.Visible;
            GameModeError.Visibility = Visibility.Visible;
        }

        private void HideErrorText()
        {
            BoardSizeError.Visibility = Visibility.Hidden;
            GameModeError.Visibility = Visibility.Hidden;
        }
    }

    public class NewGameEventArgs : EventArgs
    {
        public int BoardSize { get; }
        public string Player1Name { get; }
        public string Player2Name { get; }
        public bool Player2IsAI { get; }
        public bool IsGeneralGame { get; }

        public NewGameEventArgs(int n, string n1, string n2, bool AI, bool generalGame)
        {
            BoardSize = n;
            Player1Name = n1;
            Player2Name = n2;
            Player2IsAI = AI;
            IsGeneralGame = generalGame;
        }
    }
}
