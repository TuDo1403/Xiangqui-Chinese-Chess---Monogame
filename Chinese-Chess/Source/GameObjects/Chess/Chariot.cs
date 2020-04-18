using ChineseChess.Source.GameRule;
using ChineseChess.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChineseChess.Source.GameObjects.Chess
{
    public sealed class Chariot : Piece
    {
        public Chariot(Texture2D texture, Vector2 position, int type, ChessBoard board) : base(texture, position, type, board) { }



        protected override void FindLegalMoves(int[][] board)
        {
            base.FindLegalMoves(board);
            FindHorizontalMoves(board);
            FindVerticalMoves(board);
        }

        protected override void FindHorizontalMoves(int[][] board)
        {
            int posY = Index.Y;
            for (int i = Index.X + 1; i < (int)BoardRule.COL; ++i)
                if (!StillHasLegalMoves(posY, i, board)) break;

            if (Index.X - 1 < 0) return;
            for (int i = Index.X - 1; i >= 0; --i)
                if (!StillHasLegalMoves(posY, i, board)) break;
        }

        protected override void FindVerticalMoves(int[][] board)
        {
            int posX = Index.X;
            for (int i = Index.Y + 1; i < (int)BoardRule.ROW; ++i)
                if (!StillHasLegalMoves(i, posX, board)) break;

            if (Index.Y - 1 < 0) return;
            for (int i = Index.Y - 1; i >= 0; --i)
                if (!StillHasLegalMoves(i, posX, board)) break;
        }
    }
}
