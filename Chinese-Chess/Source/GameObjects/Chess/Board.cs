using Microsoft.Xna.Framework.Graphics;
using System;

namespace ChineseChess.Source.GameObjects.Chess
{
    public sealed class Board : GameModel
    {
        private static Board _instance;


        public int Width { get; set; }
        public int Height { get; set; }



        public static Board GetInstance(Texture2D texture)
        {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            if (_instance == null)
                _instance = new Board(texture);

            return _instance;
        }

        private Board(Texture2D texture) : base(texture)
        {
            Width = texture.Width;
            Height = texture.Height;
        }

    }
}
