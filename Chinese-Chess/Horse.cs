using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public class Horse : Piece
    {

        public Horse(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Type = Rules.HORSE;
            FindNextMoves();
        }


        protected override void FindNextMoves()
        {
            base.FindNextMoves();

            var matrixPosToVector2 = MatrixPos.ToVector2();
            FindLShapedMoves(matrixPosToVector2);
            RemoveInvalidMoves();
        }

        private void RemoveInvalidMoves()
        {
            ValidMoves.RemoveAll(OutOfRangeMove());

            var horseValue = Xiangqui.Board[MatrixPos.X][MatrixPos.Y];
            ValidMoves.RemoveAll(c => Xiangqui.Board[c.X][c.Y] * horseValue > 0);

            ValidMoves.RemoveAll(IsBlockedMove);
        }

        private static Predicate<Point> OutOfRangeMove()
        {
            return c => c.X < 0 || c.X > 9 || c.Y < 0 || c.Y > 8;
        }

        private bool IsBlockedMove(Point move)
        {
            var offsetX = (move.X - MatrixPos.X) / Math.Abs(move.X - MatrixPos.X);
            var offsetY = (move.Y - MatrixPos.Y) / Math.Abs(move.Y - MatrixPos.Y);
            if (Math.Abs(move.X - MatrixPos.X) > Math.Abs(move.Y - MatrixPos.Y))
            {
                return Xiangqui.Board[move.X - offsetX][MatrixPos.Y] != 0;
            }
            else
            {
                return Xiangqui.Board[MatrixPos.X][move.Y - offsetY] != 0;
            }
        }

        private void FindLShapedMoves(Vector2 currentPos)
        {
            var currentLocation = MatrixPos.ToVector2();
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
