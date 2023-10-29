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
    public partial class BoardView : Page
    {
        public const double BoardSizePixels = 620.0;
        private string player1Name, player2Name, player1ColorHex, player2ColorHex;
        private SolidColorBrush player1Brush, player2Brush;
        public BoardView()
        {
            InitializeComponent();
            player1Name = "Player 1";
            player2Name = "Player 2";
            player1ColorHex = "#ef3a0c";
            player2ColorHex = "#3c9f9c";
            player1Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(player1ColorHex));
            player2Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(player2ColorHex));
        }

        public void StartNewGame(object sender, NewGameEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Player1Name))
            {
                player1Name = e.Player1Name;
            }
            if (!string.IsNullOrEmpty(e.Player2Name))
            {
                player2Name = e.Player2Name;
            }
            DrawGrid(e.BoardSize);
        }

        private void DrawGrid(int n)
        {
            double cellSize = BoardSizePixels / (double)n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var cell = new Button();
                    var row = new RowDefinition();
                    var col = new ColumnDefinition();
                    cell.Width = cellSize;
                    cell.Height = cellSize;
                    cell.Style = (Style)this.Resources["GridCellStyle"];
                    row.Height = GridLength.Auto;
                    col.Width = GridLength.Auto;
                    GameGrid.RowDefinitions.Add(row);
                    GameGrid.ColumnDefinitions.Add(col);
                    cell.Click += GameCell_Click;
                    cell.MouseEnter += GameCell_MouseEnter;
                    GameGrid.Children.Add(cell);
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                }
            }

            UpdatePlayerTurnText();
        }

        private void GameCell_Click(object sender, RoutedEventArgs e) 
        { 

        }

        private void GameCell_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void UpdatePlayerTurnText()
        {
            PlayerTurnText.Inlines.Clear();
            if (GameState.player1Turn)
            {
                Run run1 = new Run(player1Name);
                run1.Foreground = player1Brush;
                PlayerTurnText.Inlines.Add(run1);
            }
            else
            {
                Run run1 = new Run(player2Name);
                run1.Foreground = player2Brush;
                PlayerTurnText.Inlines.Add(run1);
            }
            Run run2 = new Run("'s turn to play");
            run2.Foreground = Brushes.White;
            PlayerTurnText.Inlines.Add(run2);
        }
    }
}
