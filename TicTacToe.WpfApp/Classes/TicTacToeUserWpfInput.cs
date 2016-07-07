using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Enums;
using TicTacToe.Interfaces;

namespace TicTacToe.WpfApp.Classes
{
    public class TicTacToeUserWpfInput : ITicTacToeInput
    {
        public TicTacToeCellType CellType { get; set; }
        public bool NeedsOutput => true;

        public event InputSentHandler InputSent;

        public void SendInput(int row, int col)
        {
            InputSent?.Invoke(this, Tuple.Create(row, col));
        }
    }
}
