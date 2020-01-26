using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public class Horse : Piece
    {

        public Horse(Texture2D texture, Vector2 position) : base(texture, position)
        {
               
        }

        protected override void FindHorizontalMoves()
        {
            throw new NotImplementedException();
        }

        protected override void FindVerticalMoves()
        {
            throw new NotImplementedException();
        }
    }
}
