using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Chess
{
    public class Board : GameModel
    {
        public int Width { get; set; }
        public int Height { get; set; }



        public Board(Texture2D texture) : base(texture)
        {
            Width = texture.Width;
            Height = texture.Height;
        }

    }
}
