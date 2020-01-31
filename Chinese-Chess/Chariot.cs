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
        public Chariot(Texture2D texture, Vector2 position, int type) : base(texture, position, type)
        {
        }

        

        protected override void FindNextMoves()
        {
            base.FindNextMoves();
            FindHorizontalMoves();
            FindVerticalMoves();
            //PrintValidMove();
        }

        protected override void FindHorizontalMoves()
        {
            int posY = MatrixPos.Y;
            for (int i = MatrixPos.X + 1; i < Rules.COLUMNS; ++i)
            {
                if (!StillHasValidMoves(posY, i))
                {
                    break;
                }
            }

            if (MatrixPos.X - 1 < 0) return;
            for (int i = MatrixPos.X - 1; i >= 0; --i)
            {
                if (!StillHasValidMoves(posY, i))
                {
                    break;
                }
            }
        }

        protected override void FindVerticalMoves()
        {
            int posX = MatrixPos.X;
            for (int i = MatrixPos.Y + 1; i < Rules.ROWS; ++i)
            {
                if (!StillHasValidMoves(i, posX))
                {
                    break;
                }
            }

            if (MatrixPos.Y - 1 < 0) return;
            for (int i = MatrixPos.Y - 1; i >= 0; --i)
            {
                if (!StillHasValidMoves(i, posX))
                {
                    break;
                }
            }
        }
    }
}
