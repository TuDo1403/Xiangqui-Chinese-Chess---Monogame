using ChineseChess.Source.GameObjects.Chess;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ChineseChess.Source.GameRule.MoveLogic
{
    public class GeneralMove : IMovable
    {
        public int Value { get; set; }
        public List<Point> LegalMoves { get; }
        public Point Index { get; set; }

        public List<Point> FindLegalMoves(BoardState board)
        {
            Value = board[Index.Y, Index.X];
            FindVerticalMoves(board);
            FindHorizontalMoves(board);
            FindFlyingMove(board);
            return LegalMoves;
        }

        public GeneralMove(Point idx)
        {
            Index = idx;
            LegalMoves = new List<Point>();
        }

        private void FindFlyingMove(BoardState board)
        {
            var colIdx = Index.X;
            if (Value > 0)
                for (int i = Index.Y - 1; i >= 0; --i)
                {
                    if (board[i, colIdx] != (int)Pieces.B_General &&
                        board[i, colIdx] != 0)
                        return;
                    if (board[i, colIdx] == (int)Pieces.B_General)
                    {
                        LegalMoves.Add(new Point(colIdx, i));
                        break;
                    }

                }
            else
                for (int i = Index.Y + 1; i < (int)Rule.ROW; ++i)
                {
                    if (board[i, colIdx] != (int)Pieces.R_General &&
                        board[i, colIdx] != 0)
                        return;

                    if (board[i, colIdx] == (int)Pieces.R_General)
                        LegalMoves.Add(new Point(colIdx, i));
                }
        }

        private void FindHorizontalMoves(BoardState board)
        {
            if (Index.X + 1 <= (int)Rule.R_CASTLE)
                if (board[Index.Y, Index.X + 1] * Value <= 0)
                    LegalMoves.Add(new Point(Index.X + 1, Index.Y));
            if (Index.X - 1 >= (int)Rule.L_CASTLE)
                if (board[Index.Y, Index.X - 1] * Value <= 0)
                    LegalMoves.Add(new Point(Index.X - 1, Index.Y));
        }

        private void FindVerticalMoves(BoardState board)
        {
            if (Value > 0)
            {
                if (Index.Y + 1 < (int)Rule.ROW)
                    if (board[Index.Y + 1, Index.X] * Value <= 0)
                        LegalMoves.Add(new Point(Index.X, Index.Y + 1));
                if (Index.Y - 1 >= (int)Rule.FR_CASTLE)
                    if (board[Index.Y - 1, Index.X] * Value <= 0)
                        LegalMoves.Add(new Point(Index.X, Index.Y - 1));
            }
            else
            {
                if (Index.Y + 1 <= (int)Rule.FB_CASTLE)
                    if (board[Index.Y + 1, Index.X] * Value <= 0)
                        LegalMoves.Add(new Point(Index.X, Index.Y + 1));
                if (Index.Y - 1 >= 0)
                    if (board[Index.Y - 1, Index.X] * Value <= 0)
                        LegalMoves.Add(new Point(Index.X, Index.Y - 1));
            }
        }
    }
}

