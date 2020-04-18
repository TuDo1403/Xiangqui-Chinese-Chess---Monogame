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
        public int Value { get; }
        public List<Point> LegalMoves { get; }
        public Point Index { get; set ; }

        public List<Point> FindLegalMoves(int[][] board)
        {
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

        private void FindFlyingMove(int[][] board)
        {
            //var opponentSide = Value < 0 ? 1 : 0;
            //var enemyGeneral = ChessBoard.GetInstance()
            //                             .Pieces[opponentSide].Where(c => c.GetType() == typeof(General))
            //                                                  .SingleOrDefault();
            //if (enemyGeneral != null)
            //{
            //    var enemyGeneralPos = enemyGeneral.Index;
            //    // Check if two general are in the same rank (horizontal line)
            //    if (Index.X / enemyGeneralPos.X == 1)
            //        if (!IsBlockedMove(enemyGeneralPos, board))
            //            LegalMoves.Add(enemyGeneralPos);
            //}
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 9; ++j)
                    if (Math.Abs(board[i][j]) == 100 && board[i][j] * Value < 0)
                        if (Index.X == j)
                            if (!IsBlockedMove(new Point(j, i), board))
                                LegalMoves.Add(new Point(j, i));
        }

        private bool IsBlockedMove(Point point, int[][] board)
        {
            var sum = 0;
            var highY = Index.Y < point.Y ? point.Y : Index.Y;
            var lowY = Index.Y > point.Y ? point.Y : Index.Y;
            while (lowY < highY - 1)
                sum += Math.Abs(board[++lowY][Index.X]);

            return sum != 0;
        }

        private void FindHorizontalMoves(int[][] board)
        {
            if (Index.X + 1 <= (int)BoardRule.R_CASTLE)
                StillHasLegalMoves(Index.Y, Index.X + 1, board);

            if (Index.X - 1 >= (int)BoardRule.L_CASTLE)
                StillHasLegalMoves(Index.Y, Index.X - 1, board);
        }

        private void FindVerticalMoves(int[][] board)
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
        protected bool StillHasLegalMoves(int row, int column, int[][] board)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            if (board[row][column] * Value > 0) return false;
            else
            {
                LegalMoves.Add(new Point(column, row));
                if (board[row][column] * Value < 0) return false;
            }
            return true;
        }
    }
}

