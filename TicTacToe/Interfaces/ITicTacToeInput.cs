using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Enums;

namespace TicTacToe.Interfaces
{
    public delegate void InputSentHandler(ITicTacToeInput sender, Tuple<int, int> cell);
    public interface ITicTacToeInput
    {
        TicTacToeCellType CellType { get; set; }
        bool NeedsOutput { get; }
        event InputSentHandler InputSent;
    }
}
