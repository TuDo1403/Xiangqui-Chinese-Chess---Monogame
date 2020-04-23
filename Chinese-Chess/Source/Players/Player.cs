using ChineseChess.Source.GameObjects.Chess;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.Players
{
    public abstract class Player
    {
        public List<Piece> Pieces { get; set; }


        public Player(List<Piece> pieces)
        {
            Pieces = pieces;
        }

        public abstract void Update(MouseState mouseState);

        protected abstract void MakeMove(int[][] board);
    }
}
