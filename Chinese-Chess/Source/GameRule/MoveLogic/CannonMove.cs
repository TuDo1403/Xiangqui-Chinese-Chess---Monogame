using ChineseChess.Source.GameObjects.Chess;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ChineseChess.Source.GameRule.MoveLogic
{
    public class CannonMove : IMovable
    {
        public int Value { get; set; } = 5;
        public List<Point> LegalMoves { get; }
        public Point Index { get; set; }

        public List<Point> FindLegalMoves(BoardState board)
        {
            Value = board[Index.Y, Index.X];
            FindHorizontalMoves(board);
            FindVerticalMoves(board);
            return LegalMoves;
        }

        public CannonMove(Point idx)
        {
            LegalMoves = new List<Point>();
            Index = idx;
        }

        private void FindHorizontalMoves(BoardState board)
        {
            var rowIdx = Index.Y;
            for (int i = Index.X + 1; i < (int)Rule.COL; ++i)
            {
                if (board[rowIdx, i] != 0)
                {
                    while (i++ < (int)Rule.COL - 1)
                    {
                        if (board[rowIdx, i] * Value > 0) break;
                        if (board[rowIdx, i] * Value < 0)
                        {
                            LegalMoves.Add(new Point(i, rowIdx));
                            break;
                        }
                    }
                    break;
                }
                else LegalMoves.Add(new Point(i, rowIdx));
            }

            for (int i = Index.X - 1; i >= 0; --i)
            {
                if (board[rowIdx, i] != 0)
                {
                    while (i-- > 0)
                    {
                        if (board[rowIdx, i] * Value > 0) break;
                        if (board[rowIdx, i] * Value < 0)
                        {
                            LegalMoves.Add(new Point(i, rowIdx));
                            break;
                        }
                    }
                    break;
                }
                else LegalMoves.Add(new Point(i, rowIdx));
            }
        }

        private void FindVerticalMoves(BoardState board)
        {
            var colIdx = Index.X;
            for (int i = Index.Y + 1; i < (int)Rule.ROW; ++i)
            {
                if (board[i, colIdx] != 0)
                {
                    while (i++ < (int)Rule.ROW - 1)
                    {
                        if (board[i, colIdx] * Value > 0) break;
                        if (board[i, colIdx] * Value < 0)
                        {
                            LegalMoves.Add(new Point(colIdx, i));
                            break;
                        }
                    }
                    break;
                }
                else LegalMoves.Add(new Point(colIdx, i));
            }

            for (int i = Index.Y - 1; i >= 0; --i)
            {
                if (board[i, colIdx] != 0)
                {
                    while (i-- > 0)
                    {
                        if (board[i, colIdx] * Value > 0) break;

                        if (board[i, colIdx] * Value < 0)
                        {
                            LegalMoves.Add(new Point(colIdx, i));
                            break;
                        }
                    }
                    break;
                }
                else LegalMoves.Add(new Point(colIdx, i));
            }
        }
    }
}

