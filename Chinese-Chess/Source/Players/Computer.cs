using ChineseChess.Source.AI;
using ChineseChess.Source.GameObjects.Chess;
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
        public Computer(int player, int depth)
        {
            Tag = GameRule.PlayerTag.COM;
            AIAgent = new MiniMax(player);
            Depth = depth;
        }

        public override void Update(int[][] board, int d=0)
        {
            var depth = d == 0 ? Depth : d;
            Search(board, depth);
        }

        private void Search(int[][] board, int depth)
        {
            var move = AIAgent.Minimax(board, depth);
            var focusingPiece = Pieces.Where(p => p.Index == move.Item2)
                                      .SingleOrDefault();
            if (focusingPiece != null)
            {
                focusingPiece.OnFocusing();
                focusingPiece.SetMove(move.Item3);
            }
            Console.WriteLine(move.Item1);
        }
    }
}
