using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI.MoveLogic
{
    public class ChariotMove : IMovable
    {

        public int Value { get; set; }
        public List<Point> LegalMoves { get; }
        public Point Index { get; set; }

        public List<Point> FindLegalMoves(int[][] board)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));
            Value = board[Index.Y][Index.X];
            FindHorizontalMoves(board);
            FindVerticalMoves(board);
            return LegalMoves;
        }

        public ChariotMove(Point idx)
        {
            Index = idx;
            LegalMoves = new List<Point>();
        }

        private void FindHorizontalMoves(int[][] board)
        {
            int posY = Index.Y;
            for (int i = Index.X + 1; i < (int)BoardRule.COL; ++i)
                if (!StillHasLegalMoves(posY, i, board)) break;

            if (Index.X - 1 < 0) return;
            for (int i = Index.X - 1; i >= 0; --i)
                if (!StillHasLegalMoves(posY, i, board)) break;
        }

        private void FindVerticalMoves(int[][] board)
        {
            int posX = Index.X;
            for (int i = Index.Y + 1; i < (int)BoardRule.ROW; ++i)
                if (!StillHasLegalMoves(i, posX, board)) break;

            if (Index.Y - 1 < 0) return;
            for (int i = Index.Y - 1; i >= 0; --i)
                if (!StillHasLegalMoves(i, posX, board)) break;
        }

        private bool StillHasLegalMoves(int row, int column, int[][] board)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            if (board[row][column] * Value > 0) return false;
            else
            {
                LegalMoves.Add(new Point(column, row));
                if (board[row][column] * Value < 0) return false;
            }
            return true;
        }
    }
}
