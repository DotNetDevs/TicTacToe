using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Enums;
using TicTacToe.Interfaces;

namespace TicTacToe.ConsoleApp.Classes
{
    public class TicTacToeConsoleOutput : ITicTacToeOutput
    {
        public void PrintSchema(IEnumerable<TicTacToeCellType> cells)
        {
            for (var i = 0; i < cells.Count(); i++)
            {
                var row = i / 3;
                var col = i - row * 3;
                var cell = cells.ElementAt(i);

                if (col > 0) Console.Write("|");

                Console.Write(GetSymbol(cell));

                if (col == 2 && row < 2) Console.WriteLine($"{Environment.NewLine}-------");
            }

            Console.WriteLine();
        }

        public string GetSymbol(TicTacToeCellType cell)
        {
            switch (cell)
            {
            case TicTacToeCellType.Cross:
                return "X";
            case TicTacToeCellType.Circle:
                return "O";
            case TicTacToeCellType.Empty:
            default:
                return "  ";
            }
        }
    }
}
