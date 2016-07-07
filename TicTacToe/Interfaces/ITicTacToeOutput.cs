using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Enums;

namespace TicTacToe.Interfaces
{
    public interface ITicTacToeOutput
    {
        void PrintSchema(IEnumerable<TicTacToeCellType> cells);
        string GetSymbol(TicTacToeCellType cell);
    }
}
