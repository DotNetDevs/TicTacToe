using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Enums;
using TicTacToe.Interfaces;

namespace TicTacToe.Classes
{
    public delegate void UserMoveHandler(TicTacToePlayer sender, Tuple<int, int> cell);
    public delegate void UserWaitingHandler(TicTacToePlayer sender);
    public class TicTacToePlayer
    {
        private bool _canMove;
        public bool CanMove
        {
            get
            {
                return _canMove;
            }
            set
            {
                _canMove = value;
                if (_canMove)
                    PlayerWaiting?.Invoke(this);
            }
        }
        public TicTacToeCellType CellType => _input.CellType;
        public event UserMoveHandler PlayerMoved;
        public event UserWaitingHandler PlayerWaiting;
        private readonly ITicTacToeInput _input;
        public bool NeedsOutput => _input?.NeedsOutput ?? false;

        public TicTacToePlayer(ITicTacToeInput input)
        {
            _input = input;
            _input.InputSent += (sender, args) =>
            {
                if (CanMove)
                {
                    PlayerMoved?.Invoke(this, args);
                }
            };
        }
    }
}
