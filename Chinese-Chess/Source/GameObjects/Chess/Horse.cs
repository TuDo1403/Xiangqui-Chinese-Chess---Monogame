using ChineseChess.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.GameObjects.Chess
{
    public sealed class Horse : Piece
    {

        public Horse(Texture2D texture, Vector2 position, int type) : base(texture, position, type)
        {
        }


        protected override void FindNextMoves()
        {
            base.FindNextMoves();

            var matrixPosToVector2 = MatrixPos.ToVector2();
            FindLShapedMoves(matrixPosToVector2);
            RemoveInvalidMoves();
        }

        protected override void RemoveInvalidMoves()
        {
            ValidMoves.RemoveAll(OutOfRangeMove());

            var horseValue = ChessBoard.MatrixBoard[MatrixPos.Y][MatrixPos.X];
            ValidMoves.RemoveAll(c => ChessBoard.MatrixBoard[c.Y][c.X] * horseValue > 0);

            ValidMoves.RemoveAll(IsBlockedMove);
        }

        protected override Predicate<Point> OutOfRangeMove()
        {
            return c => c.Y < 0 || c.Y > 9 || c.X < 0 || c.X > 8;
        }

        protected override bool IsBlockedMove(Point move)
        {
            if (Math.Abs(MatrixPos.X - move.X) == 2)
            {
                return ChessBoard.MatrixBoard[MatrixPos.Y][(MatrixPos.X + move.X) / 2] != 0;
            }
            else
            {
                return ChessBoard.MatrixBoard[(MatrixPos.Y + move.Y) / 2][MatrixPos.X] != 0;
            }
        }

        private void FindLShapedMoves(Vector2 currentPos)
        {
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(2, 1)).ToPoint());
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(-2, 1)).ToPoint());
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(2, -1)).ToPoint());
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(-2, -1)).ToPoint());

            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(1, 2)).ToPoint());
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(-1, 2)).ToPoint());
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(1, -2)).ToPoint());
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(-1, -2)).ToPoint());
        }
    }
}
