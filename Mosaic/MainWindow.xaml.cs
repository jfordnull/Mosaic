using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class MainWindow : Window
    {
        private WelcomeView welcomeView;
        private BoardView boardView;
        private CreateGameView createGameView;
        private SettingsView settingsView;
        private Button lastHoveredMenuButton;
        public MainWindow(BoardView board, SettingsView settings, CreateGameView newGame)
        {
            InitializeComponent();
            welcomeView = new WelcomeView();
            boardView = board;
            createGameView = newGame;
            settingsView = settings;
            ContentView.Navigate(welcomeView);
        }

        private void PlayButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Button playButton = (Button)sender;
            PlayButton_Highlight(playButton);
            //if (GameState.gameActive)
            //{
            //    ContentView.Navigate(boardView);
            //}
            //else
            //{
            //    ContentView.Navigate(welcomeView);
            //}
            ContentView.Navigate(boardView);
        }

        private void PlayButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Button playButton = (Button)sender;
            if (playButton != lastHoveredMenuButton)
            {
                playButton.Background = Brushes.White;
                playButton.Foreground= Brushes.Black;
            }
        }

        private void NewGame_MouseEnter(object sender, MouseEventArgs e)
        {
            ContentView.Navigate(createGameView);
        }

        private void Settings_MouseEnter(object sender, MouseEventArgs e)
        {
            ContentView.Navigate(settingsView);
        }

        private void PlayButton_Highlight(Button playButton)
        {
            playButton.Background = Brushes.Black;
            playButton.Foreground = Brushes.White;
            if (lastHoveredMenuButton != null && lastHoveredMenuButton != playButton)
            {
                lastHoveredMenuButton.Background = Brushes.White;
                lastHoveredMenuButton.Foreground = Brushes.Black;
            }
            lastHoveredMenuButton = playButton;
        }

        private void MenuButton_Highlight(object sender, MouseEventArgs e)
        {
            Button currentButton = (Button)sender;

            currentButton.Background = Brushes.LightGray;
            currentButton.Foreground = Brushes.White;

            if (lastHoveredMenuButton != null && lastHoveredMenuButton != currentButton)
            {
                lastHoveredMenuButton.Background = Brushes.White;
                lastHoveredMenuButton.Foreground = Brushes.Black;
            }

            lastHoveredMenuButton = currentButton;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
