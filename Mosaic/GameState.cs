using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
    public static class GameState
    {
        public static bool gameActive {  get; set; } = false;
        public static bool player1Turn {  get; set; } = true;
        public static bool player2IsAI {  get; set; } = true;

        public static bool testing { get; set; } = false;

    }
}
