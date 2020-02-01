using ChineseChess.Source.GameObjects.Chess;
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
            var texture = contentManager.Load<Texture2D>(Rules.PieceDictionary[key]);
            var spritePos = matrixPos.ToSpritePos();

            var type = key;
            key = Math.Abs(key);
            if (key == Rules.CHARIOT)
            {
                return new Chariot(texture, spritePos, type);
            }
            if (key == Rules.HORSE)
            {
                return new Horse(texture, spritePos, type);
            }
            if (key == Rules.ADVISOR)
            {
                return new Advisor(texture, spritePos, type);
            }
            if (key == Rules.CANNON)
            {
                return new Cannon(texture, spritePos, type);
            }
            if (key == Rules.GENERAL)
            {
                return new General(texture, spritePos, type);
            }
            if (key == Rules.SOLDIER)
            {
                return new Soldier(texture, spritePos, type);
            }
            if (key == Rules.ELEPHANT)
            {
                return new Elephant(texture, spritePos, type);
            }
            throw new ArgumentException($"Create piece error: key values {key}");
        }
    }
}
