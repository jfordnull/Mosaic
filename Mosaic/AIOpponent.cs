using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{   public class AIOpponent
    {
        private Random random;
        public AIOpponent() { random = new Random(); }
        public void AIMoveAttempt(object sender, AIMoveEventArgs e)
        {
            var XOXMatches = new List<((int, int), (int, int))>();
            for (int i = 0; i < e.BoardPosition.Count; i++)
            {
                for (int j = 0; j < e.BoardPosition[i].Count; j++)
                {
                    if (e.BoardPosition[i][j]== CellState.Unclaimed)
                    {
                        XOXMatches = (sender as Board)?.CheckForXOX((i, j));
                        Debug.WriteLine("Found unclaimed cell");
                        if (XOXMatches.Count > 0) 
                        {
                            Debug.WriteLine("Found XOX");
                            (sender as Board)?.HandleAIMove(XOXMatches, (i,j));
                            break;
                        }
                    }
                }
                if (XOXMatches.Count > 0) { break; }
            }
            if (XOXMatches.Count <= 0 )
            {
                int i, j;
                bool flag = false;
                do
                {
                    i = random.Next(0, e.BoardPosition.Count);
                    j = random.Next(0, e.BoardPosition.Count);
                    if (e.BoardPosition[i][j] == CellState.Unclaimed)
                    {
                        (sender as Board)?.HandleAIMove(new List<((int, int), (int, int))>(), (i, j));
                        flag = true;
                    }
                } while (flag == false);
            }
        }
    }
}
