using ChineseChess.Source.GameObjects.Chess;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ChineseChess.Source.GameRule.MoveLogic
{
    public class ChariotMove : IMovable
    {

        public int Value { get; set; }
        public List<Point> LegalMoves { get; } = new List<Point>();
        public Point Index { get; set; }

        public List<Point> FindLegalMoves(BoardState board)
        {
            Value = board[Index.Y, Index.X];
            FindHorizontalMoves(board);
            FindVerticalMoves(board);
            return LegalMoves;
        }

        public ChariotMove(Point idx)
        {
            Index = idx;
        }

        private void FindHorizontalMoves(BoardState board)
        {
            var rowIdx = Index.Y;
            for (int i = Index.X + 1; i < (int)Rule.COL; ++i)
            {
                if (board[rowIdx, i] * Value > 0)
                {
                    break;
                }
                else
                {
                    LegalMoves.Add(new Point(i, rowIdx));
                    if (board[rowIdx, i] * Value < 0)
                    {
                        break;
                    }
                }
            }
            for (int i = Index.X - 1; i >= 0; --i)
            {
                if (board[rowIdx, i] * Value > 0)
                {
                    break;
                }
                else
                {
                    LegalMoves.Add(new Point(i, rowIdx));
                    if (board[rowIdx, i] * Value < 0)
                    {
                        break;
                    }
                }
            }
        }

        private void FindVerticalMoves(BoardState board)
        {
            var colIdx = Index.X;
            for (int i = Index.Y + 1; i < (int)Rule.ROW; ++i)
            {
                if (board[i, colIdx] * Value > 0)
                {
                    break;
                }
                else
                {
                    LegalMoves.Add(new Point(colIdx, i));
                    if (board[i, colIdx] * Value < 0)
                    {
                        break;
                    }
                }
            }
            for (int i = Index.Y - 1; i >= 0; --i)
            {
                if (board[i, colIdx] * Value > 0)
                {
                    break;
                }
                else
                {
                    LegalMoves.Add(new Point(colIdx, i));
                    if (board[i, colIdx] * Value < 0)
                    {
                        break;
                    }
                }
            }
        }
    }
}
