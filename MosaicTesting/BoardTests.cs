using Mosaic;
using System.Windows.Controls;

namespace MosaicTesting
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void TestCreateNewBoard_GeneralGame()
        {
            Board board = new Board();

            NewGameEventArgs args = new NewGameEventArgs(9, "Jacob", "Watson", false, true);

            board.CreateNewBoard(this, args);

            Assert.AreEqual(9, board.boardSize, "Board size should be 9, but was something else");
            Assert.IsTrue(board.generalGame, "Game should be general, but was initialized as simple");
            Assert.AreEqual(9, board.boardPosition.Count, "Board should have 9 rows, but doesn't");
            Assert.AreEqual(9, board.boardPosition[0].Count, "First row should have 9 cells, but didn't");
            Assert.AreEqual(CellState.Unclaimed, board.boardPosition[0][0], "Board cells should be initialized to Unclaimed");
            Assert.AreEqual(true, GameState.player1Turn, "Game did not begin with first player's turn");
        }

        [TestMethod]
        public void TestCreateNewBoard_SimpleGame()
        {
            Board board = new Board();

            NewGameEventArgs args = new NewGameEventArgs(16, "Jacob", "Watson", false, false);

            board.CreateNewBoard(this, args);

            Assert.AreEqual(16, board.boardSize, "Board size should be 16, but was something else");
            Assert.IsFalse(board.generalGame, "Game should be simple, but was initialized as general");
            Assert.AreEqual(16, board.boardPosition.Count, "Board should have 16 rows, but doesn't");
            Assert.AreEqual(16, board.boardPosition[0].Count, "First row should have 9 cells, but didn't");
            Assert.AreEqual(CellState.Unclaimed, board.boardPosition[0][0], "Board cells should be initialized to Unclaimed");
            Assert.AreEqual(true, GameState.player1Turn, "Game did not begin with first player's turn");
        }

        [TestMethod]
        public void TestXOX_MakeOMove_Simple()
        {
            (int, int) testCell = (2, 2);

            Board board = new Board();
            NewGameEventArgs args = new NewGameEventArgs(5, "Jacob", "Watson", false, true);
            board.CreateNewBoard(this, args);

            board.boardPosition[1][2] = CellState.X;
            board.boardPosition[3][2] = CellState.X;
            GameState.player1Turn = false;

            Assert.IsTrue(board.CheckForXOX(testCell, out (int, int) startIndex, out (int, int) endIndex), 
                "Should be XOX, but match was not found");
            Assert.AreEqual(startIndex, (1, 2),
                "Starting index of the line to be drawn should be above the cell where move was made");
            Assert.AreEqual(endIndex, (3, 2),
                "Ending index of the line  to be draw should be below the cell where move was made");
        }

        [TestMethod]
        public void TestXOX_MakeXMove_Simple()
        {
            (int, int) testCell = (0, 0);

            Board board = new Board();
            NewGameEventArgs args = new NewGameEventArgs(5, "Jacob", "Watson", false, true);
            board.CreateNewBoard(this, args);

            board.boardPosition[1][1] = CellState.O;
            board.boardPosition[2][2] = CellState.X;

            Assert.IsTrue(board.CheckForXOX(testCell, out (int, int) startIndex, out (int, int) endIndex),
                "Should be XOX, but match was not found");
            Assert.AreEqual(startIndex, (0, 0),
                "Starting index of line to be drawn should be cell where move was made");
            Assert.AreEqual(endIndex, (2, 2),
                "Ending index of line to be drawn should be two cells away from cell where move was made");
        }

        [TestMethod]
        public void TestXOX_MakeOMove_General()
        {
            (int, int) testCell = (2, 2);

            Board board = new Board();
            NewGameEventArgs args = new NewGameEventArgs(5, "Jacob", "Watson", false, true);
            board.CreateNewBoard(this, args);

            board.boardPosition[1][2] = CellState.X;
            board.boardPosition[3][2] = CellState.X;
            GameState.player1Turn = false;

            Assert.IsTrue(board.CheckForXOX(testCell, out (int, int) startIndex, out (int, int) endIndex),
                "Should be XOX, but match was not found");
            Assert.AreEqual(startIndex, (1, 2),
                "Starting index of the line to be drawn should be above the cell where move was made");
            Assert.AreEqual(endIndex, (3, 2),
                "Ending index of the line  to be draw should be below the cell where move was made");
        }

        [TestMethod]
        public void TestXOX_MakeXMove_General()
        {
            (int, int) testCell = (0, 0);

            Board board = new Board();
            NewGameEventArgs args = new NewGameEventArgs(5, "Jacob", "Watson", false, true);
            board.CreateNewBoard(this, args);

            board.boardPosition[1][1] = CellState.O;
            board.boardPosition[2][2] = CellState.X;

            Assert.IsTrue(board.CheckForXOX(testCell, out (int, int) startIndex, out (int, int) endIndex),
                "Should be XOX, but match was not found");
            Assert.AreEqual(startIndex, (0, 0),
                "Starting index of line to be drawn should be cell where move was made");
            Assert.AreEqual(endIndex, (2, 2),
                "Ending index of line to be drawn should be two cells away from cell where move was made");
        }

        [TestMethod]
        public void TestVictory_Simple() { }

        [TestMethod]
        public void TestVictory_General() { }
    }
}