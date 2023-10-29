using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mosaic
{
    public class Board
    {
        private List<List<CellState>> boardPosition = new List<List<CellState>>();
        private int boardSize;
        private (int,int) playerScores;
        public Board() { }
        public void CreateNewBoard(object sender, NewGameEventArgs e)
        {
            ResetBoard();
            boardSize = e.BoardSize;
            for (int i = 0; i < boardSize; i++)
            {
                boardPosition.Add(new List<CellState>());
                for(int j = 0; j < boardSize; j++)
                {
                    boardPosition[i].Add(CellState.Unclaimed);
                }
            }
        }

        public void TryMove(object sender, MoveAttemptedArgs e)
        {
            Button cell = e.Cell;
            var row = Grid.GetRow(cell);
            var col = Grid.GetColumn(cell);
            if (boardPosition[row][col] == CellState.Unclaimed)
            {
                if (GameState.player1Turn)
                {
                    boardPosition[row][col] = CellState.X;
                    cell.Content = "X";
                    if(CheckForXOX(cell, out var startIndex, out var endIndex))
                    {
                        (sender as BoardView)?.MarkXOX(cell.ActualHeight, startIndex, endIndex, playerScores);
                    }
                    GameState.player1Turn = false;
                }
                else
                {
                    boardPosition[row][col] = CellState.O;
                    cell.Content = "O";
                    if (CheckForXOX(cell, out var startIndex, out var endIndex))
                    {
                        (sender as BoardView)?.MarkXOX(cell.ActualHeight, startIndex, endIndex, playerScores);
                    }
                    GameState.player1Turn = true;
                }
                (sender as BoardView)?.UpdatePlayerTurnText();
            }
        }

        private bool CheckForXOX(Button cell, out (int, int) startIndex, out (int, int) endIndex)
        {
            var row = Grid.GetRow(cell);
            var col = Grid.GetColumn(cell);
            if (GameState.player1Turn)
            {
                //[S][][]
                //[O][][]
                //[x][][]
                if (row >= 2)
                {
                    if (boardPosition[row - 1][col] == CellState.O && boardPosition[row - 2][col] == CellState.X)
                    {
                        startIndex = (row, col);
                        endIndex = (row - 2, col);
                        return true;
                    }
                }

                //[][][S]
                //[][O][]
                //[x][][]
                if (row >= 2 && col + 2 < boardSize)
                {
                    if (boardPosition[row - 1][col + 1] == CellState.O && boardPosition[row - 2][col + 2] == CellState.X)
                    {
                        startIndex = (row, col);
                        endIndex = (row - 2, col + 2);
                        return true;
                    }
                }

                //[][][]
                //[x][O][S]
                //[][][]
                if (col + 2 < boardSize)
                {
                    if (boardPosition[row][col + 1] == CellState.O && boardPosition[row][col + 2] == CellState.X)
                    {
                        startIndex = (row, col);
                        endIndex = (row, col + 2);
                        return true;
                    }
                }

                //[x][][]
                //[][O][]
                //[][][S]
                if (row + 2 < boardSize && col + 2 < boardSize)
                {
                    if (boardPosition[row + 1][col + 1] == CellState.O && boardPosition[row + 2][col + 2] == CellState.X)
                    {
                        startIndex = (row, col);
                        endIndex = (row + 2, col + 2);
                        return true;
                    }
                }

                //[x][][]
                //[O][][]
                //[S][][]
                if (row + 2 < boardSize)
                {
                    if (boardPosition[row + 1][col] == CellState.O && boardPosition[row + 2][col] == CellState.X)
                    {
                        startIndex = (row, col);
                        endIndex = (row + 2, col);
                        return true;
                    }
                }

                //[][][x]
                //[][O][]
                //[S][][]
                if (row + 2 < boardSize && col >= 2)
                {
                    if (boardPosition[row + 1][col - 1] == CellState.O && boardPosition[row + 2][col - 2] == CellState.X)
                    {
                        startIndex = (row, col);
                        endIndex = (row + 2, col - 2);
                        return true;
                    }
                }

                //[][][]
                //[S][O][x]
                //[][][]
                if (col >= 2)
                {
                    if (boardPosition[row][col - 1] == CellState.O && boardPosition[row][col - 2] == CellState.X)
                    {
                        startIndex = (row, col);
                        endIndex = (row, col - 2);
                        return true;
                    }
                }

                //[S][][]
                //[][O][]
                //[][][x]
                if (row >= 2 && col >= 2)
                {
                    if (boardPosition[row - 1][col - 1] == CellState.O && boardPosition[row - 2][col - 2] == CellState.X)
                    {
                        startIndex = (row, col);
                        endIndex = (row - 2, col - 2);
                        return true;
                    }
                }
            }
            else
            {

                //[][S][]
                //[][x][]
                //[][S][]
                if (row + 1 < boardSize && row >= 1)
                {
                    if (boardPosition[row + 1][col] == CellState.X && boardPosition[row - 1][col] == CellState.X)
                    {
                        startIndex = (row - 1, col);
                        endIndex = (row + 1, col);
                        return true;
                    }
                }

                //[][][]
                //[S][x][S]
                //[][][]
                if (col + 1 < boardSize && col >= 1)
                {
                    if (boardPosition[row][col - 1] == CellState.X && boardPosition[row][col + 1] == CellState.X)
                    {
                        startIndex = (row, col - 1);
                        endIndex = (row, col + 1);
                        return true;
                    }
                }
                if (row + 1 < boardSize && row >= 1 && col + 1 < boardSize && col >= 1)
                {

                    //[][][S]
                    //[][x][]
                    //[S][][]
                    if (boardPosition[row - 1][col + 1] == CellState.X && boardPosition[row + 1][col - 1] == CellState.X)
                    {
                        startIndex = (row - 1, col + 1);
                        endIndex = (row + 1, col - 1);
                        return true;
                    }

                    //[S][][]
                    //[][x][]
                    //[][][S]
                    if (boardPosition[row - 1][col - 1] == CellState.X && boardPosition[row + 1][col + 1] == CellState.X)
                    {
                        startIndex = (row - 1, col - 1);
                        endIndex = (row + 1, col + 1);
                        return true;
                    }
                }
            }
            startIndex = endIndex = (0, 0);
            return false;
        }

        private void ResetBoard()
        {
            playerScores = (0, 0);
            boardPosition.Clear();
        }
    }

    public enum CellState
    {
        Unclaimed,
        X,
        O
    }
}
