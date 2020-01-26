using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public static class PositionHelper
    {
        private const int X_OFFSET_FROM_TOP_LEFT_WIN = 12;
        private const int X_OFFSET_PIECE = 97;
        private const int Y_OFFSET_PIECE = 102;


        /// <summary>
        /// Vector2 Extension Method for converting position in the matrix to position in the game window
        /// </summary>
        /// <param name="matrixPos"></param>
        /// <returns></returns>
        public static Vector2 ToSpritePos(this Vector2 matrixPos)
        {
            var spritePos = new Vector2(matrixPos.Y, matrixPos.X);
            spritePos.X = (spritePos.X* X_OFFSET_PIECE) + X_OFFSET_FROM_TOP_LEFT_WIN;
            spritePos.Y *= Y_OFFSET_PIECE;
            return spritePos;
        }

        /// <summary>
        /// Vector2 Extension Method for converting position in the game window to position in the matrix
        /// </summary>
        /// <param name="spritePos"></param>
        /// <returns></returns>
        public static Vector2 ToMatrixPos(this Vector2 spritePos)
        {
            var matrixPos = new Vector2(spritePos.Y, spritePos.X);
            matrixPos.X = (float)Math.Round((matrixPos.X - X_OFFSET_FROM_TOP_LEFT_WIN) / X_OFFSET_PIECE);
            matrixPos.Y = (float)Math.Round(matrixPos.Y / Y_OFFSET_PIECE);
            return matrixPos;
        }


        public static Vector2 GetValidMovePosition(this Vector2 releasedPosition, List<Vector2> validMoves)
        {
            if (validMoves == null)
            {
                throw new ArgumentNullException(nameof(validMoves));
            }
            if (validMoves.Count == 0)
            {
                return Vector2.Zero;
            }

            var minDistance = validMoves.Min(c => Vector2.Distance(releasedPosition, c.ToSpritePos()));
            if (minDistance <= 70)
            {
                var nearestMatrixPos = validMoves.Where(c => Vector2.Distance(releasedPosition, c.ToSpritePos()) == minDistance)
                                                 .SingleOrDefault();
                return nearestMatrixPos.ToSpritePos();
            }
            return Vector2.Zero;
        }
    }
}
