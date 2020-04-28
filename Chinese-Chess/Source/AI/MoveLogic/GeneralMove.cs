using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI.MoveLogic
{
    public class GeneralMove : IMovable
    {
        public int Value { get; set; }
        public List<Point> LegalMoves { get; }
        public Point Index { get; set ; }

        public List<Point> FindLegalMoves(BoardState board)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));
            Value = board[Index.Y,Index.X];
            FindVerticalMoves(board);
            FindHorizontalMoves(board);
            FindFlyingMove(board);
            return LegalMoves;
        }

        public GeneralMove(Point idx)
        {
            Index = idx;
            LegalMoves = new List<Point>();
        }

        private void FindFlyingMove(BoardState board)
        {
            for (int i = (int)BoardRule.FB_CASTLE; i <= (int)BoardRule.COL; ++i)
                for (int j = 0; j < 9; ++j)
                    if (Math.Abs(board[i,j]) == 6000 && board[i,j] * Value < 0)
                        if (Index.X == j)
                            if (!IsBlockedMove(new Point(j, i), board))
                                LegalMoves.Add(new Point(j, i));
        }

        private bool IsBlockedMove(Point point, BoardState board)
        {
            var sum = 0;
            var highY = Index.Y < point.Y ? point.Y : Index.Y;
            var lowY = Index.Y > point.Y ? point.Y : Index.Y;
            while (lowY < highY - 1)
                sum += Math.Abs(board[++lowY,Index.X]);

            return sum != 0;
        }

        private void FindHorizontalMoves(BoardState board)
        {
            if (Index.X + 1 <= (int)BoardRule.R_CASTLE)
                StillHasLegalMoves(Index.Y, Index.X + 1, board);

            if (Index.X - 1 >= (int)BoardRule.L_CASTLE)
                StillHasLegalMoves(Index.Y, Index.X - 1, board);
        }

        private void FindVerticalMoves(BoardState board)
        {
            if (Value > 0)
            {
                if (Index.Y + 1 < (int)BoardRule.ROW)
                    StillHasLegalMoves(Index.Y + 1, Index.X, board);
                if (Index.Y - 1 >= (int)BoardRule.FR_CASTLE)
                    StillHasLegalMoves(Index.Y - 1, Index.X, board);
            }
            else
            {
                if (Index.Y + 1 <= (int)BoardRule.FB_CASTLE)
                    StillHasLegalMoves(Index.Y + 1, Index.X, board);
                if (Index.Y - 1 >= 0)
                    StillHasLegalMoves(Index.Y - 1, Index.X, board);
            }
        }
        protected bool StillHasLegalMoves(int row, int column, BoardState board)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            if (board[row,column] * Value > 0) return false;
            else
            {
                LegalMoves.Add(new Point(column, row));
                if (board[row,column] * Value < 0) return false;
            }
            return true;
        }
    }
}

