using System;
using TicTacToe.Enums;
using TicTacToe.Interfaces;

namespace TicTacToe.Classes
{
    public class TicTacToeCpuInput : ITicTacToeInput
    {
        private readonly Func<int, int, TicTacToeCellType> _getCell;
        private readonly TicTacToeCpuDifficulty _difficulty;

        public event InputSentHandler InputSent;

        public TicTacToeCpuInput(TicTacToeCpuDifficulty difficulty, Func<int, int, TicTacToeCellType> getCell)
        {
            _getCell = getCell;
            _difficulty = difficulty;
        }
        public TicTacToeCellType CellType { get; set; }
        public bool NeedsOutput => true;//false;
        public void GetInput()
        {
            var random = false;
            var rndGen = new Random((int)DateTime.Now.Ticks);
            int row = -1, col = -1;
            switch (_difficulty)
            {
            case TicTacToeCpuDifficulty.Easy:
                random = rndGen.Next() % 2 == 0;
                break;
            case TicTacToeCpuDifficulty.Medium:
                random = rndGen.Next() % 4 == 0;
                break;
            case TicTacToeCpuDifficulty.Hard:
                random = rndGen.Next() % 8 == 0;
                break;
            case TicTacToeCpuDifficulty.Impossible:
            default:
                random = false;
                break;
            }

            if (random)
            {
                row = rndGen.Next() % 3;
                col = rndGen.Next() % 3;
            }
            else
            {
                #region IA
                var starts = true;
                var emptyCount = 0;
                var found = false;

                for (var i = 0; i < 9; i++)
                {
                    if (_getCell(i / 3, i % 3) != TicTacToeCellType.Empty)
                        starts = false;
                    else
                        emptyCount++;
                }

                if (starts)
                {
                    row = 0;
                    col = 0;
                    found = true;
                }
                else if (emptyCount == 8)
                {
                    var center = _getCell(1, 1);

                    if (center == TicTacToeCellType.Empty)
                    {
                        row = 1;
                        col = 1;
                        found = true;
                    }
                }

                if (emptyCount == 1)
                {
                    for (var i = 0; i < 9; i++)
                    {
                        if (_getCell(i / 3, i % 3) == TicTacToeCellType.Empty)
                        {
                            found = true;
                            row = i / 3;
                            col = i % 3;
                        }
                    }
                }

                if (!found)
                {
                    #region Complete self three
                    for (var r = 0; r < 3 && !found; r++)
                    {
                        var selfCount = 0;
                        var empty = Tuple.Create(r, -1);

                        for (var c = 0; c < 3 && !found; c++)
                        {
                            var cell = _getCell(r, c);

                            if (cell != TicTacToeCellType.Empty && cell == CellType)
                            {
                                selfCount++;
                            }
                            else if (cell == TicTacToeCellType.Empty)
                            {
                                empty = Tuple.Create(r, c);
                            }
                        }

                        if (selfCount == 2 && empty.Item1 >= 0 && empty.Item2 >= 0)
                        {
                            found = true;
                            row = empty.Item1;
                            col = empty.Item2;
                        }
                    }

                    for (var c = 0; c < 3 && !found; c++)
                    {
                        var selfCount = 0;
                        var empty = Tuple.Create(-1, c);

                        for (var r = 0; r < 3 && !found; r++)
                        {
                            var cell = _getCell(r, c);

                            if (cell != TicTacToeCellType.Empty && cell == CellType)
                            {
                                selfCount++;
                            }
                            else if (cell == TicTacToeCellType.Empty)
                            {
                                empty = Tuple.Create(r, c);
                            }
                        }

                        if (selfCount == 2 && empty.Item1 >= 0 && empty.Item2 >= 0)
                        {
                            found = true;
                            row = empty.Item1;
                            col = empty.Item2;
                        }
                    }

                    if (!found)
                    {
                        var selfCount = 0;
                        var empty = Tuple.Create(-1, -1);

                        for (int r = 0, c = 0; r < 3 && c < 3 && !found; r++, c++)
                        {
                            var cell = _getCell(r, c);

                            if (cell != TicTacToeCellType.Empty && cell == CellType)
                            {
                                selfCount++;
                            }
                            else if (cell == TicTacToeCellType.Empty)
                            {
                                empty = Tuple.Create(r, c);
                            }
                        }

                        if (selfCount == 2 && empty.Item1 >= 0 && empty.Item2 >= 0)
                        {
                            found = true;
                            row = empty.Item1;
                            col = empty.Item2;
                        }
                    }

                    if (!found)
                    {
                        var selfCount = 0;
                        var empty = Tuple.Create(-1, -1);

                        for (int r = 0, c = 2; r < 3 && c >= 0 && !found; r++, c--)
                        {
                            var cell = _getCell(r, c);

                            if (cell != TicTacToeCellType.Empty && cell == CellType)
                            {
                                selfCount++;
                            }
                            else if (cell == TicTacToeCellType.Empty)
                            {
                                empty = Tuple.Create(r, c);
                            }
                        }

                        if (selfCount == 2 && empty.Item1 >= 0 && empty.Item2 >= 0)
                        {
                            found = true;
                            row = empty.Item1;
                            col = empty.Item2;
                        }
                    }
                    #endregion

                    #region Opposite block
                    for (var r = 0; r < 3 && !found; r++)
                    {
                        var oppositeCount = 0;
                        var empty = Tuple.Create(r, -1);

                        for (var c = 0; c < 3 && !found; c++)
                        {
                            var cell = _getCell(r, c);

                            if (cell != TicTacToeCellType.Empty && cell != CellType)
                            {
                                oppositeCount++;
                            }
                            else if (cell == TicTacToeCellType.Empty)
                            {
                                empty = Tuple.Create(r, c);
                            }
                        }

                        if (oppositeCount == 2 && empty.Item1 >= 0 && empty.Item2 >= 0)
                        {
                            found = true;
                            row = empty.Item1;
                            col = empty.Item2;
                        }
                    }

                    for (var c = 0; c < 3 && !found; c++)
                    {
                        var oppositeCount = 0;
                        var empty = Tuple.Create(-1, c);

                        for (var r = 0; r < 3 && !found; r++)
                        {
                            var cell = _getCell(r, c);

                            if (cell != TicTacToeCellType.Empty && cell != CellType)
                            {
                                oppositeCount++;
                            }
                            else if (cell == TicTacToeCellType.Empty)
                            {
                                empty = Tuple.Create(r, c);
                            }
                        }

                        if (oppositeCount == 2 && empty.Item1 >= 0 && empty.Item2 >= 0)
                        {
                            found = true;
                            row = empty.Item1;
                            col = empty.Item2;
                        }
                    }

                    if (!found)
                    {
                        var oppositeCount = 0;
                        var empty = Tuple.Create(-1, -1);

                        for (int r = 0, c = 0; r < 3 && c < 3 && !found; r++, c++)
                        {
                            var cell = _getCell(r, c);

                            if (cell != TicTacToeCellType.Empty && cell != CellType)
                            {
                                oppositeCount++;
                            }
                            else if (cell == TicTacToeCellType.Empty)
                            {
                                empty = Tuple.Create(r, c);
                            }
                        }

                        if (oppositeCount == 2 && empty.Item1 >= 0 && empty.Item2 >= 0)
                        {
                            found = true;
                            row = empty.Item1;
                            col = empty.Item2;
                        }
                    }

                    if (!found)
                    {
                        var oppositeCount = 0;
                        var empty = Tuple.Create(-1, -1);

                        for (int r = 0, c = 2; r < 3 && c >= 0 && !found; r++, c--)
                        {
                            var cell = _getCell(r, c);

                            if (cell != TicTacToeCellType.Empty && cell != CellType)
                            {
                                oppositeCount++;
                            }
                            else if (cell == TicTacToeCellType.Empty)
                            {
                                empty = Tuple.Create(r, c);
                            }
                        }

                        if (oppositeCount == 2 && empty.Item1 >= 0 && empty.Item2 >= 0)
                        {
                            found = true;
                            row = empty.Item1;
                            col = empty.Item2;
                        }
                    }
                    #endregion

                    #region Corners or random
                    if (!found)
                    {
                        var corner1 = _getCell(0, 0);
                        var corner2 = _getCell(0, 2);
                        var corner3 = _getCell(2, 2);
                        var corner4 = _getCell(2, 0);
                        var edge1 = _getCell(0, 1);
                        var edge2 = _getCell(1, 2);
                        var edge3 = _getCell(2, 1);
                        var edge4 = _getCell(1, 0);

                        if (corner1 == TicTacToeCellType.Empty && corner1 == corner2 && corner2 == corner3 && corner3 == corner4)
                        {
                            found = true;
                            row = 0;
                            col = 0;
                        }
                        else
                        {
                            var center = _getCell(1, 1);
                            if (corner1 == CellType)
                            {
                                if (emptyCount == 8)
                                {
                                    if (corner3 == TicTacToeCellType.Empty)
                                    {
                                        found = true;
                                        row = 2;
                                        col = 2;
                                    }

                                    if (!found && corner2 == TicTacToeCellType.Empty)
                                    {
                                        found = true;
                                        row = 0;
                                        col = 2;
                                    }

                                    if (!found && corner4 == TicTacToeCellType.Empty)
                                    {
                                        found = true;
                                        row = 2;
                                        col = 0;
                                    }
                                }
                                else if (emptyCount == 7)
                                {
                                    if (corner2 == TicTacToeCellType.Empty && corner2 == corner3 && corner3 == corner4 && corner4 == center)
                                    {
                                        found = true;
                                        row = 1;
                                        col = 1;
                                    }
                                }
                                else if (emptyCount == 6)
                                {
                                    if (corner3 == TicTacToeCellType.Empty)
                                    {
                                        found = true;
                                        row = 2;
                                        col = 2;
                                    }

                                    if (!found && corner2 == TicTacToeCellType.Empty)
                                    {
                                        found = true;
                                        row = 0;
                                        col = 2;
                                    }

                                    if (!found && corner4 == TicTacToeCellType.Empty)
                                    {
                                        found = true;
                                        row = 2;
                                        col = 0;
                                    }
                                }
                            }
                            else
                            {
                                if (center == CellType)
                                {
                                    if (emptyCount == 6)
                                    {
                                        if (edge1 == TicTacToeCellType.Empty && edge3 == TicTacToeCellType.Empty)
                                        {
                                            found = true;
                                            row = 0;
                                            col = 1;
                                        }

                                        if (!found && edge2 == TicTacToeCellType.Empty && edge4 == TicTacToeCellType.Empty)
                                        {
                                            found = true;
                                            row = 1;
                                            col = 2;
                                        }

                                        if (!found && corner1 == TicTacToeCellType.Empty)
                                        {
                                            found = true;
                                            row = 0;
                                            col = 0;
                                        }

                                        if (!found && corner2 == TicTacToeCellType.Empty)
                                        {
                                            found = true;
                                            row = 0;
                                            col = 2;
                                        }

                                        if (!found && corner3 == TicTacToeCellType.Empty)
                                        {
                                            found = true;
                                            row = 2;
                                            col = 2;
                                        }

                                        if (!found && corner4 == TicTacToeCellType.Empty)
                                        {
                                            found = true;
                                            row = 2;
                                            col = 0;
                                        }

                                        if (!found && edge3 == TicTacToeCellType.Empty)
                                        {
                                            found = true;
                                            row = 2;
                                            col = 1;
                                        }

                                        if (!found && edge4 == TicTacToeCellType.Empty)
                                        {
                                            found = true;
                                            row = 1;
                                            col = 0;
                                        }
                                    }
                                }
                            }

                            if (!found && corner2 == TicTacToeCellType.Empty && corner2 == corner3 && corner3 == corner4)
                            {
                                if (edge1 == TicTacToeCellType.Empty && edge2 == TicTacToeCellType.Empty)
                                {
                                    found = true;
                                    row = 0;
                                    col = 2;
                                }
                            }

                            if (!found)
                            {
                                if (corner3 == TicTacToeCellType.Empty && corner3 == corner4)
                                {
                                    if (edge2 == TicTacToeCellType.Empty && edge3 == TicTacToeCellType.Empty)
                                    {
                                        found = true;
                                        row = 2;
                                        col = 2;
                                    }
                                }
                                else
                                {
                                    if (corner4 == TicTacToeCellType.Empty)
                                    {
                                        found = true;
                                        row = 2;
                                        col = 0;
                                    }
                                    else
                                    {
                                        if (center == CellType)
                                        {
                                            if (corner1 == CellType)
                                            {
                                                if (edge1 == TicTacToeCellType.Empty)
                                                {
                                                    found = true;
                                                    row = 1;
                                                    col = 0;
                                                }
                                                else if (edge2 == TicTacToeCellType.Empty)
                                                {
                                                    found = true;
                                                    row = 0;
                                                    col = 1;
                                                }
                                            }
                                            else if (corner2 == CellType)
                                            {
                                                if (edge1 == TicTacToeCellType.Empty)
                                                {
                                                    found = true;
                                                    row = 1;
                                                    col = 2;
                                                }
                                                else if (edge2 == TicTacToeCellType.Empty)
                                                {
                                                    found = true;
                                                    row = 0;
                                                    col = 1;
                                                }
                                            }
                                            else if (corner3 == CellType)
                                            {
                                                if (edge1 == TicTacToeCellType.Empty)
                                                {
                                                    found = true;
                                                    row = 1;
                                                    col = 2;
                                                }
                                                else if (edge2 == TicTacToeCellType.Empty)
                                                {
                                                    found = true;
                                                    row = 2;
                                                    col = 1;
                                                }
                                            }
                                            else if (corner4 == CellType)
                                            {
                                                if (edge1 == TicTacToeCellType.Empty)
                                                {
                                                    found = true;
                                                    row = 1;
                                                    col = 0;
                                                }
                                                else if (edge2 == TicTacToeCellType.Empty)
                                                {
                                                    found = true;
                                                    row = 2;
                                                    col = 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }

                if (!found || row < 0 || col < 0)
                {
                    row = rndGen.Next() % 3;
                    col = rndGen.Next() % 3;
                }
                #endregion
            }

            InputSent?.Invoke(this, Tuple.Create(row, col));
        }
    }
}
