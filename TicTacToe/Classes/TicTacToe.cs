using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Enums;
using TicTacToe.Interfaces;

namespace TicTacToe.Classes
{
    public delegate void GameEndHandler(TicTacToe game, GameEndType end);
    public class TicTacToe
    {
        private IEnumerable<TicTacToeCellType> Cells;
        private TicTacToePlayer _player1;
        private TicTacToePlayer _player2;
        private Random _random;
        public event GameEndHandler GameEnded;
        private ITicTacToeOutput _output;

        public TicTacToe(ITicTacToeOutput output)
        {
            Cells = Enumerable.Repeat(TicTacToeCellType.Empty, 9).ToArray();
            _output = output;
        }

        public TicTacToeCellType this[int idx] => Cells.ElementAt(idx);
        public TicTacToeCellType this[int row, int column] => this[ConvertToIndex(row, column)];
        public TicTacToeCellType this[Tuple<int, int> t] => this[t.Item1, t.Item2];

        public void MovePlayer(TicTacToePlayer p)
        {
            if (p.NeedsOutput)
                PrintSchema();

            p.CanMove = true;
        }

        public void Start(TicTacToePlayer player1, TicTacToePlayer player2)
        {
            if (player1 == null) throw new ArgumentNullException(nameof(player1));
            if (player2 == null) throw new ArgumentNullException(nameof(player2));

            _player1 = player1;
            _player2 = player2;

            _player1.PlayerMoved += OnPlayerMoved;
            _player2.PlayerMoved += OnPlayerMoved;

            _random = new Random((int)DateTime.Now.Ticks);
            var starts1 = _random.Next() % 2 == 0;

            if (starts1)
                MovePlayer(_player1);
            else
                MovePlayer(_player2);
        }

        private void PrintSchema()
        {
            _output.PrintSchema(Cells);
        }

        protected void OnPlayerMoved(TicTacToePlayer player, Tuple<int, int> cell)
        {
            player.CanMove = false;

            if (this[cell] == TicTacToeCellType.Empty)
            {
                var newCells = Cells.ToArray();
                newCells[ConvertToIndex(cell.Item1, cell.Item2)] = player.CellType;
                Cells = newCells;

                var endType = CheckWin();
                if (endType != null)
                {
                    PrintSchema();
                    GameEnded?.Invoke(this, endType.Value);
                }
                else
                {
                    if (player == _player1)
                        MovePlayer(_player2);
                    else if (player == _player2)
                        MovePlayer(_player1);
                }
            }
            else
            {
                MovePlayer(player);
            }
        }

        private static int ConvertToIndex(int row, int column) => row * 3 + column;

        protected GameEndType? CheckWin()
        {
            var found = false;
            var cell = TicTacToeCellType.Empty;
            Func<Func<int, int>, bool> findSequence = i =>
            {
                var res = true;
                cell = TicTacToeCellType.Empty;

                for (var c = 0; c < 3 && res; c++)
                {
                    var curr = this[i(c)];
                    if (curr == TicTacToeCellType.Empty)
                        res = false;
                    else
                    {
                        if (cell == TicTacToeCellType.Empty)
                            cell = curr;
                        else if (cell != curr)
                            res = false;
                    }
                }

                return res;
            };

            for (var r = 0; r < 3 && !found; r++)
            {
                found = findSequence(c => ConvertToIndex(r, c));

                if (!found) found = findSequence(c => ConvertToIndex(c, r));
            }

            if (!found)
                found = findSequence(c => c * 2 + 2);

            if (!found)
                found = findSequence(c => c * 4);

            if (found)
            {
                if (cell == _player1.CellType)
                    return GameEndType.Player1Win;
                else
                    return GameEndType.Player2Win;
            }
            else if (Cells.All(t => t != TicTacToeCellType.Empty))
                return GameEndType.Draw;
            else return null;
        }
    }
}
