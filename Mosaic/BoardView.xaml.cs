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

        public delegate void MoveAttemptHandler(object sender, MoveAttemptedArgs e);
        public event MoveAttemptHandler MoveAttempted;
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
            EraseGrid();
            DrawGrid(e.BoardSize);
        }

        public void MarkXOX(double cellSize, (int,int) startIndex, (int,int) endIndex, (int,int) playerScores)
        {
            DrawLine(cellSize, startIndex, endIndex);
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

        private void EraseGrid()
        {
            GameGrid.Children.Clear();
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();
            LineCanvas.Children.Clear();
        }

        private void DrawLine(double cellSize, (int,int) startIndex, (int,int) endIndex)
        {
            int startRow = startIndex.Item1;
            int startCol = startIndex.Item2;
            int endRow = endIndex.Item1;
            int endCol = endIndex.Item2;
            SolidColorBrush lineColor = new SolidColorBrush();
            if (GameState.player1Turn) {lineColor = player1Brush;}
            else {lineColor = player2Brush;}

            Line line = new Line
            {
                Stroke = lineColor,
                StrokeThickness = 2
            };

            Point startPoint = new Point(startCol * cellSize + cellSize / 2, startRow * cellSize + cellSize / 2);
            Point endPoint = new Point(endCol * cellSize + cellSize / 2, endRow * cellSize + cellSize / 2);

            line.X1 = startPoint.X;
            line.Y1 = startPoint.Y;
            line.X2 = endPoint.X;
            line.Y2 = endPoint.Y;

            LineCanvas.Children.Add(line);
        }

        private void GameCell_Click(object sender, RoutedEventArgs e) 
        { 
            Button cell = (Button)sender;
            MoveAttempted(this, new MoveAttemptedArgs(cell));
        }

        private void GameCell_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        public void UpdatePlayerTurnText()
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

        private void UpdatePlayerScores()
        {

        }
    }

    public class MoveAttemptedArgs : EventArgs
    {
        public Button Cell { get; }
        public MoveAttemptedArgs(Button cell)
        {
            Cell = cell;
        }
    }
}
