using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.GameObjects
{
    public class Message
    {
        private readonly SpriteFont _spriteFont;

        private Vector2 _position;

        public int CurrentFrame { get; set; } = 0;
        public int TotalFrames { get; set; } = 50;




        public Message(SpriteFont spriteFont, Vector2 position)
        {
            _spriteFont = spriteFont;
            _position = position;
        }


        public void Update()
        {
            if (CurrentFrame < TotalFrames)
            {
                CurrentFrame++;
            }                      
        }


        public void ResetTimer()
        {
            CurrentFrame = 0;
        }


        public void DrawString(SpriteBatch spriteBatch, string message, Color color)
        {
            if (spriteBatch == null)
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }
            if (CurrentFrame < TotalFrames)
            {
                spriteBatch.DrawString(_spriteFont, message, _position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1);
            }
        }
    }
}
