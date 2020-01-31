using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public class Elephant : Piece
    {
        public Elephant(Texture2D texture, Vector2 position, int type) : base(texture, position, type)
        {
        }


        protected override void FindNextMoves()
        {
            base.FindNextMoves();

            var matrixPosToVector2 = MatrixPos.ToVector2();
            FindCrossMove(matrixPosToVector2);
            RemoveInvalidMoves();
            //PrintValidMove();
        }

        protected override void RemoveInvalidMoves()
        {
            ValidMoves.RemoveAll(OutOfRangeMove());

            var elephantValue = Xiangqui.Board[MatrixPos.Y][MatrixPos.X];
            ValidMoves.RemoveAll(c => Xiangqui.Board[c.Y][c.X] * elephantValue > 0);

            ValidMoves.RemoveAll(IsBlockedMove);
        }

        protected override bool IsBlockedMove(Point move)
        {
            return Xiangqui.Board[(MatrixPos.Y + move.Y)/2][(MatrixPos.X + move.X)/2] != 0;
        }

        protected override Predicate<Point> OutOfRangeMove()
        {
            return c => c.Y < 0 || c.Y > 9 || c.X < 0 || c.X > 8 ||
                        Type > 0 && c.Y < 5 ||
                        Type < 0 && c.Y > 4;
        }

        private void FindCrossMove(Vector2 currentPos)
        {
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(2, 2)).ToPoint());
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(2, -2)).ToPoint());

            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(-2, 2)).ToPoint());
            ValidMoves.Add(Vector2.Add(currentPos, new Vector2(-2, -2)).ToPoint());
        }
    }
}
