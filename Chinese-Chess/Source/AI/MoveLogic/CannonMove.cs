using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI.MoveLogic
{
    public class CannonMove : IMovable
    {
        public int Value { get; set; } = 5;
        public List<Point> LegalMoves { get; }
        public Point Index { get; set; }

        public List<Point> FindLegalMoves(BoardState board)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));
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
            int posY = Index.Y;
            for (int i = Index.X + 1; i < (int)BoardRule.COL; ++i)
            {
                if (board[posY,i] != 0)
                {
                    while (i < (int)BoardRule.COL - 1)
                    {
                        ++i;
                        if (board[posY,i] * Value > 0) break;
                        if (board[posY,i] * Value < 0)
                        {
                            LegalMoves.Add(new Point(i, posY));
                            break;
                        }
                    }
                    break;
                }
                else
                    LegalMoves.Add(new Point(i, posY));
            }

            if (Index.X - 1 < 0) return;
            for (int i = Index.X - 1; i >= 0; --i)
            {
                if (board[posY,i] != 0)
                {
                    while (i > 0)
                    {
                        --i;
                        if (board[posY,i] * Value > 0) break;
                        if (board[posY,i] * Value < 0)
                        {
                            LegalMoves.Add(new Point(i, posY));
                            break;
                        }
                    }
                    break;
                }
                else
                    LegalMoves.Add(new Point(i, posY));
            }
        }

        private void FindVerticalMoves(BoardState board)
        {
            int posX = Index.X;
            for (int i = Index.Y + 1; i < (int)BoardRule.ROW; ++i)
            {
                if (board[i,posX] != 0)
                {
                    while (i < (int)BoardRule.ROW - 1)
                    {
                        ++i;
                        if (board[i,posX] * Value > 0) break;
                        if (board[i,posX] * Value < 0)
                        {
                            LegalMoves.Add(new Point(posX, i));
                            break;
                        }
                    }
                    break;
                }
                else
                    LegalMoves.Add(new Point(posX, i));
            }

            if (Index.Y - 1 < 0) return;
            for (int i = Index.Y - 1; i >= 0; --i)
            {
                if (board[i,posX] != 0)
                {
                    while (i > 0)
                    {
                        --i;
                        if (board[i,posX] * Value > 0) break;
                        if (board[i,posX] * Value < 0)
                        {
                            LegalMoves.Add(new Point(posX, i));
                            break;
                        }
                    }
                    break;
                }
                else
                    LegalMoves.Add(new Point(posX, i));
            }
        }
    }
}

