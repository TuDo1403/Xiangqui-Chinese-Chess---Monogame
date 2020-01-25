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
    public abstract class Piece : GameModel
    {
        private List<Vector2> _validMoves;

        public const int CHARIOT = 1;
        public const int HORSE = 2;
        public const int ELEPHANT = 3;
        public const int ADVISOR = 4;
        public const int GENERAL = 5;
        public const int CANNON = 6;
        public const int SOLDIER = 7;


        public Rectangle Bounds { get; set; }



        public Piece(Texture2D texture, Vector2 position) : base(texture)
        {
            Position = position;
            SetBounds();
        }


        public override void Update(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (Bounds.Contains(mouseState.Position))
                {
                    SetPosition(mouseState.X - (texture.Width / 2), mouseState.Y - (texture.Height / 2));
                    SetBounds();
                }
            }
        }

        private void SetBounds()
        {
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
        }

        private void SetPosition(int x, int y)
        {
            var position = new Vector2(x, y);
            Position = position;
        }
    }
}
