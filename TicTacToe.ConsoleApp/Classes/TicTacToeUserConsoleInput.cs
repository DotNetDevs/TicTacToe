using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Enums;
using TicTacToe.Interfaces;

namespace TicTacToe.ConsoleApp.Classes
{
    public class TicTacToeUserConsoleInput : ITicTacToeInput
    {
        public TicTacToeCellType CellType { get; set; }
        public bool NeedsOutput => true;

        public event InputSentHandler InputSent;

        public void GetInput()
        {
            var row = 1;
            var col = 1;

            do
            {
                Console.WriteLine("Inserisci riga");
                if (!int.TryParse(Console.ReadLine(), out row)) continue;
                Console.WriteLine("Inserisci colonna");
                if (!int.TryParse(Console.ReadLine(), out col)) continue;
            }
            while (row <= 0 || col <= 0);

            InputSent?.Invoke(this, Tuple.Create(row - 1, col - 1));
        }
    }
}
