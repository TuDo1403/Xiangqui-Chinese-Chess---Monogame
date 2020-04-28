using ChineseChess.Source.AI;
using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using System.Linq;

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

        public override void Update(BoardState board)
        {
            FindBestMove(board);
        }

        private void FindBestMove(BoardState board)
        {
            var move = AIAgent.MinimaxRoot(board, Depth);
            var focusingPiece = Pieces.Where(p => p.Index == move.Item1)
                                      .SingleOrDefault();
            focusingPiece.OnFocusing();
            focusingPiece.SetMove(move.Item2);
        }
    }
}
