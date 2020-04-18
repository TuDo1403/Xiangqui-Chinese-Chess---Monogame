using ChineseChess.Source.GameRule;
using ChineseChess.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.GameObjects.Chess
{
    public sealed class General : Piece
    {

        public General(Texture2D texture, Vector2 position, int type, ChessBoard board) : base(texture, position, type, board)
        {
        }



        protected override void FindLegalMoves(int[][] board)
        {
            base.FindLegalMoves(board);
            FindVerticalMoves(board);
            FindHorizontalMoves(board);
            FindFlyingMove(board);
        }

        private void FindFlyingMove(int[][] board)
        {
            var opponentSide = Value < 0 ? 1 : 0;
            var enemyGeneral = ChessBoard.GetInstance()
                                         .Pieces[opponentSide].Where(c => c.GetType() == typeof(General))
                                                              .SingleOrDefault();
            if (enemyGeneral != null)
            {
                var enemyGeneralPos = enemyGeneral.Index;
                // Check if two general are in the same rank (horizontal line)
                if (Index.X / enemyGeneralPos.X == 1)
                    if (!IsBlockedMove(enemyGeneralPos, board))
                        LegalMoves.Add(enemyGeneralPos);
            }
        }

        protected override bool IsBlockedMove(Point point, int[][] board)
        {
            var sum = 0;
            var highY = Index.Y < point.Y ? point.Y : Index.Y;
            var lowY = Index.Y > point.Y ? point.Y : Index.Y;
            while (lowY < highY-1)
                sum += Math.Abs(board[++lowY][Index.X]);

            return sum != 0;
        }

        protected override void FindHorizontalMoves(int[][] board)
        {
            if (Index.X + 1 <= (int)BoardRule.R_CASTLE)
                StillHasLegalMoves(Index.Y, Index.X + 1, board);

            if (Index.X - 1 >= (int)BoardRule.L_CASTLE)
                StillHasLegalMoves(Index.Y, Index.X - 1, board);
        }

        protected override void FindVerticalMoves(int[][] board)
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
    }
}
