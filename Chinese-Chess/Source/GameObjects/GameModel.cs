using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.GameObjects
{
    public abstract class GameModel
    {
        protected Texture2D Texture { get; set; }


        public Vector2 Position { get; set; } = new Vector2(0, 0);



        public GameModel(Texture2D texture)
        {
            Texture = texture ?? throw new ArgumentNullException(nameof(texture));
        }

        public GameModel()
        {

        }


        public virtual void Update(MouseState mouseState) { }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {
                spriteBatch.Draw(Texture, Position, Texture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }
        }
    }
}
