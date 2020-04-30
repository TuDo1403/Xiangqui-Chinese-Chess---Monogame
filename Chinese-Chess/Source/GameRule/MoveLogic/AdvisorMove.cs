using ChineseChess.Source.GameObjects.Chess;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ChineseChess.Source.GameRule.MoveLogic
{
    public class AdvisorMove : IMovable
    {
        public Point Index { get; set; }

        public List<Point> LegalMoves { get; }

        public int Value { get; set; } = 2;



        public AdvisorMove(Point idx)
        {
            LegalMoves = new List<Point>();
            Index = idx;
        }

        public List<Point> FindLegalMoves(BoardState board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            Value = board[Index.Y, Index.X];
            var IdxToVector2 = Index.ToVector2();
            FindCrossMove(IdxToVector2);

            RemoveIllegalMoves(board);

            return LegalMoves;
        }

        private void FindCrossMove(Vector2 currentPosition)
        {
            LegalMoves.Add(Vector2.Add(currentPosition, new Vector2(1, 1)).ToPoint());
            LegalMoves.Add(Vector2.Add(currentPosition, new Vector2(1, -1)).ToPoint());

            LegalMoves.Add(Vector2.Add(currentPosition, new Vector2(-1, 1)).ToPoint());
            LegalMoves.Add(Vector2.Add(currentPosition, new Vector2(-1, -1)).ToPoint());
        }

        protected void RemoveIllegalMoves(BoardState board)
        {
            LegalMoves.RemoveAll(OutOfRangeMove());

            LegalMoves.RemoveAll(c => board[c.Y, c.X] * Value > 0);
        }

        protected Predicate<Point> OutOfRangeMove()
        {
            return c => c.Y < 0 || c.Y >= (int)Rule.ROW ||
                        c.X > (int)Rule.R_CASTLE ||
                        c.X < (int)Rule.L_CASTLE ||
                        Value > 0 && c.Y < (int)Rule.FR_CASTLE ||
                        Value < 0 && c.Y > (int)Rule.FB_CASTLE;
        }
    }
}
