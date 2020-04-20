using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI.MoveLogic
{
    public class ElephantMove : IMovable
    {
        public int Value { get; set; }
        public List<Point> LegalMoves { get; }
        public Point Index { get; set; }

        public List<Point> FindLegalMoves(int[][] board)
        {
            Value = board[Index.Y][Index.X];
            var IdxToVector2 = Index.ToVector2();
            FindCrossMove(IdxToVector2);

            RemoveIllegalMoves(board);
            return LegalMoves;
        }

        public ElephantMove(Point idx)
        {
            Index = idx;
            LegalMoves = new List<Point>();
        }

        private void RemoveIllegalMoves(int[][] board)
        {
            LegalMoves.RemoveAll(OutOfRangeMove());

            LegalMoves.RemoveAll(c => board[c.Y][c.X] * Value > 0);

            LegalMoves.RemoveAll(c => IsBlockedMove(c, board));
        }

        private bool IsBlockedMove(Point move, int[][] board)
        {
            return board[(Index.Y + move.Y) / 2][(Index.X + move.X) / 2] != 0;
        }

        private Predicate<Point> OutOfRangeMove()
        {
            return c => c.Y < 0 || c.Y >= (int)BoardRule.ROW ||
                        c.X < 0 || c.X >= (int)BoardRule.COL ||
                        Value > 0 && c.Y < (int)BoardRule.R_BORD ||
                        Value < 0 && c.Y > (int)BoardRule.B_BORD;
        }

        private void FindCrossMove(Vector2 currentPos)
        {
            LegalMoves.Add(Vector2.Add(currentPos, new Vector2(2, 2)).ToPoint());
            LegalMoves.Add(Vector2.Add(currentPos, new Vector2(2, -2)).ToPoint());

            LegalMoves.Add(Vector2.Add(currentPos, new Vector2(-2, 2)).ToPoint());
            LegalMoves.Add(Vector2.Add(currentPos, new Vector2(-2, -2)).ToPoint());
        }
    }
}

