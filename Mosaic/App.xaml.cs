using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mosaic
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            BoardView boardView = new BoardView();
            SettingsView settingsView = new SettingsView();
            CreateGameView createGameView = new CreateGameView();
            MainWindow window = new MainWindow(boardView, settingsView, createGameView);
            window.Show();

            createGameView.NewGameCreated += boardView.StartNewGame;
        }
    }
}
