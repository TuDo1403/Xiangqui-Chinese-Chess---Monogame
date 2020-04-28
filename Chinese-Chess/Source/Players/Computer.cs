using ChineseChess.Source.AI;
using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.Players
{
    public class Computer : Player
    {
        public MiniMax AIAgent { get; set; }
        public int Depth { get; set; }
        public Computer(Team player, int depth)
        {
            Tag = PlayerTag.COM;
            AIAgent = new MiniMax(player);
            Depth = depth;
        }

        public override void Update(BoardState board, int d=0)
        {
            var depth = d == 0 ? Depth : d;
            Search(board, depth);
        }

        private void Search(BoardState board, int depth)
        {
            var move = AIAgent.MinimaxRoot(board, depth);
            var focusingPiece = Pieces.Where(p => p.Index == move.Item1)
                                      .SingleOrDefault();
            focusingPiece.OnFocusing();
            focusingPiece.SetMove(move.Item2);
        }
    }
}
