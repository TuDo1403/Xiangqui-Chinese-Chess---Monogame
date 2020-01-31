using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public class General : Piece
    {
        
        public General(Texture2D texture, Vector2 position, int type) : base(texture, position, type)
        {
        }

        

        protected override void FindNextMoves()
        {
            base.FindNextMoves();
            FindVerticalMoves();
            FindHorizontalMoves();
        }

        protected override void FindHorizontalMoves()
        {
            if (MatrixPos.X + 1 <= 5)
            {
                StillHasValidMoves(MatrixPos.Y, MatrixPos.X + 1);
            }
            if (MatrixPos.X - 1 >= 3)
            {
                StillHasValidMoves(MatrixPos.Y, MatrixPos.X - 1);
            }
        }

        protected override void FindVerticalMoves()
        {
            if (Type > 0)
            {
                if (MatrixPos.Y + 1 <= 9)
                {
                    StillHasValidMoves(MatrixPos.Y + 1, MatrixPos.X);
                }
                if (MatrixPos.Y - 1 >= 7)
                {
                    StillHasValidMoves(MatrixPos.Y - 1, MatrixPos.X);
                }
            }
            else
            {
                if (MatrixPos.Y + 1 <= 2)
                {
                    StillHasValidMoves(MatrixPos.Y + 1, MatrixPos.X);
                }
                if (MatrixPos.Y - 1 >= 0)
                {
                    StillHasValidMoves(MatrixPos.Y - 1, MatrixPos.X);
                }
            }
        }
    }
}
