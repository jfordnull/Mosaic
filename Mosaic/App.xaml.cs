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
            Board board = new Board(boardView);
            AIOpponent ai = new AIOpponent();
            MainWindow window = new MainWindow(boardView, settingsView, createGameView);
            MoveTracker moveTracker = new MoveTracker();
            window.Show();

            createGameView.NewGameCreated += boardView.StartNewGame;
            createGameView.NewGameCreated += moveTracker.NewGameFileCreated;
            createGameView.NewGameCreated += board.CreateNewBoard;
            boardView.MoveAttempted += board.TryMove;
            board.AIMoveMade += ai.AIMoveAttempt;
            board.RecordMoveMade += moveTracker.RecordMoveMade;
        }
    }
}
