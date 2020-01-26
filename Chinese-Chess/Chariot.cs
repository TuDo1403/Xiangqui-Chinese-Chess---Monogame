using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public class Chariot : Piece
    {
        public Chariot(Texture2D texture, Vector2 position) : base(texture, position)
        {
            FindNextMoves();
        }

        

        protected override void FindNextMoves()
        {
            base.FindNextMoves();
            FindHorizontalMoves();
            FindVerticalMoves();
        }

        protected override void FindHorizontalMoves()
        {
            int posX = (int)matrixPos.X;
            for (int i = (int)matrixPos.Y+1; i < Rules.COLUMNS; ++i)
            {
                if (!StillHasValidMoves(posX, i))
                {
                    break;
                }
            }

            if ((int)matrixPos.Y-1 < 0) return;
            for (int i = (int)matrixPos.Y - 1; i >= 0; --i)
            {
                if (!StillHasValidMoves(posX, i))
                {
                    break;
                }
            }
        }

        protected override void FindVerticalMoves()
        {
            int posY = (int)matrixPos.Y;
            for (int i = (int)matrixPos.X + 1; i < Rules.ROWS; ++i)
            {
                if (!StillHasValidMoves(i, posY))
                {
                    break;
                }
            }

            if ((int)matrixPos.X - 1 < 0) return;
            for (int i = (int)matrixPos.X-1; i >= 0; --i)
            {
                if (!StillHasValidMoves(i, posY))
                {
                    break;
                }
            }
        }
    }
}
