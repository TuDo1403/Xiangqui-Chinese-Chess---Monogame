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

        public General(Texture2D texture, Vector2 position, int type) : base(texture, position, type)
        {
        }



        protected override void FindNextMoves()
        {
            base.FindNextMoves();
            FindVerticalMoves();
            FindHorizontalMoves();
            FindFlyingMove();
        }

        private void FindFlyingMove()
        {
            var opponentSide = 0;
            if (Type < 0)
            {
                opponentSide = 1;
            }
            var enemyGeneral = ChessBoard.GetInstance()
                                         .Pieces[opponentSide].Where(c => c.GetType() == typeof(General))
                                                              .SingleOrDefault();
            if (enemyGeneral != null)
            {
                var enemyGeneralPos = enemyGeneral.MatrixPos;
                // Check if two general are in the same rank (horizontal line)
                if (MatrixPos.X / enemyGeneralPos.X == 1)
                {
                    if (!IsBlockedMove(enemyGeneralPos))
                    {
                        ValidMoves.Add(enemyGeneralPos);
                    }
                }
            }
        }

        protected override bool IsBlockedMove(Point point)
        {
            var sum = 0;
            var highY = MatrixPos.Y < point.Y ? point.Y : MatrixPos.Y;
            var lowY = MatrixPos.Y > point.Y ? point.Y : MatrixPos.Y;
            while (lowY < highY-1)
            {
                sum += Math.Abs(ChessBoard.MatrixBoard[++lowY][MatrixPos.X]);
            }
            return sum != 0;
        }

        protected override void FindHorizontalMoves()
        {
            if (MatrixPos.X + 1 <= (int)BoardRule.RightCastle)
            {
                StillHasValidMoves(MatrixPos.Y, MatrixPos.X + 1);
            }
            if (MatrixPos.X - 1 >= (int)BoardRule.LeftCastle)
            {
                StillHasValidMoves(MatrixPos.Y, MatrixPos.X - 1);
            }
        }

        protected override void FindVerticalMoves()
        {
            if (Type > 0)
            {
                if (MatrixPos.Y + 1 < (int)BoardRule.Rows)
                {
                    StillHasValidMoves(MatrixPos.Y + 1, MatrixPos.X);
                }
                if (MatrixPos.Y - 1 >= (int)BoardRule.FrontRedCastle)
                {
                    StillHasValidMoves(MatrixPos.Y - 1, MatrixPos.X);
                }
            }
            else
            {
                if (MatrixPos.Y + 1 <= (int)BoardRule.FrontBlackCastle)
                {
                    StillHasValidMoves(MatrixPos.Y + 1, MatrixPos.X);
                }
                if (MatrixPos.Y - 1 >= 0)
                {
                    StillHasValidMoves(MatrixPos.Y - 1, MatrixPos.X);
                }
            }
        }
    }
}
