using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.ConsoleApp
{
    using Classes;
    using Enums;
    using System.Threading;
    using TicTacToe.Classes;
    using TicTacToe = TicTacToe.Classes.TicTacToe;

    class Program
    {
        private static ManualResetEvent waitHandle = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            var ttt = new TicTacToe(new TicTacToeConsoleOutput());
            ttt.GameEnded += OnGameEnded;

            var random = new Random((int)DateTime.Now.Ticks);
            var p1Input = new TicTacToeCpuInput(TicTacToeCpuDifficulty.Impossible, (r, c) => ttt[r, c]) { CellType = TicTacToeCellType.Circle };
            var p2Input = new TicTacToeCpuInput(TicTacToeCpuDifficulty.Easy, (r, c) => ttt[r, c]) { CellType = TicTacToeCellType.Cross };//new TicTacToeUserConsoleInput { CellType = TicTacToeCellType.Cross };
            var p1 = new TicTacToePlayer(p1Input);
            var p2 = new TicTacToePlayer(p2Input);
            p2.PlayerWaiting += sender => p2Input.GetInput();
            p1.PlayerWaiting += sender => p1Input.GetInput();

            ttt.Start(p1, p2);
            waitHandle.WaitOne();
        }

        static void OnGameEnded(TicTacToe game, GameEndType end)
        {
            Console.WriteLine(end);
            waitHandle.Set();
            Console.Read();
        }
    }
}
