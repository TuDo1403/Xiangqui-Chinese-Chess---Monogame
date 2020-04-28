using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ChineseChess.Source.AI.MoveLogic
{
    public class SoldierMove : IMovable
    {
        public int Value { get; set; }

        public List<Point> LegalMoves { get; }

        public Point Index { get; set; }

        public List<Point> FindLegalMoves(BoardState board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            Value = board[Index.Y, Index.X];
            FindVerticalMoves(board);
            if (RiverCrossed())
            {
                FindHorizontalMoves(board);
            }

            return LegalMoves;
        }

        public SoldierMove(Point idx)
        {
            Index = idx;
            LegalMoves = new List<Point>();
        }

        private bool RiverCrossed()
        {
            return Value < 0 && Index.Y > (int)Rule.B_BORD ||
                   Value > 0 && Index.Y < (int)Rule.R_BORD;
        }

        private void FindHorizontalMoves(BoardState board)
        {
            if (Index.X + 1 < (int)Rule.COL)
            {
                StillHasLegalMoves(Index.Y, Index.X + 1, board);
            }
            if (Index.X - 1 >= 0)
            {
                StillHasLegalMoves(Index.Y, Index.X - 1, board);
            }
        }

        private void FindVerticalMoves(BoardState board)
        {
            var step = 1;
            if (Value > 0)
            {
                step = -step;
            }
            StillHasLegalMoves(Index.Y + step, Index.X, board);
        }

        protected bool StillHasLegalMoves(int row, int column, BoardState board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            if (row < 0 || row > (int)Rule.ROW - 1)
            {
                return false;
            }

            if (column < 0 || column > (int)Rule.COL - 1)
            {
                return false;
            }

            if (board[row, column] * Value > 0)
            {
                return false;
            }
            else
            {
                LegalMoves.Add(new Point(column, row));
                if (board[row, column] * Value < 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
