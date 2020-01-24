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
    public class Horse
    {
        private Texture2D _texture;

        private int _value;


        public Vector2 Position { get; set; }

        public Rectangle Bounds { get; set; }



        public Horse(Texture2D texture, Vector2 position, int value)
        {
            _texture = texture;
            _value = value;
            Position = position;
            SetBounds();
        }


        public void Update(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (Bounds.Contains(mouseState.Position))
                {
                    SetPosition(mouseState.X - (_texture.Width / 2), mouseState.Y - (_texture.Height / 2));
                    SetBounds();
                }
            }
        }

        private void SetBounds()
        {
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
        }

        private void SetPosition(int x, int y)
        {
            var position = new Vector2(x, y);
            Position = position;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
        }
    }
}
