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
        public Computer(int player)
        {
            Tag = GameRule.PlayerTag.COM;
            AIAgent = new MiniMax(player);
        }

        public override async void Update(int[][] board)
        {
            Search(board);
        }

        private void Search(int[][] board)
        {
            var move = AIAgent.Minimax(board, 8);
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
