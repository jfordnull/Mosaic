using Mosaic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mosaic
{
    public class Board
    {
        public List<List<CellState>> boardPosition { get; private set; }

        public delegate void AIMoveEventHandler(object sender, AIMoveEventArgs e);
        public event AIMoveEventHandler AIMoveMade;
        public int boardSize { get; private set; }
        public int movesMade { get; private set; }
        public bool generalGame { get; private set; }
        private (int, int) playerScores;
        private bool player2IsAI, player1IsAI;
        private List<((int, int), (int, int))> aiMatches;
        private (int, int) indexToMarkAIMove;
        private BoardView boardView;

        public Board() { }
        public Board(BoardView boardView) { this.boardView = boardView; }
        public void CreateNewBoard(object sender, NewGameEventArgs e)
        {
            ResetBoard();
            boardSize = e.BoardSize;
            aiMatches = new List<((int, int), (int, int))> ();
            indexToMarkAIMove = (0, 0);
            generalGame = e.IsGeneralGame;
            player1IsAI = e.Player1IsAI;
            player2IsAI = e.Player2IsAI;
            for (int i = 0; i < boardSize; i++)
            {
                boardPosition.Add(new List<CellState>());
                for(int j = 0; j < boardSize; j++)
                {
                    boardPosition[i].Add(CellState.Unclaimed);
                }
            }
            GameState.player1Turn = true;
            if (player1IsAI && player2IsAI)
            {
                HandleAIGame();
            }
            else if (player1IsAI) { GameState.player1Turn = false; }
            if (!GameState.testing)
            {
                boardView.UpdatePlayerTurnText();
            }
        }

        public void TryMove(object sender, MoveAttemptedArgs e)
        {
            Button cell = e.Cell;
            var row = Grid.GetRow(cell);
            var col = Grid.GetColumn(cell);
            var XOXMatches = new List<((int, int), (int, int))>();
            if (boardPosition[row][col] == CellState.Unclaimed)
            {
                if (GameState.player1Turn && !player1IsAI)
                {
                    boardPosition[row][col] = CellState.X;
                    cell.Content = "X";
                    XOXMatches = CheckForXOX((row, col));
                    if (XOXMatches.Count > 0)
                    {
                        foreach (var match in XOXMatches)
                        {
                            playerScores.Item1 += 1;
                            (sender as BoardView)?.MarkXOX(cell.ActualHeight, match.Item1, match.Item2, playerScores);
                        }
                        if (!generalGame) { (sender as BoardView)?.HandleVictory(EndConditions.Player1); }
                    }
                    movesMade++;
                    GameState.player1Turn = false;
                }
                else if (!player2IsAI)
                {
                    boardPosition[row][col] = CellState.O;
                    cell.Content = "O";
                    XOXMatches = CheckForXOX((row, col));
                    if (XOXMatches.Count > 0)
                    {
                        foreach (var match in XOXMatches)
                        {
                            playerScores.Item2 += 1;
                            (sender as BoardView)?.MarkXOX(cell.ActualHeight, match.Item1, match.Item2, playerScores);
                        }
                        if (!generalGame) { (sender as BoardView)?.HandleVictory(EndConditions.Player2); }
                    }
                    GameState.player1Turn = true;
                    movesMade++;
                }
                if ((player1IsAI || player2IsAI) && !BoardFull())
                {
                    //(sender as BoardView)?.UpdatePlayerTurnText();
                    AIMoveMade(this, new AIMoveEventArgs(boardPosition));
                    if (aiMatches.Count > 0)
                    {
                        foreach (var match in aiMatches)
                        {
                            if (GameState.player1Turn) { playerScores.Item1 += 1; }
                            else { playerScores.Item2 += 1; }
                            (sender as BoardView)?.MarkXOX(cell.ActualHeight, match.Item1, match.Item2, playerScores);
                        }
                        if (!generalGame)
                        {
                            if (GameState.player1Turn) { (sender as BoardView)?.HandleVictory(EndConditions.Player1); }
                            else { (sender as BoardView)?.HandleVictory(EndConditions.Player2); }
                        }
                    }
                    if (GameState.player1Turn) { boardPosition[indexToMarkAIMove.Item1][indexToMarkAIMove.Item2] = CellState.X; }
                    else { boardPosition[indexToMarkAIMove.Item1][indexToMarkAIMove.Item2] = CellState.O; }
                    (sender as BoardView)?.UpdateCellContent(indexToMarkAIMove);
                    aiMatches.Clear();
                    movesMade++;
                    GameState.player1Turn = !GameState.player1Turn;
                }
                if (BoardFull())
                {
                    EndConditions endCondition = new EndConditions();
                    if (playerScores.Item1 > playerScores.Item2) {endCondition = EndConditions.Player1;}
                    else if (playerScores.Item2 > playerScores.Item1) {endCondition = EndConditions.Player2;}
                    else {endCondition = EndConditions.Draw;}
                    (sender as BoardView)?.HandleVictory(endCondition);
                }
                (sender as BoardView)?.UpdatePlayerTurnText();
            }
        }

        public void HandleAIMove(List<((int,int),(int,int))> matches, (int, int) moveIndex)
        {
            aiMatches = matches;
            indexToMarkAIMove = moveIndex;
        }

        private void HandleAIGame()
        {
            while (!BoardFull())
            {
                AIMoveMade(this, new AIMoveEventArgs(boardPosition));
                if (aiMatches.Count > 0)
                {
                    foreach (var match in aiMatches){
                        if (GameState.player1Turn) { playerScores.Item1 += 1; }
                        else { playerScores.Item2 += 1; }
                        boardView.MarkXOX((double)620.0/boardSize, match.Item1, match.Item2, playerScores);
                    }
                    if (!generalGame)
                    {
                        boardView.UpdateCellContent(indexToMarkAIMove);
                        if (GameState.player1Turn) { boardView.HandleVictory(EndConditions.Player1); }
                        else { boardView.HandleVictory(EndConditions.Player2); }
                        break;
                    }
                }
                if (GameState.player1Turn) { boardPosition[indexToMarkAIMove.Item1][indexToMarkAIMove.Item2] = CellState.X; }
                else { boardPosition[indexToMarkAIMove.Item1][indexToMarkAIMove.Item2] = CellState.O; }
                boardView.UpdateCellContent(indexToMarkAIMove);
                aiMatches.Clear();
                movesMade++;
                GameState.player1Turn = !GameState.player1Turn;
            }
            if (BoardFull())
            {
                EndConditions endCondition = new EndConditions();
                if (playerScores.Item1 > playerScores.Item2) { endCondition = EndConditions.Player1; }
                else if (playerScores.Item2 > playerScores.Item1) { endCondition = EndConditions.Player2; }
                else { endCondition = EndConditions.Draw; }
                boardView.HandleVictory(endCondition);
            }
        }

        public List<((int,int),(int,int))> CheckForXOX((int,int) cellIndex)
        {
            var row = cellIndex.Item1;
            var col = cellIndex.Item2;
            (int, int) startIndex, endIndex;
            List<((int,int),(int,int))> XOXMatches = new List<((int, int), (int, int))>();
            if (GameState.player1Turn)
            {
                //[S][][]
                //[O][][]
                //[x][][]
                if (row >= 2)
                {
                    if (boardPosition[row - 1][col] == CellState.O && boardPosition[row - 2][col] == CellState.X)
                    {
                        Debug.WriteLine("Found Match");
                        startIndex = (row, col);
                        endIndex = (row - 2, col);
                        XOXMatches.Add((startIndex, endIndex));
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
                        XOXMatches.Add((startIndex,endIndex));
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
                        XOXMatches.Add((startIndex, endIndex));
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
                        XOXMatches.Add((startIndex, endIndex));
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
                        XOXMatches.Add((startIndex, endIndex));
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
                        XOXMatches.Add((startIndex, endIndex));
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
                        XOXMatches.Add((startIndex, endIndex));
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
                        XOXMatches.Add((startIndex, endIndex));
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
                        XOXMatches.Add((startIndex, endIndex));
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
                        XOXMatches.Add((startIndex, endIndex));
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
                        XOXMatches.Add((startIndex, endIndex));
                    }

                    //[S][][]
                    //[][x][]
                    //[][][S]
                    if (boardPosition[row - 1][col - 1] == CellState.X && boardPosition[row + 1][col + 1] == CellState.X)
                    {
                        startIndex = (row - 1, col - 1);
                        endIndex = (row + 1, col + 1);
                        XOXMatches.Add((startIndex, endIndex));
                    }
                }
            }
            return XOXMatches;
        }

        private void ResetBoard()
        {
            boardPosition = new List<List<CellState>>();
            movesMade = 0;
            playerScores = (0, 0);
        }

        private bool BoardFull()
        {
            if(movesMade >= boardSize * boardSize)
            {
                Debug.WriteLine("Board Full");
                return true;
            }
            return false;
        }
    }

    public enum CellState
    {
        Unclaimed,
        X,
        O
    }

    public enum EndConditions
    {
        Player1,
        Player2,
        Draw
    }
}

public class AIMoveEventArgs : EventArgs
{
    public List<List<CellState>> BoardPosition { get; }

    public AIMoveEventArgs(List<List<CellState>> board) 
    { 
        BoardPosition = board;
    }
}
