using ChineseChess.Source.GameObjects.Chess;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ChineseChess.Source.GameRule.MoveLogic
{
    public class SoldierMove : IMovable
    {
        public int Value { get; set; }

        public List<Point> LegalMoves { get; }

        public Point Index { get; set; }

        public List<Point> FindLegalMoves(BoardState board)
        {
            Value = board[Index.Y, Index.X];
            FindVerticalMoves(board);
            if (RiverCrossed()) FindHorizontalMoves(board);

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
                if (board[Index.Y, Index.X + 1] * Value <= 0)
                    LegalMoves.Add(new Point(Index.X + 1, Index.Y));

            if (Index.X - 1 >= 0)
                if (board[Index.Y, Index.X - 1] * Value <= 0)
                    LegalMoves.Add(new Point(Index.X - 1, Index.Y));
        }

        private void FindVerticalMoves(BoardState board)
        {
            var step = Value > 0 ? -1 : 1;
            if (Index.Y + step >= 0 && Index.Y + step < (int)Rule.ROW)
                if (board[Index.Y + step, Index.X] * Value <= 0)
                    LegalMoves.Add(new Point(Index.X, Index.Y + step));
        }
    }
}
