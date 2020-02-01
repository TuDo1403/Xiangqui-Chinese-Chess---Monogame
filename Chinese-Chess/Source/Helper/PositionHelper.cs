using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.Helper
{
    public static class PositionHelper
    {
        public const int X_OFFSET_FROM_TOP_LEFT_WIN = 12;


        private const int X_OFFSET_PIECE = 97;
        private const int Y_OFFSET_PIECE = 102;
        private const int DISTANCE_BETWEEN_CENTER_AND_CORNERS = 84;


        /// <summary>
        /// Vector2 Extension Method for converting position in the matrix to position in the game window
        /// </summary>
        /// <param name="matrixPos"></param>
        /// <returns></returns>
        public static Vector2 ToSpritePos(this Point matrixPos)
        {
            var spritePos = new Vector2(matrixPos.X, matrixPos.Y);
            spritePos.X = spritePos.X * X_OFFSET_PIECE + X_OFFSET_FROM_TOP_LEFT_WIN;
            spritePos.Y *= Y_OFFSET_PIECE;
            return spritePos;
        }

        /// <summary>
        /// Vector2 Extension Method for converting position in the game window to position in the matrix
        /// </summary>
        /// <param name="spritePos"></param>
        /// <returns></returns>
        public static Point ToMatrixPos(this Vector2 spritePos)
        {
            var matrixPos = new Vector2(spritePos.X, spritePos.Y);
            matrixPos.X = (float)Math.Round((matrixPos.X - X_OFFSET_FROM_TOP_LEFT_WIN) / X_OFFSET_PIECE);
            matrixPos.Y = (float)Math.Round(matrixPos.Y / Y_OFFSET_PIECE);
            return matrixPos.ToPoint();
        }


        public static Vector2 GetValidMovePosition(this Vector2 releasedPosition, List<Point> validMoves)
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
            if (minDistance <= DISTANCE_BETWEEN_CENTER_AND_CORNERS)
            {
                var nearestMatrixPos = validMoves.Where(c => Vector2.Distance(releasedPosition, c.ToSpritePos()) == minDistance)
                                                 .SingleOrDefault();
                return nearestMatrixPos.ToSpritePos();
            }
            return Vector2.Zero;
        }


        public static Vector2 ToSpriteTopLeftPosition(this Point centerPosition, int textureWidth, int textureHeight)
        {
            return new Vector2(centerPosition.X - textureWidth / 2, centerPosition.Y - textureHeight / 2);
        }


        public static Vector2 ToSpriteCenterPosition(this Vector2 topLeftPosition, int textureWidth, int textureHeight)
        {
            return new Vector2(topLeftPosition.X + textureWidth / 2, topLeftPosition.Y + textureHeight / 2);
        }
    }
}
