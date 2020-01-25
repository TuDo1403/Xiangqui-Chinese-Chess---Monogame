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
    public abstract class GameModel
    {
        protected Texture2D texture { get; set; }


        public Vector2 Position { get; set; } = new Vector2(0, 0);



        public GameModel(Texture2D texture)
        {
            this.texture = texture;
        }

        public GameModel()
        {

        }


        public virtual void Update(MouseState mouseState) { }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {
                spriteBatch.Draw(texture, Position, Color.White);
            }
            else
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }
        }
    }
}
