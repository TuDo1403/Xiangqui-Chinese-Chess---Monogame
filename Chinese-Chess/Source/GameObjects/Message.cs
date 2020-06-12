using ChineseChess.Source.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ChineseChess.Source.GameObjects
{
    public class Message
    {
        private readonly SpriteFont _spriteFont;

        private Vector2 _position;

        private readonly string _text;


        public int CurrentFrame { get; set; } = 0;
        public int TotalFrames { get; set; } = 3000;



        public Message(SpriteFont spriteFont, string text, Point centerPosition)
        {
            _spriteFont = spriteFont ?? throw new ArgumentNullException(nameof(spriteFont));
            _text = text;

            var width = _spriteFont.MeasureString(_text).X;
            var height = _spriteFont.MeasureString(_text).Y;
            _position = centerPosition.ToTopLeftPosition((int)width, (int)height);
        }


        public void Update()
        {
            if (CurrentFrame < TotalFrames) CurrentFrame++;
        }


        public void ResetTimer() => CurrentFrame = 0;


        public void DrawString(SpriteBatch spriteBatch, Color color)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException(nameof(spriteBatch));
            if (CurrentFrame < TotalFrames)
                spriteBatch.DrawString(_spriteFont, _text, _position, color, 0f, 
                                       Vector2.Zero, 1f, SpriteEffects.None, 1);
        }
    }
}
