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
        public static Piece CreatePiece(float key, Point matrixPos, ContentManager contentManager)
        {
            if (contentManager == null)
            {
                throw new ArgumentNullException(nameof(contentManager));
            }
            var texture = contentManager.Load<Texture2D>(Rules.PieceDictionary[key]);
            var spritePos = matrixPos.ToSpritePos();

            key = Math.Abs(key);
            if (key == Rules.CHARIOT)
            {
                return new Chariot(texture, spritePos);
            }
            if (key == Rules.HORSE)
            {
                return new Horse(texture, spritePos);
            }
            if (key == Rules.ADVISOR)
            {
                return new Advisor(texture, spritePos);
            }
            if (key == Rules.CANNON)
            {
                return new Cannon(texture, spritePos);
            }
            if (key == Rules.GENERAL)
            {
                return new General(texture, spritePos);
            }
            if (key == Rules.SOLDIER)
            {
                return new Soldier(texture, spritePos);
            }
            if (key == Rules.ELEPHANT)
            {
                return new Elephant(texture, spritePos);
            }
            throw new ArgumentException($"Create piece error: key values {key}");
        }
    }
}
