using ChineseChess.Source.GameObjects.Chess;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.Players
{
    public class Human : Player
    {
        public Human()
        {
            Tag = GameRule.PlayerTag.HUMAN;
        }

        public override void Update(MouseState mouseState)
        {
            foreach (var piece in Pieces)
                piece.Update(mouseState);
        }
    }
}
