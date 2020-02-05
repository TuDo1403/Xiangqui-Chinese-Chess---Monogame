using ChineseChess.Source.GameRule;
using ChineseChess.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ChineseChess.Source.GameObjects.Chess
{
    public sealed class Advisor : Piece
    {
        public Advisor(Texture2D texture, Vector2 position, int type) : base(texture, position, type)
        {
        }


        protected override void FindNextMoves()
        {
            base.FindNextMoves();

            var matrixPosToVector2 = MatrixPos.ToVector2();
            FindCrossMove(matrixPosToVector2);
            RemoveInvalidMoves();

        }

        private void FindCrossMove(Vector2 currentPos)
        {
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(1, 1)).ToPoint());
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(1, -1)).ToPoint());

            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(-1, 1)).ToPoint());
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(-1, -1)).ToPoint());
        }

        protected override void RemoveInvalidMoves()
        {
            ValidMoves.RemoveAll(OutOfRangeMove());

            var advisorValue = ChessBoard.MatrixBoard[MatrixPos.Y][MatrixPos.X];
            ValidMoves.RemoveAll(c => ChessBoard.MatrixBoard[c.Y][c.X] * advisorValue > 0);
        }

        protected override Predicate<Point> OutOfRangeMove()
        {
            return c => c.Y < 0 || c.Y >= (int)BoardRule.Rows ||
                        c.X > (int)BoardRule.RightCastle || 
                        c.X < (int)BoardRule.LeftCastle ||
                        Type > 0 && c.Y < (int)BoardRule.FrontRedCastle ||
                        Type < 0 && c.Y > (int)BoardRule.FrontBlackCastle;
        }
    }
}
