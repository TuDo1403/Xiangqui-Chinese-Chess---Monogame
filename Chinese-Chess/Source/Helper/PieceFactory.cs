using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
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
                throw new ArgumentNullException(nameof(contentManager));

            var txt = contentManager.Load<Texture2D>(PieceLoader.TextureLoader[key]);
            var position = idx.ToPosition();
            var val = key;
            return new Piece(txt, position, val, board);
            //key = Math.Abs(key);
            //if (key == (int)Pieces.R_Chariot)
            //    return new Chariot(txt, position, val, board);
            //if (key == (int)Pieces.R_Horse)
            //    return new Horse(txt, position, val, board);
            //if (key == (int)Pieces.R_Advisor)
            //    return new Advisor(txt, position, val, board);
            //if (key == (int)Pieces.R_Cannon)
            //    return new Cannon(txt, position, val, board);
            //if (key == (int)Pieces.R_General)
            //    return new General(txt, position, val, board);
            //if (key == (int)Pieces.R_Soldier)
            //    return new Soldier(txt, position, val, board);
            //if (key == (int)Pieces.R_Elephant)
            //    return new Elephant(txt, position, val, board);

            throw new ArgumentException($"Create piece error: key values {key}");
        }
    }
}
