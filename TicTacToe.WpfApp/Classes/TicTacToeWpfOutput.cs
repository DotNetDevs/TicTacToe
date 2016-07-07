using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TicTacToe.Enums;
using TicTacToe.Interfaces;

namespace TicTacToe.WpfApp.Classes
{
    public class TicTacToeWpfOutput : ITicTacToeOutput
    {
        private readonly IEnumerable<Button> _buttons;

        public TicTacToeWpfOutput(IEnumerable<Button> buttons)
        {
            _buttons = buttons;
        }

        public void PrintSchema(IEnumerable<TicTacToeCellType> cells)
        {
            for (var i = 0; i < cells.Count(); i++)
            {
                var cell = cells.ElementAt(i);
                var button = _buttons.ElementAt(i);

                button.Content = GetSymbol(cell);
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
                return " ";
            }
        }
    }
}
