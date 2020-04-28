using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ChineseChess.Source.Helper
{
    public static class PieceFactory
    {
        public static Piece CreatePiece(int key, Point idx, ChessBoard board, ContentManager contentManager)
        {
            if (contentManager == null)
            {
                throw new ArgumentNullException(nameof(contentManager));
            }

            var txt = contentManager.Load<Texture2D>(PieceLoader.TextureLoader[key]);
            var position = idx.ToPosition();
            var val = key;
            return new Piece(txt, position, val, board);
        }
    }
}
