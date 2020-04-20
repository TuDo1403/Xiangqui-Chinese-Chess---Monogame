using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI.MoveLogic
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

        public List<Point> FindLegalMoves(int[][] board)
        {
            Value = board[Index.Y][Index.X];
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

        protected void RemoveIllegalMoves(int[][] board)
        {
            LegalMoves.RemoveAll(OutOfRangeMove());

            LegalMoves.RemoveAll(c => board[c.Y][c.X] * Value > 0);
        }

        protected Predicate<Point> OutOfRangeMove()
        {
            return c => c.Y < 0 || c.Y >= (int)BoardRule.ROW ||
                        c.X > (int)BoardRule.R_CASTLE ||
                        c.X < (int)BoardRule.L_CASTLE ||
                        Value > 0 && c.Y < (int)BoardRule.FR_CASTLE ||
                        Value < 0 && c.Y > (int)BoardRule.FB_CASTLE;
        }
    }
}
