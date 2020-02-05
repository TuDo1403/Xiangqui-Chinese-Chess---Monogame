using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ChineseChess.Source.Helper
{
    public static class PieceFactory
    {
        public static Piece CreatePiece(int key, Point matrixPos, ContentManager contentManager)
        {
            if (contentManager == null)
            {
                throw new ArgumentNullException(nameof(contentManager));
            }
            var texture = contentManager.Load<Texture2D>(PieceLoader.TextureLoader[key]);
            var spritePos = matrixPos.ToSpritePos();

            var type = key;
            key = Math.Abs(key);
            if (key == (int)Pieces.R_Chariot)
            {
                return new Chariot(texture, spritePos, type);
            }
            if (key == (int)Pieces.R_Horse)
            {
                return new Horse(texture, spritePos, type);
            }
            if (key == (int)Pieces.R_Advisor)
            {
                return new Advisor(texture, spritePos, type);
            }
            if (key == (int)Pieces.R_Cannon)
            {
                return new Cannon(texture, spritePos, type);
            }
            if (key == (int)Pieces.R_General)
            {
                return new General(texture, spritePos, type);
            }
            if (key == (int)Pieces.R_Soldier)
            {
                return new Soldier(texture, spritePos, type);
            }
            if (key == (int)Pieces.R_Elephant)
            {
                return new Elephant(texture, spritePos, type);
            }
            throw new ArgumentException($"Create piece error: key values {key}");
        }
    }
}
