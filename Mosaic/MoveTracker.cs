using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
    public class MoveTracker
    {
        string directory, fileName, output;
        StringBuilder csvData;
        public MoveTracker()
        {
            csvData = new StringBuilder();
            directory = "C:\\Users\\tsasf\\OneDrive\\Documents\\MosaicRecordedMoves\\";
            fileName = "Default.csv";
            output = $"{directory}{fileName}";
        }

        public void NewGameFileCreated(object sender, NewGameEventArgs e)
        {
            fileName = e.MatchFileName;
            output = $"{directory}{fileName}.csv";
            Debug.WriteLine("File directory: " + output);
            csvData.Clear();
        }

        public void RecordMoveMade(object sender, MoveRecordEventArgs e)
        {
            int x = e.moveIndex.Item1;
            int y = e.moveIndex.Item2;
            int turn = GameState.player1Turn ? 1 : 0;
            csvData.AppendLine($"{x}, {y}, {turn}");
            File.WriteAllText(output, csvData.ToString() );
        }
    }
}
