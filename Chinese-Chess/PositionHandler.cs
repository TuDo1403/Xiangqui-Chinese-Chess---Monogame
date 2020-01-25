using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public static class PositionHandler
    {
        private const int X_OFFSET_FROM_TOP_LEFT_WIN = 12;
        private const int X_OFFSET_PIECE = 97;
        private const int Y_OFFSET_PIECE = 102;



        public static Vector2 MatrixPosToSpritePos(Vector2 matrixPos)
        {
            var spritePos = matrixPos;
            spritePos.X = (spritePos.X* X_OFFSET_PIECE) + X_OFFSET_FROM_TOP_LEFT_WIN;
            spritePos.Y *= Y_OFFSET_PIECE;
            return spritePos;
        }


        public static Vector2 SpritePosToMatrixPos(Vector2 spritePos)
        {
            return Vector2.Zero;
        }
    }
}
