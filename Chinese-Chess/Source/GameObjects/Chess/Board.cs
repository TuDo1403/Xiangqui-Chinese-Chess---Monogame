using Microsoft.Xna.Framework.Graphics;
using System;

namespace ChineseChess.Source.GameObjects.Chess
{
    public sealed class Board : GameModel
    {
        private static Board _instance;


        public int Width { get; set; }
        public int Height { get; set; }



        public Board(Texture2D texture) : base(texture)
        {
            Width = texture.Width;
            Height = texture.Height;
        }

    }
}
