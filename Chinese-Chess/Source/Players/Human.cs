using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChineseChess.Source.Players
{
    public class Human : Player
    {
        public Human()
        {
            Tag = GameRule.PlayerTag.HUMAN;
        }

        public override void Update(MouseState mouseState, GameTime gameTime)
        {
            foreach (var piece in Pieces)
            {
                piece.Update(mouseState, gameTime);
            }
        }
    }
}
