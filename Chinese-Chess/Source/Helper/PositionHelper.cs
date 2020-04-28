using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChineseChess.Source.Helper
{
    public static class PositionHelper
    {
        private const int X_OFFS_TOPLEFT_WIN = 12;
        private const int X_GAP = 97;
        private const int Y_GAP = 102;
        private const int CENTER_TOPLEFT_GAP = 84;


        /// <summary>
        /// Vector2 Extension Method for converting position in the matrix to position in the game window
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static Vector2 ToPosition(this Point idx)
        {
            var position = new Vector2(idx.X, idx.Y);
            position.X = position.X * X_GAP + X_OFFS_TOPLEFT_WIN;
            position.Y *= Y_GAP;
            return position;
        }

        /// <summary>
        /// Vector2 Extension Method for converting position in the game window to position in the matrix
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Point ToIndex(this Vector2 position)
        {
            var idx = new Vector2(position.X, position.Y);
            idx.X = (float)Math.Round((idx.X - X_OFFS_TOPLEFT_WIN) / X_GAP);
            idx.Y = (float)Math.Round(idx.Y / Y_GAP);
            return idx.ToPoint();
        }


        public static Vector2 GetLegalMovePosition(this Vector2 releasedPosition, List<Point> legalMoves)
        {
            if (legalMoves == null)
            {
                throw new ArgumentNullException(nameof(legalMoves));
            }

            if (legalMoves.Count == 0)
            {
                return Vector2.Zero;
            }

            var minDistance = legalMoves.Min(c => Vector2.Distance(releasedPosition, c.ToPosition()));
            if (minDistance <= CENTER_TOPLEFT_GAP)
            {
                var nearestMatrixPos = legalMoves.Where(c => Vector2.Distance(releasedPosition, c.ToPosition()) == minDistance)
                                                 .SingleOrDefault();
                return nearestMatrixPos.ToPosition();
            }
            return Vector2.Zero;
        }


        public static Vector2 ToTopLeftPosition(this Point ctPosition, int txtWidth, int txtHeight)
        {
            return new Vector2(ctPosition.X - txtWidth / 2, ctPosition.Y - txtHeight / 2);
        }


        public static Vector2 ToCenterPosition(this Vector2 tlPosition, int txtWidth, int txtHeight)
        {
            return new Vector2(tlPosition.X + txtWidth / 2, tlPosition.Y + txtHeight / 2);
        }
    }
}
