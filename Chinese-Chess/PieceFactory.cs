using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public static class PieceFactory
    {
        public static Piece CreatePiece(float key, Vector2 matrixPos, ContentManager contentManager)
        {
            if (contentManager == null)
            {
                throw new ArgumentNullException(nameof(contentManager));
            }
            var texture = contentManager.Load<Texture2D>(Pieces.PieceDictionary[key]);
            var spritePos = PositionHandler.MatrixPosToSpritePos(matrixPos);

            key = Math.Abs(key);
            if (key == Piece.CHARIOT)
            {
                return new Chariot(texture, spritePos);
            }
            if (key == Piece.HORSE)
            {
                return new Horse(texture, spritePos);
            }
            if (key == Piece.ADVISOR)
            {
                return new Advisor(texture, spritePos);
            }
            if (key == Piece.CANNON)
            {
                return new Cannon(texture, spritePos);
            }
            if (key == Piece.GENERAL)
            {
                return new General(texture, spritePos);
            }
            if (key == Piece.SOLDIER)
            {
                return new General(texture, spritePos);
            }
            if (key == Piece.ELEPHANT)
            {
                return new Elephant(texture, spritePos);
            }
            throw new ArgumentException($"Create piece error: key values {key}");
        }
    }
}
